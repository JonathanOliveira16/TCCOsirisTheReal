using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OsirisPdvReal.Utils
{
    public class EmailHelper
    {
        public static void SendEmail(string to, String token)
        {
            var smtpClient = new SmtpClient("mail.osirispdv.com")
            {
                Port = 25,
                Credentials = new NetworkCredential("postmaster@osirispdv.com", "321Aa@321"),
                EnableSsl = true,

            };

            smtpClient.Send("postmaster@osirispdv.com", to, "Reset de senha Osiris PDV", "Seu código de reset de senha é " + token);
        }

        public static void SendNew(string destinatarios, string token)
        {
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@osirispdv.com"); //IMPORTANT: This must be same as your smtp authentication address.
            mail.To.Add(destinatarios);

            //set the content 
            mail.Subject = "This is an email";
            mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
            //send the message 
            SmtpClient smtp = new SmtpClient("mail.osirispdv.com");

            //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
            NetworkCredential Credentials = new NetworkCredential("postmaster@osirispdv.com", "321Aa@321");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = Credentials;
            smtp.Port = 25;    //alternative port number is 8889
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }
    }
}
