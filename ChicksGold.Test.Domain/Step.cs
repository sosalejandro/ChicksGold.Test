using ChicksGold.Test.Domain.Enums;
using ChicksGold.Test.Domain.Extensions;
using System.Text.Json.Serialization;

namespace ChicksGold.Test.Domain;

/// <summary>
/// Represents a step in the bucket challenge solution.
/// </summary>
public class Step
{
    /// <summary>
    /// Gets the step count.
    /// </summary>
    public int StepCount { get; }

    /// <summary>
    /// Gets the volume of bucket X.
    /// </summary>
    public int BucketX { get; }

    /// <summary>
    /// Gets the volume of bucket Y.
    /// </summary>
    public int BucketY { get; }

    /// <summary>
    /// Gets the action performed in this step.
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// Gets or sets the status of the step.
    /// </summary>
    public string Status { get; set; }

    /// <param name="stepCount">The step count.</param>
    /// <param name="bucketX">The volume of bucket X.</param>
    /// <param name="bucketY">The volume of bucket Y.</param>
    /// <param name="action">The action performed in this step.</param>
    /// <param name="status">The status of the step.</param>
    [JsonConstructor]
    public Step(int stepCount, int bucketX, int bucketY, BucketAction action, string status = null)
    {
        StepCount = stepCount;
        BucketX = bucketX;
        BucketY = bucketY;
        Action = action.GetDescription();
        Status = status;
    }

    /// <summary>
    /// Sets the status of the step to "Solved".
    /// </summary>
    public void SetStatusSolved()
    {
        Status = "Solved";
    }
}
