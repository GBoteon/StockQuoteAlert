using System.Net;
using System.Net.Mail;

namespace StockQuoteAlert.Services
{
    public class EmailService
    {
        public static void SendEmail(string to, string subject, string body, string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
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
                        Console.WriteLine($"E-mail para {to} enviado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao enviar e-mail {ex}");
                    }
                }
            }
        }
    }
}