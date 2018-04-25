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
    [Route("api/BajaProveeduria")]
    public class BajaProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public BajaProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarBajasProveeduria")]
        public async Task<List<BajaProveeduria>> GetBajasProveeduria()
        {
            try
            {
                return await db.BajaProveeduria.OrderBy(x => x.FechaBaja).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<BajaProveeduria>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetBajaProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaProveeduria = await db.BajaProveeduria.SingleOrDefaultAsync(m => m.IdBaja == id);
                return new Response { IsSuccess = bajaProveeduria != null, Message = bajaProveeduria != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bajaProveeduria };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }        
        
        [HttpPut("{id}")]
        public async Task<Response> PutBajaProveeduria([FromRoute] int id, [FromBody] BajaProveeduria BajaProveeduria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaProveeduriaActualizar = await db.BajaProveeduria.Where(x => x.IdArticulo == id).FirstOrDefaultAsync();
                if (bajaProveeduriaActualizar != null)
                {
                    try
                    {
                        bajaProveeduriaActualizar.FechaBaja = BajaProveeduria.FechaBaja;
                        bajaProveeduriaActualizar.IdProveedor = BajaProveeduria.IdProveedor;
                        db.BajaProveeduria.Update(bajaProveeduriaActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                        return new Response { IsSuccess = false, Message = Mensaje.Error };
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
        [Route("InsertarBajaProveeduria")]
        public async Task<Response> PostBajaProveeduria([FromBody] BajaProveeduria BajaProveeduria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                db.BajaProveeduria.Add(BajaProveeduria);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteBajaProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.BajaProveeduria.SingleOrDefaultAsync(m => m.IdArticulo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.BajaProveeduria.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(BajaProveeduria BajaProveeduria)
        {
            var bdd = BajaProveeduria.IdArticulo;
            var bdd_fecha = BajaProveeduria.FechaBaja;
            var loglevelrespuesta = db.BajaProveeduria.Where(p => p.IdArticulo == bdd && p.FechaBaja == bdd_fecha).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}