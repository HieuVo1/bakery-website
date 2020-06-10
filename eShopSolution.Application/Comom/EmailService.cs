using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace eShopSolution.Application.Comom
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public async Task<ApiResult<string>> SendEmailAsync(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);

            return await SendAsync(mailMessage);
        }
        public async Task<ApiResult<string>> SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

                    await client.SendAsync(mailMessage);
                    return new ApiResultSuccess<string>();
                }
                catch(Exception e)
                {
                    //log an error message or throw an exception, or both.
                    return new ApiResultErrors<string>(e.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        public ApiResult<string> SendEmail(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);
            return  Send(mailMessage);
        }
        private ApiResult<string> Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                    client.Send(mailMessage);
                    return new ApiResultSuccess<string>();
                }
                catch(Exception e)
                {
                    return new ApiResultErrors<string>(e.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
