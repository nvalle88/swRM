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
    [Route("api/FondoFinanciamiento")]
    public class FondoFinanciamientoController : Controller
    {
        private readonly SwRMDbContext db;

        public FondoFinanciamientoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarFondoFinanciamiento")]
        public async Task<List<FondoFinanciamiento>> GetFondoFinanciamiento()
        {
            try
            {
                return await db.FondoFinanciamiento.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<FondoFinanciamiento>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetFondoFinanciamiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var fondoFinanciamiento = await db.FondoFinanciamiento.SingleOrDefaultAsync(m => m.IdFondoFinanciamiento == id);
                return new Response { IsSuccess = fondoFinanciamiento != null, Message = fondoFinanciamiento != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = fondoFinanciamiento };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutFondoFinanciamiento([FromRoute] int id, [FromBody] FondoFinanciamiento fondoFinanciamiento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.FondoFinanciamiento.Where(c => c.Nombre.ToUpper().Trim() == fondoFinanciamiento.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdFondoFinanciamiento != fondoFinanciamiento.IdFondoFinanciamiento))
                {
                    var fondoFinanciamientoActualizar = await db.FondoFinanciamiento.Where(x => x.IdFondoFinanciamiento == id).FirstOrDefaultAsync();
                    if (fondoFinanciamientoActualizar != null)
                    {
                        try
                        {
                            fondoFinanciamientoActualizar.Nombre = fondoFinanciamiento.Nombre;
                            db.FondoFinanciamiento.Update(fondoFinanciamientoActualizar);
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
        [Route("InsertarFondoFinanciamiento")]
        public async Task<Response> PostFondoFinanciamiento([FromBody] FondoFinanciamiento fondoFinanciamiento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.FondoFinanciamiento.AnyAsync(c => c.Nombre.ToUpper().Trim() == fondoFinanciamiento.Nombre.ToUpper().Trim()))
                {
                    db.FondoFinanciamiento.Add(fondoFinanciamiento);
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
        public async Task<Response> DeleteFondoFinanciamiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.FondoFinanciamiento.SingleOrDefaultAsync(m => m.IdFondoFinanciamiento == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.FondoFinanciamiento.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(FondoFinanciamiento fondoFinanciamiento)
        {
            var bdd = fondoFinanciamiento.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.FondoFinanciamiento.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}