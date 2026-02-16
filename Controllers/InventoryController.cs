
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
            [FromQuery] string search, 
            [FromQuery] string building, 
            [FromQuery] string category)
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

            var items = await query.ToListAsync();
            return Ok(ApiResponse<IEnumerable<Inventory>>.Ok(items));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Inventory>>> PostItem(Inventory item)
        {
            item.LastUpdated = DateTime.UtcNow;
            _context.Inventory.Add(item);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Inventory>.Ok(item));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> PutItem(string id, Inventory item)
        {
            if (id != item.Id) return BadRequest(ApiResponse<string>.Fail("ID mismatch"));

            _context.Entry(item).State = EntityState.Modified;
            item.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(ApiResponse<string>.Ok("Update successful"));
        }
    }
}
