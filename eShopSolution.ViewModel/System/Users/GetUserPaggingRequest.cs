using eShopSolution.ViewModel.Common;

namespace eShopSolution.ViewModel.System.Users
{
    public class GetUserPaggingRequest: PaggingRequestBase
    {
        public string Keyword { set; get; }
    }
}
