using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base.Attributes;
using horus.fw.FwUtil;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace horus.fw.Base
{
    public static class Extension
    {
        public static void ClickJs(this IWebElement element)
        {
            ((IJavaScriptExecutor)Selenium.Driver).ExecuteScript("arguments[0].click();", element);
        }

        public static void RightClick(this IWebElement element)
        {
            Actions action = new Actions(Selenium.Driver);
            action.ContextClick(element).Build().Perform();
        }

        public static void ClickActions(this IWebElement element)
        {
            Actions action = new Actions(Selenium.Driver);
            action.MoveToElement(element).Click().Build().Perform();
        }

        public static void HoverMouse(this IWebElement element)
        {
            Actions action = new Actions(Selenium.Driver);
            action.MoveToElement(element).Build().Perform();
        }

        public static void ClickAndHold(this IWebElement element)
        {
            Actions action = new Actions(Selenium.Driver);
            action.ClickAndHold(element).Build().Perform();
        }

        public static bool IsElementDisplayed(this IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void ScrollElementIntoView(this IWebElement element)
        {
            ((IJavaScriptExecutor)Selenium.Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static bool PageSourceContains(this IWebDriver driver, string content, bool isLower = false)
        {
            string pageSource = driver.PageSource;
            if (isLower)
            {
                pageSource = pageSource.ToLower();
                content = content.ToLower();
            }

            return pageSource.Contains(content);
        }


        public static List<Type> GetTypes(this Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes().Where(t => t.IsClass && t.CustomAttributes.Any(att => att.AttributeType == attributeType)).ToList();
        }

        public static List<MemberInfo> GetMembers(this object suite, Type attributeType)
        {
            var type = suite.GetType();
            var members = new List<MemberInfo>();
            members.AddRange(type.GetFields(Constant.PublicInstanceOptions).Where(t => t.CustomAttributes.Any(a => a.AttributeType == attributeType)));
            members.AddRange(type.GetProperties(Constant.PublicInstanceOptions).Where(t => t.CustomAttributes.Any(a => a.AttributeType == attributeType)));
            while (type != null)
            {
                members.AddRange(type.GetFields(Constant.NonPublicInstanceOptions).Where(t => t.CustomAttributes.Any(a => a.AttributeType == attributeType)));
                members.AddRange(type.GetProperties(Constant.NonPublicInstanceOptions).Where(t => t.CustomAttributes.Any(a => a.AttributeType == attributeType)));
                type = type.BaseType;
            }

            return members;
        }

        public static List<MethodInfo> GetMethods(this Type classType, Type attributeType)
        {
            return classType.GetMethods().Where(t => t.CustomAttributes.Any(att => att.AttributeType == attributeType)).ToList();
        }

        public static List<MethodInfo> GetMethods(this List<MethodInfo> methods, Type attributeType, string testCaseID = "")
        {
            var returnedMethods = new List<MethodInfo>();
            if (attributeType == typeof(BeforeTestCaseAttribute))
            {
                returnedMethods = methods
                    .Where(m =>
                    //Methods with undefined RunningIds and undefined SkippedIds
                    (string.IsNullOrWhiteSpace(m.BeforeTest().RunningIds) && string.IsNullOrWhiteSpace(m.BeforeTest().SkippedIds)) ||
                    //Methods with undefined RunningIds and defined SkippedIds
                    (string.IsNullOrWhiteSpace(m.BeforeTest().RunningIds) && !string.IsNullOrWhiteSpace(m.BeforeTest().SkippedIds)
                    && !m.BeforeTest().SkippedIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)) ||
                    //Methods with defined RunningIds and undefined SkippedIds
                    (!string.IsNullOrWhiteSpace(m.BeforeTest().RunningIds) && string.IsNullOrWhiteSpace(m.BeforeTest().SkippedIds)
                    && m.BeforeTest().RunningIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)) ||
                    //Methods with defined RunningIds and defined SkippedIds
                    (!string.IsNullOrWhiteSpace(m.BeforeTest().RunningIds) && !string.IsNullOrWhiteSpace(m.BeforeTest().SkippedIds)
                    && m.BeforeTest().RunningIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)
                    && !m.BeforeTest().SkippedIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID))).ToList();
            }
            else
            {
                returnedMethods = methods
                    .Where(m =>
                    //Methods with undefined RunningIds and undefined SkippedIds
                    (string.IsNullOrWhiteSpace(m.AfterTest().RunningIds) && string.IsNullOrWhiteSpace(m.AfterTest().SkippedIds)) ||
                    //Methods with undefined RunningIds and defined SkippedIds
                    (string.IsNullOrWhiteSpace(m.AfterTest().RunningIds) && !string.IsNullOrWhiteSpace(m.AfterTest().SkippedIds)
                    && !m.AfterTest().SkippedIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)) ||
                    //Methods with defined RunningIds and undefined SkippedIds
                    (!string.IsNullOrWhiteSpace(m.AfterTest().RunningIds) && string.IsNullOrWhiteSpace(m.AfterTest().SkippedIds)
                    && m.AfterTest().RunningIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)) ||
                    //Methods with defined RunningIds and defined SkippedIds
                    (!string.IsNullOrWhiteSpace(m.AfterTest().RunningIds) && !string.IsNullOrWhiteSpace(m.AfterTest().SkippedIds)
                    && m.AfterTest().RunningIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID)
                    && !m.AfterTest().SkippedIds.Replace(" ", string.Empty).Split(new string[] { "|", ";", ",", ":" }, StringSplitOptions.RemoveEmptyEntries).Contains(testCaseID))).ToList();
            }

            return returnedMethods.Distinct().ToList();
        }

        public static ManagedAttribute ManagedAttribute(this MemberInfo methodInfo)
        {
            return ((ManagedAttribute)methodInfo.GetCustomAttribute(typeof(ManagedAttribute)));
        }

        public static BeforeSuiteAttribute BeforeSuite(this MethodInfo methodInfo)
        {
            return ((BeforeSuiteAttribute)methodInfo.GetCustomAttribute(typeof(BeforeSuiteAttribute)));
        }

        public static BeforeSuiteAttribute BeforeSuite(this Type type)
        {
            return ((BeforeSuiteAttribute)type.GetCustomAttribute(typeof(BeforeSuiteAttribute)));
        }

        public static AfterSuiteAttribute AfterSuite(this MethodInfo methodInfo)
        {
            return ((AfterSuiteAttribute)methodInfo.GetCustomAttribute(typeof(AfterSuiteAttribute)));
        }

        public static AfterSuiteAttribute AfterSuite(this Type type)
        {
            return ((AfterSuiteAttribute)type.GetCustomAttribute(typeof(AfterSuiteAttribute)));
        }

        public static BeforeTestCaseAttribute BeforeTest(this MethodInfo methodInfo)
        {
            return ((BeforeTestCaseAttribute)methodInfo.GetCustomAttribute(typeof(BeforeTestCaseAttribute)));
        }

        public static BeforeTestCaseAttribute BeforeTest(this Type type)
        {
            return ((BeforeTestCaseAttribute)type.GetCustomAttribute(typeof(BeforeTestCaseAttribute)));
        }

        public static AfterTestCaseAttribute AfterTest(this MethodInfo methodInfo)
        {
            return ((AfterTestCaseAttribute)methodInfo.GetCustomAttribute(typeof(AfterTestCaseAttribute)));
        }

        public static AfterTestCaseAttribute AfterTest(this Type type)
        {
            return ((AfterTestCaseAttribute)type.GetCustomAttribute(typeof(AfterTestCaseAttribute)));
        }

        public static TestSuiteAttribute TestSuite(this MethodInfo methodInfo)
        {
            return ((TestSuiteAttribute)methodInfo.GetCustomAttribute(typeof(TestSuiteAttribute)));
        }

        public static TestSuiteAttribute TestSuite(this Type type)
        {
            return ((TestSuiteAttribute)type.GetCustomAttribute(typeof(TestSuiteAttribute)));
        }

        public static TestCaseAttribute TestCase(this MethodInfo methodInfo)
        {
            return ((TestCaseAttribute)methodInfo.GetCustomAttribute(typeof(TestCaseAttribute)));
        }

        public static TestCaseAttribute TestCase(this Type type)
        {
            return ((TestCaseAttribute)type.GetCustomAttribute(typeof(TestCaseAttribute)));
        }

        public static TestStepAttribute TestStep(this MethodInfo methodInfo)
        {
            return ((TestStepAttribute)methodInfo.GetCustomAttribute(typeof(TestStepAttribute)));
        }

        public static TestStepAttribute TestStep(this Type type)
        {
            return ((TestStepAttribute)type.GetCustomAttribute(typeof(TestStepAttribute)));
        }

        public static Exception GetLastInnerException(this Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception;
            }
            else
            {
                return exception.InnerException.GetLastInnerException();
            }
        }
    }
}
