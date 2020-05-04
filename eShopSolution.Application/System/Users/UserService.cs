﻿using eShopSolution.Application.Comom;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly RoleManager<RoleApp> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;

        public UserService(UserManager<UserApp> userManager, 
            SignInManager<UserApp> signInManager, 
            RoleManager<RoleApp> roleManager, 
            IConfiguration configuration,
            IStorageService storageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _storageService = storageService;
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiResultErrors<string>("UserName not found");
            var result = await _signInManager.PasswordSignInAsync(user, request.Passwork, request.RememberMe, true);
            if (!result.Succeeded) return new ApiResultErrors<string>("UserName or Password is incorrect");
            var role = _roleManager.FindByIdAsync(user.RoleID.ToString());
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,string.Join(";",role.Result.Name)),
                new Claim(ClaimTypes.Name, request.UserName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new ApiResultSuccess<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) new ApiResultErrors<bool>($"Can not find user with id: {userId}");
            var result=await _userManager.DeleteAsync(user);
            await _storageService.DeleteFileAsync(user.ImagePath);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>(" Xóa không thành công");
        }

        public async Task<ApiResult<UserViewModel>> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResultErrors<UserViewModel>($"Can not find user with email: {email}");
            }
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                Dob = user.Dob,
                Id = user.Id,
                ImagePath = user.ImagePath,
                RoleId = user.RoleID
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
               return new ApiResultErrors<UserViewModel>($"Can not find user with id: {userId}");
            }
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                Dob = user.Dob,
                Id = user.Id,
                ImagePath = user.ImagePath,
                RoleId = user.RoleID
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public async Task<ApiResult<UserViewModel>> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiResultErrors<UserViewModel>($"Can not find user with userName: {userName}");
            }
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                Dob = user.Dob,
                Id = user.Id,
                ImagePath = user.ImagePath,
                RoleId = user.RoleID
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public async Task<ApiResult<PageViewModel<RoleViewModel>>> GetListRole()
        {
            var query = _roleManager.Roles;
            int totalRow = await query.CountAsync();
            var data = await query
                .Select(x => new RoleViewModel()
                {
                    Name=x.Name,
                    Description=x.Description,
                    Id=x.Id
                }).ToListAsync();
            var result = new PageViewModel<RoleViewModel>
            {
                TotalRecords = totalRow,
                Items = data
            };
            return new ApiResultSuccess<PageViewModel<RoleViewModel>>(result);
        }

        public async Task<ApiResult<PageViewModel<UserViewModel>>> GetListUser(GetUserPaggingRequest request)
        {
            var query = from u in _userManager.Users
                        join r in _roleManager.Roles on u.RoleID equals r.Id
                        select new { r, u };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();
            if(request.PageIndex!=0|| request.PageSize != 0)
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select( x => new UserViewModel()
                {
                    Email = x.u.Email,
                    FullName = x.u.FullName,
                    Phone = x.u.PhoneNumber,
                    UserName = x.u.UserName,
                    Dob = x.u.Dob,
                    Id = x.u.Id,
                    ImagePath = x.u.ImagePath,
                    Role = x.r.Name
                }).ToListAsync();

                var pageResult = new PageViewModel<UserViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                };
                return new ApiResultSuccess<PageViewModel<UserViewModel>>(pageResult);
            }
            else { 
                var data = await query
                    .Select(x => new UserViewModel()
                    {
                        Email = x.u.Email,
                        FullName = x.u.FullName,
                        Phone = x.u.PhoneNumber,
                        UserName = x.u.UserName,
                        Dob = x.u.Dob,
                        Id = x.u.Id,
                        ImagePath = x.u.ImagePath,
                        Role = x.r.Name
                    }).ToListAsync();

                var pageResult = new PageViewModel<UserViewModel>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                };
                return new ApiResultSuccess<PageViewModel<UserViewModel>>(pageResult);
            }
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null) {
                return new ApiResultErrors<bool>("UserName is already");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiResultErrors<bool>("Email is already");
            }
            var roleDefault = await _roleManager.FindByNameAsync("client");
            if (request.RoleId == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                request.RoleId = roleDefault.Id;
            }
            var user = new UserApp()
            {
                Dob = request.Dob,
                Email = request.Email,
                PhoneNumber = request.Phone,
                UserName = request.UserName,
                FullName = request.FullName,
                RoleID=request.RoleId
            };

            
            //Save Image
            if (request.ThumbnailImage != null)
            {
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            var result = await _userManager.CreateAsync(user, request.Passwork);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Register failed");

        }

        public async Task<ApiResult<bool>> Update(Guid userId, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != userId))
            {
                return new ApiResultErrors<bool>("Emai already");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null ) return new ApiResultErrors<bool>($"Can not find user with id: {userId}");

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.PhoneNumber = request.Phone;

            //Save Image
            if (request.ThumbnailImage != null)
            {
                var OldImagePath = user.ImagePath;
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
                await _storageService.DeleteFileAsync(OldImagePath);
            }

           var result= await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("update failed");
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
