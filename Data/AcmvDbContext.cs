
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Models;

namespace AcmvInventory.Data
{
    public class AcmvDbContext : DbContext
    {
        public AcmvDbContext(DbContextOptions<AcmvDbContext> options) : base(options) { }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockTransaction> Transactions { get; set; }
        public DbSet<TransactionLine> TransactionLines { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<PRLine> PRLines { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<POLine> POLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasIndex(i => i.TagNo);

            modelBuilder.Entity<PurchaseRequest>()
                .HasMany(p => p.Lines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(p => p.Lines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
