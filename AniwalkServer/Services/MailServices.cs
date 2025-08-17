using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace AniwalkServer.Services
{
    public class MailServices
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MailServices(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("MailServices");
        }
        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="toEmail">收件人郵箱</param>
        /// <param name="subject">郵件主題</param>
        /// <param name="body">郵件內容</param>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var Client = GetClient();

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["From"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);
                await Client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendEmailAsync : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SmtpClient GetClient()
        {
            Debug.WriteLine($"Host : {_configuration["Host"]}, Port : {_configuration["Port"]}, User : {_configuration["Username"]}, PW : {_configuration["Password"]}");

            return new SmtpClient(_configuration["Host"])
            {
                Port = int.Parse(_configuration["Port"]),
                Credentials = new NetworkCredential(_configuration["Username"], _configuration["Password"]),
                EnableSsl = bool.Parse(_configuration["EnableSsl"]),
            };
        }
    }
}
