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
    [Route("api/MaestroDetalleArticulo")]
    public class MaestroDetalleArticuloController : Controller
    {
        private readonly SwRMDbContext _context;

        public MaestroDetalleArticuloController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/MaestroDetalleArticulo
        [HttpGet]
        public IEnumerable<MaestroDetalleArticulo> GetMaestroDetalleArticulo()
        {
            return _context.MaestroDetalleArticulo;
        }

        // GET: api/MaestroDetalleArticulo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaestroDetalleArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.MaestroDetalleArticulo.SingleOrDefaultAsync(m => m.IdArticulo == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }
        
        // POST: api/MaestroDetalleArticulo
        [HttpPost]
        public async Task<IActionResult> PostMaestroDetalleArticulo([FromBody]MaestroDetalleArticulo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MaestroDetalleArticulo.Add(_entidad);
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

            return CreatedAtAction("GetMaestroDetalleArticulo", new { id = _entidad.IdArticulo }, _entidad);
        }
        
        // PUT: api/MaestroDetalleArticulo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaestroDetalleArticulo([FromRoute] int id, [FromBody]MaestroDetalleArticulo _entidad)
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
        public async Task<IActionResult> DeleteMaestroDetalleArticulo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.MaestroDetalleArticulo.SingleOrDefaultAsync(m => m.IdArticulo == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.MaestroDetalleArticulo.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.MaestroDetalleArticulo.Any(e => e.IdArticulo == id);
        }
    }
}
