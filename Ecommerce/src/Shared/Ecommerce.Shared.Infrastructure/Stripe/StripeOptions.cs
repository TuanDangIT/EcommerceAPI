using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Stripe
{
    public class StripeOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Mode {  get; set; } = string.Empty;
        public string Currency {  get; set; } = string.Empty;
        public string BlobStorageUrl { get; set; } = string.Empty;
        public string WebhookSecret { get; set; } = string.Empty;
    }
}
