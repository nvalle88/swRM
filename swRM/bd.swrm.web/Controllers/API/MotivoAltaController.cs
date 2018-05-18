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
    [Route("api/MotivoAlta")]
    public class MotivoAltaController : Controller
    {
        private readonly SwRMDbContext db;

        public MotivoAltaController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarMotivoAlta")]
        public async Task<List<MotivoAlta>> GetMotivoAlta()
        {
            try
            {
                return await db.MotivoAlta.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MotivoAlta>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetMotivoAlta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var motivoAlta = await db.MotivoAlta.SingleOrDefaultAsync(m => m.IdMotivoAlta == id);
                return new Response { IsSuccess = motivoAlta != null, Message = motivoAlta != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = motivoAlta };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutMotivoAlta([FromRoute] int id, [FromBody] MotivoAlta motivoAlta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoAlta.Where(c => c.Descripcion.ToUpper().Trim() == motivoAlta.Descripcion.ToUpper().Trim()).AnyAsync(c => c.IdMotivoAlta != motivoAlta.IdMotivoAlta))
                {
                    var motivoAltaActualizar = await db.MotivoAlta.Where(x => x.IdMotivoAlta == id).FirstOrDefaultAsync();
                    if (motivoAltaActualizar != null)
                    {
                        try
                        {
                            motivoAltaActualizar.Descripcion = motivoAlta.Descripcion;
                            db.MotivoAlta.Update(motivoAltaActualizar);
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
        [Route("InsertarMotivoAlta")]
        public async Task<Response> PostMotivoAlta([FromBody] MotivoAlta motivoAlta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoAlta.AnyAsync(c => c.Descripcion.ToUpper().Trim() == motivoAlta.Descripcion.ToUpper().Trim()))
                {
                    db.MotivoAlta.Add(motivoAlta);
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
        public async Task<Response> DeleteMotivoAlta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MotivoAlta.SingleOrDefaultAsync(m => m.IdMotivoAlta == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MotivoAlta.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(MotivoAlta motivoAlta)
        {
            var bdd = motivoAlta.Descripcion.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.MotivoAlta.Where(p => p.Descripcion.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}