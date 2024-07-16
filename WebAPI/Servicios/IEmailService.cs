using System.Threading.Tasks;

namespace TestAoniken.Servicios
{
    public interface IEmailService
    {
        Task SendEmailAsync(int usuarioId, string subject, string message);
    }
}