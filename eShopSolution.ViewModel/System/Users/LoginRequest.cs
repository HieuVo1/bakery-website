﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class LoginRequest
    {
        public string UserName { set; get; }
        public string Passwork { set; get; }
        public bool RememberMe { set; get; }
    }
}
