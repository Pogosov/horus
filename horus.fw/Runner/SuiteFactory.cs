using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base.Attributes;
using horus.fw.Base;
using horus.fw.Base.Model;
using OpenQA.Selenium;
using horus.fw.FwUtil;

namespace horus.fw.Runner
{
    public class SuiteFactory
    {
        public static void InitSuite(object suite)
        {
            if (suite == null)
            {
                throw new ArgumentNullException("suite", "suite cannot be null!");
            }

            var driver = InitiateDriver(suite);
            InitMembers(suite, driver, typeof(StepsAttribute));
        }

        public static void InitSuite(IWebDriver driver, object suite)
        {
            if (suite == null)
            {
                throw new ArgumentNullException("suite", "suite cannot be null!");
            }

            if (driver == null)
            {
                throw new ArgumentNullException("driver", "driver cannot be null");
            }

            InitMembers(suite, driver, typeof(StepsAttribute));
        }

        private static void InitMembers(object suite, IWebDriver driver, Type attributeType)
        {
            var type = suite.GetType();
            var members = suite.GetMembers(attributeType);
            foreach (var member in members)
            {
                object decorated = StepWrapper(member, driver);
                if (decorated == null)
                {
                    continue;
                }

                var field = member as FieldInfo;
                var property = member as PropertyInfo;
                if (field != null)
                {
                    field.SetValue(suite, decorated);
                }
                else if (property != null)
                {
                    property.SetValue(suite, decorated, null);
                }
            }
        }

        private static object StepWrapper(MemberInfo member, IWebDriver driver)
        {
            var field = member as FieldInfo;
            var property = member as PropertyInfo;

            Type targetType = null;
            if (field != null)
            {
                targetType = field.FieldType;
            }

            bool hasPropertySet = false;
            if (property != null)
            {
                hasPropertySet = property.CanWrite;
                targetType = property.PropertyType;
            }

            if (field == null & (property == null || !hasPropertySet))
            {
                return null;
            }

            var stepProxy = new StepProxy(targetType, driver);
            return stepProxy.GetTransparentProxy();
        }

        private static IWebDriver InitiateDriver(object suite)
        {
            var type = suite.GetType();
            var members = suite.GetMembers(typeof(ManagedAttribute));
            if (members.Count == 1)
            {
                var field = members[0] as FieldInfo;
                var property = members[0] as PropertyInfo;

                var isIWebDriverMember = false;
                if (field != null)
                {
                    isIWebDriverMember = field.FieldType == typeof(IWebDriver);
                }
                else if (property != null)
                {
                    isIWebDriverMember = property.PropertyType == typeof(IWebDriver);
                }

                if (isIWebDriverMember)
                {
                    var attr = members[0].ManagedAttribute();
                    if (attr.Browser == Browser.Undefined)
                    {
                        attr.Browser = (Browser)Enum.Parse(typeof(Browser), Config.DefaultBrowser);
                    }

                    IWebDriver driver = Selenium.GetDriver(attr.Browser, attr.DriverPath, attr.ProfilePath, attr.ProxyString, attr.ChromeHeadlessMode, attr.Timeout);
                    if (!string.IsNullOrWhiteSpace(attr.Url))
                    {
                        if (attr.Url.Equals("Default"))
                        {
                            attr.Url = Config.DefaultUrl;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(attr.Url))
                    {
                        driver.Navigate().GoToUrl(attr.Url);
                    }

                    bool isMaximize;
                    switch (attr.WindowMaximize)
                    {
                        case WindowMaximize.Default:
                            isMaximize = Config.IsWindowMaximize;
                            break;
                        case WindowMaximize.Maximize:
                            isMaximize = true;
                            break;
                        case WindowMaximize.NoMaximize:
                            isMaximize = false;
                            break;
                        default:
                            isMaximize = false;
                            break;
                    }

                    if (isMaximize)
                    {
                        driver.Manage().Window.Maximize();
                    }

                    return driver;
                }
                else
                {
                    throw new ApplicationException($"Test suite name: {type.Name} has a driver member with [Managed] attribute. But it is not declared with IWebDriver type!");
                }
            }
            else if (members.Count > 1)
            {
                throw new ApplicationException($"Test suite name: {type.Name} has more than one driver field with [Managed] attribute.");
            }
            else
            {
                // throw new NotFoundException($"Test suite name: {type.Name} has no driver field with [Managed] attribute.");
                return null;
            }
        }
    }
}
