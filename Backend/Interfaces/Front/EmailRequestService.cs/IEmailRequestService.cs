

using ST_API.Models;

namespace ST_API.Interfaces
{
    public interface IEmailRequestService
    {
        cEmailRequestResult GetEmail(EmailRequest objEmail);
        cEmailRequestResult SaveEmailConfirmation(EmailData objEmailConfirm);
    }
}