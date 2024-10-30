using ChicksGold.Test.Domain.Enums;
using ChicksGold.Test.Domain.Extensions;

namespace ChicksGold.Test.Domain;

/// <summary>
/// Represents a step in the bucket challenge solution.
/// </summary>
/// <param name="stepCount">The step count.</param>
/// <param name="bucketX">The volume of bucket X.</param>
/// <param name="bucketY">The volume of bucket Y.</param>
/// <param name="action">The action performed in this step.</param>
/// <param name="status">The status of the step.</param>
public class Step(int stepCount, int bucketX, int bucketY, BucketAction action, string status = null)
{
    /// <summary>
    /// Gets the step count.
    /// </summary>
    public int StepCount { get; } = stepCount;

    /// <summary>
    /// Gets the volume of bucket X.
    /// </summary>
    public int BucketX { get; } = bucketX;

    /// <summary>
    /// Gets the volume of bucket Y.
    /// </summary>
    public int BucketY { get; } = bucketY;

    /// <summary>
    /// Gets the action performed in this step.
    /// </summary>
    public string Action { get; } = action.GetDescription();

    /// <summary>
    /// Gets or sets the status of the step.
    /// </summary>
    public string Status { get; set; } = status;

    /// <summary>
    /// Sets the status of the step to "Solved".
    /// </summary>
    public void SetStatusSolved()
    {
        Status = "Solved";
    }
}
