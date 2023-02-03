using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TOS.Controllers.LoginController;
using System.Collections.Generic;
using TOS.Models;
using TOS.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace TOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawcardController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public DrawcardController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("id")]
        public ActionResult<DrawCardListDto> DrawCard(int id)
        {
            var user = (from u in _db.Users
                       where u.Userid == id
                       select u).FirstOrDefault();
            
            // 後端讀取當玩家石頭不夠返回前端
            if(user.UserMagicstone < 50)
            {
                return NotFound("魔法石不足十連抽");
            }
            
            // 回傳抽到的卡片列表
            List<DrawCardListDto> drawCardList = new List<DrawCardListDto>();

            // 宣告一個 List 接 稀有度卡池
            List<string> list = new List<string>();

            // 稀有度權重
            int r = 160;
            int sr = 30;
            int ssr = 10;

            var random = new Random();

            for (int i = 0; i < r; i++)
            {
                list.Add("R");
            }
            for (int i = 0; i < sr; i++)
            {
                list.Add("SR");
            }
            for (int i = 0; i < ssr; i++)
            {
                list.Add("SSR");
            }

            // 進行抽卡循環
            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(list.Count);

                string rare = list[index];

                var CardList = Rare(rare);
                // var attri = CardList.Value.Attri;
                var SkillList = Skill(CardList.Value.Cardid);

                // SkillList.Value[0].skillname

                drawCardList.Add(new DrawCardListDto
                {
                    Cardid = CardList.Value.Cardid,
                    CradName = CardList.Value.CradName,
                    Cardrare = CardList.Value.Cardrare,
                    Race = CardList.Value.Race,
                    Attri = CardList.Value.Attri,
                    FirskillName = SkillList.Value[0].Skillname,
                    SecondSkillName = SkillList.Value[1].Skillname,
                    BigImg= CardList.Value.BigImg,
                    LittleImg= CardList.Value.LittleImg,
                });

                Item insert = new Item
                {
                    Userid = user.Userid,
                    Cardid = CardList.Value.Cardid,
                    Firstskill = SkillList.Value[0].Skillid,
                    FirstskillLv = 1,
                    Secondskill = SkillList.Value[1].Skillid,
                    SecondskillLv = 1,
                    Itemstate = true
                };
                _db.Items.Add(insert);
                
            }
            user.UserMagicstone = user.UserMagicstone - 50;
            _db.SaveChanges();
            
            return Ok(drawCardList);
        }
        // 傳入抽到的稀有度 傳回 對應稀有度的卡片資訊
        private ActionResult<Card> Rare(string rare)
        {
            var res = (from r in _db.Cards
                      where r.Cardrare == rare
                      select r).ToList();
            
            var x = res.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            
            return x;
        }
        // 抽取技能列表 傳入卡片ID
        private ActionResult<List<Skill>> Skill(int id)
        {
            var res = (from s in _db.Skills
                       where s.Inherent == false || s.Cardid == id
                       select s);

            var x = res.OrderBy(a => Guid.NewGuid()).Take(2).ToList();

            return x;
        }
    }
}
