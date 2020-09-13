using Microsoft.Extensions.Localization;

namespace SarifWorld.App
{
    /// <summary>
    /// Implements the <see cref="ILocalizationWrapper{T}"/> interface, which allows extension
    /// methods such as <see cref="IStringLocalizer{T}.GetString"/> to be mocked.
    /// </summary>
    public class LocalizationWrapper<T> : ILocalizationWrapper<T>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LocalizationWrapper{T}"/> class which wraps
        /// the specified <see cref="IStringLocalizer{T}"/> instance.
        /// </summary>
        /// <param name="localizer">
        /// The actual localizer object that this object wraps.
        /// </param>
        public LocalizationWrapper(IStringLocalizer<T> localizer)
        {
            Localizer = localizer;
        }

        /// <inheritdoc/>
        public IStringLocalizer<T> Localizer { get; }

        /// <inheritdoc/>
        public string GetString(string name, params object[] arguments)
            => Localizer.GetString(name, arguments);
    }
}
