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
            var createdBy = "System";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "demo@demo.com",
                    TenantGuid = Guid.NewGuid(),
                    TenantType = "Carrier",
                    ConnectionId = "12345",
                    CreatedBy = createdBy,
                    CreatedDateUtc = currentDateUtc,
                    LastModifiedBy = createdBy,
                    LastModifiedDateUtc = currentDateUtc,
                    IsDeleted = false,
                });
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            SetCreatedAndLastModified();
            return base.Set<TEntity>();
        }

        private void SetCreatedAndLastModified(string username = "system")
        {
            var currentUtcTime = DateTime.UtcNow;
            var entries = ChangeTracker.Entries().Where(x => x.Entity != null && typeof(BaseEntity).IsAssignableFrom(x.Entity.GetType()));
            foreach (var entry in entries)
            {
                BaseEntity entity = entry.Entity as BaseEntity;
                if (entity == null)
                    continue;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDateUtc = currentUtcTime;
                    entity.LastModifiedDateUtc = currentUtcTime;
                    if (!string.IsNullOrEmpty(username))
                    {
                        entity.CreatedBy = username;
                        entity.LastModifiedBy = username;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.LastModifiedDateUtc = currentUtcTime;
                    if (!string.IsNullOrEmpty(username))
                        entity.LastModifiedBy = username;
                }
            }
        }
    }
}
