using System.Collections.Generic;

namespace eShopSolution.ViewModel.Common
{
    public class PageViewModel<T>: PageResultPage
    {
        public List<T> Items { set; get; }
    }
}
