using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string symbol = "PETR4";
            decimal entryPrice = 22.67m;
            decimal sellPrice = 22.59m;

            // Ler as configurações do arquivo de configuração
            string emailDestino = ReadSetting("EmailDestino");
            string smtpHost = ReadSetting("SMTPHost");
            int smtpPort = int.Parse(ReadSetting("SMTPPort"));
            string smtpUsername = ReadSetting("SMTPUsername");
            string smtpPassword = ReadSetting("SMTPPassword");

            //loop para previnir encerramento do programa
            while (true)
            {
                // Loop para monitorar a cotação continuamente
                while (true)
                {
                    GlobalQuote globalQuote = await GetStockQuote(symbol);
                    decimal quote = decimal.Parse(globalQuote!.Price!.Replace('.', ','));
                    Console.WriteLine($"Cotação atual de {symbol}: {quote}");

                    // Verificar se é necessário enviar um e-mail
                    if (quote > entryPrice)
                    {
                        SendEmail(emailDestino, $"Venda recomendada do symbol {symbol}", $"O preço atual ({quote}) está acima do preço de venda ({entryPrice})", smtpHost, smtpPort, smtpUsername, smtpPassword);
                    }
                    else if (quote < sellPrice)
                    {
                        SendEmail(emailDestino, $"Compra recomendada do symbol {symbol}", $"O preço atual ({quote}) está abaixo do preço de compra ({sellPrice})", smtpHost, smtpPort, smtpUsername, smtpPassword);
                    }

                    Thread.Sleep(5000); // Aguardar 5 segundos antes de verificar novamente
                }
            }

        }

        static string ReadSetting(string key)
        {
            string filePath = "config.txt"; // Nome do arquivo de configuração
            string value = string.Empty;

            // Verificar se o arquivo de configuração existe
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2 && parts[0].Trim() == key)
                    {
                        value = parts[1].Trim();
                        break;
                    }
                }
            }

            return value;
        }

        static async Task<GlobalQuote> GetStockQuote(string symbol)
        {
            // Substitua o valor "YOUR_API_KEY" pelo seu próprio token de acesso à API Alpha Vantage
            string apiKey = "XQ5KZXVWW72DJEPE";
            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}.SA&apikey={apiKey}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                // Analisar a resposta JSON e extrair o preço da cotação
                var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(responseBody);
                var globalQuote = quoteResponse!.GlobalQuote;

                // Retornar classe de resposta
                return globalQuote!;
            }
        }

        static void SendEmail(string to, string subject, string body, string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        smtpClient.Send(mailMessage);
                        Console.WriteLine($"E-mail para {to} com sucesso");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao enviar email {ex}");
                    }
                }
            }
        }
    }

    public class QuoteResponse
    {
        [JsonProperty("Global Quote")]
        public GlobalQuote? GlobalQuote { get; set; }
    }

    public class GlobalQuote
    {
        [JsonProperty("01. symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("02. open")]
        public string? Open { get; set; }

        [JsonProperty("03. high")]
        public string? High { get; set; }

        [JsonProperty("04. low")]
        public string? Low { get; set; }

        [JsonProperty("05. price")]
        public string? Price { get; set; }

        [JsonProperty("06. volume")]
        public string? Volume { get; set; }

        [JsonProperty("07. latest trading day")]
        public string? LatestTradingDay { get; set; }

        [JsonProperty("08. previous close")]
        public string? PreviousClose { get; set; }

        [JsonProperty("09. change")]
        public string? Change { get; set; }

        [JsonProperty("10. change percent")]
        public string? ChangePercent { get; set; }
    }
}
