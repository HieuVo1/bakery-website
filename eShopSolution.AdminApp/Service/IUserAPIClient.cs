using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service
{
    public interface IUserAPIClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
