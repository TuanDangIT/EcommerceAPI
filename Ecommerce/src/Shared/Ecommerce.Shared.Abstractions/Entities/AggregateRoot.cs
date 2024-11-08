using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Entities
{
    public abstract class AggregateRoot<T> : BaseEntity<T>
    {
        public int Version { get; protected set; }
        public IEnumerable<IDomainEvent> Events => _events;
        private readonly List<IDomainEvent> _events = new();
        private bool _versionIncremented;
        protected void AddEvent(IDomainEvent @event)
        {
            if (!_events.Any() && !_versionIncremented)
            {
                Version++;
                _versionIncremented = true;
            }
            _events.Add(@event);
        }
        public void ClearEvents() => _events.Clear();
        protected void IncrementVersion()
        {
            if (_versionIncremented)
            {
                return;
            }
            Version++;
            _versionIncremented = true;
        }
    }
    public abstract class AggregateRoot : AggregateRoot<Guid>
    {
    }
}
