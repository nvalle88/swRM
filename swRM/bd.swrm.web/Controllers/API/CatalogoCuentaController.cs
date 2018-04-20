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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/CatalogoCuenta")]
    public class CatalogoCuentaController : Controller
    {
        private readonly SwRMDbContext db;

        public CatalogoCuentaController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarCatalogosCuenta")]
        public async Task<List<CatalogoCuenta>> GetCatalogoCuenta()
        {
            try
            {
                return await db.CatalogoCuenta.OrderBy(x => x.Codigo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<CatalogoCuenta>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetCatalogosCuenta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var catalogoCuenta = await db.CatalogoCuenta.SingleOrDefaultAsync(m => m.IdCatalogoCuenta == id);
                return new Response { IsSuccess = catalogoCuenta != null, Message = catalogoCuenta != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = catalogoCuenta };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutCatalogoCuenta([FromRoute] int id, [FromBody] CatalogoCuenta catalogoCuenta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CatalogoCuenta.Where(c => c.Codigo.ToUpper().Trim() == catalogoCuenta.Codigo.ToUpper().Trim()).AnyAsync(c => c.IdCatalogoCuenta != catalogoCuenta.IdCatalogoCuenta))
                {
                    var catalogoCuentaActualizar = await db.CatalogoCuenta.Where(x => x.IdCatalogoCuenta == id).FirstOrDefaultAsync();
                    if (catalogoCuentaActualizar != null)
                    {
                        try
                        {
                            catalogoCuentaActualizar.Codigo = catalogoCuenta.Codigo;
                            catalogoCuentaActualizar.IdCatalogoCuentaHijo = catalogoCuenta.IdCatalogoCuentaHijo;
                            db.CatalogoCuenta.Update(catalogoCuentaActualizar);
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
        
        [HttpPost]
        [Route("InsertarCatalogoCuenta")]
        public async Task<Response> PostCatalogoCuenta([FromBody] CatalogoCuenta catalogoCuenta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CatalogoCuenta.AnyAsync(c => c.Codigo.ToUpper().Trim() == catalogoCuenta.Codigo.ToUpper().Trim()))
                {
                    db.CatalogoCuenta.Add(catalogoCuenta);
                    await db.SaveChangesAsync();

                    if (catalogoCuenta.IdCatalogoCuentaHijo == 0)
                    {
                        catalogoCuenta = db.CatalogoCuenta.FirstOrDefault();
                        if (catalogoCuenta != null)
                        {
                            catalogoCuenta.IdCatalogoCuentaHijo = catalogoCuenta.IdCatalogoCuenta;
                            await db.SaveChangesAsync();
                        }
                    }
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
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteCatalogoCuenta([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.CatalogoCuenta.SingleOrDefaultAsync(m => m.IdCatalogoCuenta == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.CatalogoCuenta.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(CatalogoCuenta catalogoCuenta)
        {
            var bdd = catalogoCuenta.Codigo.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.CatalogoCuenta.Where(p => p.Codigo.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}