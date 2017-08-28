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
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/MaestroDetalleArticulo")]
    public class MaestroDetalleArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public MaestroDetalleArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/MaestroDetalleArticulo
        [HttpGet]
        [Route("ListarMaestroDetalleArticulo")]
        public async Task<List<MaestroDetalleArticulo>> GetMaestroDetalleArticulo()
        {
            try
            {
                return await db.MaestroDetalleArticulo.OrderBy(x => x.IdArticulo).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<MaestroDetalleArticulo>();
            }
        }

        // GET: api/MaestroDetalleArticulo/5
        [HttpGet("{id}")]
        public async Task<Response> GetMaestroDetalleArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo no válido",
                    };
                }

                var _entidad = await db.MaestroDetalleArticulo.SingleOrDefaultAsync(m => m.IdMaestroDetalleArticulo == id);

                if (_entidad == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No encontrado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Resultado = _entidad,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }
        
        // POST: api/MaestroDetalleArticulo
        [HttpPost]
        [Route("InsertarMaestroDetalleArticulo")]
        public async Task<Response> PostMaestroDetalleArticulo([FromBody]MaestroDetalleArticulo _maestroDetalleArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo inválido"
                    };
                }

                var respuesta = Existe(_maestroDetalleArticulo);
                if (!respuesta.IsSuccess)
                {
                    db.MaestroDetalleArticulo.Add(_maestroDetalleArticulo);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "OK"
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "OK"
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }
        
        // PUT: api/MaestroDetalleArticulo/5
        [HttpPut("{id}")]
        public async Task<Response> PutMaestroDetalleArticulo([FromRoute] int id, [FromBody]MaestroDetalleArticulo _maestroDetalleArticulo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo inválido"
                    };
                }

                var _maestroDetalleArticuloActualizar = await db.MaestroDetalleArticulo.Where(x => x.IdMaestroDetalleArticulo == id).FirstOrDefaultAsync();
                if (_maestroDetalleArticuloActualizar != null)
                {
                    try
                    {
                        _maestroDetalleArticuloActualizar.Cantidad = _maestroDetalleArticulo.Cantidad;
                        _maestroDetalleArticuloActualizar.IdArticulo = _maestroDetalleArticulo.IdArticulo;
                        _maestroDetalleArticuloActualizar.IdMaestroArticuloSucursal = _maestroDetalleArticulo.IdMaestroArticuloSucursal;

                        db.MaestroDetalleArticulo.Update(_maestroDetalleArticuloActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwRm),
                            ExceptionTrace = ex,
                            Message = "Se ha producido una exepción",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Error ",
                        };
                    }
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe"
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepción"
                };
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteMaestroDetalleArticulo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo no válido ",
                    };
                }

                var respuesta = await db.MaestroDetalleArticulo.SingleOrDefaultAsync(m => m.IdMaestroDetalleArticulo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.MaestroDetalleArticulo.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = "Eliminado ",
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        private bool MaestroDetalleArticuloExists(int IdArticulo, int IdMaestroArticuloSucursal)
        {
            return db.MaestroDetalleArticulo.Any(e => e.IdMaestroDetalleArticulo == IdArticulo);
        }

        public Response Existe(MaestroDetalleArticulo _maestroDetalleArticulo)
        {
            var loglevelrespuesta = db.MaestroDetalleArticulo.Where(p => p.IdArticulo == _maestroDetalleArticulo.IdArticulo && p.IdMaestroArticuloSucursal == _maestroDetalleArticulo.IdMaestroArticuloSucursal).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = String.Format("Ya existe una cantidad insertada para el Artículo {0} y el Maestro de artículo de Sucursal {1}", _maestroDetalleArticulo?.Articulo?.Nombre ?? "<Sin nombre>", _maestroDetalleArticulo?.MaestroArticuloSucursal?.Sucursal?.Nombre ?? "<Sin nombre>"),
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
