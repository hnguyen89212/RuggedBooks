using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities.EmailWithMailKit
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage emailMessage);

        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
