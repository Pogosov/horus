using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horus.fw.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;

namespace horus.fw.FwUtil
{
    public class Selenium
    {
        public static IWebDriver Driver { get; set; }

        public static WebDriverWait DriverWait { get; set; }

        public static int DriverWaitTimeOut { get; set; }


        public static Browser Browser { get; set; }

        public static string DriverPath { get; set; }

        public static string ProfilePath { get; set; }

        public static string ProxyString { get; set; }

        public static bool ChromeHeadlessMode { get; set; }


        public static IWebDriver GetDriver(Browser browser, int timeout = 0)
        {
            return GetDriver(browser, string.Empty, string.Empty, string.Empty, false, timeout);
        }

        public static IWebDriver GetDriver(Browser browser, bool chromeHeadlessMode, int timeout = 0)
        {
            return GetDriver(browser, string.Empty, string.Empty, string.Empty, chromeHeadlessMode, timeout);
        }

        public static IWebDriver GetDriver(Browser browser, string driverPath, bool chromeHeadlessMode, int timeout = 0)
        {
            return GetDriver(browser, driverPath, string.Empty, string.Empty, chromeHeadlessMode, timeout);
        }

        public static IWebDriver GetDriver(Browser browser, string driverPath, string profilePath, bool chromeHeadlessMode, int timeout = 0)
        {
            return GetDriver(browser, driverPath, profilePath, string.Empty, chromeHeadlessMode, timeout);
        }

        public static IWebDriver GetDriver(Browser browser, string driverPath, string profilePath, string proxyString, bool chromeHeadlessMode, int timeout = 0)
        {
            if (timeout == 0)
            {
                timeout = Config.DefaultDriverWaitTimeout;
            }

            switch (browser)
            {
                case Browser.Firefox:
                    Driver = GetFirefoxDriver(driverPath, profilePath, profilePath);
                    break;
                case Browser.Chrome:
                    Driver = GetChromeDriver(driverPath, profilePath, chromeHeadlessMode, proxyString);
                    break;
                case Browser.Ie:
                    Driver = GetIeDriver(driverPath, proxyString);
                    break;
                default:
                    throw new NotSupportedException($"The browser {browser.ToString()}  is not supported !!!");
            }

            DriverWait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(timeout));
            SetImplicitWait(0);
            SetPageLoadTimeOut(0);

            Browser = browser;
            DriverPath = driverPath;
            ProfilePath = profilePath;
            ProxyString = proxyString;
            ChromeHeadlessMode = chromeHeadlessMode;
            DriverWaitTimeOut = timeout;

            return Driver;
        }


        private static IWebDriver GetFirefoxDriver(string driverPath, string profilePath, string proxyString)
        {
            Environment.SetEnvironmentVariable("MOZ_CRASHREPORTER", "0");
            Environment.SetEnvironmentVariable("MOZ_CRASHREPORTER_DISABLE", "1");
            Environment.SetEnvironmentVariable("MOZ_CRASHREPORTER_NO_REPORT", "1");
            Environment.SetEnvironmentVariable("XRE_NO_WINDOWS_CRASH_DIALOG", "1");
            Environment.SetEnvironmentVariable("GNOME_DISABLE_CRASH_DIALOG", "1");

            FirefoxOptions fOptions = new FirefoxOptions();
            fOptions.SetPreference("browser.startup.page", 0);
            fOptions.SetPreference("browser.startup.homepage", "about:blank");
            fOptions.SetPreference("browser.startup.homepage_override.mstone", "ignore");
            fOptions.SetPreference(CapabilityType.UnexpectedAlertBehavior, "ignore");
            fOptions.BrowserExecutableLocation = Config.FirefoxBrowserPath;

            if (!string.IsNullOrWhiteSpace(profilePath))
            {
                FirefoxProfile profile = new FirefoxProfile(profilePath)
                {
                    AcceptUntrustedCertificates = true,
                    AssumeUntrustedCertificateIssuer = false,
                    DeleteAfterUse = true
                };

                fOptions.Profile = profile;
            }

            if (!string.IsNullOrWhiteSpace(proxyString))
            {
                Proxy proxy = new Proxy
                {
                    HttpProxy = proxyString,
                    SslProxy = proxyString,
                    FtpProxy = proxyString
                };

                fOptions.Proxy = proxy;
            }

            if (string.IsNullOrWhiteSpace(driverPath))
            {
                driverPath = Config.FirefoxDriverPath;
            }

            return new FirefoxDriver(driverPath, fOptions);
        }

