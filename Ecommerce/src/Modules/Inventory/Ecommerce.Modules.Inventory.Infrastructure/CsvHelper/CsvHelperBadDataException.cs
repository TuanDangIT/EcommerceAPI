using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.CsvHelper
{
    internal class CsvHelperBadDataException : EcommerceException
    {
        public CsvHelperBadDataException() : base("One or more datas are incorrect.")
        {
        }
    }
}
