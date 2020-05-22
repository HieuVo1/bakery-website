using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IO;
using eShopSolution.Application.Comom;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.Utilities.functions;

namespace eShopSolution.Application.Catelog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

       

        public Task AddViewCount(int ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                CategoryId = request.CategoryId,
                Stock = request.Stock,
                Created_At = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name=request.Name,
                        Description=request.Description,
                        ProductUrl=GetUrlByName.Converts(request.Name),
                        LanguageId=request.LanguageId,
                        
                    }
                },

            };
            //Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,

                    }
                };
            }
            _context.Products.Add(product);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int ProductId)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) new ApiResultErrors<bool>($"Cannot find  a product:{ProductId}");

            var images = _context.ProductImages.Where(i => i.ProductId == ProductId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);

            return await SaveChangeService.SaveChangeAsyncNotImage(_context);

        }


        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryUrl(GetProductPaggingRequest request, string LanguageId)
        {
            //Select
            var query = from p in _context.Products
                        join img in _context.ProductImages on p.Id equals img.ProductId into imgNull
                        from m in imgNull.DefaultIfEmpty() 
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join l in _context.Languages on pt.LanguageId equals l.Id
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.CategoryUrl == request.CategoryUrl  &&
                        pt.LanguageId == LanguageId 
                        select new { p, pt, c,l,m,ct};
            //filter
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }


            //Pagging
            int totalRow = await query.CountAsync();
            if (request.PageIndex == 0 || request.PageSize == 0)
            {
                var data = await query
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Created_At = x.p.Created_At,
                    Description = x.pt.Description,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    CategoryId = x.c.Id,
                    ProductUrl = x.pt.ProductUrl,
                    Language = x.l.Name,
                    ImagePath=x.m.ImagePath,
                    categoryUrl=x.ct.CategoryUrl
                }).ToListAsync();
                //Select and  projection
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data
                };

                return new ApiResultSuccess<PageViewModel<ProductViewModel>>(pageViewModel);
            }
            else
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Created_At = x.p.Created_At,
                    Description = x.pt.Description,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    CategoryId = x.c.Id,
                    ProductUrl = x.pt.ProductUrl,
                    Language = x.l.Name,
                    ImagePath = x.m.ImagePath
                }).ToListAsync();
                //Select and  projection
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data
                };

                return new ApiResultSuccess<PageViewModel<ProductViewModel>>(pageViewModel);
            }
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllPagging(GetProductPaggingRequest request)
        {
            //Select
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join img in _context.ProductImages on p.Id equals img.ProductId
                        join c in _context.Categories on p.CategoryId equals c.Id
                        where pt.LanguageId
                        == request.LanguageId
                        select new { p, pt, c, img };
            //filter
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.MinPrice != 0 || request.MaxPrice != 0)
            {
                query = query.Where(x => (x.p.Price >= request.MinPrice && x.p.Price <= request.MaxPrice));
            }
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => request.CategoryId == p.c.Id);
            }
            //Pagging
            int totalRow = await query.CountAsync();
            if (request.PageIndex == 0 || request.PageSize == 0)
            {
                var data = await query
               .Select(x => new ProductViewModel()
               {
                   Id = x.p.Id,
                   Name = x.pt.Name,
                   Created_At = x.p.Created_At,
                   Description = x.pt.Description,
                   LanguageId = x.pt.LanguageId,
                   OriginalPrice = x.p.OriginalPrice,
                   Price = x.p.Price,
                   Stock = x.p.Stock,
                   CategoryId = x.c.Id,
                   ProductUrl = x.pt.ProductUrl,
                   ImagePath = x.img.ImagePath
               }).ToListAsync();
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                };
                return new ApiResultSuccess<PageViewModel<ProductViewModel>>(pageViewModel);
            }
            else
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Created_At = x.p.Created_At,
                    Description = x.pt.Description,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    CategoryId = x.c.Id,
                    ProductUrl = x.pt.ProductUrl,
                    ImagePath = x.img.ImagePath
                }).ToListAsync();
                //Select and  projection
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                };
                return new ApiResultSuccess<PageViewModel<ProductViewModel>>(pageViewModel);
            }

        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);
            var imageproduct = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId);

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Created_At = product.Created_At,
                ProductUrl = productTranslation != null ? productTranslation.ProductUrl : null,
                ImagePath =imageproduct!=null? imageproduct.ImagePath:null
            };
            return new ApiResultSuccess<ProductViewModel>(productViewModel);
        }
        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) return new ApiResultErrors<bool>($"Cannot find a product with id: {request.Id}");

            productTranslation.Name = request.Name;
            productTranslation.ProductUrl = GetUrlByName.Converts(request.Name);
            productTranslation.Description = request.Description;
            //Save Image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    var OldImagePath = thumbnailImage.ImagePath;
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    if (OldImagePath != null)
                    {
                        await _storageService.DeleteFileAsync(OldImagePath);
                    }
                    _context.ProductImages.Update(thumbnailImage);
                }
                else
                {
                    var image = new ProductImage()
                    {
                        ProductId = request.Id,
                        IsDefault = false,
                        Caption = "ThumbanailImage",
                    };
                    image.FileSize = request.ThumbnailImage.Length;
                    image.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Add(image);

                }
            }

            return await SaveChangeService.SaveChangeAsyncNotImage(_context);

        }

        public async Task<ApiResult<bool>> UpdatePrice(int ProductId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) return new ApiResultErrors<bool>($"Cannot find a product with id: {ProductId}");
            product.Price = newPrice;
            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }

        public async Task<ApiResult<bool>> UpdateStock(int ProductId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) return new ApiResultErrors<bool>($"Cannot find a product with id: {ProductId}");
            product.Stock += addedQuantity;
            await _context.SaveChangesAsync() ;
            return new ApiResultSuccess<bool>();
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
