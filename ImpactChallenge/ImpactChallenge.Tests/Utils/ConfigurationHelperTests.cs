using ImpactChallenge.WebApi.Utils;
using Microsoft.Extensions.Configuration;

namespace ImpactChallenge.Tests.Utils
{
    [TestClass]
    public class ConfigurationHelperTests
    {
        private readonly string apiUrl = "https://test-feed.com";
        private readonly int limitMax = 10;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WhenServiceIsNull_ThrowException()
        {
            new ConfigurationHelper(null);
        }

        [TestMethod]
        public void Values_Exist_returnsValue()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"AppSettings:BasketApiUrl", apiUrl},
            };


            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var controller = new ConfigurationHelper(configuration);
            Assert.AreEqual(controller.BasketApiUrl, apiUrl);
        }

        [TestMethod]
        public void Value_DoestNotExists_returnsNull()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"AppSettings", "{}"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var controller = new ConfigurationHelper(configuration);
            Assert.AreEqual(controller.BasketApiUrl, null);
        }
    }
}