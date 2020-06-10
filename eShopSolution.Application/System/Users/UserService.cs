using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
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
        private readonly EShopDbContext _context;

        public UserService(UserManager<UserApp> userManager,
            SignInManager<UserApp> signInManager,
            RoleManager<RoleApp> roleManager,
            IConfiguration configuration,
            EShopDbContext context,
            IStorageService storageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _storageService = storageService;
            _context = context;
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiResultErrors<string>("UserName not found");
            var result = await _signInManager.PasswordSignInAsync(user, request.Passwork, request.RememberMe, true);
            if (result.IsLockedOut) return new ApiResultErrors<string>("You login faild more five. Your Account is locked");
            if (!result.Succeeded) return new ApiResultErrors<string>("UserName or Password is incorrect");
            var role = await _roleManager.FindByIdAsync(user.RoleID.ToString());
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var userToken = new UserToken
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                Role = string.Join(";", role.Name),
                ImagePath = user.ImagePath,
                Email = user.Email,
                CartId = cart.Id.ToString(),
            };
            var token = CreateToken(userToken);
            return new ApiResultSuccess<string>(token);

        }

        public async Task<ApiResult<bool>> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) new ApiResultErrors<bool>($"Can not find user with id: {userId}");
            var result = await _userManager.DeleteAsync(user);
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
                Address = user.Address,
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
                Address = user.Address,
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
                Address = user.Address,
                Dob = user.Dob,
                Id = user.Id,
                ImagePath = user.ImagePath,
                RoleId = user.RoleID
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public async Task<ApiResult<string>> ExternalLoginCallback(ExternalLoginRequest request)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(request.LoginProvider,
                request.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var role = await _roleManager.FindByIdAsync(user.RoleID.ToString());
                var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == user.Id);
                var userToken = new UserToken
                {
                    CartId = cart.Id.ToString(),
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Role = string.Join(";", role.Name),
                    ImagePath = user.ImagePath,
                    Email = user.Email,
                };
                var token = CreateToken(userToken);
                return new ApiResultSuccess<string>(token);
            }
            else
            {
                var email = request.Email;
                if (email != null)
                {

                    var user = await _userManager.FindByEmailAsync(email);
                    var roleDefault = await _roleManager.FindByNameAsync("client");

                    if (user == null)
                    {
                        user = new UserApp
                        {
                            FullName = request.FullName,
                            ImagePath = request.ImagePath,
                            Email = request.Email,
                            RoleID = roleDefault.Id,
                            UserName = request.Email,

                        };
                        var result = await _userManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            var cartadd = new Cart
                            {
                                Created_At = DateTime.Now,
                                UserId = user.Id,
                                Price = 0,
                                CartProducts = new List<CartProduct>(),
                            };
                            _context.Carts.Add(cartadd);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    var info = new UserLoginInfo(request.LoginProvider, request.ProviderKey, request.ProviderDisPlayName);
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var role = _roleManager.FindByIdAsync(user.RoleID.ToString());
                    var userToken = new UserToken
                    {
                        CartId = cart.Id.ToString(),
                        UserId = user.Id.ToString(),
                        UserName = user.UserName,
                        Role = string.Join(";", role.Result.Name),
                        ImagePath = user.ImagePath,
                        Email = user.Email,
                    };
                    var token = CreateToken(userToken);
                    return new ApiResultSuccess<string>(token);
                }
                return new ApiResultErrors<string>($"Email claim not received from:{request.LoginProvider}");
            }
        }

        public async Task<ApiResult<PageViewModel<RoleViewModel>>> GetListRole()
        {
            var query = _roleManager.Roles;
            int totalRow = await query.CountAsync();
            var data = await query
                .Select(x => new RoleViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Id = x.Id
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
            if (request.PageIndex != 0 || request.PageSize != 0)
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Email = x.u.Email,
                    FullName = x.u.FullName,
                    Phone = x.u.PhoneNumber,
                    Address = x.u.Address,
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
            else
            {
                var data = await query
                    .Select(x => new UserViewModel()
                    {
                        Email = x.u.Email,
                        FullName = x.u.FullName,
                        Phone = x.u.PhoneNumber,
                        Address = x.u.Address,
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


        public async Task<ApiResult<VerificationViewModel>> Register(RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new ApiResultErrors<VerificationViewModel>("UserName is already");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiResultErrors<VerificationViewModel>("Email is already");
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
                Address = request.Address,
                UserName = request.UserName,
                FullName = request.FullName,
                RoleID = request.RoleId
            };


            //Save Image
            if (request.ThumbnailImage != null)
            {
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            var result = await _userManager.CreateAsync(user, request.Passwork);


            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                VerificationViewModel model = new VerificationViewModel
                {
                    Token = token,
                    UserId = user.Id,
                };
                var cart = new Cart
                {
                    Created_At = DateTime.Now,
                    UserId = user.Id,
                    Price = 0,
                    CartProducts = new List<CartProduct>(),
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                return new ApiResultSuccess<VerificationViewModel>(model);
            }
            return new ApiResultErrors<VerificationViewModel>("Register failed");

        }

        public async Task<ApiResult<bool>> Update(Guid userId, UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ApiResultErrors<bool>($"Can not find user with id: {userId}");

            user.FullName = request.FullName;
            user.PhoneNumber = request.Phone;

            //Save Image
            if (request.ThumbnailImage != null)
            {
                var OldImagePath = user.ImagePath;
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
                if (OldImagePath != null)
                {
                    await _storageService.DeleteFileAsync(OldImagePath);
                }

            }

            var result = await _userManager.UpdateAsync(user);
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
            return _storageService.GetFileUrl(fileName);
        }

        public async Task<ApiResult<bool>> ConfirmEmail(VerificationViewModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new ApiResultErrors<bool>($"Can not find user with id: {request.UserId}");
            }
            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Confirm email faild");
        }

        public async Task<ApiResult<string>> GetPasswordResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return new ApiResultSuccess<string>(token);
            }
            return new ApiResultErrors<string>($"User not found");
        }

        public async Task<ApiResult<bool>> ResetPassword(ResetPasswordViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ApiResultErrors<bool>("Can not found user");
            }
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
            {
                if (await _userManager.IsLockedOutAsync(user))
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                }
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Reset Password faild");
        }

        public async Task<ApiResult<string>> ChangePassword(ChangePasswordViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ApiResultErrors<string>("Can not found user");
            }
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                var result = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (result.Succeeded)
                {
                    var role = _roleManager.FindByIdAsync(user.RoleID.ToString());
                    var userToken = new UserToken
                    {
                        UserId = user.Id.ToString(),
                        UserName = user.UserName,
                        Role = string.Join(";", role.Result.Name),
                        ImagePath = user.ImagePath,
                        Email = user.Email,
                    };
                    var token = CreateToken(userToken);
                    return new ApiResultSuccess<string>(token);
                }
                return new ApiResultErrors<string>("Change Password faild");
            }
            var change = await _userManager.ChangePasswordAsync(user, request.CurentPassword, request.NewPassword);
            if (change.Succeeded)
            {
                var role = _roleManager.FindByIdAsync(user.RoleID.ToString());
                var userToken = new UserToken
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Role = string.Join(";", role.Result.Name),
                    ImagePath = user.ImagePath,
                    Email = user.Email,
                };
                var token = CreateToken(userToken);
                return new ApiResultSuccess<string>(token);
            }
            string errors = string.Empty;
            foreach (var error in change.Errors)
            {
                errors += error.Description;
            }
            return new ApiResultErrors<string>(errors);
        }
        public string CreateToken(UserToken user)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Picture", (user.ImagePath!=null?user.ImagePath:"")),
                new Claim("CartId", user.CartId),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResult<bool>> UserUpdateProfile(Guid userId, UpdateProfile request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ApiResultErrors<bool>($"Can not find user with id: {userId}");

            user.FullName = request.FullName;
            user.PhoneNumber = request.Phone;
            user.Address = request.Address;
            user.Dob = request.Dob;

            //Save Image
            if (request.ThumbnailImage != null)
            {
                var OldImagePath = user.ImagePath;
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
                if (OldImagePath != null)
                {
                    await _storageService.DeleteFileAsync(OldImagePath);
                }

            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("update failed");
        }
    }
}
