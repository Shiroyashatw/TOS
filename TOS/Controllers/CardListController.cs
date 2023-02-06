using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TOS.Dtos;
using TOS.Models;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardListController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public CardListController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        public ActionResult<CardListTable> GetCard()
        {
            var res = (from c in _db.CardListTables
                       select c).OrderBy(a => a.CardId);

            return Ok(res.ToList());
        }
        [HttpPost]
        public string PostData(User user)
        {
            return "1";
        }
    }
}
