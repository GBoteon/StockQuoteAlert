using NUnit.Framework;
using StockQuoteAlert.Models;
using StockQuoteAlert.Services;

namespace StockQuoteAlert.Tests
{
    [TestFixture]
    public class StockQuoteServiceTests
    {
        [Test]
        public void GetStockQuote_ValidSymbol_ReturnsGlobalQuote()
        {
            // Arrange
            string symbol = "PETR4";
            string apiKey = "XQ5KZXVWW72DJEPE";

            // Act
            GlobalQuote globalQuote = StockQuoteService.GetStockQuote(symbol, apiKey).Result;

            // Assert
            Assert.IsNotNull(globalQuote);
            Assert.AreEqual(symbol, globalQuote.Symbol);
        }
    }
}