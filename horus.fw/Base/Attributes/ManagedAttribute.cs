using horus.fw.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ManagedAttribute : Attribute
    {
        public Browser Browser { get; set; }

        public string DriverPath { get; set; }

        public string ProfilePath { get; set; }

        public string ProxyString { get; set; }

        public bool ChromeHeadlessMode { get; set; }

        public int Timeout { get; set; }

        public WindowMaximize WindowMaximize { get; set; }

        public string Url { get; set; }
    }
}
