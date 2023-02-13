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
        public string GetInfo()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            // 查詢是否已設定列表
            var res = (from u in _db.Users
                       where u.Username == username && u.BackupState != null
                       select u).SingleOrDefault();

            if (res == null)
            {
                return "0";
            }
            else
            {
                return "1";
            }

        }
        [HttpGet]
        [Route("UserName")]
        [AllowAnonymous]
        public ActionResult GetUserName()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();
            if (Claim.Count() == 0) return Ok("未登入");
            
            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = (from u in _db.Users
                       where u.Username == username
                       select new
                       {
                           u.Username,
                           u.UserState
                       }).SingleOrDefault();
            if (res == null)
            {
                return Ok("未登入");
            }
            else
            {
                return Ok(res);
            }

        }
        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public ActionResult<ExchangeTableDto> GetUserExTableData(int id)
        {
            var userInfo = (from u in _db.Users
                           where u.BackupState != null && u.Userid == id
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
                           }).SingleOrDefault();
            
            if (userInfo == null) return NotFound("無對應玩家ID");

            return Ok(userInfo);
        }
        [HttpGet]
        [Route("Info")]
        public ActionResult<ExchangeTableDto> GetUserInfoData()
        {
            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            var res = (from u in _db.Users
                       where u.Username == username && u.BackupState != null
                       select u).SingleOrDefault();
            if(res == null)
            {
                return NotFound("查無該玩家");
            }
            var userInfo = (from u in _db.Users
                            where u.BackupState != null && u.Username == res.Username
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
                            }).SingleOrDefault();

            if (userInfo == null) return NotFound("查無該玩家");

            return Ok(userInfo);
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

            var changeList = from e in _db.ExchangeTables
                             where e.UserId == res.Userid
                             select e.CardListId;
                              
            if(changeList.Count() >= 10)
            {
                return NotFound("已進行過設置");
            }
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
        [HttpPatch]
        public ActionResult<UpdateChangeDto> UpdateChangeList(UpdateChangeDto update)
        {
            var total = update.UpdateChangeList.Length;

            var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var username = Claim.Where(a => a.Type == "UserName").First().Value;

            // 查詢是否已設定列表
            var res = (from u in _db.Users
                       where u.Username == username && u.BackupState != null
                       select u).SingleOrDefault();

            var changeList = from e in _db.ExchangeTables
                             where e.UserId == res.Userid && e.CardState == 0
                             select e;
            for (int i = 0; i < total; i++)
            {
                var up = changeList.Where(x => x.CardId == update.UpdateChangeList[i]).SingleOrDefault();
                up.CardState = 1;
                up.UpdateTime = DateTime.Now;
            }
            res.UserState += (short)total;
            _db.SaveChanges();
            return Ok("更新成功");
        } 
    }
}
