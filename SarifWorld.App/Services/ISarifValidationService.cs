// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SarifWorld.App.Models;

namespace SarifWorld.App.Services
{
    public interface ISarifValidationService
    {
        ValidationResult ValidateFile(string fileName, string fileContents);
    }
}