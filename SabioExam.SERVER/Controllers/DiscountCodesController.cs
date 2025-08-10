using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SabioExam.DATA;
using SabioExam.DATA.Entities;
using SabioExam.SERVER.Services;

namespace SabioExam.SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDiscountService _discountService;

        public DiscountCodesController(DataContext context, IDiscountService discountService)
        {
            _context = context;
            _discountService = discountService;
        }

        // GET: api/DiscountCodes
        [HttpGet("get/{length}")]
        public async Task<ActionResult<IEnumerable<DiscountCode>>> GetDiscountCodes(byte length)
        {
            var codes = await _discountService.GetDiscountCodesAsync(length);
            return codes?.ToList() ?? new List<DiscountCode>();
        }

        // GET: api/DiscountCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountCode>> GetDiscountCode(string id)
        {
            var discountCode = await _context.DiscountCodes.FindAsync(id);

            if (discountCode == null)
            {
                return NotFound();
            }

            return discountCode;
        }

        // PUT: api/DiscountCodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscountCode(string id, DiscountCode discountCode)
        {
            if (id != discountCode.Code)
            {
                return BadRequest();
            }

            _context.Entry(discountCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountCodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DiscountCodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DiscountCode>> PostDiscountCode(DiscountCode discountCode)
        {
            _context.DiscountCodes.Add(discountCode);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DiscountCodeExists(discountCode.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDiscountCode", new { id = discountCode.Code }, discountCode);
        }

        // DELETE: api/DiscountCodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscountCode(string id)
        {
            var discountCode = await _context.DiscountCodes.FindAsync(id);
            if (discountCode == null)
            {
                return NotFound();
            }

            _context.DiscountCodes.Remove(discountCode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiscountCodeExists(string id)
        {
            return _context.DiscountCodes.Any(e => e.Code == id);
        }
    }
}
