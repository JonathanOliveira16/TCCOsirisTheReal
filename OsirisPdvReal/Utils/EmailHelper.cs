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
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tinforma2018@gmail.com", "sulamerica110"),
                EnableSsl = true,
            };

            smtpClient.Send("tinforma2018@gmail.com", to, "Reset de senha Osiris PDV", "Seu código de reset de senha é " + token);
        }
    }
}
