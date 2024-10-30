namespace ChicksGold.Test.Domain;

/// <summary>
/// Represents the state of the buckets in the bucket challenge problem.
/// </summary>
public class State
{
    /// <summary>
    /// Gets the volume of bucket 1.
    /// </summary>
    public int Bucket1Volume { get; }

    /// <summary>
    /// Gets the volume of bucket 2.
    /// </summary>
    public int Bucket2Volume { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> class with the specified bucket volumes.
    /// </summary>
    /// <param name="bucket1Volume">The volume of bucket 1.</param>
    /// <param name="bucket2Volume">The volume of bucket 2.</param>
    public State(int bucket1Volume, int bucket2Volume)
    {
        Bucket1Volume = bucket1Volume;
        Bucket2Volume = bucket2Volume;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        if (obj is State other)
        {
            return Bucket1Volume == other.Bucket1Volume && Bucket2Volume == other.Bucket2Volume;
        }
        return false;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Bucket1Volume, Bucket2Volume);
    }
}
