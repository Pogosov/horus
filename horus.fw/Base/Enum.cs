using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.Base
{
    /// <summary>
    /// Status of test case
    /// </summary>
    public enum Status
    {
        Undefined = 0,

        Passed = 1,

        Failed = 2,

        Skipped = 3,

        Pending = 4
    }

    /// <summary>
    /// Log Severity
    /// </summary>
    public enum Severity
    {
        INFO = 0,

        DEBUG = 1,

        FATAL = 2,

        ERROR = 3,

        WARN = 4
    }

    /// <summary>
    /// Runner mode
    /// </summary>
    public enum RunnerMode
    {
        Undefined = 0,

        All = 1,

        Xml = 2
    }

    /// <summary>
    /// Supported browsers
    /// </summary>
    public enum Browser
    {
        Undefined = 0,

        Firefox = 1,

        Chrome = 2,

        Ie = 3,

        Edge = 4,

        Safari = 5
    }

    /// <summary>
    /// Default WindowMaximize Values
    /// </summary>
    public enum WindowMaximize
    {
        Default = 0,

        Maximize = 1,

        NoMaximize = 2
    }
}
