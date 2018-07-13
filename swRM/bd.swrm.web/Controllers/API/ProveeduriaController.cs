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
using bd.swrm.servicios.Interfaces;
using bd.swrm.entidades.ObjectTransfer;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Proveeduria")]
    public class ProveeduriaController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;
        private readonly IClaimsTransfer claimsTransfer;

        public ProveeduriaController(SwRMDbContext db, IUploadFileService uploadFileService, IClaimsTransfer claimsTransfer, IHttpContextAccessor httpContextAccessor)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
            this.claimsTransfer = claimsTransfer;
        }

        [HttpGet]
        [Route("ObtenerOrdenCompra/{id}")]
        public async Task<Response> GetOrdenCompra([FromRoute] int id)
        {
            try
            {
                var ordenCompra = await db.OrdenCompra
                    .Include(c => c.Factura)
                    .Include(c => c.Estado)
                    .Include(c=> c.EmpleadoResponsable).ThenInclude(c=> c.Persona)
                    .Include(c=> c.Bodega).ThenInclude(c=> c.Sucursal)
                    .Include(c=> c.MotivoRecepcionArticulos)
                    .FirstOrDefaultAsync(c => c.IdOrdenCompra == id);

                if (ordenCompra.IdProveedor != null)
                    ordenCompra.Proveedor = await db.Proveedor.FirstOrDefaultAsync(c => c.IdProveedor == ordenCompra.IdProveedor);

                if (ordenCompra.IdEmpleadoDevolucion != null)
                    ordenCompra.EmpleadoDevolucion = await db.Empleado.Include(c=> c.Persona)
                        .Include(c=> c.Dependencia).ThenInclude(c=> c.Sucursal)
                        .FirstOrDefaultAsync(c => c.IdEmpleado == ordenCompra.IdEmpleadoDevolucion);

                ordenCompra.OrdenCompraDetalles = await db.OrdenCompraDetalles.Include(c=> c.MaestroArticuloSucursal).ThenInclude(c=> c.Articulo).ThenInclude(c=> c.UnidadMedida).Where(c => c.IdOrdenCompra == ordenCompra.IdOrdenCompra).ToListAsync();
                return new Response { IsSuccess = ordenCompra != null, Message = ordenCompra != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = ordenCompra };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpGet]
        [Route("ObtenerRequerimientoArticulos/{id}")]
        public async Task<Response> GetRequerimientoArticulos([FromRoute] int id)
        {
            try
            {
                var requerimientoArticulos = await db.RequerimientoArticulos.Include(c => c.Estado).Include(c => c.FuncionarioSolicitante).ThenInclude(c => c.Persona).Include(c => c.FuncionarioSolicitante).ThenInclude(c => c.Dependencia).ThenInclude(c => c.Sucursal).FirstOrDefaultAsync(c => c.IdRequerimientoArticulos == id);
                requerimientoArticulos.RequerimientosArticulosDetalles = await db.RequerimientosArticulosDetalles.Include(c=> c.MaestroArticuloSucursal).ThenInclude(c=> c.Articulo).Where(c => c.IdRequerimientosArticulos == requerimientoArticulos.IdRequerimientoArticulos).ToListAsync();
                
                foreach (var item in requerimientoArticulos.RequerimientosArticulosDetalles)
                {
                    var bodega = await PostBodegaPorDependencia((int)requerimientoArticulos.FuncionarioSolicitante.IdDependencia);
                    if (bodega != null)
                    {
                        var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdArticulo == item.MaestroArticuloSucursal.IdArticulo && c.IdBodega == bodega.IdBodega);
                        item.CantidadBodega = inventarioArticulos.Cantidad;
                    }
                }
                return new Response { IsSuccess = requerimientoArticulos != null, Message = requerimientoArticulos != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = requerimientoArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ListadoOrdenCompraPorEstado")]
        public async Task<List<OrdenCompra>> PostListadoOrdenCompraPorEstado([FromBody] string estado)
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoOrdenesCompra = new List<OrdenCompra>();
                var idsOrdenesCompra = claimTransfer.IsAdminNacionalProveeduria ? await db.OrdenCompra
                    .Include(c=> c.Estado)
                    .Where(c => c.Estado.Nombre == estado)
                    .Select(c=> c.IdOrdenCompra).ToListAsync() :
                    await db.OrdenCompra
                    .Include(c => c.Estado)
                    .Include(c=> c.EmpleadoResponsable)
                    .Where(c => c.Estado.Nombre == estado && c.EmpleadoResponsable.IdDependencia == claimTransfer.IdDependencia)
                    .Select(c => c.IdOrdenCompra).ToListAsync();
                foreach (var item in idsOrdenesCompra)
                {
                    var response = await GetOrdenCompra(item);
                    if (response.IsSuccess)
                        listadoOrdenesCompra.Add(response.Resultado as OrdenCompra);
                }
                return listadoOrdenesCompra;
            }
            catch (Exception)
            {
                return new List<OrdenCompra>();
            }
        }

        [HttpPost]
        [Route("BodegaPorDependencia")]
        public async Task<Bodega> PostBodegaPorDependencia([FromBody] int idDependencia)
        {
            try
            {
                var dependencia = await db.Dependencia.Include(c=> c.Bodega).FirstOrDefaultAsync(c => c.IdDependencia == idDependencia);
                return dependencia?.Bodega;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("ListadoRequerimientoArticulosPorEstado")]
        public async Task<List<RequerimientoArticulos>> PostListadoRequerimientoArticulosPorEstado([FromBody] string estado)
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoRequerimientoArticulos = new List<RequerimientoArticulos>();
                var idsRequerimientoArticulos = claimTransfer.IsAdminNacionalProveeduria ? await db.RequerimientoArticulos
                    .Include(c => c.Estado)
                    .Where(c => c.Estado.Nombre == estado)
                    .Select(c => c.IdRequerimientoArticulos).ToListAsync() :
                    await db.RequerimientoArticulos
                    .Include(c => c.Estado)
                    .Include(c => c.FuncionarioSolicitante)
                    .Where(c => c.Estado.Nombre == estado && c.FuncionarioSolicitante.IdDependencia == claimTransfer.IdDependencia)
                    .Select(c => c.IdRequerimientoArticulos).ToListAsync();
                foreach (var item in idsRequerimientoArticulos)
                {
                    var response = await GetRequerimientoArticulos(item);
                    if (response.IsSuccess)
                        listadoRequerimientoArticulos.Add(response.Resultado as RequerimientoArticulos);
                }
                return listadoRequerimientoArticulos;
            }
            catch (Exception)
            {
                return new List<RequerimientoArticulos>();
            }
        }

        [HttpPost]
        [Route("DetallesMaestroArticulo")]
        public async Task<List<MaestroArticuloSucursalSeleccionado>> PostDetallesArticulos([FromBody] IdSucursalIdRecepcionActivoFijoDetalleSeleccionado idSucursalIdRecepcionActivoFijoDetalleSeleccionado)
        {
            var lista = new List<MaestroArticuloSucursalSeleccionado>();
            try
            {
                var listaArticulos = await db.MaestroArticuloSucursal.Include(c=> c.Articulo).ThenInclude(c => c.SubClaseArticulo).ThenInclude(c => c.ClaseArticulo).ThenInclude(c => c.TipoArticulo).Include(c=> c.Articulo).ThenInclude(c => c.UnidadMedida).Include(c => c.Articulo).ThenInclude(c => c.Modelo).ThenInclude(c => c.Marca).Where(c=> c.IdSucursal == idSucursalIdRecepcionActivoFijoDetalleSeleccionado.IdSucursal && c.Habilitado).OrderBy(x => x.Articulo.Nombre).ToListAsync();
                var listaIdsRAFDSeleccionados = idSucursalIdRecepcionActivoFijoDetalleSeleccionado.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();

                foreach (var item in listaArticulos)
                {
                    int cantidadBodega = 0;
                    if (claimTransfer.IsFuncionarioSolicitante)
                    {
                        var bodega = await PostBodegaPorDependencia((int)claimTransfer.IdDependencia);
                        if (bodega != null)
                        {
                            var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdArticulo == item.IdArticulo && c.IdBodega == bodega.IdBodega);
                            cantidadBodega = inventarioArticulos?.Cantidad ?? 0;
                        }
                    }
                    if (!claimTransfer.IsFuncionarioSolicitante || cantidadBodega > 0)
                    {
                        lista.Add(new MaestroArticuloSucursalSeleccionado
                        {
                            MaestroArticuloSucursal = item,
                            Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdArticulo),
                            CantidadBodega = cantidadBodega
                        });
                    }
                }
            }
            catch (Exception)
            {
                return new List<MaestroArticuloSucursalSeleccionado>();
            }
            return lista;
        }

        [HttpPost]
        [Route("InsertarOrdenCompra")]
        public async Task<Response> PostInsertarOrdenCompra([FromBody] OrdenCompra ordenCompra)
        {
            try
            {
                var nuevaOrdenCompra = new OrdenCompra();
                using (var transaction = db.Database.BeginTransaction())
                {
                    var factura = new FacturaActivoFijo
                    {
                        NumeroFactura = ordenCompra.Factura.NumeroFactura,
                        FechaFactura = ordenCompra.Factura.FechaFactura
                    };
                    db.FacturaActivoFijo.Add(factura);
                    await db.SaveChangesAsync();

                    var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                    var estadoEnTramite = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.EnTramite);
                    nuevaOrdenCompra.IdOrdenCompra = ordenCompra.IdOrdenCompra;
                    nuevaOrdenCompra.IdMotivoRecepcionArticulos = ordenCompra.IdMotivoRecepcionArticulos;
                    nuevaOrdenCompra.IdBodega = ordenCompra.IdBodega;
                    nuevaOrdenCompra.IdProveedor = ordenCompra.IdProveedor;
                    nuevaOrdenCompra.Codigo = ordenCompra.Codigo;
                    nuevaOrdenCompra.Fecha = ordenCompra.Fecha;
                    nuevaOrdenCompra.IdEstado = estadoEnTramite.IdEstado;
                    nuevaOrdenCompra.IdFacturaActivoFijo = factura.IdFacturaActivoFijo;
                    nuevaOrdenCompra.IdEmpleadoResponsable = (int)claimTransfer.IdEmpleado;
                    nuevaOrdenCompra.IdEmpleadoDevolucion = ordenCompra.IdEmpleadoDevolucion;
                    db.OrdenCompra.Add(nuevaOrdenCompra);
                    await db.SaveChangesAsync();

                    foreach (var item in ordenCompra.OrdenCompraDetalles)
                    {
                        db.OrdenCompraDetalles.Add(new OrdenCompraDetalles
                        {
                            IdOrdenCompra = nuevaOrdenCompra.IdOrdenCompra,
                            IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                            ValorUnitario = item.ValorUnitario,
                            Cantidad = item.Cantidad
                        });
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevaOrdenCompra };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarRequerimientoArticulos")]
        public async Task<Response> PostInsertarRequerimientoArticulos([FromBody] RequerimientoArticulos requerimientoArticulos)
        {
            try
            {
                var nuevoRequerimientoArticulos = new RequerimientoArticulos();
                using (var transaction = db.Database.BeginTransaction())
                {
                    var estadoSolicitado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Solicitado);
                    var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();

                    nuevoRequerimientoArticulos.IdRequerimientoArticulos = requerimientoArticulos.IdRequerimientoArticulos;
                    nuevoRequerimientoArticulos.IdFuncionarioSolicitante = (int)claimTransfer.IdEmpleado;
                    nuevoRequerimientoArticulos.FechaSolicitud = DateTime.Now;
                    nuevoRequerimientoArticulos.FechaAprobadoDenegado = null;
                    nuevoRequerimientoArticulos.Observaciones = requerimientoArticulos.Observaciones;
                    nuevoRequerimientoArticulos.IdEstado = estadoSolicitado.IdEstado;
                    db.RequerimientoArticulos.Add(nuevoRequerimientoArticulos);
                    await db.SaveChangesAsync();

                    foreach (var item in requerimientoArticulos.RequerimientosArticulosDetalles)
                    {
                        db.RequerimientosArticulosDetalles.Add(new RequerimientosArticulosDetalles
                        {
                            IdRequerimientosArticulos = nuevoRequerimientoArticulos.IdRequerimientoArticulos,
                            IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                            CantidadSolicitada = item.CantidadSolicitada,
                            CantidadAprobada = 0
                        });
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevoRequerimientoArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("DenegarRequerimientoArticulos")]
        public async Task<Response> PostDenegarRequerimientoArticulos([FromBody] int id)
        {
            try
            {
                var requerimientoArticulos = await db.RequerimientoArticulos.FirstOrDefaultAsync(c => c.IdRequerimientoArticulos == id);
                if (requerimientoArticulos != null)
                {
                    var estadoDesaprobado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Desaprobado);
                    requerimientoArticulos.IdEstado = estadoDesaprobado.IdEstado;
                    requerimientoArticulos.FechaAprobadoDenegado = DateTime.Now;
                    db.RequerimientoArticulos.Update(requerimientoArticulos);
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
        [Route("ExisteBodegaParaDependencia")]
        public async Task<Response> PostExisteBodegaParaDependencia([FromBody] int idDependencia)
        {
            try
            {
                var dependencia = await db.Dependencia.Include(c=> c.Bodega).FirstOrDefaultAsync(c => c.IdDependencia == idDependencia);
                return new Response { IsSuccess = dependencia.Bodega != null, Message = dependencia.Bodega != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("DespacharRequerimientoArticulos")]
        public async Task<Response> PostDespacharRequerimientoArticulos([FromBody] SalidaArticulos salidaArticulos)
        {
            try
            {
                var nuevaSalidaArticulos = new SalidaArticulos();
                using (var transaction = db.Database.BeginTransaction())
                {
                    nuevaSalidaArticulos.IdSalidaArticulos = salidaArticulos.IdSalidaArticulos;
                    nuevaSalidaArticulos.IdMotivoSalidaArticulos = salidaArticulos.IdMotivoSalidaArticulos;
                    nuevaSalidaArticulos.DescripcionMotivo = salidaArticulos.DescripcionMotivo;
                    nuevaSalidaArticulos.IdEmpleadoRealizaBaja = salidaArticulos.IdEmpleadoRealizaBaja;
                    nuevaSalidaArticulos.IdProveedorDevolucion = salidaArticulos.IdProveedorDevolucion;
                    nuevaSalidaArticulos.IdEmpleadoDespacho = salidaArticulos.IdEmpleadoDespacho;
                    nuevaSalidaArticulos.IdRequerimientoArticulos = salidaArticulos.IdRequerimientoArticulos;
                    db.SalidaArticulos.Add(nuevaSalidaArticulos);
                    await db.SaveChangesAsync();

                    var fechaSalida = DateTime.Now;
                    foreach (var item in salidaArticulos.RequerimientoArticulos.RequerimientosArticulosDetalles)
                    {
                        var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdArticulo == item.MaestroArticuloSucursal.IdArticulo && c.IdBodega == salidaArticulos.RequerimientoArticulos.FuncionarioSolicitante.Dependencia.IdBodega);
                        if (inventarioArticulos != null)
                        {
                            int diferencia = inventarioArticulos.Cantidad < item.CantidadSolicitada ? (item.CantidadSolicitada - inventarioArticulos.Cantidad) : (inventarioArticulos.Cantidad - item.CantidadSolicitada);
                            db.InventarioArticulos.Add(new InventarioArticulos
                            {
                                IdArticulo = item.MaestroArticuloSucursal.IdArticulo,
                                IdBodega = (int)salidaArticulos.RequerimientoArticulos.FuncionarioSolicitante.Dependencia.IdBodega,
                                Cantidad = diferencia,
                                Fecha = fechaSalida
                            });
                            await db.SaveChangesAsync();

                            var requerimientoArticulosDetalles = await db.RequerimientosArticulosDetalles.FirstOrDefaultAsync(c => c.IdRequerimientosArticulos == salidaArticulos.IdRequerimientoArticulos && c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal);
                            if (requerimientoArticulosDetalles != null)
                            {
                                requerimientoArticulosDetalles.CantidadAprobada = inventarioArticulos.Cantidad < item.CantidadSolicitada ? inventarioArticulos.Cantidad : item.CantidadSolicitada;
                                db.RequerimientosArticulosDetalles.Update(requerimientoArticulosDetalles);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                    var requerimientoArticulos = await db.RequerimientoArticulos.FirstOrDefaultAsync(c => c.IdRequerimientoArticulos == salidaArticulos.IdRequerimientoArticulos);
                    if (requerimientoArticulos != null)
                    {
                        var estadoDespachado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Despachado);
                        requerimientoArticulos.IdEstado = estadoDespachado.IdEstado;
                        requerimientoArticulos.FechaAprobadoDenegado = fechaSalida;
                        db.RequerimientoArticulos.Update(requerimientoArticulos);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevaSalidaArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ProcesarOrdenCompra")]
        public async Task<Response> PostProcesarOrdenCompra([FromBody] int id)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var response = await GetOrdenCompra(id);
                    if (response.IsSuccess)
                    {
                        var ordenCompraActualizar = response.Resultado as OrdenCompra;
                        if (ordenCompraActualizar != null)
                        {
                            var fechaOrdenCompra = new DateTime(ordenCompraActualizar.Fecha.Year, ordenCompraActualizar.Fecha.Month, ordenCompraActualizar.Fecha.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            var estadoProcesada = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Procesada);
                            ordenCompraActualizar.IdEstado = estadoProcesada.IdEstado;

                            foreach (var item in ordenCompraActualizar.OrdenCompraDetalles)
                            {
                                var inventarioArticulos = await db.InventarioArticulos.FirstOrDefaultAsync(c => c.IdArticulo == item.MaestroArticuloSucursal.IdArticulo && c.IdBodega == ordenCompraActualizar.IdBodega);
                                db.InventarioArticulos.Add(new InventarioArticulos
                                {
                                    IdArticulo = item.MaestroArticuloSucursal.IdArticulo,
                                    IdBodega = ordenCompraActualizar.IdBodega,
                                    Cantidad = inventarioArticulos == null ? item.Cantidad : (inventarioArticulos.Cantidad + item.Cantidad),
                                    Fecha = fechaOrdenCompra
                                });
                            }
                            await db.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarOrdenCompra/{id}")]
        public async Task<Response> PutEditarOrdenCompra([FromRoute] int id, [FromBody] OrdenCompra ordenCompra)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var ordenCompraActualizar = await db.OrdenCompra.Include(c=> c.Factura).FirstOrDefaultAsync(c => c.IdOrdenCompra == id);
                    if (ordenCompraActualizar != null)
                    {
                        var estadoEnTramite = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.EnTramite);
                        var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();

                        ordenCompraActualizar.IdProveedor = ordenCompra.IdProveedor;
                        ordenCompraActualizar.IdMotivoRecepcionArticulos = ordenCompra.IdMotivoRecepcionArticulos;
                        ordenCompraActualizar.IdBodega = ordenCompra.IdBodega;
                        ordenCompraActualizar.Codigo = ordenCompra.Codigo;
                        ordenCompraActualizar.Fecha = ordenCompra.Fecha;
                        ordenCompraActualizar.IdEstado = estadoEnTramite.IdEstado;
                        ordenCompraActualizar.Factura.NumeroFactura = ordenCompra.Factura.NumeroFactura;
                        ordenCompraActualizar.Factura.FechaFactura = ordenCompra.Factura.FechaFactura;
                        ordenCompraActualizar.IdEmpleadoResponsable = (int)claimTransfer.IdEmpleado;
                        ordenCompraActualizar.IdEmpleadoDevolucion = ordenCompra.IdEmpleadoDevolucion;
                        await db.SaveChangesAsync();
                    }
                    var listaExcept = await db.OrdenCompraDetalles.Where(c => c.IdOrdenCompra == ordenCompra.IdOrdenCompra).Select(c => c.IdMaestroArticuloSucursal).Except(ordenCompra.OrdenCompraDetalles.Select(c => c.IdMaestroArticuloSucursal)).ToListAsync();
                    foreach (var item in listaExcept)
                    {
                        var ordenCompraDetalles = await db.OrdenCompraDetalles.FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item && c.IdOrdenCompra == ordenCompra.IdOrdenCompra);
                        db.OrdenCompraDetalles.Remove(ordenCompraDetalles);
                        await db.SaveChangesAsync();
                    }
                    foreach (var item in ordenCompra.OrdenCompraDetalles)
                    {
                        var ordenCompraDetalles = await db.OrdenCompraDetalles.FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdOrdenCompra == item.IdOrdenCompra);
                        if (ordenCompraDetalles == null)
                        {
                            db.OrdenCompraDetalles.Add(new OrdenCompraDetalles
                            {
                                IdOrdenCompra = item.IdOrdenCompra,
                                IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                                ValorUnitario = item.ValorUnitario,
                                Cantidad = item.Cantidad
                            });
                        }
                        else
                        {
                            ordenCompraDetalles.ValorUnitario = item.ValorUnitario;
                            ordenCompraDetalles.Cantidad = item.Cantidad;
                            db.OrdenCompraDetalles.Update(ordenCompraDetalles);
                        }
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = ordenCompra };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpPut("EditarRequerimientoArticulos/{id}")]
        public async Task<Response> PutEditarRequerimientoArticulos([FromRoute] int id, [FromBody] RequerimientoArticulos requerimientoArticulos)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var requerimientoArticulosActualizar = await db.RequerimientoArticulos.Include(c=> c.Estado).FirstOrDefaultAsync(c => c.IdRequerimientoArticulos == id);
                    if (requerimientoArticulosActualizar != null)
                    {
                        var estadoSolicitado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Solicitado);
                        var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();

                        requerimientoArticulosActualizar.FechaSolicitud = DateTime.Now;
                        requerimientoArticulosActualizar.FechaAprobadoDenegado = null;
                        requerimientoArticulosActualizar.Observaciones = requerimientoArticulos.Observaciones;
                        requerimientoArticulosActualizar.IdEstado = estadoSolicitado.IdEstado;
                        await db.SaveChangesAsync();
                    }
                    var listaExcept = await db.RequerimientosArticulosDetalles.Where(c => c.IdRequerimientosArticulos == requerimientoArticulos.IdRequerimientoArticulos).Select(c => c.IdMaestroArticuloSucursal).Except(requerimientoArticulos.RequerimientosArticulosDetalles.Select(c => c.IdMaestroArticuloSucursal)).ToListAsync();
                    foreach (var item in listaExcept)
                    {
                        var requerimientoArticulosDetalles = await db.RequerimientosArticulosDetalles.FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item && c.IdRequerimientosArticulos == requerimientoArticulos.IdRequerimientoArticulos);
                        db.RequerimientosArticulosDetalles.Remove(requerimientoArticulosDetalles);
                        await db.SaveChangesAsync();
                    }
                    foreach (var item in requerimientoArticulos.RequerimientosArticulosDetalles)
                    {
                        var requerimientoArticulosDetalles = await db.RequerimientosArticulosDetalles.FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdRequerimientosArticulos == item.IdRequerimientosArticulos);
                        if (requerimientoArticulosDetalles == null)
                        {
                            db.RequerimientosArticulosDetalles.Add(new RequerimientosArticulosDetalles
                            {
                                IdRequerimientosArticulos = item.IdRequerimientosArticulos,
                                IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                                CantidadSolicitada = item.CantidadSolicitada,
                                CantidadAprobada = 0
                            });
                        }
                        else
                        {
                            requerimientoArticulosDetalles.CantidadSolicitada = item.CantidadSolicitada;
                            requerimientoArticulosDetalles.CantidadAprobada = 0;
                            db.RequerimientosArticulosDetalles.Update(requerimientoArticulosDetalles);
                        }
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = requerimientoArticulos };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpDelete]
        [Route("EliminarOrdenCompra/{id}")]
        public async Task<Response> DeleteOrdenCompra([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.OrdenCompra.Include(c => c.Proveedor).Include(c => c.Factura).SingleOrDefaultAsync(m => m.IdOrdenCompra == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdFacturaActivoFijo == respuesta.IdFacturaActivoFijo).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                foreach (var item in listaDocumentosActivoFijo)
                    uploadFileService.DeleteFile(item.Url);

                db.FacturaActivoFijo.Remove(await db.FacturaActivoFijo.FirstOrDefaultAsync(c => c.IdFacturaActivoFijo == respuesta.IdFacturaActivoFijo));
                db.OrdenCompraDetalles.RemoveRange(await db.OrdenCompraDetalles.Where(c => c.IdOrdenCompra == id).ToListAsync());
                db.OrdenCompra.Remove(respuesta);

                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpDelete]
        [Route("EliminarRequerimientoArticulos/{id}")]
        public async Task<Response> DeteleRequerimientoArticulos([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.RequerimientoArticulos.SingleOrDefaultAsync(m => m.IdRequerimientoArticulos == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.RequerimientosArticulosDetalles.RemoveRange(await db.RequerimientosArticulosDetalles.Where(c => c.IdRequerimientosArticulos == id).ToListAsync());
                db.RequerimientoArticulos.Remove(respuesta);

                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
    }
}