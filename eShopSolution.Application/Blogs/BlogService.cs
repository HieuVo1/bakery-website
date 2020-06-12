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
                    UserLikeId = x.l.UserId.ToString()
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
                         UserLikeId = x.l.UserId.ToString()
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

        }

        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetById(int blogId)
        {
            var query = from p in _context.Blogs where p.Id == blogId
                        join us in _context.Users on p.UserId equals us.Id
                        join c in _context.CategoryTranslations on p.CategoryId equals c.CategoryId
                        where c.LanguageId == "vn"
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
                   UserLikeId = x.l.UserId.ToString()
               }).ToListAsync();
            var pageViewModel = new PageViewModel<BlogViewModel>()
            {
                TotalRecords = 1,
                Items = data,
                PageSize =0,
                PageIndex = 0
            };
            return new ApiResultSuccess<PageViewModel<BlogViewModel>>(pageViewModel);
        }

        public async Task<ApiResult<bool>> Like(LikeCreateRequest request)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.BlogId == request.BlogId && x.UserId==new Guid(request.UserId));
            if (like == null)
            {
                var newLike = new Like{
                    BlogId=request.BlogId,
                    UserId = new Guid(request.UserId)
                };
                _context.Likes.Add(newLike);
                await _context.SaveChangesAsync();
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("You liked  this blog");
        }
        public async Task<ApiResult<bool>> DisLike(LikeCreateRequest request)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.BlogId == request.BlogId && x.UserId == new Guid(request.UserId));
            if (like == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
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
