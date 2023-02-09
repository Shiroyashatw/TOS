using System;

namespace TOS.Dtos
{
    public class ExchangeTableDto
    {
        public bool BackupState { get; set; }
        public short[] HaveCard { get; set; }
        public short[] Wantcard { get; set; }
        public string AccountInfo { get; set; }
    }
}
