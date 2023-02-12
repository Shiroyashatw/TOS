using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TOS.Dtos;
using TOS.Models;
using System.Linq;
using System.Text;
using System;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SingupController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public SingupController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<SingUpDto>> SingUp(SingUpDto SingUpDtos)
        {
            var userTable = (from u in _db.Users
                             where u.Account == SingUpDtos.Account
                             select u.Account
                             ).SingleOrDefault();

            if (userTable != null)
            {
                return NotFound("重複");
            }
            var str = SingUpDtos.Password;
            var md5 = MD5code(str);
            User insert = new User
            {
                Account = SingUpDtos.Account,
                Password = md5,
                Username = SingUpDtos.Username,
                UserSingupTime = System.DateTime.Now,
                UserState = 0
            };
            _db.Users.Add(insert);
            await _db.SaveChangesAsync();
            return Ok("註冊成功");
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
