using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TOS.Models
{
    public partial class TosDBContext : DbContext
    {
        public TosDBContext()
        {
        }

        public TosDBContext(DbContextOptions<TosDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AttrTable> AttrTables { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardListTable> CardListTables { get; set; }
        public virtual DbSet<ExchangeTable> ExchangeTables { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=TosDB;Integrated Security=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<AttrTable>(entity =>
            {
                entity.HasKey(e => e.AttrName);

                entity.ToTable("AttrTable");

                entity.Property(e => e.AttrName)
                    .HasMaxLength(10)
                    .HasColumnName("attr_Name")
                    .IsFixedLength(true);

                entity.Property(e => e.AttrId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("attr_id");

                entity.Property(e => e.AttrImg)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("attr_Img");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Attri)
                    .HasMaxLength(10)
                    .HasColumnName("attri");

                entity.Property(e => e.BigImg)
                    .HasMaxLength(30)
                    .HasColumnName("bigImg");

                entity.Property(e => e.Cardrare)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("cardrare");

                entity.Property(e => e.CradName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cradName");

                entity.Property(e => e.LittleImg)
                    .HasMaxLength(30)
                    .HasColumnName("littleImg");

                entity.Property(e => e.Race)
                    .HasMaxLength(10)
                    .HasColumnName("race");
            });

            modelBuilder.Entity<CardListTable>(entity =>
            {
                entity.HasKey(e => e.CardName);

                entity.ToTable("CardListTable");

                entity.Property(e => e.CardName)
                    .HasMaxLength(30)
                    .HasColumnName("card_name");

                entity.Property(e => e.CardId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("card_id");

                entity.Property(e => e.CardImg)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("card_img");

                entity.Property(e => e.CardNum).HasColumnName("card_num");
            });

            modelBuilder.Entity<ExchangeTable>(entity =>
            {
                entity.HasKey(e => e.CardListId);

                entity.ToTable("exchangeTable");

                entity.Property(e => e.CardListId).HasColumnName("cardListID");

                entity.Property(e => e.CardId).HasColumnName("cardID");

                entity.Property(e => e.CardState).HasColumnName("cardState");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createTime");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("updateTime");

                entity.Property(e => e.UserId).HasColumnName("userID");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Firstskill).HasColumnName("firstskill");

                entity.Property(e => e.FirstskillLv).HasColumnName("firstskillLv");

                entity.Property(e => e.Itemstate).HasColumnName("itemstate");

                entity.Property(e => e.Secondskill).HasColumnName("secondskill");

                entity.Property(e => e.SecondskillLv).HasColumnName("secondskillLv");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Skillname);

                entity.ToTable("Skill");

                entity.Property(e => e.Skillname)
                    .HasMaxLength(10)
                    .HasColumnName("skillname");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Inherent).HasColumnName("inherent");

                entity.Property(e => e.Skillact)
                    .HasMaxLength(2)
                    .HasColumnName("skillact")
                    .IsFixedLength(true);

                entity.Property(e => e.Skilldesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("skilldesc");

                entity.Property(e => e.Skillid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("skillid");

                entity.Property(e => e.Skillint).HasColumnName("skillint");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Account);

                entity.Property(e => e.Account)
                    .HasMaxLength(15)
                    .HasColumnName("account")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountInfo)
                    .HasMaxLength(150)
                    .HasColumnName("accountInfo");

                entity.Property(e => e.BackupState).HasColumnName("backupState");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(35)
                    .HasColumnName("password");

                entity.Property(e => e.Token)
                    .HasMaxLength(10)
                    .HasColumnName("token")
                    .IsFixedLength(true);

                entity.Property(e => e.UserMagicstone).HasColumnName("user_magicstone");

                entity.Property(e => e.UserMoney).HasColumnName("user_money");

                entity.Property(e => e.UserSingupTime)
                    .HasColumnType("datetime")
                    .HasColumnName("user_SingupTime");

                entity.Property(e => e.UserSoul).HasColumnName("user_soul");

                entity.Property(e => e.UserState).HasColumnName("user_state");

                entity.Property(e => e.Userid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("userid");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
