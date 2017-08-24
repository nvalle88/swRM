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
    [Route("api/MotivoRecepcion")]
    public class MotivoRecepcionController : Controller
    {
        private readonly SwRMDbContext _context;

        public MotivoRecepcionController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/MotivoRecepcion
        [HttpGet]
        public IEnumerable<MotivoRecepcion> GetMotivoRecepcion()
        {
            return _context.MotivoRecepcion;
        }

        // GET: api/MotivoRecepcion/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotivoRecepcion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.MotivoRecepcion.SingleOrDefaultAsync(m => m.IdMotivoRecepcion == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }
        
        // POST: api/MotivoRecepcion
        [HttpPost]
        public async Task<IActionResult> PostMotivoRecepcion([FromBody]MotivoRecepcion _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MotivoRecepcion.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdMotivoRecepcion))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMotivoRecepcion", new { id = _entidad.IdMotivoRecepcion }, _entidad);
        }
        
        // PUT: api/MotivoRecepcion/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMotivoRecepcion([FromRoute] int id, [FromBody]MotivoRecepcion _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdMotivoRecepcion)
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
        public async Task<IActionResult> DeleteMotivoRecepcion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.MotivoRecepcion.SingleOrDefaultAsync(m => m.IdMotivoRecepcion == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.MotivoRecepcion.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.MotivoRecepcion.Any(e => e.IdMotivoRecepcion == id);
        }
    }
}
