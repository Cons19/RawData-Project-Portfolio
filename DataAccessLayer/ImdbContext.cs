using DataAccessLayer.Domain;
using DataAccessLayer.Domain.Functions;
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
        public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public DbSet<BookmarkPerson> BookmarkPersons { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<RatingHistory> RatingHistory { get; set; }
        public DbSet<SearchTitle> SearchTitle { get; set; }
        public DbSet<FindPersonByProfession> FindPersonByProfession { get; set; }
        public DbSet<StructuredStringSearch> StructuredStringSearch { get; set; }
        public DbSet<ExactMatch> ExactMatch { get; set; }

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
            modelBuilder.Entity<User>().Property(x => x.Salt).HasColumnName("salt");
            modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<User>().Property(x => x.UpdatedAt).HasColumnName("updated_at");

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


            // BookmarkPerson mapping
            modelBuilder.Entity<BookmarkPerson>().ToTable("bookmark_person");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.PersonId).HasColumnName("person_id");
            modelBuilder.Entity<BookmarkPerson>().HasOne(x => x.User).WithMany(x => x.BookmarkPersons).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<BookmarkPerson>().HasOne(x => x.Person).WithMany(x => x.BookmarkPersons).HasForeignKey(x => x.PersonId);

            // Person mapping
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Person>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birth_year");
            modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("death_year");
            modelBuilder.Entity<Person>().Property(x => x.Rating).HasColumnName("rating");

            // Search history mapping
            modelBuilder.Entity<SearchHistory>().ToTable("search_history");
            modelBuilder.Entity<SearchHistory>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<SearchHistory>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<SearchHistory>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
            modelBuilder.Entity<SearchHistory>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<SearchHistory>().Property(x => x.SearchText).HasColumnName("search_text");

            // Rating history mapping
            modelBuilder.Entity<RatingHistory>().ToTable("rating_history");
            modelBuilder.Entity<RatingHistory>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<RatingHistory>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<RatingHistory>().Property(x => x.TitleId).HasColumnName("title_id");
            modelBuilder.Entity<RatingHistory>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<RatingHistory>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
            modelBuilder.Entity<RatingHistory>().Property(x => x.Rate).HasColumnName("rate");
            modelBuilder.Entity<RatingHistory>().HasOne(x => x.User).WithMany(x => x.RatingHistories).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<RatingHistory>().HasOne(x => x.Title).WithMany(x => x.RatingHistories).HasForeignKey(x => x.TitleId);

            // Search Title
            modelBuilder.Entity<SearchTitle>().HasNoKey();
            modelBuilder.Entity<SearchTitle>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<SearchTitle>().Property(x => x.PrimaryTitle).HasColumnName("primary_title");

            // Find Person By Profession
            modelBuilder.Entity<FindPersonByProfession>().HasNoKey();
            modelBuilder.Entity<FindPersonByProfession>().Property(x => x.Name).HasColumnName("name");

            // Structured String Search
            modelBuilder.Entity<StructuredStringSearch>().HasNoKey();
            modelBuilder.Entity<StructuredStringSearch>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<StructuredStringSearch>().Property(x => x.PrimaryTitle).HasColumnName("primary_title");
            modelBuilder.Entity<StructuredStringSearch>().Property(x => x.Description).HasColumnName("description");

            // Exact Match
            modelBuilder.Entity<ExactMatch>().HasNoKey();
            modelBuilder.Entity<ExactMatch>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<ExactMatch>().Property(x => x.Title).HasColumnName("title");
        }
    }
}
