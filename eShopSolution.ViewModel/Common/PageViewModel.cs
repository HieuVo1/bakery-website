using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Common
{
    public class PageViewModel<T>: PageResultPage
    {
        public List<T> Items { set; get; }
    }
}
