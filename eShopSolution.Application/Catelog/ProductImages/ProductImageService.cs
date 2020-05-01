using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.ProductImages
{
    public class ProductImageService : IProductImageService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductImageService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> AddImage(int ProductId, ProductImageCreateRequest request)
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

            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
        public async Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) return new ApiResultErrors<ProductImageViewModel>($"Can not find image with id: {imageId}");

            var productimageViewModel = new ProductImageViewModel()
            {
                Id = image.Id,
                Caption = image.Caption,
                IsDefault = image.IsDefault,
                ImagePath = image.ImagePath,
                FileSize = image.FileSize,
                ProductId = image.ProductId
            };
            return new ApiResultSuccess<ProductImageViewModel>(productimageViewModel);
        }

        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId)
        {
            var data= await _context.ProductImages.Where(i => i.ProductId == ProductId)
                .Select(i => new ProductImageViewModel()
                {
                    FileSize = i.FileSize,
                    Id = i.Id,
                    IsDefault = i.IsDefault,
                    Caption = i.Caption,
                    ImagePath = i.ImagePath,
                    ProductId = i.ProductId
                }).ToListAsync();
            return new ApiResultSuccess<List<ProductImageViewModel>>(data);
        }


        public async Task<ApiResult<bool>> RemoveImage(int ImageId)
        {
            var image = await _context.ProductImages.FindAsync(ImageId);
            if (image == null) return new ApiResultErrors<bool>($"Can not find image with id: {ImageId}");
            _context.ProductImages.Remove(image);
            return await SaveChangeService.SaveChangeAsyncImage(_context,image.ImagePath,_storageService);
        }

       

        public async Task<ApiResult<bool>> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) return new ApiResultErrors<bool>($"can not find image with id: {imageId}");
            image.Caption = request.Caption;
            image.IsDefault = request.IsDefault;
            if (request.ThumbnailImage != null)
            {
                var oldImagePath = image.ImagePath;
                image.FileSize = request.ThumbnailImage.Length;
                image.ImagePath = await this.SaveFile(request.ThumbnailImage);
                await _storageService.DeleteFileAsync(oldImagePath);
            }
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);

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
