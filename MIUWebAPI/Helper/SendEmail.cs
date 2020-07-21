using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MIUWebAPI.Helper
{
    public class SendEmail
    {
        public class EmailManager
        {
            public static void SendEmail(string code, string toEmail, string subject)
            {
                MailMessage msg = new MailMessage();
                string sender = ConfigurationManager.AppSettings.Get("smtpUser");
                string password = ConfigurationManager.AppSettings.Get("smtpPass");

                msg.From = new MailAddress(sender);
                msg.To.Add(toEmail);
                msg.Subject = subject;
                msg.Body = code;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential(sender, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(msg);

            }
        }
    }
}