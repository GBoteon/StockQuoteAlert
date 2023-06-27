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
            decimal sellPrice = 40.43m;
            decimal entryPrice = 35.10m;
            if (args.Length == 3)
            {
                symbol = args[0];
                sellPrice = decimal.Parse(args[1]);
                entryPrice = decimal.Parse(args[2]);
            }

            string emailDestino = ConfigReader.ReadSetting("EmailDestino");
            string smtpHost = ConfigReader.ReadSetting("SMTPHost");
            string smtpPort = ConfigReader.ReadSetting("SMTPPort");
            string smtpUsername = ConfigReader.ReadSetting("SMTPUsername");
            string smtpPassword = ConfigReader.ReadSetting("SMTPPassword");
            string apiKey = ConfigReader.ReadSetting("APIKey");

            while (true)
            {
                GlobalQuote globalQuote = await StockQuoteService.GetStockQuote(symbol, apiKey);
                var quote = 00.00m;

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

                if (quote > sellPrice)
                {
                    EmailService.SendEmail(emailDestino, $"Venda recomendada do symbol {symbol}", $"O preço atual ({quote}) está acima do preço de venda ({sellPrice})", smtpHost, int.Parse(smtpPort), smtpUsername, smtpPassword);
                }
                else if (quote < entryPrice)
                {
                    EmailService.SendEmail(emailDestino, $"Compra recomendada do symbol {symbol}", $"O preço atual ({quote}) está abaixo do preço de compra ({entryPrice})", smtpHost, int.Parse(smtpPort), smtpUsername, smtpPassword);
                }

                Thread.Sleep(10000); // Aguardar 10 segundos antes de verificar novamente
            }
        }
    }
}