namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageViewModel
    {
        public int Id { set; get; }
        public bool IsDefault { set; get; }
        public long FileSize { set; get; }
        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }

    }
}
