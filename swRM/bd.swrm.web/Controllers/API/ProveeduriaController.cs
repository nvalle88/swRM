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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using bd.swrm.entidades.Comparadores;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Proveeduria")]
    public class ProveeduriaController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;
        private readonly IClaimsTransfer claimsTransfer;
        private readonly IClonacion clonacionService;
        private readonly IExcelMethods excelMethodsService;

        public ProveeduriaController(SwRMDbContext db, IUploadFileService uploadFileService, IClaimsTransfer claimsTransfer, IHttpContextAccessor httpContextAccessor, IClonacion clonacionService, IExcelMethods excelMethodsService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
            this.claimsTransfer = claimsTransfer;
            this.clonacionService = clonacionService;
            this.excelMethodsService = excelMethodsService;
        }

        [HttpGet]
        [Route("ObtenerOrdenCompra/{id}")]
        public async Task<Response> GetOrdenCompra([FromRoute] int id)
        {
            try
            {
                var ordenCompra = clonacionService.ClonarOrdenCompra(await ObtenerOrdenCompra(id));
                ordenCompra.Factura.DocumentoActivoFijo = clonacionService.ClonarListadoDocumentoActivoFijo(await db.DocumentoActivoFijo.Where(c => c.IdFacturaActivoFijo == ordenCompra.IdFacturaActivoFijo).ToListAsync());
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

                ordenCompra.OrdenCompraDetalles = await db.OrdenCompraDetalles
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c => c.UnidadMedida)
                    .Include(c=> c.MaestroArticuloSucursal).ThenInclude(c=> c.Articulo).ThenInclude(c=> c.SubClaseArticulo).ThenInclude(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo)
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c=> c.Modelo).ThenInclude(c=> c.Marca)
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

        [HttpGet]
        [Route("ListadoOrdenCompra")]
        public async Task<List<OrdenCompra>> GetListadoOrdenCompra()
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoOrdenesCompra = new List<OrdenCompra>();
                var idsOrdenesCompra = claimTransfer.IsAdminNacionalProveeduria ? await db.OrdenCompra.Select(c => c.IdOrdenCompra).ToListAsync() : await db.OrdenCompra.Include(c => c.EmpleadoResponsable).Where(c => c.EmpleadoResponsable.IdDependencia == claimTransfer.IdDependencia).Select(c => c.IdOrdenCompra).ToListAsync();
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
        [Route("ListadoOrdenCompraPorEstado")]
        public async Task<List<OrdenCompra>> PostListadoOrdenCompraPorEstado([FromBody] List<string> estados)
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoOrdenesCompra = new List<OrdenCompra>();
                var idsOrdenesCompra = claimTransfer.IsAdminNacionalProveeduria ? await db.OrdenCompra.Include(c=> c.Estado).Where(c => estados.Contains(c.Estado.Nombre)).Select(c=> c.IdOrdenCompra).ToListAsync() : await db.OrdenCompra.Include(c => c.Estado).Include(c=> c.EmpleadoResponsable).Where(c => estados.Contains(c.Estado.Nombre) && c.EmpleadoResponsable.IdDependencia == claimTransfer.IdDependencia).Select(c => c.IdOrdenCompra).ToListAsync();
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
        [Route("ListadoOrdenCompraPorEstadoRangoFecha")]
        public async Task<List<OrdenCompra>> PostListadoOrdenCompraPorRangoFecha([FromBody] RangoFechaEstadoTransfer rangoFechaEstadoTransfer)
        {
            try
            {
                var listadoOrdenesCompra = await PostListadoOrdenCompraPorEstado(rangoFechaEstadoTransfer.Estados);
                return listadoOrdenesCompra.Where(c => c.Fecha >= rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial && c.Fecha <= rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal).ToList();
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
        public async Task<List<RequerimientoArticulos>> PostListadoRequerimientoArticulosPorEstado([FromBody] List<string> estados)
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var listadoRequerimientoArticulos = new List<RequerimientoArticulos>();
                var idsRequerimientoArticulos = claimTransfer.IsAdminNacionalProveeduria ? await db.RequerimientoArticulos
                    .Include(c => c.Estado)
                    .Where(c => estados.Contains(c.Estado.Nombre))
                    .Select(c => c.IdRequerimientoArticulos).ToListAsync() :
                    await db.RequerimientoArticulos
                    .Include(c => c.Estado)
                    .Include(c => c.FuncionarioSolicitante)
                    .Where(c => estados.Contains(c.Estado.Nombre) && c.FuncionarioSolicitante.IdDependencia == claimTransfer.IdDependencia)
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
        [Route("ListadoRequerimientoArticulosPorEstadoRangoFecha")]
        public async Task<List<RequerimientoArticulos>> PostListadoRequerimientoArticulosPorEstadoRangoFecha([FromBody] RangoFechaEstadoTransfer rangoFechaEstadoTransfer)
        {
            try
            {
                var listadoRequerimientos = await PostListadoRequerimientoArticulosPorEstado(rangoFechaEstadoTransfer.Estados);
                return listadoRequerimientos.Where(c => c.FechaAprobadoDenegado >= rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial && c.FechaAprobadoDenegado <= rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal).ToList();
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
                        item.ValorActual = await new MaestroArticuloSucursalController(db).ObtenerValorActual(item.IdMaestroArticuloSucursal);
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
        [Route("ListadoProveedoresPorArticulo")]
        public async Task<List<ArticuloProveedoresTransfer>> PostProveedoresPorArticulo([FromBody] int idArticulo)
        {
            try
            {
                var lista = new List<ArticuloProveedoresTransfer>();
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var myComparer = new MyComparer<Proveedor>((x, y) => { return x.IdProveedor == y.IdProveedor; }, (obj) => { return obj.IdProveedor.GetHashCode(); });

                var ordenesCompraDetallesBD = ObtenerOrdenesCompra();
                var ordenesCompraDetalles = idArticulo == -1 ? ordenesCompraDetallesBD : ordenesCompraDetallesBD.Where(c => c.MaestroArticuloSucursal.IdArticulo == idArticulo);
                var groupOrdenesCompraDetalles = ordenesCompraDetalles.GroupBy(c => c.MaestroArticuloSucursal.Articulo);

                foreach (var item in groupOrdenesCompraDetalles)
                {
                    var listadoProveedores = claimTransfer.IsAdminNacionalProveeduria ? item.Select(c => c.OrdenCompra.Proveedor).Distinct(myComparer).ToList() : item.Where(c=> c.OrdenCompra.EmpleadoResponsable.Dependencia.IdSucursal == claimTransfer.IdSucursal).Select(c => c.OrdenCompra.Proveedor).Distinct(myComparer).ToList();
                    lista.Add(new ArticuloProveedoresTransfer {
                        Articulo = clonacionService.ClonarArticulo(item.Key),
                        ListadoProveedores = listadoProveedores.Select(c=> clonacionService.ClonarProveedor(c)).ToList()
                    });
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ArticuloProveedoresTransfer>();
            }
        }

        private IQueryable<OrdenCompraDetalles> ObtenerOrdenesCompra()
        {
            var ordenesCompraDetalles = db.OrdenCompraDetalles
                    .Include(c => c.OrdenCompra).ThenInclude(c => c.Proveedor).ThenInclude(c => c.LineaServicio)
                    .Include(c => c.OrdenCompra).ThenInclude(c => c.EmpleadoResponsable)
                    .Include(c=> c.MaestroArticuloSucursal).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c=> c.SubClaseArticulo).ThenInclude(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo)
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c=> c.UnidadMedida)
                    .Include(c => c.MaestroArticuloSucursal).ThenInclude(c => c.Articulo).ThenInclude(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .Where(c => c.OrdenCompra.EmpleadoResponsable.IdDependencia != null);

            foreach (var item in ordenesCompraDetalles)
                item.OrdenCompra.EmpleadoResponsable.Dependencia = db.Dependencia.Include(c => c.Sucursal).FirstOrDefault(c => c.IdDependencia == item.OrdenCompra.EmpleadoResponsable.IdDependencia);

            return ordenesCompraDetalles;
        }

        [HttpPost]
        [Route("ListadoArticulosPorProveedor")]
        public async Task<List<MaestroArticuloSucursalSeleccionado>> PostArticulosPorProveedor([FromBody] int idProveedor)
        {
            try
            {
                var lista = new List<MaestroArticuloSucursalSeleccionado>();
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var myComparer = new MyComparer<MaestroArticuloSucursal>((x, y) => { return x.IdMaestroArticuloSucursal == y.IdMaestroArticuloSucursal; }, (obj) => { return obj.IdMaestroArticuloSucursal.GetHashCode(); });

                var ordenesCompraDetallesBD = ObtenerOrdenesCompra();
                var ordenesCompraDetalles = idProveedor == -1 ? ordenesCompraDetallesBD : ordenesCompraDetallesBD.Where(c => c.OrdenCompra.IdProveedor == idProveedor);
                var groupOrdenesCompraDetalles = ordenesCompraDetalles.GroupBy(c => c.OrdenCompra.Proveedor);

                foreach (var item in groupOrdenesCompraDetalles)
                {
                    var listadoMaestrosArticulosArticulos = claimTransfer.IsAdminNacionalProveeduria ? item.Select(c => c.MaestroArticuloSucursal).Distinct(myComparer).ToList() : item.Where(c => c.OrdenCompra.EmpleadoResponsable.Dependencia.IdSucursal == claimTransfer.IdSucursal).Select(c => c.MaestroArticuloSucursal).Distinct(myComparer).ToList();
                    foreach (var mas in listadoMaestrosArticulosArticulos)
                    {
                        var maestroArticuloSucursal = clonacionService.ClonarMaestroArticuloSucursal(mas);
                        maestroArticuloSucursal.ValorActual = await new MaestroArticuloSucursalController(db).ObtenerValorActual(mas.IdMaestroArticuloSucursal);

                        lista.Add(new MaestroArticuloSucursalSeleccionado
                        {
                            MaestroArticuloSucursal = maestroArticuloSucursal,
                            Proveedor = clonacionService.ClonarProveedor(item.Key)
                        });
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MaestroArticuloSucursalSeleccionado>();
            }
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
                    nuevoRequerimientoArticulos.CodigoPedido = requerimientoArticulos.CodigoPedido;
                    db.RequerimientoArticulos.Add(nuevoRequerimientoArticulos);
                    await db.SaveChangesAsync();

                    foreach (var item in requerimientoArticulos.RequerimientosArticulosDetalles)
                    {
                        db.RequerimientosArticulosDetalles.Add(new RequerimientosArticulosDetalles
                        {
                            IdRequerimientosArticulos = nuevoRequerimientoArticulos.IdRequerimientoArticulos,
                            IdMaestroArticuloSucursal = item.IdMaestroArticuloSucursal,
                            CantidadSolicitada = item.CantidadSolicitada,
                            CantidadAprobada = 0,
                            CantidadEntregada = 0,
                            ValorActual = await new MaestroArticuloSucursalController(db).ObtenerValorActual(item.IdMaestroArticuloSucursal)
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
                            int diferencia = inventarioArticulos.Cantidad < item.CantidadEntregada ? (item.CantidadEntregada - inventarioArticulos.Cantidad) : (inventarioArticulos.Cantidad - item.CantidadEntregada);
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
                                requerimientoArticulosDetalles.CantidadAprobada = item.CantidadAprobada;
                                requerimientoArticulosDetalles.CantidadEntregada = item.CantidadEntregada;
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
                        requerimientoArticulosActualizar.CodigoPedido = requerimientoArticulos.CodigoPedido;
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
                                CantidadAprobada = 0,
                                CantidadEntregada = 0,
                                ValorActual = await new MaestroArticuloSucursalController(db).ObtenerValorActual(item.IdMaestroArticuloSucursal)
                            });
                        }
                        else
                        {
                            requerimientoArticulosDetalles.CantidadSolicitada = item.CantidadSolicitada;
                            requerimientoArticulosDetalles.CantidadAprobada = 0;
                            requerimientoArticulosDetalles.CantidadEntregada = 0;
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

        #region Exportar a Excel
        [HttpPost]
        [Route("ExcelOrdenCompra")]
        public async Task<byte[]> PostExcelOrdenCompra([FromBody] int idOrdenCompra)
        {
            try
            {
                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("COMPRAS");
                    await ExcelOrdenCompra(idOrdenCompra, ws, 1, 1);
                    AjustarPropiedadesExcelOrdenCompra(ws, 1, 1);
                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }
        private async Task<int> ExcelOrdenCompra(int idOrdenCompra, ExcelWorksheet ws, int fila, int columna)
        {
            var ordenCompra = await ObtenerOrdenCompra(idOrdenCompra);
            int filaOriginal = fila;
            int columnaOriginal = columna;

            var fontCalibri10 = excelMethodsService.CalibriFont(10);
            var fontCalibri11 = excelMethodsService.CalibriFont(11);

            var codigoAreaGeografica = "[CÓDIGO ÁREA GEOGRÁFICA]";
            var excelRange = excelMethodsService.Ajustar(ws, $"BANCO DE DESARROLLO DEL ECUADOR B.P. - {codigoAreaGeografica}", fila, columna, fila, columna + 6, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "REPORTE DE MOVIMIENTOS (COMPRAS)", fila, columna, fila, columna + 5, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "FECHA:", fila, columna + 5, font: fontCalibri11, isBold: true, isMerge: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Right);
            excelRange = excelMethodsService.Ajustar(ws, ordenCompra.Fecha.ToString("dd/MM/yyyy"), fila, columna + 6, font: fontCalibri11, isBold: true);

            excelRange = ws.Cells[filaOriginal, columnaOriginal, fila, columnaOriginal + 6];
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "TIPO DE MOVIMIENTO", fila, columna, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "FECHA DE PROCESO", fila, columna + 1, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "NOMBRE DEL ARTÍCULO", fila, columna + 2, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "NOMBRE DE LA BODEGA", fila, columna + 3, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "CANTIDAD / UNIDADES DE MEDIDA", fila, columna + 4, font: fontCalibri10, isWrapText: true, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "COSTO UNITARIO", fila, columna + 5, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "SUBTOTAL", fila, columna + 6, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange.Worksheet.Row(fila).Height = 40.50D;
            excelRange = ws.Cells[fila, columna, fila, columna + 6];
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila++;

            decimal total = 0;
            foreach (var item in ordenCompra.OrdenCompraDetalles)
            {
                excelRange = excelMethodsService.Ajustar(ws, "INGRESO DE COMPRA", fila, columna, font: fontCalibri11, isWrapText: true);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, ordenCompra.Fecha.ToString("dd/MM/yyyy"), fila, columna + 1, font: fontCalibri11);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, item.MaestroArticuloSucursal.Articulo.Nombre, fila, columna + 2, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, ordenCompra.Bodega.Nombre, fila, columna + 3, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, isWrapText: true);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, $"{item.Cantidad} {item.MaestroArticuloSucursal.Articulo.UnidadMedida.Nombre}", fila, columna + 4, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, item.ValorUnitario.ToString("C2").Replace("$", ""), fila, columna + 5, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                decimal subtotal = item.Cantidad * item.ValorUnitario;
                excelRange = excelMethodsService.Ajustar(ws, subtotal.ToString(), fila, columna + 6, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                total += subtotal;
                fila++;
            }

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna, font: fontCalibri11, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 1, font: fontCalibri11, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 2, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 3, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 4, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, ordenCompra.OrdenCompraDetalles.Sum(c => c.ValorUnitario).ToString(), fila, columna + 5, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, total.ToString(), fila, columna + 6, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange.Worksheet.Row(fila).Height = 25.50D;
            excelRange = ws.Cells[fila, columna, fila, columna + 6];
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila += 2;
            return fila;
        }
        private void AjustarPropiedadesExcelOrdenCompra(ExcelWorksheet ws, int fila, int columna)
        {
            ws.Cells[fila, columna].Worksheet.Column(columna).Width = 13.15D;
            ws.Cells[fila, columna + 1].Worksheet.Column(columna + 1).Width = 11.43D;
            ws.Cells[fila, columna + 2].Worksheet.Column(columna + 2).Width = 30.43D;
            ws.Cells[fila, columna + 3].Worksheet.Column(columna + 3).Width = 12.72D;
            ws.Cells[fila, columna + 4].Worksheet.Column(columna + 4).Width = 18.29D;
            ws.Cells[fila, columna + 5].Worksheet.Column(columna + 5).Width = 10.29D;
            ws.Cells[fila, columna + 5].Worksheet.Column(columna + 6).Width = 15.43D;
            AjustarPropiedadesHojaImpresion(ws);
        }
        private void AjustarPropiedadesHojaImpresion(ExcelWorksheet ws)
        {
            ws.PrinterSettings.PaperSize = ePaperSize.A4;
            ws.PrinterSettings.Orientation = eOrientation.Landscape;
        }
        private string ObtenerRangoFecha(RangoFechaEstadoTransfer rangoFechaEstadoTransfer)
        {
            return rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial != null && rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal != null ? $" ({rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial.Value.ToString("dd-MM-yyyy")} a {rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal.Value.ToString("dd-MM-yyyy")})" : "";
        }

        [HttpPost]
        [Route("ExcelMovimientosRecepcion")]
        public async Task<byte[]> PostExcelMovimientosRecepcion([FromBody] RangoFechaEstadoTransfer rangoFechaEstadoTransfer)
        {
            try
            {
                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    string rango = ObtenerRangoFecha(rangoFechaEstadoTransfer);
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add($"COMPRAS{rango}");
                    var listadoOrdenesCompra = rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial != null && rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal != null ? await PostListadoOrdenCompraPorRangoFecha(rangoFechaEstadoTransfer) : await GetListadoOrdenCompra();
                    var groupingListadoOC = listadoOrdenesCompra.GroupBy(c => c.Fecha);

                    int fila = 1;
                    int columna = 1;

                    foreach (var item in groupingListadoOC)
                    {
                        var excelRange = excelMethodsService.Ajustar(ws, item.Key.ToString("dd/MM/yyyy"), fila, columna, fila, columna + 6, font: excelMethodsService.CalibriFont(12), isWrapText: true, isMerge: true, isBold: true);
                        excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(221, 221, 221));
                        excelRange.Worksheet.Row(fila).Height = 31.50D;
                        fila++;

                        foreach (var oc in item)
                            fila = (await ExcelOrdenCompra(oc.IdOrdenCompra, ws, fila, columna)) + 1;
                    }
                    AjustarPropiedadesExcelOrdenCompra(ws, fila, 1);
                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }

        [HttpPost]
        [Route("ExcelRequerimientoArticulos")]
        public async Task<byte[]> PostExcelRequerimientoArticulos([FromBody] int idRequerimientoArticulos)
        {
            try
            {
                var requerimientoArticulos = (await GetRequerimientoArticulos(idRequerimientoArticulos)).Resultado as RequerimientoArticulos;
                bool isRequerimientoSolicitado = requerimientoArticulos.Estado.Nombre == Estados.Solicitado;

                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add(isRequerimientoSolicitado ? "ORDEN-PEDIDO" : "ENTREGA MATERIALES");
                    ExcelRequerimientoArticulos(requerimientoArticulos, ws, 1, 1);
                    AjustarPropiedadesExcelRequerimientoArticulos(ws, 1, isRequerimientoSolicitado ? 5 : 6);
                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }
        private int ExcelRequerimientoArticulos(RequerimientoArticulos requerimientoArticulos, ExcelWorksheet ws, int fila, int columna)
        {
            bool isRequerimientoSolicitado = requerimientoArticulos.Estado.Nombre == Estados.Solicitado;
            int columnaAux = isRequerimientoSolicitado ? 5 : 6;

            int filaOriginal = fila;
            int columnaOriginal = columna;

            var fontCalibri10 = excelMethodsService.CalibriFont(10);
            var fontCalibri11 = excelMethodsService.CalibriFont(11);

            var codigoAreaGeografica = "[CÓDIGO ÁREA GEOGRÁFICA]";
            var excelRange = excelMethodsService.Ajustar(ws, $"BANCO DE DESARROLLO DEL ECUADOR B.P. - {codigoAreaGeografica}", fila, columna, fila, columna + columnaAux, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "REQUISICIÓN DE MATERIALES", fila, columna, fila, columna + columnaAux, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "NO PEDIDO:", fila, columna, font: fontCalibri11, isBold: true);
            excelRange = excelMethodsService.Ajustar(ws, requerimientoArticulos.CodigoPedido, fila, columna + 1, fila, columna + 2, font: fontCalibri11, isBold: true, isMerge: true);

            excelRange = excelMethodsService.Ajustar(ws, "FECHA DE REQUISICIÓN:", fila, columna + 3 + (isRequerimientoSolicitado ? 0 : 1), fila, columna + 4 + (isRequerimientoSolicitado ? 0 : 1), font: fontCalibri11, isBold: true, isMerge: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Right);
            excelRange = excelMethodsService.Ajustar(ws, requerimientoArticulos.FechaSolicitud.ToString("dd/MM/yyyy"), fila, columna + columnaAux, font: fontCalibri11, isBold: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "CENTRO DE COSTO:", fila, columna, font: fontCalibri11, isBold: true);
            excelRange = excelMethodsService.Ajustar(ws, requerimientoArticulos?.FuncionarioSolicitante?.Dependencia?.Nombre ?? String.Empty, fila, columna + 1, fila, columna + columnaAux, font: fontCalibri11, isBold: true, isMerge: true);
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "USUARIO:", fila, columna, font: fontCalibri11, isBold: true);
            string usuarioFuncionarioSolicitante = requerimientoArticulos?.FuncionarioSolicitante?.NombreUsuario ?? String.Empty;
            if (!String.IsNullOrEmpty(usuarioFuncionarioSolicitante))
                usuarioFuncionarioSolicitante = $" ({usuarioFuncionarioSolicitante})";
            excelRange = excelMethodsService.Ajustar(ws, $"{requerimientoArticulos.FuncionarioSolicitante.Persona.Nombres} {requerimientoArticulos.FuncionarioSolicitante.Persona.Apellidos}{usuarioFuncionarioSolicitante}", fila, columna + 1, fila, columna + columnaAux, font: fontCalibri11, isBold: true, isMerge: true, isWrapText: true);

            excelRange = ws.Cells[filaOriginal, columnaOriginal, fila, columnaOriginal + columnaAux];
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila++;

            excelRange = excelMethodsService.Ajustar(ws, "CÓDIGO", fila, columna, font: fontCalibri10, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "DESCRIPCIÓN DEL MATERIAL", fila, columna + 1, font: fontCalibri10, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "CANTIDAD SOLICITADA", fila, columna + 2, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "CANTIDAD APROBADA", fila, columna + 3, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            if (!isRequerimientoSolicitado)
            {
                excelRange = excelMethodsService.Ajustar(ws, "CANTIDAD ENTREGADA", fila, columna + 4, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            excelRange = excelMethodsService.Ajustar(ws, "COSTO", fila, columnaAux, font: fontCalibri10, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, "SUBTOTAL", fila, columnaAux + 1, font: fontCalibri10, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange.Worksheet.Row(fila).Height = 40.50D;
            excelRange = ws.Cells[fila, columna, fila, columna + columnaAux];
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila++;
            decimal total = 0;

            foreach (var item in requerimientoArticulos.RequerimientosArticulosDetalles)
            {
                excelRange = excelMethodsService.Ajustar(ws, item.MaestroArticuloSucursal.CodigoArticulo, fila, columna, font: fontCalibri11);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, item.MaestroArticuloSucursal.Articulo.Nombre, fila, columna + 1, font: fontCalibri11);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, item.CantidadSolicitada.ToString(), fila, columna + 2, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                excelRange = excelMethodsService.Ajustar(ws, item.CantidadAprobada.ToString(), fila, columna + 3, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                if (!isRequerimientoSolicitado)
                {
                    excelRange = excelMethodsService.Ajustar(ws, item.CantidadEntregada.ToString(), fila, columna + 4, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                excelRange = excelMethodsService.Ajustar(ws, item.ValorActual.ToString(), fila, columnaAux, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                int cantidad = requerimientoArticulos.Estado.Nombre == Estados.Solicitado ? item.CantidadSolicitada : item.CantidadEntregada;
                decimal subtotal = cantidad * item.ValorActual;
                excelRange = excelMethodsService.Ajustar(ws, subtotal.ToString(), fila, columnaAux + 1, font: fontCalibri11, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                total += subtotal;
                fila++;
            }
            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna, font: fontCalibri11, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 1, font: fontCalibri11, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 2, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 3, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            if (!isRequerimientoSolicitado)
            {
                excelRange = excelMethodsService.Ajustar(ws, " ", fila, columna + 4, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            excelRange = excelMethodsService.Ajustar(ws, requerimientoArticulos.RequerimientosArticulosDetalles.Sum(c => c.ValorActual).ToString("C2").Replace("$", ""), fila, columnaAux, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange = excelMethodsService.Ajustar(ws, total.ToString(), fila, columnaAux + 1, font: fontCalibri11, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            excelRange.Worksheet.Row(fila).Height = 25.50D;
            excelRange = ws.Cells[fila, columna, fila, columna + columnaAux];
            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
            fila += 2;

            excelRange = excelMethodsService.Ajustar(ws, "OBSERVACIONES:", fila, columna, font: fontCalibri11, isBold: true);
            excelRange = excelMethodsService.Ajustar(ws, requerimientoArticulos.Observaciones, fila, columna + 1, fila, columna + columnaAux, font: fontCalibri11, isBold: true, isMerge: true);

            ws.Cells[fila, columna].Worksheet.Column(columna).Width = 18D;
            ws.Cells[fila, columna + 1].Worksheet.Column(columna + 1).Width = 27.63D;
            ws.Cells[fila, columna + 2].Worksheet.Column(columna + 2).Width = 11.01D;
            ws.Cells[fila, columna + 3].Worksheet.Column(columna + 3).Width = 11.01D;

            if (!isRequerimientoSolicitado)
                ws.Cells[fila, columna + 4].Worksheet.Column(columna + 4).Width = 11.01D;

            return fila;
        }
        private void AjustarPropiedadesExcelRequerimientoArticulos(ExcelWorksheet ws, int fila, int columna)
        {
            ws.Cells[fila, columna].Worksheet.Column(columna).Width = 11.01D;
            ws.Cells[fila, columna + columna].Worksheet.Column(columna + columna).Width = 19.13D;
            AjustarPropiedadesHojaImpresion(ws);
        }

        [HttpPost]
        [Route("ExcelMovimientosSalida")]
        public async Task<byte[]> PostExcelMovimientosSalida([FromBody] RangoFechaEstadoTransfer rangoFechaEstadoTransfer)
        {
            try
            {
                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    string rango = ObtenerRangoFecha(rangoFechaEstadoTransfer);
                    var listadoRequerimientoArticulos = rangoFechaEstadoTransfer.RangoFechaTransfer.FechaInicial != null && rangoFechaEstadoTransfer.RangoFechaTransfer.FechaFinal != null ? await PostListadoRequerimientoArticulosPorEstadoRangoFecha(rangoFechaEstadoTransfer) : await PostListadoRequerimientoArticulosPorEstado(rangoFechaEstadoTransfer.Estados);
                    bool isRequerimientoSolicitado = listadoRequerimientoArticulos[0].Estado.Nombre == Estados.Solicitado;
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add(isRequerimientoSolicitado ? $"ORDEN-PEDIDO{rango}" : $"ENTREGA MATERIALES{rango}");
                    var groupingListadoReq = listadoRequerimientoArticulos.GroupBy(c => c.FechaAprobadoDenegado);

                    int fila = 1;
                    int columna = 1;

                    foreach (var item in groupingListadoReq)
                    {
                        var excelRange = excelMethodsService.Ajustar(ws, item.Key.Value.ToString("dd/MM/yyyy"), fila, columna, fila, columna + 6, font: excelMethodsService.CalibriFont(12), isWrapText: true, isMerge: true, isBold: true);
                        excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(221, 221, 221));
                        excelRange.Worksheet.Row(fila).Height = 31.50D;
                        fila++;

                        foreach (var req in item)
                            fila = (ExcelRequerimientoArticulos(req, ws, fila, columna)) + 3;
                    }
                    AjustarPropiedadesExcelRequerimientoArticulos(ws, fila, 1);
                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }

        [HttpPost]
        [Route("ExcelProveedoresPorArticulo")]
        public async Task<byte[]> PostExcelProveedoresPorArticulo([FromBody] int idArticulo)
        {
            try
            {
                var listadoProveedoresPorArticulo = await PostProveedoresPorArticulo(idArticulo);
                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("PROVEEDORES");
                    int fila = 1;
                    int filaOriginal = 1;

                    int columna = 1;
                    int columnaOriginal = 1;

                    var fontCalibri10 = excelMethodsService.CalibriFont(10);
                    var fontCalibri11 = excelMethodsService.CalibriFont(11);

                    var codigoAreaGeografica = "[CÓDIGO ÁREA GEOGRÁFICA]";
                    var excelRange = excelMethodsService.Ajustar(ws, $"BANCO DE DESARROLLO DEL ECUADOR B.P. - {codigoAreaGeografica}", fila, columna, fila, columna + 11, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
                    fila++;

                    excelRange = excelMethodsService.Ajustar(ws, "FECHA:", fila, columna + 5, font: fontCalibri11, isBold: true, isMerge: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Right);
                    excelRange = excelMethodsService.Ajustar(ws, DateTime.Now.ToString("dd/MM/yyyy"), fila, columna + 6, font: fontCalibri11, isBold: true);

                    excelRange = ws.Cells[filaOriginal, columnaOriginal, fila, columnaOriginal + 11];
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
                    fila++;

                    excelRange = excelMethodsService.Ajustar(ws, "CÓDIGO", fila, columna, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "REPRESENTANTE LEGAL", fila, columna + 1, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "PERSONA DE CONTACTO", fila, columna + 2, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "IDENTIFICACIÓN", fila, columna + 3, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "LINEA DE SERVICIO", fila, columna + 4, font: fontCalibri10, isWrapText: true, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "RAZÓN SOCIAL", fila, columna + 5, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "TELÉFONO", fila, columna + 6, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "CORREO ELECTRÓNICO", fila, columna + 7, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "CARGO", fila, columna + 8, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "DIRECCIÓN", fila, columna + 9, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "OBSERVACIONES", fila, columna + 10, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "¿ACTIVO?", fila, columna + 11, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange.Worksheet.Row(fila).Height = 40.50D;
                    excelRange = ws.Cells[fila, columna, fila, columna + 11];
                    excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
                    fila++;

                    foreach (var item in listadoProveedoresPorArticulo)
                    {
                        if (item.ListadoProveedores != null && item.ListadoProveedores.Count > 0)
                        {
                            excelRange = excelMethodsService.Ajustar(ws, item.Articulo.Nombre, fila, columna, fila, columna + 11, font: fontCalibri11, isWrapText: true, isMerge: true, isBold: true, isItalic: true);
                            excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(221, 221, 221));
                            fila++;

                            foreach (var prov in item.ListadoProveedores)
                            {
                                excelRange = excelMethodsService.Ajustar(ws, prov.Codigo, fila, columna, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.RepresentanteLegal, fila, columna + 1, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.PersonaContacto, fila, columna + 2, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Identificacion, fila, columna + 3, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.LineaServicio.Nombre, fila, columna + 4, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.RazonSocial, fila, columna + 5, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Telefono, fila, columna + 6, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Email, fila, columna + 7, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Cargo, fila, columna + 8, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Direccion, fila, columna + 9, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Observaciones, fila, columna + 10, font: fontCalibri11, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                excelRange = excelMethodsService.Ajustar(ws, prov.Activo ? "Si" : "No", fila, columna + 11, font: fontCalibri11, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;
                            }
                        }
                    }

                    ws.Cells[fila, columna].Worksheet.Column(columna).Width = 13.50D;
                    ws.Cells[fila, columna + 9].Worksheet.Column(columna + 9).Width = 29.10D;

                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }

        [HttpPost]
        [Route("ExcelArticulosPorProveedor")]
        public async Task<byte[]> PostExcelArticulosPorroveedor([FromBody] int idProveedor)
        {
            try
            {
                var listadoArticulosPorProveedor = await PostArticulosPorProveedor(idProveedor);
                var groupByArticulosPorProveedor = listadoArticulosPorProveedor.GroupBy(c => c.Proveedor.RazonSocial);

                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("ARTÍCULOS");
                    int fila = 1;
                    int filaOriginal = 1;

                    int columna = 1;
                    int columnaOriginal = 1;

                    var fontCalibri10 = excelMethodsService.CalibriFont(10);
                    var fontCalibri11 = excelMethodsService.CalibriFont(11);

                    var codigoAreaGeografica = "[CÓDIGO ÁREA GEOGRÁFICA]";
                    var excelRange = excelMethodsService.Ajustar(ws, $"BANCO DE DESARROLLO DEL ECUADOR B.P. - {codigoAreaGeografica}", fila, columna, fila, columna + 7, font: fontCalibri11, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
                    fila++;

                    excelRange = excelMethodsService.Ajustar(ws, "FECHA:", fila, columna + 4, font: fontCalibri11, isBold: true, isMerge: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Right);
                    excelRange = excelMethodsService.Ajustar(ws, DateTime.Now.ToString("dd/MM/yyyy"), fila, columna + 5, font: fontCalibri11, isBold: true);

                    excelRange = ws.Cells[filaOriginal, columnaOriginal, fila, columnaOriginal + 7];
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
                    fila++;

                    excelRange = excelMethodsService.Ajustar(ws, "ARTÍCULO", fila, columna, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "TIPO DE ARTÍCULO", fila, columna + 1, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "CLASE DE ARTÍCULO", fila, columna + 2, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "SUBCLASE DE ARTÍCULO", fila, columna + 3, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "UNIDAD DE MEDIDA", fila, columna + 4, font: fontCalibri10, isWrapText: true, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "MARCA", fila, columna + 5, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "MODELO", fila, columna + 6, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange = excelMethodsService.Ajustar(ws, "VALOR DEL ARTÍCULO", fila, columna + 7, font: fontCalibri10, isBold: true, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, excelVerticalAlignment: ExcelVerticalAlignment.Center);
                    excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    excelRange.Worksheet.Row(fila).Height = 40.50D;
                    excelRange = ws.Cells[fila, columna, fila, columna + 7];
                    excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(255, 242, 204));
                    fila++;

                    foreach (var item in groupByArticulosPorProveedor)
                    {
                        excelRange = excelMethodsService.Ajustar(ws, item.Key, fila, columna, fila, columna + 7, font: fontCalibri11, isWrapText: true, isMerge: true, isBold: true, isItalic: true);
                        excelMethodsService.ChangeBackground(excelRange, System.Drawing.Color.FromArgb(221, 221, 221));
                        fila++;

                        foreach (var masSelecc in item)
                        {
                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.Nombre, fila, columna, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.SubClaseArticulo.ClaseArticulo.TipoArticulo.Nombre, fila, columna + 1, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.SubClaseArticulo.ClaseArticulo.Nombre, fila, columna + 2, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.SubClaseArticulo.Nombre, fila, columna + 3, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.UnidadMedida.Nombre, fila, columna + 4, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.Modelo.Marca.Nombre, fila, columna + 5, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.Articulo.Modelo.Nombre, fila, columna + 6, font: fontCalibri11, isWrapText: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, masSelecc.MaestroArticuloSucursal.ValorActual.ToString("C2").Replace("$", ""), fila, columna + 7, font: fontCalibri11, isWrapText: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;
                        }
                    }
                    ws.Cells[fila, columna].Worksheet.Column(columna).Width = 28.60D;

                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;

                    return objExcelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }
        #endregion
    }
}