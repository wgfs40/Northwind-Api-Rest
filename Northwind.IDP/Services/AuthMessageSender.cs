namespace Northwind.IDP.Services
{
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public SMSoptions Options { get; set; } // set only via secre manager

        public AuthMessageSender(IOptions<SMSoptions> optionsAccesor)
        {
            Options = optionsAccesor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //var client = new SendGridClient("");
            //var from = new EmailAddress("wfernandez@wind.com.do");
            //var to = new EmailAddress("");// para donde va
            //var emailer = MailHelper.CreateSingleEmail(from, to, "probando_Subject", "Hola como estan", "Html");

            //await client.SendEmailAsync(emailer);

            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(email, "Wind Test"));

                // From
                mailMsg.From = new MailAddress("info@wind.com.do", "Wind Test");

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;
                string text = message;
                string html = @"<p>"+message+"</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("172.16.1.105");
                //smtpClient.EnableSsl = true;


                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@wind.com.do", "W1nd1234");
                smtpClient.Credentials = credentials;

                 smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }


        }

        public Task SendSmsAsync(string number, string message)
        {
            // plu in your sms service here to send a text message.
            // your account SID from twilio.com/cosole
            var accountsid = Options.SMSAccountIdentification;

            //your auth token from twilio.com/console
            var authToken = Options.SMSAccountPassword;

            TwilioClient.Init(accountsid,authToken);

            return MessageResource.CreateAsync(
                to: new PhoneNumber(number),
                from: new PhoneNumber(Options.SMSAccountFrom),
                body: message
                );
        }
    }
}
