using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Delivery
{
    public class DPDOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string GenerationPolicy { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string OutputDocFormat { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string OutputType { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
        public string OrganizationFID { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
