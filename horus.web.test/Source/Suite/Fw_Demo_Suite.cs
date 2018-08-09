using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base.Attributes;
using horus.fw.Assertion;
using horus.fw.FwUtil;
using horus.fw.web.Source.Step;
using horus.fw.Runner;
using horus.fw.Base;

namespace horus.fw.api.Source.Suite
{
    [TestSuite]
    public class Fw_Demo_Suite
    {
        [Steps]
        Fw_Demo_Step FwDemoStep { get; set; }

        public Fw_Demo_Suite()
        {
            SuiteFactory.InitSuite(this);
        }

        [BeforeSuite]
        public void Before_Suite()
        {
            Logger.LogMsg(Severity.WARN, "Before_Suite");
        }

        [BeforeTestCase]
        public void Before_Test_Case()
        {
            Logger.LogMsg(Severity.WARN, "Before_Test_Case");
        }

        [TestCase]
        public void Test_Case_1()
        {
            FwDemoStep.Test_Step_1();
        }

        [TestCase]
        public void Test_Case_2()
        {
            FwDemoStep.Test_Step_2();
        }

        [TestCase]
        public void Test_Case_3()
        {
            FwDemoStep.Test_Step_3();
        }

        [TestCase(Status = Status.Pending, Comment = "Did not complete the test case yet!")]
        public void Test_Case_4()
        {
            FwDemoStep.Test_Step_4();
        }

        [AfterTestCase]
        public void After_Test_Case()
        {
            Logger.LogMsg(Severity.WARN, "After_Test_Case");
        }

        [AfterSuite]
        public void After_Suite()
        {
            Logger.LogMsg(Severity.WARN, "After_Suite");
        }
    }
}
