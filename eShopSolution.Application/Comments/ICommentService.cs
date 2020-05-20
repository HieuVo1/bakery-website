using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Comments
{
    public interface ICommentService
    {
        Task<ApiResult<CommentViewModel>> GetById(int commentId);
        Task<ApiResult<bool>> Create(CommentCreateRequest request);
        Task<ApiResult<bool>> Update(CommentUpdateRequest request, int commentId);
        Task<ApiResult<bool>> Delete(int commentId);
        Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId);
    }
}
