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
    public class PageProxy : RealProxy
    {
        private readonly object Decorated;

        public PageProxy(Type classToProxy, IWebDriver driver) : base(classToProxy)
        {
            if (driver != null)
            {
                Decorated = Activator.CreateInstance(classToProxy, driver);
            }
            else
            {
                Decorated = Activator.CreateInstance(classToProxy);
            }
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg", "The message containing invocation information cannot be null");
            }

            IMethodCallMessage methodCall = msg as IMethodCallMessage;
            MethodInfo proxiedMethod = methodCall.MethodBase as MethodInfo;

            var reTryAction = 0;
            bool isReTryAction = false;
            var actionStatus = Status.Undefined;
            ReturnMessage returnMessage;
            do
            {
                var actionName = GetActionName(methodCall, proxiedMethod);
                Logger.LogMsg(Severity.INFO, $"Page action: {actionName} started at: {DateTime.Now}... retry action: {reTryAction}");
                try
                {
                    object invokeResult = proxiedMethod.Invoke(this.Decorated, methodCall.Args);
                    returnMessage = new ReturnMessage(invokeResult, null, 0, methodCall.LogicalCallContext, methodCall);
                    actionStatus = Status.Passed;
                }
                catch (Exception ex)
                {
                    Logger.LogMsg(Severity.ERROR, $"Action exception: {ex.GetLastInnerException().Message}. StackTrace: {ex.GetLastInnerException().StackTrace}");
                    returnMessage = new ReturnMessage(ex, methodCall);
                    actionStatus = Status.Failed;
                }

                isReTryAction = (reTryAction < Config.RetryAction && actionStatus == Status.Failed);
                Logger.LogMsg(Severity.INFO, $"Page action: {actionName} => Status is: {actionStatus}...");
                Logger.LogMsg(Severity.INFO, $"Page action: {actionName} completed at: {DateTime.Now}... retry action: {reTryAction}");

                reTryAction++;
            } while (isReTryAction);

            return returnMessage;
        }

        private string GetActionName(IMethodCallMessage methodCall, MethodInfo actionMethod)
        {
            var args = methodCall.Args;
            var paramStr = string.Empty;
            var paramInfos = actionMethod.GetParameters();
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

            return $"{actionMethod.Name} {paramStr}".Trim();
        }
    }
}
