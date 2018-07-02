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
    [Route("api/ClaseActivoFijo")]
    public class ClaseActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public ClaseActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarClaseActivoFijo")]
        public async Task<List<ClaseActivoFijo>> GetClaseActivoFijo()
        {
            try
            {
                return await db.ClaseActivoFijo.OrderBy(x => x.Nombre).Include(c=> c.TipoActivoFijo).Include(c => c.CategoriaActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ClaseActivoFijo>();
            }
        }

        [HttpGet]
        [Route("ListarClaseActivoFijoPorTipoActivoFijo/{idTipoActivoFijo}")]
        public async Task<List<ClaseActivoFijo>> GetClaseActivoFijoPorTipo(int idTipoActivoFijo)
        {
            try
            {
                return await db.ClaseActivoFijo.Where(c=> c.IdTipoActivoFijo == idTipoActivoFijo).OrderBy(x => x.Nombre).Include(c => c.TipoActivoFijo).Include(c => c.CategoriaActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ClaseActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetClaseActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var claseActivoFijo = await db.ClaseActivoFijo.Include(c=> c.TipoActivoFijo).SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);
                return new Response { IsSuccess = claseActivoFijo != null, Message = claseActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = claseActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutClaseActivoFijo([FromRoute] int id, [FromBody] ClaseActivoFijo claseActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ClaseActivoFijo.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == claseActivoFijo.Nombre.ToUpper().Trim() && p.IdCategoriaActivoFijo == claseActivoFijo.IdCategoriaActivoFijo && p.IdTipoActivoFijo == claseActivoFijo.IdTipoActivoFijo).AnyAsync(c => c.IdClaseActivoFijo != claseActivoFijo.IdClaseActivoFijo))
                {
                    var claseActivoFijoActualizar = await db.ClaseActivoFijo.Where(x => x.IdClaseActivoFijo == id).FirstOrDefaultAsync();
                    if (claseActivoFijoActualizar != null)
                    {
                        try
                        {
                            claseActivoFijoActualizar.Nombre = claseActivoFijo.Nombre;
                            claseActivoFijoActualizar.IdTipoActivoFijo = claseActivoFijo.IdTipoActivoFijo;
                            claseActivoFijoActualizar.IdCategoriaActivoFijo = claseActivoFijo.IdCategoriaActivoFijo;
                            db.ClaseActivoFijo.Update(claseActivoFijoActualizar);
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
        [Route("InsertarClaseActivoFijo")]
        public async Task<Response> PostClaseActivoFijo([FromBody] ClaseActivoFijo claseActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ClaseActivoFijo.AnyAsync(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == claseActivoFijo.Nombre.ToUpper().Trim() && p.IdCategoriaActivoFijo == claseActivoFijo.IdCategoriaActivoFijo && p.IdTipoActivoFijo == claseActivoFijo.IdTipoActivoFijo))
                {
                    db.ClaseActivoFijo.Add(claseActivoFijo);
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
        public async Task<Response> DeleteClaseActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ClaseActivoFijo.SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ClaseActivoFijo.Remove(respuesta);
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