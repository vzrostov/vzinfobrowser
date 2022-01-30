using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VZInfoBrowser.Requests
{
    public class RedisOptions
    {
        public string Title { get { return "Redis"; } }
        public string ConfigurationName { get; set; }
        public string Host { get; set; }

        public string Port { get; set; }

    }
}
