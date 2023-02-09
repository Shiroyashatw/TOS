using System;
using System.Collections.Generic;

#nullable disable

namespace TOS.Models
{
    public partial class ExchangeTable
    {
        public int CardListId { get; set; }
        public int UserId { get; set; }
        public short CardId { get; set; }
        public short CardState { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
