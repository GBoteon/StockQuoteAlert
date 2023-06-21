using System.IO;

namespace StockQuoteAlert.Utils
{
    public class ConfigReader
    {
        public static string ReadSetting(string key)
        {
            string filePath = "C:\\Users\\Gustavo\\RiderProjects\\StockQuoteAlert\\StockQuoteAlert\\src\\Utils\\config.txt"; // TODO: melhorar importação de configurações 
            string value = string.Empty;

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
    }
}