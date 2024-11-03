using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Entities
{
    public class Complaint
    {
        public Guid Id { get; set; }
        public Order Order { get; set; } = new();
        public Guid OrderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AdditionalNote { get; set; }
        public Decision? Decision { get; set; } = new();
        public ComplainStatus Status { get; set; } = ComplainStatus.NotDecided;
        public bool IsCompleted => Status is ComplainStatus.Approved || Status is ComplainStatus.Rejected;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public Complaint(Guid id, Order order, string title, string description, DateTime createdAt)
        {
            Id = id;
            Order = order;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
        }
        public Complaint()
        {
            
        }
        public void ChangeStatus(ComplainStatus status, DateTime updatedAt)
        {
            Status = status;
            UpdatedAt = updatedAt;
        }
        public void SetNote(string notes, DateTime updatedAt)
        {
            AdditionalNote = notes;
            UpdatedAt = updatedAt;
        }
        public void Approve(Decision decision, DateTime updatedAt)
        {
            WriteDecision(decision);
            Status = ComplainStatus.Approved;
            UpdatedAt = updatedAt;
        }
        public void Reject(Decision decision, DateTime updatedAt)
        {
            WriteDecision(decision);
            Status = ComplainStatus.Rejected;
            UpdatedAt = updatedAt;
        }
        public void WriteDecision(Decision decision)
        {
            Decision = decision;
        }
        public void EditDecision(Decision decision, DateTime updatedAt)
        {
            Decision = decision;
            UpdatedAt = updatedAt;
        }
    }
}
