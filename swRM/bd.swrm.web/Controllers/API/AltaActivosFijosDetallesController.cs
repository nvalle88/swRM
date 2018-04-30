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
    [Route("api/AltaActivosFijosDetalles")]
    public class AltaActivosFijosDetallesController : Controller
    {
        private readonly SwRMDbContext db;

        public AltaActivosFijosDetallesController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarAltasActivosFijosDetalles")]
        public async Task<List<AltaActivoFijoDetalle>> GetAltaActivosFijosDetalles()
        {
            try
            {
                return await db.AltaActivoFijoDetalle.Include(x => x.ActivoFijo).Include(x=> x.Factura).ToListAsync();
                
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<AltaActivoFijoDetalle>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetAltaActivosFijosDetalles([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var altaActivosFijosDetalles = await db.AltaActivoFijoDetalle.Include(x => x.ActivoFijo).Include(x => x.Factura).SingleOrDefaultAsync(m => m.IdActivoFijoAlta == id);
                return new Response { IsSuccess = altaActivosFijosDetalles != null, Message = altaActivosFijosDetalles != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = altaActivosFijosDetalles };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarAltaActivoFijoDetalle")]
        public async Task<Response> PostAltaActivosFijosDetalles([FromBody]AltaActivoFijoDetalle altaActivosFijosDetalles)
        {
            try
            {
                ModelState.Remove("IdFactura");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijoDetalle.AnyAsync(c => c.IdActivoFijoAlta == altaActivosFijosDetalles.IdActivoFijoAlta && c.IdFactura == altaActivosFijosDetalles.IdFactura))
                {
                    db.AltaActivoFijoDetalle.Add(altaActivosFijosDetalles);
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
        public async Task<Response> PutAltaActivosFijosDetalles([FromRoute] int id, [FromBody]AltaActivoFijoDetalle altaActivosFijosDetalles)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.AltaActivoFijoDetalle.Where(c => c.IdActivoFijoAlta == altaActivosFijosDetalles.IdActivoFijoAlta && c.IdFactura == altaActivosFijosDetalles.IdFactura).AnyAsync(c => c.IdActivoFijoAlta != altaActivosFijosDetalles.IdActivoFijoAlta))
                {
                    var altaActivosFijosDetallesActualizar = await db.AltaActivoFijoDetalle.Where(x => x.IdActivoFijoAlta == id).FirstOrDefaultAsync();
                    if (altaActivosFijosDetallesActualizar != null)
                    {
                        try
                        {
                            altaActivosFijosDetallesActualizar.FechaAlta = altaActivosFijosDetalles.FechaAlta;
                            altaActivosFijosDetallesActualizar.IdFactura = altaActivosFijosDetalles.IdFactura;
                            altaActivosFijosDetallesActualizar.IdActivoFijo = altaActivosFijosDetalles.IdActivoFijo;
                            db.AltaActivoFijoDetalle.Update(altaActivosFijosDetallesActualizar);
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
        public async Task<Response> DeleteAltaActivosFijosDetalles([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.AltaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdActivoFijoAlta == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.AltaActivoFijoDetalle.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(AltaActivoFijoDetalle altaActivosFijosDetalles)
        {
            var bdd = altaActivosFijosDetalles.IdActivoFijoAlta;
            var _bdd = altaActivosFijosDetalles.IdFactura;
            var loglevelrespuesta = db.AltaActivoFijoDetalle.Where(p => p.IdActivoFijoAlta == bdd && p.IdFactura == _bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
