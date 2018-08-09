using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base.Attributes;
using horus.fw.Runner;
using horus.web.test.Source.Page;
using OpenQA.Selenium;

namespace horus.web.test.Source.Step
{
    public class HomeStep : MarshalByRefObject
    {
        [Page]
        private HomePage HomePage { get; set; }

        public HomeStep(IWebDriver driver)
        {
            StepFactory.InitStep(driver, this);
        }

        [TestStep]
        public void Navigate_To_Misfit_Home_Page()
        {
            HomePage.NavigateToHomePage();
            HomePage.CloseRevealModal();
        }

        [TestStep]
        public void Login_To_Misfit_Web_Application(string username, string password)
        {
            HomePage.LoginToWebApp(username, password);
        }

        [TestStep]
        public void Add_Jet_Black_Sport_Item_To_Cart()
        {
            HomePage.GoToMisfitCommand();
            HomePage.AddAJetBlackSportToCart();
        }

        [TestStep]
        public void Verify_The_Cart_Contains_Jet_Black_Sport_Item()
        {
            HomePage.OpenCart();
            HomePage.VerifyJetBlackSportAdded();
        }
    }
}
