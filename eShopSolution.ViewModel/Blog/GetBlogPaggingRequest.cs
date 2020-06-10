using eShopSolution.ViewModel.Common;

namespace eShopSolution.ViewModel.Blog
{
    public class GetBlogPaggingRequest : PaggingRequestBase
    {
        public string CategoryUrl { set; get; }
        public string Keyword { set; get; }
        public string languageId { set; get; }
        public string UserId { set; get; }
    }
}
