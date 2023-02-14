using System;
using System.Collections.Generic;

#nullable disable

namespace TOS.Models
{
    public partial class CardListTable
    {
        public int CardId { get; set; }
        public int? CardNum { get; set; }
        public string CardName { get; set; }
        public string CardImg { get; set; }
    }
}
