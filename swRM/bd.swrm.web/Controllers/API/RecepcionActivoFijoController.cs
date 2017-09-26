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
using bd.swrm.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/RecepcionActivoFijo")]
    public class RecepcionActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public RecepcionActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/RecepcionActivoFijo
        [HttpGet]
        [Route("ListarRecepcionActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijo()
        {
            try
            {
                return await (from recAFD in db.RecepcionActivoFijoDetalle
                                  join recAF in db.RecepcionActivoFijo on recAFD.IdRecepcionActivoFijo equals recAF.IdRecepcionActivoFijo
                                  join af in db.ActivoFijo on recAFD.IdActivoFijo equals af.IdActivoFijo
                                  join est in db.Estado on recAFD.IdEstado equals est.IdEstado
                                  join subCAf in db.SubClaseActivoFijo on recAF.IdSubClaseActivoFijo equals subCAf.IdSubClaseActivoFijo
                                  join cAF in db.ClaseActivoFijo on subCAf.IdClaseActivoFijo equals cAF.IdClaseActivoFijo
                                  join tAF in db.TipoActivoFijo on cAF.IdTipoActivoFijo equals tAF.IdTipoActivoFijo
                                  join prov in db.Proveedor on recAF.IdProveedor equals prov.IdProveedor
                                  join libAF in db.LibroActivoFijo on af.IdLibroActivoFijo equals libAF.IdLibroActivoFijo
                                  join suc in db.Sucursal on libAF.IdSucursal equals suc.IdSucursal
                                  join ciud in db.Ciudad on suc.IdCiudad equals ciud.IdCiudad
                                  join provin in db.Provincia on ciud.IdProvincia equals provin.IdProvincia
                                  join pais in db.Pais on provin.IdPais equals pais.IdPais
                                  join emp in db.Empleado on recAF.IdEmpleado equals emp.IdEmpleado
                                  join pers in db.Persona on emp.IdPersona equals pers.IdPersona
                                  join codAF in db.CodigoActivoFijo on af.IdCodigoActivoFijo equals codAF.IdCodigoActivoFijo
                                  join motRec in db.MotivoRecepcion on recAF.IdMotivoRecepcion equals motRec.IdMotivoRecepcion
                                  join um in db.UnidadMedida on af.IdUnidadMedida equals um.IdUnidadMedida
                                  join mod in db.Modelo on af.IdModelo equals mod.IdModelo
                                  join marca in db.Marca on mod.IdMarca equals marca.IdMarca
                                  select new RecepcionActivoFijoDetalle {
                                      IdRecepcionActivoFijoDetalle = recAFD.IdRecepcionActivoFijoDetalle,
                                      IdActivoFijo = recAFD.IdActivoFijo,
                                      IdEstado = recAFD.IdEstado,
                                      IdRecepcionActivoFijo = recAFD.IdRecepcionActivoFijo,
                                      NumeroPoliza = recAFD.NumeroPoliza,
                                      RecepcionActivoFijo = new RecepcionActivoFijo { Fondo = recAF.Fondo, OrdenCompra = recAF.OrdenCompra, Cantidad = recAF.Cantidad, ValidacionTecnica = recAF.ValidacionTecnica, FechaRecepcion = recAF.FechaRecepcion, IdSubClaseActivoFijo = recAF.IdSubClaseActivoFijo, SubClaseActivoFijo = new SubClaseActivoFijo { IdSubClaseActivoFijo = subCAf.IdSubClaseActivoFijo, Nombre = subCAf.Nombre, IdClaseActivoFijo = subCAf.IdClaseActivoFijo, ClaseActivoFijo = new ClaseActivoFijo { IdClaseActivoFijo = cAF.IdClaseActivoFijo, Nombre = cAF.Nombre, IdTipoActivoFijo = cAF.IdTipoActivoFijo, TipoActivoFijo = new TipoActivoFijo { IdTipoActivoFijo = tAF.IdTipoActivoFijo, Nombre = tAF.Nombre } } }, IdProveedor = recAF.IdProveedor, Proveedor = new Proveedor { IdProveedor = prov.IdProveedor, Nombre = prov.Nombre, Apellidos = prov.Apellidos }, Empleado = new Empleado { IdEmpleado = emp.IdEmpleado, IdPersona = emp.IdPersona, Persona = new Persona { IdPersona = pers.IdPersona, Nombres = pers.Nombres, Apellidos = pers.Apellidos, Identificacion = pers.Identificacion, CorreoPrivado = pers.CorreoPrivado, FechaNacimiento = pers.FechaNacimiento, LugarTrabajo = pers.LugarTrabajo, TelefonoCasa = pers.TelefonoCasa, TelefonoPrivado = pers.TelefonoPrivado } }, IdMotivoRecepcion = recAF.IdMotivoRecepcion, MotivoRecepcion = new MotivoRecepcion { IdMotivoRecepcion = motRec.IdMotivoRecepcion, Descripcion = motRec.Descripcion } },
                                      ActivoFijo = new ActivoFijo { Nombre = af.Nombre, Serie = af.Serie, Ubicacion = af.Ubicacion, ValorCompra = af.ValorCompra, LibroActivoFijo = new LibroActivoFijo { IdLibroActivoFijo = libAF.IdLibroActivoFijo, IdSucursal = libAF.IdSucursal, Sucursal = new Sucursal { IdSucursal = suc.IdSucursal, IdCiudad = suc.IdCiudad, Nombre = suc.Nombre, Ciudad = new Ciudad { IdCiudad = ciud.IdCiudad, Nombre = ciud.Nombre, IdProvincia = ciud.IdProvincia, Provincia = new Provincia { IdProvincia = provin.IdProvincia, Nombre = provin.Nombre, IdPais = provin.IdPais, Pais = new Pais { IdPais = pais.IdPais, Nombre = pais.Nombre } } } } }, IdCodigoActivoFijo = af.IdCodigoActivoFijo, CodigoActivoFijo = new CodigoActivoFijo { IdCodigoActivoFijo = codAF.IdCodigoActivoFijo, Codigosecuencial = codAF.Codigosecuencial, CodigoBarras = codAF.CodigoBarras }, IdUnidadMedida = af.IdUnidadMedida, UnidadMedida = new UnidadMedida { IdUnidadMedida = um.IdUnidadMedida, Nombre = um.Nombre }, IdModelo = af.IdModelo, Modelo = new Modelo { IdModelo = mod.IdModelo, Nombre = mod.Nombre, IdMarca = mod.IdMarca, Marca = new Marca { IdMarca = marca.IdMarca, Nombre = marca.Nombre } } },
                                      Estado = new Estado { Nombre = est.Nombre }
                                  }).ToListAsync();
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
                return new List<RecepcionActivoFijoDetalle>();
            }
        }

        // GET: api/RecepcionActivoFijo/5
        [HttpGet("{id}")]
        public async Task<Response> GetRecepcionActivoFijoDetalle([FromRoute] int id)
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

                var recepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Empleado).ThenInclude(c=> c.Persona)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.LibroActivoFijo).ThenInclude(c=> c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c=> c.UnidadMedida)
                    .Include(c => c.ActivoFijo).ThenInclude(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .Include(c=> c.ActivoFijo).ThenInclude(c=> c.CodigoActivoFijo)
                    .Include(c => c.ActivoFijo)
                    .Include(c => c.Estado)
                    .Where(c=> c.IdRecepcionActivoFijoDetalle == id).SingleOrDefaultAsync();

                if (recepcionActivoFijoDetalle == null)
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
                    Resultado = recepcionActivoFijoDetalle,
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

        [HttpPost]
        [Route("ValidarModeloRecepcionActivoFijo")]
        public Response PostValidacionModeloRecepcionActivoFijo([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            ModelState.Remove("IdActivoFijo");
            ModelState.Remove("IdRecepcionActivoFijo");
            ModelState.Remove("ActivoFijo.IdCodigoActivoFijo");

            if (!ModelState.IsValid)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ModeloInvalido
                };
            }

            var respuesta = Existe(recepcionActivoFijoDetalle);
            if (!respuesta.IsSuccess)
            {
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

        [HttpPut("EstadoActivoFijo/{id}")]
        public async Task<Response> PutModificarEstadoActivoFijo([FromRoute] int id, [FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.Where(x => x.IdRecepcionActivoFijoDetalle == id).FirstOrDefaultAsync();
                if (recepcionActivoFijoDetalleActualizar != null)
                {
                    try
                    {
                        recepcionActivoFijoDetalleActualizar.Estado = await db.Estado.SingleOrDefaultAsync(c => c.Nombre == "Recepcionado");
                        recepcionActivoFijoDetalleActualizar.IdEstado = recepcionActivoFijoDetalleActualizar.Estado.IdEstado;
                        db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
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

        [HttpPost]
        [Route("DesaprobarActivoFijo")]
        public async Task<Response> PostDesaprobacionActivoFijo([FromBody] int idRecepcionActivoFijoDetalle)
        {
            try
            {
                var recepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
                if (recepcionActivoFijoDetalle != null)
                {
                    recepcionActivoFijoDetalle.Estado = await db.Estado.SingleOrDefaultAsync(c => c.Nombre == "Desaprobado");
                    recepcionActivoFijoDetalle.IdEstado = recepcionActivoFijoDetalle.Estado.IdEstado;

                    db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalle);
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
                    Message = Mensaje.RegistroNoEncontrado
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

        // POST: api/RecepcionActivoFijo
        [HttpPost]
        [Route("InsertarRecepcionActivoFijo")]
        public async Task<Response> PostRecepcionActivoFijo([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                ModelState.Remove("IdActivoFijo");
                ModelState.Remove("IdRecepcionActivoFijo");
                ModelState.Remove("ActivoFijo.IdCodigoActivoFijo");

                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var respuesta = Existe(recepcionActivoFijoDetalle);
                if (!respuesta.IsSuccess)
                {
                    db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo.SubClaseActivoFijo).State = EntityState.Unchanged;
                    db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo.LibroActivoFijo).State = EntityState.Unchanged;
                    await db.RecepcionActivoFijo.AddAsync(recepcionActivoFijoDetalle.RecepcionActivoFijo);
                    await db.SaveChangesAsync();

                    db.Entry(recepcionActivoFijoDetalle.ActivoFijo.Modelo).State = EntityState.Unchanged;
                    db.Entry(recepcionActivoFijoDetalle.ActivoFijo.Ciudad).State = EntityState.Unchanged;
                    db.Entry(recepcionActivoFijoDetalle.ActivoFijo.UnidadMedida).State = EntityState.Unchanged;
                    await db.ActivoFijo.AddAsync(recepcionActivoFijoDetalle.ActivoFijo);
                    await db.SaveChangesAsync();

                    db.Entry(recepcionActivoFijoDetalle.ActivoFijo).State = EntityState.Unchanged;                    
                    db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo).State = EntityState.Unchanged;
                    db.Entry(recepcionActivoFijoDetalle.Estado).State = EntityState.Unchanged;

                    db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                    await db.SaveChangesAsync();

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = recepcionActivoFijoDetalle
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

        // PUT: api/AsignarPoliza/5
        [HttpPost]
        [Route("AsignarPoliza")]
        public async Task<Response> AsignarPoliza([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var ActualizarActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Where(x => x.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle).FirstOrDefaultAsync();
                if (ActualizarActivoFijoDetalle != null)
                {
                    try
                    {
                        ActualizarActivoFijoDetalle.NumeroPoliza = recepcionActivoFijoDetalle.NumeroPoliza;
                        db.RecepcionActivoFijoDetalle.Update(ActualizarActivoFijoDetalle);
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

        public Response Existe(RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            var nombreActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Nombre.ToUpper().TrimEnd().TrimStart();
            var serieActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Serie.ToUpper().TrimEnd().TrimStart();
            var ubicacionActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Ubicacion.ToUpper().TrimEnd().TrimStart();
            var modeloActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdModelo;
            var unidadMedidaActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdUnidadMedida;
            var fondoRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.Fondo.ToUpper().TrimStart().TrimEnd();
            var ordenCompraRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.OrdenCompra.ToUpper().TrimStart().TrimEnd();

            var loglevelrespuesta = db.RecepcionActivoFijoDetalle.Where(p => p.ActivoFijo.Nombre.ToUpper().TrimStart().TrimEnd() == nombreActivoFijo
            && p.ActivoFijo.Serie.ToUpper().TrimStart().TrimEnd() == serieActivoFijo
            && p.ActivoFijo.Ubicacion.ToUpper().TrimStart().TrimEnd() == ubicacionActivoFijo
            && p.ActivoFijo.IdModelo == modeloActivoFijo
            && p.ActivoFijo.IdUnidadMedida == unidadMedidaActivoFijo
            && p.RecepcionActivoFijo.Fondo == fondoRecepcion
            && p.RecepcionActivoFijo.OrdenCompra == ordenCompraRecepcion).FirstOrDefault();
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