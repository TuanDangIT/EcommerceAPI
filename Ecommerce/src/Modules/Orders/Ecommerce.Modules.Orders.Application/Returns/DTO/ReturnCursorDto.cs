﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.DTO
{
    public class ReturnCursorDto
    {
        public Guid? CursorId { get; set; }
        public DateTime? CursorCreatedAt { get; set; }
    }
}
