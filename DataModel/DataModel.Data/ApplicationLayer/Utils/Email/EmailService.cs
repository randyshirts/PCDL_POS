using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Abp.Configuration;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace DataModel.Data.ApplicationLayer.Utils.Email
{
    //TODO: Get setting from configuration
    /// <summary>
    /// Implements <see cref="IEmailService"/> to send emails using current settings.
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        public ILogger Logger { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="EmailService"/>.
        /// </summary>
        public EmailService()
        {
            Logger = NullLogger.Instance;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                MailMessage email = new MailMessage("playcreatediscover@gmail.com", message.Destination);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsBodyHtml = true;
                using (
                    var mailClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("playcreatediscover@gmail.com", "love2learngr8"),
                        EnableSsl = true
                    })
                {
                    await mailClient.SendMailAsync(email);
                }

                //var result = await mailClient.SendMailAsync(email);
                //result.Wait();
                //if (result.IsCompleted)
                //    return result;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send email!", ex);
            }
        }

        public bool Send(IdentityMessage message)
        {
            try
            {
                MailMessage email = new MailMessage("playcreatediscover@gmail.com", message.Destination);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsBodyHtml = true;
                using (
                    var mailClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("playcreatediscover@gmail.com", "love2learngr8"),
                        EnableSsl = true
                    })
                {
                    var task = mailClient.SendMailAsync(email);
                    return task.IsCompleted;
                }


                //var result = await mailClient.SendMailAsync(email);
                //result.Wait();
                //if (result.IsCompleted)
                //    return result;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send email!", ex);
                return false;
            }
            //return null;
        }
    }
}