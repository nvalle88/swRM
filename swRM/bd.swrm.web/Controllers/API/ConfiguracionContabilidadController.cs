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
    [Route("api/ConfiguracionContabilidad")]
    public class ConfiguracionContabilidadController : Controller
    {
        private readonly SwRMDbContext db;

        public ConfiguracionContabilidadController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarConfiguracionContabilidad")]
        public async Task<List<ConfiguracionContabilidad>> GetConfiguracionContabilidad()
        {
            try
            {
                return await db.ConfiguracionContabilidad.Include(c=> c.CatalogoCuentaD).Include(c=> c.CatalogoCuentaH).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ConfiguracionContabilidad>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetConfiguracionContabilidad([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var configuracionContabilidad = await db.ConfiguracionContabilidad.Include(c => c.CatalogoCuentaD).Include(c => c.CatalogoCuentaH).SingleOrDefaultAsync(m => m.IdConfiguracionContabilidad == id);
                return new Response { IsSuccess = configuracionContabilidad != null, Message = configuracionContabilidad != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = configuracionContabilidad };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarConfiguracionContabilidad")]
        public async Task<Response> PostConfiguracionContabilidad([FromBody]ConfiguracionContabilidad configuracionContabilidad)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ConfiguracionContabilidad.AnyAsync(c => c.IdCatalogoCuentaD == configuracionContabilidad.IdCatalogoCuentaD && c.IdCatalogoCuentaH == configuracionContabilidad.IdCatalogoCuentaH))
                {
                    db.ConfiguracionContabilidad.Add(configuracionContabilidad);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutConfiguracionContabilidad([FromRoute] int id, [FromBody]ConfiguracionContabilidad configuracionContabilidad)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ConfiguracionContabilidad.Where(c => c.IdCatalogoCuentaD == configuracionContabilidad.IdCatalogoCuentaD && c.IdCatalogoCuentaH == configuracionContabilidad.IdCatalogoCuentaH).AnyAsync(c => c.IdConfiguracionContabilidad != configuracionContabilidad.IdConfiguracionContabilidad))
                {
                    var configuracionContabilidadActualizar = await db.ConfiguracionContabilidad.Where(x => x.IdConfiguracionContabilidad == id).FirstOrDefaultAsync();
                    if (configuracionContabilidadActualizar != null)
                    {
                        try
                        {
                            configuracionContabilidadActualizar.IdCatalogoCuentaD = configuracionContabilidad.IdCatalogoCuentaD;
                            configuracionContabilidadActualizar.IdCatalogoCuentaH = configuracionContabilidad.IdCatalogoCuentaH;
                            configuracionContabilidadActualizar.ValorD = configuracionContabilidad.ValorD;
                            configuracionContabilidadActualizar.ValorH = configuracionContabilidad.ValorH;
                            db.ConfiguracionContabilidad.Update(configuracionContabilidadActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
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
        public async Task<Response> DeleteConfiguracionContabilidad([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ConfiguracionContabilidad.SingleOrDefaultAsync(m => m.IdConfiguracionContabilidad == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ConfiguracionContabilidad.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
    }
}