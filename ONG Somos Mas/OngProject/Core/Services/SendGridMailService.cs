using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OngProject.Core.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using HandlebarsDotNet;
using System.IO;
using System;


namespace OngProject.Core.Services
{
    public class SendGridMailService : IMailService
    {

        private readonly ILogger<SendGridMailService> _logger;
        private readonly IConfiguration _configuration;

        public SendGridMailService(IConfiguration configuration, ILogger<SendGridMailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetHtml(string basePathTemplate, object data)
        {
            if (!File.Exists(basePathTemplate))
                throw new Exception(string.Format("La plantilla {0} no se ha encontrado en el folder de plantillas", basePathTemplate));

            var templateText = File.ReadAllText(basePathTemplate);
            var template = Handlebars.Compile(templateText);

            return template(data);
        }


        public async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
        {
            try
            {
                var aux = _configuration["SendGridAPIKey"];
                var sender = _configuration["SenderEmail"];
                var client = new SendGridClient(aux);
                _logger.LogInformation($"The mail has been sent to {toEmail}");
                var from = new EmailAddress(sender, "ONG");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
                var response = await client.SendEmailAsync(msg);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                _logger.LogError($"The mail could not be sent to {toEmail}");
                return false;
            }
        }

       

    }
}
