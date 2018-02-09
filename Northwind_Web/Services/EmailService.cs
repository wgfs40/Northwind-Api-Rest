namespace Northwind_Web.Services
{
    using Microsoft.AspNet.Identity;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Threading.Tasks;

    public class EmailService : IIdentityMessageService
    {


        public Task SendAsync(IdentityMessage message)
        {
            //var client = new SendGridClient();
            var msg = new SendGridMessage();

            throw new NotImplementedException();
        }
    }
}