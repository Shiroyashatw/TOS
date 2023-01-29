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

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=TosDB;Integrated Security=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Cardrare)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("cardrare");

                entity.Property(e => e.CradName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cradName");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Firstskill).HasColumnName("firstskill");

                entity.Property(e => e.FirstskillLv).HasColumnName("firstskillLv");

                entity.Property(e => e.Secondskill).HasColumnName("secondskill");

                entity.Property(e => e.SecondskillLv).HasColumnName("secondskillLv");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Skillname);

                entity.ToTable("Skill");

                entity.Property(e => e.Skillname)
                    .HasMaxLength(50)
                    .HasColumnName("skillname");

                entity.Property(e => e.Cardid).HasColumnName("cardid");

                entity.Property(e => e.Inherent).HasColumnName("inherent");

                entity.Property(e => e.Skillid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("skillid");
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
