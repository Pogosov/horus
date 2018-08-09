using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.Base.Model;
using log4net;

namespace horus.fw.FwUtil
{
    public class Logger
    {
        public static void CleanUpLog()
        {
            // Clean up log
            FwUtil.DeleteFilesAndSubDirectories(Path.Combine(Config.ProjectPath, "Log"));
        }

        public static void LogMsg(Severity severity, string msg)
        {
            ILog logger = LogManager.GetLogger("Horus");
            switch (severity)
            {
                case Severity.FATAL:
                    logger.Fatal(msg);
                    break;
                case Severity.ERROR:
                    logger.Error(msg);
                    break;
                case Severity.WARN:
                    logger.Warn(msg);
                    break;
                case Severity.INFO:
                    logger.Info(msg);
                    break;
                case Severity.DEBUG:
                    logger.Debug(msg);
                    break;
                default:
                    logger.Debug(msg);
                    break;
            }
        }

        public static void WriteLine(TestSuite suite)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Suite {suite.Index} => Test suite [{suite.Name}] executed.");
            Console.WriteLine($"Start time: {suite.StartTime}");
            Console.WriteLine($"Duration: {suite.Duration}");
            Console.WriteLine($"End time: {suite.EndTime}");
            if (suite.Status != Status.Undefined)
            {
                Console.WriteLine($"Status: {suite.Status}");
            }

            if (suite.Value != null)
            {
                Console.WriteLine($"Return value: {suite.Value}");
            }

            if (!string.IsNullOrWhiteSpace(suite.Error))
            {
                Console.WriteLine($"Error message: {suite.Error}");
            }

            Console.WriteLine();
            Console.ResetColor();

            foreach (var test in suite.Tests)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Test {test.Index} => Test case [{test.Name}] executed.");
                Console.WriteLine($"Start time: {test.StartTime}");
                Console.WriteLine($"Duration: {test.Duration}");
                Console.WriteLine($"End time: {test.EndTime}");
                if (test.Status != Status.Undefined)
                {
                    Console.WriteLine($"Status: {test.Status}");
                }

                if (test.Value != null)
                {
                    Console.WriteLine($"Return value: {test.Value}");
                }

                if (!string.IsNullOrWhiteSpace(test.Error))
                {
                    Console.WriteLine($"Error message: {test.Error}");
                }

                Console.WriteLine();
                Console.ResetColor();
            }
        }
    }
}
