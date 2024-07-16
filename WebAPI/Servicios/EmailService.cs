using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TestAoniken.Data;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public EmailService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendEmailAsync(int usuarioId, string subject, string message)
        {
            Usuario usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                throw new ArgumentException($"No se encontró un usuario con el ID {usuarioId}.", nameof(usuarioId));
            }

            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                throw new InvalidOperationException($"El usuario con ID {usuarioId} no tiene un correo electrónico válido.");
            }

            using (var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"]))
            {
                smtpClient.Port = int.Parse(_configuration["Email:SmtpPort"]);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:SmtpUser"], _configuration["Email:SmtpPass"]);

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_configuration["Email:FromAddress"]);
                    mailMessage.To.Add(usuario.Email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    catch (SmtpException smtpEx)
                    {
                        Console.WriteLine($"Error SMTP: {smtpEx.StatusCode}, {smtpEx.Message}");
                        throw new ApplicationException($"Error SMTP al enviar el correo: {smtpEx.StatusCode}", smtpEx);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error general: {ex.Message}");
                        throw new ApplicationException("Error al enviar el correo electrónico.", ex);
                    }
                }
            }
        }
    }
}