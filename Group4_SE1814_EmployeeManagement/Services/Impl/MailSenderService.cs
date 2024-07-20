using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Security;
using System.Net.Mail;
namespace Services.Impl
{
   
    public class MailSenderService
    {
        public void SendEmail(string email, string newPassword)
        {
            MailMessage mail = new MailMessage();
            SmtpClient Smtp = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("quangss001@gmail.com", "Employee Management");
            mail.To.Add(email);
            mail.Subject = "Change password";
            mail.Body = $"Your password has been reset to {newPassword}";

            Smtp.Port = 587;
            Smtp.Credentials = new System.Net.NetworkCredential("quangss001@gmail.com", "xsjk rdpd brwj ikkg");
            Smtp.EnableSsl = true;
            Smtp.Send(mail);

        }
    }
}
