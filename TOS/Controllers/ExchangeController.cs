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
                               UserAccountInfo = u.AccountInfo,
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
        
        [HttpGet]
        [Route("Can")]
        public ActionResult<ExchangeTable> GetCanChangeData()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = (from u in _db.Users
                       where u.Username == username && u.BackupState != null
                       select u).SingleOrDefault();
                
            if (res == null) return NotFound("尚未設定卡片");
            // 撈出我擁有的卡
            var userHaveCard = (from e in _db.ExchangeTables
                                where e.UserId == res.Userid && e.CardState == 0
                                select e.CardId);
            // 撈出我想交換的卡
            var userWantCard = (from e in _db.ExchangeTables
                                where e.UserId == res.Userid && e.CardState == 3
                                select e.CardId);
            // 分別放入 List中
            List<short> myHaveCardList = new List<short>();
            foreach (var item in userHaveCard)
            {
                myHaveCardList.Add(item);
            }
            List<short> myWantCardList = new List<short>();
            foreach (var Watnitem in userWantCard)
            {
                myWantCardList.Add(Watnitem);
            }
            // 撈出總表 篩選出全部玩家想要交換的卡 where e.CardId == userHaveCard.Cardid 就能撈出第一階段 我有的卡別人想要
            var data = (from u in _db.Users
                       where u.Userid != res.Userid
                       select new
                       {
                           userId = u.Userid,
                           userName = u.Username,
                           userBackupState = u.BackupState,
                           //撈出其他玩家持有的卡片有符合自己想交換的
                           CanGetCard = (from e in _db.ExchangeTables
                                         join c in _db.CardListTables
                                         on e.CardId equals c.CardId
                                         join user in _db.Users
                                         on e.UserId equals user.Userid
                                         where e.UserId != res.Userid && e.CardState == 0
                                         && (e.CardId == myWantCardList[0]
                                                 || e.CardId == myWantCardList[1]
                                                 || e.CardId == myWantCardList[2]
                                                 || e.CardId == myWantCardList[3]
                                                 || e.CardId == myWantCardList[4]
                                                 )
                                         select new
                                         {
                                             userId = e.UserId,
                                             cardListId = e.CardListId,
                                             cardId = e.CardId,
                                             cardName = c.CardName,
                                             cardImg = c.CardImg,
                                         }
                                         ).Where(x => x.userId == u.Userid).ToList(),
                           //撈出其他玩家想交換的卡片有符合自己持有的
                           CanPostCard = (from ex in _db.ExchangeTables
                                          join c in _db.CardListTables
                                          on ex.CardId equals c.CardId
                                          where ex.UserId != res.Userid
                                          && ex.CardState == 3
                                          && (ex.CardId == myHaveCardList[0]
                                          || ex.CardId == myHaveCardList[1]
                                          || ex.CardId == myHaveCardList[2]
                                          || ex.CardId == myHaveCardList[3]
                                          || ex.CardId == myHaveCardList[4]
                                          )
                                          select new
                                          {
                                              cardListId = ex.CardListId,
                                              userId = ex.UserId,
                                              cardId = ex.CardId,
                                              cardName = c.CardName,
                                              cardImg = c.CardImg
                                          }
                                       ).Where(x => x.userId == u.Userid).ToList()
                       }).Where(i => i.CanGetCard.Count() != 0 && i.CanPostCard.Count() != 0);
            return Ok(data.ToList());
        }
    }
}
