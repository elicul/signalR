using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SignalR.Contracts.Entities;
using System;
using System.Linq;

namespace SignalR.Infrastructure
{
    public class SQLiteDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DbContextOptions<SQLiteDbContext> options;

        public SQLiteDbContext() : base()
        {
        }

        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>().HasKey(k => k.Id);
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(p => p.Email).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.TenantGuid).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<User>().ToTable("Users");

            Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private static void Seed(ModelBuilder modelBuilder)
        {
            var currentDateUtc = DateTime.UtcNow;

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "demo@demo.com",
                    TenantGuid = Guid.NewGuid(),
                    TenantType = "Carrier",
                    ConnectionId = "12345",
                    CreatedDateUtc = currentDateUtc,
                    LastModifiedDateUtc = currentDateUtc,
                    IsDeleted = false,
                });
        }
    }
}
