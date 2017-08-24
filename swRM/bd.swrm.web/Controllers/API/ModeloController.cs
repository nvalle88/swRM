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
    [Route("api/Modelo")]
    public class ModeloController : Controller
    {
        private readonly SwRMDbContext _context;

        public ModeloController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/Modelo
        [HttpGet]
        public IEnumerable<Modelo> GetModelo()
        {
            return _context.Modelo;
        }

        // GET: api/Modelo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.Modelo.SingleOrDefaultAsync(m => m.IdModelo == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }
        
        // POST: api/Modelo
        [HttpPost]
        public async Task<IActionResult> PostModelo([FromBody]Modelo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Modelo.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdModelo))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetModelo", new { id = _entidad.IdModelo }, _entidad);
        }
        
        // PUT: api/Modelo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelo([FromRoute] int id, [FromBody]Modelo _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdModelo)
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
        public async Task<IActionResult> DeleteModelo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.Modelo.SingleOrDefaultAsync(m => m.IdModelo == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.Modelo.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.Modelo.Any(e => e.IdModelo == id);
        }
    }
}
