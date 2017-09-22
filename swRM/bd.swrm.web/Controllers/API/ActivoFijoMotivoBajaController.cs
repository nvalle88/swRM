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
    [Route("api/ActivoFijoMotivoBaja")]
    public class ActivoFijoMotivoBajaController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivoFijoMotivoBajaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarActivoFijoMotivoBaja
        [HttpGet]
        [Route("ListarActivoFijoMotivoBaja")]
        public async Task<List<ActivoFijoMotivoBaja>> GetActivoFijoMotivoBaja()
        {
            try
            {
                return await db.ActivoFijoMotivoBaja.OrderBy(x => x.Nombre).ToListAsync();
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
                return new List<ActivoFijoMotivoBaja>();
            }
        }


        // GET: api/ActivoFijoMotivoBaja/5
        [HttpGet("{id}")]
        public async Task<Response> GetActivoFijoMotivoBaja([FromRoute] int id)
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

                var ActivoFijoMotivoBaja = await db.ActivoFijoMotivoBaja.SingleOrDefaultAsync(m => m.IdActivoFijoMotivoBaja == id);

                if (ActivoFijoMotivoBaja == null)
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
                    Resultado = ActivoFijoMotivoBaja,
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

        // PUT: api/ActivoFijoMotivoBaja/5
        [HttpPut("{id}")]
        public async Task<Response> PutActivoFijoMotivoBaja([FromRoute] int id, [FromBody] ActivoFijoMotivoBaja activoFijoMotivoBaja)
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

                var ActivoFijoMotivoBajaActualizar = await db.ActivoFijoMotivoBaja.Where(x => x.IdActivoFijoMotivoBaja == id).FirstOrDefaultAsync();
                if (ActivoFijoMotivoBajaActualizar != null)
                {
                    try
                    {
                        ActivoFijoMotivoBajaActualizar.Nombre = activoFijoMotivoBaja.Nombre;
                        db.ActivoFijoMotivoBaja.Update(ActivoFijoMotivoBajaActualizar);
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

        // POST: api/ActivoFijoMotivoBaja
        [HttpPost]
        [Route("InsertarActivoFijoMotivoBaja")]
        public async Task<Response> PostActivoFijoMotivoBaja([FromBody] ActivoFijoMotivoBaja activoFijoMotivoBaja)
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

                var respuesta = Existe(activoFijoMotivoBaja);
                if (!respuesta.IsSuccess)
                {
                    db.ActivoFijoMotivoBaja.Add(activoFijoMotivoBaja);
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

        // DELETE: api/ActivoFijoMotivoBaja/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteActivoFijoMotivoBaja([FromRoute] int id)
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

                var respuesta = await db.ActivoFijoMotivoBaja.SingleOrDefaultAsync(m => m.IdActivoFijoMotivoBaja == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.ActivoFijoMotivoBaja.Remove(respuesta);
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

        private bool ActivoFijoMotivoBajaExists(string nombre)
        {
            return db.ActivoFijoMotivoBaja.Any(e => e.Nombre == nombre);
        }

        public Response Existe(ActivoFijoMotivoBaja activoFijoMotivoBaja)
        {
            var bdd = activoFijoMotivoBaja.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.ActivoFijoMotivoBaja.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
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
