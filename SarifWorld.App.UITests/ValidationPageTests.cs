﻿using OpenQA.Selenium;
using SarifWorld.App.PageObjectModels;
using SarifWorld.App.Pages;
using SarifWorld.TestUtilities;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace SarifWorld.App
{
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class ValidationPageTests : PageTestBase<Validation>
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void ValidationPage_ShouldHaveCorrectTitleAndUrl()
        {
            var validationPage = new ValidationPage(Driver);
            validationPage.NavigateTo();

            string expectedTitle = StringResources["PageTitle"];

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
