using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.Base
{
    public class Base
    {
        /// <summary>
        /// Status of test cases / test steps
        /// </summary>
        public enum Status
        {
            Undefined = 0,

            Passed = 1,

            Failed = 2,

            Skipped = 3,

            Pending = 4
        }
    }
}
