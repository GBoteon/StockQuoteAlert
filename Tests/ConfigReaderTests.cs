using NUnit.Framework;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert.Tests
{
    [TestFixture]
    public class ConfigReaderTests
    {
        [Test]
        public void ReadSetting_ExistingKey_ReturnsValue()
        {
            // Arrange
            string key = "EmailDestino";
            string expectedValue = "test@example.com";

            // Act
            string actualValue = ConfigReader.ReadSetting(key);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ReadSetting_NonExistingKey_ReturnsEmptyString()
        {
            // Arrange
            string key = "NonExistingKey";

            // Act
            string actualValue = ConfigReader.ReadSetting(key);

            // Assert
            Assert.AreEqual(string.Empty, actualValue);
        }
    }
}