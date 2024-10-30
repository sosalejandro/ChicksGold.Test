using Xunit;
using Moq;
using ChicksGold.Test.Service.Abstractions;
using ChicksGold.Test.Domain;

namespace ChicksGold.Test.Service.Tests
{
    public class ServiceManagerTests
    {
        private readonly Mock<IBucketChallengeService> _mockBucketChallengeService;
        private readonly ServiceManager _serviceManager;

        public ServiceManagerTests()
        {
            _mockBucketChallengeService = new Mock<IBucketChallengeService>();
            _serviceManager = new ServiceManager(_mockBucketChallengeService.Object);
        }

        [Fact]
        public void BucketChallengeService_ShouldReturnLazyLoadedService()
        {
            // Arrange
            var expectedSolution = new Solution(new List<Step>());
            _mockBucketChallengeService.Setup(s => s.Solve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedSolution);

            // Act
            var result = _serviceManager.BucketChallengeService.Solve(2, 10, 4);

            // Assert
            Assert.Equal(expectedSolution, result);
            _mockBucketChallengeService.Verify(s => s.Solve(2, 10, 4), Times.Once);
        }
    }
}
