using System;
using System.Globalization;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SarifWorld.TestUtilities;
using SeleniumExtras.WaitHelpers;

namespace SarifWorld.App.PageObjectModels
{
    internal abstract class PageObjectModel<T> where T: ComponentBase
    {
        // This is a single page application; the page title always stays the same
        // (until the day comes when we write script to update it when we navigate).
        private const string WebPageTitle = "SARIF";

        // The maximum amount of time to wait for UI updates sent through SignalR
        // to complete.
        private static readonly TimeSpan DomUpdateTimeout = TimeSpan.FromSeconds(5);

        private readonly WebDriverWait wait;

        protected static ResourceStrings ResourceStrings => new ResourceStrings(typeof(T));

        internal PageObjectModel(IWebDriver driver)
        {
            Driver = driver;
            PageUri = GetPageUri();
            this.wait = new WebDriverWait(Driver, DomUpdateTimeout);
        }

        public abstract string RelativeUri { get; }

        public string PageUri { get; }

        public string Title => Driver.Title;

        public string ActualUri => Driver.Url;

        protected IWebDriver Driver { get; }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(PageUri);
            EnsurePageLoaded();
        }

        public void EnsurePageLoaded()
        {
            bool success = Driver.Url == PageUri && Driver.Title == WebPageTitle;
            success.Should().BeTrue(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.NavigationFailed,
                    PageUri,
                    Driver.Url,
                    WebPageTitle,
                    Driver.Title,
                    Driver.PageSource));
        }

        public void WaitForExpectedPageTitle()
        {
            string expectedTitle = ResourceStrings["PageTitle"];
            this.wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }

        private string GetPageUri()
        {
            string applicationUri = GetApplicationUri();
            var builder = new UriBuilder(applicationUri);
            builder.Path += RelativeUri;
            return builder.ToString();
        }

        // Constructs the HTTPS application URI from information in launchSettings.json.
        private string GetApplicationUri()
        {
            const string LaunchSettingsPath = @"Properties\launchSettings.json";
            const string IisExpressSettingsPointerString = "/iisSettings/iisExpress";
            const string ApplicationUriScheme = "https:";

            string applicationUrlPointerString = $"{IisExpressSettingsPointerString}/applicationUrl";
            string sslPortPointerString = $"{IisExpressSettingsPointerString}/sslPort";

            string documentText = File.ReadAllText(LaunchSettingsPath);
            JToken documentToken = JToken.Parse(documentText);

            var applicationUriPointer = new JsonPointer(applicationUrlPointerString);
            JToken applicationUriToken = applicationUriPointer.Evaluate(documentToken);
            var applicationUriString = (string)applicationUriToken;

            var sslPortPointer = new JsonPointer(sslPortPointerString);
            JToken sslPortToken = sslPortPointer.Evaluate(documentToken);
            var sslPort = (int)sslPortToken;

            var builder = new UriBuilder(applicationUriString)
            {
                Scheme = ApplicationUriScheme,
                Port = sslPort
            };

            return builder.ToString();
        }
    }
}
