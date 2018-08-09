using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.api.Source.Step;
using horus.fw.Base.Attributes;
using horus.fw.Runner;

namespace horus.fw.api.Source.Suite
{
    [TestSuite]
    public class Reqres_Api_Suite
    {
        [Steps]
        Reqres_Api_Step ReqresApiStep { get; set; }

        public Reqres_Api_Suite()
        {
            SuiteFactory.InitSuite(this);
        }

        [TestCase]
        public void Create_User()
        {
            ReqresApiStep.Create_User();
        }

        [TestCase]
        public void Update_User()
        {
            ReqresApiStep.Update_User();
        }

        [TestCase]
        public void Get_User()
        {
            ReqresApiStep.Get_User();
        }

        [TestCase]
        public void Delete_User()
        {
            ReqresApiStep.Delete_User();
        }
    }
}
