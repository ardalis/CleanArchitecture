using System.Reflection;
using Ardalis.SharedKernel;
using Autofac;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.ProjectAggregate;
using MediatR.Pipeline;
using MediatR;
using Module = Autofac.Module;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Data;

namespace Clean.Architecture.UseCases;

public class AutofacUseCasesModule : Module
{
  private readonly bool _isDevelopment = false;
  private readonly List<Assembly> _assemblies = new List<Assembly>();

  public AutofacUseCasesModule(bool isDevelopment, Assembly? callingAssembly = null)
  {
    _isDevelopment = isDevelopment;

    // TODO: Replace "Project" with any type from your Core project
    var coreAssembly = Assembly.GetAssembly(typeof(Project));
    if (coreAssembly != null)
    {
      _assemblies.Add(coreAssembly);
    }

    var infrastructureAssembly = Assembly.GetAssembly(typeof(AppDbContext));
    if (infrastructureAssembly != null)
    {
      _assemblies.Add(infrastructureAssembly);
    }

    var useCasesAssembly = Assembly.GetAssembly(typeof(AutofacUseCasesModule));
    if (useCasesAssembly != null)
    {
      // needed to wire up MediatR commands and queries in the Use Cases assembly
      _assemblies.Add(useCasesAssembly);
    }

    if (callingAssembly != null)
    {
      _assemblies.Add(callingAssembly);
    }
  }

  protected override void Load(ContainerBuilder builder)
  {
    if (_isDevelopment)
    {
      RegisterDevelopmentOnlyDependencies(builder);
    }
    else
    {
      RegisterProductionOnlyDependencies(builder);
    }

    RegisterCommonDependencies(builder);
  }

  private void RegisterCommonDependencies(ContainerBuilder builder)
  {
    builder.RegisterGeneric(typeof(EfRepository<>))
      .As(typeof(IRepository<>))
      .As(typeof(IReadRepository<>))
      .InstancePerLifetimeScope();

    builder
      .RegisterType<Mediator>()
      .As<IMediator>()
      .InstancePerLifetimeScope();

    builder
      .RegisterType<MediatRDomainEventDispatcher>()
      .As<IDomainEventDispatcher>()
      .InstancePerLifetimeScope();

    //builder.Register<ServiceFactory>(context =>
    //{
    //  var c = context.Resolve<IComponentContext>();

    //  return t => c.Resolve(t);
    //});

    var mediatrOpenTypes = new[]
    {
      typeof(IRequestHandler<,>),
      typeof(IRequestExceptionHandler<,,>),
      typeof(IRequestExceptionAction<,>),
      typeof(INotificationHandler<>),
    };

    foreach (var mediatrOpenType in mediatrOpenTypes)
    {
      builder
        .RegisterAssemblyTypes(_assemblies.ToArray())
        .AsClosedTypesOf(mediatrOpenType)
        .AsImplementedInterfaces();
    }
  }

  private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
  {
    // NOTE: Add any development only services here
    builder.RegisterType<FakeEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();
  }

  private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
  {
    // NOTE: Add any production only services here
    builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();
  }
}
