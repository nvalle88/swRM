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
    [Route("api/CategoriaActivoFijo")]
    public class CategoriaActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public CategoriaActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarCategoriaActivoFijo")]
        public async Task<List<CategoriaActivoFijo>> GetCategoriaActivoFijo()
        {
            try
            {
                return await db.CategoriaActivoFijo.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<CategoriaActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetCategoriaActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var categoriaActivoFijo = await db.CategoriaActivoFijo.SingleOrDefaultAsync(m => m.IdCategoriaActivoFijo == id);
                return new Response { IsSuccess = categoriaActivoFijo != null, Message = categoriaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = categoriaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("ObtenerPrimeraCategoria")]
        public async Task<CategoriaActivoFijo> PostPrimeraCategoriaActivoFijo()
        {
            try
            {
                return await db.CategoriaActivoFijo.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return null;
            }
        }

        [HttpPost]
        [Route("ObtenerCategoriaPorClaseActivoFijo")]
        public async Task<CategoriaActivoFijo> PostCategoriaPorClaseActivoFijo([FromBody] int idClaseActivoFijo)
        {
            try
            {
                var claseActivoFijo = await db.ClaseActivoFijo.Include(c => c.CategoriaActivoFijo).FirstOrDefaultAsync(c => c.IdClaseActivoFijo == idClaseActivoFijo);
                return claseActivoFijo?.CategoriaActivoFijo;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return null;
            }
        }

        [HttpPost]
        [Route("InsertarCategoriaActivoFijo")]
        public async Task<Response> PostCategoriaActivoFijo([FromBody]CategoriaActivoFijo categoriaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CategoriaActivoFijo.AnyAsync(c => c.Nombre.ToUpper().Trim() == categoriaActivoFijo.Nombre.ToUpper().Trim()))
                {
                    db.CategoriaActivoFijo.Add(categoriaActivoFijo);
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
        public async Task<Response> PutCategoriaActivoFijo([FromRoute] int id, [FromBody]CategoriaActivoFijo categoriaActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CategoriaActivoFijo.Where(c => c.Nombre.ToUpper().Trim() == categoriaActivoFijo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdCategoriaActivoFijo != categoriaActivoFijo.IdCategoriaActivoFijo))
                {
                    var categoriaActivoFijoActualizar = await db.CategoriaActivoFijo.Where(x => x.IdCategoriaActivoFijo == id).FirstOrDefaultAsync();
                    if (categoriaActivoFijoActualizar != null)
                    {
                        try
                        {
                            categoriaActivoFijoActualizar.Nombre = categoriaActivoFijo.Nombre;
                            categoriaActivoFijoActualizar.PorCientoDepreciacionAnual = categoriaActivoFijo.PorCientoDepreciacionAnual;
                            categoriaActivoFijoActualizar.AnosVidaUtil = categoriaActivoFijo.AnosVidaUtil;
                            db.CategoriaActivoFijo.Update(categoriaActivoFijoActualizar);
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
        public async Task<Response> DeleteCategoriaActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.CategoriaActivoFijo.SingleOrDefaultAsync(m => m.IdCategoriaActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.CategoriaActivoFijo.Remove(respuesta);
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