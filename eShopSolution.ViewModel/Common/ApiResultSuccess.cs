namespace eShopSolution.ViewModel.Common
{
    public class ApiResultSuccess<T>:ApiResult<T>
    {
        public ApiResultSuccess(T resultObject)
        {
            IsSuccessed = true;
            ResultObject = resultObject;
        }
        public ApiResultSuccess()
        {
            IsSuccessed = true;
        }
    }
}
