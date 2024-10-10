﻿using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Sieve
{
    public class UsersModuleSieveProcessor : SieveProcessor
    {
        public UsersModuleSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }
    }
}
