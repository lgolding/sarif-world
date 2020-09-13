using Microsoft.Extensions.Localization;

namespace SarifWorld.App
{
    /// <summary>
    /// Provides a mockable localization interface that includes the commonly used extension
    /// methods on IStringLocalizer&lt;T>.
    /// </summary>
    /// <remarks>
    /// Without this interface or some equivalent facility, you can't use Moq to mock calls to
    /// IStringLocalizer&lt;T>.GetString, because Moq can't mock extension methods.
    /// </remarks>
    public interface ILocalizationWrapper<T>
    {
        /// <summary>
        /// Returns the actual localizer object that this object wraps.
        /// </summary>
        IStringLocalizer<T> Localizer { get; }

        /// <summary>
        /// Gets the string resource with the given name and formatted with the supplied arguments.
        /// </summary>
        /// <param name="name">
        /// The name of the string resource.
        /// </param>
        /// <param name="arguments">
        /// The values to format the string with.
        /// </param>
        /// <returns>
        /// The formatted string resource.
        /// </returns>
        /// <remarks>
        /// Unlike the Blazor-supplied extension method, this method returns a simple string rather
        /// than a <see cref="LocalizedString"/>.
        /// </remarks>
        string GetString(string name, params object[] arguments);
    }
}
