using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.App
{
    // Note: If you run these tests as Admin (at least on Windows 10), Chrome crashes with "Aw,
    // snap!". Multiple posters report this and suggest running the tests as non-Admin, but
    // I don't know the root cause.
    [Trait("Category", "UITest")]
    public class IndexPageTests : IDisposable
    {
        private IWebDriver driver;
        private bool disposed;

        public IndexPageTests()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.LogPath = "chromedriver.log";
            service.EnableVerboseLogging = true;

            this.driver = new ChromeDriver(service);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void IndexPage_ShouldBeDisplayedWhenAppStarts()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            this.driver.Navigate().GoToUrl("http://localhost:61981/");

            this.driver.Title.Should().Be(expectedTitle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.driver != null)
                    {
                        this.driver.Dispose();
                        this.driver = null;
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
