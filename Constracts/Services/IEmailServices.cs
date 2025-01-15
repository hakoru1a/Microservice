﻿using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Services
{
    public interface  IEmailServices<in T> where T : class
    {
        Task SendEmailAsync(T request, CancellationToken cancellationToken = new CancellationToken());
    }
}
