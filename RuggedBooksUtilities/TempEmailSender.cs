using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RuggedBooksUtilities
{
    // Just a temporary Email sender.
    // Needs to install the Identity.UI package.
    // Configures the app to use this in Startup.cs
    public class TempEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
