using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOS.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ForgetController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public ForgetController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpPost]
        public string CheckUser(User user)
        {
            var res = (from u in _db.Users
                       where u.Account == user.Account && u.Username == user.Username
                       select u.Username
                       ).SingleOrDefault();

            if(res == null)
            {
                return "無資料";
            }
            else
            {
                return "重設密碼";
            }
        }
        [HttpPatch]
        public string Reseat(User up)
        {
            var res = (from u in _db.Users
                       where u.Username == up.Username
                       select u).SingleOrDefault();
            if(res == null)
            {
                return "查無玩家";
            }

            var str = up.Password;
            var md5 = MD5code(str);
            res.Password = md5;
            _db.SaveChanges();
            return "Ok";
        }
        //MD5加密
        private String MD5code(String str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            Byte[] data = md5Hasher.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(str));
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
