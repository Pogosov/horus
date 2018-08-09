using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using horus.fw.Base;
using horus.fw.Base.Attributes;
using horus.fw.Base.Model;
using horus.fw.FwUtil;

namespace horus.fw.Runner
{
    public class Supporter
    {
        public static List<TestSuite> XmlGetTestExecutionInfo(RunnerMode mode)
        {
            if (!mode.Equals(RunnerMode.Xml))
                return null;

            var runSuites = new List<TestSuite>();
            var runSuite = new TestSuite();
            var runCases = new List<TestCase>();

            var xmlPath = Config.TestExecutionXmlPath;
            if (File.Exists(xmlPath))
            {
                using (XmlReader reader = XmlReader.Create(xmlPath))
                {
                    bool isCaseRun = false;
                    bool isSuiteRun = false;
                    string name = string.Empty;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            switch (reader.Name)
                            {
                                case "Suite":
                                    isSuiteRun = bool.Parse(reader["IsRun"]);
                                    name = reader["Name"];
                                    if (name != null && isSuiteRun)
                                    {
                                        runSuite = new TestSuite();
                                        runSuite.SetName(name);
                                        runCases = new List<TestCase>();
                                    }

                                    break;
                                case "Case":
                                    isCaseRun = bool.Parse(reader["IsRun"]);
                                    if (isSuiteRun && isCaseRun && reader.Read())
                                    {
                                        var runCase = new TestCase();
                                        runCase.SetName(reader.Value.Trim());
                                        runCases.Add(runCase);
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            return runSuites.ToList();
        }

        public static List<Type> GetTestClasses(List<TestSuite> xmlSuites, Assembly assembly)
        {
            var testClasses = new List<Type>();
            testClasses.AddRange(assembly.GetTypes(typeof(TestSuiteAttribute)));

            if (xmlSuites != null && xmlSuites.Any())
            {
                var testSuiteNames = xmlSuites.Select(s => s.Name.ToLower());
                testClasses = testClasses.Where(c => testSuiteNames.Contains(c.Name.ToLower())).ToList();
            }

            return testClasses.OrderBy(c => c.Name).ToList();
        }

        public static List<MethodInfo> GetTestMethods(List<TestSuite> xmlSuites, Type testClass)
        {
            var testMethods = new List<MethodInfo>();
            testMethods.AddRange(testClass.GetMethods(typeof(TestCaseAttribute)));

            if (xmlSuites != null && xmlSuites.Any())
            {
                var suite = xmlSuites.FirstOrDefault(s => s.Name.ToLower().Equals(testClass.Name.ToLower()));
                if (suite != null && suite.Tests.Any())
                {
                    var testCaseNames = suite.Tests.Select(t => t.Name.ToLower());
                    if (testCaseNames.Any())
                    {
                        testMethods = testMethods.Where(m => testCaseNames.Contains(m.Name.ToLower())).ToList();
                    }
                }
            }

            return testMethods.ToList();
        }

        public static TestSuite CreateTestSuite(Type testClass, int index)
        {
            var testSuite = new TestSuite();
            testSuite.SetName(testClass.Name);
            testSuite.SetIndex(index);

            var testSuiteAttr = testClass.TestSuite();
            testSuite.SetID(testSuiteAttr.ID);
            testSuite.SetComment(testSuiteAttr.Comment);
            testSuite.SetStatus(testSuiteAttr.Status);
            testSuite.SetReportAllMethods(testSuiteAttr.ReportAllMethods);

            return testSuite;
        }

        public static List<TestCase> ExecuteTestMethodsInClass(Type testClass, List<MethodInfo> testMethods, TestSuite testSuite)
        {
            var testCases = new List<TestCase>();
            if (testMethods.Any())
            {
                // Create test suite instance
                object suiteInstance = Activator.CreateInstance(testClass);

                // Order test cases by ID
                testMethods = testMethods.OrderBy(t => t.TestCase().ID).ToList();

                // Collect & execute before suite methods
                var beforeSuiteMethods = testClass.GetMethods(typeof(BeforeSuiteAttribute));
                foreach (var beforeSuiteMethod in beforeSuiteMethods)
                {
                    var test = ExecuteTestMethod(beforeSuiteMethod, Status.Undefined, suiteInstance, 0);
                    if (testSuite.ReportAllMethods)
                    {
                        testCases.Add(test);
                    }
                }

                // Collect all before & after test case methods
                var allBeforeTCs = testClass.GetMethods(typeof(BeforeTestCaseAttribute));
                var allAfterTCs = testClass.GetMethods(typeof(AfterTestCaseAttribute));

                // Execute test case methods
                for (int i = 0; i < testMethods.Count; i++)
                {
                    var dataDrivenName = testMethods[i].TestCase().TestData;
                    List<object[]> para = new List<object[]>();

                    if (!string.IsNullOrWhiteSpace(dataDrivenName))
                        para = (List<object[]>)testClass.GetField(dataDrivenName, Constant.NonPublicInstanceOptions).GetValue(suiteInstance);

                    int j = 0;
                    do
                    {
                        var testCase = CreateTestCase(testMethods[i], testSuite.Status, i + 1 + j);

                        // Collect before test case methods
                        var specifiedBeforeTCs = allBeforeTCs.GetMethods(typeof(BeforeTestCaseAttribute), testCase.ID);
                        foreach (var method in specifiedBeforeTCs)
                        {
                            var test = ExecuteTestMethod(method, Status.Undefined, suiteInstance, 0);
                            if (testSuite.ReportAllMethods)
                            {
                                testCases.Add(test);
                            }
                        }

                        // Execute test case
                        if (testMethods[i].GetParameters().Any())
                        {
                            testCase = ExecuteTestMethod(testMethods[i], testSuite.Status, suiteInstance, i + 1, para[j], testCase);
                        }
                        else
                        {
                            testCase = ExecuteTestMethod(testMethods[i], testSuite.Status, suiteInstance, i + 1, null, testCase);
                        }

                        testCases.Add(testCase);

                        // If the test case is master one
                        if (testCase.IsStopper && testCase.Status == Status.Failed)
                        {
                            Logger.LogMsg(Severity.INFO, $"Test case {testCase.Name} is a stopper test case. It failed then all following test cases will be skipped.");
                            testCase.SetComment("This is a stopper test case. It failed then all following test cases will be skipped!");
                            testSuite.SetStatus(Status.Skipped);
                        }

                        // Collect after test case methods
                        var specifiedAfterTCs = allAfterTCs.GetMethods(typeof(AfterTestCaseAttribute), testCase.ID);
                        foreach (var method in specifiedAfterTCs)
                        {
                            // Execute after test case method if "test case status" equals to "after test case method status"
                            var afterTCStatus = method.AfterTest().Status;
                            if (afterTCStatus != Status.Undefined && afterTCStatus != testCase.Status)
                            {
                                continue;
                            }

                            var test = ExecuteTestMethod(method, Status.Undefined, suiteInstance, 0);
                            if (testSuite.ReportAllMethods)
                            {
                                testCases.Add(test);
                            }
                        }

                        j++;
                    } while (j < para.Count());
                }

                // Collect & execute after suite methods
                var afterSuiteMethods = testClass.GetMethods(typeof(AfterSuiteAttribute));
                foreach (var afterSuiteMethod in afterSuiteMethods)
                {
                    var test = ExecuteTestMethod(afterSuiteMethod, Status.Undefined, suiteInstance, 0);
                    if (testSuite.ReportAllMethods)
                    {
                        testCases.Add(test);
                    }
                }
            }

            return testCases;
        }

        public static TestCase ExecuteTestMethod(MethodInfo testMethod, Status suiteStatus, object suiteInstance, int index, object[] paras = null, TestCase testCase = null)
        {
            if (testCase == null)
            {
                testCase = CreateTestCase(testMethod, suiteStatus, index);
            }

            Logger.LogMsg(Severity.INFO, $"Test case: {testMethod.Name} started at: {DateTime.Now}");
            object testReturn = null;
            try
            {
                StepProxy.InitiateTestExecution(testCase.Status);
                testReturn = testMethod.Invoke(suiteInstance, paras);
            }
            catch (Exception ex)
            {
                testCase.SetStatus(Status.Failed);
                Logger.LogMsg(Severity.ERROR, $"Runner exception: {ex.GetLastInnerException().Message}. StackTrace: {ex.GetLastInnerException().StackTrace}");
                testCase.SetError($"Runner exception: {ex.GetLastInnerException().Message}. StackTrace: {ex.GetLastInnerException().StackTrace}");
            }

            testCase.SetValue(testReturn);
            testCase.AddTestSteps(StepProxy.TestSteps);
            StepProxy.CompleteTestExecution();
            if (testCase.Status == Status.Undefined)
            {
                testCase.SetStatus(DetermineTestCaseStatus(testCase));
            }

            testCase.Finish();
            Logger.LogMsg(Severity.INFO, $"Test case: {testCase.Name} => Status is: {testCase.Status}...");
            Logger.LogMsg(Severity.INFO, $"Test case: {testMethod.Name} completed at: {DateTime.Now}...");

            return testCase;
        }

        private static Status DetermineTestCaseStatus(TestCase testCase)
        {
            var verifiedFailedSteps = new List<TestStep>();
            var allFailedSteps = testCase.Steps.Where(s => s.Status == Status.Failed);
            foreach (var failedStep in allFailedSteps)
            {
                if (!testCase.Steps.Any(s => s.Name.Equals(failedStep.Name)
                    && s.Status == Status.Passed && (s.Index - failedStep.Index >= 1
                    && s.Index - failedStep.Index <= Config.RetryStep)))
                {
                    verifiedFailedSteps.Add(failedStep);
                }
            }

            if (verifiedFailedSteps.Any())
            {
                testCase.SetStatus(Status.Failed);
            }
            else
            {
                testCase.SetStatus(Status.Passed);
            }

            return testCase.Status;
        }

        private static TestCase CreateTestCase(MethodInfo testMethod, Status suiteStatus, int index)
        {
            var paramStr = string.Empty;
            var paramInfos = testMethod.GetParameters();
            if (paramInfos.Any())
            {
                foreach (var paramInfo in paramInfos)
                {
                    paramStr = $"{paramStr}, {paramInfo.ParameterType.Name} {paramInfo.Name}";
                }

                paramStr = $"({paramStr.Trim(',').Trim()})";
            }

            var testCase = new TestCase();
            testCase.SetName($"{testMethod.Name} {paramStr}".Trim());
            testCase.SetIndex(index);

            var caseAttr = testMethod.TestCase();
            testCase.SetID(caseAttr.ID);
            testCase.SetAuthor(caseAttr.Author);
            testCase.SetComment(caseAttr.Comment);
            testCase.SetStopper(caseAttr.IsStopper);

            if (suiteStatus != Status.Undefined)
            {
                caseAttr.Status = suiteStatus;
            }

            testCase.SetStatus(caseAttr.Status);

            return testCase;
        }
    }
}
