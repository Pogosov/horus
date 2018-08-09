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

namespace horus.fw.Runner
{
    public class StepFactory
    {
        public static void InitStep(object step)
        {
            if (step == null)
            {
                throw new ArgumentNullException("step", "step cannot be null!");
            }

            InitMembers(step, null, typeof(PageAttribute));
        }

        public static void InitStep(IWebDriver driver, object step)
        {
            if (step == null)
            {
                throw new ArgumentNullException("step", "step cannot be null!");
            }

            if (driver == null)
            {
                throw new ArgumentNullException("driver", "driver cannot be null");
            }

            InitMembers(step, driver, typeof(PageAttribute));
        }

        private static void InitMembers(object step, IWebDriver driver, Type attributeType)
        {
            var type = step.GetType();
            var members = step.GetMembers(attributeType);
            foreach (var member in members)
            {
                object decorated = PageWrapper(member, driver);
                if (decorated == null)
                {
                    continue;
                }

                var field = member as FieldInfo;
                var property = member as PropertyInfo;
                if (field != null)
                {
                    field.SetValue(step, decorated);
                }
                else if (property != null)
                {
                    property.SetValue(step, decorated, null);
                }
            }
        }

        private static object PageWrapper(MemberInfo member, IWebDriver driver)
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

            var pageProxy = new PageProxy(targetType, driver);
            return pageProxy.GetTransparentProxy();
        }
    }
}
