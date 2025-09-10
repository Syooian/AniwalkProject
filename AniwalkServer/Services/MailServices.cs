using AniwalkServer.Data;
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
        public async Task<Result> SendEmailAsync(string toEmail, string subject, string body)
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

                return new Result();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendEmailAsync : " + ex.Message);

                return new Result(ResultType.Fail);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SmtpClient GetClient()
        {
            var Host = _configuration["Host"];
            var Port = _configuration["Port"];
            var User = _configuration["Username"];
            var PW = _configuration["Password"];
            var Ssl = _configuration["EnableSsl"];

            //Debug.WriteLine($"Host : {Host}, Port : {Port}, User : {User}, PW : {PW}, EnableSsl : {Ssl}");

            var SC = new SmtpClient()
            {
                UseDefaultCredentials = false,//需在Credentials = new NetworkCredential之前做，因為UseDefaultCredentials = false會把Credentials設為null
                Host = _configuration["Host"],
                Port = int.Parse(_configuration["Port"]),
                Credentials = new NetworkCredential(_configuration["Username"], _configuration["Password"]),
                EnableSsl = bool.Parse(_configuration["EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            SC.SendCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    Debug.WriteLine("SendCompleted Error : " + e.Error.Message);
                }
                else
                {
                    Debug.WriteLine("SendCompleted");
                }
            };

            return SC;
        }
    }
}
