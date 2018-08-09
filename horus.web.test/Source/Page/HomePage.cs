using horus.fw.Assertion;
using horus.fw.Base;
using horus.fw.FwUtil;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.web.test.Source.Page
{
    public class HomePage : MarshalByRefObject
    {
        public HomePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "button.close-reveal-modal")]
        private IList<IWebElement> CloseRevelModalBtns { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='signin-head']/a")]
        private IWebElement SignInLnk { get; set; }

        [FindsBy(How = How.Id, Using = "email")]
        private IWebElement EmailTxt { get; set; }

        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement PasswordTxt { get; set; }

        [FindsBy(How = How.Id, Using = "loginBtn")]
        private IWebElement LoginBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='uppercase'][contains(@href, 'smartwatch')]")]
        private IWebElement SmartWatchLnk { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[contains(@src, 'misfit-command.jpg')]")]
        private IWebElement MisfitCmdLnk { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[text()='Jet with Black Sport Strap']")]
        private IWebElement JetBlackSport { get; set; }

        [FindsBy(How = How.Id, Using = "add-to-cart")]
        private IWebElement AddToCartBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = "img.icon-cart-dark")]
        private IWebElement CartImg { get; set; }

        [FindsBy(How = How.CssSelector, Using = "li.cart-product-item.bfx-product")]
        private IList<IWebElement> CartItems { get; set; }

        public void NavigateToHomePage()
        {
            Selenium.NavigateToUrl(Config.DefaultUrl);
            Selenium.MaximizeWindow();
        }

        public void CloseRevealModal()
        {
            foreach (var item in CloseRevelModalBtns)
            {
                if (item.IsElementDisplayed())
                {
                    item.Click();
                }
            }
        }

        public void LoginToWebApp(string user, string pass)
        {
            Selenium.WaitUntilElementIsClickable(SignInLnk);
            SignInLnk.Click();
            EmailTxt.Clear();
            EmailTxt.SendKeys(user);
            PasswordTxt.Clear();
            PasswordTxt.SendKeys(pass);
            LoginBtn.Click();
        }

        public void GoToMisfitCommand()
        {
            Selenium.WaitUntilElementIsClickable(SmartWatchLnk);
            SmartWatchLnk.HoverMouse();
            MisfitCmdLnk.Click();
        }

        public void AddAJetBlackSportToCart()
        {
            JetBlackSport.Click();
            Selenium.WaitUntilElementIsClickable(AddToCartBtn);
            AddToCartBtn.Click();
        }

        public void OpenCart()
        {
            CartImg.Click();
        }

        public void VerifyJetBlackSportAdded()
        {
            var jetBlackSport = CartItems.Where(e => e.Text.Contains("Black Sport Strap")).FirstOrDefault();
            Assert.IsNotNull(jetBlackSport);

            var price = jetBlackSport.FindElement(By.CssSelector("span.price"));
            Assert.Equals(price.Text, "$149.99");
        }
    }
}
