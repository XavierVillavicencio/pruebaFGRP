
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruebaFGRP.Data;
using pruebaFGRP.Models;

namespace pruebaFGRP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : ControllerBase
    {
        private readonly pruebaFGRPContext _context;

        public ConfigsController(pruebaFGRPContext context)
        {
            _context = context;
        }

        // GET: api/Configs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Config>>> GetConfigs()
        {
            return await _context.Config.ToListAsync();
        }

        // GET: api/Configs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Config>> GetConfig(int id)
        {
            var config = await _context.Config.FindAsync(id);

            if (config == null)
            {
                return NotFound();
            }

            return config;
        }

        // POST: api/Configs
        [HttpPost]
        public async Task<ActionResult<Config>> PostConfig(Config config)
        {
            config.CreatedAt = DateTime.UtcNow;
            _context.Config.Add(config);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await ConfigExists(config.Key))
                {
                    return Conflict("A config with this key already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetConfig), new { id = config.Id }, config);
        }

        // PUT: api/Configs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfig(int id, Config config)
        {
            if (id != config.Id)
            {
                return BadRequest();
            }

            config.UpdatedAt = DateTime.UtcNow;
            _context.Entry(config).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ConfigExistsById(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                if (await ConfigExists(config.Key, id))
                {
                    return Conflict("A config with this key already exists.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Configs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfig(int id)
        {
            var config = await _context.Config.FindAsync(id);
            if (config == null)
            {
                return NotFound();
            }

            _context.Config.Remove(config);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ConfigExistsById(int id)
        {
            return await _context.Config.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> ConfigExists(string key, int? id = null)
        {
            return await _context.Config.AnyAsync(e => e.Key == key && (id == null || e.Id != id));
        }
    }
}