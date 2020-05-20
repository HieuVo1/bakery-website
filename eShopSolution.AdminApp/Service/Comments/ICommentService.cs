using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Comments
{
    public interface ICommentService
    {
        Task<ApiResult<CommentViewModel>> GetById(int commentId);
        Task<ApiResult<string>> Create(CommentCreateRequest request);
        Task<ApiResult<string>> Update(CommentUpdateRequest request, int commentId);
        Task<ApiResult<string>> Delete(int commentId);
        Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId);
    }
}
