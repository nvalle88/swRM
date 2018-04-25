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
    [Route("api/TablaDepreciacion")]
    public class TablaDepreciacionController : Controller
    {
        private readonly SwRMDbContext db;

        public TablaDepreciacionController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarTablaDepreciacion")]
        public async Task<List<TablaDepreciacion>> GetTablaDepreciacion()
        {
            try
            {
                return await db.TablaDepreciacion.OrderBy(x => x.IdTablaDepreciacion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<TablaDepreciacion>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetTablaDepreciacion([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var tablaDepreciacion = await db.TablaDepreciacion.SingleOrDefaultAsync(m => m.IdTablaDepreciacion == id);
                return new Response { IsSuccess = tablaDepreciacion != null, Message = tablaDepreciacion != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = tablaDepreciacion };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutTablaDepreciacion([FromRoute] int id, [FromBody] TablaDepreciacion tablaDepreciacion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TablaDepreciacion.Where(c => c.IndiceDepreciacion == tablaDepreciacion.IndiceDepreciacion).AnyAsync(c => c.IdTablaDepreciacion != tablaDepreciacion.IdTablaDepreciacion))
                {
                    var tablaDepreciacionActualizar = await db.TablaDepreciacion.Where(x => x.IdTablaDepreciacion == id).FirstOrDefaultAsync();
                    if (tablaDepreciacionActualizar != null)
                    {
                        try
                        {
                            tablaDepreciacionActualizar.IdTablaDepreciacion = tablaDepreciacion.IdTablaDepreciacion;
                            db.TablaDepreciacion.Update(tablaDepreciacionActualizar);
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
        [Route("InsertarTablaDepreciacion")]
        public async Task<Response> PostTablaDepreciacion([FromBody] TablaDepreciacion tablaDepreciacion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TablaDepreciacion.AnyAsync(c => c.IndiceDepreciacion == tablaDepreciacion.IndiceDepreciacion))
                {
                    db.TablaDepreciacion.Add(tablaDepreciacion);
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
        public async Task<Response> DeleteTablaDepreciacion([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.TablaDepreciacion.SingleOrDefaultAsync(m => m.IdTablaDepreciacion == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.TablaDepreciacion.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(TablaDepreciacion TablaDepreciacion)
        {
            var bdd = TablaDepreciacion.IndiceDepreciacion;
            var loglevelrespuesta = db.TablaDepreciacion.Where(p => p.IndiceDepreciacion == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
