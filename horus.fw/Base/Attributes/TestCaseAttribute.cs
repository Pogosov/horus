using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestCaseAttribute : Attribute
    {
        public string ID { get; set; }

        public string Author { get; set; }

        public Status Status { get; set; }

        public bool IsStopper { get; set; }

        public string TestData { get; set; }

        public string Comment { get; set; }
    }
}
