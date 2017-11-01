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
    [Route("api/SolicitudProveeduria")]
    public class SolicitudProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public SolicitudProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarSolicitudProveedurias
        [HttpGet]
        [Route("ListarSolicitudProveedurias")]
        public async Task<List<SolicitudProveduria>> GetSolicitudProveeduria()
        {
            try
            {
                return await db.SolicitudProveduria.OrderBy(x => x.IdSolicitudProveduria).Include(c=> c.IdEmpleado).ToListAsync();
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
                return new List<SolicitudProveduria>();
            }
        }

        // GET: api/SolicitudProveeduria/5
        [HttpGet("{id}")]
        public async Task<Response> GetSolicitudProveeduria([FromRoute] int id)
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

                var SolicitudProveeduria = await db.SolicitudProveduria.SingleOrDefaultAsync(m => m.IdSolicitudProveduria == id);

                if (SolicitudProveeduria == null)
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
                    Resultado = SolicitudProveeduria,
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

        // PUT: api/SolicitudProveeduria/5
        [HttpPut("{id}")]
        public async Task<Response> PutSolicitudProveeduria([FromRoute] int id, [FromBody] SolicitudProveduria SolicitudProveeduria)
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

                var SolicitudProveeduriaActualizar = await db.SolicitudProveduria.Where(x => x.IdSolicitudProveduria == id).FirstOrDefaultAsync();
                if (SolicitudProveeduriaActualizar != null)
                {
                    try
                    {
                        SolicitudProveeduriaActualizar.IdSolicitudProveduria = SolicitudProveeduria.IdSolicitudProveduria;
                        SolicitudProveeduriaActualizar.IdEmpleado = SolicitudProveeduria.IdEmpleado;
                        SolicitudProveeduriaActualizar.SolicitudProveduriaDetalle = SolicitudProveeduria.SolicitudProveduriaDetalle;
                        SolicitudProveeduriaActualizar.Empleado = SolicitudProveeduria.Empleado;
                        db.SolicitudProveduria.Update(SolicitudProveeduriaActualizar);
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

        // POST: api/SolicitudProveeduria
        [HttpPost]
        [Route("InsertarSolicitudProveeduria")]
        public async Task<Response> PostSolicitudProveeduria([FromBody] SolicitudProveduria SolicitudProveeduria)
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

                var respuesta = Existe(SolicitudProveeduria);
                if (!respuesta.IsSuccess)
                {
                    db.SolicitudProveduria.Add(SolicitudProveeduria);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = SolicitudProveeduria
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

        // DELETE: api/SolicitudProveeduria/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSolicitudProveeduria([FromRoute] int id)
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

                var respuesta = await db.SolicitudProveduria.SingleOrDefaultAsync(m => m.IdSolicitudProveduria == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.SolicitudProveduria.Remove(respuesta);
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

        private bool SolicitudProveeduriaExists(int id)
        {
            return db.SolicitudProveduria.Any(e => e.IdSolicitudProveduria == id);
        }

        public Response Existe(SolicitudProveduria SolicitudProveeduria)
        {
            var bdd = SolicitudProveeduria.IdSolicitudProveduria;
            var loglevelrespuesta = db.SolicitudProveduria.Where(p => p.IdSolicitudProveduria == bdd).FirstOrDefault();
            if (loglevelrespuesta != null)
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
                Resultado = loglevelrespuesta,
            };
        }
    }
}
