using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Clean.Architecture.Core;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web;
using FastEndpoints;
using FastEndpoints.Swagger.Swashbuckle;
using FastEndpoints.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Clean.Architecture.Core.UserAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Clean.Architecture.Core.Interfaces;
using System.Security.Claims;
using Clean.Architecture.SharedKernel.Utilities;
using Clean.Architecture.Core.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");  //Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext(connectionString!);

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddFastEndpoints();
builder.Services.AddFastEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
  c.EnableAnnotations();
  c.OperationFilter<FastEndpointsOperationFilter>();
});
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));

var sitSettings = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.AddCustomIdentity(sitSettings!.IdentitySettings);
builder.Services.AddCustomJwtAuthentication(sitSettings!.JwtSettings);

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});


builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseRouting();
app.UseFastEndpoints();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.MapDefaultControllerRoute();
app.MapRazorPages();

// Seed Database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //                    context.Database.Migrate();
    var X = context.Database.EnsureCreated();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

app.Run();




public static class StartupSetup
{
  public static void AddCustomJwtAuthentication(this IServiceCollection services, JwtSettings settings)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
      .AddJwtBearer(options =>
      {
        var secretKey = Encoding.UTF8.GetBytes(settings.SecretKey);
        var encryptionKey = Encoding.UTF8.GetBytes(settings.EncryptKey);

        var validationParameters = new TokenValidationParameters
        {
          ClockSkew = TimeSpan.Zero, // default: 5 min
          RequireSignedTokens = true,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretKey),

          RequireExpirationTime = true,
          ValidateLifetime = true,

          ValidateAudience = true, //default : false
          ValidAudience = settings.Audience,

          ValidateIssuer = true, //default : false
          ValidIssuer = settings.Issuer,

          TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
        };

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = validationParameters;

        options.Events = new JwtBearerEvents
        {
          OnAuthenticationFailed = context =>
          {
            if (context.Exception != null)
            {
              throw context.Exception;
            }

            return Task.CompletedTask;
          },
          OnTokenValidated = async context =>
          {
            var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims?.Any() != true)
              context.Fail("This token has no claims.");

            //Find user and token from database and perform your custom validation
            var userId = claimsIdentity?.GetUserId<int>();
            if (userId == null)
              context.Fail("There is no UserId in claims.");

            var user = await userRepository.GetByIdAsync(userId.GetValueOrDefault(), context.HttpContext.RequestAborted);

            if (user == null)
              context.Fail("User not found.");

            if (user != null && !user.IsActive)
              context.Fail("User is not active.");

            await userRepository.UpdateLastLoginDateAsync(user!, context.HttpContext.RequestAborted);
          },
          OnChallenge = context =>
          {
            if (context.AuthenticateFailure != null)
              throw context.AuthenticateFailure;
            //throw new CleanArchAppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
            //throw new CleanArchAppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);
            throw new Exception("You are unauthorized to access this resource.");
          }
        };
      });
  }
  public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
  {
    services.AddIdentity<User, Role>(identityOptions =>
    {
      //Password Settings
      identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
      identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
      identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric; //#@!
      identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
      identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

      //UserName Settings
      identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;
    })
  .AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();
  }
}
