namespace ChicksGold.Test.Domain.Contracts;

public class StepResultDto
{
    public int StepCount { get; set; }
    public int BucketX { get; set; }
    public int BucketY { get; set; }
    public string Action { get; set; }
    public string Status { get; set; }
}