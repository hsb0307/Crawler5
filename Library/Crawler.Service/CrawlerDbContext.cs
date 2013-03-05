using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Entity;
using Husb.Common;
using Husb.Data;

namespace Crawler.Service
{
    public partial class CrawlerDbContext : DbContext, IDbContext
    {
        //public DbSet<TaskCategory> TaskCategory { get; set; }
        //public DbSet<Crawler.Entity.Task> Task { get; set; }
        //public DbSet<TaskItem> TaskItem { get; set; }
        //public DbSet<CaptureRule> CaptureRule { get; set; }
        //public DbSet<ArticleCategory> ArticleCategory { get; set; }
        //public DbSet<Article> Article { get; set; }

        public CrawlerDbContext() : base("DefaultConnection") {
            //Database.SetInitializer<CrawlerDbContext>(null);
        }

        

        //public DbSet<TaskCategory> TaskCategory { get; set; }

        protected override bool ShouldValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry)
        {
            
            if (entityEntry.Entity is Crawler.Entity.Article)
            {
                return false;
            }
            return base.ShouldValidateEntity(entityEntry);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            //Database.SetInitializer<DbContextTypeName>(null);

            //modelBuilder.IncludeMetadataInDatabase = false;
            // AutomaticMigrationsEnabled = false;
            //modelBuilder.Configurations

            modelBuilder.Entity<Article>().Property(a => a.ContentText).HasColumnType("ntext");
            modelBuilder.Entity<Article>().Property(a => a.ContentHTML).HasColumnType("image");


            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Crawler.Entity.Task>()
                .HasRequired(t => t.TaskCategory)
                .WithMany(tc => tc.Tasks)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<TaskItem>()
                .HasRequired(ti => ti.Task)
                .WithMany(t => t.TaskItems)
                .HasForeignKey(ti => ti.TaskId);

            modelBuilder.Entity<TaskItem>()
                .HasMany(ti => ti.CaptureRules)
                .WithMany(c => c.TaskItems)
                .Map(mc => mc.ToTable("TaskItemCaptureRule")
                    .MapLeftKey("TaskItemId")
                    .MapRightKey("CaptureRuleId"));

            //modelBuilder.Entity<Crawler.Entity.TaskItem>()
            //    .HasRequired(t => t.Category)
            //    .WithMany(tc => tc.TaskItems)
            //    .HasForeignKey(t => t.CategoryId);


            modelBuilder.Entity<ArticleCategory>()
                .HasMany(ac => ac.Articles)
                .WithRequired(a => a.Category)
                .HasForeignKey(a => a.CategoryId);

            // 
            modelBuilder.Entity<Article>()
                .HasRequired(a => a.TaskItem)
                .WithMany()
                .HasForeignKey(a => a.TaskItemId);

            //modelBuilder.Entity<TaskItem>()
            //    .HasRequired(t => t.Article)
            //    .WithMany()
            //    .HasForeignKey(t => t.ArticleId);
        }

        //public IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        //{
        //    return base.Set<TEntity>();
        //    //throw new NotImplementedException();
        //}

        //public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity
        //{
        //    //System.Data.Objects.ObjectStateManager = this.
        //    return base.Entry<TEntity>(entity);
        //    //throw new NotImplementedException();
        //}

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return base.Set<TEntity>();
        }

        public new DbSet Set(Type entityType)
        {
            return base.Set(entityType);
        }

        public new DbEntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return base.Entry<TEntity>(entity);
        }
    }
}
