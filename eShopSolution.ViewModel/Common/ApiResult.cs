namespace eShopSolution.ViewModel.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
    }
}
