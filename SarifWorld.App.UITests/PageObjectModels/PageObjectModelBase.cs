using System;
using System.IO;
using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace SarifWorld.App.PageObjectModels
{
    internal abstract class PageObjectModelBase
    {
        internal PageObjectModelBase(IWebDriver driver)
        {
            Driver = driver;
            PageUri = GetPageUri();
        }

        public abstract string RelativeUri { get; }

        protected IWebDriver Driver { get; }

        public string PageUri { get; }

        public string Title => Driver.Title;

        public string ActualUri => Driver.Url;

        public void NavigateTo() => Driver.Navigate().GoToUrl(PageUri);

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
