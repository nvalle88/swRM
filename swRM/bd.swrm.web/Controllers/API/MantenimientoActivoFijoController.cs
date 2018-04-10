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

        // GET: api/ListarMantenimientosActivoFijo
        [HttpGet]
        [Route("ListarMantenimientosActivoFijo")]
        public async Task<List<MantenimientoActivoFijo>> GetMantenimientoActivoFijo()
        {
            try
            {
                return await db.MantenimientoActivoFijo.OrderBy(x => x.FechaMantenimiento).Include(x => x.Empleado).ThenInclude(x => x.Persona).Include(x => x.ActivoFijo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<MantenimientoActivoFijo>();
            }
        }

        // GET: api/MantenimientoActivoFijo/5
        [HttpGet("{id}")]
        public async Task<Response> GetMantenimientoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var MantenimientoActivoFijo = await db.MantenimientoActivoFijo.SingleOrDefaultAsync(m => m.IdMantenimientoActivoFijo == id);

                if (MantenimientoActivoFijo == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = MantenimientoActivoFijo,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // PUT: api/MantenimientoActivoFijo/5
        [HttpPut("{id}")]
        public async Task<Response> PutMantenimientoActivoFijo([FromRoute] int id, [FromBody] MantenimientoActivoFijo mantenimientoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var MantenimientoActivoFijoActualizar = await db.MantenimientoActivoFijo.Where(x => x.IdMantenimientoActivoFijo == id).FirstOrDefaultAsync();
                if (MantenimientoActivoFijoActualizar != null)
                {
                    try
                    {
                        MantenimientoActivoFijoActualizar.FechaMantenimiento = mantenimientoActivoFijo.FechaMantenimiento;
                        MantenimientoActivoFijoActualizar.FechaDesde = mantenimientoActivoFijo.FechaDesde;
                        MantenimientoActivoFijoActualizar.FechaHasta = mantenimientoActivoFijo.FechaHasta;
                        MantenimientoActivoFijoActualizar.Valor = mantenimientoActivoFijo.Valor;
                        MantenimientoActivoFijoActualizar.Observaciones = mantenimientoActivoFijo.Observaciones;
                        MantenimientoActivoFijoActualizar.IdEmpleado = mantenimientoActivoFijo.IdEmpleado;
                        MantenimientoActivoFijoActualizar.IdActivoFijo = mantenimientoActivoFijo.IdActivoFijo;
                        db.MantenimientoActivoFijo.Update(MantenimientoActivoFijoActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = Mensaje.ModeloInvalido,
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwRm),
                            ExceptionTrace = ex.Message,
                            Message = Mensaje.Excepcion,
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = Mensaje.Error,
                        };
                    }
                }




                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion
                };
            }
        }

        // POST: api/MantenimientoActivoFijo
        [HttpPost]
        [Route("InsertarMantenimientoActivoFijo")]
        public async Task<Response> PostMantenimientoActivoFijo([FromBody] MantenimientoActivoFijo mantenimientoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var respuesta = Existe(mantenimientoActivoFijo);
                if (!respuesta.IsSuccess)
                {
                    db.MantenimientoActivoFijo.Add(mantenimientoActivoFijo);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // DELETE: api/MantenimientoActivoFijo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteMantenimientoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var respuesta = await db.MantenimientoActivoFijo.SingleOrDefaultAsync(m => m.IdMantenimientoActivoFijo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.MantenimientoActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        public Response Existe(MantenimientoActivoFijo mantenimientoActivoFijo)
        {
            var bdd = mantenimientoActivoFijo.FechaMantenimiento;
            var MantenimientoActivoFijoRespuesta = db.MantenimientoActivoFijo.Where(p => p.FechaMantenimiento == bdd && p.IdEmpleado == mantenimientoActivoFijo.IdEmpleado && p.IdActivoFijo == mantenimientoActivoFijo.IdActivoFijo).FirstOrDefault();
            if (MantenimientoActivoFijoRespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.ExisteRegistro,
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = MantenimientoActivoFijoRespuesta,
            };
        }
    }
}