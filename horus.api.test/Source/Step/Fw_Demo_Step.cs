using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Assertion;
using horus.fw.Base;
using horus.fw.Base.Attributes;
using horus.fw.FwUtil;
using horus.fw.Runner;

namespace horus.fw.api.Source.Step
{
    public class Fw_Demo_Step : MarshalByRefObject
    {
        [TestStep]
        public void Test_Step_1()
        {
            Logger.LogMsg(Severity.WARN, "Test_Step_1");
        }

        [TestStep]
        public void Test_Step_2()
        {
            Logger.LogMsg(Severity.WARN, "Test_Step_2");
            Assert.IsFalse(true);
        }

        [TestStep]
        public void Test_Step_3()
        {
            Logger.LogMsg(Severity.WARN, "Test_Step_3");
        }

        [TestStep]
        public void Test_Step_4()
        {
            Logger.LogMsg(Severity.WARN, "Test_Step_4");
        }
    }
}
