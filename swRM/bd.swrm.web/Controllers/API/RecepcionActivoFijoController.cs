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
using System.Linq.Expressions;

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
        
        [HttpGet]
        [Route("ListarRecepcionActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijo()
        {
            return await ListadoRecepcionActivoFijo();
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoPorEstado/{estado}")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoPorEstado(string estado)
        {
            return await ListadoRecepcionActivoFijo(c=> c.Estado.Nombre == estado);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoConPoliza")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoConPoliza()
        {
            return await ListadoRecepcionActivoFijo(c => c.Estado.Nombre == "Recepcionado" && c.NumeroPoliza != null);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoSinPoliza")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoSinPoliza()
        {
            return await ListadoRecepcionActivoFijo(c => c.Estado.Nombre == "Recepcionado" && c.NumeroPoliza == null);
        }

        [HttpGet]
        [Route("BienesReporte")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetBienesReporte()
        {
            var listaRecepcionActivoFijo = await ListadoRecepcionActivoFijo(c => c.Estado.Nombre != "Validación Técnica" && c.Estado.Nombre != "Desaprobado");
            return listaRecepcionActivoFijo.OrderBy(c => c.ActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Pais.Nombre).ThenBy(c => c.ActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Nombre).ThenBy(c => c.ActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Nombre).ThenBy(c => c.ActivoFijo.LibroActivoFijo.Sucursal.Nombre).ThenBy(c => c.ActivoFijo.LibroActivoFijo.IdSucursal).ThenBy(c => c.RecepcionActivoFijo.Empleado.Persona.Nombres).ThenBy(c => c.RecepcionActivoFijo.Empleado.Persona.Apellidos).ToList();
        }

        private async Task<List<RecepcionActivoFijoDetalle>> ListadoRecepcionActivoFijo(Expression<Func<RecepcionActivoFijoDetalle, bool>> predicado = null)
        {
            try
            {
                var lista = (from recAFD in db.RecepcionActivoFijoDetalle
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

                              //join afb in db.ActivosFijosBaja on af.IdActivoFijo equals afb.IdActivo
                              //into tmpAfb
                              //from resAFB in tmpAfb.DefaultIfEmpty()

                              //join motivoBaja in db.ActivoFijoMotivoBaja on resAFB.IdMotivoBaja equals motivoBaja.IdActivoFijoMotivoBaja
                              //into tmpMotB
                              //from resMotivoBaja in tmpMotB.DefaultIfEmpty()

                              //join afa in db.ActivosFijosAlta on af.IdActivoFijo equals afa.IdActivoFijo
                              //into tmp
                              //from resAFA in tmp.DefaultIfEmpty()

                              //join factura in db.Factura on resAFA.IdFactura equals factura.IdFactura
                              //into tmp1
                              //from resFac in tmp1.DefaultIfEmpty()

                              //let listaMantenimientoActivo = (from mant in db.MantenimientoActivoFijo
                              //                                join empleadMant in db.Empleado on mant.IdEmpleado equals empleadMant.IdEmpleado
                              //                                where mant.IdActivoFijo == af.IdActivoFijo
                              //                                select new MantenimientoActivoFijo { IdMantenimientoActivoFijo = mant.IdMantenimientoActivoFijo, FechaDesde = mant.FechaDesde, FechaHasta = mant.FechaHasta, FechaMantenimiento = mant.FechaMantenimiento, Observaciones = mant.Observaciones, Valor = mant.Valor, Empleado = empleadMant, IdEmpleado = empleadMant.IdEmpleado }).ToList()

                              //let listaFacturasProveedor = (from provFactura in db.Proveedor
                              //                              join fact in db.Factura on provFactura.IdProveedor equals fact.IdProveedor
                              //                              where provFactura.IdProveedor == prov.IdProveedor
                              //                              select new Factura { IdFactura = fact.IdFactura, Numero = fact.Numero }).ToList()

                              //let listaComponentesAdd = (from afadd in db.ActivosFijosAdicionados
                              //                          join afaf in db.ActivoFijo on afadd.idActivoFijoOrigen equals afaf.IdActivoFijo
                              //                          where afadd.idActivoFijoOrigen == af.IdActivoFijo
                              //                          select new ActivosFijosAdicionados { idAdicion = afadd.idAdicion, idActivoFijoOrigen = afadd.idActivoFijoOrigen, idActivoFijoDestino = afadd.idActivoFijoDestino, fechaAdicion = afadd.fechaAdicion }).ToList()
                              
                              select new RecepcionActivoFijoDetalle
                              {
                                  IdRecepcionActivoFijoDetalle = recAFD.IdRecepcionActivoFijoDetalle,
                                  IdActivoFijo = recAFD.IdActivoFijo,
                                  IdEstado = recAFD.IdEstado,
                                  IdRecepcionActivoFijo = recAFD.IdRecepcionActivoFijo,
                                  NumeroPoliza = recAFD.NumeroPoliza,
                                  RecepcionActivoFijo = new RecepcionActivoFijo { Fondo = recAF.Fondo, OrdenCompra = recAF.OrdenCompra, Cantidad = recAF.Cantidad, ValidacionTecnica = recAF.ValidacionTecnica, FechaRecepcion = recAF.FechaRecepcion, IdSubClaseActivoFijo = recAF.IdSubClaseActivoFijo, SubClaseActivoFijo = new SubClaseActivoFijo { IdSubClaseActivoFijo = subCAf.IdSubClaseActivoFijo, Nombre = subCAf.Nombre, IdClaseActivoFijo = subCAf.IdClaseActivoFijo, ClaseActivoFijo = new ClaseActivoFijo { IdClaseActivoFijo = cAF.IdClaseActivoFijo, Nombre = cAF.Nombre, IdTipoActivoFijo = cAF.IdTipoActivoFijo, TipoActivoFijo = new TipoActivoFijo { IdTipoActivoFijo = tAF.IdTipoActivoFijo, Nombre = tAF.Nombre } } }, IdProveedor = recAF.IdProveedor, Proveedor = new Proveedor { IdProveedor = prov.IdProveedor, Nombre = prov.Nombre, Apellidos = prov.Apellidos/*, Factura = listaFacturasProveedor*/ }, Empleado = new Empleado { IdEmpleado = emp.IdEmpleado, IdPersona = emp.IdPersona, Persona = new Persona { IdPersona = pers.IdPersona, Nombres = pers.Nombres, Apellidos = pers.Apellidos, Identificacion = pers.Identificacion, CorreoPrivado = pers.CorreoPrivado, FechaNacimiento = pers.FechaNacimiento, LugarTrabajo = pers.LugarTrabajo, TelefonoCasa = pers.TelefonoCasa, TelefonoPrivado = pers.TelefonoPrivado } }, IdMotivoRecepcion = recAF.IdMotivoRecepcion, MotivoRecepcion = new MotivoRecepcion { IdMotivoRecepcion = motRec.IdMotivoRecepcion, Descripcion = motRec.Descripcion } },
                                  ActivoFijo = new ActivoFijo { Nombre = af.Nombre, Serie = af.Serie, Ubicacion = af.Ubicacion, ValorCompra = af.ValorCompra, LibroActivoFijo = new LibroActivoFijo { IdLibroActivoFijo = libAF.IdLibroActivoFijo, IdSucursal = libAF.IdSucursal, Sucursal = new Sucursal { IdSucursal = suc.IdSucursal, IdCiudad = suc.IdCiudad, Nombre = suc.Nombre, Ciudad = new Ciudad { IdCiudad = ciud.IdCiudad, Nombre = ciud.Nombre, IdProvincia = ciud.IdProvincia, Provincia = new Provincia { IdProvincia = provin.IdProvincia, Nombre = provin.Nombre, IdPais = provin.IdPais, Pais = new Pais { IdPais = pais.IdPais, Nombre = pais.Nombre } } } } }, IdCodigoActivoFijo = af.IdCodigoActivoFijo, CodigoActivoFijo = new CodigoActivoFijo { IdCodigoActivoFijo = codAF.IdCodigoActivoFijo, Codigosecuencial = codAF.Codigosecuencial, CodigoBarras = codAF.CodigoBarras }, IdUnidadMedida = af.IdUnidadMedida, UnidadMedida = new UnidadMedida { IdUnidadMedida = um.IdUnidadMedida, Nombre = um.Nombre }, IdModelo = af.IdModelo, Modelo = new Modelo { IdModelo = mod.IdModelo, Nombre = mod.Nombre, IdMarca = mod.IdMarca, Marca = new Marca { IdMarca = marca.IdMarca, Nombre = marca.Nombre }, }, /*MantenimientoActivoFijo = listaMantenimientoActivo, ActivosFijosAlta = resAFA != null ? new ActivosFijosAlta { IdActivoFijo = resAFA.IdActivoFijo, IdFactura = resAFA.IdFactura, FechaAlta = resAFA.FechaAlta, Factura = resFac != null ? new Factura { IdFactura = resFac.IdFactura, Numero = resFac.Numero } : null } : null, ActivosFijosBaja = resAFB != null ? new ActivosFijosBaja { IdActivo = resAFB.IdActivo, FechaBaja = resAFB.FechaBaja, IdMotivoBaja = resAFB.IdMotivoBaja, ActivoFijoMotivoBaja = resMotivoBaja != null? new ActivoFijoMotivoBaja { IdActivoFijoMotivoBaja = resMotivoBaja.IdActivoFijoMotivoBaja, Nombre = resMotivoBaja.Nombre } : null } : null*/},
                                  Estado = new Estado { Nombre = est.Nombre }
                              });
                return await (predicado != null ? lista.Where(predicado).ToListAsync() : lista.ToListAsync());
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RecepcionActivoFijoDetalle>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetRecepcionActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var recepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.UnidadMedida)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.CodigoActivoFijo)
                    .Include(c => c.ActivoFijo)
                    .Include(c => c.Estado)
                    .Include(c => c.RecepcionActivoFijo.Proveedor.Factura)
                    //.Include(c => c.ActivoFijo.ActivosFijosAdicionados)
                    //.Include(c => c.ActivoFijo).ThenInclude(c => c.ActivosFijosBaja)
                    .Include(c => c.ActivoFijo.MantenimientoActivoFijo)
                    .Where(c=> c.IdRecepcionActivoFijoDetalle == id).SingleOrDefaultAsync();

                recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.TAF = recepcionActivoFijoDetalle?.RecepcionActivoFijo?.SubClaseActivoFijo?.ClaseActivoFijo?.TipoActivoFijo?.Nombre;
                recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.CAF = recepcionActivoFijoDetalle?.RecepcionActivoFijo?.SubClaseActivoFijo?.ClaseActivoFijo?.Nombre;
                return new Response { IsSuccess = recepcionActivoFijoDetalle != null, Message = recepcionActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
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
                        recepcionActivoFijoDetalleActualizar.Estado = await db.Estado.SingleOrDefaultAsync(c => c.Nombre == recepcionActivoFijoDetalle.Estado.Nombre);
                        recepcionActivoFijoDetalleActualizar.IdEstado = recepcionActivoFijoDetalleActualizar.Estado.IdEstado;
                        db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
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
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPost]
        [Route("InsertarRecepcionActivoFijo")]
        public async Task<Response> PostRecepcionActivoFijo([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                ModelState.Remove("IdActivoFijo");
                ModelState.Remove("IdRecepcionActivoFijo");
                ModelState.Remove("IdEstado");
                ModelState.Remove("ActivoFijo.IdCodigoActivoFijo");
                ModelState.Remove("ActivoFijo.LibroActivoFijo.Sucursal.Nombre");
                ModelState.Remove("RecepcionActivoFijo.LibroActivoFijo.Sucursal.Nombre");

                if (recepcionActivoFijoDetalle.RecepcionActivoFijo.ValidacionTecnica)
                {
                    ModelState.Remove("ActivoFijo.CodigoActivoFijo.CodigoBarras");
                    ModelState.Remove("ActivoFijo.CodigoActivoFijo.Codigosecuencial");
                }

                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                db.Entry(recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.ActivoFijo.LibroActivoFijo).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.ActivoFijo.Ciudad).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.ActivoFijo.UnidadMedida).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.ActivoFijo.Modelo).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo.Empleado).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo.MotivoRecepcion).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.RecepcionActivoFijo.Proveedor).State = EntityState.Unchanged;
                db.Entry(recepcionActivoFijoDetalle.Estado).State = EntityState.Unchanged;

                db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                db.EmpleadoActivoFijo.Add(new EmpleadoActivoFijo { IdActivoFijo = recepcionActivoFijoDetalle.IdActivoFijo, IdEmpleado = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdEmpleado, FechaAsignacion = DateTime.Now });
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = recepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutRecepcionActivoFijo([FromRoute] int id, [FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                ModelState.Remove("id");
                ModelState.Remove("ActivoFijo.LibroActivoFijo.Sucursal.Nombre");
                ModelState.Remove("RecepcionActivoFijo.LibroActivoFijo.Sucursal.Nombre");

                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.Include(c=> c.Estado).Include(c=> c.RecepcionActivoFijo).Include(c=> c.ActivoFijo).ThenInclude(c=> c.CodigoActivoFijo).SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                if (recepcionActivoFijoDetalleActualizar != null)
                {
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.FechaRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.FechaRecepcion;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.Cantidad = recepcionActivoFijoDetalle.RecepcionActivoFijo.Cantidad;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.ValidacionTecnica = recepcionActivoFijoDetalle.RecepcionActivoFijo.ValidacionTecnica;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.Fondo = recepcionActivoFijoDetalle.RecepcionActivoFijo.Fondo;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.OrdenCompra = recepcionActivoFijoDetalle.RecepcionActivoFijo.OrdenCompra;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.IdLibroActivoFijo = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdLibroActivoFijo;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.IdEmpleado = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdEmpleado;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.IdSubClaseActivoFijo = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdSubClaseActivoFijo;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.IdMotivoRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdMotivoRecepcion;
                    recepcionActivoFijoDetalleActualizar.RecepcionActivoFijo.IdProveedor = recepcionActivoFijoDetalle.RecepcionActivoFijo.IdProveedor;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.Nombre = recepcionActivoFijoDetalle.ActivoFijo.Nombre;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.Serie = recepcionActivoFijoDetalle.ActivoFijo.Serie;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.ValorCompra = recepcionActivoFijoDetalle.ActivoFijo.ValorCompra;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.Ubicacion = recepcionActivoFijoDetalle.ActivoFijo.Ubicacion;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdSubClaseActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdSubClaseActivoFijo;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdLibroActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdLibroActivoFijo;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdCiudad = recepcionActivoFijoDetalle.ActivoFijo.IdCiudad;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdUnidadMedida = recepcionActivoFijoDetalle.ActivoFijo.IdUnidadMedida;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdCodigoActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdCodigoActivoFijo;
                    recepcionActivoFijoDetalleActualizar.ActivoFijo.IdModelo = recepcionActivoFijoDetalle.ActivoFijo.IdModelo;
                    recepcionActivoFijoDetalleActualizar.IdEstado = recepcionActivoFijoDetalle.IdEstado;

                    if (!recepcionActivoFijoDetalle.RecepcionActivoFijo.ValidacionTecnica)
                    {
                        recepcionActivoFijoDetalleActualizar.ActivoFijo.CodigoActivoFijo.CodigoBarras = recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.CodigoBarras;
                        recepcionActivoFijoDetalleActualizar.ActivoFijo.CodigoActivoFijo.Codigosecuencial = recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.Codigosecuencial;
                    }
                    else
                    {
                        recepcionActivoFijoDetalleActualizar.ActivoFijo.CodigoActivoFijo.CodigoBarras = null;
                        recepcionActivoFijoDetalleActualizar.ActivoFijo.CodigoActivoFijo.Codigosecuencial = null;
                    }

                    db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }
        
        [HttpPost]
        [Route("AsignarPoliza")]
        public async Task<Response> AsignarPoliza([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                if (String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroPoliza))
                    return new Response { IsSuccess = false, Message = "Debe introducir el Número de Póliza." };

                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.Where(x => x.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle).FirstOrDefaultAsync();
                if (recepcionActivoFijoDetalleActualizar != null)
                {
                    if (!(recepcionActivoFijoDetalleActualizar.NumeroPoliza == null ? await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroPoliza == recepcionActivoFijoDetalle.NumeroPoliza) : await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroPoliza == recepcionActivoFijoDetalle.NumeroPoliza).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle)))
                    {
                        try
                        {
                            recepcionActivoFijoDetalleActualizar.NumeroPoliza = recepcionActivoFijoDetalle.NumeroPoliza;
                            db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                            return new Response { IsSuccess = false, Message = Mensaje.Error };
                        }
                    }
                    return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}