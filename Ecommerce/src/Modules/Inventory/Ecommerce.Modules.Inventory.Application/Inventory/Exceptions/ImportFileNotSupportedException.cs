using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ImportFileNotSupportedException(string contentType) : 
        EcommerceException($"Given file with content-type: {contentType} is not supported. Please use {MediaTypeNames.Text.Csv} files.")
    {
    }
}
