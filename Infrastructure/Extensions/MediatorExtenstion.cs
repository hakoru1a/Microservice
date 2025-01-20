﻿using Constracts.Common.Events;
using Constracts.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MediatorExtenstion
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
