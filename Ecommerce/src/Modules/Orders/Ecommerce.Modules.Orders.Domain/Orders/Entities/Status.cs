﻿using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Status : BaseEntity<int>
    {
        public OrderStatus OrderStatus {  get; private set; }
        public Status(int id, OrderStatus orderStatus)
        {
            Id = id;
            OrderStatus = orderStatus;
        }
    }
}
