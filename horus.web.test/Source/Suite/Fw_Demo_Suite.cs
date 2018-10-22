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
        private List<object[]> Paras = new List<object[]>()
        {
            new object[] { "1111", 2222 },
            new object[] { "3333", 4444 }
        };

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

        [TestCase(ID = "1.1")]
        public void Test_Case_1()
        {
            FwDemoStep.Test_Step_1();
        }

        [TestCase(ID = "1.2")]
        public void Test_Case_2()
        {
            FwDemoStep.Test_Step_2();
        }

        [TestCase(ID = "1.3", TestData = "Paras")]
        public void Test_Case_3(string para1, int para2)
        {
            Console.WriteLine($"Data-Driven Test => values: para1='{para1}', para2='{para2}'...");
            FwDemoStep.Test_Step_3();
        }

        [TestCase(ID = "1.4", Status = Status.Pending, Comment = "Did not complete the test case yet!")]
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
