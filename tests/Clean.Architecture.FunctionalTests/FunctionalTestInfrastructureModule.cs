using Autofac;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure;

namespace Clean.Architecture.FunctionalTests;

public class FunctionalTestInfrastructureModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<FakeEmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
  }
}
