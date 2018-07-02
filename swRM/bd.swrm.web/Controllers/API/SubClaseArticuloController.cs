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
    [Route("api/SubClaseArticulo")]
    public class SubClaseArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public SubClaseArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarSubClaseArticulos")]
        public async Task<List<SubClaseArticulo>> GetSubClaseArticulo()
        {
            try
            {
                return await db.SubClaseArticulo.OrderBy(x => x.Nombre).Include(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<SubClaseArticulo>();
            }
        }

        [HttpGet]
        [Route("ListarSubClaseArticulosPorClase/{idClaseArticulo}")]
        public async Task<List<SubClaseArticulo>> GetSubClaseArticuloPorClase(int idClaseArticulo)
        {
            try
            {
                return await db.SubClaseArticulo.Where(c=> c.IdClaseArticulo == idClaseArticulo).OrderBy(x => x.Nombre).Include(c => c.ClaseArticulo).ThenInclude(c => c.TipoArticulo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<SubClaseArticulo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetSubClaseArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var subClaseArticulo = await db.SubClaseArticulo.Include(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo).SingleOrDefaultAsync(m => m.IdSubClaseArticulo == id);
                return new Response { IsSuccess = subClaseArticulo != null, Message = subClaseArticulo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = subClaseArticulo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutSubClaseArticulo([FromRoute] int id, [FromBody] SubClaseArticulo subClaseArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.SubClaseArticulo.Where(c => c.Nombre.ToUpper().Trim() == subClaseArticulo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdSubClaseArticulo != subClaseArticulo.IdSubClaseArticulo))
                {
                    var SubClaseArticuloActualizar = await db.SubClaseArticulo.Where(x => x.IdSubClaseArticulo == id).FirstOrDefaultAsync();
                    if (SubClaseArticuloActualizar != null)
                    {
                        try
                        {
                            SubClaseArticuloActualizar.Nombre = subClaseArticulo.Nombre;
                            SubClaseArticuloActualizar.IdClaseArticulo = subClaseArticulo.IdClaseArticulo;
                            db.SubClaseArticulo.Update(SubClaseArticuloActualizar);
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
        [Route("InsertarSubClaseArticulo")]
        public async Task<Response> PostSubClaseArticulo([FromBody] SubClaseArticulo subClaseArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.SubClaseArticulo.AnyAsync(c => c.Nombre.ToUpper().Trim() == subClaseArticulo.Nombre.ToUpper().Trim()))
                {
                    db.Entry(subClaseArticulo.ClaseArticulo).State = EntityState.Unchanged;
                    db.SubClaseArticulo.Add(subClaseArticulo);
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
        public async Task<Response> DeleteSubClaseArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.SubClaseArticulo.SingleOrDefaultAsync(m => m.IdSubClaseArticulo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.SubClaseArticulo.Remove(respuesta);
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
