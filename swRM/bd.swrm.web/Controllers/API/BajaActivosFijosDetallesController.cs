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
    [Route("api/BajaActivosFijosDetalles")]
    public class BajaActivosFijosDetallesController : Controller
    {
        private readonly SwRMDbContext db;

        public BajaActivosFijosDetallesController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarBajaActivosFijosDetalles")]
        public async Task<List<BajaActivoFijoDetalle>> GetBajaActivosFijosDetalles()
        {
            try
            {
                return await db.BajaActivoFijoDetalle.Include(c=> c.ActivoFijo).OrderBy(x => x.FechaBaja).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<BajaActivoFijoDetalle>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetBajaActivosFijosDetalles([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaActivosFijosDetalles = await db.BajaActivoFijoDetalle.Include(c => c.ActivoFijo).SingleOrDefaultAsync(m => m.IdActivoFijoBaja == id);
                return new Response { IsSuccess = bajaActivosFijosDetalles != null, Message = bajaActivosFijosDetalles != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bajaActivosFijosDetalles };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutBajaActivosFijosDetalles([FromRoute] int id, [FromBody] BajaActivoFijoDetalle bajaActivosFijosDetalles)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaActivosFijosDetallesActualizar = await db.BajaActivoFijoDetalle.Where(x => x.IdActivoFijoBaja == id).FirstOrDefaultAsync();
                if (bajaActivosFijosDetallesActualizar != null)
                {
                    try
                    {
                        bajaActivosFijosDetallesActualizar.FechaBaja = bajaActivosFijosDetalles.FechaBaja;
                        bajaActivosFijosDetallesActualizar.IdMotivoBaja = bajaActivosFijosDetalles.IdMotivoBaja;
                        bajaActivosFijosDetallesActualizar.IdActivo = bajaActivosFijosDetalles.IdActivo;
                        bajaActivosFijosDetallesActualizar.MemoOficioResolucion = bajaActivosFijosDetalles.MemoOficioResolucion;
                        db.BajaActivoFijoDetalle.Update(bajaActivosFijosDetallesActualizar);
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
        [Route("InsertarBajaActivoFijoDetalle")]
        public async Task<Response> PostBajaActivosFijosDetalles([FromBody] BajaActivoFijoDetalle bajaActivosFijosDetalles)
        {
            try
            {
                ModelState.Remove("IdActivo");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.BajaActivoFijoDetalle.AnyAsync(c => c.IdActivoFijoBaja == bajaActivosFijosDetalles.IdActivoFijoBaja))
                {
                    db.BajaActivoFijoDetalle.Add(bajaActivosFijosDetalles);
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
        public async Task<Response> DeleteBajaActivosFijosDetalles([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.BajaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdActivoFijoBaja == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.BajaActivoFijoDetalle.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(BajaActivoFijoDetalle bajaActivosFijosDetalles)
        {
            var bdd = bajaActivosFijosDetalles.IdActivoFijoBaja;
            var loglevelrespuesta = db.BajaActivoFijoDetalle.Where(p => p.IdActivoFijoBaja == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
