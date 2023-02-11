using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOS.Models;
using Microsoft.AspNetCore.Authorization;
using TOS.Dtos;

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

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = from u in _db.Users
                      where u.Username == username
                      select u;

            return await res.ToListAsync();

        }
        [HttpPost]
        public ActionResult<ExchangeTableDto> PostData(ExchangeTableDto exchangeTableDtos)
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = (from u in _db.Users
                      where u.Username == username
                      select u).SingleOrDefault();
            if (res.BackupState != null) return NotFound("已進行過設置");
            // 更新帳號 回鍋狀態跟聯絡管道
            res.BackupState = exchangeTableDtos.BackupState;
            res.AccountInfo = exchangeTableDtos.AccountInfo;
            

            for(int i = 0; i < exchangeTableDtos.HaveCard.Length; i++)
            {
                ExchangeTable exchangeTable = new ExchangeTable
                {
                    UserId = res.Userid,
                    CardId = exchangeTableDtos.HaveCard[i],
                    CardState = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,

                    
                };
                _db.ExchangeTables.Add(exchangeTable);

            }
            for (int i = 0; i < exchangeTableDtos.Wantcard.Length; i++)
            {
                ExchangeTable exchangeTable = new ExchangeTable
                {
                    UserId = res.Userid,
                    CardId = exchangeTableDtos.Wantcard[i],
                    CardState = 3,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                };
                _db.ExchangeTables.Add(exchangeTable);
            }
            
            _db.SaveChanges();
            return Content("123");
        }
        
    }
}
