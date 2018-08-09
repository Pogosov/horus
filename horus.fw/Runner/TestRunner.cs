using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.Base.Model;
using horus.fw.FwUtil;

namespace horus.fw.Runner
{
    public class TestRunner
    {
        public List<TestSuite> RunTest(RunnerMode mode)
        {
            var assembly = Assembly.GetCallingAssembly();
            var xmlSuites = Supporter.XmlGetTestExecutionInfo(mode);
            var testClasses = Supporter.GetTestClasses(xmlSuites, assembly);

            Logger.LogMsg(Severity.INFO, $" >>> The following suites will be executed <<<<");
            Logger.LogMsg(Severity.INFO, string.Join(Environment.NewLine, testClasses.Select(c => c.Name)));

            var index = 1;
            var testSuites = new List<TestSuite>();
            foreach (var testClass in testClasses)
            {
                var isRerun = false;
                var rerunTime = 0;
                var failedCases = new List<TestCase>();
                var testMethodsInClass = Supporter.GetTestMethods(xmlSuites, testClass);
                do
                {
                    var testSuite = Supporter.CreateTestSuite(testClass, index);
                    Logger.LogMsg(Severity.INFO, $"Test suite: {testSuite.Name} stared at: {DateTime.Now}");

                    var testMethodsExecuted = failedCases.Any() ?
                        testMethodsInClass.Where(t => failedCases.Any(tc => tc.Name.ToLower().Equals(t.Name.ToLower()))).Distinct().ToList() :
                        testMethodsInClass.Distinct().ToList();
                    var testCases = Supporter.ExecuteTestMethodsInClass(testClass, testMethodsExecuted, testSuite);

                    testSuite.AddTestCases(testCases);
                    testSuite.Finish();
                    testSuites.Add(testSuite);

                    Logger.WriteLine(testSuite);
                    Logger.LogMsg(Severity.INFO, $"Test suite: {testSuite.Name} completed at: {DateTime.Now} => runtime: {rerunTime + 1}, rerun: {rerunTime}");

                    failedCases = testCases.Where(t => t.Status == Status.Failed).ToList();
                    isRerun = (failedCases.Any() && rerunTime < Config.RerunCase && testSuite.Status == Status.Undefined);
                    rerunTime++;
                    index++;
                } while (isRerun);
            }

            var reporter = new Reporter();
            reporter.GenerateReports(testSuites);

            return testSuites;
        }

        public void WriteLine(List<TestSuite> suites)
        {
            foreach (TestSuite suite in suites)
            {
                Logger.WriteLine(suite);
            }
        }
    }
}
