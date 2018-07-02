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
    [Route("api/MotivoRecepcionArticulos")]
    public class MotivoRecepcionArticulosController : Controller
    {
        private readonly SwRMDbContext db;

        public MotivoRecepcionArticulosController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarMotivoRecepcionArticulos")]
        public async Task<List<MotivoRecepcionArticulos>> GetMotivoRecepcionArticulos()
        {
            try
            {
                return await db.MotivoRecepcionArticulos.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MotivoRecepcionArticulos>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetMotivoRecepcionArticulos([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var motivoRecepcionArticulos = await db.MotivoRecepcionArticulos.SingleOrDefaultAsync(m => m.IdMotivoRecepcionArticulos == id);
                return new Response { IsSuccess = motivoRecepcionArticulos != null, Message = motivoRecepcionArticulos != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = motivoRecepcionArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarMotivoRecepcionArticulos")]
        public async Task<Response> PostMotivoRecepcionArticulos([FromBody]MotivoRecepcionArticulos motivoRecepcionArticulos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoRecepcionArticulos.AnyAsync(c => c.Descripcion.ToUpper().Trim() == motivoRecepcionArticulos.Descripcion.ToUpper().Trim()))
                {
                    db.MotivoRecepcionArticulos.Add(motivoRecepcionArticulos);
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
        public async Task<Response> PutMotivoRecepcionArticulos([FromRoute] int id, [FromBody]MotivoRecepcionArticulos motivoRecepcionArticulos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MotivoRecepcionArticulos.Where(c => c.Descripcion.ToUpper().Trim() == motivoRecepcionArticulos.Descripcion.ToUpper().Trim()).AnyAsync(c => c.IdMotivoRecepcionArticulos != motivoRecepcionArticulos.IdMotivoRecepcionArticulos))
                {
                    var motivoRecepcionArticulosActualizar = await db.MotivoRecepcionArticulos.Where(x => x.IdMotivoRecepcionArticulos == id).FirstOrDefaultAsync();
                    if (motivoRecepcionArticulosActualizar != null)
                    {
                        try
                        {
                            motivoRecepcionArticulosActualizar.Descripcion = motivoRecepcionArticulos.Descripcion;
                            db.MotivoRecepcionArticulos.Update(motivoRecepcionArticulosActualizar);
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
        public async Task<Response> DeleteMotivoRecepcionArticulos([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MotivoRecepcionArticulos.SingleOrDefaultAsync(m => m.IdMotivoRecepcionArticulos == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MotivoRecepcionArticulos.Remove(respuesta);
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