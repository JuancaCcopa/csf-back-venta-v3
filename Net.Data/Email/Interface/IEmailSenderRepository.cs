using System.Threading.Tasks;

namespace Net.Data
{
    public interface IEmailSenderRepository
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
