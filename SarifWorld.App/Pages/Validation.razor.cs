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
                ValidationResult validationResult = SarifValidationService.ValidateFile(droppedFile.Name, droppedFile.Text);
            }
            else
            {
                alert.Show(Localizer.GetString("ErrorNotASarifFile", droppedFile.Name));
            }
        }

        internal static bool IsSarifFile(string fileName)
        {
            fileName = fileName.ToLowerInvariant();
            return fileName.EndsWith(SarifConstants.SarifFileExtension) || fileName.EndsWith(SarifConstants.SarifFileExtension + ".json");
        }
    }
}
