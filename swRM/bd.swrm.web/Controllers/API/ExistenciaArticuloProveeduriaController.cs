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
    [Route("api/ExistenciaArticuloProveeduria")]
    public class ExistenciaArticuloProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public ExistenciaArticuloProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarExistenciasArticulosProveeduria
        [HttpGet]
        [Route("ListarExistenciasArticulosProveeduria")]
        public async Task<List<ExistenciaArticuloProveeduria>> GetExistenciaArticuloProveeduria()
        {
            try
            {
                return await db.ExistenciaArticuloProveeduria.ToListAsync();
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
                return new List<ExistenciaArticuloProveeduria>();
            }
        }

        // GET: api/ExistenciaArticuloProveeduria/5
        [HttpGet("{id}")]
        public async Task<Response> GetExistenciaArticuloProveeduria([FromRoute] int id)
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

                var existenciaArticuloProveeduria = await db.ExistenciaArticuloProveeduria.SingleOrDefaultAsync(m => m.IdArticulo == id);

                if (existenciaArticuloProveeduria == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = existenciaArticuloProveeduria
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

        // PUT: api/ExistenciaArticuloProveeduria/5
        [HttpPut("{id}")]
        public async Task<Response> PutExistenciaArticuloProveeduria([FromRoute] int id, [FromBody] ExistenciaArticuloProveeduria existenciaArticuloProveeduria)
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

                var existenciaArticuloProveeduriaActualizar = await db.ExistenciaArticuloProveeduria.Where(x => x.IdArticulo == id).FirstOrDefaultAsync();
                if (existenciaArticuloProveeduriaActualizar != null)
                {
                    try
                    {
                        existenciaArticuloProveeduriaActualizar.Existencia = existenciaArticuloProveeduria.Existencia;
                        db.ExistenciaArticuloProveeduria.Update(existenciaArticuloProveeduriaActualizar);
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

        // POST: api/ExistenciaArticuloProveeduria
        [HttpPost]
        [Route("InsertarExistenciaArticuloProveeduria")]
        public async Task<Response> PostExistenciaArticuloProveeduria([FromBody] ExistenciaArticuloProveeduria existenciaArticuloProveeduria)
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

                var respuesta = Existe(existenciaArticuloProveeduria);
                if (!respuesta.IsSuccess)
                    db.ExistenciaArticuloProveeduria.Add(existenciaArticuloProveeduria);
                else
                    await PutExistenciaArticuloProveeduria(existenciaArticuloProveeduria.IdArticulo, existenciaArticuloProveeduria);

                await db.SaveChangesAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio
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

        // DELETE: api/ExistenciaArticuloProveeduria/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteExistenciaArticuloProveeduria([FromRoute] int id)
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

                var respuesta = await db.ExistenciaArticuloProveeduria.SingleOrDefaultAsync(m => m.IdArticulo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.ExistenciaArticuloProveeduria.Remove(respuesta);
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

        private bool ArticuloExists(string nombre)
        {
            return db.Articulo.Any(e => e.Nombre == nombre);
        }

        public Response Existe(ExistenciaArticuloProveeduria existenciaArticuloProveeduria)
        {
            var loglevelrespuesta = db.ExistenciaArticuloProveeduria.Where(p => p.IdArticulo == existenciaArticuloProveeduria.IdArticulo).FirstOrDefault();
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
