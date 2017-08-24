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
    [Route("api/Marca")]
    public class MarcaController : Controller
    {
        private readonly SwRMDbContext _context;

        public MarcaController(SwRMDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/Marca
        [HttpGet]
        public IEnumerable<Marca> GetMarca()
        {
            return _context.Marca;
        }

        // GET: api/Marca/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarca([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _entidad = await _context.Marca.SingleOrDefaultAsync(m => m.IdMarca == id);

            if (_entidad == null)
                return NotFound();

            return Ok(_entidad);
        }
        
        // POST: api/Marca
        [HttpPost]
        public async Task<IActionResult> PostMarca([FromBody]Marca _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Marca.Add(_entidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntidadExists(_entidad.IdMarca))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMarca", new { id = _entidad.IdMarca }, _entidad);
        }
        
        // PUT: api/Marca/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarca([FromRoute] int id, [FromBody]Marca _entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != _entidad.IdMarca)
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
        public async Task<IActionResult> DeleteMarca([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _entidad = await _context.Marca.SingleOrDefaultAsync(m => m.IdMarca == id);
            if (_entidad == null)
            {
                return NotFound();
            }

            _context.Marca.Remove(_entidad);
            await _context.SaveChangesAsync();

            return Ok(_entidad);
        }

        private bool EntidadExists(int id)
        {
            return _context.Marca.Any(e => e.IdMarca == id);
        }
    }
}
