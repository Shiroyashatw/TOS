using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOS.Models;
using Microsoft.AspNetCore.Authorization;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetInfo()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "FullName").First().Value;

            var res = from u in _db.Users
                      where u.Username == username
                      select u;

            return await res.ToListAsync();

        }


    }
}
