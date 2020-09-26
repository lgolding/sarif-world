using FluentAssertions;
using OpenQA.Selenium;
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
            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Title.Should().Be(WebPageTitle);
            Driver.Url.Should().Be(ApplicationUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanRefresh()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Navigate().Refresh();

            Driver.Title.Should().Be(WebPageTitle);
            Driver.Url.Should().Be(ApplicationUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }


        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanNavigateToValidationPage()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Validation));
            string expectedTitle = stringResources["PageTitle"];
            string expectedUri = GetPageUri(RelativePageUris.Validation);

            IWebElement validationNavLink = Driver.FindElement(By.CssSelector($"[data-nav-target='{RelativePageUris.Validation}']"));
            validationNavLink.Click();

            Driver.Title.Should().Be(WebPageTitle);
            Driver.Url.Should().Be(expectedUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
