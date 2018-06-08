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
    [Route("api/RevalorizacionActivoFijo")]
    public class RevalorizacionActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public RevalorizacionActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarRevalorizacionActivoFijo")]
        public async Task<List<RevalorizacionActivoFijo>> GetRevalorizacionActivoFijo()
        {
            try
            {
                return await db.RevalorizacionActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).OrderByDescending(x => x.FechaRevalorizacion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RevalorizacionActivoFijo>();
            }
        }

        [HttpPost]
        [Route("ListarRevalorizacionActivoFijoPorIdDetalleActivoFijo")]
        public async Task<List<RevalorizacionActivoFijo>> GetRevalorizacionActivoFijoPorIdDetalleActivoFijo([FromBody] int idRecepcionActivoFijoDetalle)
        {
            try
            {
                return await db.RevalorizacionActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle).OrderByDescending(x => x.FechaRevalorizacion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RevalorizacionActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetRevalorizacionActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var revalorizacionActivoFijo = await db.RevalorizacionActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c=> c.ActivoFijo).SingleOrDefaultAsync(m => m.IdRevalorizacionActivoFijo == id);
                return new Response { IsSuccess = revalorizacionActivoFijo != null, Message = revalorizacionActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = revalorizacionActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("UltimoValorCompra")]
        public async Task<decimal> GetUltimaRevalorizacionActivoFijo([FromBody]int id)
        {
            try
            {
                var revalorizacionActivoFijo = await db.RevalorizacionActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.ActivoFijo).OrderByDescending(x => x.FechaRevalorizacion).FirstOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
                return revalorizacionActivoFijo != null ? revalorizacionActivoFijo.ValorCompra : (await db.RecepcionActivoFijoDetalle.Include(c=> c.ActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == id)).ActivoFijo.ValorCompra;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpPost]
        [Route("InsertarRevalorizacionActivoFijo")]
        public async Task<Response> PostRevalorizacionActivoFijo([FromBody]RevalorizacionActivoFijo revalorizacionActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.RevalorizacionActivoFijo.AnyAsync(c => c.ValorCompra == revalorizacionActivoFijo.ValorCompra && c.IdRecepcionActivoFijoDetalle == revalorizacionActivoFijo.IdRecepcionActivoFijoDetalle))
                {
                    db.RevalorizacionActivoFijo.Add(revalorizacionActivoFijo);
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
        public async Task<Response> PutRevalorizacionActivoFijo([FromRoute] int id, [FromBody]RevalorizacionActivoFijo revalorizacionActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.RevalorizacionActivoFijo.Where(c => c.ValorCompra == revalorizacionActivoFijo.ValorCompra && c.IdRecepcionActivoFijoDetalle == revalorizacionActivoFijo.IdRecepcionActivoFijoDetalle).AnyAsync(c => c.IdRevalorizacionActivoFijo != revalorizacionActivoFijo.IdRevalorizacionActivoFijo))
                {
                    var revalorizacionActivoFijoActualizar = await db.RevalorizacionActivoFijo.Where(x => x.IdRevalorizacionActivoFijo == id).FirstOrDefaultAsync();
                    if (revalorizacionActivoFijoActualizar != null)
                    {
                        try
                        {
                            revalorizacionActivoFijoActualizar.ValorCompra = revalorizacionActivoFijo.ValorCompra;
                            revalorizacionActivoFijoActualizar.FechaRevalorizacion = revalorizacionActivoFijo.FechaRevalorizacion;
                            revalorizacionActivoFijoActualizar.IdRecepcionActivoFijoDetalle = revalorizacionActivoFijo.IdRecepcionActivoFijoDetalle;
                            db.RevalorizacionActivoFijo.Update(revalorizacionActivoFijoActualizar);
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
        public async Task<Response> DeleteRevalorizacionActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.RevalorizacionActivoFijo.SingleOrDefaultAsync(m => m.IdRevalorizacionActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.RevalorizacionActivoFijo.Remove(respuesta);
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