using StockQuoteAlert.Models;
using StockQuoteAlert.Services;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string symbol = "PETR4";
            decimal entryPrice = 30.43m;
            decimal sellPrice = 30.10m;
            if (args.Length == 3)
            {
                symbol = args[0];
                entryPrice = decimal.Parse(args[1]);
                sellPrice = decimal.Parse(args[2]);
            }

            string emailDestino = ConfigReader.ReadSetting("EmailDestino");
            string smtpHost = ConfigReader.ReadSetting("SMTPHost");
            int smtpPort = int.Parse(ConfigReader.ReadSetting("SMTPPort"));
            string smtpUsername = ConfigReader.ReadSetting("SMTPUsername");
            string smtpPassword = ConfigReader.ReadSetting("SMTPPassword");
            string apiKey = ConfigReader.ReadSetting("APIKey");

            while (true)
            {
                GlobalQuote globalQuote = await StockQuoteService.GetStockQuote(symbol, apiKey);
                decimal quote = 0.0m;
                try
                {
                    quote = decimal.Parse(globalQuote.Price!.Replace('.', ','));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Chamada da API obteve um retorno nulo: {e}\nErro ocorrido devido ao limite de chamadas na API");
                    Thread.Sleep(5000);
                    continue;
                }

                Console.WriteLine($"Cotação atual de {symbol}: {quote}");

                if (quote > entryPrice)
                {
                    EmailService.SendEmail(emailDestino, $"Venda recomendada do symbol {symbol}", $"O preço atual ({quote}) está acima do preço de venda ({entryPrice})", smtpHost, smtpPort, smtpUsername, smtpPassword);
                }
                else if (quote < sellPrice)
                {
                    EmailService.SendEmail(emailDestino, $"Compra recomendada do symbol {symbol}", $"O preço atual ({quote}) está abaixo do preço de compra ({sellPrice})", smtpHost, smtpPort, smtpUsername, smtpPassword);
                }

                Thread.Sleep(10000); // Aguardar 10 segundos antes de verificar novamente
            }
        }
    }
}