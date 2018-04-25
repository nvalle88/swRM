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
    [Route("api/ActivosFijosAlta")]
    public class ActivosFijosAltaController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivosFijosAltaController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarAltasActivosFijos")]
        public async Task<List<ActivosFijosAlta>> GetActivosFijosAlta()
        {
            try
            {
                return await db.ActivosFijosAlta.Include(x => x.ActivoFijo).ToListAsync();
                
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivosFijosAlta>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijosAlta([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activosFijosAlta = await db.ActivosFijosAlta.SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                return new Response { IsSuccess = activosFijosAlta != null, Message = activosFijosAlta != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activosFijosAlta };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarActivosFijosAlta")]
        public async Task<Response> PostActivosFijosAlta([FromBody]ActivosFijosAlta activosFijosAlta)
        {
            try
            {
                ModelState.Remove("IdFactura");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivosFijosAlta.AnyAsync(c => c.IdActivoFijo == activosFijosAlta.IdActivoFijo && c.IdFactura == activosFijosAlta.IdFactura))
                {
                    db.ActivosFijosAlta.Add(activosFijosAlta);
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
        public async Task<Response> PutActivosFijosAlta([FromRoute] int id, [FromBody]ActivosFijosAlta activosFijosAlta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivosFijosAlta.Where(c => c.IdActivoFijo == activosFijosAlta.IdActivoFijo && c.IdFactura == activosFijosAlta.IdFactura).AnyAsync(c => c.IdActivoFijo != activosFijosAlta.IdActivoFijo))
                {
                    var _ActivosFijosAltaActualizar = await db.ActivosFijosAlta.Where(x => x.IdActivoFijo == id).FirstOrDefaultAsync();
                    if (_ActivosFijosAltaActualizar != null)
                    {
                        try
                        {
                            _ActivosFijosAltaActualizar.FechaAlta = activosFijosAlta.FechaAlta;
                            db.ActivosFijosAlta.Update(_ActivosFijosAltaActualizar);
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
        public async Task<Response> DeleteActivosFijosAlta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivosFijosAlta.SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ActivosFijosAlta.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ActivosFijosAlta _ActivosFijosAlta)
        {
            var bdd = _ActivosFijosAlta.IdActivoFijo;
            var _bdd = _ActivosFijosAlta.IdFactura;
            var loglevelrespuesta = db.ActivosFijosAlta.Where(p => p.IdActivoFijo == bdd && p.IdFactura == _bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
