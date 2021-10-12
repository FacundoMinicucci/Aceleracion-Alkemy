using OngProject.Core.DTOs;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string content);

        string GetHtml(string basePathTemplate, object data);

    }
}
