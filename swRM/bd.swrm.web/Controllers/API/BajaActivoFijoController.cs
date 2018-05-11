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
    [Route("api/BajaActivoFijoDetalle")]
    public class BajaActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public BajaActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarBajaActivoFijoDetalle")]
        public async Task<List<BajaActivoFijo>> GetBajaActivoFijoDetalle()
        {
            try
            {
                return await db.BajaActivoFijo.Include(c=> c.MotivoBaja).OrderBy(x => x.FechaBaja).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<BajaActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetBajaActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaActivoFijoDetalle = await db.BajaActivoFijo.Include(c => c.MotivoBaja).SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
                return new Response { IsSuccess = bajaActivoFijoDetalle != null, Message = bajaActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bajaActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutBajaActivoFijoDetalle([FromRoute] int id, [FromBody] BajaActivoFijo bajaActivoFijoDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaActivoFijoDetalleActualizar = await db.BajaActivoFijo.Where(x => x.IdRecepcionActivoFijoDetalle == id).FirstOrDefaultAsync();
                if (bajaActivoFijoDetalleActualizar != null)
                {
                    try
                    {
                        bajaActivoFijoDetalleActualizar.FechaBaja = bajaActivoFijoDetalle.FechaBaja;
                        bajaActivoFijoDetalleActualizar.IdMotivoBaja = bajaActivoFijoDetalle.IdMotivoBaja;
                        bajaActivoFijoDetalleActualizar.MemoOficioResolucion = bajaActivoFijoDetalle.MemoOficioResolucion;
                        db.BajaActivoFijo.Update(bajaActivoFijoDetalleActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                        return new Response { IsSuccess = false, Message = Mensaje.Error };
                    }
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpPost]
        [Route("InsertarBajaActivoFijo")]
        public async Task<Response> PostBajaActivoFijoDetalle([FromBody] BajaActivoFijo bajaActivoFijoDetalle)
        {
            try
            {
                ModelState.Remove("IdActivo");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.BajaActivoFijo.AnyAsync(c => c.IdRecepcionActivoFijoDetalle == bajaActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                {
                    db.BajaActivoFijo.Add(bajaActivoFijoDetalle);
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
        public async Task<Response> DeleteBajaActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.BajaActivoFijo.SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.BajaActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(BajaActivoFijo bajaActivoFijoDetalle)
        {
            var bdd = bajaActivoFijoDetalle.IdRecepcionActivoFijoDetalle;
            var loglevelrespuesta = db.BajaActivoFijo.Where(p => p.IdRecepcionActivoFijoDetalle == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
