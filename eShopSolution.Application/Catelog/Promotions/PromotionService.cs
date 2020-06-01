using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly EShopDbContext _context;
        public PromotionService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(PromotionCreateRequest request)
        {
            var promotion = new Promotion()
            {
                Code = request.Code,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount= request.DiscountAmount,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            _context.Promotions.Add(promotion);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null) return new ApiResultErrors<bool>($"Can not find promotion with id: {promotionId}");
            _context.Promotions.Remove(promotion);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<List<PromotionViewModel>>> GetAll()
        {
            var query = from p in _context.Promotions
                        select new { p };
            int totalRow = await query.CountAsync();
            if (totalRow > 0)
            {
                var data = await query
                 .Select(x => new PromotionViewModel()
                 {
                     Id = x.p.Id,
                     Code = x.p.Code,
                     DiscountPercent = x.p.DiscountPercent,
                     DiscountAmount = x.p.DiscountAmount,
                     FromDate = x.p.FromDate,
                     ToDate = x.p.ToDate
                 }).ToListAsync();
                return new ApiResultSuccess<List<PromotionViewModel>>(data);
            }

            return new ApiResultErrors<List<PromotionViewModel>>("Not found");
        }

        public async Task<ApiResult<PromotionViewModel>> GetById(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null) return new ApiResultErrors<PromotionViewModel>($"Can not find promotion with id: {promotionId}");
           
            var promotionViewModel = new PromotionViewModel
            {
                Id = promotion.Id,
                Code = promotion.Code,
                DiscountPercent = promotion.DiscountPercent,
                DiscountAmount = promotion.DiscountAmount,
                FromDate = promotion.FromDate,
                ToDate = promotion.ToDate
            };

            return new ApiResultSuccess<PromotionViewModel>(promotionViewModel);
        }

        public async Task<ApiResult<PromotionViewModel>> GetByCode(string code)
        {
            var promotion = await _context.Promotions.FirstOrDefaultAsync(x=>x.Code.Equals(code));
            if (promotion == null) return new ApiResultErrors<PromotionViewModel>($"Can not find promotion with code: {code}");
            if((DateTime.Now>promotion.ToDate)||(DateTime.Now<promotion.FromDate)) {
                return new ApiResultErrors<PromotionViewModel>($"This code is out of date");
            }
            var promotionViewModel = new PromotionViewModel
            {
                Id = promotion.Id,
                Code = promotion.Code,
                DiscountPercent = promotion.DiscountPercent,
                DiscountAmount = promotion.DiscountAmount,
                FromDate = promotion.FromDate,
                ToDate = promotion.ToDate
            };

            return new ApiResultSuccess<PromotionViewModel>(promotionViewModel);
        }

        public async Task<ApiResult<bool>> Update(PromotionUpdateRequest request, int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null) return new ApiResultErrors<bool>($"Can not find promotion with id: {promotionId}");
            promotion.DiscountAmount = request.DiscountAmount;
            promotion.FromDate = request.FromDate;
            promotion.ToDate = request.ToDate;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
