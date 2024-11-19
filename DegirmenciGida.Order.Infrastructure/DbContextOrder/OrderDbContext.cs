using DegirmenciGida.Order.Domain;
using Microsoft.EntityFrameworkCore;

namespace DegirmenciGida.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }


        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
                .Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<OrderDetail>()
                .Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<OrderDetail>()
                .Property(o => o.ProductPrice)
                .HasPrecision(18, 4);
            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderDetails)
                .WithOne()
                .HasForeignKey(od => od.OrdersId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
