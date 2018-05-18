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
    [Route("api/TipoUtilizacionAlta")]
    public class TipoUtilizacionAltaController : Controller
    {
        private readonly SwRMDbContext db;

        public TipoUtilizacionAltaController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarTipoUtilizacionAlta")]
        public async Task<List<TipoUtilizacionAlta>> GetTipoUtilizacionAlta()
        {
            try
            {
                return await db.TipoUtilizacionAlta.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<TipoUtilizacionAlta>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetTipoUtilizacionAlta([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var tipoUtilizacionAlta = await db.TipoUtilizacionAlta.SingleOrDefaultAsync(m => m.IdTipoUtilizacionAlta == id);
                return new Response { IsSuccess = tipoUtilizacionAlta != null, Message = tipoUtilizacionAlta != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = tipoUtilizacionAlta };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarTipoUtilizacionAlta")]
        public async Task<Response> PostTipoUtilizacionAlta([FromBody]TipoUtilizacionAlta tipoUtilizacionAlta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TipoUtilizacionAlta.AnyAsync(c => c.Nombre.ToUpper().Trim() == tipoUtilizacionAlta.Nombre.ToUpper().Trim()))
                {
                    db.TipoUtilizacionAlta.Add(tipoUtilizacionAlta);
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
        public async Task<Response> PutTipoUtilizacionAlta([FromRoute] int id, [FromBody]TipoUtilizacionAlta tipoUtilizacionAlta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TipoUtilizacionAlta.Where(c => c.Nombre.ToUpper().Trim() == tipoUtilizacionAlta.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdTipoUtilizacionAlta != tipoUtilizacionAlta.IdTipoUtilizacionAlta))
                {
                    var tipoUtilizacionAltaActualizar = await db.TipoUtilizacionAlta.Where(x => x.IdTipoUtilizacionAlta == id).FirstOrDefaultAsync();
                    if (tipoUtilizacionAltaActualizar != null)
                    {
                        try
                        {
                            tipoUtilizacionAltaActualizar.Nombre = tipoUtilizacionAlta.Nombre;
                            db.TipoUtilizacionAlta.Update(tipoUtilizacionAltaActualizar);
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
        public async Task<Response> DeleteTipoUtilizacionAlta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.TipoUtilizacionAlta.SingleOrDefaultAsync(m => m.IdTipoUtilizacionAlta == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.TipoUtilizacionAlta.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(TipoUtilizacionAlta tipoUtilizacionAlta)
        {
            var bdd = tipoUtilizacionAlta.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.TipoUtilizacionAlta.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}