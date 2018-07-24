using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.Base.Model
{
    public class TestStep : TestBase
    {
        public string ScreenshotPath { get; private set; }

        public TestStep() : base()
        {
        }

        public void SetScreenshotPath(string screenPath)
        {
            ScreenshotPath = screenPath;
        }
    }
}
