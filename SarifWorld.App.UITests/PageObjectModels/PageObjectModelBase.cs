using OpenQA.Selenium;

namespace SarifWorld.App.PageObjectModels
{
    internal abstract class PageObjectModelBase
    {
        internal PageObjectModelBase(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebDriver Driver { get; }

        public string Title => Driver.Title;

        public string Url => Driver.Url;
    }
}
