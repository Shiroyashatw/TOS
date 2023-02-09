using System;
using System.Collections.Generic;

#nullable disable

namespace TOS.Models
{
    public partial class User
    {
        public int Userid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public int UserMoney { get; set; }
        public int UserSoul { get; set; }
        public int UserMagicstone { get; set; }
        public DateTime? UserSingupTime { get; set; }
        public short? UserState { get; set; }
        public bool? BackupState { get; set; }
        public string AccountInfo { get; set; }
    }
}
