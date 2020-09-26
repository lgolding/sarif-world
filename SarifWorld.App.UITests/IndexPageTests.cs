using FluentAssertions;
using OpenQA.Selenium;
using SarifWorld.App.PageObjectModels;
using SarifWorld.TestUtilities;
using SeleniumExtras.WaitHelpers;
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

            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            indexPage.Title.Should().Be(WebPageTitle);
            indexPage.Url.Should().Be(ApplicationUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanRefresh()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Navigate().Refresh();

            indexPage.Title.Should().Be(WebPageTitle);
            indexPage.Url.Should().Be(ApplicationUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }


        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanNavigateToValidationPage()
        {
            var indexPage = new IndexPage(Driver);
            indexPage.NavigateTo();

            var stringResources = new ResourceStrings(typeof(Pages.Validation));
            string expectedTitle = stringResources["PageTitle"];
            string expectedUri = GetPageUri(RelativePageUris.Validation);

            IWebElement validationNavLink = Driver.FindElement(By.CssSelector($"[data-nav-target='{RelativePageUris.Validation}']"));
            validationNavLink.Click();

            indexPage.Title.Should().Be(WebPageTitle);
            indexPage.Url.Should().Be(expectedUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
