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
    [Route("api/ActivosFijos")]
    public class ActivosFijosController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivosFijosController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarActivosFijos")]
        public async Task<List<ActivoFijo>> GetActivosFijos()
        {
            try
            {
                return await db.ActivoFijo
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c => c.UnidadMedida)
                    .Include(c => c.CodigoActivoFijo)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _marca = await db.ActivoFijo
                    .Include(c=> c.SubClaseActivoFijo).ThenInclude(c=> c.ClaseActivoFijo).ThenInclude(c=> c.TipoActivoFijo)
                    .Include(c=> c.LibroActivoFijo).ThenInclude(c=> c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.UnidadMedida)
                    .Include(c=> c.CodigoActivoFijo)
                    .Include(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                return new Response { IsSuccess = _marca != null, Message = _marca != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = _marca };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarActivoFijo")]
        public async Task<Response> PostActivoFijo([FromBody]ActivoFijo activoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                db.ActivoFijo.Add(activoFijo);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutActivoFijo([FromRoute] int id, [FromBody]ActivoFijo activosFijos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activosFijosActualizar = await db.ActivoFijo.Where(x => x.IdActivoFijo == id).FirstOrDefaultAsync();
                if (activosFijosActualizar != null)
                {
                    try
                    {
                        activosFijosActualizar.IdSubClaseActivoFijo = activosFijos.IdSubClaseActivoFijo;
                        activosFijosActualizar.IdLibroActivoFijo = activosFijos.IdLibroActivoFijo;
                        activosFijosActualizar.IdCiudad = activosFijos.IdCiudad;
                        activosFijosActualizar.IdUnidadMedida = activosFijos.IdUnidadMedida;
                        activosFijosActualizar.IdCodigoActivoFijo = activosFijos.IdCodigoActivoFijo;
                        activosFijosActualizar.IdModelo = activosFijos.IdModelo;
                        activosFijosActualizar.Nombre = activosFijos.Nombre;
                        activosFijosActualizar.Serie = activosFijos.Serie;
                        activosFijosActualizar.ValorCompra = activosFijos.ValorCompra;
                        activosFijosActualizar.Ubicacion = activosFijos.Ubicacion;
                        db.ActivoFijo.Update(activosFijosActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
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

        [HttpDelete("{id}")]
        public async Task<Response> DeleteActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivoFijo.SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ActivoFijo _ActivosFijos)
        {
            var bdd = _ActivosFijos.IdActivoFijo;
            var loglevelrespuesta = db.ActivoFijo.Where(p => p.IdActivoFijo == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
