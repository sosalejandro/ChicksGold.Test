using ChicksGold.Test.Domain.Enums;

namespace ChicksGold.Test.Domain.Tests;

public class SolutionTests
{
    [Fact]
    public void Solution_ShouldInitializeWithSteps()
    {
        // Arrange
        var steps = new List<Step>
        {
            new Step(1, 2, 0, BucketAction.FillBucketX),
            new Step(2, 0, 2, BucketAction.TransferFromXToY)
        };

        // Act
        var solution = new Solution(steps);

        // Assert
        Assert.Equal(steps, solution.Steps);
    }
}
