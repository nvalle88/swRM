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
using bd.swrm.entidades.Utils;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/TransferenciaActivoFijoDetalle")]
    public class TransferenciaActivoFijoDetalleController : Controller
    {
        private readonly SwRMDbContext db;

        public TransferenciaActivoFijoDetalleController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarTransferenciaActivoFijoDetalle")]
        public async Task<List<TransferenciaActivoFijoDetalle>> GetTransferenciaActivoFijoDetalle()
        {
            try
            {
                return await db.TransferenciaActivoFijoDetalle.OrderBy(x => x.IdTransferenciaActivoFijoDetalle).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<TransferenciaActivoFijoDetalle>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetTransferenciaActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _TransferenciaActivoFijoDetalle = await db.TransferenciaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijoDetalle == id);
                return new Response { IsSuccess = _TransferenciaActivoFijoDetalle != null, Message = _TransferenciaActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = _TransferenciaActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarTransferenciaActivoFijoDetalle")]
        public async Task<Response> PostTransferenciaActivoFijoDetalle([FromBody]TransferenciaActivoFijoDetalle transferenciaActivoFijoDetalle)
        {
            try
            {
                ModelState.Remove("IdTransferenciaActivoFijo");
                ModelState.Remove("ActivoFijo.IdCiudad");
                ModelState.Remove("ActivoFijo.IdModelo");
                ModelState.Remove("ActivoFijo.Ubicacion");
                ModelState.Remove("ActivoFijo.IdUnidadMedida");
                ModelState.Remove("ActivoFijo.IdCodigoActivoFijo");
                ModelState.Remove("ActivoFijo.IdSubClaseActivoFijo");
                ModelState.Remove("ActivoFijo.LibroActivoFijo.Sucursal.Nombre");
                ModelState.Remove("ActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Nombre");
                ModelState.Remove("ActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Nombre");
                ModelState.Remove("ActivoFijo.Serie");
                ModelState.Remove("ActivoFijo.Nombre");

                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.TransferenciaActivoFijoDetalle.AnyAsync(p => p.IdActivoFijo == transferenciaActivoFijoDetalle.IdActivoFijo && p.IdTransferenciaActivoFijo == transferenciaActivoFijoDetalle.IdTransferenciaActivoFijo))
                {
                    db.Entry(transferenciaActivoFijoDetalle.ActivoFijo).State = EntityState.Unchanged;
                    db.TransferenciaActivoFijoDetalle.Add(transferenciaActivoFijoDetalle);
                    await db.SaveChangesAsync();

                    var activosFijosActualizar = await db.ActivoFijo.Where(x => x.IdActivoFijo == transferenciaActivoFijoDetalle.IdActivoFijo).FirstOrDefaultAsync();
                    if (activosFijosActualizar != null)
                    {
                        activosFijosActualizar.IdLibroActivoFijo = transferenciaActivoFijoDetalle.ActivoFijo.IdLibroActivoFijo;
                        activosFijosActualizar.IdCiudad = transferenciaActivoFijoDetalle.ActivoFijo.LibroActivoFijo.Sucursal.IdCiudad;
                        activosFijosActualizar.Ubicacion = transferenciaActivoFijoDetalle.TransferenciaActivoFijo.Destino;
                        await db.SaveChangesAsync();
                    }
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
        public async Task<Response> PutTransferenciaActivoFijoDetalle([FromRoute] int id, [FromBody]TransferenciaActivoFijoDetalle _TransferenciaActivoFijoDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var _TransferenciaActivoFijoDetalleActualizar = await db.TransferenciaActivoFijoDetalle.Where(x => x.IdTransferenciaActivoFijoDetalle == id).FirstOrDefaultAsync();
                if (_TransferenciaActivoFijoDetalleActualizar != null)
                {
                    try
                    {
                        _TransferenciaActivoFijoDetalleActualizar.IdActivoFijo = _TransferenciaActivoFijoDetalle.IdActivoFijo;
                        _TransferenciaActivoFijoDetalleActualizar.IdTransferenciaActivoFijo = _TransferenciaActivoFijoDetalle.IdTransferenciaActivoFijo;
                        db.TransferenciaActivoFijoDetalle.Update(_TransferenciaActivoFijoDetalleActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
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
        public async Task<Response> DeleteTransferenciaActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.TransferenciaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijoDetalle == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.TransferenciaActivoFijoDetalle.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(TransferenciaActivoFijoDetalle _TransferenciaActivoFijoDetalle)
        {
            var loglevelrespuesta = db.TransferenciaActivoFijoDetalle.Where(p => p.IdActivoFijo == _TransferenciaActivoFijoDetalle.IdActivoFijo && p.IdTransferenciaActivoFijo == _TransferenciaActivoFijoDetalle.IdTransferenciaActivoFijo).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}