using ChicksGold.Test.Service.Abstractions;

namespace ChicksGold.Test.Service;

/// <summary>
/// Manages the services for the application.
/// </summary>
/// <param name="bucketChallengeService">The bucket challenge service instance.</param>
public class ServiceManager(IBucketChallengeService bucketChallengeService) : IServiceManager
{
    private readonly Lazy<IBucketChallengeService> _bucketChallengeService = new(bucketChallengeService);

    /// <summary>
    /// Gets the bucket challenge service.
    /// </summary>
    public IBucketChallengeService BucketChallengeService => _bucketChallengeService.Value;
}