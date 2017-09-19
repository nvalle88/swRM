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
    [Route("api/EmpleadoActivoFijo")]
    public class EmpleadoActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public EmpleadoActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarEmpleados
        [HttpGet]
        [Route("ListarEmpleadosActivoFijo")]
        public async Task<List<EmpleadoActivoFijo>> GetEmpleadoActivoFijo()
        {
            try
            {
                return await db.EmpleadoActivoFijo.OrderBy(x => x.Empleado.Persona.Nombres).ThenBy(x=> x.Empleado.Persona.Apellidos).ToListAsync();
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
                return new List<EmpleadoActivoFijo>();
            }
        }

        // GET: api/EmpleadoActivoFijo/5
        [HttpGet("{id}")]
        public async Task<Response> GetEmpleadoActivoFijo([FromRoute] int id)
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

                var empleadoActivoFijo = await db.EmpleadoActivoFijo.SingleOrDefaultAsync(m => m.IdEmpleadoActivoFijo == id);

                if (empleadoActivoFijo == null)
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
                    Resultado = empleadoActivoFijo,
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

        // PUT: api/EmpleadoActivoFijo/5
        [HttpPut("{id}")]
        public async Task<Response> PutEmpleadoActivoFijo([FromRoute] int id, [FromBody] EmpleadoActivoFijo empleadoActivoFijo)
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

                var empleadoActivoFijoActualizar = await db.EmpleadoActivoFijo.Where(x => x.IdEmpleadoActivoFijo == id).FirstOrDefaultAsync();
                if (empleadoActivoFijoActualizar != null)
                {
                    try
                    {
                        empleadoActivoFijoActualizar.FechaAsignacion = empleadoActivoFijo.FechaAsignacion;
                        empleadoActivoFijoActualizar.IdActivoFijo = empleadoActivoFijo.IdActivoFijo;
                        empleadoActivoFijoActualizar.IdEmpleado = empleadoActivoFijo.IdEmpleado;

                        db.EmpleadoActivoFijo.Update(empleadoActivoFijoActualizar);
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

        // POST: api/EmpleadoActivoFijo
        [HttpPost]
        [Route("InsertarEmpleadoActivoFijo")]
        public async Task<Response> PostEmpleadoActivoFijo([FromBody] EmpleadoActivoFijo empleadoActivoFijo)
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

                var respuesta = Existe(empleadoActivoFijo);
                if (!respuesta.IsSuccess)
                {
                    db.EmpleadoActivoFijo.Add(empleadoActivoFijo);
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

        // DELETE: api/EmpleadoActivoFijo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteEmpleadoActivoFijo([FromRoute] int id)
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

                var respuesta = await db.EmpleadoActivoFijo.SingleOrDefaultAsync(m => m.IdEmpleadoActivoFijo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.EmpleadoActivoFijo.Remove(respuesta);
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

        public Response Existe(EmpleadoActivoFijo empleadoActivoFijo)
        {
            var empleadoActivoFijoRespuesta = db.EmpleadoActivoFijo.Where(p => p.IdEmpleado == empleadoActivoFijo.IdEmpleado && p.IdActivoFijo == empleadoActivoFijo.IdActivoFijo).FirstOrDefault();
            if (empleadoActivoFijoRespuesta != null)
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
                Resultado = empleadoActivoFijoRespuesta,
            };
        }
    }
}