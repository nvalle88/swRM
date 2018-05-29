using bd.swrm.entidades.Constantes;
using bd.swrm.servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendMails.methods;
using EnviarCorreo;

namespace bd.swrm.servicios.Servicios
{
    public class AuthMessageSender : IEmailSender
    {
        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            try
            {
                MailConfig.HostUri = ConstantesCorreo.Smtp;
                MailConfig.PrimaryPort = Convert.ToInt32(ConstantesCorreo.PrimaryPort);
                MailConfig.SecureSocketOptions = Convert.ToInt32(ConstantesCorreo.SecureSocketOptions);

                Mail mail = new Mail
                {
                    Password = ConstantesCorreo.PasswordCorreo,
                    Body = message,
                    EmailFrom = ConstantesCorreo.CorreoRM,
                    EmailTo = emailTo,
                    NameFrom = ConstantesCorreo.NameFrom,
                    NameTo = "Name To",
                    Subject = subject
                };
                //await Emails.SendEmailAsync(mail);
                Emails.SendEmail(mail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
