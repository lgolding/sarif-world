using OpenQA.Selenium;
using SarifWorld.App.Pages;

namespace SarifWorld.App.PageObjectModels
{
    internal class ValidationPage : PageObjectModelBase<Validation>
    {
        internal ValidationPage(IWebDriver driver) : base(driver) { }

        public override string RelativeUri => "validation";
    }
}
