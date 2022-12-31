using Autofac;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services;
using Clean.Architecture.Core.Services.Auth.Jwt;

namespace Clean.Architecture.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();

    builder.RegisterType<DeleteContributorService>()
        .As<IDeleteContributorService>().InstancePerLifetimeScope();

    builder.RegisterType<JwtService>()
       .As<IJwtService>().InstancePerLifetimeScope();
  }
}
