using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.swrm.entidades.Utils;
using bd.log.guardar.Enumeradores;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/TransferenciaActivoFijo")]
    public class TransferenciaActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public TransferenciaActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarTransferenciaActivoFijo")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciaActivoFijo()
        {
            try
            {
                return await db.TransferenciaActivoFijo.OrderBy(x => x.FechaTransferencia).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<TransferenciaActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetTransferenciaActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _TransferenciaActivoFijo = await db.TransferenciaActivoFijo.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijo == id);
                return new Response { IsSuccess = _TransferenciaActivoFijo != null, Message = _TransferenciaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = _TransferenciaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarTransferenciaActivoFijo")]
        public async Task<Response> PostTransferenciaActivoFijo([FromBody]TransferenciaActivoFijo transferenciaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TransferenciaActivoFijo.AnyAsync(p => p.IdEmpleadoRegistra == transferenciaActivoFijo.IdEmpleadoRegistra && p.IdEmpleadoResponsableEnvio == transferenciaActivoFijo.IdEmpleadoResponsableEnvio && p.IdEmpleadoResponsableRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdEmpleadoRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdMotivoTransferencia == transferenciaActivoFijo.IdMotivoTransferencia && p.FechaTransferencia == transferenciaActivoFijo.FechaTransferencia && p.Destino == transferenciaActivoFijo.Destino && p.Origen == transferenciaActivoFijo.Origen && p.Observaciones == transferenciaActivoFijo.Observaciones))
                {
                    db.TransferenciaActivoFijo.Add(transferenciaActivoFijo);
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
        
        [HttpPut("{id}")]
        public async Task<Response> PutTransferenciaActivoFijo([FromRoute] int id, [FromBody]TransferenciaActivoFijo transferenciaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TransferenciaActivoFijo.Where(p => p.IdEmpleadoRegistra == transferenciaActivoFijo.IdEmpleadoRegistra && p.IdEmpleadoResponsableEnvio == transferenciaActivoFijo.IdEmpleadoResponsableEnvio && p.IdEmpleadoResponsableRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdEmpleadoRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdMotivoTransferencia == transferenciaActivoFijo.IdMotivoTransferencia && p.FechaTransferencia == transferenciaActivoFijo.FechaTransferencia && p.Destino == transferenciaActivoFijo.Destino && p.Origen == transferenciaActivoFijo.Origen && p.Observaciones == transferenciaActivoFijo.Observaciones).AnyAsync(c => c.IdTransferenciaActivoFijo != transferenciaActivoFijo.IdTransferenciaActivoFijo))
                {
                    var transferenciaActivoFijoActualizar = await db.TransferenciaActivoFijo.Where(x => x.IdTransferenciaActivoFijo == id).FirstOrDefaultAsync();
                    if (transferenciaActivoFijoActualizar != null)
                    {
                        try
                        {
                            transferenciaActivoFijoActualizar.IdEmpleadoRegistra = transferenciaActivoFijo.IdEmpleadoRegistra;
                            transferenciaActivoFijoActualizar.IdEmpleadoResponsableEnvio = transferenciaActivoFijo.IdEmpleadoResponsableEnvio;
                            transferenciaActivoFijoActualizar.IdEmpleadoResponsableRecibo = transferenciaActivoFijo.IdEmpleadoResponsableRecibo;
                            transferenciaActivoFijoActualizar.IdEmpleadoRecibo = transferenciaActivoFijo.IdEmpleadoRecibo;
                            transferenciaActivoFijoActualizar.IdMotivoTransferencia = transferenciaActivoFijo.IdMotivoTransferencia;
                            transferenciaActivoFijoActualizar.FechaTransferencia = transferenciaActivoFijo.FechaTransferencia;
                            transferenciaActivoFijoActualizar.Origen = transferenciaActivoFijo.Origen;
                            transferenciaActivoFijoActualizar.Destino = transferenciaActivoFijo.Destino;
                            transferenciaActivoFijoActualizar.Observaciones = transferenciaActivoFijo.Observaciones;
                            db.TransferenciaActivoFijo.Update(transferenciaActivoFijoActualizar);
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
        public async Task<Response> DeleteTransferenciaActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.TransferenciaActivoFijo.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.TransferenciaActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(TransferenciaActivoFijo transferenciaActivoFijo)
        {
            var loglevelrespuesta = db.TransferenciaActivoFijo.Where(p => p.IdEmpleadoRegistra == transferenciaActivoFijo.IdEmpleadoRegistra && p.IdEmpleadoResponsableEnvio == transferenciaActivoFijo.IdEmpleadoResponsableEnvio && p.IdEmpleadoResponsableRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdEmpleadoRecibo == transferenciaActivoFijo.IdEmpleadoResponsableRecibo && p.IdMotivoTransferencia == transferenciaActivoFijo.IdMotivoTransferencia && p.FechaTransferencia == transferenciaActivoFijo.FechaTransferencia && p.Destino == transferenciaActivoFijo.Destino && p.Origen == transferenciaActivoFijo.Origen && p.Observaciones == transferenciaActivoFijo.Observaciones).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}