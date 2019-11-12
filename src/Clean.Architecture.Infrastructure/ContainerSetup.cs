using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Clean.Architecture.Core.SharedKernel;
using Clean.Architecture.Infrastructure.Data;

namespace Clean.Architecture.Infrastructure
{
	public static class ContainerSetup
	{
		public static IServiceProvider InitializeWeb(Assembly webAssembly, IServiceCollection services) =>
			new AutofacServiceProvider(BaseAutofacInitialization(setupAction =>
			{
				setupAction.Populate(services);
				setupAction.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces();
			}));

		public static IContainer BaseAutofacInitialization(Action<ContainerBuilder> setupAction = null)
		{
			var builder = new ContainerBuilder();

			var coreAssembly = Assembly.GetAssembly(typeof(BaseEntity));
			var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
			builder.RegisterAssemblyTypes(coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

			setupAction?.Invoke(builder);
			return builder.Build();
		}
	}
}
