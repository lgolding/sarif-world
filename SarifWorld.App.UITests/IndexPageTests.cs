using OpenQA.Selenium;
using SarifWorld.App.PageObjectModels;
using SarifWorld.App.Pages;
using SarifWorld.TestUtilities;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace SarifWorld.App
{
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class IndexPageTests : PageTestBase<Index>
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_ShouldHaveCorrectTitleAndUrl()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanRefresh()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            Driver.Navigate().Refresh();
            indexPage.EnsurePageLoaded();

            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanNavigateToValidationPage()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            ValidationPage validationPage = indexPage.ClickValidationNavLink();
            validationPage.EnsurePageLoaded();

            var stringResources = new ResourceStrings(typeof(Pages.Validation));
            string expectedTitle = stringResources["PageTitle"];

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
