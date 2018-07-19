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
                var ordenCompra = ObtenerOrdenCompraFinal(await ObtenerOrdenCompra(id));
                return new Response { IsSuccess = ordenCompra != null, Message = ordenCompra != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = ordenCompra };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        private async Task<OrdenCompra> ObtenerOrdenCompra(int id)
        {
            try
            {
                var ordenCompra = await db.OrdenCompra
                    .Include(c => c.Factura)
                    .Include(c => c.Estado)
                    .Include(c => c.EmpleadoResponsable).ThenInclude(c => c.Persona)
                    .Include(c => c.Bodega).ThenInclude(c => c.Sucursal)
                    .Include(c => c.MotivoRecepcionArticulos)
                    .FirstOrDefaultAsync(c => c.IdOrdenCompra == id);

                if (ordenCompra.IdProveedor != null)
                    ordenCompra.Proveedor = await db.Proveedor.FirstOrDefaultAsync(c => c.IdProveedor == ordenCompra.IdProveedor);

                if (ordenCompra.IdEmpleadoDevolucion != null)
                    ordenCompra.EmpleadoDevolucion = await db.Empleado.Include(c => c.Persona)
                        .Include(c => c.Dependencia).ThenInclude(c => c.Sucursal)
                        .FirstOrDefaultAsync(c => c.IdEmpleado == ordenCompra.IdEmpleadoDevolucion);

                var listadoOrdenCompraDetalles = await db.OrdenCompraDetalles
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c => c.UnidadMedida)
                    .Where(c => c.IdOrdenCompra == ordenCompra.IdOrdenCompra).ToListAsync();

                return ordenCompra;
            }
            catch (Exception)
            {
                return null;
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
                        var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdBodega == bodega.IdBodega);
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

        [HttpGet]
        [Route("ObtenerAjusteInventarioArticulos/{id}")]
        public async Task<Response> GetAjusteInventarioArticulos([FromRoute] int id)
        {
            try
            {
                var ajusteInventarioArticulos = await db.AjusteInventarioArticulos.Include(c => c.EmpleadoAutoriza).ThenInclude(c => c.Persona).Include(c => c.Bodega).ThenInclude(c => c.Sucursal).FirstOrDefaultAsync(c => c.IdAjusteInventario == id);
                ajusteInventarioArticulos.InventarioArticulos = await db.InventarioArticulos.Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).Include(c=> c.Bodega).ThenInclude(c=> c.Sucursal).Where(c => c.Fecha == ajusteInventarioArticulos.Fecha).ToListAsync();
                return new Response { IsSuccess = ajusteInventarioArticulos != null, Message = ajusteInventarioArticulos != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = ajusteInventarioArticulos };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpGet]
        [Route("ObtenerAjusteInventarioArticulosAnterior/{id}")]
        public async Task<Response> GetObtenerAjusteInventarioArticulosAnterior([FromRoute] int id)
        {
            try
            {
                var listadoAjustesInventarioArticulos = await db.AjusteInventarioArticulos.OrderBy(c=> c.Fecha).ToListAsync();
                int posAjusteInventarioActual = listadoAjustesInventarioArticulos.FindIndex(c => c.IdAjusteInventario == id);
                var ajusteInventarioAnterior = listadoAjustesInventarioArticulos.ElementAtOrDefault(posAjusteInventarioActual - 1);
                var response = await GetAjusteInventarioArticulos(ajusteInventarioAnterior?.IdAjusteInventario ?? -1);
                if (response.IsSuccess)
                    ajusteInventarioAnterior = response.Resultado as AjusteInventarioArticulos;
                return new Response { IsSuccess = ajusteInventarioAnterior != null, Message = ajusteInventarioAnterior != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = ajusteInventarioAnterior };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ListadoInventarioArticulosAnterior")]
        public async Task<List<InventarioArticulos>> PostListadoInventarioArticulosAnterior([FromBody] DateTime fechaAjusteInventario)
        {
            try
            {
                var listaInventarioArticulos = new List<InventarioArticulos>();
                var listadoInventarioArticulosGroupFecha = await db.InventarioArticulos.OrderBy(c=> c.Fecha).GroupBy(c=> c.Fecha).ToListAsync();
                int posInventarioActual = listadoInventarioArticulosGroupFecha.FindIndex(c => c.Key == fechaAjusteInventario);
                var inventarioAnterior = listadoInventarioArticulosGroupFecha.ElementAtOrDefault(posInventarioActual - 1);
                if (inventarioAnterior != null)
                    listaInventarioArticulos.AddRange(inventarioAnterior);
                return listaInventarioArticulos;
            }
            catch (Exception)
            {
                return new List<InventarioArticulos>();
            }
        }

        [HttpGet]
        [Route("ListadoAjusteInventarioArticulos")]
        public async Task<List<AjusteInventarioArticulos>> GetListadoAjusteInventarioArticulos()
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoAjusteInventarioArticulos = new List<AjusteInventarioArticulos>();
                var idsAjustesInventario = claimTransfer.IsAdminNacionalProveeduria ? await db.AjusteInventarioArticulos.Select(c => c.IdAjusteInventario).ToListAsync() : await db.AjusteInventarioArticulos.Where(c => c.Bodega.IdSucursal == claimTransfer.IdSucursal).Select(c => c.IdAjusteInventario).ToListAsync();
                foreach (var item in idsAjustesInventario)
                {
                    var response = await GetAjusteInventarioArticulos(item);
                    if (response.IsSuccess)
                        listadoAjusteInventarioArticulos.Add(response.Resultado as AjusteInventarioArticulos);
                }
                return listadoAjusteInventarioArticulos;
            }
            catch (Exception)
            {
                return new List<AjusteInventarioArticulos>();
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
        [Route("ListadoInventarioPorBodega")]
        public async Task<List<InventarioArticulos>> PostListadoInventarioPorBodega([FromBody] int idBodega)
        {
            try
            {
                var listadoInventarioArticulos = new List<InventarioArticulos>();
                var lista = await db.InventarioArticulos.Include(c=> c.MaestroArticuloSucursal).ThenInclude(c=> c.Articulo).Where(c => c.IdBodega == idBodega).GroupBy(c=> c.IdMaestroArticuloSucursal).ToListAsync();
                foreach (var item in lista)
                    listadoInventarioArticulos.Add(item.OrderByDescending(c => c.Fecha).FirstOrDefault());
                return listadoInventarioArticulos;
            }
            catch (Exception)
            {
                return new List<InventarioArticulos>();
            }
        }

        [HttpPost]
        [Route("ListadoInventarioPorBodegaFecha")]
        public async Task<List<InventarioArticulos>> PostListadoInventarioPorBodegaFecha([FromBody] IdBodegaFecha idBodegaFecha)
        {
            try
            {
                var listadoInventarioArticulos = new List<InventarioArticulos>();
                var fechaAjusteInventario = new DateTime(idBodegaFecha.Fecha.Year, idBodegaFecha.Fecha.Month, idBodegaFecha.Fecha.Day, 23, 59, 59);
                var lista = await db.InventarioArticulos.Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).Where(c => c.IdBodega == idBodegaFecha.IdBodega && c.Fecha <= fechaAjusteInventario).GroupBy(c => c.IdMaestroArticuloSucursal).ToListAsync();
                foreach (var item in lista)
                    listadoInventarioArticulos.Add(item.OrderByDescending(c => c.Fecha).FirstOrDefault());
                return listadoInventarioArticulos;
            }
            catch (Exception)
            {
                return new List<InventarioArticulos>();
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
                            var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdBodega == bodega.IdBodega);
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
        [Route("InsertarAjusteInventarioArticulos")]
        public async Task<Response> PostInsertarAjusteInventarioArticulos([FromBody] AjusteInventarioArticulos ajusteInventarioArticulos)
        {
            try
            {
                var nuevoAjusteInventarioArticulos = new AjusteInventarioArticulos();
                using (var transaction = db.Database.BeginTransaction())
                {
                    var fechaAjusteInventario = new DateTime(ajusteInventarioArticulos.Fecha.Year, ajusteInventarioArticulos.Fecha.Month, ajusteInventarioArticulos.Fecha.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    nuevoAjusteInventarioArticulos.Motivo = ajusteInventarioArticulos.Motivo;
                    nuevoAjusteInventarioArticulos.IdEmpleadoAutoriza = ajusteInventarioArticulos.IdEmpleadoAutoriza;
                    nuevoAjusteInventarioArticulos.Fecha = fechaAjusteInventario;
                    nuevoAjusteInventarioArticulos.IdBodega = ajusteInventarioArticulos.IdBodega;
                    db.AjusteInventarioArticulos.Add(nuevoAjusteInventarioArticulos);
                    await db.SaveChangesAsync();
                    
                    foreach (var item in ajusteInventarioArticulos.InventarioArticulos)
                    {
                        db.InventarioArticulos.Add(new InventarioArticulos
                        {
                            IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                            IdBodega = ajusteInventarioArticulos.IdBodega,
                            Cantidad = item.Cantidad,
                            Fecha = fechaAjusteInventario
                        });
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevoAjusteInventarioArticulos };
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
                        var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdBodega == salidaArticulos.RequerimientoArticulos.FuncionarioSolicitante.Dependencia.IdBodega);
                        if (inventarioArticulos != null)
                        {
                            int diferencia = inventarioArticulos.Cantidad < item.CantidadSolicitada ? (item.CantidadSolicitada - inventarioArticulos.Cantidad) : (inventarioArticulos.Cantidad - item.CantidadSolicitada);
                            db.InventarioArticulos.Add(new InventarioArticulos
                            {
                                IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
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
                    var ordenCompraActualizar = await ObtenerOrdenCompra(id);
                    if (ordenCompraActualizar != null)
                    {
                        var fechaOrdenCompra = new DateTime(ordenCompraActualizar.Fecha.Year, ordenCompraActualizar.Fecha.Month, ordenCompraActualizar.Fecha.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        var estadoProcesada = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Procesada);
                        ordenCompraActualizar.IdEstado = estadoProcesada.IdEstado;

                        foreach (var item in ordenCompraActualizar.OrdenCompraDetalles)
                        {
                            var inventarioArticulos = await db.InventarioArticulos.OrderByDescending(c => c.Fecha).FirstOrDefaultAsync(c => c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal && c.IdBodega == ordenCompraActualizar.IdBodega);
                            db.InventarioArticulos.Add(new InventarioArticulos
                            {
                                IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                                IdBodega = ordenCompraActualizar.IdBodega,
                                Cantidad = inventarioArticulos == null ? item.Cantidad : (inventarioArticulos.Cantidad + item.Cantidad),
                                Fecha = fechaOrdenCompra
                            });
                        }
                        await db.SaveChangesAsync();
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

        #region Objetos de Clonación Comunes
        private Empleado ObtenerEmpleadoFinal(Empleado empleado)
        {
            return empleado != null ? new Empleado
            {
                IdEmpleado = empleado.IdEmpleado,
                IdPersona = empleado.IdPersona,
                Persona = empleado.Persona != null ? new Persona
                {
                    IdPersona = empleado.Persona.IdPersona,
                    Nombres = empleado.Persona.Nombres,
                    Apellidos = empleado.Persona.Apellidos
                } : null
            } : null;
        }
        private Proveedor ObtenerProveedorFinal(Proveedor proveedor)
        {
            return proveedor != null ? new Proveedor
            {
                IdProveedor = proveedor.IdProveedor,
                Nombre = proveedor.Nombre,
                Apellidos = proveedor.Apellidos,
                RazonSocial = proveedor.RazonSocial,
                Direccion = proveedor.Direccion,
                Identificacion = proveedor.Identificacion
            } : null;
        }
        private MotivoRecepcionArticulos ObtenerMotivoRecepcionArticulosFinal(MotivoRecepcionArticulos motivoRecepcionArticulos)
        {
            return motivoRecepcionArticulos != null ? new MotivoRecepcionArticulos
            {
                IdMotivoRecepcionArticulos = motivoRecepcionArticulos.IdMotivoRecepcionArticulos,
                Descripcion = motivoRecepcionArticulos.Descripcion
            } : null;
        }
        private Estado ObtenerEstadoFinal(Estado estado)
        {
            return estado != null ? new Estado
            {
                IdEstado = estado.IdEstado,
                Nombre = estado.Nombre
            } : null;
        }
        private FacturaActivoFijo ObtenerFacturaActivoFijoFinal(FacturaActivoFijo facturaActivoFijo)
        {
            return facturaActivoFijo != null ? new FacturaActivoFijo
            {
                IdFacturaActivoFijo = facturaActivoFijo.IdFacturaActivoFijo,
                FechaFactura = facturaActivoFijo.FechaFactura,
                NumeroFactura = facturaActivoFijo.NumeroFactura
            } : null;
        }
        private Bodega ObtenerBodegaFinal(Bodega bodega)
        {
            return bodega != null ? new Bodega
            {
                IdBodega = bodega.IdBodega,
                Nombre = bodega.Nombre,
                IdSucursal = bodega.IdSucursal,
                Sucursal = ObtenerSucursalFinal(bodega?.Sucursal)
            } : null;
        }
        private Sucursal ObtenerSucursalFinal(Sucursal sucursal)
        {
            return sucursal != null ? new Sucursal
            {
                IdSucursal = sucursal.IdSucursal,
                Nombre = sucursal.Nombre
            } : null;
        }
        private TipoArticulo ObtenerTipoArticuloFinal(TipoArticulo tipoArticulo)
        {
            return tipoArticulo != null ? new TipoArticulo
            {
                IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                Nombre = tipoArticulo.Nombre
            } : null;
        }
        private ClaseArticulo ObtenerClaseArticuloFinal(ClaseArticulo claseArticulo)
        {
            return claseArticulo != null ? new ClaseArticulo
            {
                IdClaseArticulo = claseArticulo.IdClaseArticulo,
                Nombre = claseArticulo.Nombre,
                IdTipoArticulo = claseArticulo.IdTipoArticulo,
                TipoArticulo = ObtenerTipoArticuloFinal(claseArticulo?.TipoArticulo)
            } : null;
        }
        private SubClaseArticulo ObtenerSubClaseArticuloFinal(SubClaseArticulo subClaseArticulo)
        {
            return subClaseArticulo != null ? new SubClaseArticulo
            {
                IdSubClaseArticulo = subClaseArticulo.IdSubClaseArticulo,
                Nombre = subClaseArticulo.Nombre,
                IdClaseArticulo = subClaseArticulo.IdClaseArticulo,
                ClaseArticulo = ObtenerClaseArticuloFinal(subClaseArticulo?.ClaseArticulo)
            } : null;
        }
        private UnidadMedida ObtenerUnidadMedidaFinal(UnidadMedida unidadMedida)
        {
            return unidadMedida != null ? new UnidadMedida
            {
                IdUnidadMedida = unidadMedida.IdUnidadMedida,
                Nombre = unidadMedida.Nombre
            } : null;
        }
        private Marca ObtenerMarcaFinal(Marca marca)
        {
            return marca != null ? new Marca
            {
                IdMarca = marca.IdMarca,
                Nombre = marca.Nombre
            } : null;
        }
        private Modelo ObtenerModeloFinal(Modelo modelo)
        {
            return modelo != null ? new Modelo
            {
                IdModelo = modelo.IdModelo,
                Nombre = modelo.Nombre,
                IdMarca = modelo.IdMarca,
                Marca = ObtenerMarcaFinal(modelo?.Marca)
            } : null;
        }
        private Articulo ObtenerArticuloFinal(Articulo articulo)
        {
            return articulo != null ? new Articulo
            {
                IdArticulo = articulo.IdArticulo,
                IdSubClaseArticulo = articulo.IdSubClaseArticulo,
                IdUnidadMedida = articulo.IdUnidadMedida,
                IdModelo = articulo.IdModelo,
                Nombre = articulo.Nombre,
                SubClaseArticulo = ObtenerSubClaseArticuloFinal(articulo?.SubClaseArticulo),
                UnidadMedida = ObtenerUnidadMedidaFinal(articulo?.UnidadMedida),
                Modelo = ObtenerModeloFinal(articulo?.Modelo)
            } : null;
        }
        private MaestroArticuloSucursal ObtenerMaestroArticuloSucursalFinal(MaestroArticuloSucursal maestroArticuloSucursal)
        {
            return maestroArticuloSucursal != null ? new MaestroArticuloSucursal
            {
                IdMaestroArticuloSucursal = maestroArticuloSucursal.IdMaestroArticuloSucursal,
                IdSucursal = maestroArticuloSucursal.IdSucursal,
                IdArticulo = maestroArticuloSucursal.IdArticulo,
                Minimo = maestroArticuloSucursal.Minimo,
                Maximo = maestroArticuloSucursal.Maximo,
                CodigoArticulo = maestroArticuloSucursal.CodigoArticulo,
                Habilitado = maestroArticuloSucursal.Habilitado,
                FechaSinExistencia = maestroArticuloSucursal.FechaSinExistencia,
                Sucursal = ObtenerSucursalFinal(maestroArticuloSucursal?.Sucursal),
                Articulo = ObtenerArticuloFinal(maestroArticuloSucursal?.Articulo)
            } : null;
        }
        private OrdenCompra ObtenerOrdenCompraFinal(OrdenCompra ordenCompra)
        {
            var nuevaOrdenCompra = ordenCompra != null ? new OrdenCompra
            {
                IdOrdenCompra = ordenCompra.IdOrdenCompra,
                IdMotivoRecepcionArticulos = ordenCompra.IdMotivoRecepcionArticulos,
                Fecha = ordenCompra.Fecha,
                IdEstado = ordenCompra.IdEstado,
                IdFacturaActivoFijo = ordenCompra.IdFacturaActivoFijo,
                IdEmpleadoResponsable = ordenCompra.IdEmpleadoResponsable,
                IdEmpleadoDevolucion = ordenCompra.IdEmpleadoDevolucion,
                Codigo = ordenCompra.Codigo,
                IdBodega = ordenCompra.IdBodega,
                IdProveedor = ordenCompra.IdProveedor,
                MotivoRecepcionArticulos = ObtenerMotivoRecepcionArticulosFinal(ordenCompra?.MotivoRecepcionArticulos),
                Estado = ObtenerEstadoFinal(ordenCompra?.Estado),
                Factura = ObtenerFacturaActivoFijoFinal(ordenCompra?.Factura),
                EmpleadoResponsable = ObtenerEmpleadoFinal(ordenCompra?.EmpleadoResponsable),
                EmpleadoDevolucion = ObtenerEmpleadoFinal(ordenCompra?.EmpleadoDevolucion),
                Bodega = ObtenerBodegaFinal(ordenCompra?.Bodega),
                Proveedor = ObtenerProveedorFinal(ordenCompra?.Proveedor),
                OrdenCompraDetalles = ordenCompra.OrdenCompraDetalles
            } : null;

            if (nuevaOrdenCompra != null)
            {
                foreach (var item in ordenCompra.OrdenCompraDetalles)
                {
                    item.OrdenCompra = null;
                    item.MaestroArticuloSucursal = ObtenerMaestroArticuloSucursalFinal(item?.MaestroArticuloSucursal);
                }
            }
            return nuevaOrdenCompra;
        }
        private OrdenCompraDetalles ObtenerOrdenCompraDetallesFinal(OrdenCompraDetalles ordenCompraDetalles)
        {
            return ordenCompraDetalles != null ? new OrdenCompraDetalles
            {
                IdOrdenCompra = ordenCompraDetalles.IdOrdenCompra,
                IdMaestroArticuloSucursal = ordenCompraDetalles.IdMaestroArticuloSucursal,
                Cantidad = ordenCompraDetalles.Cantidad,
                ValorUnitario = ordenCompraDetalles.ValorUnitario,
                MaestroArticuloSucursal = ObtenerMaestroArticuloSucursalFinal(ordenCompraDetalles?.MaestroArticuloSucursal)
            } : null;
        }
        #endregion
    }
}