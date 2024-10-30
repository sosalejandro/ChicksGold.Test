using System.ComponentModel;

namespace ChicksGold.Test.Domain.Extensions;

/// <summary>
/// Extension methods for enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description attribute of an enum value.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description of the enum value, or the enum value as a string if no description is found.</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}