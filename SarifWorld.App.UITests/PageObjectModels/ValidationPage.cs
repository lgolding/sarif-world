using OpenQA.Selenium;

namespace SarifWorld.App.PageObjectModels
{
    internal class ValidationPage : PageObjectModelBase
    {
        internal ValidationPage(IWebDriver driver) : base(driver, "validation") { }
    }
}
