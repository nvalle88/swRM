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
    [Route("api/ProveeduriaReportes")]
    public class ProveeduriaReportesController : Controller
    {
        private readonly SwRMDbContext db;

        public ProveeduriaReportesController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ProveeduriaAltasReporte")]
        public async Task<List<RecepcionArticulos>> GetProveeduriaAltasReporte()
        {
            try
            {
                return await (
                        from recA in db.RecepcionArticulos
                        join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                        join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                        join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                        join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                        join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                        join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                        join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                        join persona in db.Persona on empleado.IdPersona equals persona.IdPersona
                        join alta in db.AltaProveeduria on recA.IdArticulo equals alta.IdArticulo

                        where !(
                            from sp in db.SolicitudProveeduriaDetalle
                            where sp.IdEstado == 6
                            select sp.IdArticulo
                        ).Contains(alta.IdArticulo)

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
                                        TipoArticulo = new TipoArticulo { IdTipoArticulo = tipoArticulo.IdTipoArticulo, Nombre = tipoArticulo.Nombre }
                                    }
                                },
                                Modelo = new Modelo { IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1, Nombre = modelo.Nombre }
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
                        }).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RecepcionArticulos>();
            }
        }
        
        [HttpGet]
        [Route("ProveeduriaBajasReporte")]
        public async Task<List<RecepcionArticulos>> GetProveeduriaBajasReporte()
        {
            try
            {
                var lista = await (

                    from recA in db.RecepcionArticulos

                    join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                    join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                    join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    where (
                        from sp in db.SolicitudProveeduriaDetalle
                        where sp.IdEstado == 6
                        select sp.IdArticulo
                    ).Contains(recA.IdArticulo)

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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
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
                return lista;
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

        //Get: api/EstadisticasConsumoAreaReporte
        [HttpGet]
        [Route("EstadisticasConsumoAreaReporte")]
        public async Task<List<SolicitudProveeduriaDetalle>> GetEstadisticasConsumoAreaReporte()
        {
            try
            {
                var lista = await (

                    from SPD in db.SolicitudProveeduriaDetalle

                    join articulo in db.Articulo on SPD.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo

                    join MAS in db.MaestroArticuloSucursal on SPD.IdMaestroArticuloSucursal equals MAS.IdMaestroArticuloSucursal
                    join sucursal in db.Sucursal on MAS.IdSucursal equals sucursal.IdSucursal
                    join estado in db.Estado on SPD.IdEstado equals estado.IdEstado

                    select new SolicitudProveeduriaDetalle
                    {
                        IdSolicitudProveeduriaDetalle = SPD.IdSolicitudProveeduriaDetalle,
                        FechaSolicitud = SPD.FechaSolicitud,
                        FechaAprobada = SPD.FechaAprobada,
                        CantidadSolicitada = SPD.CantidadSolicitada,
                        CantidadAprobada = SPD.CantidadAprobada,
                        IdSolicitudProveeduria = SPD.IdSolicitudProveeduria,
                        IdEstado = SPD.IdEstado,
                        Estado = new Estado
                        {
                            Nombre = estado.Nombre
                        },
                        IdMaestroArticuloSucursal = SPD.IdMaestroArticuloSucursal,
                        MaestroArticuloSucursal = new MaestroArticuloSucursal
                        {
                            Sucursal = new Sucursal
                            {
                                Nombre = sucursal.Nombre
                            }
                        },
                        IdArticulo = SPD.IdArticulo,
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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
                                Nombre = modelo.Nombre
                            }
                        }
                    }
                ).ToListAsync();
                return lista;
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
                return new List<SolicitudProveeduriaDetalle>();
            }
        }

        //Get: api/AlertaVencimientoReporte
        [HttpGet]
        [Route("AlertaVencimientoReporte")]
        public async Task<List<RecepcionArticulos>> GetAlertaVencimientoReporte()
        {
            try
            {
                var lista = await (

                    from recA in db.RecepcionArticulos

                    join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                    join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                    join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    join maestroSuc in db.MaestroArticuloSucursal on recA.IdMaestroArticuloSucursal equals maestroSuc.IdMaestroArticuloSucursal
                    join sucursal in db.Sucursal on maestroSuc.IdSucursal equals sucursal.IdSucursal

                    //alerta cuando el articulo se acerca 10 lugares al minimo y es aun mayor al minimo
                    where (recA.Cantidad <= maestroSuc.Minimo + 10) && (recA.Cantidad >= maestroSuc.Minimo)
                    
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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
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
                        },
                        MaestroArticuloSucursal = new MaestroArticuloSucursal
                        {
                            Sucursal = new Sucursal
                            {
                                Nombre = sucursal.Nombre
                            }
                        }
                    }
                
                ).ToListAsync();
                return lista;
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

        //Get: api/ConsolidadoInventarioReporte
        [HttpGet]
        [Route("ConsolidadoInventarioReporte")]
        public async Task<List<RecepcionArticulos>> GetConsolidadoInventarioReporte()
        {
            try
            {
                var lista = await (

                    from recA in db.RecepcionArticulos

                    join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                    join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                    join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    where !(
                        from alta in db.AltaProveeduria
                        select alta.IdArticulo
                    ).Contains(recA.IdArticulo)

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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
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
                return lista;
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

        //Get: api/ConsolidadoSolicitudReporte
        [HttpGet]
        [Route("ConsolidadoSolicitudReporte")]
        public async Task<List<SolicitudProveeduriaDetalle>> GetConsolidadoSolicitudReporte()
        {
            try
            {
                var lista = await (

                    from SPD in db.SolicitudProveeduriaDetalle

                    join articulo in db.Articulo on SPD.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo

                    join MAS in db.MaestroArticuloSucursal on SPD.IdMaestroArticuloSucursal equals MAS.IdMaestroArticuloSucursal
                    join sucursal in db.Sucursal on MAS.IdSucursal equals sucursal.IdSucursal
                    join estado in db.Estado on SPD.IdEstado equals estado.IdEstado
                    join sp in db.SolicitudProveeduria on SPD.IdSolicitudProveeduria equals sp.IdSolicitudProveeduria
                    join empleado in db.Empleado on sp.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    select new SolicitudProveeduriaDetalle
                    {
                        IdSolicitudProveeduriaDetalle = SPD.IdSolicitudProveeduriaDetalle,
                        FechaSolicitud = SPD.FechaSolicitud,
                        FechaAprobada = SPD.FechaAprobada,
                        CantidadSolicitada = SPD.CantidadSolicitada,
                        CantidadAprobada = SPD.CantidadAprobada,
                        IdSolicitudProveeduria = SPD.IdSolicitudProveeduria,
                        SolicitudProveeduria = new SolicitudProveeduria
                        {
                            IdSolicitudProveeduria = sp.IdSolicitudProveeduria,
                            IdEmpleado = sp.IdEmpleado,
                            Empleado = new Empleado
                            {
                                IdEmpleado = empleado.IdEmpleado,
                                IdPersona =  empleado.IdPersona,
                                Persona = new Persona
                                {
                                    IdPersona = persona.IdPersona,
                                    Nombres = persona.Nombres,
                                    Apellidos = persona.Apellidos
                                }
                            }
                        },
                        IdEstado = SPD.IdEstado,
                        Estado = new Estado
                        {
                            Nombre = estado.Nombre
                        },
                        IdMaestroArticuloSucursal = SPD.IdMaestroArticuloSucursal,
                        MaestroArticuloSucursal = new MaestroArticuloSucursal
                        {
                            Maximo = MAS.Maximo,
                            Minimo = MAS.Minimo,
                            Sucursal = new Sucursal
                            {
                                Nombre = sucursal.Nombre
                            }
                        },
                        IdArticulo = SPD.IdArticulo,
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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
                                Nombre = modelo.Nombre
                            }
                        }                        
                    }
                ).ToListAsync();
                return lista;
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
                return new List<SolicitudProveeduriaDetalle>();
            }
        }

        //Get: api/ProveeduriaMinMaxReporte
        [HttpGet]
        [Route("ProveeduriaMinMaxReporte")]
        public async Task<List<RecepcionArticulos>> GetProveeduriaMinMaxReporte()
        {
            try
            {
                var lista = await (

                    from recA in db.RecepcionArticulos

                    join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                    join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                    join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    join maestroSuc in db.MaestroArticuloSucursal on recA.IdMaestroArticuloSucursal equals maestroSuc.IdMaestroArticuloSucursal
                    join sucursal in db.Sucursal on maestroSuc.IdSucursal equals sucursal.IdSucursal

                    where recA.Cantidad <= maestroSuc.Minimo

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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
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
                        },
                        MaestroArticuloSucursal = new MaestroArticuloSucursal
                        {
                            Sucursal = new Sucursal
                            {
                                Nombre = sucursal.Nombre
                            }
                        }
                    }
                
                ).Union(
                    from recA in db.RecepcionArticulos

                    join articulo in db.Articulo on recA.IdArticulo equals articulo.IdArticulo
                    join modelo in db.Modelo on articulo.IdModelo equals modelo.IdModelo
                    join subClaseArticulo in db.SubClaseArticulo on articulo.IdSubClaseArticulo equals subClaseArticulo.IdSubClaseArticulo
                    join claseArticulo in db.ClaseArticulo on subClaseArticulo.IdClaseArticulo equals claseArticulo.IdClaseArticulo
                    join tipoArticulo in db.TipoArticulo on claseArticulo.IdTipoArticulo equals tipoArticulo.IdTipoArticulo
                    join proveedor in db.Proveedor on recA.IdProveedor equals proveedor.IdProveedor
                    join empleado in db.Empleado on recA.IdEmpleado equals empleado.IdEmpleado
                    join persona in db.Persona on empleado.IdPersona equals persona.IdPersona

                    join maestroSuc in db.MaestroArticuloSucursal on recA.IdMaestroArticuloSucursal equals maestroSuc.IdMaestroArticuloSucursal
                    join sucursal in db.Sucursal on maestroSuc.IdSucursal equals sucursal.IdSucursal

                    where recA.Cantidad >= maestroSuc.Maximo

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
                                    TipoArticulo = new TipoArticulo
                                    {
                                        IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                                        Nombre = tipoArticulo.Nombre
                                    }
                                }
                            },
                            Modelo = new Modelo
                            {
                                IdModelo = articulo.IdModelo != null ? (int)articulo.IdModelo : -1,
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
                        },
                        MaestroArticuloSucursal = new MaestroArticuloSucursal
                        {
                            Sucursal = new Sucursal
                            {
                                Nombre = sucursal.Nombre
                            }
                        }
                    }

                ).ToListAsync();

                return lista;
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
    }
}
