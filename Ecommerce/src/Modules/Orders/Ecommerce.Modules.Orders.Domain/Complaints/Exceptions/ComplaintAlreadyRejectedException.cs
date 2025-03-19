using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Exceptions
{
    internal class ComplaintAlreadyRejectedException(Guid complaintId) : EcommerceException($"Complaint: {complaintId} cannot be rejected more than twice.")
    {
    }
}
