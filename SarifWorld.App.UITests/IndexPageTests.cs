using SarifWorld.App.PageObjectModels;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.App
{
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class IndexPageTests : PageTestBase
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_ShouldHaveCorrectTitleAndUrl()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            indexPage.WaitForExpectedPageTitle();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanRefresh()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            Driver.Navigate().Refresh();

            indexPage.EnsurePageLoaded();
            indexPage.WaitForExpectedPageTitle();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanNavigateToValidationPage()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();
            indexPage.WaitForExpectedPageTitle();

            ValidationPage validationPage = indexPage.ClickValidationNavLink();

            validationPage.EnsurePageLoaded();
            validationPage.WaitForExpectedPageTitle();
        }
    }
}
