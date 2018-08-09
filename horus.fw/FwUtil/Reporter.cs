using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base.Model;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using System.IO;

namespace horus.fw.FwUtil
{
    public class Reporter
    {
        public void GenerateReports(List<TestSuite> suites)
        {
            var extent = new ExtentReports();
            extent.AttachReporter(CreateExtentHtmlReporter());
            extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            foreach (var suite in suites)
            {
                var testSuite = extent.CreateTest(suite.Name);
                foreach (var test in suite.Tests)
                {
                    var testCase = testSuite.CreateNode(test.Name);
                    testCase.AssignCategory(suite.Name);
                    testCase.AssignAuthor(test.Author);
                    foreach (var step in test.Steps)
                    {
                        var extraInfo = string.Empty;
                        if (!string.IsNullOrWhiteSpace(step.Comment))
                        {
                            extraInfo = $", with comment => {step.Comment}";
                        }

                        var errorInfo = string.Empty;
                        if (!string.IsNullOrWhiteSpace(step.Error))
                        {
                            errorInfo = $". There is an error => {step.Error}";
                        }

                        switch (step.Status)
                        {
                            case Base.Status.Passed:
                                testCase.Pass($"{step.Name} : Passed{extraInfo}");
                                break;
                            case Base.Status.Failed:
                                testCase.Fail($"{step.Name} : Failed{extraInfo}{errorInfo}");
                                break;
                            case Base.Status.Skipped:
                                testCase.Skip($"{step.Name} : Skipped{extraInfo}");
                                break;
                            case Base.Status.Pending:
                                testCase.Warning($"{step.Name} : Pending{extraInfo}");
                                break;
                            default:
                                testCase.Info(step.Error);
                                break;
                        }

                        testCase.AddScreenCaptureFromPath(step.ScreenshotPath);
                    }
                }
            }

            extent.Flush();
        }

        public static void CleanUpReport()
        {
            // Clean up report
            FwUtil.DeleteFilesAndSubDirectories(Path.Combine(Config.ProjectPath, "Screenshot"));
            FwUtil.DeleteFilesAndSubDirectories(Path.Combine(Config.ProjectPath, "Report"));
        }

        private ExtentHtmlReporter CreateExtentHtmlReporter()
        {
            var htmlReporter = new ExtentHtmlReporter($@"{Config.ReportPath}\Fossil Automation Test_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.html");
            htmlReporter.Configuration().DocumentTitle = "Fossil Automation Test - ExtentReports";
            htmlReporter.Configuration().ReportName = "Fossil Automation Test Report";

            htmlReporter.Configuration().ChartVisibilityOnOpen = true;
            htmlReporter.Configuration().ChartLocation = ChartLocation.Top;
            htmlReporter.Configuration().Encoding = "UTF-8";
            htmlReporter.Configuration().Protocol = Protocol.HTTP;
            htmlReporter.Configuration().Theme = Theme.Dark;

            return htmlReporter;
        }
    }
}
