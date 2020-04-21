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
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int ProductId, ProductImageCreateRequest request)
        {

            var image = new ProductImage()
            {
                ProductId = ProductId,
                IsDefault = request.IsDefault,
                Caption = request.Caption
            };
            if (request.ThumbnailImage != null)
            {
                image.FileSize = request.ThumbnailImage.Length;
                image.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            _context.ProductImages.Add(image);

            await _context.SaveChangesAsync();
            return image.Id;

        }

        public Task AddViewCount(int ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(ProductCreateRequest request)
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
                        ProductUrl=GetUrlByName.converts(request.Name),
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
             await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int ProductId)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) throw new EShopException($"Cannot find  a product:{ProductId}");

            var images = _context.ProductImages.Where(i => i.ProductId == ProductId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();

        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllByCategoryUrl(GetProductManagePaggingRequest request, string LanguageId)
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
            if (!String.IsNullOrEmpty(request.Keywork))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keywork));
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
                    TotalRecord = totalRow,
                    Items = data
                };

                return pageViewModel;
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
                    TotalRecord = totalRow,
                    Items = data
                };

                return pageViewModel;
            }
        }

        public async Task<PageViewModel<ProductViewModel>> getAllPagging(GetProductManagePaggingRequest request)
        {
            //Select
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId where pt.LanguageId == request.languageId
                        join c in _context.Categories on p.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keywork)
                        select new { p, pt, c };
            //filter
            if (!String.IsNullOrEmpty(request.Keywork))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keywork));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.c.Id));
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
                    ProductUrl = x.pt.ProductUrl
                }).ToListAsync();
                //Select and  projection
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecord = totalRow,
                    Items = data
                };

                return pageViewModel;
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
                    ProductUrl = x.pt.ProductUrl
                }).ToListAsync();
                //Select and  projection
                var pageViewModel = new PageViewModel<ProductViewModel>()
                {
                    TotalRecord = totalRow,
                    Items = data
                };

                return pageViewModel;
            }
            
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
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
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) throw new EShopException($"Can not find image with id: {imageId}");

            var productimageViewModel = new ProductImageViewModel()
            {
                Id = image.Id,
                Caption=image.Caption,
                IsDefault=image.IsDefault,
                ImagePath=image.ImagePath,
                FileSize=image.FileSize,
                ProductId=image.ProductId
            };
            return productimageViewModel;
        }

        public async  Task<List<ProductImageViewModel>> GetListImage(int ProductId)
        {
            return await _context.ProductImages.Where(i => i.ProductId == ProductId)
                .Select(i=>new ProductImageViewModel() { 
                    FileSize= i.FileSize,
                    Id=i.Id,
                    IsDefault=i.IsDefault,
                    Caption = i.Caption,
                    ImagePath = i.ImagePath,
                    ProductId = i.ProductId
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int ImageId)
        {
            var image = await _context.ProductImages.FindAsync(ImageId);
            if (image == null) throw new EShopException($"Can not find image with id: {ImageId}");
            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            productTranslation.Name = request.Name;
            productTranslation.ProductUrl = request.ProductUrl;
            productTranslation.Description = request.Description;
            //Save Image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
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
                    if (request.ThumbnailImage != null)
                    {
                        image.FileSize = request.ThumbnailImage.Length;
                        image.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    }
                    _context.ProductImages.Add(image);

                }
            }

            return await _context.SaveChangesAsync();

        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if(image.Caption==request.Caption && image.IsDefault==request.IsDefault && request.ThumbnailImage == null)
            {
                return 1;
            }
            if (image == null) throw new EShopException($"can not find image with id: {imageId}");
            image.Caption = request.Caption;
            image.IsDefault = request.IsDefault;
            if (request.ThumbnailImage != null)
            {
                image.FileSize = request.ThumbnailImage.Length;
                image.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            return await _context.SaveChangesAsync();

        }

        public async Task<bool> UpdatePrice(int ProductId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {ProductId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int ProductId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {ProductId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }


        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
