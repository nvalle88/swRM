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
    [Route("api/Articulo")]
    public class ArticuloController : Controller
    {
        private readonly SwRMDbContext _context;

        public ArticuloController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/Articulo
        [HttpGet]
        public IEnumerable<Articulo> GetArticulo()
        {
            return _context.Articulo;
        }

        // GET: api/Articulo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.Articulo.SingleOrDefaultAsync(m => m.IdArticulo == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }

        // POST: api/Articulo
        [HttpPost]
        public async Task<IActionResult> PostArticulo([FromBody] Articulo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Articulo.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdArticulo))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArticulo", new { id = _entidad.IdArticulo }, _entidad);
        }

        // PUT: api/Articulo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticulo([FromRoute] int id, [FromBody] Articulo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdArticulo)
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
        public async Task<IActionResult> DeleteArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.Articulo.SingleOrDefaultAsync(m => m.IdArticulo == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.Articulo.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.Articulo.Any(e => e.IdArticulo == id);
        }
    }
}
