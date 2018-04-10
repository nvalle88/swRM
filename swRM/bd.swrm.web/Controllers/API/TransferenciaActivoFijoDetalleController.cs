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

        // GET: api/TransferenciaActivoFijoDetalle
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
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<TransferenciaActivoFijoDetalle>();
            }
        }

        // GET: api/TransferenciaActivoFijoDetalle/5
        [HttpGet("{id}")]
        public async Task<Response> GetTransferenciaActivoFijoDetalle([FromRoute] int id)
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

                var _TransferenciaActivoFijoDetalle = await db.TransferenciaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijoDetalle == id);

                if (_TransferenciaActivoFijoDetalle == null)
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
                    Resultado = _TransferenciaActivoFijoDetalle
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

        // POST: api/TransferenciaActivoFijoDetalle
        [HttpPost]
        [Route("InsertarTransferenciaActivoFijoDetalle")]
        public async Task<Response> PostTransferenciaActivoFijoDetalle([FromBody]TransferenciaActivoFijoDetalle _TransferenciaActivoFijoDetalle)
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

                var respuesta = Existe(_TransferenciaActivoFijoDetalle);
                if (!respuesta.IsSuccess)
                {
                    db.TransferenciaActivoFijoDetalle.Add(_TransferenciaActivoFijoDetalle);
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
                    Message = Mensaje.Satisfactorio
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

        // PUT: api/TransferenciaActivoFijoDetalle/5
        [HttpPut("{id}")]
        public async Task<Response> PutTransferenciaActivoFijoDetalle([FromRoute] int id, [FromBody]TransferenciaActivoFijoDetalle _TransferenciaActivoFijoDetalle)
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

                var _TransferenciaActivoFijoDetalleActualizar = await db.TransferenciaActivoFijoDetalle.Where(x => x.IdTransferenciaActivoFijoDetalle == id).FirstOrDefaultAsync();
                if (_TransferenciaActivoFijoDetalleActualizar != null)
                {
                    try
                    {
                        _TransferenciaActivoFijoDetalleActualizar.IdActivoFijo = _TransferenciaActivoFijoDetalle.IdActivoFijo;
                        _TransferenciaActivoFijoDetalleActualizar.IdTransferenciaActivoFijo = _TransferenciaActivoFijoDetalle.IdTransferenciaActivoFijo;
                        
                        db.TransferenciaActivoFijoDetalle.Update(_TransferenciaActivoFijoDetalleActualizar);
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteTransferenciaActivoFijoDetalle([FromRoute] int id)
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

                var respuesta = await db.TransferenciaActivoFijoDetalle.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijoDetalle == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.TransferenciaActivoFijoDetalle.Remove(respuesta);
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

        private bool TransferenciaActivoFijoDetalleExists(int id)
        {
            return db.TransferenciaActivoFijoDetalle.Any(e => e.IdTransferenciaActivoFijoDetalle == id);
        }

        public Response Existe(TransferenciaActivoFijoDetalle _TransferenciaActivoFijoDetalle)
        {
            //var bdd = _TransferenciaActivoFijo.Motivo_Transferencia.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.TransferenciaActivoFijoDetalle.Where(p => p.IdActivoFijo == _TransferenciaActivoFijoDetalle.IdActivoFijo && p.IdTransferenciaActivoFijo == _TransferenciaActivoFijoDetalle.IdTransferenciaActivoFijo).FirstOrDefault();

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