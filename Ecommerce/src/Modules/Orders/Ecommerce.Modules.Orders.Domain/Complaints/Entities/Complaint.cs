using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Entities
{
    public class Complaint : AggregateRoot, IAuditable
    {
        public Order Order { get; private set; } = default!;
        public Guid OrderId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string? AdditionalNote { get; private set; }
        public Decision? Decision { get; private set; } = new();
        public ComplainStatus Status { get; private set; } = ComplainStatus.NotDecided;
        public bool IsCompleted => Status is ComplainStatus.Approved || Status is ComplainStatus.Rejected;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Complaint(Guid id, Order order, string title, string description)
        {
            Id = id;
            Order = order;
            Title = title;
            Description = description;
        }
        public Complaint()
        {
            
        }
        public void ChangeStatus(ComplainStatus status)
        {
            Status = status;
        }
        public void SetNote(string notes)
        {
            AdditionalNote = notes;
            IncrementVersion();
        }
        public void Approve(Decision decision)
        {
            WriteDecision(decision);
            ChangeStatus(ComplainStatus.Approved);
            IncrementVersion();
        }
        public void Reject(Decision decision)
        {
            WriteDecision(decision);
            ChangeStatus(ComplainStatus.Rejected);
            IncrementVersion();
        }
        public void WriteDecision(Decision decision)
        {
            if(decision.RefundAmount is not null && decision.RefundAmount < 0)
            {
                throw new ComplaintRefundAmountBelowZeroException();
            }
            Decision = decision;
        }
        public void EditDecision(Decision decision)
        {
            WriteDecision(decision);
            IncrementVersion();
        }
    }
}
