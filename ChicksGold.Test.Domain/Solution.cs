namespace ChicksGold.Test.Domain;

/// <summary>
/// Represents the solution to the bucket challenge problem.
/// </summary>
/// <param name="steps">The list of steps to achieve the target volume.</param>
public class Solution(List<Step> steps)
{
    /// <summary>
    /// Gets the list of steps to achieve the target volume.
    /// </summary>
    public List<Step> Steps { get; set; } = steps;
}
