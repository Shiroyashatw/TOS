using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TOS.Controllers.LoginController;
using System.Collections.Generic;
using TOS.Models;

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
            List<DataModel> datalist = new List<DataModel>();

            List<string> list = new List<string>();

            // 稀有度權重
            int r = 160;
            int sr = 30;
            int ssr = 10;


            //var rlist = (from c in _db.Cards
            //             where c.Cardrare == "R"
            //             select c).ToList();
            // rlist.ToList()[0].Cardrare = "R"
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

                int cardid = Rare(rare);

                fskill = random.Next(skilllist.Count);
                sskill = random.Next(skilllist.Count);

                datalist.Add(new DataModel
                {
                    cardid = cardid,
                    firstSkill = skilllist[fskill],
                    firstSkillLv = 1,
                    secondSkill = skilllist[sskill],
                    secondSkillLv = 1,
                });
            }

            // return new string[] { y };
            return datalist;
        }
        
        private int Rare(string rare)
        {
            var rareList = (from c in _db.Cards
                            where c.Cardrare == rare
                            select c).ToList();
            // (偽)隨機亂數
            var random = new Random();

            int index = random.Next(rareList.Count);

            int cardid = rareList[index].Cardid;

            return cardid;
        }
    }
}
