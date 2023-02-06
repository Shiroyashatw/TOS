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
    public class UserItemBagController : ControllerBase
    {
        private readonly TosDBContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserItemBagController(TosDBContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("{id}")]
        public ActionResult<Item> Getitem(int id)
        {
            //var Claim = _contextAccessor.HttpContext.User.Claims.ToList();

            //var username = Claim.Where(a => a.Type == "UserAccount").First().Value;

            
            var res = (from item in _db.Items
                      join c in _db.Cards
                      on item.Cardid equals c.Cardid
                      join a in _db.AttrTables
                      on c.Attri equals a.AttrName
                      where item.Userid == id
                      select new
                      {
                          Cardid = item.Cardid,
                          CardName = c.CradName,
                          BigImg = c.BigImg,
                          LittleImg = c.LittleImg,
                          AttriImg = a.AttrImg,
                          FirstSkillName = (from s in _db.Skills
                                            where s.Skillid == item.Firstskill
                                            select s.Skillname
                                             ).FirstOrDefault(),
                          SecondSkillName = (from s in _db.Skills
                                             where s.Skillid == item.Secondskill
                                             select s.Skillname).FirstOrDefault(),
                      }).ToList();
            
            //List<ItemBagDto> itemBags = new List<ItemBagDto>();

            //foreach (var item in res)
            //{
            //    itemBags.Add(new ItemBagDto
            //    {
            //        Cardid = item.Cardid,
                   
            //    });
            //}
            return Ok(res);
        }
    }
}
