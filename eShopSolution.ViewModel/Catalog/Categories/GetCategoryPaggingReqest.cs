using eShopSolution.ViewModel.Common;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class GetCategoryPaggingReqest: PaggingRequestBase
    {
        public string Keywork { set; get; }
    }
}
