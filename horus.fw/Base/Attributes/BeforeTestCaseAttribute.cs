using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeTestCaseAttribute : TestCaseAttribute
    {
        public string SkippedIds { get; set; }

        public string RunningIds { get; set; }
    }
}
