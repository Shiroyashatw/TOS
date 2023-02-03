using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TOS.Models;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserItemBagController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserItemBagController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("{id}")]
        public ActionResult<Item> Getitem(int id)
        {
            //var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            //var username = Claim.Where(a => a.Type == "UserAccount").First().Value;

            
            var res = (from item in _db.Items
                      where item.Userid == id
                      select item).ToList();

            return Ok(res);
        }
    }
}
