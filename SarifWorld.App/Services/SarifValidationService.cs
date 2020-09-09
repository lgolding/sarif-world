// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Multitool;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SarifWorld.App.Models;
using SarifWorld.App.Pages;

namespace SarifWorld.App.Services
{
    public class SarifValidationService : ISarifValidationService
    {
        private const string AppDataDirectory = "App_Data";
        private const string SchemaFileName = "sarif-2.1.0-rtm.5.json";
        private const string ValidationFileNameMarker = "-validation";

        private readonly string s_schemaFilePath = Path.Combine(AppDataDirectory, SchemaFileName);

        private readonly IFileSystem fileSystem;
        private readonly IStringLocalizer localizer;

        public SarifValidationService(IFileSystem fileSystem, IStringLocalizer<Validation> localizer)
        {
            this.fileSystem = fileSystem;
            this.localizer = localizer;
        }

        public ValidationResult ValidateFile(string fileName, string fileContents)
        {
            var validationResult = new ValidationResult();

            if (IsSarifFile(fileName))
            {
                (string inputFilePath, string outputFilePath) = MakeTempFilePaths(fileName);
                this.fileSystem.WriteAllText(inputFilePath, fileContents);

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
                    validationResult.ErrorMessage = ex.Message;
                }
                finally
                {
                    DeleteTempFile(inputFilePath);
                    DeleteTempFile(outputFilePath);
                }
            }
            else
            {
                validationResult.ErrorMessage = this.localizer.GetString("ErrorNotASarifFile", fileName);
            }

            return validationResult;
        }

        private static bool IsSarifFile(string fileName)
        {
            fileName = fileName.ToLowerInvariant();
            return fileName.EndsWith(SarifConstants.SarifFileExtension) || fileName.EndsWith(SarifConstants.SarifFileExtension + ".json");
        }

        private static (string, string) MakeTempFilePaths(string fileName)
        {
            string tempDirectory = Path.GetTempPath();
            string bareFileName = Path.GetFileNameWithoutExtension(fileName);
            string guid = Guid.NewGuid().ToString("D");
            string extension = Path.GetExtension(fileName);

            string inputFilePath = Path.Combine(tempDirectory, $"{bareFileName}.{guid}{extension}");
            string outputFilePath = Path.Combine(tempDirectory, $"{bareFileName}.{guid}{ValidationFileNameMarker}{extension}");

            return (inputFilePath, outputFilePath);
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
    }
}
