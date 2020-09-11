using Microsoft.AspNetCore.Components;
using SarifWorld.App.Models;
using SarifWorld.App.Services;
using SarifWorld.ComponentsLibrary;

namespace SarifWorld.App.Pages
{
    public partial class Validation
    {
        [Inject]
        public ISarifValidationService SarifValidationService { get; set; }

        public Alert Alert { get; set; }

        public void ValidateDroppedFile(DroppedFile droppedFile)
        {
            ValidationResult validationResult = SarifValidationService.ValidateFile(droppedFile.Name, droppedFile.Text);
            if (string.IsNullOrEmpty(validationResult.ErrorMessage))
            {
                Alert.ShowMessage($"Number of results: {validationResult.ValidationLog.Runs[0].Results.Count}");
            }
            else
            {
                Alert.ShowError(validationResult.ErrorMessage);
            }
        }
    }
}
