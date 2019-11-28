﻿using System.Threading.Tasks;
using CleanArchitecture.SharedKernel.Interfaces;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.UnitTests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task Dispatch(BaseDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
