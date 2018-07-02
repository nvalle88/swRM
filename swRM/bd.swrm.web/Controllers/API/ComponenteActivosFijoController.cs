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
    [Route("api/ComponenteActivoFijo")]
    public class ComponenteActivosFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public ComponenteActivosFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarComponenteActivoFijo")]
        public async Task<List<ComponenteActivoFijo>> GetComponenteActivoFijo()
        {
            try
            {
                return await db.ComponenteActivoFijo.OrderBy(x => x.IdComponenteActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ComponenteActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetComponenteActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var componenteActivoFijo = await db.ComponenteActivoFijo.SingleOrDefaultAsync(m => m.IdComponenteActivoFijo == id);
                return new Response { IsSuccess = componenteActivoFijo != null, Message = componenteActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = componenteActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarComponenteActivoFijo")]
        public async Task<Response> PostMarca([FromBody]ComponenteActivoFijo componenteActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ComponenteActivoFijo.AnyAsync(c => c.IdRecepcionActivoFijoDetalleOrigen == componenteActivoFijo.IdRecepcionActivoFijoDetalleOrigen && c.IdRecepcionActivoFijoDetalleComponente == componenteActivoFijo.IdRecepcionActivoFijoDetalleComponente))
                {
                    db.ComponenteActivoFijo.Add(componenteActivoFijo);
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
        public async Task<Response> PutComponenteActivoFijo([FromRoute] int id, [FromBody]ComponenteActivoFijo componenteActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == componenteActivoFijo.IdRecepcionActivoFijoDetalleOrigen && c.IdRecepcionActivoFijoDetalleComponente == componenteActivoFijo.IdRecepcionActivoFijoDetalleComponente).AnyAsync(c => c.IdComponenteActivoFijo != componenteActivoFijo.IdComponenteActivoFijo))
                {
                    var componenteActivoFijoActualizar = await db.ComponenteActivoFijo.Where(x => x.IdComponenteActivoFijo == id).FirstOrDefaultAsync();
                    if (componenteActivoFijoActualizar != null)
                    {
                        try
                        {
                            componenteActivoFijoActualizar.IdRecepcionActivoFijoDetalleOrigen = componenteActivoFijo.IdRecepcionActivoFijoDetalleOrigen;
                            componenteActivoFijoActualizar.IdRecepcionActivoFijoDetalleComponente = componenteActivoFijo.IdRecepcionActivoFijoDetalleComponente;
                            componenteActivoFijoActualizar.FechaAdicion = componenteActivoFijo.FechaAdicion;
                            db.ComponenteActivoFijo.Update(componenteActivoFijoActualizar);
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
        public async Task<Response> DeleteComponenteActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ComponenteActivoFijo.SingleOrDefaultAsync(m => m.IdComponenteActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ComponenteActivoFijo.Remove(respuesta);
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
