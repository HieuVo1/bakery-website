using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Comments
{
    public interface ICommentService
    {
        Task<ApiResult<CommentViewModel>> GetById(int commmentId);
        Task<ApiResult<string>> Create(CommentCreateRequest request);
        Task<ApiResult<string>> Update(CommentUpdateRequest request, int commmentId);
        Task<ApiResult<string>> Delete(int commmentId);
        Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId,int pageIndex = 0, int pageSize = 0);
    }
}
