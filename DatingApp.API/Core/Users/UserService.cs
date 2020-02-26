using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using DatingApp.API.Core.Base;
using DatingApp.API.Core.Common;
using DatingApp.API.Core.HashingHelper;
using DatingApp.API.Core.Users.DTO;
using DatingApp.API.Data;
using DatingApp.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Core.Users
{

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IHashingHelper _hashingHelper;
        private readonly IConfiguration _config;

        public UserService(DataContext context, IHashingHelper hashingHelper, IConfiguration config)
        {
            this._hashingHelper = hashingHelper;
            this._config = config;
            this._context = context;
        }
        public async Task<BaseReturnModel<bool>> Register(UserRegisterInputDTO userInfor)
        {
            var isExisting = await UserNameIsExisting(userInfor.Username);
            if (isExisting)
            {
                return BaseReturnModel<bool>.CreateFailInstance(false, CommonConstant.ERROR_UserNameIsAlreadyTaken);
            }
            else
            {
                User newUser = new User()
                {
                    UserName = userInfor.Username
                };
                byte[] passWordSalt, passWordHash;
                _hashingHelper.GeneratePassWordHash(userInfor.Password, out passWordHash, out passWordSalt);
                newUser.PassWordHash = passWordHash;
                newUser.PassWordSalt = passWordSalt;
                await _context.Set<User>().AddAsync(newUser);
                await _context.SaveChangesAsync();
                return BaseReturnModel<bool>.CreateSuccessInstance(Data: true, message: string.Empty);
            }
        }

        private async Task<bool> UserNameIsExisting(string userName)
        {
            return await _context.Set<User>()
                .Where(x => x.UserName == userName)
                .AnyAsync();
        }
        private async Task<bool> IsValidPassword(string userName, string passWord)
        {
            var user = await _context.Users.Where(c => c.UserName == userName)
            .Select(c => new { c.PassWordHash, c.PassWordSalt })
            .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception(CommonConstant.ERROR_UserNotFound);
            }

            var correctPassword = _hashingHelper.CheckPassword(passWord, user.PassWordHash, user.PassWordSalt);
            return correctPassword;
        }
        private async Task<AccessToken> GenerateAccessToken(string userName)
        {
            int id = await _context.Set<User>().Where(c => c.UserName == userName).Select(c => c.Id).FirstOrDefaultAsync();
            if (id == 0)
            {
                throw new Exception(CommonConstant.ERROR_UserNotFound);
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString()),
                new Claim(ClaimTypes.Name,userName)
            };
            string secretKey = _config.GetSection("AppSettings:LoginKey").Value;
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("Please add LoginKey in web.config");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AccessToken()
            {
                Token = tokenHandler.WriteToken(token)
            };

        }
        public async Task<BaseReturnModel<AccessToken>> Login(LoginDTO loginDTO)
        {
            string userName = loginDTO.Username;
            string passWord = loginDTO.Password;
            if (!await UserNameIsExisting(userName))
            {
                return BaseReturnModel<AccessToken>.CreateFailInstance(null, CommonConstant.ERROR_UserNotFound);
            }
            if (!await IsValidPassword(userName, passWord))
            {
                return BaseReturnModel<AccessToken>.CreateFailInstance(null, CommonConstant.ERROR_InvalidCredential);
            }
            var token = await GenerateAccessToken(userName);
            return BaseReturnModel<AccessToken>.CreateSuccessInstance(token, string.Empty);
        }
    }
}