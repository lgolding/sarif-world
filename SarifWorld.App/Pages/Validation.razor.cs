using Microsoft.AspNetCore.Components;
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

        private Alert alert;

        protected override void OnInitialized()
        {
            this.alert = new Alert(JSRuntime);
        }

        public void ValidateDroppedFile(DroppedFile droppedFile)
        {
            ValidationResult validationResult = SarifValidationService.ValidateFile(droppedFile.Name, droppedFile.Text);
            if (string.IsNullOrEmpty(validationResult.ErrorMessage))
            {
                // Temporary UI.
                alert.Show($"Number of results: {validationResult.ValidationLog.Runs[0].Results.Count}");
            }
            else
            {
                // Temporary UI.
                alert.Show(validationResult.ErrorMessage);
            }
        }
    }
}
