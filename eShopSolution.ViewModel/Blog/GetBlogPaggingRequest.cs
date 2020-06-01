﻿using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Blog
{
    public class GetBlogPaggingRequest : PaggingRequestBase
    {
        public string CategoryUrl { set; get; }
        public string Keyword { set; get; }
    }
}
