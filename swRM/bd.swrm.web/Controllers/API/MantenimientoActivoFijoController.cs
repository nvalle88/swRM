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
    [Route("api/MantenimientoActivoFijo")]
    public class MantenimientoActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public MantenimientoActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarMantenimientosActivoFijo")]
        public async Task<List<MantenimientoActivoFijo>> GetMantenimientoActivoFijo()
        {
            try
            {
                return await db.MantenimientoActivoFijo.OrderBy(x => x.FechaMantenimiento).Include(x => x.Empleado).ThenInclude(x => x.Persona).Include(x => x.RecepcionActivoFijoDetalle).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MantenimientoActivoFijo>();
            }
        }

        [HttpPost]
        [Route("ListarMantenimientosActivoFijoPorIdDetalleActivoFijo")]
        public async Task<List<MantenimientoActivoFijo>> GetMantenimientosActivoFijoPorIdDetalleActivoFijo([FromBody] int idRecepcionActivoFijoDetalle)
        {
            try
            {
                return await db.MantenimientoActivoFijo.Include(x => x.Empleado).ThenInclude(x => x.Persona).Include(x => x.RecepcionActivoFijoDetalle).Where(c=> c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle).OrderBy(x => x.FechaMantenimiento).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MantenimientoActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetMantenimientoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var mantenimientoActivoFijo = await db.MantenimientoActivoFijo.SingleOrDefaultAsync(m => m.IdMantenimientoActivoFijo == id);
                return new Response { IsSuccess = mantenimientoActivoFijo != null, Message = mantenimientoActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = mantenimientoActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutMantenimientoActivoFijo([FromRoute] int id, [FromBody] MantenimientoActivoFijo mantenimientoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var mantenimientoActivoFijoActualizar = await db.MantenimientoActivoFijo.Where(x => x.IdMantenimientoActivoFijo == id).FirstOrDefaultAsync();
                if (mantenimientoActivoFijoActualizar != null)
                {
                    try
                    {
                        mantenimientoActivoFijoActualizar.FechaMantenimiento = mantenimientoActivoFijo.FechaMantenimiento;
                        mantenimientoActivoFijoActualizar.FechaDesde = mantenimientoActivoFijo.FechaDesde;
                        mantenimientoActivoFijoActualizar.FechaHasta = mantenimientoActivoFijo.FechaHasta;
                        mantenimientoActivoFijoActualizar.Valor = mantenimientoActivoFijo.Valor;
                        mantenimientoActivoFijoActualizar.Observaciones = mantenimientoActivoFijo.Observaciones;
                        mantenimientoActivoFijoActualizar.IdEmpleado = mantenimientoActivoFijo.IdEmpleado;
                        mantenimientoActivoFijoActualizar.IdRecepcionActivoFijoDetalle = mantenimientoActivoFijo.IdRecepcionActivoFijoDetalle;
                        db.MantenimientoActivoFijo.Update(mantenimientoActivoFijoActualizar);
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
        [Route("InsertarMantenimientoActivoFijo")]
        public async Task<Response> PostMantenimientoActivoFijo([FromBody] MantenimientoActivoFijo mantenimientoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                db.MantenimientoActivoFijo.Add(mantenimientoActivoFijo);
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
        public async Task<Response> DeleteMantenimientoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MantenimientoActivoFijo.SingleOrDefaultAsync(m => m.IdMantenimientoActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MantenimientoActivoFijo.Remove(respuesta);
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