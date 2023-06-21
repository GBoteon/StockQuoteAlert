using Newtonsoft.Json;

namespace StockQuoteAlert.Models
{
    public class QuoteResponse
    {
        [JsonProperty("Global Quote")]
        public GlobalQuote? GlobalQuote { get; set; }
    }
}