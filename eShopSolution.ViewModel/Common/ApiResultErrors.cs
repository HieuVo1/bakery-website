using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Common
{
    public class ApiResultErrors<T>:ApiResult<T>
    {
        public ApiResultErrors(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        public ApiResultErrors()
        {
            IsSuccessed = false;
        }
    }
}
