// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.Sarif;

namespace SarifWorld.App.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            InputFileContents = string.Empty;
            OutputFileContents = string.Empty;
            ValidationLog = null;
            ExitCode = 0;
            ErrorMessage = string.Empty;
        }

        public string InputFileContents { get; set; }

        public string OutputFileContents { get; set; }

        public SarifLog ValidationLog { get; set; }

        public int ExitCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
