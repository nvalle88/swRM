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
    [Route("api/RecepcionArticulo")]
    public class RecepcionArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public RecepcionArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarRecepcionArticulos
        [HttpGet]
        [Route("ListarRecepcionArticulos")]
        public async Task<List<RecepcionArticulos>> GetRecepcionArticulo()
        {
            try
            {
                //return await db.RecepcionArticulos.Include(c => c.Articulo).ThenInclude(c =>c.Modelo).ToListAsync();

                return await (from recA in db.RecepcionArticulos
                              join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                              join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                              join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                              join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                              join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                              join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                              join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                              join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                              select new RecepcionArticulos
                              {
                                  Articulo = new Articulo
                                  {
                                      Nombre = articulo.Nombre,
                                      IdArticulo = articulo.IdArticulo,
                                      IdModelo = articulo.IdModelo,
                                      IdSubClaseArticulo = articulo.IdSubClaseArticulo,
                                      IdUnidadMedida = articulo.IdUnidadMedida,
                                      SubClaseArticulo = new SubClaseArticulo
                                      {
                                          IdSubClaseArticulo = articulo.IdSubClaseArticulo,
                                          Nombre = subClaseArticulo.Nombre,
                                          IdClaseArticulo = subClaseArticulo.IdClaseArticulo,
                                          ClaseArticulo = new ClaseArticulo
                                          {
                                              IdClaseArticulo = subClaseArticulo.IdClaseArticulo,
                                              Nombre = claseArticulo.Nombre,
                                              TipoArticulo = new TipoArticulo {
                                                  IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                                  Nombre = tipoArticulo.Nombre
                                              }
                                          }
                                      },
                                      Modelo = new Modelo
                                      {
                                          IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1 ,
                                          Nombre = modelo.Nombre
                                      }
                                  },
                                  Cantidad = recA.Cantidad,
                                  Fecha = recA.Fecha,
                                  IdArticulo = recA.IdArticulo,
                                  IdEmpleado = recA.IdEmpleado,
                                  IdMaestroArticuloSucursal = recA.IdMaestroArticuloSucursal,
                                  IdProveedor = recA.IdProveedor,
                                  IdRecepcionArticulos = recA.IdRecepcionArticulos,
                                  Empleado = new Empleado
                                  {
                                      IdEmpleado = recA.IdEmpleado,
                                      IdPersona = empleado.IdPersona,
                                      Persona = new Persona
                                      {
                                          IdPersona = empleado.IdPersona,
                                          Nombres = persona.Nombres,
                                          Apellidos = persona.Apellidos
                                      }
                                  },
                                  Proveedor = new Proveedor
                                  {
                                      IdProveedor = recA.IdProveedor,
                                      Nombre = proveedor.Nombre,
                                      Apellidos = proveedor.Apellidos
                                  }
                              }
                              ).ToListAsync();
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
                return new List<RecepcionArticulos>();
            }
        }

        // GET: api/RecepcionArticulo/5
        [HttpGet("{id}")]
        public async Task<Response> GetRecepcionArticulo([FromRoute] int id)
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

                var recepcionArticulo = await db.RecepcionArticulos
                    .Include(c => c.Proveedor)
                    .Include(c => c.MaestroArticuloSucursal)
                    .Include(c => c.Articulo).ThenInclude(c => c.SubClaseArticulo).ThenInclude(c => c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo)
                    .Include(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.Articulo).ThenInclude(c => c.DetalleFactura)
                    .Include(c=> c.MaestroArticuloSucursal.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Where(c => c.IdRecepcionArticulos == id).SingleOrDefaultAsync();

                if (recepcionArticulo == null)
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
                    Resultado = recepcionArticulo,
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

        // PUT: api/RecepcionArticulo/5
        [HttpPut("{id}")]
        public async Task<Response> PutRecepcionArticulo([FromRoute] int id, [FromBody] RecepcionArticulos recepcionArticulo)
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

                var recepcionArticuloActualizar = await db.RecepcionArticulos.Where(x => x.IdRecepcionArticulos == id).FirstOrDefaultAsync();
                if (recepcionArticuloActualizar != null)
                {
                    try
                    {
                        var existenciaArticuloProveeduria = await db.ExistenciaArticuloProveeduria.SingleOrDefaultAsync(c => c.IdArticulo == recepcionArticulo.IdArticulo);
                        if (await db.AltaProveeduria.CountAsync(c=> c.IdArticulo == recepcionArticulo.IdArticulo) > 0)
                        {
                            if (recepcionArticulo.Cantidad < existenciaArticuloProveeduria.Existencia)
                            {
                                return new Response
                                {
                                    IsSuccess = false,
                                    Message = String.Format("La Cantidad no puede ser menor que la existencia real de Artículos ({0})", existenciaArticuloProveeduria.Existencia)
                                };
                            }
                            else
                                existenciaArticuloProveeduria.Existencia = recepcionArticulo.Cantidad >= recepcionArticuloActualizar.Cantidad ? (recepcionArticulo.Cantidad - recepcionArticuloActualizar.Cantidad) + existenciaArticuloProveeduria.Existencia : recepcionArticulo.Cantidad - existenciaArticuloProveeduria.Existencia;
                        }
                        else
                            existenciaArticuloProveeduria.Existencia = recepcionArticulo.Cantidad;

                        if (recepcionArticulo.MaestroArticuloSucursal.Minimo > recepcionArticulo.Cantidad || recepcionArticulo.MaestroArticuloSucursal.Maximo < recepcionArticulo.Cantidad)
                        {
                            return new Response
                            {
                                IsSuccess = false,
                                Message = "La Cantidad no está en el rango del Mínimo y Máximo"
                            };
                        }
                        
                        recepcionArticuloActualizar.Fecha = recepcionArticulo.Fecha;
                        recepcionArticuloActualizar.Cantidad = recepcionArticulo.Cantidad;
                        recepcionArticuloActualizar.IdEmpleado = recepcionArticulo.IdEmpleado;
                        recepcionArticuloActualizar.IdArticulo = recepcionArticulo.IdArticulo;
                        recepcionArticuloActualizar.IdMaestroArticuloSucursal = recepcionArticulo.IdMaestroArticuloSucursal;
                        recepcionArticuloActualizar.IdProveedor = recepcionArticulo.IdProveedor;
                        db.RecepcionArticulos.Update(recepcionArticuloActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = Mensaje.ModeloInvalido,
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

        // POST: api/RecepcionArticulo
        [HttpPost]
        [Route("InsertarRecepcionArticulo")]
        public async Task<Response> PostRecepcionArticulo([FromBody] RecepcionArticulos recepcionArticulo)
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

                if (recepcionArticulo.MaestroArticuloSucursal.Minimo > recepcionArticulo.Cantidad || recepcionArticulo.MaestroArticuloSucursal.Maximo < recepcionArticulo.Cantidad)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La Cantidad no está en el rango del Mínimo y Máximo"
                    };
                }
                
                db.Entry(recepcionArticulo.Articulo).State = EntityState.Unchanged;
                db.Entry(recepcionArticulo.MaestroArticuloSucursal).State = EntityState.Unchanged;
                db.Entry(recepcionArticulo).State = EntityState.Added;
                await db.SaveChangesAsync();

                db.ExistenciaArticuloProveeduria.Add(new ExistenciaArticuloProveeduria { IdArticulo = recepcionArticulo.IdArticulo, Existencia = recepcionArticulo.Cantidad });
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

        // DELETE: api/RecepcionArticulo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteRecepcionArticulo([FromRoute] int id)
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

                var respuesta = await db.RecepcionArticulos.SingleOrDefaultAsync(m => m.IdRecepcionArticulos == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.RecepcionArticulos.Remove(respuesta);
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
                
    }
}
