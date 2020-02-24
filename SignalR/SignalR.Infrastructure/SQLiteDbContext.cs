using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using SignalR.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SignalR.Infrastructure
{
    public class SQLiteDbContext : DbContext, IDesignTimeDbContextFactory<SQLiteDbContext> 
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SQLiteDbContext() : base()
        {
        }

        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public SQLiteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SQLiteDbContext>();
            optionsBuilder.UseSqlite("Filename=SQLite.db");

            return new SQLiteDbContext(optionsBuilder.Options, httpContextAccessor);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename=TestDatabase.db", options =>
        //    {
        //        options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        //    });
        //    base.OnConfiguring(optionsBuilder);
        //}

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
        private static void Seed(ModelBuilder modelBuilder)
        {
            var currentDateUtc = DateTime.UtcNow;
            var createdBy = "System";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = createdBy,
                    CreatedDateUtc = currentDateUtc,
                    LastModifiedBy = createdBy,
                    LastModifiedDateUtc = currentDateUtc,
                    IsDeleted = false,
                    Email = "demo@demo.com"
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
