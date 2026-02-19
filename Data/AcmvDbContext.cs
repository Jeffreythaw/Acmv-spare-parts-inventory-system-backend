
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
        public DbSet<OrderSchedule> OrderSchedules { get; set; }
        public DbSet<OrderScheduleLine> OrderScheduleLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .ToTable("TKS_Inventory", "dbo")
                .HasIndex(i => i.TagNo);

            modelBuilder.Entity<Supplier>()
                .ToTable("TKS_Suppliers", "dbo");

            modelBuilder.Entity<StockTransaction>()
                .ToTable("TKS_Transactions", "dbo");

            modelBuilder.Entity<TransactionLine>()
                .ToTable("TKS_TransactionLines", "dbo");

            modelBuilder.Entity<PurchaseRequest>()
                .ToTable("TKS_PurchaseRequests", "dbo");

            modelBuilder.Entity<PRLine>()
                .ToTable("TKS_PRLines", "dbo");

            modelBuilder.Entity<PurchaseOrder>()
                .ToTable("TKS_PurchaseOrders", "dbo");

            modelBuilder.Entity<POLine>()
                .ToTable("TKS_POLines", "dbo");

            modelBuilder.Entity<OrderSchedule>()
                .ToTable("TKS_OrderSchedules", "dbo");

            modelBuilder.Entity<OrderScheduleLine>()
                .ToTable("TKS_OrderScheduleLines", "dbo");

            modelBuilder.Entity<PurchaseRequest>()
                .HasMany(p => p.Lines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(p => p.Lines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderSchedule>()
                .HasMany(s => s.Lines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
