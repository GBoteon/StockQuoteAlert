using Newtonsoft.Json;
using StockQuoteAlert.Models;

namespace StockQuoteAlert.Services
{
    public class StockQuoteService
    {
        public static async Task<GlobalQuote> GetStockQuote(string symbol, string apiKey)
        {
            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}.SA&apikey={apiKey}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(responseBody);
                var globalQuote = quoteResponse.GlobalQuote;

                return globalQuote;
            }
        }
    }
}