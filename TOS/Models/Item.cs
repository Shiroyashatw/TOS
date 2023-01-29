using System;
using System.Collections.Generic;

#nullable disable

namespace TOS.Models
{
    public partial class Item
    {
        public int Itemid { get; set; }
        public int Userid { get; set; }
        public int Cardid { get; set; }
        public int Firstskill { get; set; }
        public int FirstskillLv { get; set; }
        public int Secondskill { get; set; }
        public int SecondskillLv { get; set; }
    }
}
