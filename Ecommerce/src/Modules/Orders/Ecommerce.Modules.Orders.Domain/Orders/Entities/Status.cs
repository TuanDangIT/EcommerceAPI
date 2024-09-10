﻿using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus {  get; set; }
        public Status(int id, OrderStatus orderStatus)
        {
            Id = id;
            OrderStatus = orderStatus;
        }
    }
}
