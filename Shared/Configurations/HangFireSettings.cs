﻿using Shared.Configurations.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class HangFireSettings
    {
        public string Route { get; set; }
        public string ServerName { get; set; }
        public Dashboard Dashboard { get; set; }
        public DatabaseSettings Storage { get; set; }
    }

    public class Dashboard
    {
        public string AppPath { get; set; }
        public int StatsPollingInterval { get; set; }
        public string DashboardTitle { get; set; }
    }

   
}