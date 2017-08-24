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
    [Route("api/DetalleFactura")]
    public class DetalleFacturaController : Controller
    {
        private readonly SwRMDbContext _context;

        public DetalleFacturaController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/DetalleFactura
        [HttpGet]
        [HttpGet]
        public IEnumerable<DetalleFactura> GetDetalleFactura()
        {
            return _context.DetalleFactura;
        }

        // GET: api/DetalleFactura/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetalleFactura([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.DetalleFactura.SingleOrDefaultAsync(m => m.IdDetalleFactura == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }

        // POST: api/DetalleFactura
        [HttpPost]
        public async Task<IActionResult> PostDetalleFactura([FromBody] DetalleFactura _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DetalleFactura.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdDetalleFactura))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDetalleFactura", new { id = _entidad.IdDetalleFactura }, _entidad);
        }

        // PUT: api/DetalleFactura/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleFactura([FromRoute] int id, [FromBody] DetalleFactura _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdDetalleFactura)
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
        public async Task<IActionResult> DeleteDetalleFactura([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.DetalleFactura.SingleOrDefaultAsync(m => m.IdDetalleFactura == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.DetalleFactura.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.DetalleFactura.Any(e => e.IdDetalleFactura == id);
        }
    }
}
