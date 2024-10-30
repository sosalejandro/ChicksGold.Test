namespace ChicksGold.Test.Service.Abstractions;

/// <summary>
/// Interface for the service manager.
/// </summary>
public interface IServiceManager
{
    /// <summary>
    /// Gets the bucket challenge service.
    /// </summary>
    IBucketChallengeService BucketChallengeService { get; }
}