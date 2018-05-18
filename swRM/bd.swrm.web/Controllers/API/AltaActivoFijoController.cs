using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Utiles;
using bd.swrm.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/AltaActivoFijo")]
    public class AltaActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public AltaActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarAltaActivoFijo")]
        public async Task<List<AltaActivoFijo>> GetAltaActivoFijo()
        {
            try
            {
                return await db.AltaActivoFijo.Include(x=> x.FacturaActivoFijo).Include(c=> c.MotivoAlta).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<AltaActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetAltaActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var altaActivoFijo = await db.AltaActivoFijo.Include(x => x.FacturaActivoFijo).Include(c=> c.MotivoAlta).SingleOrDefaultAsync(m => m.IdAltaActivoFijo == id);
                return new Response { IsSuccess = altaActivoFijo != null, Message = altaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = altaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarAltaActivoFijo")]
        public async Task<Response> PostAltaActivoFijo([FromBody]AltaActivoFijo altaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijo.AnyAsync(c => c.FechaAlta == altaActivoFijo.FechaAlta && c.IdMotivoAlta == altaActivoFijo.IdMotivoAlta && c.IdFacturaActivoFijo == altaActivoFijo.IdFacturaActivoFijo))
                {
                    db.AltaActivoFijo.Add(altaActivoFijo);
                    await db.SaveChangesAsync();
                    Temporizador.Temporizador.InicializarTemporizadorDepreciacion();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro, Resultado = altaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutAltaActivoFijo([FromRoute] int id, [FromBody]AltaActivoFijo altaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijo.Where(c => c.FechaAlta == altaActivoFijo.FechaAlta && c.IdMotivoAlta == altaActivoFijo.IdMotivoAlta && c.IdFacturaActivoFijo == altaActivoFijo.IdFacturaActivoFijo).AnyAsync(c => c.IdAltaActivoFijo != altaActivoFijo.IdAltaActivoFijo))
                {
                    var altaActivoFijoActualizar = await db.AltaActivoFijo.FirstOrDefaultAsync(c=> c.IdAltaActivoFijo == id);
                    if (altaActivoFijoActualizar != null)
                    {
                        try
                        {
                            altaActivoFijoActualizar.FechaAlta = altaActivoFijo.FechaAlta;
                            altaActivoFijoActualizar.IdMotivoAlta = altaActivoFijo.IdMotivoAlta;
                            altaActivoFijoActualizar.IdFacturaActivoFijo = altaActivoFijo.IdFacturaActivoFijo;
                            db.AltaActivoFijo.Update(altaActivoFijoActualizar);
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

        [HttpDelete("{id}")]
        public async Task<Response> DeleteAltaActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.AltaActivoFijo.SingleOrDefaultAsync(m => m.IdAltaActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.AltaActivoFijo.Remove(respuesta);
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
