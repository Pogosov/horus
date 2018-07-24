using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.Utility
{
    public class HorusConfig
    {
        public static bool SkipOnFail
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["SkipOnFail"]); }
        }

        public static int RerunCase
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RerunCase"]); }
        }

        public static int RetryStep
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RetryStep"]); }
        }

        public static int RetrySubStep
        {
            get { return int.Parse(ConfigurationManager.AppSettings["RetrySubStep"]); }
        }

        public static string LogPath
        {
            get { return Path.Combine(SolutionPath, ConfigurationManager.AppSettings["LogPath"]); }
        }

        public static string ReportLocalPath
        {
            get { return Path.Combine(SolutionPath, ConfigurationManager.AppSettings["ReportLocalPath"]); }
        }

        public static string ScreenshotsPath
        {
            get { return Path.Combine(SolutionPath, ConfigurationManager.AppSettings["ScreenshotsPath"]); }
        }

        public static string ReportApiUrl
        {
            get { return Path.Combine(SolutionPath, ConfigurationManager.AppSettings["ReportApiUrl"]); }
        }

        public static string SqlConnection
        {
            get { return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString; }
        }

        public static string SolutionPath
        {
            // GetCurrentDirectory = Debug/Release => Parent = bin => Parent = TestProject => Parent = Solution
            get { return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName; }
        }

        public static string DemoPath
        {
            // GetCurrentDirectory = Debug/Release => Parent = bin => Parent = TestProject
            get { return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName; }
        }
    }
}
