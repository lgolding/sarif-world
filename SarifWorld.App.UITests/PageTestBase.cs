using System;
using System.IO;
using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SarifWorld.App
{
    public abstract class PageTestBase : IDisposable
    {
        // This is a single page application; the page title always stays the same
        // (until the day comes when we write script to update it when we navigate).
        protected const string WebPageTitle = "SARIF";

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

        // Perform an action once SignalR has finished updating the DOM. There's now way to know
        // when the DOM is done updating, so just catch StaleElementReferenceException and retry.
        // CONSIDER: Limit number or time duration of retries.
        protected void WaitForSignalR(Action assertion)
        {
            bool isStale;
            do
            {
                try
                {
                    isStale = false;
                    assertion();
                }
                catch (StaleElementReferenceException)
                {
                    isStale = true;
                }
            } while (isStale);
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
