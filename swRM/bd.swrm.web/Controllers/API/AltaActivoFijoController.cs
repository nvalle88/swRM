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
                return await db.AltaActivoFijo.Include(x=> x.Factura).ToListAsync();
                
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

                var altaActivoFijo = await db.AltaActivoFijo.Include(x => x.Factura).SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
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
                ModelState.Remove("IdFactura");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijo.AnyAsync(c => c.IdRecepcionActivoFijoDetalle == altaActivoFijo.IdRecepcionActivoFijoDetalle && c.IdFactura == altaActivoFijo.IdFactura))
                {
                    db.AltaActivoFijo.Add(altaActivoFijo);
                    await db.SaveChangesAsync();
                    Temporizador.Temporizador.InicializarTemporizadorDepreciacion();
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
        
        [HttpPut("{id}")]
        public async Task<Response> PutAltaActivoFijo([FromRoute] int id, [FromBody]AltaActivoFijo altaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == altaActivoFijo.IdRecepcionActivoFijoDetalle && c.IdFactura == altaActivoFijo.IdFactura).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != altaActivoFijo.IdRecepcionActivoFijoDetalle))
                {
                    var altaActivoFijoActualizar = await db.AltaActivoFijo.Where(x => x.IdRecepcionActivoFijoDetalle == id).FirstOrDefaultAsync();
                    if (altaActivoFijoActualizar != null)
                    {
                        try
                        {
                            altaActivoFijoActualizar.FechaAlta = altaActivoFijo.FechaAlta;
                            altaActivoFijoActualizar.IdFactura = altaActivoFijo.IdFactura;
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

                var respuesta = await db.AltaActivoFijo.SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
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

        public Response Existe(AltaActivoFijo altaActivoFijo)
        {
            var bdd = altaActivoFijo.IdRecepcionActivoFijoDetalle;
            var _bdd = altaActivoFijo.IdFactura;
            var loglevelrespuesta = db.AltaActivoFijo.Where(p => p.IdRecepcionActivoFijoDetalle == bdd && p.IdFactura == _bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
