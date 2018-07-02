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
    [Route("api/MotivoSalidaArticulos")]
    public class MotivoSalidaArticulosController : Controller
    {
        private readonly SwRMDbContext db;

        public MotivoSalidaArticulosController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarMotivoSalidaArticulos")]
        public async Task<List<MotivoSalidaArticulos>> GetMotivoSalidaArticulos()
        {
            try
            {
                return await db.MotivoSalidaArticulos.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MotivoSalidaArticulos>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetMotivoSalidaArticulos([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var motivoSalidaArticulos = await db.MotivoSalidaArticulos.SingleOrDefaultAsync(m => m.IdMotivoSalidaArticulos == id);
                return new Response { IsSuccess = motivoSalidaArticulos != null, Message = motivoSalidaArticulos != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = motivoSalidaArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarMotivoSalidaArticulos")]
        public async Task<Response> PostMotivoSalidaArticulos([FromBody]MotivoSalidaArticulos motivoSalidaArticulos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoSalidaArticulos.AnyAsync(c => c.Descripcion.ToUpper().Trim() == motivoSalidaArticulos.Descripcion.ToUpper().Trim()))
                {
                    db.MotivoSalidaArticulos.Add(motivoSalidaArticulos);
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
        public async Task<Response> PutMotivoSalidaArticulos([FromRoute] int id, [FromBody]MotivoSalidaArticulos motivoSalidaArticulos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoSalidaArticulos.Where(c => c.Descripcion.ToUpper().Trim() == motivoSalidaArticulos.Descripcion.ToUpper().Trim()).AnyAsync(c => c.IdMotivoSalidaArticulos != motivoSalidaArticulos.IdMotivoSalidaArticulos))
                {
                    var motivoSalidaArticulosActualizar = await db.MotivoSalidaArticulos.Where(x => x.IdMotivoSalidaArticulos == id).FirstOrDefaultAsync();
                    if (motivoSalidaArticulosActualizar != null)
                    {
                        try
                        {
                            motivoSalidaArticulosActualizar.Descripcion = motivoSalidaArticulos.Descripcion;
                            db.MotivoSalidaArticulos.Update(motivoSalidaArticulosActualizar);
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
        public async Task<Response> DeleteMotivoSalidaArticulos([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MotivoSalidaArticulos.SingleOrDefaultAsync(m => m.IdMotivoSalidaArticulos == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MotivoSalidaArticulos.Remove(respuesta);
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