using System.Text.Json;
using AcmvInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace AcmvInventory.Data;

public static class InventorySeeder
{
    public static async Task SeedAsync(
        AcmvDbContext db,
        ILogger logger,
        string contentRootPath,
        CancellationToken ct = default)
    {
        if (await db.Inventory.AnyAsync(ct)) return;

        var seedPath = Path.Combine(contentRootPath, "Seed", "initialInventory.json");
        if (!File.Exists(seedPath))
        {
            logger.LogWarning("Seed file not found at {Path}. Inventory seeding skipped.", seedPath);
            return;
        }

        List<InventorySeedItem>? raw;
        try
        {
            var json = await File.ReadAllTextAsync(seedPath, ct);
            raw = JsonSerializer.Deserialize<List<InventorySeedItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to read or parse inventory seed file at {Path}", seedPath);
            return;
        }

        if (raw is null || raw.Count == 0)
        {
            logger.LogWarning("Seed file parsed but contained no records: {Path}", seedPath);
            return;
        }

        var now = DateTime.UtcNow;
        var entities = raw.Select(item => new Inventory
        {
            Id = string.IsNullOrWhiteSpace(item.Id) ? Guid.NewGuid().ToString() : item.Id!,
            Building = Safe(item.Building, "Unknown"),
            Room = Safe(item.Room),
            TagNo = Safe(item.TagNo),
            InstallationType = Safe(item.InstallationType),
            SystemType = Safe(item.SystemType),
            Brand = Safe(item.Brand),
            EquipmentModel = Safe(item.EquipmentModel),
            PartCategory = Safe(item.PartCategory),
            PartName = Safe(item.PartName, "Unnamed Part"),
            PartModel = Safe(item.PartModel),
            Unit = Safe(item.Unit, "pcs"),
            Status = ParseEnum(item.Status, PartStatus.Spare),
            Criticality = ParseEnum(item.Criticality, Criticality.Medium),
            ImageBase64 = Safe(item.ImageBase64),
            Specs = Safe(item.Specs),
            WarrantyExpiry = item.WarrantyExpiry,
            Remark = Safe(item.Remark),
            QuantityOnHand = item.QuantityOnHand ?? 0,
            MinStock = Math.Max(item.MinStock ?? 1, 0),
            ReorderPoint = item.ReorderPoint,
            ReorderQty = item.ReorderQty,
            PreferredSupplierId = Safe(item.PreferredSupplierId),
            LocationBin = Safe(item.LocationBin),
            RowVersion = [1],
            CreatedAt = item.LastUpdated ?? now,
            LastUpdated = item.LastUpdated ?? now
        }).ToList();

        db.Inventory.AddRange(entities);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Seeded {Count} inventory items from {Path}", entities.Count, seedPath);
    }

    private static string Safe(string? value, string fallback = "") =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        return Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed) ? parsed : fallback;
    }

    private sealed class InventorySeedItem
    {
        public string? Id { get; set; }
        public string? Building { get; set; }
        public string? Room { get; set; }
        public string? TagNo { get; set; }
        public string? InstallationType { get; set; }
        public string? SystemType { get; set; }
        public string? Brand { get; set; }
        public string? EquipmentModel { get; set; }
        public string? PartCategory { get; set; }
        public string? PartName { get; set; }
        public string? PartModel { get; set; }
        public string? Unit { get; set; }
        public string? Status { get; set; }
        public string? Criticality { get; set; }
        public string? ImageBase64 { get; set; }
        public string? Specs { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public string? Remark { get; set; }
        public int? QuantityOnHand { get; set; }
        public int? MinStock { get; set; }
        public int? ReorderPoint { get; set; }
        public int? ReorderQty { get; set; }
        public string? PreferredSupplierId { get; set; }
        public string? LocationBin { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
