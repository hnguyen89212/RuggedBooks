using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities.EmailWithMailKit
{
    public interface IEmailConfiguration
    {
        // SMTP
        string SmtpServer { get; }

        int SmtpPort { get; }

        string SmtpUsername { get; set; }

        string SmtpPassword { get; set; }

        // POP
        //string PopServer { get; }
        //int PopPort { get; }
        //string PopUsername { get; }
        //string PopPassword { get; }
    }
}
