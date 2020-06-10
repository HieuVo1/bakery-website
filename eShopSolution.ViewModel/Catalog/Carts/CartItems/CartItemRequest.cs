namespace eShopSolution.ViewModel.Catalog.Carts.CartItems
{
    public class CartItemRequest
    {
        public int ProductID { set; get; }
        public int CartID { set; get; }
        public int Quantity { set; get; }
        public decimal PriceChange { set; get; }
    }
}
