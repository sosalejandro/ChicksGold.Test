using ChicksGold.Test.Domain.Enums;
using ChicksGold.Test.Domain.Extensions;

namespace ChicksGold.Test.Domain.Tests;

public class StepTests
{
    [Fact]
    public void Step_ShouldInitializeWithValues()
    {
        // Arrange
        int stepCount = 1;
        int bucketX = 2;
        int bucketY = 0;
        var action = BucketAction.FillBucketX;
        string status = "In Progress";

        // Act
        var step = new Step(stepCount, bucketX, bucketY, action, status);

        // Assert
        Assert.Equal(stepCount, step.StepCount);
        Assert.Equal(bucketX, step.BucketX);
        Assert.Equal(bucketY, step.BucketY);
        Assert.Equal(action.GetDescription(), step.Action);
        Assert.Equal(status, step.Status);
    }

    [Fact]
    public void Step_ShouldInitializeWithoutStatus()
    {
        // Arrange
        int stepCount = 1;
        int bucketX = 2;
        int bucketY = 0;
        var action = BucketAction.FillBucketX;

        // Act
        var step = new Step(stepCount, bucketX, bucketY, action);

        // Assert
        Assert.Equal(stepCount, step.StepCount);
        Assert.Equal(bucketX, step.BucketX);
        Assert.Equal(bucketY, step.BucketY);
        Assert.Equal(action.GetDescription(), step.Action);
        Assert.Null(step.Status);
    }
}