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
    [Route("api/ActivosFijosAdicionados")]
    public class ActivosFijosAdicionadosController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivosFijosAdicionadosController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarActivosFijosAdicionados")]
        public async Task<List<ActivosFijosAdicionados>> GetActivosFijosAdicionados()
        {
            try
            {
                return await db.ActivosFijosAdicionados.OrderBy(x => x.idAdicion).Include(x => x.ActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivosFijosAdicionados>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijosAdicionados([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activosFijosAdicionados = await db.ActivosFijosAdicionados.SingleOrDefaultAsync(m => m.idAdicion == id);
                return new Response { IsSuccess = activosFijosAdicionados != null, Message = activosFijosAdicionados != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activosFijosAdicionados };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarActivosFijosAdicionados")]
        public async Task<Response> PostMarca([FromBody]ActivosFijosAdicionados activosFijosAdicionados)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivosFijosAdicionados.AnyAsync(c => c.idActivoFijoOrigen == activosFijosAdicionados.idActivoFijoOrigen && c.idActivoFijoDestino == activosFijosAdicionados.idActivoFijoDestino))
                {
                    db.ActivosFijosAdicionados.Add(activosFijosAdicionados);
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
        public async Task<Response> PutActivosFijosAdicionados([FromRoute] int id, [FromBody]ActivosFijosAdicionados activosFijosAdicionados)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivosFijosAdicionados.Where(c => c.idActivoFijoOrigen == activosFijosAdicionados.idActivoFijoOrigen && c.idActivoFijoDestino == activosFijosAdicionados.idActivoFijoDestino).AnyAsync(c => c.idAdicion != activosFijosAdicionados.idAdicion))
                {
                    var activosFijosAdicionadosActualizar = await db.ActivosFijosAdicionados.Where(x => x.idAdicion == id).FirstOrDefaultAsync();
                    if (activosFijosAdicionadosActualizar != null)
                    {
                        try
                        {
                            activosFijosAdicionadosActualizar.idActivoFijoOrigen = activosFijosAdicionados.idActivoFijoOrigen;
                            activosFijosAdicionadosActualizar.idActivoFijoDestino = activosFijosAdicionados.idActivoFijoDestino;
                            activosFijosAdicionadosActualizar.fechaAdicion = activosFijosAdicionados.fechaAdicion;
                            db.ActivosFijosAdicionados.Update(activosFijosAdicionadosActualizar);
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
        public async Task<Response> DeleteActivosFijosAdicionados([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivosFijosAdicionados.SingleOrDefaultAsync(m => m.idAdicion == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ActivosFijosAdicionados.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ActivosFijosAdicionados _ActivosFijosAdicionados)
        {
            var bdd = _ActivosFijosAdicionados.idActivoFijoOrigen;
            var _bdd = _ActivosFijosAdicionados.idActivoFijoDestino;
            var loglevelrespuesta = db.ActivosFijosAdicionados.Where(p => p.idActivoFijoOrigen == bdd && p.idActivoFijoDestino == _bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
