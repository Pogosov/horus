using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using horus.fw.Base.Attributes;
using horus.fw.FwUtil;
using horus.fw.Runner;
using horus.web.test.Source.Step;
using OpenQA.Selenium;

namespace horus.web.test.Source.Suite
{
    [TestSuite]
    public class Web_Demo_Suite
    {
        [Managed]
        private IWebDriver Driver { get; set; }

        [Steps]
        private HomeStep HomeStep { get; set; }

        public Web_Demo_Suite()
        {
            SuiteFactory.InitSuite(this);
        }

        [BeforeSuite]
        public void Navigate_To_Misfit_Home_Page()
        {
            HomeStep.Navigate_To_Misfit_Home_Page();
        }

        [TestCase]
        public void Add_Jet_Black_Sport_Item_To_Cart()
        {
            HomeStep.Login_To_Misfit_Web_Application(Config.DefaultUser, Config.DefaultPassword);
            HomeStep.Add_Jet_Black_Sport_Item_To_Cart();
            HomeStep.Verify_The_Cart_Contains_Jet_Black_Sport_Item();
        }

        [AfterSuite]
        public void Clean_Up_Driver()
        {
            Selenium.Quit();
        }
    }
}
