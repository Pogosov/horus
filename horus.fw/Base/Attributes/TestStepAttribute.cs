using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestStepAttribute : Attribute
    {
        public string ID { get; set; }

        public Status Status { get; set; }

        public string Comment { get; set; }
    }
}
