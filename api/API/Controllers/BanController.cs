using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public BanController(CafeDbContext context)
        {
            _context = context;
        }

        // GET: api/Ban
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BanDto>>> GetAll()
        {
            var bans = await _context.Bans
                .Select(b => new BanDto
                {
                    Id = b.Id,
                    TenBan = b.TenBan,
                    TrangThai = b.TrangThai
                })
                .ToListAsync();

            return Ok(bans);
        }

        // GET: api/Ban/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BanDto>> GetById(int id)
        {
            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            return new BanDto
            {
                Id = ban.Id,
                TenBan = ban.TenBan,
                TrangThai = ban.TrangThai
            };
        }

        // POST: api/Ban
        [HttpPost]
        public async Task<ActionResult<BanDto>> Create([FromBody] BanCreateUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ban = new Ban
            {
                TenBan = dto.TenBan,
                TrangThai = dto.TrangThai
            };

            _context.Bans.Add(ban);
            await _context.SaveChangesAsync();

            var result = new BanDto
            {
                Id = ban.Id,
                TenBan = ban.TenBan,
                TrangThai = ban.TrangThai
            };

            return CreatedAtAction(nameof(GetById), new { id = ban.Id }, result);
        }

        // PUT: api/Ban/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BanCreateUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            ban.TenBan = dto.TenBan;
            ban.TrangThai = dto.TrangThai;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Ban/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
