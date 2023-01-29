using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TOS.Models;

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
        public async Task<ActionResult<User>> SingUp(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok("新增成功");
        }
    }
}
