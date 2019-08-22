using Autofac;
using Autofac.Extensions.DependencyInjection;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CleanArchitecture.Web
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            AddDbContextToServices(services);

            return BuildDependencyInjectionProvider(services);
        }

        private static void AddDbContextToServices(IServiceCollection services)
        {
            // Create lightweight container just so we can get IDatabaseRegistrar instance.
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(GetAssembliesToRegister(false)).AsImplementedInterfaces();
            builder.Build().Resolve<IDatabaseRegistrar>().Register(services);
        }

        private static IServiceProvider BuildDependencyInjectionProvider(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Populate the container using the service collection
            builder.Populate(services);
            builder.RegisterAssemblyTypes(GetAssembliesToRegister()).AsImplementedInterfaces();

            IContainer applicationContainer = builder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        private static Assembly[] GetAssembliesToRegister(bool includeWeb = true)
        {
            List<Assembly> assemblies = new List<Assembly>();
            if (includeWeb)
            {
                assemblies.Add(Assembly.GetExecutingAssembly());
            }
            assemblies.Add(Assembly.GetAssembly(typeof(BaseEntity)));
            assemblies.Add(GetInfrastructureAssembly());
            return assemblies.ToArray();
        }

        private static Assembly GetInfrastructureAssembly()
        {
            string location = AppDomain.CurrentDomain.BaseDirectory;
            const string infrastructureDll = "CleanArchitecture.Infrastructure.dll";
            return Assembly.LoadFile($"{location}{infrastructureDll}");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
