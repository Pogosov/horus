using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.Base.Attributes;
using horus.fw.Base.Model;
using horus.fw.FwUtil;
using OpenQA.Selenium;

namespace horus.fw.Runner
{
    public class StepProxy : RealProxy
    {
        private readonly object Decorated;

        public static List<TestStep> TestSteps { get; set; }

        private static Status TestCaseStatus { get; set; }

        private static Status PreviousStepStatus { get; set; }

        private static int Index { get; set; }

        static StepProxy()
        {
            TestSteps = new List<TestStep>();
            PreviousStepStatus = Status.Undefined;
            Index = 1;
        }

        public StepProxy(Type classToProxy, IWebDriver driver) : base(classToProxy)
        {
            if(driver != null)
            {
                Decorated = Activator.CreateInstance(classToProxy, driver);
            }
            else
            {
                Decorated = Activator.CreateInstance(classToProxy);
            }
        }

        public static void InitiateTestExecution(Status caseStatus)
        {
            TestCaseStatus = caseStatus;
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg", "The message containing invocation information cannot be null");
            }

            IMethodCallMessage methodCall = msg as IMethodCallMessage;
            MethodInfo decoratedMethod = methodCall.MethodBase as MethodInfo;

            var reTryStepTime = 0;
            var isReTryStep = false;
            ReturnMessage returnMessage;
            do
            {
                var testStep = CreateTestStep(methodCall, decoratedMethod, TestCaseStatus, Index);
                Logger.LogMsg(Severity.INFO, $"Test step: {testStep.Name} started at: {DateTime.Now}... retry step time: {reTryStepTime}");

                object invokeResult = null;
                try
                {
                    if (testStep.Status == Status.Undefined)
                    {
                        bool isStoppedOnFail = Config.StopOnFail;
                        if (!isReTryStep && isStoppedOnFail && (PreviousStepStatus == Status.Failed || PreviousStepStatus == Status.Skipped))
                        {
                            testStep.SetStatus(Status.Skipped);
                        }
                        else
                        {
                            invokeResult = decoratedMethod.Invoke(this.Decorated, methodCall.Args);
                            testStep.SetValue(invokeResult);
                            testStep.SetStatus(Status.Passed);
                        }
                    }

                    returnMessage = new ReturnMessage(invokeResult, null, 0, methodCall.LogicalCallContext, methodCall);
                }
                catch (Exception ex)
                {
                    var screenName = $"{testStep.ID}_{decoratedMethod.Name}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpeg";
                    testStep.SetScreenshotPath($@"{Config.ReportToScreenshot}\{screenName}");
                    Logger.LogMsg(Severity.ERROR, $"Test step Screenshot file name: {screenName}.");

                    var screenPath = $@"{Config.ScreenshotPath}\{screenName}";
                    FwUtil.Screenshot.CaptureScreen(screenPath);

                    Logger.LogMsg(Severity.ERROR, $"Step exception: {ex.GetLastInnerException().Message}. StackTrace: {ex.GetLastInnerException().StackTrace}");
                    testStep.SetError($"Step exception: {ex.GetLastInnerException().Message}. StackTrace: {ex.GetLastInnerException().StackTrace}");

                    testStep.SetStatus(Status.Failed);
                    returnMessage = new ReturnMessage(invokeResult, null, 0, methodCall.LogicalCallContext, methodCall);
                    // Remark: do not return exception from a step method to test method so that we can continue next step !!!
                    // returnMessage = new ReturnMessage(ex, methodCall);
                }

                testStep.Finish();
                TestSteps.Add(testStep);

                PreviousStepStatus = testStep.Status;
                isReTryStep = (reTryStepTime < Config.RetryStep && (PreviousStepStatus == Status.Failed));

                Logger.LogMsg(Severity.INFO, $"Test step: {testStep.Name} => Status is: {testStep.Status}...");
                Logger.LogMsg(Severity.INFO, $"Test step: {testStep.Name} completed at: {DateTime.Now}... retry step: {reTryStepTime}");

                reTryStepTime++;
                Index++;
            } while (isReTryStep);

            return returnMessage;
        }

        private TestStep CreateTestStep(IMethodCallMessage methodCall, MethodInfo testMethod, Status caseStatus, int index)
        {
            var testStep = new TestStep();
            var args = methodCall.Args;
            var paramStr = string.Empty;
            var paramInfos = testMethod.GetParameters();
            if (paramInfos.Any())
            {
                if (methodCall.ArgCount == paramInfos.Length)
                {
                    for (int i = 0; i < paramInfos.Length; i++)
                    {
                        if (args[i].GetType() == typeof(List<string>))
                        {
                            paramStr = $"{paramStr}, {paramInfos[i].ParameterType.Name} {paramInfos[i].Name} = '{string.Join(", ", args[i] as List<string>)}'";
                        }
                        else
                        {
                            paramStr = $"{paramStr}, {paramInfos[i].ParameterType.Name} {paramInfos[i].Name} = '{args[i]}'";
                        }
                    }
                }
                else
                {
                    foreach (var paramInfo in paramInfos)
                    {
                        paramStr = $"{paramStr}, {paramInfo.ParameterType.Name} {paramInfo.Name}";
                    }
                }

                paramStr = $"({paramStr.Trim(',').Trim()})";
            }

            testStep.SetName($"{testMethod.Name} {paramStr}".Trim());
            testStep.SetIndex(index);

            var stepAttr = testMethod.TestStep();
            testStep.SetID(stepAttr.ID);
            testStep.SetComment(stepAttr.Comment);

            if (caseStatus != Status.Undefined)
            {
                stepAttr.Status = caseStatus;
            }

            testStep.SetStatus(stepAttr.Status);

            return testStep;
        }

        public static void CompleteTestExecution()
        {
            TestSteps = new List<TestStep>();
            PreviousStepStatus = Status.Undefined;
            TestCaseStatus = Status.Undefined;
            Index = 1;
        }
    }
}
