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

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasMaxLength(10)
                    .HasColumnName("account")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.Token)
                    .HasMaxLength(10)
                    .HasColumnName("token")
                    .IsFixedLength(true);

                entity.Property(e => e.UserMagicstone).HasColumnName("user_magicstone");

                entity.Property(e => e.UserMoney).HasColumnName("user_money");

                entity.Property(e => e.UserSoul).HasColumnName("user_soul");

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
