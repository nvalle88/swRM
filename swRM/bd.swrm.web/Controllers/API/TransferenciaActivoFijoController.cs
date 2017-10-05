using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.swrm.entidades.Utils;
using bd.log.guardar.Enumeradores;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/TransferenciaActivoFijo")]
    public class TransferenciaActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public TransferenciaActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/TransferenciaActivoFijo
        [HttpGet]
        [Route("ListarTransferenciaActivoFijo")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciaActivoFijo()
        {
            try
            {
                return await db.TransferenciaActivoFijo.OrderBy(x => x.FechaTransferencia).ToListAsync();
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
                return new List<TransferenciaActivoFijo>();
            }
        }

        // GET: api/TransferenciaActivoFijo/5
        [HttpGet("{id}")]
        public async Task<Response> GetTransferenciaActivoFijo([FromRoute] int id)
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

                var _TransferenciaActivoFijo = await db.TransferenciaActivoFijo.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijo == id);

                if (_TransferenciaActivoFijo == null)
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
                    Resultado = _TransferenciaActivoFijo
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

        // POST: api/MotivoTransferencia
        [HttpPost]
        [Route("InsertarTransferenciaActivoFijo")]
        public async Task<Response> PostTransferenciaActivoFijo([FromBody]TransferenciaActivoFijo _TransferenciaActivoFijo)
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

                var respuesta = Existe(_TransferenciaActivoFijo);
                if (!respuesta.IsSuccess)
                {
                    db.TransferenciaActivoFijo.Add(_TransferenciaActivoFijo);
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

        // PUT: api/TransferenciaActivoFijo/5
        [HttpPut("{id}")]
        public async Task<Response> PutTransferenciaActivoFijo([FromRoute] int id, [FromBody]TransferenciaActivoFijo _TransferenciaActivoFijo)
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

                var _TransferenciaActivoFijoActualizar = await db.TransferenciaActivoFijo.Where(x => x.IdTransferenciaActivoFijo == id).FirstOrDefaultAsync();
                if (_TransferenciaActivoFijoActualizar != null)
                {
                    try
                    {
                        _TransferenciaActivoFijoActualizar.IdEmpleado = _TransferenciaActivoFijo.IdEmpleado;
                        _TransferenciaActivoFijoActualizar.IdMotivoTransferencia = _TransferenciaActivoFijo.IdMotivoTransferencia;
                        _TransferenciaActivoFijoActualizar.FechaTransferencia = _TransferenciaActivoFijo.FechaTransferencia;
                        _TransferenciaActivoFijoActualizar.Origen = _TransferenciaActivoFijo.Origen;
                        _TransferenciaActivoFijoActualizar.Destino = _TransferenciaActivoFijo.Destino;
                        _TransferenciaActivoFijoActualizar.Observaciones = _TransferenciaActivoFijo.Observaciones;
                        
                        db.TransferenciaActivoFijo.Update(_TransferenciaActivoFijoActualizar);
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteTransferenciaActivoFijo([FromRoute] int id)
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

                var respuesta = await db.TransferenciaActivoFijo.SingleOrDefaultAsync(m => m.IdTransferenciaActivoFijo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.TransferenciaActivoFijo.Remove(respuesta);
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

        private bool TransferenciaActivoFijoExists(int id)
        {
            return db.TransferenciaActivoFijo.Any(e => e.IdTransferenciaActivoFijo == id);
        }

        public Response Existe(TransferenciaActivoFijo _TransferenciaActivoFijo)
        {
            //var bdd = _TransferenciaActivoFijo.Motivo_Transferencia.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.TransferenciaActivoFijo.Where(p => p.IdEmpleado == _TransferenciaActivoFijo.IdEmpleado && p.IdMotivoTransferencia == _TransferenciaActivoFijo.IdMotivoTransferencia
                                        && p.FechaTransferencia == _TransferenciaActivoFijo.FechaTransferencia && p.Destino == _TransferenciaActivoFijo.Destino && p.Origen == _TransferenciaActivoFijo.Origen
                                        && p.Observaciones == _TransferenciaActivoFijo.Observaciones).FirstOrDefault();

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