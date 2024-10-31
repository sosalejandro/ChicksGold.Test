using ChicksGold.Test.Domain.Enums;
using ChicksGold.Test.Domain;
using ChicksGold.Test.Service.Abstractions;
using Microsoft.Extensions.Logging;

/// <summary>
/// Service to solve the bucket challenge problem.
/// </summary>
public class BucketChallengeService : IBucketChallengeService
{
    private readonly ILogger<BucketChallengeService> _logger;

    public BucketChallengeService(ILogger<BucketChallengeService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Solves the bucket challenge problem for the given bucket capacities and target volume.
    /// </summary>
    /// <param name="bucket1Capacity">The capacity of the first bucket.</param>
    /// <param name="bucket2Capacity">The capacity of the second bucket.</param>
    /// <param name="targetVolume">The target volume to achieve.</param>
    /// <returns>A solution containing the steps to achieve the target volume.</returns>
    /// <exception cref="NoAvailableSolutionException">Thrown when no solution is available for the given input.</exception>
    public Solution Solve(int bucket1Capacity, int bucket2Capacity, int targetVolume)
    {
        _logger.LogInformation("Starting to solve the bucket challenge with bucket1Capacity: {Bucket1Capacity}, bucket2Capacity: {Bucket2Capacity}, targetVolume: {TargetVolume}", bucket1Capacity, bucket2Capacity, targetVolume);

        var visited = new HashSet<State>();
        var queue = new Queue<(State state, List<Step> steps)>();
        queue.Enqueue((new State(0, 0), new List<Step>()));

        while (queue.Count > 0)
        {
            var (currentState, steps) = queue.Dequeue();
            _logger.LogDebug("Processing state: {CurrentState}", currentState);

            if (currentState.Bucket1Volume == targetVolume || currentState.Bucket2Volume == targetVolume)
            {
                steps.Last().SetStatusSolved();
                _logger.LogInformation("Solution found with steps count: {Steps}", steps.Count);
                return new Solution(steps);
            }

            if (visited.Contains(currentState))
            {
                _logger.LogDebug("State {CurrentState} already visited", currentState);
                continue;
            }

            visited.Add(currentState);

            // Generate all possible next states
            var nextStates = GetNextStates(currentState, bucket1Capacity, bucket2Capacity);
            foreach (var (nextState, action) in nextStates)
            {
                if (!visited.Contains(nextState))
                {
                    var newSteps = new List<Step>(steps)
                    {
                        new(steps.Count + 1, nextState.Bucket1Volume, nextState.Bucket2Volume, action)
                    };
                    queue.Enqueue((nextState, newSteps));
                    _logger.LogDebug("Enqueued new state: {NextState} with action: {Action}", nextState, action);
                }
            }
        }

        _logger.LogWarning("No solution available for the given input: bucket1Capacity: {Bucket1Capacity}, bucket2Capacity: {Bucket2Capacity}, targetVolume: {TargetVolume}", bucket1Capacity, bucket2Capacity, targetVolume);
        throw new NoAvailableSolutionException("No solution available for the given input.");
    }

    /// <summary>
    /// Generates all possible next states from the current state.
    /// </summary>
    /// <param name="currentState">The current state of the buckets.</param>
    /// <param name="bucket1Capacity">The capacity of the first bucket.</param>
    /// <param name="bucket2Capacity">The capacity of the second bucket.</param>
    /// <returns>A list collection of possible next states and the actions to achieve them.</returns>
    private static List<(State state, BucketAction action)> GetNextStates(State currentState, int bucket1Capacity, int bucket2Capacity)
    {
        var nextStates = new List<(State, BucketAction)>
        {
            (new State(bucket1Capacity, currentState.Bucket2Volume), BucketAction.FillBucketX),
            (new State(currentState.Bucket1Volume, bucket2Capacity), BucketAction.FillBucketY),
            (new State(0, currentState.Bucket2Volume), BucketAction.EmptyBucketX),
            (new State(currentState.Bucket1Volume, 0), BucketAction.EmptyBucketY)
        };

        int pourToBucket2 = Math.Min(currentState.Bucket1Volume, bucket2Capacity - currentState.Bucket2Volume);
        nextStates.Add((new State(currentState.Bucket1Volume - pourToBucket2, currentState.Bucket2Volume + pourToBucket2), BucketAction.TransferFromXToY));

        int pourToBucket1 = Math.Min(currentState.Bucket2Volume, bucket1Capacity - currentState.Bucket1Volume);
        nextStates.Add((new State(currentState.Bucket1Volume + pourToBucket1, currentState.Bucket2Volume - pourToBucket1), BucketAction.TransferFromYToX));

        return nextStates;
    }
}