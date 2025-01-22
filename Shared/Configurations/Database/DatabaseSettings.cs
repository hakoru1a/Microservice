﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations.Database
{
    public abstract class DatabaseSettings
    {
        public string ConnectionStrings { set; get; }

        public string DBProvider { set; get; }
    }
}
