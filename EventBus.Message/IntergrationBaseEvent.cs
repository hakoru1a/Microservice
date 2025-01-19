﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages
{
    public record IntergrationBaseEvent() : IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }

        public Guid Id { get; set; }
    }
}
