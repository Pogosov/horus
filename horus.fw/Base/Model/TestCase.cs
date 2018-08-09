using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base.Model
{
    public class TestCase : TestBase
    {
        public List<TestStep> Steps { get; private set; }

        public TestCase() : base()
        {
            Steps = new List<TestStep>();
        }

        public void AddTestSteps(List<TestStep> testSteps)
        {
            Steps.AddRange(testSteps);
        }
    }
}
