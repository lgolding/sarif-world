using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SarifWorld.App
{
    public abstract class PageTestBase : IDisposable
    {
        private bool disposed;

        protected PageTestBase()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.LogPath = "chromedriver.log";
            service.EnableVerboseLogging = true;

            Driver = new ChromeDriver(service);
            Driver.Navigate().GoToUrl(ApplicationUri);
        }

        protected IWebDriver Driver { get; private set; }

        protected string ApplicationUri { get; } = "https://localhost:44392/";

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
    }
}
