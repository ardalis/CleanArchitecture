using Ardalis.GuardClauses;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clean.Architecture.Core.ProjectAggregate
{
    public class Project : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }

        private List<ToDoItem> _items = new List<ToDoItem>();
        public IEnumerable<ToDoItem> Items => _items.AsReadOnly();
        public ProjectStatus Status => _items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

        public Project(string name)
        {
            Name = name;
        }

        public void AddItem(ToDoItem newItem)
        {
            Guard.Against.Null(newItem, nameof(newItem));
            _items.Add(newItem);
        }
    }
}
