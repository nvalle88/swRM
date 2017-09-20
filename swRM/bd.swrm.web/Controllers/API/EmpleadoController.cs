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
    [Route("api/Empleado")]
    public class EmpleadoController : Controller
    {
        private readonly SwRMDbContext db;

        public EmpleadoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarEmpleados
        [HttpGet]
        [Route("ListarEmpleados")]
        public async Task<List<Empleado>> GetEmpleado()
        {
            try
            {
                return await db.Empleado.OrderBy(x => x.Persona.Nombres).ThenBy(x=> x.Persona.Apellidos).Include(c=> c.Persona).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<Empleado>();
            }
        }

        // GET: api/Empleado/5
        [HttpGet("{id}")]
        public async Task<Response> GetEmpleado([FromRoute] int id)
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

                var empleado = await db.Empleado.SingleOrDefaultAsync(m => m.IdEmpleado == id);

                if (empleado == null)
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
                    Resultado = empleado,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
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

        // PUT: api/Empleado/5
        [HttpPut("{id}")]
        public async Task<Response> PutEmpleado([FromRoute] int id, [FromBody] Empleado empleado)
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

                var empleadoActualizar = await db.Empleado.Where(x => x.IdEmpleado == id).FirstOrDefaultAsync();
                if (empleadoActualizar != null)
                {
                    try
                    {
                        empleadoActualizar.FechaIngreso = empleado.FechaIngreso;
                        empleadoActualizar.FechaIngresoSectorPublico = empleado.FechaIngresoSectorPublico;
                        empleadoActualizar.TrabajoSuperintendenciaBanco = empleado.TrabajoSuperintendenciaBanco;
                        empleadoActualizar.Nepotismo = empleado.Nepotismo;
                        empleadoActualizar.DeclaracionJurada = empleado.DeclaracionJurada;
                        empleadoActualizar.IngresosOtraActividad = empleado.IngresosOtraActividad;
                        empleadoActualizar.MesesImposiciones = empleado.MesesImposiciones;
                        empleadoActualizar.DiasImposiciones = empleado.DiasImposiciones;
                        empleadoActualizar.IdPersona = empleado.IdPersona;
                        empleadoActualizar.IdCiudadLugarNacimiento = empleado.IdCiudadLugarNacimiento;
                        empleadoActualizar.IdProvinciaLugarSufragio = empleado.IdProvinciaLugarSufragio;

                        db.Empleado.Update(empleadoActualizar);
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
                            ExceptionTrace = ex,
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

        // POST: api/Empleado
        [HttpPost]
        [Route("InsertarEmpleado")]
        public async Task<Response> PostEmpleado([FromBody] Empleado empleado)
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

                var respuesta = Existe(empleado);
                if (!respuesta.IsSuccess)
                {
                    db.Empleado.Add(empleado);
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
                    ExceptionTrace = ex,
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

        // DELETE: api/Empleado/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteEmpleado([FromRoute] int id)
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

                var respuesta = await db.Empleado.SingleOrDefaultAsync(m => m.IdEmpleado == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.Empleado.Remove(respuesta);
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
                    ExceptionTrace = ex,
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

        public Response Existe(Empleado empleado)
        {
            var identificacion = empleado.Persona.Identificacion.ToUpper().TrimEnd().TrimStart();
            var EmpleadoRespuesta = db.Empleado.Where(p => p.Persona.Nombres.ToUpper().TrimStart().TrimEnd() == identificacion).FirstOrDefault();
            if (EmpleadoRespuesta != null)
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
                Resultado = EmpleadoRespuesta,
            };
        }
    }
}