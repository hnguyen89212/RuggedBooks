using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities.EmailWithMailKit
{
    public class EmailMessage
    {
		public List<EmailAddress> ToAddresses { get; set; }
		
		public List<EmailAddress> FromAddresses { get; set; }
		
		public string Subject { get; set; }
		
		public string Content { get; set; }

		public EmailMessage()
		{
			ToAddresses = new List<EmailAddress>();
			FromAddresses = new List<EmailAddress>();
		}
	}
}
