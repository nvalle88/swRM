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
    [Route("api/ActivoFijoComponentes")]
    public class ActivosFijoComponentesController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivosFijoComponentesController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarActivosFijosComponentes")]
        public async Task<List<ActivoFijoComponentes>> GetActivosFijosComponentes()
        {
            try
            {
                return await db.ActivoFijoComponentes.Include(x => x.ActivoFijoOrigen).Include(x=> x.ActivoFijoComponente).OrderBy(x => x.IdAdicion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijoComponentes>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijosComponentes([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activosFijosComponentes = await db.ActivoFijoComponentes.Include(x => x.ActivoFijoOrigen).Include(x => x.ActivoFijoComponente).SingleOrDefaultAsync(m => m.IdAdicion == id);
                return new Response { IsSuccess = activosFijosComponentes != null, Message = activosFijosComponentes != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activosFijosComponentes };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarActivosFijosComponentes")]
        public async Task<Response> PostMarca([FromBody]ActivoFijoComponentes activosFijosComponentes)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivoFijoComponentes.AnyAsync(c => c.IdActivoFijoOrigen == activosFijosComponentes.IdActivoFijoOrigen && c.IdActivoFijoComponente == activosFijosComponentes.IdActivoFijoComponente))
                {
                    db.ActivoFijoComponentes.Add(activosFijosComponentes);
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
        public async Task<Response> PutActivosFijosComponentes([FromRoute] int id, [FromBody]ActivoFijoComponentes activosFijosComponentes)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivoFijoComponentes.Where(c => c.IdActivoFijoOrigen == activosFijosComponentes.IdActivoFijoOrigen && c.IdActivoFijoComponente == activosFijosComponentes.IdActivoFijoComponente).AnyAsync(c => c.IdAdicion != activosFijosComponentes.IdAdicion))
                {
                    var activosFijosComponentesActualizar = await db.ActivoFijoComponentes.Where(x => x.IdAdicion == id).FirstOrDefaultAsync();
                    if (activosFijosComponentesActualizar != null)
                    {
                        try
                        {
                            activosFijosComponentesActualizar.IdActivoFijoOrigen = activosFijosComponentes.IdActivoFijoOrigen;
                            activosFijosComponentesActualizar.IdActivoFijoComponente = activosFijosComponentes.IdActivoFijoComponente;
                            activosFijosComponentesActualizar.FechaAdicion = activosFijosComponentes.FechaAdicion;
                            db.ActivoFijoComponentes.Update(activosFijosComponentesActualizar);
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
        public async Task<Response> DeleteActivosFijosComponentes([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivoFijoComponentes.SingleOrDefaultAsync(m => m.IdAdicion == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ActivoFijoComponentes.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ActivoFijoComponentes activosFijosComponentes)
        {
            var bdd = activosFijosComponentes.IdActivoFijoOrigen;
            var _bdd = activosFijosComponentes.IdActivoFijoComponente;
            var loglevelrespuesta = db.ActivoFijoComponentes.Where(p => p.IdActivoFijoOrigen == bdd && p.IdActivoFijoComponente == _bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
