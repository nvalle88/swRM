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
using bd.swrm.entidades.Utils;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/FacturaPorAltaProveeduria")]
    public class FacturaPorAltaProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public FacturaPorAltaProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarFacturasPorAltaProveeduria")]
        public async Task<List<FacturasPorAltaProveeduria>> GetFacturasPorAltaProveeduria()
        {
            try
            {
                return await db.FacturasPorAltaProveeduria.OrderBy(x => x.NumeroFactura).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<FacturasPorAltaProveeduria>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetFacturasPorAltaProveeduria([FromRoute]string id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _factura = await db.FacturasPorAltaProveeduria.SingleOrDefaultAsync(m => m.NumeroFactura == id);
                return new Response { IsSuccess = _factura != null, Message = _factura != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = _factura };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarFacturasPorAltaProveeduria")]
        public async Task<Response> PostFacturasPorAltaProveeduria([FromBody]FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.FacturasPorAltaProveeduria.AnyAsync(c => c.NumeroFactura.ToUpper().Trim() == _FacturasPorAltaProveeduria.NumeroFactura.ToUpper().Trim()))
                {
                    db.FacturasPorAltaProveeduria.Add(_FacturasPorAltaProveeduria);
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
        public async Task<Response> PutFacturasPorAltaProveeduria([FromRoute] int id, [FromBody]FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _FacturasPorAltaProveeduriaActualizar = await db.FacturasPorAltaProveeduria.Where(x => x.IdFacturasPorAlta == id).FirstOrDefaultAsync();
                if (_FacturasPorAltaProveeduriaActualizar != null)
                {
                    try
                    {
                        _FacturasPorAltaProveeduriaActualizar.NumeroFactura = _FacturasPorAltaProveeduria.NumeroFactura;
                        _FacturasPorAltaProveeduriaActualizar.IdAlta = _FacturasPorAltaProveeduria.IdAlta;
                        db.FacturasPorAltaProveeduria.Update(_FacturasPorAltaProveeduriaActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                        return new Response { IsSuccess = false, Message = Mensaje.Error };
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
        public async Task<Response> DeleteFacturasPorAltaProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.FacturasPorAltaProveeduria.SingleOrDefaultAsync(m => m.IdFacturasPorAlta == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.FacturasPorAltaProveeduria.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
        {
            var bdd = _FacturasPorAltaProveeduria.NumeroFactura;
            var loglevelrespuesta = db.FacturasPorAltaProveeduria.Where(p => p.NumeroFactura == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}