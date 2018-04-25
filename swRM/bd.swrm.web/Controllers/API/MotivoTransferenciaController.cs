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
    [Route("api/MotivoTransferencia")]
    public class MotivoTransferenciaController : Controller
    {
        private readonly SwRMDbContext db;

        public MotivoTransferenciaController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarMotivoTransferencia")]
        public async Task<List<MotivoTransferencia>> GetMotivoTransferencia()
        {
            try
            {
                return await db.MotivoTransferencia.OrderBy(x => x.Motivo_Transferencia).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MotivoTransferencia>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetMotivoTransferencia([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _motivoTransferencia = await db.MotivoTransferencia.SingleOrDefaultAsync(m => m.IdMotivoTransferencia == id);
                return new Response { IsSuccess = _motivoTransferencia != null, Message = _motivoTransferencia != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = _motivoTransferencia };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarMotivoTransferencia")]
        public async Task<Response> PostMotivoTransferencia([FromBody]MotivoTransferencia _motivoTransferencia)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoTransferencia.AnyAsync(c => c.Motivo_Transferencia.ToUpper().Trim() == _motivoTransferencia.Motivo_Transferencia.ToUpper().Trim()))
                {
                    db.MotivoTransferencia.Add(_motivoTransferencia);
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
        public async Task<Response> PutMotivoTransferencia([FromRoute] int id, [FromBody]MotivoTransferencia _motivoTransferencia)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoTransferencia.Where(c => c.Motivo_Transferencia.ToUpper().Trim() == _motivoTransferencia.Motivo_Transferencia.ToUpper().Trim()).AnyAsync(c => c.IdMotivoTransferencia != _motivoTransferencia.IdMotivoTransferencia))
                {
                    var _motivoTransferenciaActualizar = await db.MotivoTransferencia.Where(x => x.IdMotivoTransferencia == id).FirstOrDefaultAsync();
                    if (_motivoTransferenciaActualizar != null)
                    {
                        try
                        {
                            _motivoTransferenciaActualizar.Motivo_Transferencia = _motivoTransferencia.Motivo_Transferencia;
                            db.MotivoTransferencia.Update(_motivoTransferenciaActualizar);
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
        public async Task<Response> DeleteMotivoTransferencia([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MotivoTransferencia.SingleOrDefaultAsync(m => m.IdMotivoTransferencia == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MotivoTransferencia.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(MotivoTransferencia _motivoTransferencia)
        {
            var bdd = _motivoTransferencia.Motivo_Transferencia.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.MotivoTransferencia.Where(p => p.Motivo_Transferencia.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
