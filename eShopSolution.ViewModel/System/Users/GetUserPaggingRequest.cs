using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class GetUserPaggingRequest: PaggingRequestBase
    {
        public string Keyword { set; get; }
    }
}
