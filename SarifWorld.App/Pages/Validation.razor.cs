using System;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SarifWorld.App.Models;
using SarifWorld.App.Services;
using SarifWorld.ComponentsLibrary;

namespace SarifWorld.App.Pages
{
    public partial class Validation
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public ISarifValidationService SarifValidationService { get; set; }

        [Inject]
        public IFileSystem FileSystem { get; set; }

        private Alert alert;

        protected override void OnInitialized()
        {
            this.alert = new Alert(JSRuntime);
        }

        public void ValidateDroppedFile(DroppedFile droppedFile)
        {
            if (IsSarifFile(droppedFile.Name))
            {
                ValidateSarifFile(droppedFile);
            }
            else
            {
                alert.Show(Localizer.GetString("ErrorNotASarifFile", droppedFile.Name));
            }
        }

        private void ValidateSarifFile(DroppedFile droppedFile)
        {
            string tempFilePath = MakeTempFilePath(droppedFile.Name);
            try
            {
                FileSystem.WriteAllText(tempFilePath, droppedFile.Text);
                ValidationResult validationResult = SarifValidationService.ValidateFile(tempFilePath);
            }
            finally
            {
                DeleteTempFile(tempFilePath);
            }
        }

        private void DeleteTempFile(string tempFilePath)
        {
            // The SARIF SDK's IFileSystem doesn't implement File.Delete, so call the real API.
            // But in tests, the file won't actually exist, so ignore exceptions.
            // https://github.com/microsoft/sarif-sdk/issues/2033
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
            }
        }

        private string MakeTempFilePath(string fileName)
        {
            string tempDirectory = Path.GetTempPath();
            string bareFileName = Path.GetFileNameWithoutExtension(fileName);
            string guid = Guid.NewGuid().ToString("D");
            string extension = Path.GetExtension(fileName);

            return Path.Combine(tempDirectory, $"{bareFileName}.{guid}{extension}");
        }

        internal static bool IsSarifFile(string path)
        {
            path = path.ToLowerInvariant();
            return path.EndsWith(SarifConstants.SarifFileExtension) || path.EndsWith(SarifConstants.SarifFileExtension + ".json");
        }
    }
}
