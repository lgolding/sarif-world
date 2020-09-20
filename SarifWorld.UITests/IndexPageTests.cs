using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SarifWorld.App.Pages;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.UITests
{
    public class IndexPageTests
    {
        [Fact]
        [Trait("Category", "Smoke")]
        public void IndexPage_ShouldBeDisplayedWhenAppStarts()
        {
            var stringResources = new ResourceStrings(typeof(Index));
            string expectedTitle = stringResources["PageTitle"];

            // Note: If you run these tests with VS running as Admin,
            // Chrome crashes (displays a frowny face) and the test
            // hangs and never finishes. I searched and found several
            // people who observed this, and the thing that most say
            // is to run the tests non-Admin. I don't know the root cause.
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://localhost:44392/");

            driver.Title.Should().Be(expectedTitle);
        }
    }
}
