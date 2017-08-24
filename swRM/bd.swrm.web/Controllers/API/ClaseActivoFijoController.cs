using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using Microsoft.EntityFrameworkCore;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ClaseActivoFijo")]
    public class ClaseActivoFijoController : Controller
    {
        private readonly SwRMDbContext _context;

        public ClaseActivoFijoController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/ClaseActivoFijo
        [HttpGet]
        public IEnumerable<ClaseActivoFijo> GetClaseActivoFijo()
        {
            return _context.ClaseActivoFijo;
        }

        // GET: api/ClaseActivoFijo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaseActivoFijo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.ClaseActivoFijo.SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }

        // POST: api/ClaseActivoFijo
        [HttpPost]
        public async Task<IActionResult> PostClaseActivoFijo([FromBody] ClaseActivoFijo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ClaseActivoFijo.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdClaseActivoFijo))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClaseActivoFijo", new { id = _entidad.IdClaseActivoFijo }, _entidad);
        }

        // PUT: api/ClaseActivoFijo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaseActivoFijo([FromRoute] int id, [FromBody] ClaseActivoFijo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdClaseActivoFijo)
            {
                return BadRequest();
            }

            _context.Entry(_entidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntidadExists(id))
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaseActivoFijo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.ClaseActivoFijo.SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.ClaseActivoFijo.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.ClaseActivoFijo.Any(e => e.IdClaseActivoFijo == id);
        }
    }
}
