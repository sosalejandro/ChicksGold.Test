using AutoMapper;
using ChicksGold.Test.Domain;
using ChicksGold.Test.Domain.AutoMapper;
using ChicksGold.Test.Domain.Contracts;
using ChicksGold.Test.Domain.Enums;
using ChicksGold.Test.Presentation.ActionFilters;
using ChicksGold.Test.Service.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace ChicksGold.Test.Presentation.Tests
{
    public class TwoBucketControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IServiceManager> _serviceManagerMock;
        private readonly Mock<IBucketChallengeService> _bucketChallengeServiceMock;
        private readonly IMapper _mapper;

        public TwoBucketControllerTests(WebApplicationFactory<Program> factory)
        {
            _serviceManagerMock = new Mock<IServiceManager>();
            _bucketChallengeServiceMock = new Mock<IBucketChallengeService>();
            _serviceManagerMock.Setup(sm => sm.BucketChallengeService).Returns(_bucketChallengeServiceMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ValidationFilterAttribute>();
                    services.AddScoped<CacheAttribute>();
                    services.AddSingleton(_serviceManagerMock.Object);
                    services.AddSingleton(_mapper);
                    services.AddControllers(options =>
                    {
                        options.Filters.Add<ValidationFilterAttribute>();
                    });
                });
            });
        }

        //[Fact]
        //public async Task SolveTwoBucketProblem_ShouldReturnUnprocessableEntity_WhenDtoHasNegativeValue()
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();
        //    var dto = new TwoBucketProblemDto
        //    {
        //        XCapacity = -1,
        //        YCapacity = 5,
        //        ZAmountWanted = 3
        //    };

        //    // Act
        //    var response = await client.PostAsJsonAsync("/api/TwoBucket", dto);

        //    // Assert
        //    Assert.Equal(422, (int)response.StatusCode);
        //}

        [Fact]
        public async Task SolveTwoBucketProblem_ShouldReturnBadRequest_WhenNoAvailableSolutionExceptionIsThrown()
        {
            // Arrange
            var client = _factory.CreateClient();
            var dto = new TwoBucketProblemDto
            {
                XCapacity = 2,
                YCapacity = 6,
                ZAmountWanted = 5
            };

            _bucketChallengeServiceMock
                .Setup(s => s.Solve(dto.XCapacity, dto.YCapacity, dto.ZAmountWanted))
                .Throws(new NoAvailableSolutionException("No solution available"));

            // Act
            var response = await client.PostAsJsonAsync("/api/TwoBucket", dto);

            // Assert
            Assert.Equal(400, (int)response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("No solution available", content);
        }

        [Fact]
        public async Task SolveTwoBucketProblem_ShouldReturnOk_WhenSolutionIsFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var dto = new TwoBucketProblemDto
            {
                XCapacity = 2,
                YCapacity = 10,
                ZAmountWanted = 4
            };

            var expectedSolution = new Solution(new List<Step>  {
                new Step(1, 2, 0, BucketAction.FillBucketX),
                new Step(2, 0, 2, BucketAction.TransferFromXToY),
                new Step(3, 2, 2, BucketAction.FillBucketX),
                new Step(4, 0, 4, BucketAction.TransferFromXToY, "Solved")
            });

            _bucketChallengeServiceMock
                .Setup(s => s.Solve(dto.XCapacity, dto.YCapacity, dto.ZAmountWanted))
                .Returns(expectedSolution);

            // Act
            var response = await client.PostAsJsonAsync("/api/TwoBucket", dto);

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
            var solutionResultDto = await response.Content.ReadFromJsonAsync<SolutionResultDto>();
            Assert.Equal(expectedSolution.Steps.Count, solutionResultDto.Steps.Count);
            for (int i = 0; i < expectedSolution.Steps.Count; i++)
            {
                Assert.Equal(expectedSolution.Steps[i].StepCount, solutionResultDto.Steps[i].StepCount);
                Assert.Equal(expectedSolution.Steps[i].BucketX, solutionResultDto.Steps[i].BucketX);
                Assert.Equal(expectedSolution.Steps[i].BucketY, solutionResultDto.Steps[i].BucketY);
                Assert.Equal(expectedSolution.Steps[i].Action, solutionResultDto.Steps[i].Action);
                Assert.Equal(expectedSolution.Steps[i].Status, solutionResultDto.Steps[i].Status);
            }
        }


        [Fact]
        public async Task OnActionExecuting_ShouldReturnUnprocessableEntity_WhenModelIsInvalid()
        {
            // Arrange
            var validationFilter = new ValidationFilterAttribute();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());
            actionContext.ModelState.AddModelError("XCapacity", "XCapacity must be a non-negative value.");

            var actionExecutingContext = new ActionExecutingContext(actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null);

            ActionExecutionDelegate mockDelegate = () =>
            {
                return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), null));
            };

            // Act
            validationFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            var result = Assert.IsType<UnprocessableEntityObjectResult>(actionExecutingContext.Result);
            Assert.Equal(422, result.StatusCode);
        }
    }
}