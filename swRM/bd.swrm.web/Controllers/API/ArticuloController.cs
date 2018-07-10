using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.Enumeradores;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.log.guardar.Utiles;
using bd.swrm.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Articulo")]
    public class ArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public ArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarArticulos")]
        public async Task<List<Articulo>> GetArticulo()
        {
            try
            {
                return await db.Articulo
                    .Include(c=> c.SubClaseArticulo).ThenInclude(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo)
                    .Include(c=> c.UnidadMedida)
                    .Include(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Articulo>();
            }
        }

        [HttpGet]
        [Route("ListarArticulosPorSubClase/{idSubClaseArticulo}")]
        public async Task<List<Articulo>> GetArticuloPorSubClase(int idSubClaseArticulo)
        {
            try
            {
                return await db.Articulo.Where(c=> c.IdSubClaseArticulo == idSubClaseArticulo)
                    .Include(c => c.SubClaseArticulo).ThenInclude(c => c.ClaseArticulo).ThenInclude(c => c.TipoArticulo)
                    .Include(c => c.UnidadMedida)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Articulo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var articulo = await db.Articulo
                    .Include(c=> c.SubClaseArticulo).ThenInclude(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo)
                    .Include(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .Include(c=> c.UnidadMedida)
                    .SingleOrDefaultAsync(m => m.IdArticulo == id);
                return new Response { IsSuccess = articulo != null, Message = articulo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = articulo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutArticulo([FromRoute] int id, [FromBody] Articulo articulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Articulo.Where(c => c.Nombre.ToUpper().Trim() == articulo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdArticulo != articulo.IdArticulo))
                {
                    var articuloActualizar = await db.Articulo.Where(x => x.IdArticulo == id).FirstOrDefaultAsync();
                    if (articuloActualizar != null)
                    {
                        try
                        {
                            articuloActualizar.Nombre = articulo.Nombre;
                            articuloActualizar.IdSubClaseArticulo = articulo.IdSubClaseArticulo;
                            articuloActualizar.IdUnidadMedida = articulo.IdUnidadMedida;
                            articuloActualizar.IdModelo = articulo.IdModelo;
                            db.Articulo.Update(articuloActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                            return new Response { IsSuccess = false, Message = Mensaje.Error };
                        }
                    }
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }
        
        [HttpPost]
        [Route("InsertarArticulo")]
        public async Task<Response> PostArticulo([FromBody] Articulo articulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Articulo.AnyAsync(c => c.Nombre.ToUpper().Trim() == articulo.Nombre.ToUpper().Trim()))
                {
                    db.Entry(articulo.Modelo).State = EntityState.Unchanged;
                    db.Entry(articulo.SubClaseArticulo).State = EntityState.Unchanged;
                    db.Articulo.Add(articulo);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.Articulo.SingleOrDefaultAsync(m => m.IdArticulo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.Articulo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
    }
}
