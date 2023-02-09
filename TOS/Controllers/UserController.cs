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
        public ActionResult<ExchangeTable> PostData(ExchangeTableDto exchangeTableDtos)
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
        [HttpGet]
        [Route("/Can")]
        public ActionResult<ExchangeTable> GetCanChangeData()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = (from u in _db.Users
                      where u.Username == username
                      select u).SingleOrDefault();
            // 撈出我擁有的卡
            var userHaveCard = (from e in _db.ExchangeTables
                            where e.UserId == res.Userid && e.CardState == 0
                            select e.CardId);
            var userWantCard = (from e in _db.ExchangeTables
                                where e.UserId == res.Userid && e.CardState == 3
                                select e.CardId);
            List<int> myHaveCardList = new List<int>();
            foreach (var item in userHaveCard)
            {
                myHaveCardList.Add(item);
            }
            List<int> myWantCardList = new List<int>();
            foreach (var item in myWantCardList)
            {
                myWantCardList.Add(item);
            }
            // 撈出總表 篩選出全部玩家想要交換的卡 where e.CardId == userHaveCard.Cardid 就能撈出第一階段 我有的卡別人想要
            var data = from e in _db.ExchangeTables
                       where (e.CardState == 3 && 
                       (e.CardId == myHaveCardList[0] 
                       || e.CardId == myHaveCardList[1]
                       || e.CardId == myHaveCardList[2]
                       || e.CardId == myHaveCardList[3]
                       || e.CardId == myHaveCardList[4]
                       )) 
                       //&& 
                       //(e.CardState == 1 &&
                       //(e.CardId == myWantCardList[0]
                       //|| e.CardId == myWantCardList[1]
                       //|| e.CardId == myWantCardList[2]
                       //|| e.CardId == myWantCardList[3]
                       //|| e.CardId == myWantCardList[4]
                       //))
                       select new
                       {
                           CardListId = e.CardListId,
                       };
            //userdata.Select
            //var data = from e in _db.ExchangeTables
            //           where e.CardState == 3 && e.Ca

            return Ok(data.ToList());
        }
    }
}
