
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;
using AcmvInventory.Services;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AcmvDbContext _context;
        private readonly TransactionService _transactionService;

        public TransactionsController(AcmvDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockTransaction>>> GetTransactions()
        {
            return await _context.Transactions
                .Include(t => t.Lines)
                .OrderByDescending(t => t.TxnTime)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<StockTransaction>> CreateTransaction(StockTransaction txn)
        {
            try
            {
                var result = await _transactionService.CreateMovement(txn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
