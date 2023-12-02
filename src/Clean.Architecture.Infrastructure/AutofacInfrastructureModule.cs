using System.Reflection;
using Ardalis.SharedKernel;
using Autofac;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Data.Queries;
using Clean.Architecture.Infrastructure.Email;
using Clean.Architecture.UseCases.Contributors.Create;
using Clean.Architecture.UseCases.Contributors.List;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Clean.Architecture.Infrastructure;

/// <summary>
/// An Autofac module responsible for wiring up services defined in Infrastructure.
/// Mainly responsible for setting up EF and MediatR, as well as other one-off services.
/// </summary>
public class AutofacInfrastructureModule : Module
{
  readonly bool _isDevelopment = false;
  readonly List<Assembly> _assemblies = [];

  public AutofacInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
  {
    _isDevelopment = isDevelopment;
    AddToAssembliesIfNotNull(callingAssembly);
  }

  void AddToAssembliesIfNotNull(Assembly? assembly)
  {
    if (assembly != null)
    {
      _assemblies.Add(assembly);
    }
  }

  void LoadAssemblies()
  {
    // TODO: Replace these types with any type in the appropriate assembly/project
    var coreAssembly = Assembly.GetAssembly(typeof(Contributor));
    var infrastructureAssembly = Assembly.GetAssembly(typeof(AutofacInfrastructureModule));
    var useCasesAssembly = Assembly.GetAssembly(typeof(CreateContributorCommand));

    AddToAssembliesIfNotNull(coreAssembly);
    AddToAssembliesIfNotNull(infrastructureAssembly);
    AddToAssembliesIfNotNull(useCasesAssembly);
  }

  protected override void Load(ContainerBuilder builder)
  {
    LoadAssemblies();
    if (_isDevelopment)
    {
      RegisterDevelopmentOnlyDependencies(builder);
    }
    else
    {
      RegisterProductionOnlyDependencies(builder);
    }
    RegisterEF(builder);
    RegisterQueries(builder);
    RegisterMediatR(builder);
  }

  static void RegisterEF(ContainerBuilder builder) =>
    builder.RegisterGeneric(typeof(EfRepository<>))
      .As(typeof(IRepository<>))
      .As(typeof(IReadRepository<>))
      .InstancePerLifetimeScope();

  static void RegisterQueries(ContainerBuilder builder) =>
    builder.RegisterType<ListContributorsQueryService>()
      .As<IListContributorsQueryService>()
      .InstancePerLifetimeScope();

  void RegisterMediatR(ContainerBuilder builder)
  {
    builder
      .RegisterType<Mediator>()
      .As<IMediator>()
      .InstancePerLifetimeScope();

    builder
      .RegisterGeneric(typeof(LoggingBehavior<,>))
      .As(typeof(IPipelineBehavior<,>))
      .InstancePerLifetimeScope();

    builder
      .RegisterType<MediatRDomainEventDispatcher>()
      .As<IDomainEventDispatcher>()
      .InstancePerLifetimeScope();

    Type[] mediatrOpenTypes =
    [
      typeof(IRequestHandler<,>),
      typeof(IRequestExceptionHandler<,,>),
      typeof(IRequestExceptionAction<,>),
      typeof(INotificationHandler<>),
    ];

    foreach (Type mediatrOpenType in mediatrOpenTypes)
    {
      builder
          .RegisterAssemblyTypes([.. _assemblies])
          .AsClosedTypesOf(mediatrOpenType)
          .AsImplementedInterfaces();
    }
  }

  static void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
  {
    // NOTE: Add any development only services here
    builder.RegisterType<FakeEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();

    //builder.RegisterType<FakeListContributorsQueryService>()
    //  .As<IListContributorsQueryService>()
    //  .InstancePerLifetimeScope();
  }
  static void RegisterProductionOnlyDependencies(ContainerBuilder builder) =>
    // NOTE: Add any production only (real) services here
    builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();
}
