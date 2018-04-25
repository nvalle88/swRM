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
    [Route("api/CodigoActivoFijo")]
    public class CodigoActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public CodigoActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarCodigosActivoFijo")]
        public async Task<List<CodigoActivoFijo>> GetCodigoActivoFijo()
        {
            try
            {
                return await db.CodigoActivoFijo.OrderBy(x => x.Codigosecuencial).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<CodigoActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetCodigoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var codigoActivoFijo = await db.CodigoActivoFijo.SingleOrDefaultAsync(m => m.IdCodigoActivoFijo == id);
                return new Response { IsSuccess = codigoActivoFijo != null, Message = codigoActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = codigoActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutCodigoActivoFijo([FromRoute] int id, [FromBody] CodigoActivoFijo codigoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CodigoActivoFijo.Where(c => c.Codigosecuencial.ToUpper().Trim() == codigoActivoFijo.Codigosecuencial.ToUpper().Trim()).AnyAsync(c => c.IdCodigoActivoFijo != codigoActivoFijo.IdCodigoActivoFijo))
                {
                    var CodigoActivoFijoActualizar = await db.CodigoActivoFijo.Where(x => x.IdCodigoActivoFijo == id).FirstOrDefaultAsync();
                    if (CodigoActivoFijoActualizar != null)
                    {
                        try
                        {
                            CodigoActivoFijoActualizar.Codigosecuencial = codigoActivoFijo.Codigosecuencial;
                            CodigoActivoFijoActualizar.CodigoBarras = codigoActivoFijo.CodigoBarras;
                            db.CodigoActivoFijo.Update(CodigoActivoFijoActualizar);
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
        [Route("InsertarCodigoActivoFijo")]
        public async Task<Response> PostCodigoActivoFijo([FromBody] CodigoActivoFijo codigoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CodigoActivoFijo.AnyAsync(c => c.Codigosecuencial.ToUpper().Trim() == codigoActivoFijo.Codigosecuencial.ToUpper().Trim()))
                {
                    db.CodigoActivoFijo.Add(codigoActivoFijo);
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
        public async Task<Response> DeleteCodigoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.CodigoActivoFijo.SingleOrDefaultAsync(m => m.IdCodigoActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.CodigoActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(CodigoActivoFijo codigoActivoFijo)
        {
            var bdd = codigoActivoFijo.Codigosecuencial.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.CodigoActivoFijo.Where(p => p.Codigosecuencial.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}