﻿using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Entity
{
    public class Return
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; } = new();
        public Order Order { get; set; } = new();
        public Guid OrderId { get; set; }
        private readonly List<ReturnProduct> _products = [];
        public IEnumerable<ReturnProduct> Products => _products;
        public string ReasonForReturn { get; set; } = string.Empty;
        public string AdditionalNote { get; set; } = string.Empty;
        public ReturnStatus Status { get; set; } = ReturnStatus.NotHandled;
        public bool IsCompleted => Status is ReturnStatus.Handled;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Return(Guid id, Customer customer, Order order, IEnumerable<ReturnProduct> products, string reasonForReturn, 
            string additionalNote, DateTime createdAt)
        {
            Id = id;
            Customer = customer;
            Order = order;
            _products = products.ToList();
            ReasonForReturn = reasonForReturn;
            AdditionalNote = additionalNote;
            CreatedAt = createdAt;
        }
        public void ChangeStatus(ReturnStatus status, DateTime updatedAt)
        {
            Status = status;
            UpdatedAt = updatedAt;
        }
        public void setNote(string note, DateTime updatedAt)
        {
            AdditionalNote = note;
            UpdatedAt = updatedAt;
        }
    }
}
