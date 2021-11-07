using DataAccessLayer.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer
{
    public class ImdbContext : DbContext
    {
        private const string host = "rawdata.ruc.dk";
        private const string database = "raw4";
        private const string uid = "raw4";
        private const string password = "UXyNO(IR";
        public DbSet<User> User { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public DbSet<Title> Titles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql($"host={host};db={database};uid={uid};pwd={password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User mapping
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
            modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<User>().Property(x => x.UpdatedAt).HasColumnName("updated_at");

            // Search history mapping
            modelBuilder.Entity<SearchHistory>().ToTable("search_history");
            modelBuilder.Entity<SearchHistory>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<SearchHistory>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<SearchHistory>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
            modelBuilder.Entity<SearchHistory>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<SearchHistory>().Property(x => x.SearchText).HasColumnName("search_text");

            // Title mapping
            modelBuilder.Entity<Title>().ToTable("titles");
            modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Title>().Property(x => x.TitleType).HasColumnName("title_type");
            modelBuilder.Entity<Title>().Property(x => x.PrimaryTitle).HasColumnName("primary_title");
            modelBuilder.Entity<Title>().Property(x => x.OriginalTitle).HasColumnName("original_title");
            modelBuilder.Entity<Title>().Property(x => x.IsAdult).HasColumnName("is_adult");
            modelBuilder.Entity<Title>().Property(x => x.StartYear).HasColumnName("start_year");
            modelBuilder.Entity<Title>().Property(x => x.EndYear).HasColumnName("end_year");
            modelBuilder.Entity<Title>().Property(x => x.RunTimeMinutes).HasColumnName("run_time_minutes");
            modelBuilder.Entity<Title>().Property(x => x.Poster).HasColumnName("poster");
            modelBuilder.Entity<Title>().Property(x => x.Awards).HasColumnName("awards");
            modelBuilder.Entity<Title>().Property(x => x.Plot).HasColumnName("plot");

            // BookmarkTitle mapping
            modelBuilder.Entity<BookmarkTitle>().ToTable("bookmark_title");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.TitleId).HasColumnName("title_id");
            modelBuilder.Entity<BookmarkTitle>().HasOne(x => x.User).WithMany(x => x.BookmarkTitles).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<BookmarkTitle>().HasOne(x => x.Title).WithMany(x => x.BookmarkTitles).HasForeignKey(x => x.TitleId);
        }
    }
}
