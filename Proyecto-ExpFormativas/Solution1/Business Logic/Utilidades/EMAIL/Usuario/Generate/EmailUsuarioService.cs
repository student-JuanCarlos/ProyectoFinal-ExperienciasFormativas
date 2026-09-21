using Business_Logic.Utilidades.EMAIL.Usuario.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.EMAIL.Usuario.Generate
{
    public class EmailUsuarioService : IEmailUsuario
    {
        private readonly IConfiguration _config;
        private readonly IEmailUsuarioEstructure _emailEstructure;

        public EmailUsuarioService(IConfiguration config, IEmailUsuarioEstructure estructure)
        {
            _config = config;
            _emailEstructure = estructure;
        }

        public async Task EnviarCorreo(Entities.Usuario usuario)
        {
            var smtpSettings = _config.GetSection("Smtp");

            var (asunto, cuerpo) = _emailEstructure.BienvenidaUsuario(usuario);

            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("La Estancia Gourmet", smtpSettings["Usuario"]));
            mensaje.To.Add(new MailboxAddress("", usuario.Email));
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo.ToMessageBody();

            using var cliente = new SmtpClient();
            await cliente.ConnectAsync(smtpSettings["Host"], int.Parse(smtpSettings["Port"]), SecureSocketOptions.StartTls);
            await cliente.AuthenticateAsync(smtpSettings["Usuario"], smtpSettings["Contraseña"]);
            await cliente.SendAsync(mensaje);
            await cliente.DisconnectAsync(true);

        }
    }
}
