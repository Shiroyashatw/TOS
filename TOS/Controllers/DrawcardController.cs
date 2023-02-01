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

        public class DataModel
        {
            public int cardid { get; set; }
            public int firstSkill { get; set; }
            public int firstSkillLv { get; set; }
            public int secondSkill { get; set; }
            public int secondSkillLv { get; set; }
        }
        [HttpGet]
        public List<DataModel> Test()
        {
            DrawCardList drawCardList = new DrawCardList();

            List<DataModel> datalist = new List<DataModel>();

            // 宣告一個 List 接 稀有度卡池
            List<string> list = new List<string>();

            // 稀有度權重
            int r = 160;
            int sr = 30;
            int ssr = 10;


            List<int> skilllist = new List<int> { 1, 2, 3, 4, 5, 6, 10, 20 };

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

            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(list.Count);
                int fskill;
                int sskill;
                string rare = list[index];

                var cardid = Rare(rare);

                fskill = random.Next(skilllist.Count);
                sskill = random.Next(skilllist.Count);

                datalist.Add(new DataModel
                {
                    //cardid = cardid,
                    firstSkill = skilllist[fskill],
                    firstSkillLv = 1,
                    secondSkill = skilllist[sskill],
                    secondSkillLv = 1,
                });
            }

            // return new string[] { y };
            return datalist;
        }
        // 傳入抽到的稀有度 傳回 對應稀有度 隨機 Cardid
        private ActionResult<IEnumerable<Card>> Rare(string rare)
        {
            var res = (from r in _db.Cards
                      where r.Cardrare == rare
                      select r).ToList().OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            
            var rareList = (from c in _db.Cards
                            where c.Cardrare == rare
                            select c).ToList();
            // (偽)隨機亂數
            var random = new Random();

            int index = random.Next(rareList.Count);

            int cardid = rareList[index].Cardid;

            return Ok(res);
        }
    }
}
