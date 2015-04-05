using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DataModel.Data.ApplicationLayer.Utils.Logging;
using Microsoft.AspNet.Identity;

namespace DataModel.Data.ApplicationLayer.Utils.Email
{
    //TODO: Get setting from configuration
    /// <summary>
    /// Implements <see cref="IEmailService"/> to send emails using current settings.
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        //public ILogger Logger { get; set; }

        //private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new instance of <see cref="EmailService"/>.
        /// </summary>
        //public EmailService()
        //{
        //    Logger = NullLogger.Instance;
        //}

        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                MailMessage email = new MailMessage("donotreply@playcreatediscover.com", message.Destination);
                //MailMessage email = new MailMessage("playcreatediscover@gmail.com", message.Destination);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsBodyHtml = true;
                using (
                    var mailClient = new SmtpClient("mail.host4asp.net", 25) //,"smtp.gmail.com","mail.host4asp.net" 587, "mail.playcreatediscover.com"
                    {
                        //Credentials = new NetworkCredential("playcreatediscover@gmail.com", "love2learngr8"),
                        Credentials = new NetworkCredential("donotreply@playcreatediscover.com", "uqLdSLu6t0Uy"),
                        EnableSsl = false
                        
                    })
                {
                    //DataLog.Instance.Warn("Sending email!");
                    await mailClient.SendMailExAsync(email);
                }

                //var result = await mailClient.SendMailAsync(email);
                //result.Wait();
                //if (result.IsCompleted)
                //    return result;
            }
            catch (Exception ex)
            {
                //Logger.Warn("Could not send email!", ex);
                DataLog.Instance.Warn("Could not send email!", ex);
            }
        }

        public bool Send(IdentityMessage message)
        {
            try
            {
                MailMessage email = new MailMessage("donotreply@playcreatediscover.com", message.Destination);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsBodyHtml = true;
                using (
                    var mailClient = new SmtpClient("playcreatediscover.com", 587)
                    {
                        Credentials = new NetworkCredential("donotreply@playcreatediscover.com", "uqLdSLu6t0Uy"),
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
                //Logger.Warn("Could not send email!", ex);
                DataLog.Instance.Warn("Could not send email!", ex);
                return false;
            }
            //return null;
        }
    }
}