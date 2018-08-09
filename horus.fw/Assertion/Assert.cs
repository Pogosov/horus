using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.FwUtil;

namespace horus.fw.Assertion
{
    public class Assert
    {
        public static bool IsTrue(bool expression, string errorMsg = "", string successMsg = "")
        {
            if (!expression)
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"Expected expression value is 'True' but actual value is 'False'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"Expected expression value is 'True' but actual value is 'FALSE'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"Expected expression value & actual value are 'True'" : successMsg);
            }

            return true;
        }

        public static bool IsNull(object obj, string errorMsg = "", string successMsg = "")
        {
            if (obj != null)
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"Expected expression value is 'null' but actual value is '#null'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"Expected expression value is 'null' but actual value is '#null'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"Expected expression value & actual value are 'null'" : successMsg);
            }

            return true;
        }

        public static bool IsFalse(bool expression, string errorMsg = "", string successMsg = "")
        {
            if (expression)
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"Expected expression value is 'False' but actual value is 'True'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"Expected expression value is 'False' but actual value is 'True'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"Expected expression value & actual value are 'FALSE'" : successMsg);
            }

            return true;
        }

        public static bool IsNotNull(object obj, string errorMsg = "", string successMsg = "")
        {
            if (obj == null)
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"Expected expression value is '#null' but actual value is 'null'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"Expected expression value is '#null' but actual value is 'null'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"Expected expression value & actual value are '#null'" : successMsg);
            }

            return true;
        }

        public static bool Equals(object expected, object actual, string errorMsg = "", string successMsg = "")
        {
            if (!expected.Equals(actual))
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"The actual value '{actual}' does not equal expected value '{expected}'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"The actual value '{actual}' does not equal expected value '{expected}'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"The actual value '{actual}' equals expected value '{expected}'" : successMsg);
            }

            return true;
        }

        public static bool DoesNotEqual(object expected, object actual, string errorMsg = "", string successMsg = "")
        {
            if (expected.Equals(actual))
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"The actual value '{actual}' equals expected value '{expected}'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"The actual value '{actual}' equals expected value '{expected}'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"The actual value '{actual}' does not equal expected value '{expected}'" : successMsg);
            }

            return true;
        }

        public static bool Contains(dynamic parent, dynamic child, string errorMsg = "", string successMsg = "")
        {
            if (!parent.Contains(child))
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"The parent value '{parent}' does not contain value '{child}'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"The parent value '{parent}' does not contain value '{child}'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"The parent value '{parent}' contains value '{child}'" : successMsg);
            }

            return true;
        }

        public static bool DoesNotContain(dynamic parent, dynamic child, string errorMsg = "", string successMsg = "")
        {
            if (parent.Contains(child))
            {
                Logger.LogMsg(Severity.ERROR, string.IsNullOrWhiteSpace(errorMsg) ? $"The parent value '{parent}' contains value '{child}'" : errorMsg);
                throw string.IsNullOrWhiteSpace(errorMsg) ? new TestException($"The parent value '{parent}' contains value '{child}'") : new TestException(errorMsg);
            }
            else
            {
                Logger.LogMsg(Severity.INFO, string.IsNullOrWhiteSpace(successMsg) ? $"The parent value '{parent}' does not contain value '{child}'" : successMsg);
            }

            return true;
        }
    }
}
