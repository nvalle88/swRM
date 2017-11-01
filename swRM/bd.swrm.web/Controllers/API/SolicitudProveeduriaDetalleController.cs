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
    [Route("api/SolicitudDetalleProveeduria")]
    public class SolicitudProveeduriaDetalleController : Controller
    {
        private readonly SwRMDbContext db;

        public SolicitudProveeduriaDetalleController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarSolicitudProveeduriasDetalle
        [HttpGet]
        [Route("ListarSolicitudProveeduriasDetalle")]
        public async Task<List<SolicitudProveduriaDetalle>> GetSolicitudProveeduria()
        {
            try
            {
                //return await db.SolicitudProveduriaDetalle.OrderBy(x => x.IdSolicitudProveduriaDetalle).Include(c=> c).ToListAsync();

                return await (from solProv in db.SolicitudProveduria
                              join solProvDet in db.SolicitudProveduriaDetalle on solProv.IdSolicitudProveduria equals solProvDet.IdSolicitudProveduria
                              join empl in db.Empleado on solProv.IdEmpleado equals empl.IdEmpleado
                              join pers in db.Persona on empl.IdPersona equals pers.IdPersona
                              join art in db.Articulo on solProvDet.IdArticulo equals art.IdArticulo
                              join est in db.Estado on solProvDet.IdEstado equals est.IdEstado

                              select new SolicitudProveduriaDetalle
                              {
                                  CantidadAprobada = solProvDet.CantidadAprobada,
                                  CantidadSolicitada = solProvDet.CantidadSolicitada,
                                  FechaAprobada = solProvDet.FechaAprobada,
                                  FechaSolicitud = solProvDet.FechaSolicitud,
                                  IdMaestroArticuloSucursal = solProvDet.IdMaestroArticuloSucursal,
                                  IdSolicitudProveduria = solProv.IdSolicitudProveduria,
                                  IdArticulo = art.IdArticulo,
                                  IdEstado = est.IdEstado,
                                  IdSolicitudProveduriaDetalle = solProvDet.IdSolicitudProveduriaDetalle,
                                  Articulo = new Articulo
                                  {
                                      IdArticulo = solProvDet.IdArticulo,
                                      Nombre = art.Nombre
                                  },
                                  Estado = new Estado
                                  {
                                      IdEstado = solProvDet.IdEstado,
                                      Nombre = est.Nombre
                                  },
                                  SolicitudProveduria = new SolicitudProveduria
                                  {
                                      IdSolicitudProveduria = solProv.IdSolicitudProveduria,
                                      IdEmpleado = empl.IdEmpleado,
                                      Empleado = new Empleado
                                      {
                                          IdEmpleado = solProv.IdEmpleado,
                                          IdPersona = empl.IdPersona,
                                          Persona = new Persona
                                          {
                                              IdPersona = empl.IdPersona,
                                              Nombres = pers.Nombres,
                                              Apellidos = pers.Apellidos
                                          }
                                      }
                                  }
                              }
                    ).Where(c => c.Estado.Nombre == "Solicitado").ToListAsync();
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
                return new List<SolicitudProveduriaDetalle>();
            }
        }

        // GET: api/SolicitudProveeduriaDetalle/5
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

                var SolicitudProveeduriaDetalle = await db.SolicitudProveduriaDetalle //.SingleOrDefaultAsync(m => m.IdSolicitudProveduriaDetalle == id);
                    .Include(c => c.Articulo)
                    .Include(c => c.Estado)
                    .Include(c => c.SolicitudProveduria).ThenInclude(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Where(c => c.IdSolicitudProveduriaDetalle == id).SingleOrDefaultAsync();
                    
                if (SolicitudProveeduriaDetalle == null)
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
                    Resultado = SolicitudProveeduriaDetalle,
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
        public async Task<Response> PutSolicitudProveeduriaDetalle([FromRoute] int id, [FromBody] SolicitudProveduriaDetalle SolicitudProveeduriaDetalle)
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

                var SolicitudProveeduriaDetalleActualizar = await db.SolicitudProveduriaDetalle.Where(x => x.IdSolicitudProveduriaDetalle == id).FirstOrDefaultAsync();
                if (SolicitudProveeduriaDetalleActualizar != null)
                {
                    try
                    {
                        SolicitudProveeduriaDetalleActualizar.IdSolicitudProveduriaDetalle = SolicitudProveeduriaDetalle.IdSolicitudProveduriaDetalle;
                        SolicitudProveeduriaDetalleActualizar.IdMaestroArticuloSucursal = SolicitudProveeduriaDetalle.IdMaestroArticuloSucursal;
                        SolicitudProveeduriaDetalleActualizar.IdArticulo = SolicitudProveeduriaDetalle.IdArticulo;
                        SolicitudProveeduriaDetalleActualizar.IdSolicitudProveduria = SolicitudProveeduriaDetalle.IdSolicitudProveduria;
                        SolicitudProveeduriaDetalleActualizar.CantidadAprobada = SolicitudProveeduriaDetalle.CantidadAprobada;
                        SolicitudProveeduriaDetalleActualizar.CantidadSolicitada = SolicitudProveeduriaDetalle.CantidadSolicitada;
                        SolicitudProveeduriaDetalleActualizar.FechaAprobada = SolicitudProveeduriaDetalle.FechaAprobada;
                        SolicitudProveeduriaDetalleActualizar.FechaSolicitud = SolicitudProveeduriaDetalle.FechaSolicitud;
                        SolicitudProveeduriaDetalleActualizar.IdEstado = SolicitudProveeduriaDetalle.IdEstado;
                        db.SolicitudProveduriaDetalle.Update(SolicitudProveeduriaDetalleActualizar);
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
        [Route("InsertarSolicitudProveeduriaDetalle")]
        public async Task<Response> PostSolicitudProveeduriaDetalle([FromBody] SolicitudProveduriaDetalle SolicitudProveeduriaDetalle)
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

                var respuesta = Existe(SolicitudProveeduriaDetalle);
                if (!respuesta.IsSuccess)
                {
                    db.SolicitudProveduriaDetalle.Add(SolicitudProveeduriaDetalle);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = SolicitudProveeduriaDetalle
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
        public async Task<Response> DeleteSolicitudProveeduriaDetalle([FromRoute] int id)
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

                var respuesta = await db.SolicitudProveduriaDetalle.SingleOrDefaultAsync(m => m.IdSolicitudProveduriaDetalle == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.SolicitudProveduriaDetalle.Remove(respuesta);
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

        private bool SolicitudProveeduriaDetalleExists(int id)
        {
            return db.SolicitudProveduriaDetalle.Any(e => e.IdSolicitudProveduriaDetalle == id);
        }

        public Response Existe(SolicitudProveduriaDetalle SolicitudProveeduriaDetalle)
        {
            var bdd = SolicitudProveeduriaDetalle.IdSolicitudProveduriaDetalle;
            var loglevelrespuesta = db.SolicitudProveduriaDetalle.Where(p => p.IdSolicitudProveduriaDetalle == bdd).FirstOrDefault();
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
