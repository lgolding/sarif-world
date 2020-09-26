using OpenQA.Selenium;

namespace SarifWorld.App.PageObjectModels
{
    internal class IndexPage : PageObjectModelBase
    {
        internal IndexPage(IWebDriver driver) : base(driver) { }

        public override string RelativeUri => string.Empty;
    }
}
