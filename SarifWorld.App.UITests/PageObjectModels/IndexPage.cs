using OpenQA.Selenium;

namespace SarifWorld.App.PageObjectModels
{
    internal class IndexPage : PageObjectModelBase
    {
        internal IndexPage(IWebDriver driver) : base(driver) { }

        public override string RelativeUri => string.Empty;

        public ValidationPage ClickValidationNavLink()
        {
            var validationPage = new ValidationPage(Driver);
            IWebElement validationNavLink = Driver.FindElement(By.CssSelector($"[data-nav-target='{validationPage.RelativeUri}']"));
            validationNavLink.Click();
            return validationPage;
        }
    }
}
