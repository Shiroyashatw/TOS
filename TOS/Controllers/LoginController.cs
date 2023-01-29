using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOS.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // 開放不用登入驗證也可以呼叫
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        
        [HttpGet]
        public string Login()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            

            if (Claim.Count != 0)
            {
                return "已登入";
            }
            return "尚未登入";
        }
        [HttpPost]
        public string Login(User value)
        {
            //var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            //if (Claim != null)
            //{
            //    return "已登入";
            //}
            // 搜尋對應帳號密碼 因帳號是唯一值 故後面加上SingleOrDefault()
            var user = (from u in _db.Users
                        where u.Account == value.Account
                        && u.Password == value.Password
                        select u).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            {
                // 開始驗證
                var claims = new List<Claim>
                {
                    // 可自行設定 其他Controller地方可以相對使用
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("FullName", user.Username)
                };

                // 登入期限控制
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return "OK";
            };
        }
        // 登出
        [HttpDelete]
        public string Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "登出";
        }
        [HttpGet("NoLogin")]
        public string NoLogin()
        {
            return "尚未登入";
        }
        
    }
}
