using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.FwUtil
{
    public static class Constant
    {
        public const string StartTestingInfoMessage = "======== Start Testing ========";

        public const string StopTestingInfoMessage = "======== Stop Testing ========";

        public const BindingFlags PublicInstanceOptions = BindingFlags.Instance | BindingFlags.Public;

        public const BindingFlags NonPublicInstanceOptions = BindingFlags.Instance | BindingFlags.NonPublic;
    }
}
