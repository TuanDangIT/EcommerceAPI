﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class DiscountDto
    {
        public string Code { get; set; } = string.Empty;
        public DateTime? EndingDate { get; set; }
        public DateTime CreatedAt { get; set; }
}
}
