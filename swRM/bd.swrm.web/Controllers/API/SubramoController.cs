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
    [Route("api/Subramo")]
    public class SubramoController : Controller
    {
        private readonly SwRMDbContext db;

        public SubramoController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarSubramo")]
        public async Task<List<Subramo>> GetSubramo()
        {
            try
            {
                return await db.Subramo.OrderBy(x => x.Nombre).Include(c => c.Ramo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Subramo>();
            }
        }

        [HttpGet]
        [Route("ListarSubramoPorRamo/{idRamo}")]
        public async Task<List<Subramo>> GetSubramoPorRamo(int idRamo)
        {
            try
            {
                return await db.Subramo.Where(c => c.IdRamo == idRamo).OrderBy(x => x.Nombre).Include(c => c.Ramo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Subramo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetSubramo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var subramo = await db.Subramo.Include(c => c.Ramo).SingleOrDefaultAsync(m => m.IdSubramo == id);
                return new Response { IsSuccess = subramo != null, Message = subramo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = subramo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutSubramo([FromRoute] int id, [FromBody] Subramo subramo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Subramo.Where(c => c.Nombre.ToUpper().Trim() == subramo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdSubramo != subramo.IdSubramo))
                {
                    var subramoActualizar = await db.Subramo.Where(x => x.IdSubramo == id).FirstOrDefaultAsync();
                    if (subramoActualizar != null)
                    {
                        try
                        {
                            subramoActualizar.Nombre = subramo.Nombre;
                            subramoActualizar.IdRamo = subramo.IdRamo;
                            db.Subramo.Update(subramoActualizar);
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
        [Route("InsertarSubramo")]
        public async Task<Response> PostArticulo([FromBody] Subramo subramo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Subramo.AnyAsync(c => c.Nombre.ToUpper().Trim() == subramo.Nombre.ToUpper().Trim()))
                {
                    db.Subramo.Add(subramo);
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
        public async Task<Response> DeleteSubramo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.Subramo.SingleOrDefaultAsync(m => m.IdSubramo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.Subramo.Remove(respuesta);
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