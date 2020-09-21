using System;
using System.IO;
using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SarifWorld.App
{
    public abstract class PageTestBase : IDisposable
    {
        // This is a single page application; the page title always stays the same
        // (until the day comes when we write script to update it when we navigate).
        protected const string WebPageTitle = "SARIF";

        // The maximum amount of time to wait for UI updates sent through SignalR
        // to complete.
        private static readonly TimeSpan DomUpdateTimeout = TimeSpan.FromSeconds(5);

        private bool disposed;

        protected PageTestBase()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.LogPath = "chromedriver.log";
            service.EnableVerboseLogging = true;

            ApplicationUri = GetApplicationUri();

            Driver = new ChromeDriver(service);
            Driver.Navigate().GoToUrl(ApplicationUri);
        }

        protected IWebDriver Driver { get; private set; }

        protected string ApplicationUri { get; }

        protected IWebElement WaitFor(By locator, Func<IWebElement, bool> condition)
        {
            var wait = new WebDriverWait(Driver, DomUpdateTimeout);
            return wait.Until(driver => ElementSatisfiesCondition(driver, locator, condition));
        }

        private static IWebElement ElementSatisfiesCondition(IWebDriver driver, By locator, Func<IWebElement, bool> condition)
        {
            IWebElement element = driver.FindElement(locator);
            try
            {
                if (!condition(element))
                {
                    element = null;
                }
            }
            catch (StaleElementReferenceException)
            {
                element = null;
            }

            return element;
        }

        #region IDispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Driver != null)
                    {
                        Driver.Dispose();
                        Driver = null;
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        // Constructs the HTTPS application URI from information in launchSettings.json.
        private string GetApplicationUri()
        {
            const string LaunchSettingsPath = @"Properties\launchSettings.json";
            const string IisExpressSettingsPointerString = "/iisSettings/iisExpress";
            const string ApplicationUriScheme = "https:";

            string applicationUrlPointerString = $"{IisExpressSettingsPointerString}/applicationUrl";
            string sslPortPointerString = $"{IisExpressSettingsPointerString}/sslPort";

            string documentText = File.ReadAllText(LaunchSettingsPath);
            JToken documentToken = JToken.Parse(documentText);

            var applicationUriPointer = new JsonPointer(applicationUrlPointerString);
            JToken applicationUriToken = applicationUriPointer.Evaluate(documentToken);
            var applicationUriString = (string)applicationUriToken;

            var sslPortPointer = new JsonPointer(sslPortPointerString);
            JToken sslPortToken = sslPortPointer.Evaluate(documentToken);
            var sslPort = (int)sslPortToken;

            var builder = new UriBuilder(applicationUriString);
            builder.Scheme = ApplicationUriScheme;
            builder.Port = sslPort;

            return builder.ToString();
        }
    }
}
