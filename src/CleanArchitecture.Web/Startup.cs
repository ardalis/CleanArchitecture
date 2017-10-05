using System;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;

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
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("database"));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .AddControllersAsServices();

            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup)); // Web
                    _.AssemblyContainingType(typeof(BaseEntity)); // Core
                    _.Assembly("CleanArchitecture.Infrastructure"); // Infrastructure
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
                });
                
                // TODO: Add Registry Classes to eliminate reference to Infrastructure

                // TODO: Move to Infrastucture Registry
                config.For(typeof(IRepository<>)).Add(typeof(EfRepository<>));

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            this.Configure(app, env, loggerFactory);
            SeedData.PopulateTestData(app.ApplicationServices.GetService<AppDbContext>());
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
