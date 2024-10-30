using ChicksGold.Test.Domain;

namespace ChicksGold.Test.Service.Abstractions;

/// <summary>
/// Interface for the bucket challenge service.
/// </summary>
public interface IBucketChallengeService
{
    /// <summary>
    /// Solves the bucket challenge problem for the given bucket capacities and target volume.
    /// </summary>
    /// <param name="bucket1Capacity">The capacity of the first bucket.</param>
    /// <param name="bucket2Capacity">The capacity of the second bucket.</param>
    /// <param name="targetVolume">The target volume to achieve.</param>
    /// <returns>A solution containing the steps to achieve the target volume.</returns>
    /// <exception cref="NoAvailableSolutionException">Thrown when no solution is available for the given input.</exception>
    Solution Solve(int bucket1Capacity, int bucket2Capacity, int targetVolume);
}