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
    [Route("api/ClaseArticulo")]
    public class ClaseArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public ClaseArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarClaseArticulo")]
        public async Task<List<ClaseArticulo>> GetClaseArticulo()
        {
            try
            {
                return await db.ClaseArticulo.OrderBy(x => x.Nombre).Include(c=> c.TipoArticulo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ClaseArticulo>();
            }
        }
        
        [HttpGet("{id}")]

        public async Task<Response> GetClaseArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var claseArticulo = await db.ClaseArticulo.SingleOrDefaultAsync(m => m.IdClaseArticulo == id);
                claseArticulo.TipoArticulo = await db.TipoArticulo.SingleOrDefaultAsync(c => c.IdTipoArticulo == claseArticulo.IdTipoArticulo);
                return new Response { IsSuccess = claseArticulo != null, Message = claseArticulo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = claseArticulo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutClaseArticulo([FromRoute] int id, [FromBody] ClaseArticulo claseArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ClaseArticulo.Where(c => c.Nombre.ToUpper().Trim() == claseArticulo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdClaseArticulo != claseArticulo.IdClaseArticulo))
                {
                    var claseArticuloActualizar = await db.ClaseArticulo.Where(x => x.IdClaseArticulo == id).FirstOrDefaultAsync();
                    if (claseArticuloActualizar != null)
                    {
                        try
                        {
                            claseArticuloActualizar.Nombre = claseArticulo.Nombre;
                            claseArticuloActualizar.IdTipoArticulo = claseArticulo.IdTipoArticulo;
                            db.ClaseArticulo.Update(claseArticuloActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
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
        [Route("InsertarClaseArticulo")]
        public async Task<Response> PostArticulo([FromBody] ClaseArticulo claseArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ClaseArticulo.AnyAsync(c => c.Nombre.ToUpper().Trim() == claseArticulo.Nombre.ToUpper().Trim()))
                {
                    db.ClaseArticulo.Add(claseArticulo);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteClaseArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ClaseArticulo.SingleOrDefaultAsync(m => m.IdClaseArticulo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ClaseArticulo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ClaseArticulo claseArticulo)
        {
            var bdd = claseArticulo.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.ClaseArticulo.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
