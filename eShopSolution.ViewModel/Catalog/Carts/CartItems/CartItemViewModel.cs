using eShopSolution.ViewModel.Catalog.Products;

namespace eShopSolution.ViewModel.Catalog.Carts.CartItems
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public int CartID { set; get; }
        public int Quantity { set; get; }
    }
}
