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
    }
}
