using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SarifWorld.App
{
    public abstract class PageTestBase : IDisposable
    {
        // The maximum amount of time to wait for UI updates sent through SignalR
        // to complete.
        private static readonly TimeSpan DomUpdateTimeout = TimeSpan.FromSeconds(5);

        protected readonly WebDriverWait Wait;

        private bool disposed;

        protected PageTestBase()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.LogPath = "chromedriver.log";
            service.EnableVerboseLogging = true;

            Driver = new ChromeDriver(service);
            this.Wait = new WebDriverWait(Driver, DomUpdateTimeout);
        }

        protected IWebDriver Driver { get; private set; }

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
    }
}
