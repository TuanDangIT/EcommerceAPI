using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class IndividualDiscountCannotBeAppliedException : EcommerceException
    {
        public IndividualDiscountCannotBeAppliedException() : base($"Discount cannot be applied because of diffrent user or not matching SKU.")
        {
        }
    }
}
