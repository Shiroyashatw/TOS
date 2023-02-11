using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TOS.Dtos;
using TOS.Models;
using System.Linq;

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
            User insert = new User
            {
                Account = SingUpDtos.Account,
                Password = SingUpDtos.Password,
                Username = SingUpDtos.Username,
                UserSingupTime = System.DateTime.Now,
                UserState = 0
            };
            _db.Users.Add(insert);
            await _db.SaveChangesAsync();
            return Ok("註冊成功");
        }
    }
}
