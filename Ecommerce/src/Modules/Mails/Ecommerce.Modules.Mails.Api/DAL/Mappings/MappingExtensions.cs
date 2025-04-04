using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static MailBrowseDto AsBrowseDto(this Mail mail)
            => new()
            {
                Id = mail.Id,
                From = mail.From,
                To = mail.To,
                Subject = mail.Subject,
                AttachmentFiles = mail.AttachmentFiles?.Select(af => af.AsDto()),
                CreatedAt = mail.CreatedAt
            };
        public static MailDetailsDto AsDetailsDto(this Mail mail)
            => new()
            {
                Id = mail.Id,
                From = mail.From,
                To = mail.To,
                Subject = mail.Subject,
                Body = mail.Body,
                AttachmentFiles = mail.AttachmentFiles?.Select(af => af.AsDto()),
                CreatedAt = mail.CreatedAt
            };

        public static AttachmentFileDto AsDto(this AttachmentFile file)
            => new()
            {
                Id = file.Id,
                FileName = file.FileName
            };
    }
}
