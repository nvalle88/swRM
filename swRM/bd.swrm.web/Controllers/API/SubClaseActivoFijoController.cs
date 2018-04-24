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
    [Route("api/SubClaseActivoFijo")]
    public class SubClaseActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public SubClaseActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarSubClasesActivoFijo")]
        public async Task<List<SubClaseActivoFijo>> GetClaseActivoFijo()
        {
            try
            {
                return await db.SubClaseActivoFijo.OrderBy(x => x.Nombre).Include(c => c.ClaseActivoFijo).ThenInclude(c=> c.TipoActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<SubClaseActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetSubClaseActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var subClaseActivoFijo = await db.SubClaseActivoFijo.Include(c=> c.ClaseActivoFijo).ThenInclude(c=> c.TipoActivoFijo).SingleOrDefaultAsync(m => m.IdSubClaseActivoFijo == id);
                return new Response { IsSuccess = subClaseActivoFijo != null, Message = subClaseActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = subClaseActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutSubClaseActivoFijo([FromRoute] int id, [FromBody] SubClaseActivoFijo subClaseActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.SubClaseActivoFijo.Where(c => c.Nombre.ToUpper().Trim() == subClaseActivoFijo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdSubClaseActivoFijo != subClaseActivoFijo.IdSubClaseActivoFijo))
                {
                    var subClaseActivoFijoActualizar = await db.SubClaseActivoFijo.Where(x => x.IdSubClaseActivoFijo == id).FirstOrDefaultAsync();
                    if (subClaseActivoFijoActualizar != null)
                    {
                        try
                        {
                            subClaseActivoFijoActualizar.Nombre = subClaseActivoFijo.Nombre;
                            subClaseActivoFijoActualizar.IdClaseActivoFijo = subClaseActivoFijo.IdClaseActivoFijo;
                            db.SubClaseActivoFijo.Update(subClaseActivoFijoActualizar);
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
        [Route("InsertarSubClaseActivoFijo")]
        public async Task<Response> PostSubClaseActivoFijo([FromBody] SubClaseActivoFijo subClaseActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.SubClaseActivoFijo.AnyAsync(c => c.Nombre.ToUpper().Trim() == subClaseActivoFijo.Nombre.ToUpper().Trim()))
                {
                    db.Entry(subClaseActivoFijo.ClaseActivoFijo).State = EntityState.Unchanged;
                    db.SubClaseActivoFijo.Add(subClaseActivoFijo);
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
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSubClaseActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.SubClaseActivoFijo.SingleOrDefaultAsync(m => m.IdSubClaseActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.SubClaseActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(SubClaseActivoFijo subClaseActivoFijo)
        {
            var bdd = subClaseActivoFijo.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.SubClaseActivoFijo.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}