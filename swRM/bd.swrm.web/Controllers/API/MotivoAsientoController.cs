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
    [Route("api/MotivoAsiento")]
    public class MotivoAsientoController : Controller
    {
        private readonly SwRMDbContext db;

        public MotivoAsientoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarMotivoAsiento")]
        public async Task<List<MotivoAsiento>> GetMotivoAsiento()
        {
            try
            {
                return await db.MotivoAsiento.Include(c=> c.ConfiguracionContabilidad).OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MotivoAsiento>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetMotivoAsiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var motivoAsiento = await db.MotivoAsiento.Include(c => c.ConfiguracionContabilidad).SingleOrDefaultAsync(m => m.IdMotivoAsiento == id);
                return new Response { IsSuccess = motivoAsiento != null, Message = motivoAsiento != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = motivoAsiento };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutMotivoAsiento([FromRoute] int id, [FromBody] MotivoAsiento motivoAsiento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoAsiento.Where(c => c.Descripcion.ToUpper().Trim() == motivoAsiento.Descripcion.ToUpper().Trim()).AnyAsync(c => c.IdMotivoAsiento != motivoAsiento.IdMotivoAsiento))
                {
                    var motivoAsientoActualizar = await db.MotivoAsiento.Where(x => x.IdMotivoAsiento == id).FirstOrDefaultAsync();
                    if (motivoAsientoActualizar != null)
                    {
                        try
                        {
                            motivoAsientoActualizar.Descripcion = motivoAsiento.Descripcion;
                            motivoAsientoActualizar.IdConfiguracionContabilidad = motivoAsiento.IdConfiguracionContabilidad;
                            db.MotivoAsiento.Update(motivoAsientoActualizar);
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
        [Route("InsertarMotivoAsiento")]
        public async Task<Response> PostMotivoAsiento([FromBody] MotivoAsiento motivoAsiento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoAsiento.AnyAsync(c => c.Descripcion.ToUpper().Trim() == motivoAsiento.Descripcion.ToUpper().Trim()))
                {
                    db.MotivoAsiento.Add(motivoAsiento);
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
        public async Task<Response> DeleteMotivoAsiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MotivoAsiento.SingleOrDefaultAsync(m => m.IdMotivoAsiento == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MotivoAsiento.Remove(respuesta);
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