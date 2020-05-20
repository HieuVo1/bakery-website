﻿using eShopSolution.ViewModel.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Contact
{
    public class ContactCreateRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Messmage { get; set; }
        public EmailMessage EmailMessage { get; set; }
    }
}
