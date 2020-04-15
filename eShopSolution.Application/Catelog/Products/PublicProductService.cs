using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;
        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join c in _context.Categories on p.CategoryId equals c.Id
                        select new { p, pt, c };
            //Pagging
            var data = await query.Select(x => new ProductViewModel()
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
            return data;
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(GetProductPublicPaggingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join c in _context.Categories on p.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keywork)
                        select new { p, pt, c };
            //filter
            if (!String.IsNullOrEmpty(request.Keywork))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keywork));
            }
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => request.CategoryId == p.c.Id);
            }
            //Pagging
            int totalRow = await query.CountAsync();
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
}
