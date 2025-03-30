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
        public CsvHelperBadDataException(string header, int row, string value) : base($"{header} in row: {row} has invalid value: {value}.")
        {
        }
        public CsvHelperBadDataException(string header, int row) : base($"{header} in row: {row} cannot be empty;")
        {
            
        }
    }
}
