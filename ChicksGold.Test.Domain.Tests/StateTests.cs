namespace ChicksGold.Test.Domain.Tests;

public class StateTests
{
    [Fact]
    public void State_ShouldInitializeWithVolumes()
    {
        // Arrange
        int bucket1Volume = 3;
        int bucket2Volume = 5;

        // Act
        var state = new State(bucket1Volume, bucket2Volume);

        // Assert
        Assert.Equal(bucket1Volume, state.Bucket1Volume);
        Assert.Equal(bucket2Volume, state.Bucket2Volume);
    }

    [Fact]
    public void State_Equals_ShouldReturnTrueForSameVolumes()
    {
        // Arrange
        var state1 = new State(3, 5);
        var state2 = new State(3, 5);

        // Act & Assert
        Assert.Equal(state1, state2);
    }

    [Fact]
    public void State_Equals_ShouldReturnFalseForDifferentVolumes()
    {
        // Arrange
        var state1 = new State(3, 5);
        var state2 = new State(4, 5);

        // Act & Assert
        Assert.NotEqual(state1, state2);
    }

    [Fact]
    public void State_GetHashCode_ShouldReturnSameHashCodeForSameVolumes()
    {
        // Arrange
        var state1 = new State(3, 5);
        var state2 = new State(3, 5);

        // Act & Assert
        Assert.Equal(state1.GetHashCode(), state2.GetHashCode());
    }
}
