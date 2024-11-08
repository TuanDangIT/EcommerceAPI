using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;

using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Entities
{
    public class Return : AggregateRoot, IAuditable
    {
        public Order Order { get; private set; } = new();
        public Guid OrderId { get; private set; }
        private readonly List<ReturnProduct> _products = [];
        public IEnumerable<ReturnProduct> Products => _products;
        public string ReasonForReturn { get; private set; } = string.Empty;
        public string? AdditionalNote { get; private set; }
        public string? RejectReason { get; private set; }
        public ReturnStatus Status { get; private set; } = ReturnStatus.NotHandled;
        public bool IsFullReturn { get; private set; }
        public bool IsCompleted => Status is ReturnStatus.Handled;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Return(Guid id, Order order, IEnumerable<ReturnProduct> products, string reasonForReturn, 
            bool isFullReturn)
        {
            Id = id;
            Order = order;
            _products = products.ToList();
            ReasonForReturn = reasonForReturn;
            IsFullReturn = isFullReturn;
        }
        private Return()
        {
            
        }
        public void ChangeStatus(ReturnStatus status)
        {
            Status = status;
        }
        public void Handle()
        {
            ChangeStatus(ReturnStatus.Handled);
            IncrementVersion();
        }
        public void SetNote(string note)
        {
            AdditionalNote = note;
            IncrementVersion();
        }
        public void Reject(string rejectReason)
        {
            RejectReason = rejectReason;    
            ChangeStatus(ReturnStatus.Rejected);
            IncrementVersion();
        }
    }
}
