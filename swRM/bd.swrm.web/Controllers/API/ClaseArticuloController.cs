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
    [Route("api/ClaseArticulo")]
    public class ClaseArticuloController : Controller
    {
        private readonly SwRMDbContext _context;

        public ClaseArticuloController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/ClaseArticulo
        [HttpGet]
        public IEnumerable<ClaseArticulo> GetClaseArticulo()
        {
            return _context.ClaseArticulo;
        }

        // GET: api/ClaseArticulo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaseArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.ClaseArticulo.SingleOrDefaultAsync(m => m.IdClaseArticulo == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }

        // POST: api/ClaseArticulo
        [HttpPost]
        public async Task<IActionResult> PostClaseArticulo([FromBody] ClaseArticulo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ClaseArticulo.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdClaseArticulo))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClaseArticulo", new { id = _entidad.IdClaseArticulo }, _entidad);
        }

        // PUT: api/ClaseArticulo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaseArticulo([FromRoute] int id, [FromBody] ClaseArticulo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdClaseArticulo)
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
        public async Task<IActionResult> DeleteClaseArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.ClaseArticulo.SingleOrDefaultAsync(m => m.IdClaseArticulo == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.ClaseArticulo.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.ClaseArticulo.Any(e => e.IdClaseArticulo == id);
        }
    }
}
