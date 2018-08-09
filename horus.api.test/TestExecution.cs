using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.FwUtil;
using horus.fw.Runner;

namespace horus.fw.api
{
    class TestExecution
    {
        static void Main(string[] args)
        {
            // ===================== TEST PREPARATION ===================== //
            Logger.CleanUpLog();
            Reporter.CleanUpReport();

            // ===================== TEST RUNNER EXECUTES TESTS ===================== //
            var testRunner = new TestRunner();
            var suites = testRunner.RunTest(RunnerMode.All);
            testRunner.WriteLine(suites);

            // ===================== COMPLETE TEST ===================== //
            Console.Read();
        }
    }
}
