namespace ChicksGold.Test.Domain;

/// <summary>
/// Exception thrown when a bucket overflows.
/// </summary>
/// <param name="message">The exception message.</param>
public class BucketOverflowException(string message) : Exception(message)
{
}

/// <summary>
/// Exception thrown when a bucket is empty.
/// </summary>
/// <param name="message">The exception message.</param>
public class BucketEmptyException(string message) : Exception(message)
{
}

/// <summary>
/// Exception thrown when no solution is available for the given input.
/// </summary>
/// <param name="message">The exception message.</param>
public class NoAvailableSolutionException(string message) : Exception(message)
{
}