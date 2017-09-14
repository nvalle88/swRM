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
    [Route("api/ClaseActivoFijo")]
    public class ClaseActivoFijoController : Controller
    {

        private readonly SwRMDbContext db;

        public ClaseActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarClaseActivoFijo
        [HttpGet]
        [Route("ListarClaseActivoFijo")]
        public async Task<List<ClaseActivoFijo>> GetClaseActivoFijo()
        {
            try
            {
                return await db.ClaseActivoFijo.OrderBy(x => x.Nombre).Include(c=> c.TipoActivoFijo).Include(c=> c.TablaDepreciacion).ToListAsync();
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
                return new List<ClaseActivoFijo>();
            }
        }

        // GET: api/ClaseActivoFijo/5
        [HttpGet("{id}")]
        public async Task<Response> GetClaseActivoFijo([FromRoute] int id)
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

                var claseActivoFijo = await db.ClaseActivoFijo.SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);

                if (claseActivoFijo == null)
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
                    Resultado = claseActivoFijo,
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

        // PUT: api/ClaseActivoFijo/5
        [HttpPut("{id}")]
        public async Task<Response> PutClaseActivoFijo([FromRoute] int id, [FromBody] ClaseActivoFijo claseActivoFijo)
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

                var claseActivoFijoActualizar = await db.ClaseActivoFijo.Where(x => x.IdClaseActivoFijo == id).FirstOrDefaultAsync();
                if (claseActivoFijoActualizar != null)
                {
                    try
                    {
                        claseActivoFijoActualizar.Nombre = claseActivoFijo.Nombre;
                        claseActivoFijoActualizar.IdTipoActivoFijo = claseActivoFijo.IdTipoActivoFijo;
                        claseActivoFijoActualizar.IdTablaDepreciacion = claseActivoFijo.IdTablaDepreciacion;
                        db.ClaseActivoFijo.Update(claseActivoFijoActualizar);
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

        // POST: api/ClaseActivoFijo
        [HttpPost]
        [Route("InsertarClaseActivoFijo")]
        public async Task<Response> PostClaseActivoFijo([FromBody] ClaseActivoFijo claseActivoFijo)
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

                var respuesta = Existe(claseActivoFijo);
                if (!respuesta.IsSuccess)
                {
                    db.ClaseActivoFijo.Add(claseActivoFijo);
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

        // DELETE: api/ClaseActivoFijo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteClaseActivoFijo([FromRoute] int id)
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

                var respuesta = await db.ClaseActivoFijo.SingleOrDefaultAsync(m => m.IdClaseActivoFijo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.ClaseActivoFijo.Remove(respuesta);
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

        private bool ClaseActivoFijoExists(string nombre)
        {
            return db.ClaseActivoFijo.Any(e => e.Nombre == nombre);
        }

        public Response Existe(ClaseActivoFijo claseActivoFijo)
        {
            var bdd = claseActivoFijo.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.ClaseActivoFijo.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd && p.IdTablaDepreciacion == claseActivoFijo.IdTablaDepreciacion && p.IdTipoActivoFijo == claseActivoFijo.IdTipoActivoFijo).FirstOrDefault();
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
