using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.FwUtil;

namespace horus.fw.Base.Model
{
    public class TestSuite : TestBase
    {
        public string Platform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }

        public int RerunCase
        {
            get
            {
                return Config.RerunCase;
            }
        }

        public int TotalTests
        {
            get
            {
                return Tests.Count;
            }
        }

        public int TotalPassed
        {
            get
            {
                return Tests.Count(x => x.Status == Status.Passed);
            }
        }

        public int TotalFailed
        {
            get
            {
                return Tests.Count(x => x.Status == Status.Failed);
            }
        }

        public int TotalSkipped
        {
            get
            {
                return Tests.Count(x => x.Status == Status.Skipped);
            }
        }

        public int TotalPending
        {
            get
            {
                return Tests.Count(x => x.Status == Status.Pending);
            }
        }

        public int TotalExecutedTests
        {
            get
            {
                return TotalTests - TotalPending - TotalSkipped;
            }
        }

        public int PassedRate
        {
            get
            {
                return TotalPassed / TotalTests;
            }
        }

        public int FailedRate
        {
            get
            {
                return TotalFailed / TotalTests;
            }
        }

        public int SkippedRate
        {
            get
            {
                return TotalSkipped / TotalTests;
            }
        }

        public int PendingRate
        {
            get
            {
                return TotalPending / TotalTests;
            }
        }

        public List<TestCase> Tests { get; set; }

        public bool ReportAllMethods { get; set; }

        public TestSuite() : base()
        {
            Tests = new List<TestCase>();
        }

        public void AddTestCases(List<TestCase> testCases)
        {
            Tests.AddRange(testCases);
        }

        public void SetReportAllMethods(bool isReported)
        {
            ReportAllMethods = isReported;
        }
    }
}
