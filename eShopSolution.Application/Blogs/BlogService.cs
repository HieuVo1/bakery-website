using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        private readonly UserManager<UserApp> _userManager;
        public BlogService(EShopDbContext context,
            IStorageService storageService,
            UserManager<UserApp> userManager)
        {
            _context = context;
            _storageService = storageService;
            _userManager = userManager;
        }
        public async Task<ApiResult<bool>> Create(BlogCreateRequest request)
        {
            var blog = new Blog()
            {
                Title = request.Title,
                Created_At = DateTime.Now,
                Content = request.Content,
                UserId = request.UserId,
                LikeCount = 0,
                CategoryId = request.CategoryId
            };
            //Save Image
            if (request.ThumbnailImage != null)
            {
                blog.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            _context.Blogs.Add(blog);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null) return new ApiResultErrors<bool>($"Can not find blog with id: {blogId}");
            _context.Blogs.Remove(blog);
            return await SaveChangeService.SaveChangeAsyncImage(_context, blog.ImagePath, _storageService);
        }

        public async Task<ApiResult<bool>> DisLike(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            blog.LikeCount--;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(GetBlogPaggingRequest request)
        {
            var query = from p in _context.Blogs
                        join us in _context.Users on p.UserId equals us.Id
                        join c in _context.CategoryTranslations on p.CategoryId equals c.CategoryId
                        where c.LanguageId == request.languageId
                        join l in _context.Likes on p.Id equals l.BlogId
                        into ps
                        from l in ps.DefaultIfEmpty()
                        select new
                        {
                            p,
                            us,
                            c,
                            l
                        };
            //filter
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Title.Contains(request.Keyword));
            }
            //filter
            if (!String.IsNullOrEmpty(request.CategoryUrl))
            {
                query = query.Where(x => x.c.CategoryUrl.Equals(request.CategoryUrl));
            }
            int totalRow = await query.CountAsync();

            //Pagging
            if (request.PageIndex == 0 || request.PageSize == 0)
            {
                var data = await query
                .Select(x => new BlogViewModel()
                {
                    Id = x.p.Id,
                    Content = x.p.Content,
                    Created_At = x.p.Created_At,
                    Title = x.p.Title,
                    LikeCount = x.p.LikeCount,
                    ImagePath = x.p.ImagePath,
                    UserName = x.us.UserName,
                    CategoryName = x.c.Name,
                    CategoryUrl = x.c.CategoryUrl,
                   
                }).ToListAsync();
                var pageViewModel = new PageViewModel<BlogViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                };
                return new ApiResultSuccess<PageViewModel<BlogViewModel>>(pageViewModel);
            }
            else
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                     .Select(x => new BlogViewModel()
                     {
                         Id = x.p.Id,
                         Content = x.p.Content,
                         Created_At = x.p.Created_At,
                         Title = x.p.Title,
                         LikeCount = x.p.LikeCount,
                         ImagePath = x.p.ImagePath,
                         UserName = x.us.UserName,
                         CategoryName = x.c.Name,
                         CategoryUrl = x.c.CategoryUrl,
                     }).ToListAsync();
                //if (!String.IsNullOrEmpty(request.Keyword))
                //{
                //    query = query.Where(x => x.blog.Title.Contains(request.Keyword));
                //}
                ////filter
                //if (!String.IsNullOrEmpty(request.CategoryUrl))
                //{
                //    query = query.Where(x => x.category.CategoryUrl.Equals(request.CategoryUrl));
                //}
                //int totalRow = await query.CountAsync();

                ////Pagging
                //if (request.PageIndex == 0 || request.PageSize == 0)
                //{
                //    var data = await query
                //    .Select(x => new BlogViewModel()
                //    {
                //        Id=x.blog.Id,
                //        Content = x.blog.Content,
                //        Created_At = x.blog.Created_At,
                //        Title = x.blog.Title,
                //        LikeCount=x.blog.LikeCount,
                //        ImagePath = x.blog.ImagePath,
                //        UserName = x.user.UserName,
                //        CategoryName = x.category.Name,
                //        CategoryUrl=x.category.CategoryUrl,
                //    }).ToListAsync();
                //    var pageViewModel = new PageViewModel<BlogViewModel>()
                //    {
                //        TotalRecords = totalRow,
                //        Items = data,
                //        PageSize = request.PageSize,
                //        PageIndex = request.PageIndex
                //    };
                //    return new ApiResultSuccess<PageViewModel<BlogViewModel>>(pageViewModel);
                //}
                //else
                //{
                //    var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                //         .Select(x => new BlogViewModel()
                //         {
                //             Id = x.blog.Id,
                //             Content = x.blog.Content,
                //             Created_At = x.blog.Created_At,
                //             Title = x.blog.Title,
                //             LikeCount = x.blog.LikeCount,
                //             ImagePath = x.blog.ImagePath,
                //             UserName = x.user.UserName,
                //             CategoryName = x.category.Name,
                //             CategoryUrl = x.category.CategoryUrl,
                //         }).ToListAsync();
                var pageViewModel = new PageViewModel<BlogViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                };
                return new ApiResultSuccess<PageViewModel<BlogViewModel>>(pageViewModel);
            }

        }

        public async Task<ApiResult<BlogViewModel>> GetById(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            var user = await _userManager.FindByIdAsync(blog.UserId.ToString());
            var category = await _context.CategoryTranslations.FirstOrDefaultAsync(x=>x.CategoryId==blog.CategoryId);
            var commentCount = _context.Comments.Where(c => c.BlogId == blogId).Count();
            var Liked =  _context.Likes.Where(x => x.BlogId == blogId);
            var data = await Liked.Select(x => x.UserId.ToString()).ToListAsync();
            var blogViewModel = new BlogViewModel()
            {
                Id = blog.Id,
                Content = blog.Content,
                Created_At = blog.Created_At,
                Title = blog.Title,
                LikeCount = blog.LikeCount,
                ImagePath = blog.ImagePath,
                UserName = user.UserName,
                CategoryName = category.Name,
                CountComment=commentCount,
                CategoryUrl =category.CategoryUrl,
            };
            return new ApiResultSuccess<BlogViewModel>(blogViewModel);
        }

        public async Task<ApiResult<bool>> Liked(LikeCreateRequest request)
        {
            var blog = await _context.Blogs.FindAsync(request.BlogId);
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.BlogId == request.BlogId && x.UserId==new Guid(request.UserId));
            if (like == null)
            {
                var newLike = new Like{
                    BlogId=request.BlogId,
                    UserId = new Guid(request.UserId)
                };
                _context.Likes.Add(newLike);
                blog.LikeCount++;
                return await SaveChangeService.SaveChangeAsyncNotImage(_context);
            }
            blog.LikeCount--;
            _context.Likes.Remove(like);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Update(BlogUpdateRequest request, int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);

            blog.Content = request.Content;
            blog.Title = request.Title;
            blog.CategoryId = request.CategoryId;
            //Save Image
            if (request.ThumbnailImage != null)
            {
                var OldImagePath = blog.ImagePath;
                blog.ImagePath = await this.SaveFile(request.ThumbnailImage);
                await _storageService.DeleteFileAsync(OldImagePath);
            }
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return _storageService.GetFileUrl(fileName);
        }
    }
}
