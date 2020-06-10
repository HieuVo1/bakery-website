namespace eShopSolution.ViewModel.Common
{
    public class PageResultPage
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }
                return (TotalRecords / PageSize) == 0 ? TotalRecords / PageSize : TotalRecords / PageSize + 1;
            }
        }
    }
}
