using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShopSolution.ViewModel.Email
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }


    }
}
