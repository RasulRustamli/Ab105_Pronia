using Ab_105_Pronia.Helpers.Email;

namespace Ab_105_Pronia.Abstractions
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
