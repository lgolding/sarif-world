// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Multitool;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SarifWorld.App.Models;
using SarifWorld.ComponentsLibrary;

namespace SarifWorld.App.Services
{
    public class SarifValidationService : ISarifValidationService
    {
        private const string AppDataDirectory = "App_Data";
        private const string SchemaFileName = "sarif-2.1.0-rtm.5.json";
        private const string ValidationFileNameMarker = "validation";

        private readonly string s_schemaFilePath = Path.Combine(AppDataDirectory, SchemaFileName);

        private readonly IFileSystem fileSystem;
        private readonly Alert alert;

        public SarifValidationService(IFileSystem fileSystem, IJSRuntime jsRuntime)
        {
            this.fileSystem = fileSystem;
            alert = new Alert(jsRuntime);
        }

        public ValidationResult ValidateFile(string inputFilePath)
        {
            string outputFilePath = MakeOutputFilePath(inputFilePath);

            var validateOptions = new ValidateOptions
            {
                TargetFileSpecifiers = new List<string>
                {
                    inputFilePath
                },
                SchemaFilePath = s_schemaFilePath,
                OutputFilePath = outputFilePath,
                Force = true,
                PrettyPrint = true,
                Verbose = true,
                RichReturnCode = true,
            };

            var validateCommand = new ValidateCommand(this.fileSystem);
            var validationResult = new ValidationResult();
            try
            {
                validationResult.ExitCode = validateCommand.Run(validateOptions);

                validationResult.InputFileContents = this.fileSystem.ReadAllText(inputFilePath);
                validationResult.ResultFileContents = fileSystem.ReadAllText(outputFilePath);
                validationResult.ValidationLog = JsonConvert.DeserializeObject<SarifLog>(validationResult.ResultFileContents);

                // Provide each result with a back-pointer to the run that contains it. This is necessary so that
                // the result can stand on its own in the ResultsView.
                validationResult.ValidationLog.Runs[0].SetRunOnResults();
            }
            catch (Exception ex)
            {
                // TODO: Use a bootstrap alert bar.
                alert.Show($"Exception: {ex.Message}");
                validationResult.ErrorMessage = ex.Message;
            }

            return validationResult;
        }

        internal static string MakeOutputFilePath(string inputFilePath)
        {
            string guid = Guid.NewGuid().ToString("D");
            string fileName = Path.GetFileNameWithoutExtension(inputFilePath);

            return Path.Combine(AppDataDirectory, $"{fileName}.{guid}.{ValidationFileNameMarker}{SarifConstants.SarifFileExtension}");
        }
    }
}
