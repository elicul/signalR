using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using SignalR.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalR.Infrastructure
{
    public class UserDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public UserDbContext(DbContextOptions<UserDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
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
            modelBuilder.Entity<User>().Property(x => x.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<User>().ToTable("Users");

            Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            SetCreatedAndLastModified();
            return base.Set<TEntity>();
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            var currentDateUtc = DateTime.UtcNow;
            var createdBy = "System";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid(),
                    CreatedBy = createdBy,
                    CreatedDateUtc = currentDateUtc,
                    LastModifiedBy = createdBy,
                    LastModifiedDateUtc = currentDateUtc,
                    IsDeleted = false,
                    Email = "demo@demo.com"
                });
        }

        protected static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);

            if (sqlExt == null)
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));

            return new DbContextOptionsBuilder<T>()
                .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString)
                .Options;
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
