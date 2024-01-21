using Core.Database.Configurations;
using Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.Context
{
    public class MySqlDatabase : DbContext
    {
        public MySqlDatabase(DbContextOptions<MySqlDatabase> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var type = GetType();
            modelBuilder.AddEntityConfiguration(type.Assembly, type);
            base.OnModelCreating(modelBuilder);
        }

        #region DbSets

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        #endregion
    }
}
