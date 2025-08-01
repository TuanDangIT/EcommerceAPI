﻿using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Modules.Mails.Api.Sieve;
using Ecommerce.Shared.Abstractions.Modules;
using Ecommerce.Shared.Infrastructure.Delivery;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api
{
    internal class MailsModule : IModule
    {
        public const string BasePath = "mails-module";
        public string Name { get; } = "Mails";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgres<MailsDbContext>();
            services.AddScoped<IMailsDbContext>(sp =>
            {
                return sp.GetRequiredService<MailsDbContext>();
            });
            services.AddScoped<IMailService, MailService>();
            services.AddKeyedScoped<ISieveProcessor, MailsModuleSieveProcessor>("mails-sieve-processor");
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Mails module is working...");
            });
        }
    }

}
