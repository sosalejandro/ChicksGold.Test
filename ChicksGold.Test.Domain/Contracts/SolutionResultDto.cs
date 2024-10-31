using System.Text.Json.Serialization;

namespace ChicksGold.Test.Domain.Contracts;

public class SolutionResultDto
{
    [JsonPropertyName("solution")]
    public List<StepResultDto> Steps { get; set; }
}