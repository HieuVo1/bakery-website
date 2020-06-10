using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Common;
using System;
using System.Threading.Tasks;

namespace eShopSolution.Application.Comom
{
    public static class SaveChangeService
    {
        public static async Task<ApiResult<bool>> SaveChangeAsyncNotImage(EShopDbContext _context)
        {
            try
            {
                var change = await _context.SaveChangesAsync();
                if (change > 0)
                {
                    return new ApiResultSuccess<bool>();
                }
                else
                {
                    return new ApiResultErrors<bool>("Faild");
                }
            }
            catch (Exception ex)
            {
                return new ApiResultErrors<bool>(ex.InnerException.Message);
            }
        }
        public static async Task<ApiResult<bool>> SaveChangeAsyncImage(EShopDbContext context,string imagePath, IStorageService storageService)
        {
            try
            {
                var change = await context.SaveChangesAsync();
                if (change > 0)
                {
                    await storageService.DeleteFileAsync(imagePath);
                    return new ApiResultSuccess<bool>();
                }
                else
                {
                    return new ApiResultErrors<bool>("Update faild");
                }
            }
            catch (Exception ex)
            {
                return new ApiResultErrors<bool>(ex.InnerException.Message);
            }
        }
    }
}
