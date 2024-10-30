using System.ComponentModel;

namespace ChicksGold.Test.Domain.Enums;

/// <summary>
/// Enum representing the possible actions that can be performed on the buckets.
/// </summary>
public enum BucketAction
{
    /// <summary>
    /// Action to fill bucket X.
    /// </summary>
    [Description("Fill bucket X")]
    FillBucketX,

    /// <summary>
    /// Action to fill bucket Y.
    /// </summary>
    [Description("Fill bucket Y")]
    FillBucketY,

    /// <summary>
    /// Action to empty bucket X.
    /// </summary>
    [Description("Empty bucket X")]
    EmptyBucketX,

    /// <summary>
    /// Action to empty bucket Y.
    /// </summary>
    [Description("Empty bucket Y")]
    EmptyBucketY,

    /// <summary>
    /// Action to transfer contents from bucket X to bucket Y.
    /// </summary>
    [Description("Transfer from bucket X to Y")]
    TransferFromXToY,

    /// <summary>
    /// Action to transfer contents from bucket Y to bucket X.
    /// </summary>
    [Description("Transfer from bucket Y to X")]
    TransferFromYToX
}