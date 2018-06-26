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
    [Route("api/Bodega")]
    public class BodegaController : Controller
    {
        private readonly SwRMDbContext db;

        public BodegaController(SwRMDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("ListarBodega")]
        public async Task<List<Bodega>> GetBodega()
        {
            try
            {
                return await db.Bodega.Include(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais).Include(c => c.EmpleadoResponsable).ThenInclude(c => c.Persona).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Bodega>();
            }
        }

        [HttpGet]
        [Route("ListarBodegaPorSucursal/{idSucursal}")]
        public async Task<List<Bodega>> GetBodegaPorSucursal(int idSucursal)
        {
            try
            {
                return await db.Bodega.Where(c => c.IdSucursal == idSucursal).Include(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais).Include(c=> c.EmpleadoResponsable).ThenInclude(c=> c.Persona).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Bodega>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetBodega([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bodega = await db.Bodega.Include(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais).Include(c => c.EmpleadoResponsable).ThenInclude(c => c.Persona).SingleOrDefaultAsync(m => m.IdBodega == id);
                return new Response { IsSuccess = bodega != null, Message = bodega != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bodega };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutBodega([FromRoute] int id, [FromBody] Bodega bodega)
        {
            try
            {
                ModelState.Remove("Sucursal.Nombre");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Bodega.Where(c => c.Nombre.ToUpper().Trim() == bodega.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdBodega != bodega.IdBodega))
                {
                    var bodegaActualizar = await db.Bodega.Where(x => x.IdBodega == id).FirstOrDefaultAsync();
                    if (bodegaActualizar != null)
                    {
                        try
                        {
                            bodegaActualizar.Nombre = bodega.Nombre;
                            bodegaActualizar.IdSucursal = bodega.IdSucursal;
                            bodegaActualizar.IdEmpleadoResponsable = bodega.IdEmpleadoResponsable;
                            db.Bodega.Update(bodegaActualizar);
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
        [Route("InsertarBodega")]
        public async Task<Response> PostBodega([FromBody] Bodega bodega)
        {
            try
            {
                ModelState.Remove("Sucursal.Nombre");
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Bodega.AnyAsync(c => c.Nombre.ToUpper().Trim() == bodega.Nombre.ToUpper().Trim()))
                {
                    db.Bodega.Add(bodega);
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
        public async Task<Response> DeleteBodega([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.Bodega.SingleOrDefaultAsync(m => m.IdBodega == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.Bodega.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(Bodega bodega)
        {
            var bdd = bodega.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.Bodega.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}