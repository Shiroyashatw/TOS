using System;
using System.Collections.Generic;

#nullable disable

namespace TOS.Models
{
    public partial class Skill
    {
        public int Skillid { get; set; }
        public string Skillname { get; set; }
        public bool Inherent { get; set; }
        public int? Cardid { get; set; }
        public string Skillact { get; set; }
        public double? Skillint { get; set; }
        public string Skilldesc { get; set; }
    }
}
