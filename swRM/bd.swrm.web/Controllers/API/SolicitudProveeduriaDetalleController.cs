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
        
        [HttpGet]
        [Route("ListarSolicitudProveeduriasDetalle")]
        public async Task<List<SolicitudProveeduriaDetalle>> GetSolicitudProveeduria()
        {
            try
            {
                return await (from solProv in db.SolicitudProveeduria
                              join solProvDet in db.SolicitudProveeduriaDetalle on solProv.IdSolicitudProveeduria equals solProvDet.IdSolicitudProveeduria
                              join empl in db.Empleado on solProv.IdEmpleado equals empl.IdEmpleado
                              join pers in db.Persona on empl.IdPersona equals pers.IdPersona
                              join art in db.Articulo on solProvDet.IdArticulo equals art.IdArticulo
                              join est in db.Estado on solProvDet.IdEstado equals est.IdEstado

                              select new SolicitudProveeduriaDetalle
                              {
                                  CantidadAprobada = solProvDet.CantidadAprobada,
                                  CantidadSolicitada = solProvDet.CantidadSolicitada,
                                  FechaAprobada = solProvDet.FechaAprobada,
                                  FechaSolicitud = solProvDet.FechaSolicitud,
                                  IdMaestroArticuloSucursal = solProvDet.IdMaestroArticuloSucursal,
                                  IdSolicitudProveeduria = solProv.IdSolicitudProveeduria,
                                  IdArticulo = art.IdArticulo,
                                  IdEstado = est.IdEstado,
                                  IdSolicitudProveeduriaDetalle = solProvDet.IdSolicitudProveeduriaDetalle,
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
                                  SolicitudProveeduria = new SolicitudProveeduria
                                  {
                                      IdSolicitudProveeduria = solProv.IdSolicitudProveeduria,
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
                              }).Where(c => c.Estado.Nombre == "Baja Solicitada").ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<SolicitudProveeduriaDetalle>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetSolicitudProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var solicitudProveeduriaDetalle = await db.SolicitudProveeduriaDetalle
                    .Include(c => c.Articulo)
                    .Include(c => c.Estado)
                    .Include(c => c.SolicitudProveeduria).ThenInclude(c => c.Empleado).ThenInclude(c => c.Persona)
                    .SingleOrDefaultAsync(c => c.IdSolicitudProveeduriaDetalle == id);
                return new Response { IsSuccess = solicitudProveeduriaDetalle != null, Message = solicitudProveeduriaDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = solicitudProveeduriaDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutSolicitudProveeduriaDetalle([FromRoute] int id, [FromBody] SolicitudProveeduriaDetalle SolicitudProveeduriaDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var SolicitudProveeduriaDetalleActualizar = await db.SolicitudProveeduriaDetalle.Where(x => x.IdSolicitudProveeduriaDetalle == id).FirstOrDefaultAsync();
                if (SolicitudProveeduriaDetalleActualizar != null)
                {
                    try
                    {
                        SolicitudProveeduriaDetalleActualizar.IdSolicitudProveeduriaDetalle = SolicitudProveeduriaDetalle.IdSolicitudProveeduriaDetalle;
                        SolicitudProveeduriaDetalleActualizar.IdMaestroArticuloSucursal = SolicitudProveeduriaDetalle.IdMaestroArticuloSucursal;
                        SolicitudProveeduriaDetalleActualizar.IdArticulo = SolicitudProveeduriaDetalle.IdArticulo;
                        SolicitudProveeduriaDetalleActualizar.IdSolicitudProveeduria = SolicitudProveeduriaDetalle.IdSolicitudProveeduria;
                        SolicitudProveeduriaDetalleActualizar.CantidadAprobada = SolicitudProveeduriaDetalle.CantidadAprobada;
                        SolicitudProveeduriaDetalleActualizar.CantidadSolicitada = SolicitudProveeduriaDetalle.CantidadSolicitada;
                        SolicitudProveeduriaDetalleActualizar.FechaAprobada = SolicitudProveeduriaDetalle.FechaAprobada;
                        SolicitudProveeduriaDetalleActualizar.FechaSolicitud = SolicitudProveeduriaDetalle.FechaSolicitud;
                        SolicitudProveeduriaDetalleActualizar.IdEstado = SolicitudProveeduriaDetalle.IdEstado;
                        db.SolicitudProveeduriaDetalle.Update(SolicitudProveeduriaDetalleActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                        return new Response { IsSuccess = false, Message = Mensaje.Error };
                    }
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }
        
        [HttpPost]
        [Route("InsertarSolicitudProveeduriaDetalle")]
        public async Task<Response> PostSolicitudProveeduriaDetalle([FromBody] SolicitudProveeduriaDetalle SolicitudProveeduriaDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                db.SolicitudProveeduriaDetalle.Add(SolicitudProveeduriaDetalle);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = SolicitudProveeduriaDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSolicitudProveeduriaDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.SolicitudProveeduriaDetalle.SingleOrDefaultAsync(m => m.IdSolicitudProveeduriaDetalle == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.SolicitudProveeduriaDetalle.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(SolicitudProveeduriaDetalle SolicitudProveeduriaDetalle)
        {
            var bdd = SolicitudProveeduriaDetalle.IdSolicitudProveeduriaDetalle;
            var loglevelrespuesta = db.SolicitudProveeduriaDetalle.Where(p => p.IdSolicitudProveeduriaDetalle == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
