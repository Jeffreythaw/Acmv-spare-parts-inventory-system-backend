
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AcmvDbContext _context;

        public InventoryController(AcmvDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Inventory>>>> GetItems(
            [FromQuery] string? search = null,
            [FromQuery] string? building = null,
            [FromQuery] string? category = null,
            [FromQuery] string? status = null)
        {
            var query = _context.Inventory.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.PartName.Contains(search) || i.TagNo.Contains(search));
            }

            if (!string.IsNullOrEmpty(building) && building != "All Buildings")
            {
                query = query.Where(i => i.Building == building);
            }

            if (!string.IsNullOrEmpty(category) && category != "All Categories")
            {
                query = query.Where(i => i.PartCategory == category);
            }

            if (!string.IsNullOrEmpty(status) && status != "All Statuses")
            {
                if (Enum.TryParse<PartStatus>(status, true, out var parsedStatus))
                {
                    query = query.Where(i => i.Status == parsedStatus);
                }
            }

            var items = await query.ToListAsync();
            return Ok(ApiResponse<IEnumerable<Inventory>>.Ok(items));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Inventory>>> PostItem(Inventory item)
        {
            if (item.RowVersion == null || item.RowVersion.Length == 0) item.RowVersion = [1];
            item.LastUpdated = DateTime.UtcNow;
            _context.Inventory.Add(item);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Inventory>.Ok(item));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> PutItem(string id, Inventory item)
        {
            if (id != item.Id) return BadRequest(ApiResponse<string>.Fail("ID mismatch"));
            if (item.RowVersion == null || item.RowVersion.Length == 0) item.RowVersion = [1];

            _context.Entry(item).State = EntityState.Modified;
            item.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(ApiResponse<string>.Ok("Update successful"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteItem(string id)
        {
            var item = await _context.Inventory.FindAsync(id);
            if (item == null) return NotFound(ApiResponse<string>.Fail("Item not found"));

            _context.Inventory.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<string>.Ok("Delete successful"));
        }
    }
}
