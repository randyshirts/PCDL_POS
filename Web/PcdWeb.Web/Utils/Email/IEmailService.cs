
using System.Net.Mail;
using Abp.Domain.Services;

namespace PcdWeb.Web.Utils.Email
{
    public interface IEmailService : IDomainService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="mail">Email to be sent</param>
        void SendEmail(MailMessage mail);
    }
}