using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class BackgroundJobSettings
    {
        public string HangfireUrl {set; get; }

        public string CheckourUrl {set; get; }

        public string BasketUrl { set; get; }

        public string ScheduleJobUrl { set; get; }
    }
}
