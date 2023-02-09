using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TOS.Dtos;
namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        public ExchangeController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        public ActionResult<ExchangeTable> GetExchangeData()
        {
            var userList = from u in _db.Users
                           where u.BackupState != null
                           select new
                           {
                               UserName = u.Username,
                               UserBackState = u.BackupState,
                               HaveCard = (from e in _db.ExchangeTables
                                         join c in _db.CardListTables
                                         on e.CardId equals c.CardId
                                         where e.UserId == u.Userid && e.CardState == 0
                                         select new
                                         {
                                             CardId = e.CardId,
                                             CardName = c.CardName,
                                             Cardimg = c.CardImg
                                         }).ToList(),
                               WantCard = (from e in _db.ExchangeTables
                                                     join c in _db.CardListTables
                                                     on e.CardId equals c.CardId
                                                     where e.UserId == u.Userid && e.CardState == 3
                                                     select new
                                                     {
                                                         CardId = e.CardId,
                                                         CardName = c.CardName,
                                                         Cardimg = c.CardImg
                                                     }).ToList()
                           };
            
            return Ok(userList.ToList());
        }
    }
}
