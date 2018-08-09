using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.FwUtil
{
    public class Config
    {
        // Test Runner Mode
        public static bool StopOnFail
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["StopOnFail"]); }
        }

        public static int RerunCase
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RerunCase"]); }
        }

        public static int RetryStep
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RetryStep"]); }
        }

        public static int RetryAction
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RetryAction"]); }
        }

        // Log & Report Configurations
        public static string LogPath
        {
            get { return ConfigurationManager.AppSettings["LogPath"]; }
        }

        public static string ReportPath
        {
            get { return ConfigurationManager.AppSettings["ReportPath"]; }
        }

        public static string ScreenshotPath
        {
            get { return ConfigurationManager.AppSettings["ScreenshotPath"]; }
        }

        public static string ReportToScreenshot
        {
            get { return ConfigurationManager.AppSettings["ReportToScreenshot"]; }
        }

        public static string TestExecutionXmlPath
        {
            get { return ConfigurationManager.AppSettings["TestExecutionXmlPath"]; }
        }

        // Web Configurations
        public static string DefaultUrl
        {
            get { return ConfigurationManager.AppSettings["DefaultUrl"]; }
        }

        public static string DefaultUser
        {
            get { return ConfigurationManager.AppSettings["DefaultUser"]; }
        }

        public static string DefaultPassword
        {
            get { return ConfigurationManager.AppSettings["DefaultPassword"]; }
        }

        public static string DefaultBrowser
        {
            get { return ConfigurationManager.AppSettings["DefaultBrowser"]; }
        }

        public static bool IsWindowMaximize
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["IsWindowMaximize"]); }
        }

        public static bool ChromeHeadlessMode
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["ChromeHeadlessMode"]); }
        }

        public static string DatabaseName
        {
            get { return ConfigurationManager.AppSettings["DatabaseName"]; }
        }

        // Selenium Configurations
        public static int DefaultDriverWaitTimeout
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DefaultDriverWaitTimeout"]); }
        }

        public static int DefaultImplicitWaitTimeout
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DefaultImplicitWaitTimeout"]); }
        }

        public static int DefaultPageLoadTimeout
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DefaultPageLoadTimeout"]); }
        }

        // Driver & Browser Binary
        public static string FirefoxDriverPath
        {
            get { return ConfigurationManager.AppSettings["FirefoxDriverPath"]; }
        }

        public static string FirefoxBrowserPath
        {
            get { return ConfigurationManager.AppSettings["FirefoxBrowserPath"]; }
        }

        public static string IeDriverPath
        {
            get { return ConfigurationManager.AppSettings["IeDriverPath"]; }
        }

        public static string IeBrowserPath
        {
            get { return ConfigurationManager.AppSettings["IeBrowserPath"]; }
        }

        public static string ChromeDriverPath
        {
            get { return ConfigurationManager.AppSettings["ChromeDriverPath"]; }
        }

        public static string ChromeBrowserPath
        {
            get { return ConfigurationManager.AppSettings["ChromeBrowserPath"]; }
        }

        // Common Paths
        public static string SolutionPath
        {
            // CurrentDirectory = Debug => Parent = bin => Parent = Project => Parent = Solution
            get { return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName; }
        }

        public static string ProjectPath
        {
            // CurrentDirectory = Debug => Parent = bin => Parent = Project
            get { return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName; }
        }

        // DB connection
        public static string SqlConnection
        {
            get { return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString; }
        }
    }
}
