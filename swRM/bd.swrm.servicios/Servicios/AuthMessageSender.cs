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
                Mail mail = new Mail
                {
                    Body = ConstantesCorreo.MensajeCorreoSuperior + message,
                    EmailTo = emailTo,
                    NameTo = "Name To",
                    Subject = subject
                };
                await Emails.SendEmailAsync(mail);
            }
            catch (Exception)
            { }
        }
    }
}
