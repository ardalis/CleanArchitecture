using Autofac;
using Clean.Architecture._1.Core.Interfaces;
using Clean.Architecture._1.Core.Services;

namespace Clean.Architecture._1.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
