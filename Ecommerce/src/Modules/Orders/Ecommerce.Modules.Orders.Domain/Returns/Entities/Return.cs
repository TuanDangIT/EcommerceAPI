using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;

using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Entities
{
    public class Return
    {
        public Guid Id { get; set; }
        public Order Order { get; set; } = new();
        public Guid OrderId { get; set; }
        private readonly List<ReturnProduct> _products = [];
        public IEnumerable<ReturnProduct> Products => _products;
        public string ReasonForReturn { get; set; } = string.Empty;
        public string? AdditionalNote { get; set; }
        public string? RejectReason { get; set; }
        public ReturnStatus Status { get; set; } = ReturnStatus.NotHandled;
        public bool IsFullReturn { get; set; }
        public bool IsCompleted => Status is ReturnStatus.Handled;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Return(Guid id, Order order, IEnumerable<ReturnProduct> products, string reasonForReturn, 
            bool isFullReturn, DateTime createdAt)
        {
            Id = id;
            Order = order;
            _products = products.ToList();
            ReasonForReturn = reasonForReturn;
            IsFullReturn = isFullReturn;
            CreatedAt = createdAt;
        }
        private Return()
        {
            
        }
        public void ChangeStatus(ReturnStatus status, DateTime updatedAt)
        {
            Status = status;
            UpdatedAt = updatedAt;
        }
        public void Handle(DateTime updatedAt)
        {
            Status = ReturnStatus.Handled; 
            UpdatedAt = updatedAt;
        }
        public void SetNote(string note, DateTime updatedAt)
        {
            AdditionalNote = note;
            UpdatedAt = updatedAt;
        }
        public void Reject(string rejectReason, DateTime updatedAt)
        {
            RejectReason = rejectReason;    
            Status = ReturnStatus.Rejected;
            UpdatedAt = updatedAt;
        }
    }
}
