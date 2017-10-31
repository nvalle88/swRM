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
using bd.swrm.entidades.Utils;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/FacturaPorAltaProveeduria")]
    public class FacturaPorAltaProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public FacturaPorAltaProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/Factura
        [HttpGet]
        [Route("ListarFacturasPorAltaProveeduria")]
        public async Task<List<FacturasPorAltaProveeduria>> GetFacturasPorAltaProveeduria()
        {
            try
            {
                return await db.FacturasPorAltaProveeduria.OrderBy(x => x.NumeroFactura).ToListAsync();
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
                return new List<FacturasPorAltaProveeduria>();
            }
        }

        // GET: api/FacturasPorAltaProveeduria/5
        [HttpGet("{id}")]
        public async Task<Response> GetFacturasPorAltaProveeduria([FromRoute]int id)
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

                var _factura = await db.FacturasPorAltaProveeduria.SingleOrDefaultAsync(m => m.IdFacturasPorAlta == id);

                if (_factura == null)
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
                    Resultado = _factura,
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

        // POST: api/FacturasPorAltaProveeduria
        [HttpPost]
        [Route("InsertarFacturasPorAltaProveeduria")]
        public async Task<Response> PostFacturasPorAltaProveeduria([FromBody]FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
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

                var respuesta = Existe(_FacturasPorAltaProveeduria);
                if (!respuesta.IsSuccess)
                {
                    db.FacturasPorAltaProveeduria.Add(_FacturasPorAltaProveeduria);
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

        // PUT: api/FacturasPorAltaProveeduria/5
        [HttpPut("{id}")]
        public async Task<Response> PutFacturasPorAltaProveeduria([FromRoute] int id, [FromBody]FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
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

                var _FacturasPorAltaProveeduriaActualizar = await db.FacturasPorAltaProveeduria.Where(x => x.IdFacturasPorAlta == id).FirstOrDefaultAsync();
                if (_FacturasPorAltaProveeduriaActualizar != null)
                {
                    try
                    {
                        _FacturasPorAltaProveeduriaActualizar.NumeroFactura = _FacturasPorAltaProveeduria.NumeroFactura;
                        _FacturasPorAltaProveeduriaActualizar.IdAlta = _FacturasPorAltaProveeduria.IdAlta;
                        
                        db.FacturasPorAltaProveeduria.Update(_FacturasPorAltaProveeduriaActualizar);
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
        public async Task<Response> DeleteFacturasPorAltaProveeduria([FromRoute] int id)
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

                var respuesta = await db.FacturasPorAltaProveeduria.SingleOrDefaultAsync(m => m.IdFacturasPorAlta == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.FacturasPorAltaProveeduria.Remove(respuesta);
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

        private bool FacturasPorAltaProveeduriaExists(int id)
        {
            return db.FacturasPorAltaProveeduria.Any(e => e.IdFacturasPorAlta == id);
        }

        public Response Existe(FacturasPorAltaProveeduria _FacturasPorAltaProveeduria)
        {
            var bdd = _FacturasPorAltaProveeduria.NumeroFactura;
            var loglevelrespuesta = db.FacturasPorAltaProveeduria.Where(p => p.NumeroFactura == bdd).FirstOrDefault();

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