        private static IWebDriver GetChromeDriver(string driverPath, string profilePath, bool chromeHeadlessMode, string proxyString)
        {
            ChromeOptions cOptions = new ChromeOptions
            {
                AcceptInsecureCertificates = true
            };

            if (!string.IsNullOrWhiteSpace(profilePath))
            {
                cOptions.AddArguments($"--user-data-dir={profilePath}");
            }

            if (!chromeHeadlessMode)
            {
                chromeHeadlessMode = Config.ChromeHeadlessMode;
            }

            if (chromeHeadlessMode)
            {
                cOptions.AddArguments("--headless", "--disable-gpu");
            }

            if (!string.IsNullOrWhiteSpace(proxyString))
            {
                Proxy proxy = new Proxy
                {
                    HttpProxy = proxyString,
                    SslProxy = proxyString,
                    FtpProxy = proxyString
                };

                cOptions.Proxy = proxy;
            }

            if (string.IsNullOrWhiteSpace(driverPath))
            {
                driverPath = Config.ChromeDriverPath;
            }

            return new ChromeDriver(driverPath, cOptions);
        }

        private static IWebDriver GetIeDriver(string driverPath, string proxyString)
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore,
                IgnoreZoomLevel = true,
            };

            if (!string.IsNullOrWhiteSpace(proxyString))
            {
                Proxy proxy = new Proxy
                {
                    HttpProxy = proxyString,
                    SslProxy = proxyString,
                    FtpProxy = proxyString
                };

                ieOptions.Proxy = proxy;
            }

            if (string.IsNullOrWhiteSpace(driverPath))
            {
                driverPath = Config.IeDriverPath;
            }

            return new InternetExplorerDriver(driverPath, ieOptions);
        }


        public static void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public static void MaximizeWindow()
        {
            Driver.Manage().Window.Maximize();
        }

        public static void SwitchToFrame(IWebElement frameElement)
        {
            try
            {
                Driver.SwitchTo().Frame(frameElement);
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
            }
        }

        public static void SwitchToFrame(string frameName)
        {
            try
            {
                Driver.SwitchTo().Frame(frameName);
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
            }
        }

        public static void SwitchToFrame(int frameIndex)
        {
            try
            {
                Driver.SwitchTo().Frame(frameIndex);
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
            }
        }

        public static void SwitchToDefaultContent()
        {
            try
            {
                Driver.SwitchTo().DefaultContent();
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
            }
        }


        public static bool WaitUntilElementIsDisappeared(IWebElement element)
        {
            return DriverWait.Until(ExpectedConditions.StalenessOf(element));
        }

        public static bool WaitUntilInvisibilityOfElementLocated(By findElementBy)
        {
            return DriverWait.Until(ExpectedConditions.InvisibilityOfElementLocated(findElementBy));
        }

        public static IWebElement WaitUntilElementExists(By findElementBy)
        {
            return DriverWait.Until(ExpectedConditions.ElementExists(findElementBy));
        }

        public static IWebElement WaitUntilElementIsVisible(By findElementBy)
        {
            try
            {
                return DriverWait.Until(ExpectedConditions.ElementIsVisible(findElementBy));
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
                return null;
            }
        }

        public static IWebElement WaitUntilElementIsClickable(By findElementBy)
        {
            try
            {
                return DriverWait.Until(ExpectedConditions.ElementToBeClickable(findElementBy));
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
                return null;
            }
        }

        public static IWebElement WaitUntilElementIsClickable(IWebElement element)
        {
            try
            {
                return DriverWait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (UnhandledAlertException)
            {
                AcceptAlert();
                return null;
            }
        }

        public static IWebDriver WaitUntilFrameToBeAvailableAndSwitchToIt(string frameName)
        {
            return DriverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameName));
        }

        public static IList<IWebElement> WaitUntilAllElementsAreVisible(By findElementBy)
        {
            return DriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(findElementBy));
        }

        public static void AcceptAlert()
        {
            DriverWait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = Driver.SwitchTo().Alert();
            Logger.LogMsg(Severity.WARN, string.Format("There is an alert with text >>> {0}", alert.Text));
            alert.Accept();
        }

        public static void DismissAlert()
        {
            IAlert alert = Driver.SwitchTo().Alert();
            Logger.LogMsg(Severity.WARN, string.Format("There is an alert with text >>> {0}", alert.Text));
            alert.Dismiss();
        }


        public static void SetImplicitWait(int timeout = 0)
        {
            if (timeout == 0)
            {
                timeout = Config.DefaultImplicitWaitTimeout;
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(timeout);
        }

        public static void SetPageLoadTimeOut(int timeout = 0)
        {
            if (timeout == 0)
            {
                timeout = Config.DefaultPageLoadTimeout;
            }

            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(timeout);
        }

        public static void WaitForPageLoad()
        {
            var javaScriptExecutor = Driver as IJavaScriptExecutor;
            Func<IWebDriver, bool> readyCondition = webDriver => javaScriptExecutor.ExecuteScript("return document.readyState").Equals("complete");
            DriverWait.Until(readyCondition);
        }

        public static void Quit()
        {
            Driver.Quit();
        }
    }
}
