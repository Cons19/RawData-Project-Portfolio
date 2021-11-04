using Data_Access_Layer.Domain;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer
{
    class ImdbContext : DbContext
    {
        private const string host = "rawdata.ruc.dk";
        private const string database = "raw4";
        private const string uid = "raw4";
        private const string password = "UXyNO(IR";
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            //optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql($"host={host};db={database};uid={uid};pwd={password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // user mapping
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
            modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<User>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
