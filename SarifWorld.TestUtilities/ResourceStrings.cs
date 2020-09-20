using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace SarifWorld.TestUtilities
{
    /// <summary>
    /// Provides access to the resource strings associated with a specified Blazor type and the
    /// specified language.
    /// </summary>
    public class ResourceStrings
    {
        private const string DefaultLanguage = "en";

        private IReadOnlyDictionary<string, string> resourceDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceType"/> class for the specified
        /// Blazor type and the specified language.
        /// </summary>
        /// <param name="ownerType">The Blazor type with which the resources are associated.</param>
        /// <param name="language">The desired resource language (default: "en").</param>
        public ResourceStrings(Type ownerType, string language = DefaultLanguage)
        {
            Assembly resourceAssembly = ownerType.Assembly.GetSatelliteAssembly(new CultureInfo(language));
            string resourceStreamName = $"{ownerType.FullName}.{language}.resources";
            Stream resourceStream = resourceAssembly.GetManifestResourceStream(resourceStreamName);
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ErrorCannotFindResources,
                            resourceStreamName,
                            resourceAssembly.FullName,
                            Path.GetDirectoryName(resourceAssembly.Location)),
                        nameof(language));
                }
            }

            this.resourceDictionary = GetResourceStrings(resourceStream);
        }

        /// <summary>
        /// Gets the resource string with the specified name.
        /// </summary>
        /// <param name="name">The name of the resource string to get.</param>
        /// <returns>The resource string specified by <paramref name="name"/>.</returns>
        public string this[string name] => this.resourceDictionary[name];

        private IReadOnlyDictionary<string, string> GetResourceStrings(Stream resourceStream)
        {
            var resourceDictionary = new Dictionary<string, string>();

            var resourceReader = new ResourceReader(resourceStream);
            IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = (DictionaryEntry)enumerator.Current;
                resourceDictionary[(string)entry.Key] = (string)entry.Value;
            }

            return new ReadOnlyDictionary<string, string>(resourceDictionary);
        }
    }
}
