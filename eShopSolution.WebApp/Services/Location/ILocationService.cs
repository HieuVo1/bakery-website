using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Location
{
    public interface ILocationService
    {
        Task<string> GetAll(string keyword);
    }
}
