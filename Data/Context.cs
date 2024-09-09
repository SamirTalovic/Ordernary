using Microsoft.EntityFrameworkCore;
using Ordernary.Models;

namespace Ordernary.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Article>  Articles  { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<OrderArticle> OrderArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Table>()
           .HasMany(t => t.Orders)
           .WithOne(o => o.Table)
           .HasForeignKey(o => o.TableId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderArticle>()
               .HasKey(oa => new { oa.OrderId, oa.ArticleId });  

            builder.Entity<OrderArticle>()
                .HasOne(oa => oa.Order)
                .WithMany(o => o.OrderArticles)
                .HasForeignKey(oa => oa.OrderId);

            builder.Entity<OrderArticle>()
                .HasOne(oa => oa.Article)
                .WithMany(a => a.OrderArticles)
                .HasForeignKey(oa => oa.ArticleId);


        }
        }
}
