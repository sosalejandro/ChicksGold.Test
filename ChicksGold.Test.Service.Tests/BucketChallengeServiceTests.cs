using ChicksGold.Test.Domain;
using ChicksGold.Test.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace ChicksGold.Test.Service.Tests;

public class BucketChallengeServiceTests
{
    private readonly BucketChallengeService _service;
    private readonly Mock<ILogger<BucketChallengeService>> _mockLogger;

    public BucketChallengeServiceTests()
    {
        _mockLogger = new Mock<ILogger<BucketChallengeService>>();
        _service = new BucketChallengeService(_mockLogger.Object);
    }

    public static IEnumerable<object[]> GetTestCases()
    {
        yield return new object[]
        {
            2, 10, 4,
            new List<Step>
            {
                new Step(1, 2, 0, BucketAction.FillBucketX),
                new Step(2, 0, 2, BucketAction.TransferFromXToY),
                new Step(3, 2, 2, BucketAction.FillBucketX),
                new Step(4, 0, 4, BucketAction.TransferFromXToY, "Solved")
            }
        };

        yield return new object[]
        {
            2, 100, 96,
            new List<Step>
            {
                new Step(1, 0, 100, BucketAction.FillBucketY),
                new Step(2, 2, 98, BucketAction.TransferFromYToX),
                new Step(3, 0, 98, BucketAction.EmptyBucketX),
                new Step(4, 2, 96, BucketAction.TransferFromYToX, "Solved")
            }
        };

        yield return new object[]
        {
            2, 6, 5,
            null // Expecting an exception
        };
    }

    [Theory]
    [MemberData(nameof(GetTestCases))]
    public void Solve_ShouldReturnExpectedSolution(int bucket1Capacity, int bucket2Capacity, int targetVolume, List<Step> expectedSteps)
    {
        if (expectedSteps == null)
        {
            Assert.Throws<NoAvailableSolutionException>(() => _service.Solve(bucket1Capacity, bucket2Capacity, targetVolume));
        }
        else
        {
            var solution = _service.Solve(bucket1Capacity, bucket2Capacity, targetVolume);
            Assert.Equal(expectedSteps.Count, solution.Steps.Count);
            for (int i = 0; i < expectedSteps.Count; i++)
            {
                Assert.Equal(expectedSteps[i].StepCount, solution.Steps[i].StepCount);
                Assert.Equal(expectedSteps[i].BucketX, solution.Steps[i].BucketX);
                Assert.Equal(expectedSteps[i].BucketY, solution.Steps[i].BucketY);
                Assert.Equal(expectedSteps[i].Action, solution.Steps[i].Action);
                Assert.Equal(expectedSteps[i].Status, solution.Steps[i].Status);
            }
        }
    }
}