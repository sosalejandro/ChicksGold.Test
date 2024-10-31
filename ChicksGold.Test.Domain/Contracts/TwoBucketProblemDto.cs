using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChicksGold.Test.Domain.Contracts;

public class TwoBucketProblemDto
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "x_capacity must be a non-negative value.")]
    [JsonPropertyName("x_capacity")]
    public int XCapacity { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "y_capacity must be a non-negative value.")]
    [JsonPropertyName("y_capacity")]
    public int YCapacity { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "z_amount_wanted must be a non-negative value.")]
    [JsonPropertyName("z_amount_wanted")]
    public int ZAmountWanted { get; set; }
}
