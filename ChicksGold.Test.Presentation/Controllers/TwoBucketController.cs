using AutoMapper;
using ChicksGold.Test.Domain;
using ChicksGold.Test.Domain.Contracts;
using ChicksGold.Test.Presentation.ActionFilters;
using ChicksGold.Test.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ChicksGold.Test.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TwoBucketController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IServiceManager _serviceManager;
    private readonly ILogger<TwoBucketController> _logger;
    private readonly IMemoryCache _cache;

    public TwoBucketController(IMapper mapper, IServiceManager serviceManager, ILogger<TwoBucketController> logger, IMemoryCache cache)
    {
        _mapper = mapper;
        _serviceManager = serviceManager;
        _logger = logger;
        _cache = cache;
    }

    [HttpOptions]
    [SwaggerOperation(Summary = "Provides information about the communication options for the TwoBucket endpoint.")]
    public IActionResult Options()
    {
        Response.Headers.Allow = "OPTIONS, POST";
        Response.Headers.AcceptEncoding = "gzip, br";
        return Ok();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Solves the two bucket problem", Description = "Finds the steps to get the desired amount of water using two buckets with given capacities.")]
    [SwaggerResponse(200, "Returns the solution steps", typeof(SolutionResultDto))]
    [SwaggerResponse(400, "Bad request due to invalid input or no available solution", typeof(string))]
    [SwaggerResponse(422, "Unprocessable entity due to validation errors", typeof(ValidationProblemDetails))]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 300)]
    public IActionResult SolveTwoBucketProblem([FromBody] TwoBucketProblemDto dto)
    {
        _logger.LogInformation("Received request to solve two bucket problem with XCapacity: {XCapacity}, YCapacity: {YCapacity}, ZAmountWanted: {ZAmountWanted}", dto.XCapacity, dto.YCapacity, dto.ZAmountWanted);

        var cacheKey = $"{dto.XCapacity}-{dto.YCapacity}-{dto.ZAmountWanted}";
        if (_cache.TryGetValue(cacheKey, out SolutionResultDto cachedSolution))
        {
            _logger.LogInformation("Returning cached solution for the two bucket problem.");
            return Ok(cachedSolution);
        }

        try
        {
            var solution = _serviceManager.BucketChallengeService.Solve(dto.XCapacity, dto.YCapacity, dto.ZAmountWanted);
            var solutionResultDto = _mapper.Map<SolutionResultDto>(solution);

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Adjust the expiration time as needed

            // Save data in cache
            _cache.Set(cacheKey, solutionResultDto, cacheEntryOptions);

            _logger.LogInformation("Solution found for the two bucket problem.");
            return Ok(solutionResultDto);
        }
        catch (NoAvailableSolutionException ex)
        {
            _logger.LogWarning("No available solution for the given input: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while solving the two bucket problem.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}