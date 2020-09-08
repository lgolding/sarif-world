// Copyright (c) Laurence J.Golding.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace SarifWorld.Models
{
    public class ValidationOptionsModel
    {
        [Required(ErrorMessage = "Input file is required.")]
        public string InputFilePath { get; set; }
    }
}
