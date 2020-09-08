// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SarifWorld.Models;

namespace SarifWorld.Services
{
    public interface ISarifValidationService
    {
        ValidationResult ValidateFile(string inputFilePath);
    }
}