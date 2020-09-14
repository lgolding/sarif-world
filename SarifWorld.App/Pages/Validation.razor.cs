using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.Sarif;
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

        public Multiselect RulesSelector { get; set; }

        public void ValidateDroppedFile(DroppedFile droppedFile)
        {
            try
            {
                ValidationResult validationResult = SarifValidationService.ValidateFile(droppedFile.Name, droppedFile.Text);
                if (string.IsNullOrEmpty(validationResult.ErrorMessage))
                {
                    Alert.ShowMessage($"Number of results: {validationResult.ValidationLog.Runs[0].Results.Count}");
                    List<string> ruleIds = GetRuleIds(validationResult.ValidationLog);
                    RulesSelector.SetOptions(ruleIds);
                }
                else
                {
                    Alert.ShowError(validationResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Alert.ShowError(ex.Message);
            }
        }

        List<string> GetRuleIds(SarifLog log)
        {
            // The validation service _always_ returns a log file with a single, well-formed
            // run. There's no need to null-check anything...s
            IList<ReportingDescriptor> rules = log.Runs[0].Tool.Driver.Rules;

            // ... but if there were no violations, there won't be any rules.
            if (rules?.Any() == true)
            {
                return rules
                    .Select(r => r.Id)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();
            }

            return new List<string>();
        }
    }
}
