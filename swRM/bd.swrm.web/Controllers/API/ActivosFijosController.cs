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
using bd.swrm.entidades.Constantes;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Html2pdf.Css.Util;
using iText.Html2pdf;
using iText.Html2pdf.Css.Media;
using iText.Kernel.Events;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Layout.Layout;
using System.IO;
using iText.Kernel.Colors;
using iText.IO.Image;
using Microsoft.AspNetCore.Hosting;
using bd.swrm.servicios.PDFHandler;
using OfficeOpenXml;
using System.Text;
using OfficeOpenXml.Style;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ActivosFijos")]
    public class ActivosFijosController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;
        private readonly IEmailSender emailSender;
        private readonly IClaimsTransfer claimsTransfer;
        private readonly IClonacion clonacionService;
        private readonly IPdfMethods pdfMethodsService;
        private readonly IExcelMethods excelMethodsService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ActivosFijosController(SwRMDbContext db, IUploadFileService uploadFileService, IEmailSender emailSender, IClaimsTransfer claimsTransfer, IHttpContextAccessor httpContextAccessor, IClonacion clonacionService, IPdfMethods pdfMethodsService, IHostingEnvironment _hostingEnvironment, IExcelMethods excelMethodsService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
            this.emailSender = emailSender;
            this.claimsTransfer = claimsTransfer;
            this.clonacionService = clonacionService;
            this.pdfMethodsService = pdfMethodsService;
            this._hostingEnvironment = _hostingEnvironment;
            this.excelMethodsService = excelMethodsService;
        }

        [HttpGet]
        [Route("ListarActivosFijos")]
        public async Task<List<ActivoFijo>> GetActivosFijos()
        {
            return await ListarActivosFijos();
        }

        [HttpGet]
        [Route("ListarActivosFijosPorAgrupacion")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorAgrupacion()
        {
            return await ListarActivosFijosPorAgrupacionSucursalNombre();
        }

        [HttpGet]
        [Route("ListarAltasActivosFijos")]
        public async Task<List<AltaActivoFijo>> GetAltasActivosFijos()
        {
            return await ListarAltasActivoFijo();
        }

        [HttpGet]
        [Route("ListarAltasActivosFijosConComponentes")]
        public async Task<List<AltaActivoFijo>> GetAltasActivosFijosConComponentes()
        {
            return await ListarAltasActivoFijo(incluirComponentes: true);
        }

        [HttpGet]
        [Route("ListarBajasActivosFijos")]
        public async Task<List<BajaActivoFijo>> GetBajasActivosFijos()
        {
            return await ListarBajasActivoFijo();
        }

        [HttpGet]
        [Route("ListarTransferenciasCambiosCustodioActivosFijos")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciasCambiosCustodioActivosFijos()
        {
            return await ListarTransferenciasActivoFijo(new TransferenciaEstadoTransfer { MotivoTransferencia = MotivosTransferencia.CambioCustodio, Estado = Estados.Aceptada });
        }

        [HttpGet]
        [Route("ListarTransferenciasCambiosUbicacionSolicitudActivosFijos")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciasCambiosUbicacionSolicitudActivosFijos()
        {
            var claimsTransferencia = claimsTransfer.ObtenerClaimsTransferHttpContext();
            return await ListarTransferenciasActivoFijo(new TransferenciaEstadoTransfer { MotivoTransferencia = MotivosTransferencia.CambioUbicacion }, incluirClaimsTransferencia: false, predicadoTransferenciaActivoFijoDetalle: c=> c.UbicacionActivoFijoDestino.Empleado.Dependencia.IdSucursal == claimsTransferencia.IdSucursal);
        }

        [HttpGet]
        [Route("ListarTransferenciasCambiosUbicacionCreadasActivosFijos")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciasCambiosUbicacionCreadasSolicitudActivosFijos()
        {
            var claimsTransferencia = claimsTransfer.ObtenerClaimsTransferHttpContext();
            return await ListarTransferenciasActivoFijo(new TransferenciaEstadoTransfer { MotivoTransferencia = MotivosTransferencia.CambioUbicacion }, incluirClaimsTransferencia: false, predicadoTransferenciaActivoFijoDetalle: c => c.UbicacionActivoFijoOrigen.Empleado.Dependencia.IdSucursal == claimsTransferencia.IdSucursal);
        }

        [HttpGet]
        [Route("ListarInventariosActivosFijos")]
        public async Task<List<InventarioActivoFijo>> GetInventarioActivosFijos()
        {
            return await ListarInventariosActivosFijos();
        }

        [HttpGet]
        [Route("ListarMovilizacionesActivosFijos")]
        public async Task<List<MovilizacionActivoFijo>> GetMovilizacionesActivosFijos()
        {
            return await ListarMovilizacionesActivosFijos();
        }

        [HttpPost]
        [Route("ListarInventariosActivosFijosPorRangoFecha")]
        public async Task<List<InventarioActivoFijo>> PostInventariosActivosFijosPorRangoFecha([FromBody] RangoFechaTransfer rangoFechaTransfer)
        {
            return await ListarInventariosActivosFijos(predicado: c=> (c.FechaInforme >= rangoFechaTransfer.FechaInicial && c.FechaInforme <= rangoFechaTransfer.FechaFinal) || (c.FechaCorteInventario >= rangoFechaTransfer.FechaInicial && c.FechaCorteInventario <= rangoFechaTransfer.FechaFinal));
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoConPoliza")]
        public async Task<List<RecepcionActivoFijo>> GetActivosFijosConPoliza()
        {
            return await ListarRecepcionesActivosFijos(predicado: c => c.PolizaSeguroActivoFijo.NumeroPoliza != null);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoSinPoliza")]
        public async Task<List<RecepcionActivoFijo>> GetActivosFijosSinPoliza()
        {
            return await ListarRecepcionesActivosFijos(predicado: c => c.PolizaSeguroActivoFijo.NumeroPoliza == null);
        }

        [HttpGet]
        [Route("ListarRecepcionActivo")]
        public async Task<List<RecepcionActivoFijo>> GetListadoRecepcionActivo()
        {
            return await ListarRecepcionesActivosFijos(predicadoDetalleActivoFijo: c => c.Estado.Nombre.ToUpper() == Estados.Recepcionado || c.Estado.Nombre.ToUpper() == Estados.ValidacionTecnica);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoValidacionTecnica")]
        public async Task<List<RecepcionActivoFijo>> GetListadoRecepcionActivoValidacionTecnica()
        {
            return await ListarRecepcionesActivosFijos(predicadoDetalleActivoFijo: c => c.Estado.Nombre.ToUpper() == Estados.ValidacionTecnica);
        }

        [HttpGet]
        [Route("ListarIdsRecepcionesActivosFijos")]
        public async Task<List<int>> GetListadoIdsRecepcionesActivosFijos()
        {
            try
            {
                var listadoRecepcionesActivosFijos = await ListarRecepcionesActivosFijos(predicado: c=> c.RecepcionActivoFijoDetalle.Count > 0, predicadoDetalleActivoFijo: c=> c.Estado.Nombre.ToUpper() == Estados.Recepcionado && c.RecepcionActivoFijo.MotivoAlta.Descripcion.ToUpper() != MotivosAlta.Adicion);
                return listadoRecepcionesActivosFijos.Select(c => c.IdRecepcionActivoFijo).ToList();
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijos([FromRoute]int id)
        {
            return await ObtenerActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerRecepcionActivoFijo/{id}")]
        public async Task<Response> GetObtenerRecepcionActivoFijo([FromRoute] int id)
        {
            return await ObtenerRecepcionActivoFijo(id, predicadoDetalleActivoFijo: c=> c.Estado.Nombre.ToUpper() == Estados.Recepcionado || c.Estado.Nombre == Estados.ValidacionTecnica);
        }

        [HttpGet]
        [Route("ObtenerRecepcionActivoFijoValidacionTecnica/{id}")]
        public async Task<Response> GetObtenerRecepcionActivoFijoValidacionTecnica([FromRoute] int id)
        {
            return await ObtenerRecepcionActivoFijo(id, predicadoDetalleActivoFijo: c => c.Estado.Nombre.ToUpper() == Estados.ValidacionTecnica);
        }

        [HttpGet]
        [Route("ObtenerRecepcionPolizaSeguroActivoFijo/{id}")]
        public async Task<Response> GetObtenerRecepcionPolizaSeguroActivoFijo([FromRoute] int id)
        {
            return await ObtenerRecepcionActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerAltaActivosFijos/{id}")]
        public async Task<Response> GetAltaActivoFijo([FromRoute] int id)
        {
            return await ObtenerAltaActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerAltaActivosFijosConComponentes/{id}")]
        public async Task<Response> GetAltaActivoFijoConComponentes([FromRoute] int id)
        {
            return await ObtenerAltaActivoFijo(id, incluirComponentes: true);
        }

        [HttpGet]
        [Route("ObtenerBajaActivosFijos/{id}")]
        public async Task<Response> GetBajaActivoFijo([FromRoute] int id)
        {
            return await ObtenerBajaActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerBajaActivosFijosConComponentes/{id}")]
        public async Task<Response> GetBajaActivoFijoConComponentes([FromRoute] int id)
        {
            return await ObtenerBajaActivoFijo(id, incluirComponentes: true);
        }

        [HttpGet]
        [Route("ObtenerTransferenciaActivoFijo/{id}")]
        public async Task<Response> GetTransferenciaActivoFijo([FromRoute] int id)
        {
            return await ObtenerTransferenciaActivoFijo(id, incluirClaimsTransferencia: false);
        }

        [HttpGet]
        [Route("ObtenerInventarioActivosFijos/{id}")]
        public async Task<Response> GetInventarioActivoFijo([FromRoute] int id)
        {
            return await ObtenerInventarioActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerMovilizacionActivosFijos/{id}")]
        public async Task<Response> GetMovilizacionActivoFijo([FromRoute] int id)
        {
            return await ObtenerMovilizacionActivoFijo(id);
        }

        [HttpPost]
        [Route("ObtenerActivoFijoPorEstado")]
        public async Task<Response> PostActivoFijoPorEstado([FromBody] IdEstadosTransfer idActivoFijoEstadosTransfer)
        {
            return idActivoFijoEstadosTransfer.Estados.Count > 0 ? await ObtenerActivoFijo(idActivoFijoEstadosTransfer.Id, predicadoDetalleActivoFijo: c=> idActivoFijoEstadosTransfer.Estados.Contains(c.Estado.Nombre.ToUpper())) : await ObtenerActivoFijo(idActivoFijoEstadosTransfer.Id);
        }

        [HttpPost]
        [Route("DetallesActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> GetRecepcionActivoFijoDetalleSeleccionado([FromBody] List<IdRecepcionActivoFijoDetalleSeleccionado> listadoRecepcionActivoFijoDetalleSeleccionado)
        {
            var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
            try
            {
                foreach (var item in listadoRecepcionActivoFijoDetalleSeleccionado)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.idRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true, incluirAltasActivoFijo: true, incluirBajasActivoFijo: true);
                    if (recepcionActivoFijoDetalle != null)
                        lista.Add(new RecepcionActivoFijoDetalleSeleccionado { RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle, Seleccionado = item.seleccionado });
                }
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
            return lista;
        }

        [HttpPost]
        [Route("ObtenerDetalleActivoFijoParaInventario")]
        public async Task<List<RecepcionActivoFijoDetalle>> PostDetalleActivoFijoParaInventario([FromBody] InventarioTransfer inventarioTransfer)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalle>();
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).Where(c => c.CodigoActivoFijo.Codigosecuencial == inventarioTransfer.Codigosecuencial && c.Estado.Nombre.ToUpper() == Estados.Alta && !inventarioTransfer.ListadoRafdSeleccionados.Contains(c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                foreach (var item in listaRecepcionActivoFijoDetalle)
                    lista.Add(ObtenerRecepcionActivoFijoDetalle(item, incluirActivoFijo: true));
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RecepcionActivoFijoDetalle>();
            }
        }

        [HttpPost]
        [Route("DetallesActivoFijoSeleccionadoPorEstado")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostActivoFijoSeleccionadoPorEstado([FromBody] IdRecepcionActivoFijoDetalleSeleccionadoEstado idRecepcionActivoFijoDetalleSeleccionadoEstado)
        {
            var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
            try
            {
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true, incluirBajasActivoFijo: idRecepcionActivoFijoDetalleSeleccionadoEstado.Estados.Contains(Estados.Baja)).Where(c => idRecepcionActivoFijoDetalleSeleccionadoEstado.Estados.Contains(c.Estado.Nombre.ToUpper())).ToListAsync();
                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoEstado.ListaIdRecepcionActivoFijoDetalleSeleccionado.Where(c=> c.seleccionado).Select(c => c.idRecepcionActivoFijoDetalle);
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                    {
                        RecepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true),
                        Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle)
                    });
                }
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
            return lista;
        }

        [HttpPost]
        [Route("ListarActivoFijoPorEstado")]
        public async Task<List<ActivoFijo>> PostActivosFijosPorEstado([FromBody] List<string> estados)
        {
            return await ListarActivosFijos(predicadoRecepcionActivoFijoDetalle: c => estados.Contains(c.Estado.Nombre.ToUpper()));
        }

        [HttpPost]
        [Route("ListarComponentesDisponiblesActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostComponentesActivosFijos([FromBody] IdRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir)
        {
            var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
            try
            {
                var listaIdsExcluirTablaComponenteActivoFijo = await db.ComponenteActivoFijo
                    .Select(c => c.IdRecepcionActivoFijoDetalleComponente)
                    .Except(idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.ListaIdRecepcionActivoFijoDetalleSeleccionado
                    .Select(c => c.idRecepcionActivoFijoDetalle))
                    .ToListAsync();

                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);

                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true)
                .Where(c=> (c.Estado.Nombre.ToUpper() == Estados.Recepcionado || c.Estado.Nombre.ToUpper() == Estados.Alta)
                && c.RecepcionActivoFijo.MotivoAlta.Descripcion.ToUpper() == MotivosAlta.Adicion
                && c.Estado.Nombre.ToUpper() != Estados.ValidacionTecnica
                && (!idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.IdsComponentesExcluir.Contains(c.IdRecepcionActivoFijoDetalle)
                && !listaIdsExcluirTablaComponenteActivoFijo.Contains(c.IdRecepcionActivoFijoDetalle))).ToListAsync();

                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                    {
                        RecepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true),
                        Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle)
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
        }

        [HttpPost]
        [Route("ListarDepreciacionActivoFijo")]
        public async Task<List<DepreciacionActivoFijo>> PostListarDepreciacionActivoFijo([FromBody] int id)
        {
            try
            {
                var listadoDepreciacionActivosFijos = await db.DepreciacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == id).OrderByDescending(c=> c.FechaDepreciacion).ToListAsync();
                return listadoDepreciacionActivosFijos;
            }
            catch (Exception)
            {
                return new List<DepreciacionActivoFijo>();
            }
        }

        [HttpPost]
        [Route("DetallesActivoFijoSeleccionadoPorEstadoAlta")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorEstadoAlta([FromBody] IdRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => (c.Estado.Nombre.ToUpper() == Estados.Recepcionado || (c.Estado.Nombre.ToUpper() == Estados.Alta && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x=> x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))) && c.RecepcionActivoFijo.MotivoAlta.Descripcion.ToUpper() != MotivosAlta.Adicion).ToListAsync();

                if (idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.IdRecepcionActivoFijo > 0)
                    listaRecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle.Where(c => c.IdRecepcionActivoFijo == idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.IdRecepcionActivoFijo).ToList();

                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                    {
                        RecepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true),
                        Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle),
                        Componentes = await ObtenerComponentesRecepcionActivoFijo(item)
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
        }

        [HttpPost]
        [Route("DetallesActivoFijoSeleccionadoPorEstadoBaja")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorEstadoBaja([FromBody] IdRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => c.Estado.Nombre.ToUpper() == Estados.Alta || (c.Estado.Nombre.ToUpper() == Estados.Baja && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x => x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))).ToListAsync();
                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                    {
                        RecepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirComponentes: true, incluirActivoFijo: true, incluirAltasActivoFijo: true),
                        Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle),
                        Componentes = await ObtenerComponentesRecepcionActivoFijo(item)
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
        }

        [HttpPost]
        [Route("DetallesActivoFijoSeleccionadoPorEmpleado")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorEmpleado([FromBody] CambioCustodioViewModel cambioCustodioViewModel)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaIdsRafdTransferenciasCreadas = await db.TransferenciaActivoFijoDetalle.Include(c => c.UbicacionActivoFijoDestino)
                    .Where(c => !c.UbicacionActivoFijoDestino.Confirmacion)
                    .Select(c => c.IdRecepcionActivoFijoDetalle).ToListAsync();

                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true)
                    .Where(c => c.UbicacionActivoFijoActual.IdEmpleado == cambioCustodioViewModel.IdEmpleadoEntrega && !listaIdsRafdTransferenciasCreadas.Contains(c.IdRecepcionActivoFijoDetalle))
                    .ToListAsync();

                return listaRecepcionActivoFijoDetalle.Select(c=> new RecepcionActivoFijoDetalleSeleccionado {
                    RecepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(c, incluirComponentes: true, incluirActivoFijo: true),
                    Seleccionado = cambioCustodioViewModel.ListadoIdRecepcionActivoFijoDetalle.Contains(c.IdRecepcionActivoFijoDetalle)
                }).ToList();
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
        }

        [HttpPost]
        [Route("DetallesActivoFijoSeleccionadoPorMovilizacion")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorMovilizacion([FromBody] MovilizacionActivoFijoTransfer movilizacionActivoFijoTransfer)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaIdsRAFDSeleccionados = movilizacionActivoFijoTransfer.ListadoRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
                var listaRecepcionActivoFijoDetalleBD = ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).Where(c => (c.Estado.Nombre.ToUpper() == Estados.Alta));
                var listaRecepcionActivoFijoDetalle = movilizacionActivoFijoTransfer.SeleccionarTodasAltas ? await listaRecepcionActivoFijoDetalleBD.ToListAsync() : await listaRecepcionActivoFijoDetalleBD.Where(c=> listaIdsRAFDSeleccionados.Contains(c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    var rafd = ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true);
                    var movilizacionActivoFijoDetalle = movilizacionActivoFijoTransfer.IdMovilizacionActivoFijo != null ? db.MovilizacionActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdMovilizacionActivoFijo == movilizacionActivoFijoTransfer.IdMovilizacionActivoFijo) : null;

                    lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                    {
                        RecepcionActivoFijoDetalle = rafd,
                        Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle),
                        Componentes = await ObtenerComponentesRecepcionActivoFijo(rafd),
                        Observaciones = movilizacionActivoFijoDetalle?.Observaciones ?? ""
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
        }

        private async Task<string> ObtenerComponentesRecepcionActivoFijo(RecepcionActivoFijoDetalle rafd)
        {
            var listaRecepcionActivoFijoComponentes = new List<string>();
            if (rafd.ComponentesActivoFijoOrigen.Count > 0)
            {
                foreach (var comp in rafd.ComponentesActivoFijoOrigen)
                {
                    var rafComp = await db.RecepcionActivoFijoDetalle.Include(c => c.ActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                    listaRecepcionActivoFijoComponentes.Add(rafComp.ActivoFijo.Nombre);
                }
            }
            return String.Join(", ", listaRecepcionActivoFijoComponentes);
        }

        [HttpPost]
        [Route("ListarActivosFijosPorAgrupacionPorEstado")]
        public async Task<List<ActivoFijo>> PostActivosFijosPorAgrupacionPorEstado([FromBody] string estado)
        {
            return await ListarActivosFijosPorAgrupacionSucursalNombre(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre.ToUpper() == estado);
        }

        [HttpPost]
        [Route("ListarActivosFijosPorAgrupacionDepreciacion")]
        public async Task<List<ActivoFijo>> PostActivosFijosPorAgrupacionPorEstadoDepreciacion()
        {
            return await ListarActivosFijosPorAgrupacionSucursalNombre(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre.ToUpper() == Estados.Alta && c.ActivoFijo.Depreciacion);
        }

        [HttpPost]
        [Route("AsignarPolizaSeguro")]
        public async Task<Response> AsignarPoliza([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var polizaSeguroActivoFijoActualizar = await db.PolizaSeguroActivoFijo.FirstOrDefaultAsync(c=> c.IdRecepcionActivoFijo == recepcionActivoFijoDetalle.IdRecepcionActivoFijo);
                if (polizaSeguroActivoFijoActualizar != null)
                {
                    if (!(polizaSeguroActivoFijoActualizar.NumeroPoliza == null ? await db.PolizaSeguroActivoFijo.AnyAsync(c => c.NumeroPoliza == recepcionActivoFijoDetalle.RecepcionActivoFijo.PolizaSeguroActivoFijo.NumeroPoliza) : await db.PolizaSeguroActivoFijo.Where(c => c.NumeroPoliza == recepcionActivoFijoDetalle.RecepcionActivoFijo.PolizaSeguroActivoFijo.NumeroPoliza).AnyAsync(c => c.IdRecepcionActivoFijo != recepcionActivoFijoDetalle.IdRecepcionActivoFijo)))
                    {
                        try
                        {
                            polizaSeguroActivoFijoActualizar.NumeroPoliza = recepcionActivoFijoDetalle.RecepcionActivoFijo.PolizaSeguroActivoFijo.NumeroPoliza;
                            db.PolizaSeguroActivoFijo.Update(polizaSeguroActivoFijoActualizar);
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

        [HttpPost]
        [Route("InsertarRecepcionActivoFijo")]
        public async Task<Response> PostActivosFijos([FromBody] List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalleTransfer)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var recepcionActivoFijo = new RecepcionActivoFijo
                    {
                        IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor,
                        IdMotivoAlta = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoAlta,
                        FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion,
                        IdFondoFinanciamiento = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdFondoFinanciamiento,
                        OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra
                    };
                    db.RecepcionActivoFijo.Add(recepcionActivoFijo);
                    await db.SaveChangesAsync();
                    listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo;

                    var polizaSeguro = new PolizaSeguroActivoFijo
                    {
                        IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo,
                        IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro
                    };
                    db.PolizaSeguroActivoFijo.Add(polizaSeguro);
                    await db.SaveChangesAsync();

                    var igrouping = listaRecepcionActivoFijoDetalleTransfer.GroupBy(c => new { c.ActivoFijo.IdSubClaseActivoFijo, c.ActivoFijo.IdModelo, c.ActivoFijo.Nombre });
                    foreach (var item in igrouping)
                    {
                        var listadoRafd = item.ToList();
                        var activoFijo = new ActivoFijo
                        {
                            IdSubClaseActivoFijo = listadoRafd[0].ActivoFijo.IdSubClaseActivoFijo,
                            IdModelo = listadoRafd[0].ActivoFijo.IdModelo,
                            Nombre = listadoRafd[0].ActivoFijo.Nombre,
                            ValorCompra = listadoRafd[0].ActivoFijo.ValorCompra,
                            Depreciacion = listadoRafd[0].ActivoFijo.Depreciacion,
                            ValidacionTecnica = listadoRafd[0].ActivoFijo.ValidacionTecnica
                        };
                        db.ActivoFijo.Add(activoFijo);
                        await db.SaveChangesAsync();

                        for (int i = 0; i < listadoRafd.Count; i++)
                        {
                            var codigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = listadoRafd[i].CodigoActivoFijo.Codigosecuencial };
                            db.CodigoActivoFijo.Add(codigoActivoFijo);
                            await db.SaveChangesAsync();

                            var ubicacionActivoFijo = listadoRafd[i].UbicacionActivoFijoActual;
                            var recepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle
                            {
                                IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo,
                                IdActivoFijo = activoFijo.IdActivoFijo,
                                IdEstado = listadoRafd[i].IdEstado,
                                IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                                Serie = listadoRafd[i].Serie
                            };
                            db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                            await db.SaveChangesAsync();

                            if (listadoRafd[i].RecepcionActivoFijoDetalleEdificio != null)
                            {
                                var nuevaRecepcionActivoFijoDetalleEdificio = new RecepcionActivoFijoDetalleEdificio
                                {
                                    IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                    NumeroClaveCatastral = listadoRafd[i].RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral
                                };
                                db.RecepcionActivoFijoDetalleEdificio.Add(nuevaRecepcionActivoFijoDetalleEdificio);
                            }

                            if (listadoRafd[i].RecepcionActivoFijoDetalleVehiculo != null)
                            {
                                var nuevaRecepcionActivoFijoDetalleVehiculo = new RecepcionActivoFijoDetalleVehiculo
                                {
                                    IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                    NumeroChasis = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroChasis,
                                    NumeroMotor = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroMotor,
                                    Placa = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.Placa
                                };
                                db.RecepcionActivoFijoDetalleVehiculo.Add(nuevaRecepcionActivoFijoDetalleVehiculo);
                            }

                            db.UbicacionActivoFijo.Add(new UbicacionActivoFijo
                            {
                                IdEmpleado = ubicacionActivoFijo.IdEmpleado,
                                IdBodega = ubicacionActivoFijo.IdBodega,
                                IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                FechaUbicacion = ubicacionActivoFijo.FechaUbicacion,
                                Confirmacion = true
                            });
                            await db.SaveChangesAsync();

                            foreach (var item1 in listadoRafd[i].ComponentesActivoFijoOrigen)
                            {
                                item1.IdRecepcionActivoFijoDetalleOrigen = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle;
                                db.ComponenteActivoFijo.Add(item1);
                                db.UbicacionActivoFijo.Add(new UbicacionActivoFijo
                                {
                                    IdEmpleado = ubicacionActivoFijo.IdEmpleado,
                                    IdBodega = ubicacionActivoFijo.IdBodega,
                                    IdRecepcionActivoFijoDetalle = item1.IdRecepcionActivoFijoDetalleComponente,
                                    FechaUbicacion = ubicacionActivoFijo.FechaUbicacion,
                                    Confirmacion = true
                                });
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                    transaction.Commit();
                    var recepcionAF = await db.RecepcionActivoFijo
                        .Include(c=> c.PolizaSeguroActivoFijo).ThenInclude(c=> c.CompaniaSeguro)
                        .Include(c=> c.MotivoAlta)
                        .Include(c=> c.Proveedor)
                        .Include(c=> c.FondoFinanciamiento)
                        .FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == recepcionActivoFijo.IdRecepcionActivoFijo);
                    var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                    var sucursal = await db.Sucursal.FirstOrDefaultAsync(c => c.IdSucursal == claimTransfer.IdSucursal);

                    await emailSender.SendEmailAsync(ConstantesCorreo.CorreoEncargadoSeguro, "Nueva recepción de Activos Fijos.",
                    $@"Se ha guardado una recepción de Activos Fijos en el sistema de Recursos Materiales con los siguientes datos: {System.Environment.NewLine}{System.Environment.NewLine}
                            No. de recepción: {recepcionAF.IdRecepcionActivoFijo}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Motivo de recepción: {recepcionAF.MotivoAlta.Descripcion}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Proveedor: {recepcionAF.Proveedor.RazonSocial}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Fondo de financiamiento: {recepcionAF.FondoFinanciamiento.Nombre}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Fecha de acta de entrega y recepción: {recepcionAF.FechaRecepcion.ToString("dd-MM-yyyy hh:mm tt")}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Orden de compra / Contratos: {recepcionAF.OrdenCompra}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Sucursal: {sucursal.Nombre}, {System.Environment.NewLine}{System.Environment.NewLine}
                            Compañía de seguro: {recepcionAF.PolizaSeguroActivoFijo.CompaniaSeguro.Nombre}");
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = listaRecepcionActivoFijoDetalleTransfer };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarAltaActivoFijo")]
        public async Task<Response> PostAltaActivosFijos([FromBody] AltaActivoFijo altaActivoFijo)
        {
            try
            {
                var nuevaAltaActivoFijo = new AltaActivoFijo();
                using (var transaction = db.Database.BeginTransaction())
                {
                    nuevaAltaActivoFijo.IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo;
                    nuevaAltaActivoFijo.FechaAlta = altaActivoFijo.FechaAlta;
                    nuevaAltaActivoFijo.FechaPago = altaActivoFijo.FechaPago;
                    nuevaAltaActivoFijo.IdMotivoAlta = altaActivoFijo.IdMotivoAlta;

                    if (altaActivoFijo.FacturaActivoFijo != null)
                    {
                        var facturaActivoFijo = new FacturaActivoFijo
                        {
                            NumeroFactura = altaActivoFijo.FacturaActivoFijo.NumeroFactura,
                            FechaFactura = altaActivoFijo.FacturaActivoFijo.FechaFactura
                        };
                        db.FacturaActivoFijo.Add(facturaActivoFijo);
                        await db.SaveChangesAsync();
                        nuevaAltaActivoFijo.IdFacturaActivoFijo = facturaActivoFijo.IdFacturaActivoFijo;
                    }
                    db.AltaActivoFijo.Add(nuevaAltaActivoFijo);
                    await db.SaveChangesAsync();

                    var estadoAlta = await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Alta);
                    foreach (var item in altaActivoFijo.AltaActivoFijoDetalle)
                    {
                        var nuevaUbicacionActivoFijo = new UbicacionActivoFijo
                        {
                            IdEmpleado = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdEmpleado,
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            FechaUbicacion = nuevaAltaActivoFijo.FechaAlta,
                            Confirmacion = true
                        };
                        db.UbicacionActivoFijo.Add(nuevaUbicacionActivoFijo);
                        await db.SaveChangesAsync();

                        var nuevaAltaActivoFijoDetalle = new AltaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdAltaActivoFijo = nuevaAltaActivoFijo.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = item.IdTipoUtilizacionAlta,
                            IdUbicacionActivoFijo = nuevaUbicacionActivoFijo.IdUbicacionActivoFijo,
                            IsComponente = false
                        };
                        db.AltaActivoFijoDetalle.Add(nuevaAltaActivoFijoDetalle);

                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c=> c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        rafd.IdEstado = estadoAlta.IdEstado;
                        await db.SaveChangesAsync();

                        await GestionarComponentesRecepcionActivoFijoDetalle(item.RecepcionActivoFijoDetalle, new UbicacionActivoFijo {
                            IdEmpleado = nuevaUbicacionActivoFijo.IdEmpleado,
                            FechaUbicacion = nuevaAltaActivoFijo.FechaAlta,
                            Confirmacion = true
                        }, isAltaInsertar: true, altaActivoFijoDetalle: nuevaAltaActivoFijoDetalle);
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevaAltaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ReversarAltaActivosFijos")]
        public async Task<Response> PostReversarAltaActivosFijos([FromBody] int idAltaActivoFijo)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var listadoIdRafdAltaActivoFijoDetalles = await db.AltaActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado)
                    .Where(c => c.IdAltaActivoFijo == idAltaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Alta)
                    .Select(c=> c.IdRecepcionActivoFijoDetalle)
                    .ToListAsync();

                    foreach (var item in listadoIdRafdAltaActivoFijoDetalles)
                        EliminarSeccionAlta(item);

                    await db.SaveChangesAsync();
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

        [HttpPost]
        [Route("InsertarBajaActivoFijo")]
        public async Task<Response> PostBajaActivosFijos([FromBody] BajaActivoFijo bajaActivoFijo)
        {
            try
            {
                var nuevaBajaActivoFijo = new BajaActivoFijo();
                using (var transaction = db.Database.BeginTransaction())
                {
                    nuevaBajaActivoFijo.IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo;
                    nuevaBajaActivoFijo.FechaBaja = bajaActivoFijo.FechaBaja;
                    nuevaBajaActivoFijo.IdMotivoBaja = bajaActivoFijo.IdMotivoBaja;
                    nuevaBajaActivoFijo.MemoOficioResolucion = bajaActivoFijo.MemoOficioResolucion;
                    db.BajaActivoFijo.Add(nuevaBajaActivoFijo);
                    await db.SaveChangesAsync();

                    var estadoBaja = await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Baja);
                    foreach (var item in bajaActivoFijo.BajaActivoFijoDetalle)
                    {
                        db.BajaActivoFijoDetalle.Add(new BajaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdBajaActivoFijo = nuevaBajaActivoFijo.IdBajaActivoFijo
                        });
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).Include(c=> c.ComponentesActivoFijoOrigen).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        rafd.IdEstado = estadoBaja.IdEstado;
                        await db.SaveChangesAsync();
                        await GestionarComponentesRecepcionActivoFijoDetalle(rafd, isBajaInsertar: true, bajaActivoFijoDetalle: new BajaActivoFijoDetalle {
                            IdBajaActivoFijo = nuevaBajaActivoFijo.IdBajaActivoFijo
                        });
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevaBajaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        private async Task<TransferenciaActivoFijo> InsertarTransferenciaActivoFijo(string tipoEstado, string tipoMotivoTransferencia, int? idEmpleadoResponsableEnvio = null, int? idEmpleadoResponsableRecibo = null, string observaciones = null)
        {
            var estado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == tipoEstado);
            var motivoTransferencia = await db.MotivoTransferencia.FirstOrDefaultAsync(c => c.Motivo_Transferencia == tipoMotivoTransferencia);
            var nuevaTransferenciaActivoFijo = new TransferenciaActivoFijo
            {
                FechaTransferencia = DateTime.Now,
                IdMotivoTransferencia = motivoTransferencia.IdMotivoTransferencia,
                IdEstado = estado.IdEstado,
                IdEmpleadoResponsableEnvio = idEmpleadoResponsableEnvio,
                IdEmpleadoResponsableRecibo = idEmpleadoResponsableRecibo,
                Observaciones = observaciones
            };
            db.TransferenciaActivoFijo.Add(nuevaTransferenciaActivoFijo);
            await db.SaveChangesAsync();
            return nuevaTransferenciaActivoFijo;
        }
        private async Task InsertarTransferenciaActivoFijoDetalle(TransferenciaActivoFijo transferenciaActivoFijo, ICollection<int> listadoIdRecepcionActivoFijoDetalle, int idEmpleadoEntrega, int idEmpleadoRecibe)
        {
            foreach (var item in listadoIdRecepcionActivoFijoDetalle)
            {
                var ultimaUbicacionOrigen = await db.UbicacionActivoFijo.OrderByDescending(c=> c.FechaUbicacion).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item && c.Confirmacion);
                if (ultimaUbicacionOrigen != null)
                {
                    var nuevaUbicacionDestino = new UbicacionActivoFijo
                    {
                        IdEmpleado = idEmpleadoRecibe,
                        FechaUbicacion = transferenciaActivoFijo.FechaTransferencia,
                        IdRecepcionActivoFijoDetalle = item
                    };

                    if (transferenciaActivoFijo.MotivoTransferencia.Motivo_Transferencia == MotivosTransferencia.CambioCustodio)
                        nuevaUbicacionDestino.Confirmacion = true;

                    db.UbicacionActivoFijo.Add(nuevaUbicacionDestino);
                    await db.SaveChangesAsync();

                    var nuevaTransferenciaActivoFijoDetalle = new TransferenciaActivoFijoDetalle
                    {
                        IdRecepcionActivoFijoDetalle = item,
                        IdTransferenciaActivoFijo = transferenciaActivoFijo.IdTransferenciaActivoFijo,
                        IdUbicacionActivoFijoOrigen = ultimaUbicacionOrigen.IdUbicacionActivoFijo,
                        IdUbicacionActivoFijoDestino = nuevaUbicacionDestino.IdUbicacionActivoFijo
                    };
                    db.TransferenciaActivoFijoDetalle.Add(nuevaTransferenciaActivoFijoDetalle);
                    await db.SaveChangesAsync();

                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).Include(c => c.ComponentesActivoFijoOrigen).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item);
                    await GestionarComponentesRecepcionActivoFijoDetalle(rafd, new UbicacionActivoFijo
                    {
                        IdEmpleado = nuevaUbicacionDestino.IdEmpleado,
                        FechaUbicacion = nuevaUbicacionDestino.FechaUbicacion,
                        Confirmacion = nuevaUbicacionDestino.Confirmacion
                    }, isTransferenciaInsertar: true, transferenciaActivoFijoDetalle: nuevaTransferenciaActivoFijoDetalle);
                }
            }
        }

        [HttpPost]
        [Route("InsertarCambioCustodioActivoFijo")]
        public async Task<Response> PostCambioCustodioActivoFijo([FromBody] CambioCustodioViewModel cambioCustodioViewModel)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var nuevaTransferenciaActivoFijo = await InsertarTransferenciaActivoFijo(Estados.Aceptada, MotivosTransferencia.CambioCustodio, observaciones: cambioCustodioViewModel.Observaciones);
                    await InsertarTransferenciaActivoFijoDetalle(nuevaTransferenciaActivoFijo, cambioCustodioViewModel.ListadoIdRecepcionActivoFijoDetalle, cambioCustodioViewModel.IdEmpleadoEntrega, cambioCustodioViewModel.IdEmpleadoRecibe);
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

        [HttpPost]
        [Route("InsertarCambioUbicacionSucursalActivoFijo")]
        public async Task<Response> PostCambioUbicacionSucursalActivoFijo([FromBody] CambioUbicacionSucursalViewModel cambioUbicacionSucursalViewModel)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var nuevaTransferenciaActivoFijo = await InsertarTransferenciaActivoFijo(Estados.Creada, MotivosTransferencia.CambioUbicacion, cambioUbicacionSucursalViewModel.IdEmpleadoResponsableEnvio, cambioUbicacionSucursalViewModel.IdEmpleadoResponsableRecibo, cambioUbicacionSucursalViewModel.Observaciones);
                    await InsertarTransferenciaActivoFijoDetalle(nuevaTransferenciaActivoFijo, cambioUbicacionSucursalViewModel.ListadoIdRecepcionActivoFijoDetalle, cambioUbicacionSucursalViewModel.IdEmpleadoEntrega, cambioUbicacionSucursalViewModel.IdEmpleadoRecibe);
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

        [HttpPost]
        [Route("InsertarInventarioActivoFijo")]
        public async Task<Response> PostInsertarInventarioActivoFijo([FromBody] InventarioActivoFijo inventarioActivoFijo)
        {
            try
            {
                var nuevoInventarioActivoFijo = new InventarioActivoFijo();
                using (var transaction = db.Database.BeginTransaction())
                {
                    nuevoInventarioActivoFijo.FechaCorteInventario = inventarioActivoFijo.FechaCorteInventario;
                    nuevoInventarioActivoFijo.FechaInforme = inventarioActivoFijo.FechaInforme;
                    nuevoInventarioActivoFijo.NumeroInforme = inventarioActivoFijo.NumeroInforme;
                    nuevoInventarioActivoFijo.IdEstado = inventarioActivoFijo.IdEstado;
                    nuevoInventarioActivoFijo.InventarioManual = inventarioActivoFijo.InventarioManual;
                    db.InventarioActivoFijo.Add(nuevoInventarioActivoFijo);
                    await db.SaveChangesAsync();

                    foreach (var item in inventarioActivoFijo.InventarioActivoFijoDetalle)
                    {
                        db.InventarioActivoFijoDetalle.Add(new InventarioActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdInventarioActivoFijo = nuevoInventarioActivoFijo.IdInventarioActivoFijo,
                            Constatado = item.Constatado
                        });
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevoInventarioActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarMovilizacionActivoFijo")]
        public async Task<Response> PostInsertarMovilizacionActivoFijo([FromBody] MovilizacionActivoFijo movilizacionActivoFijo)
        {
            try
            {
                var nuevaMovilizacionActivoFijo = new MovilizacionActivoFijo();
                using (var transaction = db.Database.BeginTransaction())
                {
                    nuevaMovilizacionActivoFijo.IdEmpleadoAutorizado = movilizacionActivoFijo.IdEmpleadoAutorizado;
                    nuevaMovilizacionActivoFijo.IdEmpleadoResponsable = movilizacionActivoFijo.IdEmpleadoResponsable;
                    nuevaMovilizacionActivoFijo.IdEmpleadoSolicita = movilizacionActivoFijo.IdEmpleadoSolicita;
                    nuevaMovilizacionActivoFijo.FechaSalida = movilizacionActivoFijo.FechaSalida;
                    nuevaMovilizacionActivoFijo.FechaRetorno = movilizacionActivoFijo.FechaRetorno;
                    nuevaMovilizacionActivoFijo.IdMotivoTraslado = movilizacionActivoFijo.IdMotivoTraslado;
                    db.MovilizacionActivoFijo.Add(nuevaMovilizacionActivoFijo);
                    await db.SaveChangesAsync();

                    foreach (var item in movilizacionActivoFijo.MovilizacionActivoFijoDetalle)
                    {
                        db.MovilizacionActivoFijoDetalle.Add(new MovilizacionActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdMovilizacionActivoFijo = nuevaMovilizacionActivoFijo.IdMovilizacionActivoFijo,
                            Observaciones = item.Observaciones,
                        });
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = nuevaMovilizacionActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("AprobacionActivoFijo")]
        public async Task<Response> PostAprobacionActivoFijo([FromBody] AprobacionActivoFijoTransfer aprobacionActivoFijoTransfer)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var estado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == aprobacionActivoFijoTransfer.NuevoEstadoActivoFijo);
                    foreach (var item in aprobacionActivoFijoTransfer.IdsActivoFijo)
                    {
                        var activoFijo = await db.ActivoFijo.FirstOrDefaultAsync(c => c.IdActivoFijo == item);
                        activoFijo.ValidacionTecnica = aprobacionActivoFijoTransfer.ValidacionTecnica;
                        await db.SaveChangesAsync();

                        var recepcionesActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == item).ToListAsync();
                        foreach (var rafd in recepcionesActivoFijoDetalle)
                        {
                            rafd.IdEstado = estado.IdEstado;
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

        [HttpPost]
        [Route("AprobacionTransferenciaCambioUbicacionActivoFijo")]
        public async Task<Response> PostAprobacionTransferenciaCambioUbicacionActivoFijo([FromBody] TransferenciaActivoFijoTransfer transferenciaActivoFijoTransfer)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    string tipoEstado = transferenciaActivoFijoTransfer.Aprobado ? Estados.Aceptada : Estados.Desaprobado;
                    var estado = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == tipoEstado);
                    var transferenciaActivoFijoActualizar = await db.TransferenciaActivoFijo.Include(c => c.Estado).FirstOrDefaultAsync(c => c.IdTransferenciaActivoFijo == transferenciaActivoFijoTransfer.IdTransferenciaActivoFijo);
                    if (transferenciaActivoFijoActualizar != null)
                    {
                        transferenciaActivoFijoActualizar.IdEstado = estado.IdEstado;
                        db.TransferenciaActivoFijo.Update(transferenciaActivoFijoActualizar);
                        await db.SaveChangesAsync();

                        if (transferenciaActivoFijoTransfer.Aprobado)
                        {
                            var transferenciasActivoFijoDetalle = await db.TransferenciaActivoFijoDetalle.Include(c=> c.RecepcionActivoFijoDetalle).ThenInclude(c=> c.CodigoActivoFijo).Include(c => c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c=> c.Bodega).ThenInclude(c=> c.Sucursal).Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c=> c.Empleado).ThenInclude(c=> c.Dependencia).ThenInclude(c=> c.Sucursal).Where(c => c.IdTransferenciaActivoFijo == transferenciaActivoFijoTransfer.IdTransferenciaActivoFijo).ToListAsync();
                            foreach (var item in transferenciasActivoFijoDetalle)
                            {
                                var ultimaTransferencia = db.TransferenciaActivoFijoDetalle.Include(c => c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).LastOrDefault(c => c.UbicacionActivoFijoDestino.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdCodigoActivoFijo != null);
                                var ultimoCodigoActivoFijo = ultimaTransferencia?.CodigoActivoFijo ?? item.RecepcionActivoFijoDetalle.CodigoActivoFijo;

                                var arrCodigoActivoFijo = ultimoCodigoActivoFijo.Codigosecuencial.Split('.').ToList();
                                string codigoFinal = String.Join(".", arrCodigoActivoFijo.GetRange(1, arrCodigoActivoFijo.Count - 1));
                                int idSucursal = item?.UbicacionActivoFijoDestino?.Empleado?.Dependencia?.IdSucursal ?? item.UbicacionActivoFijoDestino.Bodega.IdSucursal;
                                var nuevoCodigoSecuencial = $"{idSucursal}.{codigoFinal}";

                                if (item.CodigoActivoFijo == null)
                                {
                                    var nuevoCodigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = nuevoCodigoSecuencial };
                                    db.CodigoActivoFijo.Add(nuevoCodigoActivoFijo);
                                    await db.SaveChangesAsync();

                                    item.IdCodigoActivoFijo = nuevoCodigoActivoFijo.IdCodigoActivoFijo;
                                    await db.SaveChangesAsync();
                                }
                                else
                                {
                                    if (nuevoCodigoSecuencial != item.CodigoActivoFijo.Codigosecuencial)
                                    {
                                        item.CodigoActivoFijo.Codigosecuencial = nuevoCodigoSecuencial;
                                        await db.SaveChangesAsync();
                                    }
                                }
                                item.UbicacionActivoFijoDestino.Confirmacion = true;
                                await db.SaveChangesAsync();
                            }
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

        [HttpPost]
        [Route("ValidacionRecepcionActivoFijoDetalleDatosEspecificos")]
        public async Task<Response> PostValidacionRecepcionActivoFijoDetalleDatosEspecificos([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var listaPropiedadValorErrores = new List<PropiedadValor>();
                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.Serie))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.Serie.ToUpper().Trim() == recepcionActivoFijoDetalle.Serie.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Serie", Valor = "La Serie: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.Serie.ToUpper().Trim() == recepcionActivoFijoDetalle.Serie.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Serie", Valor = "La Serie: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroChasis))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.AnyAsync(c => c.NumeroChasis.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroChasis.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El Número de chasis: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.Where(c => c.NumeroChasis.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroChasis.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El Número de chasis: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroMotor))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.AnyAsync(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroMotor.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El Número de motor: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.Where(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.NumeroMotor.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El Número de motor: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.Placa))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.AnyAsync(c => c.Placa.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.Placa.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Placa", Valor = "La Placa: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalleVehiculo.Where(c => c.Placa.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleVehiculo.Placa.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Placa", Valor = "La Placa: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalleEdificio.AnyAsync(c => c.NumeroClaveCatastral.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroClaveCatastral", Valor = "El Número de clave catastral: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalleEdificio.Where(c => c.NumeroClaveCatastral.ToUpper().Trim() == recepcionActivoFijoDetalle.RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroClaveCatastral", Valor = "El Número de clave catastral: ya existe." });
                    }
                }

                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = listaPropiedadValorErrores };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutActivosFijos([FromRoute] int id, [FromBody] List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalleTransfer)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var recepcionActivoFijoActualizar = await db.RecepcionActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo);
                    if (recepcionActivoFijoActualizar != null)
                    {
                        recepcionActivoFijoActualizar.FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion;
                        recepcionActivoFijoActualizar.IdFondoFinanciamiento = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdFondoFinanciamiento;
                        recepcionActivoFijoActualizar.OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra;
                        recepcionActivoFijoActualizar.IdMotivoAlta = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoAlta;
                        recepcionActivoFijoActualizar.IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor;
                        db.RecepcionActivoFijo.Update(recepcionActivoFijoActualizar);
                    }
                    var polizaSeguroActivoFijoActualizar = await db.PolizaSeguroActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo);
                    if (polizaSeguroActivoFijoActualizar != null)
                    {
                        polizaSeguroActivoFijoActualizar.IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro;
                        db.PolizaSeguroActivoFijo.Update(polizaSeguroActivoFijoActualizar);
                    }
                    await db.SaveChangesAsync();

                    var igrouping = listaRecepcionActivoFijoDetalleTransfer.GroupBy(c => new { c.ActivoFijo.IdSubClaseActivoFijo, c.ActivoFijo.IdModelo, c.ActivoFijo.Nombre }).ToList();
                    if (igrouping.Count > 0)
                    {
                        var listaActivosFijosEliminar = await (db.RecepcionActivoFijoDetalle.Include(c => c.Estado)
                            .Where(c => c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo
                            && (c.Estado.Nombre == Estados.Recepcionado || c.Estado.Nombre == Estados.ValidacionTecnica))
                            .Select(c => c.IdRecepcionActivoFijoDetalle)
                            .Except(listaRecepcionActivoFijoDetalleTransfer.Where(c => c.IdActivoFijo > 0).Select(c => c.IdRecepcionActivoFijoDetalle).Distinct()).ToListAsync());

                        foreach (var item in listaActivosFijosEliminar)
                            await DeleteDetalleActivoFijo(item);
                    }

                    foreach (var item in igrouping)
                    {
                        var listadoRafd = item.ToList();
                        var listaDetalleActivosFijosEliminar = await db.RecepcionActivoFijoDetalle.Include(c => c.Estado).Where(c => (c.Estado.Nombre.ToUpper() == Estados.Recepcionado || c.Estado.Nombre.ToUpper() == Estados.ValidacionTecnica) && c.IdActivoFijo == listadoRafd[0].IdActivoFijo).Select(c => c.IdRecepcionActivoFijoDetalle).Except(listadoRafd.Where(c => c.IdRecepcionActivoFijoDetalle > 0).Select(c => c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                        foreach (var item1 in listaDetalleActivosFijosEliminar)
                        {
                            await DeleteDetalleActivoFijo(item1);
                            listadoRafd.Remove(listadoRafd.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item1));
                        }
                        var activoFijoActualizar = await db.ActivoFijo.FirstOrDefaultAsync(c => c.IdActivoFijo == listadoRafd[0].IdActivoFijo);
                        if (activoFijoActualizar != null)
                        {
                            activoFijoActualizar.Nombre = listadoRafd[0].ActivoFijo.Nombre;
                            activoFijoActualizar.ValorCompra = listadoRafd[0].ActivoFijo.ValorCompra;
                            activoFijoActualizar.IdSubClaseActivoFijo = listadoRafd[0].ActivoFijo.IdSubClaseActivoFijo;
                            activoFijoActualizar.IdModelo = listadoRafd[0].ActivoFijo.IdModelo;
                            activoFijoActualizar.Depreciacion = listadoRafd[0].ActivoFijo.Depreciacion;
                            activoFijoActualizar.ValidacionTecnica = listadoRafd[0].ActivoFijo.ValidacionTecnica;
                            db.ActivoFijo.Update(activoFijoActualizar);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            activoFijoActualizar = new ActivoFijo
                            {
                                IdSubClaseActivoFijo = listadoRafd[0].ActivoFijo.IdSubClaseActivoFijo,
                                IdModelo = listadoRafd[0].ActivoFijo.IdModelo,
                                Nombre = listadoRafd[0].ActivoFijo.Nombre,
                                ValorCompra = listadoRafd[0].ActivoFijo.ValorCompra,
                                Depreciacion = listadoRafd[0].ActivoFijo.Depreciacion,
                                ValidacionTecnica = listadoRafd[0].ActivoFijo.ValidacionTecnica
                            };
                            db.ActivoFijo.Add(activoFijoActualizar);
                            await db.SaveChangesAsync();
                        }

                        for (int i = 0; i < listadoRafd.Count; i++)
                        {
                            if (listadoRafd[i].IdRecepcionActivoFijoDetalle == 0)
                            {
                                var codigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = listadoRafd[i].CodigoActivoFijo.Codigosecuencial };
                                db.CodigoActivoFijo.Add(codigoActivoFijo);
                                await db.SaveChangesAsync();

                                var nuevaRecepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle
                                {
                                    IdRecepcionActivoFijo = listadoRafd[i].IdRecepcionActivoFijo,
                                    IdActivoFijo = activoFijoActualizar.IdActivoFijo,
                                    IdEstado = listadoRafd[i].IdEstado,
                                    IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                                    Serie = listadoRafd[i].Serie
                                };
                                db.RecepcionActivoFijoDetalle.Add(nuevaRecepcionActivoFijoDetalle);
                                await db.SaveChangesAsync();

                                if (listadoRafd[i].RecepcionActivoFijoDetalleEdificio != null)
                                {
                                    var nuevaRecepcionActivoFijoDetalleEdificio = new RecepcionActivoFijoDetalleEdificio
                                    {
                                        IdRecepcionActivoFijoDetalle = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                        NumeroClaveCatastral = listadoRafd[i].RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral
                                    };
                                    db.RecepcionActivoFijoDetalleEdificio.Add(nuevaRecepcionActivoFijoDetalleEdificio);
                                }

                                if (listadoRafd[i].RecepcionActivoFijoDetalleVehiculo != null)
                                {
                                    var nuevaRecepcionActivoFijoDetalleVehiculo = new RecepcionActivoFijoDetalleVehiculo
                                    {
                                        IdRecepcionActivoFijoDetalle = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                        NumeroChasis = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroChasis,
                                        NumeroMotor = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroMotor,
                                        Placa = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.Placa
                                    };
                                    db.RecepcionActivoFijoDetalleVehiculo.Add(nuevaRecepcionActivoFijoDetalleVehiculo);
                                }

                                db.UbicacionActivoFijo.Add(new UbicacionActivoFijo
                                {
                                    IdEmpleado = listadoRafd[i].UbicacionActivoFijoActual.IdEmpleado,
                                    IdBodega = listadoRafd[i].UbicacionActivoFijoActual.IdBodega,
                                    FechaUbicacion = listadoRafd[i].UbicacionActivoFijoActual.FechaUbicacion,
                                    IdRecepcionActivoFijoDetalle = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                                    Confirmacion = true
                                });
                                await db.SaveChangesAsync();

                                foreach (var comp in listadoRafd[i].ComponentesActivoFijoOrigen)
                                {
                                    comp.IdRecepcionActivoFijoDetalleOrigen = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle;
                                    db.ComponenteActivoFijo.Add(comp);
                                }
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                var codigoActivoFijoActualizar = await db.CodigoActivoFijo.FirstOrDefaultAsync(c => c.IdCodigoActivoFijo == listadoRafd[i].IdCodigoActivoFijo);
                                if (codigoActivoFijoActualizar != null)
                                {
                                    codigoActivoFijoActualizar.Codigosecuencial = listadoRafd[i].CodigoActivoFijo.Codigosecuencial;
                                    db.CodigoActivoFijo.Update(codigoActivoFijoActualizar);
                                }

                                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == listadoRafd[i].IdRecepcionActivoFijoDetalle);
                                if (recepcionActivoFijoDetalleActualizar != null)
                                {
                                    recepcionActivoFijoDetalleActualizar.IdRecepcionActivoFijo = listadoRafd[i].IdRecepcionActivoFijo;
                                    recepcionActivoFijoDetalleActualizar.IdActivoFijo = listadoRafd[i].IdActivoFijo;
                                    recepcionActivoFijoDetalleActualizar.IdEstado = listadoRafd[i].IdEstado;
                                    recepcionActivoFijoDetalleActualizar.IdCodigoActivoFijo = listadoRafd[i].IdCodigoActivoFijo;
                                    recepcionActivoFijoDetalleActualizar.Serie = listadoRafd[i].Serie;
                                    db.Update(recepcionActivoFijoDetalleActualizar);
                                }

                                db.RecepcionActivoFijoDetalleEdificio.RemoveRange(db.RecepcionActivoFijoDetalleEdificio.Where(c => c.IdRecepcionActivoFijoDetalle == listadoRafd[i].IdRecepcionActivoFijoDetalle));
                                db.RecepcionActivoFijoDetalleVehiculo.RemoveRange(db.RecepcionActivoFijoDetalleVehiculo.Where(c => c.IdRecepcionActivoFijoDetalle == listadoRafd[i].IdRecepcionActivoFijoDetalle));
                                await db.SaveChangesAsync();

                                var rafdDetalleEdificioActualizar = await db.RecepcionActivoFijoDetalleEdificio.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == listadoRafd[i].IdRecepcionActivoFijoDetalle);
                                if (rafdDetalleEdificioActualizar != null && listadoRafd[i].RecepcionActivoFijoDetalleEdificio != null)
                                {
                                    rafdDetalleEdificioActualizar.NumeroClaveCatastral = listadoRafd[i].RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral;
                                    db.Update(rafdDetalleEdificioActualizar);
                                    await db.SaveChangesAsync();
                                }
                                else
                                {
                                    if (listadoRafd[i].RecepcionActivoFijoDetalleEdificio != null)
                                    {
                                        var nuevaRecepcionActivoFijoDetalleEdificio = new RecepcionActivoFijoDetalleEdificio
                                        {
                                            IdRecepcionActivoFijoDetalle = listadoRafd[i].IdRecepcionActivoFijoDetalle,
                                            NumeroClaveCatastral = listadoRafd[i].RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral
                                        };
                                        db.RecepcionActivoFijoDetalleEdificio.Add(nuevaRecepcionActivoFijoDetalleEdificio);
                                        await db.SaveChangesAsync();
                                    }
                                }

                                var rafdDetalleVehiculoActualizar = await db.RecepcionActivoFijoDetalleVehiculo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == listadoRafd[i].IdRecepcionActivoFijoDetalle);
                                if (rafdDetalleVehiculoActualizar != null && listadoRafd[i].RecepcionActivoFijoDetalleVehiculo != null)
                                {
                                    rafdDetalleVehiculoActualizar.NumeroChasis = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroChasis;
                                    rafdDetalleVehiculoActualizar.NumeroMotor = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroMotor;
                                    rafdDetalleVehiculoActualizar.Placa = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.Placa;
                                    db.Update(rafdDetalleVehiculoActualizar);
                                    await db.SaveChangesAsync();
                                }
                                else
                                {
                                    if (listadoRafd[i].RecepcionActivoFijoDetalleVehiculo != null)
                                    {
                                        var nuevaRecepcionActivoFijoDetalleVehiculo = new RecepcionActivoFijoDetalleVehiculo
                                        {
                                            IdRecepcionActivoFijoDetalle = listadoRafd[i].IdRecepcionActivoFijoDetalle,
                                            NumeroChasis = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroChasis,
                                            NumeroMotor = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.NumeroMotor,
                                            Placa = listadoRafd[i].RecepcionActivoFijoDetalleVehiculo.Placa
                                        };
                                        db.RecepcionActivoFijoDetalleVehiculo.Add(nuevaRecepcionActivoFijoDetalleVehiculo);
                                        await db.SaveChangesAsync();
                                    }
                                }

                                var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == listadoRafd[i].UbicacionActivoFijoActual.IdUbicacionActivoFijo && c.Confirmacion);
                                if (ubicacionActivoFijoActualizar != null)
                                {
                                    ubicacionActivoFijoActualizar.IdEmpleado = listadoRafd[i].UbicacionActivoFijoActual.IdEmpleado;
                                    ubicacionActivoFijoActualizar.IdBodega = listadoRafd[i].UbicacionActivoFijoActual.IdBodega;
                                    ubicacionActivoFijoActualizar.IdRecepcionActivoFijoDetalle = listadoRafd[i].IdRecepcionActivoFijoDetalle;
                                    ubicacionActivoFijoActualizar.FechaUbicacion = listadoRafd[i].UbicacionActivoFijoActual.FechaUbicacion;
                                    db.Update(ubicacionActivoFijoActualizar);
                                    await db.SaveChangesAsync();
                                }
                                await GestionarComponentesRecepcionActivoFijoDetalle(listadoRafd[i], new UbicacionActivoFijo {
                                    IdEmpleado = listadoRafd[i].UbicacionActivoFijoActual.IdEmpleado,
                                    IdBodega = listadoRafd[i].UbicacionActivoFijoActual.IdBodega,
                                    FechaUbicacion = listadoRafd[i].UbicacionActivoFijoActual.FechaUbicacion,
                                    Confirmacion = true
                                }, isRecepcion: true);
                            }
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = listaRecepcionActivoFijoDetalleTransfer };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarAltaActivoFijo/{id}")]
        public async Task<Response> PutAltaActivosFijos([FromRoute] int id, [FromBody] AltaActivoFijo altaActivoFijo)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var altaActivoFijoActualizar = await db.AltaActivoFijo.Include(c=> c.FacturaActivoFijo).FirstOrDefaultAsync(c => c.IdAltaActivoFijo == id);
                    if (altaActivoFijoActualizar != null)
                    {
                        altaActivoFijoActualizar.IdMotivoAlta = altaActivoFijo.IdMotivoAlta;
                        altaActivoFijoActualizar.FechaAlta = altaActivoFijo.FechaAlta;
                        altaActivoFijoActualizar.FechaPago = altaActivoFijo.FechaPago;

                        if (altaActivoFijoActualizar.FacturaActivoFijo != null && altaActivoFijo.FacturaActivoFijo == null)
                        {
                            db.FacturaActivoFijo.Remove(altaActivoFijoActualizar.FacturaActivoFijo);
                            await db.SaveChangesAsync();
                            altaActivoFijoActualizar.IdFacturaActivoFijo = null;
                        }
                        else if (altaActivoFijoActualizar.FacturaActivoFijo != null && altaActivoFijo.FacturaActivoFijo != null)
                        {
                            altaActivoFijoActualizar.FacturaActivoFijo.NumeroFactura = altaActivoFijo.FacturaActivoFijo.NumeroFactura;
                            altaActivoFijoActualizar.FacturaActivoFijo.FechaFactura = altaActivoFijo.FacturaActivoFijo.FechaFactura;
                            db.FacturaActivoFijo.Update(altaActivoFijoActualizar.FacturaActivoFijo);
                            await db.SaveChangesAsync();
                            altaActivoFijoActualizar.IdFacturaActivoFijo = altaActivoFijo.IdFacturaActivoFijo;
                        }
                        else if (altaActivoFijoActualizar.FacturaActivoFijo == null && altaActivoFijo.FacturaActivoFijo != null)
                        {
                            var facturaActivoFijo = new FacturaActivoFijo
                            {
                                NumeroFactura = altaActivoFijo.FacturaActivoFijo.NumeroFactura,
                                FechaFactura = altaActivoFijo.FacturaActivoFijo.FechaFactura
                            };
                            db.FacturaActivoFijo.Add(facturaActivoFijo);
                            await db.SaveChangesAsync();
                            altaActivoFijoActualizar.IdFacturaActivoFijo = facturaActivoFijo.IdFacturaActivoFijo;
                        }
                        db.AltaActivoFijo.Update(altaActivoFijoActualizar);
                        await db.SaveChangesAsync();

                        foreach (var item in altaActivoFijo.AltaActivoFijoDetalle)
                        {
                            var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo && c.Confirmacion);
                            if (ubicacionActivoFijoActualizar != null)
                            {
                                ubicacionActivoFijoActualizar.IdEmpleado = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdEmpleado;
                                ubicacionActivoFijoActualizar.FechaUbicacion = altaActivoFijo.FechaAlta;
                                db.UbicacionActivoFijo.Update(ubicacionActivoFijoActualizar);
                                await db.SaveChangesAsync();
                            }
                            await GestionarComponentesRecepcionActivoFijoDetalle(item.RecepcionActivoFijoDetalle, new UbicacionActivoFijo
                            {
                                IdEmpleado = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdEmpleado,
                                FechaUbicacion = altaActivoFijo.FechaAlta,
                                Confirmacion = true
                            }, isAltaEditar: true);
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = altaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarBajaActivoFijo/{id}")]
        public async Task<Response> PutBajaActivosFijos([FromRoute] int id, [FromBody] BajaActivoFijo bajaActivoFijo)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var bajaActivoFijoActualizar = await db.BajaActivoFijo.FirstOrDefaultAsync(c => c.IdBajaActivoFijo == id);
                    if (bajaActivoFijoActualizar != null)
                    {
                        bajaActivoFijoActualizar.IdMotivoBaja = bajaActivoFijo.IdMotivoBaja;
                        bajaActivoFijoActualizar.FechaBaja = bajaActivoFijo.FechaBaja;
                        bajaActivoFijoActualizar.MemoOficioResolucion = bajaActivoFijo.MemoOficioResolucion;

                        db.BajaActivoFijo.Update(bajaActivoFijoActualizar);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = bajaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarCambioUbicacionSucursalActivoFijo/{id}")]
        public async Task<Response> PutCambioUbicacionSucursalActivoFijo([FromRoute] int id, [FromBody] CambioUbicacionSucursalViewModel cambioUbicacionSucursalViewModel)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var transferenciaActivoFijoActualizar = await db.TransferenciaActivoFijo.FirstOrDefaultAsync(c => c.IdTransferenciaActivoFijo == id);
                    if (transferenciaActivoFijoActualizar != null)
                    {
                        transferenciaActivoFijoActualizar.IdEmpleadoResponsableEnvio = cambioUbicacionSucursalViewModel.IdEmpleadoResponsableEnvio;
                        transferenciaActivoFijoActualizar.IdEmpleadoResponsableRecibo = cambioUbicacionSucursalViewModel.IdEmpleadoResponsableRecibo;
                        transferenciaActivoFijoActualizar.FechaTransferencia = cambioUbicacionSucursalViewModel.FechaTransferencia;
                        transferenciaActivoFijoActualizar.Observaciones = cambioUbicacionSucursalViewModel.Observaciones;
                        db.TransferenciaActivoFijo.Update(transferenciaActivoFijoActualizar);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = cambioUbicacionSucursalViewModel };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarInventarioActivoFijo/{id}")]
        public async Task<Response> PutInventarioActivoFijo([FromRoute] int id, [FromBody] InventarioActivoFijo inventarioActivoFijo)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var inventarioActivoFijoActualizar = await db.InventarioActivoFijo.FirstOrDefaultAsync(c => c.IdInventarioActivoFijo == id);
                    if (inventarioActivoFijoActualizar != null)
                    {
                        inventarioActivoFijoActualizar.FechaCorteInventario = inventarioActivoFijo.FechaCorteInventario;
                        inventarioActivoFijoActualizar.FechaInforme = inventarioActivoFijo.FechaInforme;
                        inventarioActivoFijoActualizar.NumeroInforme = inventarioActivoFijo.NumeroInforme;
                        inventarioActivoFijoActualizar.IdEstado = inventarioActivoFijo.IdEstado;
                        inventarioActivoFijoActualizar.InventarioManual = inventarioActivoFijo.InventarioManual;
                        db.InventarioActivoFijo.Update(inventarioActivoFijoActualizar);
                        await db.SaveChangesAsync();

                        foreach (var item in inventarioActivoFijo.InventarioActivoFijoDetalle)
                        {
                            var inventarioActivoFijoDetalle = await db.InventarioActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdInventarioActivoFijo == item.IdInventarioActivoFijo && c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                            if (inventarioActivoFijoDetalle == null)
                            {
                                db.InventarioActivoFijoDetalle.Add(new InventarioActivoFijoDetalle
                                {
                                    IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                                    IdInventarioActivoFijo = inventarioActivoFijo.IdInventarioActivoFijo,
                                    Constatado = item.Constatado
                                });
                            }
                            else
                            {
                                inventarioActivoFijoDetalle.Constatado = item.Constatado;
                                db.InventarioActivoFijoDetalle.Update(inventarioActivoFijoDetalle);
                            }
                            await db.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = inventarioActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EditarMovilizacionActivoFijo/{id}")]
        public async Task<Response> PutMovilizacionActivoFijo([FromRoute] int id, [FromBody] MovilizacionActivoFijo movilizacionActivoFijo)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var movilizacionActivoFijoActualizar = await db.MovilizacionActivoFijo.FirstOrDefaultAsync(c => c.IdMovilizacionActivoFijo == id);
                    if (movilizacionActivoFijoActualizar != null)
                    {
                        movilizacionActivoFijoActualizar.IdEmpleadoAutorizado = movilizacionActivoFijo.IdEmpleadoAutorizado;
                        movilizacionActivoFijoActualizar.IdEmpleadoResponsable = movilizacionActivoFijo.IdEmpleadoResponsable;
                        movilizacionActivoFijoActualizar.IdEmpleadoSolicita = movilizacionActivoFijo.IdEmpleadoSolicita;
                        movilizacionActivoFijoActualizar.FechaSalida = movilizacionActivoFijo.FechaSalida;
                        movilizacionActivoFijoActualizar.FechaRetorno = movilizacionActivoFijo.FechaRetorno;
                        movilizacionActivoFijoActualizar.IdMotivoTraslado = movilizacionActivoFijo.IdMotivoTraslado;
                        db.MovilizacionActivoFijo.Update(movilizacionActivoFijoActualizar);
                        await db.SaveChangesAsync();

                        var listaExcept = await db.MovilizacionActivoFijoDetalle.Where(c => c.IdMovilizacionActivoFijo == movilizacionActivoFijo.IdMovilizacionActivoFijo).Select(c => c.IdRecepcionActivoFijoDetalle).Except(movilizacionActivoFijo.MovilizacionActivoFijoDetalle.Select(c => c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                        foreach (var item in listaExcept)
                        {
                            var movilizacionActivoFijoDetalle = await db.MovilizacionActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item && c.IdMovilizacionActivoFijo == movilizacionActivoFijo.IdMovilizacionActivoFijo);
                            db.MovilizacionActivoFijoDetalle.Remove(movilizacionActivoFijoDetalle);
                            await db.SaveChangesAsync();
                        }

                        foreach (var item in movilizacionActivoFijo.MovilizacionActivoFijoDetalle)
                        {
                            var movilizacionActivoFijoDetalle = await db.MovilizacionActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdMovilizacionActivoFijo == item.IdMovilizacionActivoFijo && c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                            if (movilizacionActivoFijoDetalle == null)
                            {
                                db.MovilizacionActivoFijoDetalle.Add(new MovilizacionActivoFijoDetalle
                                {
                                    IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                                    IdMovilizacionActivoFijo = movilizacionActivoFijo.IdMovilizacionActivoFijo,
                                    Observaciones = item.Observaciones
                                });
                            }
                            else
                            {
                                movilizacionActivoFijoDetalle.Observaciones = item.Observaciones;
                                db.MovilizacionActivoFijoDetalle.Update(movilizacionActivoFijoDetalle);
                            }
                            await db.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = movilizacionActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteActivosFijos([FromRoute] int id)
        {
            try
            {
                var idsRecepcionesActivosFijosDetalles = await db.RecepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == id).Select(c => c.IdRecepcionActivoFijoDetalle).ToListAsync();
                foreach (var item in idsRecepcionesActivosFijosDetalles)
                {
                    var response = await DeleteDetalleActivoFijo(item);
                    if (!response.IsSuccess)
                        return new Response { IsSuccess = false, Message = Mensaje.BorradoNoSatisfactorio };
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpDelete]
        [Route("EliminarDetalleActivoFijo/{id}")]
        public async Task<Response> DeleteDetalleActivoFijo([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.RecepcionActivoFijoDetalle
                    .FirstOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);

                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
                
                db.ComponenteActivoFijo.RemoveRange(db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleComponente == id || c.IdRecepcionActivoFijoDetalleOrigen == id));
                EliminarSeccionAlta(id);
                EliminarBajaPorIdRecepcionActivoFijoDetalle(id);
                EliminarRecepcionActivoFijoDetalleEdificioPorIdRecepcionActivoFijoDetalle(id);
                EliminarRecepcionActivoFijoDetalleVehiculoPorIdRecepcionActivoFijoDetalle(id);
                EliminarUbicacionPorIdRecepcionActivoFijoDetalle(id);

                if (db.RecepcionActivoFijoDetalle.Count(c => c.IdRecepcionActivoFijo == respuesta.IdRecepcionActivoFijo) == 1)
                {
                    EliminarPolizaSeguroPorIdRecepcionActivoFijo(respuesta.IdRecepcionActivoFijo);
                    EliminarActivoFijo(respuesta.IdActivoFijo);
                    EliminarRecepcionActivoFijo(respuesta.IdRecepcionActivoFijo);
                }
                EliminarRecepcionActivoFijoDetalle(id);
                EliminarCodigoActivoFijo(respuesta.IdCodigoActivoFijo);
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
        [Route("EliminarRecepcionActivoFijo/{id}")]
        public async Task<Response> DeleteRecepcionActivoFijo([FromRoute] int id)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var idsRecepcionesActivosFijosDetalles = await db.RecepcionActivoFijoDetalle.Where(c => c.IdRecepcionActivoFijo == id && (c.Estado.Nombre == Estados.Recepcionado || c.Estado.Nombre == Estados.ValidacionTecnica)).Select(c => c.IdRecepcionActivoFijoDetalle).ToListAsync();
                    foreach (var item in idsRecepcionesActivosFijosDetalles)
                    {
                        var response = await DeleteDetalleActivoFijo(item);
                        if (!response.IsSuccess)
                            return new Response { IsSuccess = false, Message = Mensaje.BorradoNoSatisfactorio };
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

        [HttpDelete]
        [Route("EliminarMovilizacionActivoFijo/{id}")]
        public async Task<Response> DeleteMovilizacionActivoFijo([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.MovilizacionActivoFijo.FirstOrDefaultAsync(m => m.IdMovilizacionActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MovilizacionActivoFijoDetalle.RemoveRange(db.MovilizacionActivoFijoDetalle.Where(c => c.IdMovilizacionActivoFijo == id));
                db.MovilizacionActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ActivoFijo _ActivosFijos)
        {
            var bdd = _ActivosFijos.IdActivoFijo;
            var loglevelrespuesta = db.ActivoFijo.Where(p => p.IdActivoFijo == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }

        private RecepcionActivoFijoDetalle ObtenerRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle rafdOld, bool? incluirComponentes = null, bool? incluirActivoFijo = null)
        {
            var recepcionActivoFijoDetalle = clonacionService.ClonarRecepcionActivoFijoDetalle(rafdOld);
            if (incluirComponentes != null)
            {
                if ((bool)incluirComponentes)
                    recepcionActivoFijoDetalle.ComponentesActivoFijoOrigen = clonacionService.ClonarListadoComponenteActivoFijo(db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == rafdOld.IdRecepcionActivoFijoDetalle).ToList());
            }
            if (incluirActivoFijo != null)
            {
                if ((bool)incluirActivoFijo)
                    recepcionActivoFijoDetalle.ActivoFijo = clonacionService.ClonarActivoFijo(rafdOld.ActivoFijo, new List<RecepcionActivoFijoDetalle>());
            }
            return recepcionActivoFijoDetalle;
        }

        #region IQueryable<T> Datos Comunes
        private IQueryable<ActivoFijo> ObtenerDatosActivoFijo()
        {
            return db.ActivoFijo
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c=> c.CategoriaActivoFijo)
                    .Include(c=> c.SubClaseActivoFijo).ThenInclude(c=> c.Subramo).ThenInclude(c=> c.Ramo)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .OrderBy(c=> c.IdSubClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.IdClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.IdSubramo)
                    .ThenBy(c=> c.SubClaseActivoFijo.Subramo.IdRamo)
                    .ThenBy(c=> c.Nombre);
        }
        private IQueryable<RecepcionActivoFijoDetalle> ObtenerListadoDetallesActivosFijos(int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null, bool incluirClaimsTransferencia = true)
        {
            var recepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoAlta)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.CompaniaSeguro)
                    .Include(c=> c.RecepcionActivoFijoDetalleEdificio)
                    .Include(c=> c.RecepcionActivoFijoDetalleVehiculo)
                    .Include(c => c.Estado)
                    .OrderBy(c => c.RecepcionActivoFijo.IdProveedor)
                    .ThenBy(c => c.RecepcionActivoFijo.FechaRecepcion)
                    .ThenBy(c => c.Serie)
                    .ThenBy(c => c.RecepcionActivoFijo.MotivoAlta)
                    .ThenBy(c => c.RecepcionActivoFijo.FondoFinanciamiento)
                    .ThenBy(c => c.Estado.Nombre);

            var claimsTransferencia = claimsTransfer.ObtenerClaimsTransferHttpContext();
            foreach (var item in recepcionActivoFijoDetalle)
            {
                item.UbicacionActivoFijoActual = clonacionService.ClonarUbicacionActivoFijo(db.UbicacionActivoFijo
                    .Include(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c=> c.Empleado).ThenInclude(c=> c.Dependencia).ThenInclude(c=> c.Sucursal)
                    .Include(c => c.Bodega).ThenInclude(c=> c.Sucursal)
                    .LastOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.Confirmacion));
                item.UbicacionActivoFijo.Clear();
                item.SucursalActual = item?.UbicacionActivoFijoActual?.Empleado?.Dependencia?.Sucursal ?? item.UbicacionActivoFijoActual.Bodega.Sucursal;
                
                var ultimaTransferencia = db.TransferenciaActivoFijoDetalle.Include(c=> c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).LastOrDefault(c => c.UbicacionActivoFijoDestino.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdCodigoActivoFijo != null);
                item.CodigoActivoFijo = clonacionService.ClonarCodigoActivoFijo(ultimaTransferencia != null ? ultimaTransferencia.CodigoActivoFijo : db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo));

                if (incluirAltasActivoFijo != null)
                {
                    if ((bool)incluirAltasActivoFijo)
                    {
                        var recepcionActivoFijoDetalleAltaActivoFijo = db.AltaActivoFijoDetalle.Include(c => c.AltaActivoFijo).ThenInclude(c => c.MotivoAlta).Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        if (recepcionActivoFijoDetalleAltaActivoFijo != null)
                        {
                            item.AltaActivoFijoActual = clonacionService.ClonarAltaActivoFijo(recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo);
                            item.AltaActivoFijoActual.AltaActivoFijoDetalle.Clear();
                            item.AltaActivoFijoDetalle.Clear();
                        }
                    }
                }

                if (incluirBajasActivoFijo != null)
                {
                    if ((bool)incluirBajasActivoFijo)
                    {
                        var recepcionActivoFijoDetalleBajaActivoFijo = db.BajaActivoFijoDetalle.Include(c => c.BajaActivoFijo).ThenInclude(c=> c.MotivoBaja).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        if (recepcionActivoFijoDetalleBajaActivoFijo != null)
                        {
                            item.BajaActivoFijoActual = clonacionService.ClonarBajaActivoFijo(recepcionActivoFijoDetalleBajaActivoFijo.BajaActivoFijo);
                            item.BajaActivoFijoActual.BajaActivoFijoDetalle.Clear();
                            item.BajaActivoFijoDetalle.Clear();
                        }
                    }
                }

                if (incluirActivoFijo != null)
                {
                    if ((bool)incluirActivoFijo)
                        item.ActivoFijo = clonacionService.ClonarActivoFijo(ObtenerDatosActivoFijo().FirstOrDefault(c=> c.IdActivoFijo == item.IdActivoFijo), new List<RecepcionActivoFijoDetalle>());
                }
            }
            recepcionActivoFijoDetalle = recepcionActivoFijoDetalle.OrderBy(c => c.UbicacionActivoFijoActual.IdBodega).ThenBy(c => c.UbicacionActivoFijoActual.IdEmpleado);
            
            if (claimsTransferencia != null && claimsTransferencia.IdSucursal != null && claimsTransferencia.IdSucursal > 0 && incluirClaimsTransferencia)
                return idActivoFijo != null ? recepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == idActivoFijo && c.SucursalActual.IdSucursal == claimsTransferencia.IdSucursal) : recepcionActivoFijoDetalle.Where(c => c.SucursalActual.IdSucursal == claimsTransferencia.IdSucursal);

            return idActivoFijo != null ? recepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == idActivoFijo) : recepcionActivoFijoDetalle;
        }
        #endregion

        #region Listados Comunes
        private async Task<List<ActivoFijo>> ListarActivosFijos(Expression<Func<ActivoFijo, bool>> predicadoActivoFijo = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoRecepcionActivoFijoDetalle = null)
        {
            try
            {
                var listaActivosFijos = new List<ActivoFijo>();
                var listaActivoFijoBD = ObtenerDatosActivoFijo();
                var listaActivoFijo = await (predicadoActivoFijo != null ? listaActivoFijoBD.Where(predicadoActivoFijo).ToListAsync() : listaActivoFijoBD.ToListAsync());

                foreach (var item in listaActivoFijo)
                {
                    var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                    var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(item.IdActivoFijo);
                    var listaRecepcionActivoFijoDetalleAF = await (predicadoRecepcionActivoFijoDetalle != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoRecepcionActivoFijoDetalle).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());

                    if (listaRecepcionActivoFijoDetalleAF.Count > 0)
                    {
                        listaRecepcionActivoFijoDetalleAF.ForEach(c => listaRecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(c)));
                        listaActivosFijos.Add(clonacionService.ClonarActivoFijo(item, listaRecepcionActivoFijoDetalle));
                    }
                }
                return listaActivosFijos;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }
        private async Task<List<RecepcionActivoFijo>> ListarRecepcionesActivosFijos(Expression<Func<RecepcionActivoFijo, bool>> predicado = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                var lista = new List<RecepcionActivoFijo>();
                var idsRecepciones = await db.RecepcionActivoFijo.Select(c => c.IdRecepcionActivoFijo).ToListAsync();
                foreach (var item in idsRecepciones)
                {
                    var response = await ObtenerRecepcionActivoFijo(item, predicado: predicado, predicadoDetalleActivoFijo: predicadoDetalleActivoFijo);
                    if (response.IsSuccess)
                    {
                        var recepcionActivoFijo = response.Resultado as RecepcionActivoFijo;
                        if (recepcionActivoFijo.RecepcionActivoFijoDetalle.Count > 0)
                            lista.Add(recepcionActivoFijo);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RecepcionActivoFijo>();
            }
        }
        private async Task<List<ActivoFijo>> ListarActivosFijosPorAgrupacionSucursalNombre(int? idActivoFijo = null, bool? incluirAltasActivoFijo = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoRecepcionActivoFijoDetalle = null)
        {
            try
            {
                var listaActivosFijos = new List<ActivoFijo>();
                var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(idActivoFijo: idActivoFijo, incluirActivoFijo: true, incluirAltasActivoFijo: incluirAltasActivoFijo);
                var listaRecepcionActivoFijoDetalleAF = await (predicadoRecepcionActivoFijoDetalle != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoRecepcionActivoFijoDetalle).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());
                var listaRAFDPorSucursal = listaRecepcionActivoFijoDetalleAF.GroupBy(c => new { c.SucursalActual.IdSucursal });

                foreach (var rafdPorSucursal in listaRAFDPorSucursal)
                {
                    var listaRAFDPorNombreSubclase = rafdPorSucursal.GroupBy(c => c.ActivoFijo.Nombre);
                    foreach (var rafdPorNombreSubclase in listaRAFDPorNombreSubclase)
                    {
                        var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                        var activoFijo = rafdPorNombreSubclase.FirstOrDefault().ActivoFijo;

                        foreach (var rafd in rafdPorNombreSubclase)
                        {
                            rafd.ActivoFijo = null;
                            listaRecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(rafd));
                        }
                        listaActivosFijos.Add(clonacionService.ClonarActivoFijo(activoFijo, listaRecepcionActivoFijoDetalle));
                    }
                }
                return listaActivosFijos;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }
        private async Task<List<TransferenciaActivoFijo>> ListarTransferenciasActivoFijo(TransferenciaEstadoTransfer transferenciaEstadoTransfer, Expression<Func<TransferenciaActivoFijo, bool>> predicado = null, Expression<Func<TransferenciaActivoFijoDetalle, bool>> predicadoTransferenciaActivoFijoDetalle = null, bool incluirClaimsTransferencia = true)
        {
            try
            {
                var lista = new List<TransferenciaActivoFijo>();
                var idsTransferencias = db.TransferenciaActivoFijo
                    .Include(c => c.MotivoTransferencia)
                    .Include(c => c.Estado)
                    .Where(c => c.MotivoTransferencia.Motivo_Transferencia.ToUpper() == transferenciaEstadoTransfer.MotivoTransferencia);

                if (transferenciaEstadoTransfer.Estado != null)
                    idsTransferencias = idsTransferencias.Where(c => c.Estado.Nombre.ToUpper() == transferenciaEstadoTransfer.Estado);

                var listadoIdsTransferencias = await idsTransferencias.Select(c => c.IdTransferenciaActivoFijo).ToListAsync();
                foreach (var item in listadoIdsTransferencias)
                {
                    var response = await ObtenerTransferenciaActivoFijo(item, incluirClaimsTransferencia: incluirClaimsTransferencia, predicado: predicado, predicadoTransferenciaActivoFijoDetalle: predicadoTransferenciaActivoFijoDetalle);
                    if (response.IsSuccess)
                        lista.Add(response.Resultado as TransferenciaActivoFijo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<TransferenciaActivoFijo>();
            }
        }
        private async Task<List<AltaActivoFijo>> ListarAltasActivoFijo(Expression<Func<AltaActivoFijo, bool>> predicado = null, bool incluirComponentes = false)
        {
            try
            {
                var lista = new List<AltaActivoFijo>();
                var idsAltas = await (predicado != null ? db.AltaActivoFijo.Where(predicado).Select(c => c.IdAltaActivoFijo).ToListAsync() : db.AltaActivoFijo.Select(c => c.IdAltaActivoFijo).ToListAsync());
                foreach (var item in idsAltas)
                {
                    var response = await ObtenerAltaActivoFijo(item, incluirComponentes: incluirComponentes);
                    if (response.IsSuccess)
                        lista.Add(response.Resultado as AltaActivoFijo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<AltaActivoFijo>();
            }
        }
        private async Task<List<BajaActivoFijo>> ListarBajasActivoFijo(Expression<Func<BajaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var lista = new List<BajaActivoFijo>();
                var idsBajas = await (predicado != null ? db.BajaActivoFijo.Where(predicado).Select(c => c.IdBajaActivoFijo).ToListAsync() : db.BajaActivoFijo.Select(c => c.IdBajaActivoFijo).ToListAsync());
                foreach (var item in idsBajas)
                {
                    var response = await ObtenerBajaActivoFijo(item);
                    if (response.IsSuccess)
                        lista.Add(response.Resultado as BajaActivoFijo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<BajaActivoFijo>();
            }
        }
        private async Task<List<InventarioActivoFijo>> ListarInventariosActivosFijos(Expression<Func<InventarioActivoFijo, bool>> predicado = null)
        {
            try
            {
                var lista = new List<InventarioActivoFijo>();
                var idsInventarios = await (predicado != null ? db.InventarioActivoFijo.Where(predicado).Select(c => c.IdInventarioActivoFijo).ToListAsync() : db.InventarioActivoFijo.Select(c => c.IdInventarioActivoFijo).ToListAsync());
                foreach (var item in idsInventarios)
                {
                    var response = await ObtenerInventarioActivoFijo(item);
                    if (response.IsSuccess)
                        lista.Add(response.Resultado as InventarioActivoFijo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<InventarioActivoFijo>();
            }
        }
        private async Task<List<MovilizacionActivoFijo>> ListarMovilizacionesActivosFijos(Expression<Func<MovilizacionActivoFijo, bool>> predicado = null)
        {
            try
            {
                var lista = new List<MovilizacionActivoFijo>();
                var idsMovilizaciones = await (predicado != null ? db.MovilizacionActivoFijo.Where(predicado).Select(c => c.IdMovilizacionActivoFijo).ToListAsync() : db.MovilizacionActivoFijo.Select(c => c.IdMovilizacionActivoFijo).ToListAsync());
                foreach (var item in idsMovilizaciones)
                {
                    var response = await ObtenerMovilizacionActivoFijo(item);
                    if (response.IsSuccess)
                        lista.Add(response.Resultado as MovilizacionActivoFijo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MovilizacionActivoFijo>();
            }
        }
        #endregion

        #region Gestión de Componentes
        private async Task GestionarComponentesRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle item, UbicacionActivoFijo ubicacionComponente = null, bool isRecepcion = false, bool isAltaInsertar = false, bool isAltaEditar = false, AltaActivoFijoDetalle altaActivoFijoDetalle = null, bool isTransferenciaInsertar = false, TransferenciaActivoFijoDetalle transferenciaActivoFijoDetalle = null, bool isBajaInsertar = false, BajaActivoFijoDetalle bajaActivoFijoDetalle = null)
        {
            var listaIdsComponentesBD = await db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle).Select(c => c.IdRecepcionActivoFijoDetalleComponente).ToListAsync();
            var listaExcept = listaIdsComponentesBD.Except(item.ComponentesActivoFijoOrigen.Select(c => c.IdRecepcionActivoFijoDetalleComponente).ToList());
            foreach (var comp in item.ComponentesActivoFijoOrigen)
            {
                UbicacionActivoFijo nuevaUbicacionComponente = null;
                bool existeComponente = await db.ComponenteActivoFijo
                    .Where(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle)
                    .AnyAsync(c => c.IdRecepcionActivoFijoDetalleComponente == comp.IdRecepcionActivoFijoDetalleComponente);
                if (!existeComponente)
                {
                    comp.IdRecepcionActivoFijoDetalleOrigen = item.IdRecepcionActivoFijoDetalle;
                    ubicacionComponente.IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente;
                    nuevaUbicacionComponente = clonacionService.ClonarUbicacionActivoFijo(ubicacionComponente);
                    db.UbicacionActivoFijo.Add(nuevaUbicacionComponente);
                    db.ComponenteActivoFijo.Add(comp);
                    await db.SaveChangesAsync();
                }
                else
                {
                    if (isRecepcion || isAltaEditar)
                    {
                        var ubicacionComponenteActualizar = await db.UbicacionActivoFijo.OrderBy(c => c.FechaUbicacion).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente && c.Confirmacion);
                        if (ubicacionComponenteActualizar != null)
                        {
                            ubicacionComponenteActualizar.IdEmpleado = ubicacionComponente.IdEmpleado;
                            ubicacionComponenteActualizar.IdBodega = ubicacionComponente.IdBodega;
                            ubicacionComponenteActualizar.FechaUbicacion = ubicacionComponente.FechaUbicacion;
                            await db.SaveChangesAsync();
                        }
                    }
                }
                if (isAltaInsertar || isTransferenciaInsertar)
                {
                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).Include(c=> c.Estado).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                    if (isAltaInsertar)
                    {
                        rafd.IdEstado = (await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Alta)).IdEstado;
                        await db.SaveChangesAsync();
                    }

                    if ((existeComponente && isAltaInsertar) || (existeComponente && isTransferenciaInsertar && rafd.Estado.Nombre.ToUpper() == Estados.Alta))
                    {
                        ubicacionComponente.IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente;
                        nuevaUbicacionComponente = clonacionService.ClonarUbicacionActivoFijo(ubicacionComponente);
                        db.UbicacionActivoFijo.Add(nuevaUbicacionComponente);
                        await db.SaveChangesAsync();
                    }

                    if (isAltaInsertar)
                    {
                        db.AltaActivoFijoDetalle.Add(new AltaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente,
                            IdAltaActivoFijo = altaActivoFijoDetalle.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = altaActivoFijoDetalle.IdTipoUtilizacionAlta,
                            IdUbicacionActivoFijo = nuevaUbicacionComponente.IdUbicacionActivoFijo,
                            IsComponente = true
                        });
                        await db.SaveChangesAsync();
                    }

                    if (isTransferenciaInsertar)
                    {
                        if (rafd.Estado.Nombre.ToUpper() == Estados.Alta)
                        {
                            var ultimaUbicacion = await db.UbicacionActivoFijo.OrderByDescending(c => c.FechaUbicacion).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente && c.Confirmacion);
                            db.TransferenciaActivoFijoDetalle.Add(new TransferenciaActivoFijoDetalle
                            {
                                IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente,
                                IdTransferenciaActivoFijo = transferenciaActivoFijoDetalle.IdTransferenciaActivoFijo,
                                IdUbicacionActivoFijoOrigen = ultimaUbicacion.IdUbicacionActivoFijo,
                                IdUbicacionActivoFijoDestino = nuevaUbicacionComponente.IdUbicacionActivoFijo,
                                IdCodigoActivoFijo = transferenciaActivoFijoDetalle.IdCodigoActivoFijo,
                                IsComponente = true
                            });
                            await db.SaveChangesAsync();
                        }
                    }
                }
                if (isBajaInsertar)
                {
                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.Estado).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                    if (rafd.Estado.Nombre.ToUpper() == Estados.Alta)
                    {
                        rafd.IdEstado = (await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Baja)).IdEstado;
                        await db.SaveChangesAsync();

                        db.BajaActivoFijoDetalle.Add(new BajaActivoFijoDetalle
                        {
                            IdBajaActivoFijo = bajaActivoFijoDetalle.IdBajaActivoFijo,
                            IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente,
                            IsComponente = true
                        });
                        await db.SaveChangesAsync();
                    }
                }
                Task.Run(async () => {
                    if (ubicacionComponente != null)
                        ubicacionComponente.IdRecepcionActivoFijoDetalle = 0;

                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).Include(c => c.ComponentesActivoFijoOrigen).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                    await GestionarComponentesRecepcionActivoFijoDetalle(rafd, ubicacionComponente: ubicacionComponente, isRecepcion: isRecepcion, isAltaInsertar: isAltaInsertar, isAltaEditar: isAltaEditar, altaActivoFijoDetalle: altaActivoFijoDetalle, isTransferenciaInsertar: isTransferenciaInsertar, transferenciaActivoFijoDetalle: transferenciaActivoFijoDetalle, isBajaInsertar: false, bajaActivoFijoDetalle: bajaActivoFijoDetalle);
                }).Wait(-1);
            }
            foreach (var idcomp in listaExcept)
            {
                db.ComponenteActivoFijo.Remove(await db.ComponenteActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle && c.IdRecepcionActivoFijoDetalleComponente == idcomp));
                var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idcomp);
                var estadoDB = await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Recepcionado);
                rafd.IdEstado = estadoDB.IdEstado;
                await db.SaveChangesAsync();
            }
        }
        private void ActualizarEstadoDetalleActivoFijoComponente(int idRecepcionActivoFijoDetalle, Estado estado, bool isComponentes = true)
        {
            var rafd = db.RecepcionActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
            rafd.IdEstado = estado.IdEstado;
            if (isComponentes)
            {
                var listaIdsComponentesBD = db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == idRecepcionActivoFijoDetalle).Select(c => c.IdRecepcionActivoFijoDetalleComponente);
                foreach (var item in listaIdsComponentesBD)
                    ActualizarEstadoDetalleActivoFijoComponente(item, estado, isComponentes);
            }
        }
        #endregion

        #region Obtener Response Comunes
        private async Task<Response> ObtenerActivoFijo(int id, Expression<Func<ActivoFijo, bool>> predicadoActivoFijo = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                var activoFijoBD = ObtenerDatosActivoFijo();
                var activoFijo = await (predicadoActivoFijo != null ? activoFijoBD.Where(predicadoActivoFijo).FirstOrDefaultAsync(m => m.IdActivoFijo == id) : activoFijoBD.FirstOrDefaultAsync(m => m.IdActivoFijo == id));
                if (activoFijo != null)
                {
                    var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                    var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(activoFijo.IdActivoFijo);
                    var listaRecepcionActivoFijoDetalleAF = await (predicadoDetalleActivoFijo != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoDetalleActivoFijo).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());

                    if (listaRecepcionActivoFijoDetalleAF.Count > 0)
                    {
                        listaRecepcionActivoFijoDetalleAF.ForEach(c => listaRecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(c, incluirComponentes: true)));
                        activoFijo = clonacionService.ClonarActivoFijo(activoFijo, listaRecepcionActivoFijoDetalle);
                    }
                    else
                        return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
                }
                activoFijo.DocumentosActivoFijo = clonacionService.ClonarListadoDocumentoActivoFijo(await db.DocumentoActivoFijo.Where(c => c.IdActivoFijo == id).ToListAsync());
                return new Response { IsSuccess = activoFijo != null, Message = activoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerRecepcionActivoFijo(int id, Expression<Func<RecepcionActivoFijo, bool>> predicado = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                var recepcionActivoFijoBD = db.RecepcionActivoFijo.Include(c => c.PolizaSeguroActivoFijo).Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.CompaniaSeguro).Include(c => c.MotivoAlta).Include(c => c.Proveedor).Include(c => c.FondoFinanciamiento);
                var recepcionActivoFijo = clonacionService.ClonarRecepcionActivoFijo(predicado != null ? await recepcionActivoFijoBD.Where(predicado).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == id) : await recepcionActivoFijoBD.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == id));
                recepcionActivoFijo.RecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();

                var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true, incluirBajasActivoFijo: true).Where(c=> c.IdRecepcionActivoFijo == id);
                var listaRecepcionActivoFijoDetalleAF = await (predicadoDetalleActivoFijo != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoDetalleActivoFijo).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());

                foreach (var item in listaRecepcionActivoFijoDetalleAF)
                    recepcionActivoFijo.RecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true));

                recepcionActivoFijo.DocumentoActivoFijo = clonacionService.ClonarListadoDocumentoActivoFijo(await db.DocumentoActivoFijo.Where(c => c.IdRecepcionActivoFijo == id).ToListAsync());
                return new Response { IsSuccess = recepcionActivoFijo != null, Message = recepcionActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<RecepcionActivoFijoDetalle> ObtenerDetalleActivoFijo(int idRecepcionActivoFijoDetalle, int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null, bool? incluirComponentes = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null, bool incluirClaimsTransferencia = true)
        {
            try
            {
                var recepcionActivoFijoDetalleBD = ObtenerListadoDetallesActivosFijos(idActivoFijo: idActivoFijo, incluirActivoFijo: incluirActivoFijo, incluirAltasActivoFijo: incluirAltasActivoFijo, incluirBajasActivoFijo: incluirBajasActivoFijo, incluirClaimsTransferencia: incluirClaimsTransferencia);
                return ObtenerRecepcionActivoFijoDetalle(await (predicadoDetalleActivoFijo != null ? recepcionActivoFijoDetalleBD.Where(predicadoDetalleActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle) : recepcionActivoFijoDetalleBD.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)), incluirComponentes, incluirActivoFijo: incluirActivoFijo);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private async Task<Response> ObtenerAltaActivoFijo(int id, Expression<Func<AltaActivoFijo, bool>> predicado = null, bool incluirComponentes = false)
        {
            try
            {
                var altaActivoFijoBD = db.AltaActivoFijo.Include(x => x.FacturaActivoFijo).Include(c => c.MotivoAlta);
                var altaActivoFijo = clonacionService.ClonarAltaActivoFijo(predicado != null ? await altaActivoFijoBD.Where(predicado).FirstOrDefaultAsync(c => c.IdAltaActivoFijo == id) : await altaActivoFijoBD.FirstOrDefaultAsync(c => c.IdAltaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleAltaActivoFijo = await db.AltaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdAltaActivoFijo == altaActivoFijo.IdAltaActivoFijo && c.IsComponente == incluirComponentes).ToListAsync();
                altaActivoFijo.AltaActivoFijoDetalle = new List<AltaActivoFijoDetalle>();
                foreach (var item in listadoIdsRecepcionActivoFijoDetalleAltaActivoFijo)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        altaActivoFijo.AltaActivoFijoDetalle.Add(new AltaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = item.IdTipoUtilizacionAlta,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            IdUbicacionActivoFijo = item.IdUbicacionActivoFijo,
                            TipoUtilizacionAlta = clonacionService.ClonarTipoUtilizacionAlta(await db.TipoUtilizacionAlta.FirstOrDefaultAsync(c => c.IdTipoUtilizacionAlta == item.IdTipoUtilizacionAlta)),
                            UbicacionActivoFijo = clonacionService.ClonarUbicacionActivoFijo(await db.UbicacionActivoFijo.Include(c => c.Bodega).ThenInclude(c=> c.Sucursal).Include(c => c.Empleado).ThenInclude(c => c.Persona).Include(c=> c.Empleado).ThenInclude(c=> c.Dependencia).ThenInclude(c=> c.Sucursal).FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo && c.Confirmacion)),
                            Componentes = await ObtenerComponentesRecepcionActivoFijo(recepcionActivoFijoDetalle)
                        });
                        if (recepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Alta)
                            altaActivoFijo.IsReversarAlta = true;
                    }
                }
                altaActivoFijo.DocumentoActivoFijo = clonacionService.ClonarListadoDocumentoActivoFijo(await db.DocumentoActivoFijo.Where(c => c.IdAltaActivoFijo == id).ToListAsync());
                if (altaActivoFijo.FacturaActivoFijo != null)
                    altaActivoFijo.FacturaActivoFijo.DocumentoActivoFijo = clonacionService.ClonarListadoDocumentoActivoFijo(await db.DocumentoActivoFijo.Where(c => c.IdFacturaActivoFijo == altaActivoFijo.IdFacturaActivoFijo).ToListAsync());
                return new Response { IsSuccess = altaActivoFijo != null, Message = altaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = altaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerBajaActivoFijo(int id, Expression<Func<BajaActivoFijo, bool>> predicado = null, bool incluirComponentes = false)
        {
            try
            {
                var bajaActivoFijoBD = db.BajaActivoFijo.Include(c => c.MotivoBaja);
                var bajaActivoFijo = clonacionService.ClonarBajaActivoFijo(predicado != null ? await bajaActivoFijoBD.Where(predicado).FirstOrDefaultAsync(c => c.IdBajaActivoFijo == id) : await bajaActivoFijoBD.FirstOrDefaultAsync(c => c.IdBajaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo = await db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdBajaActivoFijo == bajaActivoFijo.IdBajaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Baja && c.IsComponente == incluirComponentes).ToListAsync();
                bajaActivoFijo.BajaActivoFijoDetalle = new List<BajaActivoFijoDetalle>();
                foreach (var item in listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        bajaActivoFijo.BajaActivoFijoDetalle.Add(new BajaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            Componentes = await ObtenerComponentesRecepcionActivoFijo(recepcionActivoFijoDetalle)
                        });
                    }

                    if (bajaActivoFijo.BajaActivoFijoDetalle.Count == 0)
                        listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo.Remove(item);
                }
                return new Response { IsSuccess = bajaActivoFijo != null, Message = bajaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bajaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerTransferenciaActivoFijo(int id, Expression<Func<TransferenciaActivoFijo, bool>> predicado = null, Expression<Func<TransferenciaActivoFijoDetalle, bool>> predicadoTransferenciaActivoFijoDetalle = null, bool incluirClaimsTransferencia = true)
        {
            try
            {
                var transferenciaBD = db.TransferenciaActivoFijo.Include(c => c.Estado).Include(c => c.MotivoTransferencia);
                var transferenciaActivoFijo = clonacionService.ClonarTransferenciaActivoFijo(predicado != null ? await transferenciaBD.Where(predicado).FirstOrDefaultAsync(c => c.IdTransferenciaActivoFijo == id) : await transferenciaBD.FirstOrDefaultAsync(c => c.IdTransferenciaActivoFijo == id));

                if (transferenciaActivoFijo.IdEmpleadoResponsableEnvio != null)
                    transferenciaActivoFijo.EmpleadoResponsableEnvio = clonacionService.ClonarEmpleado(await db.Empleado.Include(c => c.Persona).Include(c=> c.Dependencia).ThenInclude(c=> c.Sucursal).FirstOrDefaultAsync(c => c.IdEmpleado == transferenciaActivoFijo.IdEmpleadoResponsableEnvio));

                if (transferenciaActivoFijo.IdEmpleadoResponsableRecibo != null)
                    transferenciaActivoFijo.EmpleadoResponsableRecibo = clonacionService.ClonarEmpleado(await db.Empleado.Include(c => c.Persona).Include(c => c.Dependencia).ThenInclude(c => c.Sucursal).FirstOrDefaultAsync(c => c.IdEmpleado == transferenciaActivoFijo.IdEmpleadoResponsableRecibo));

                var listadoTransferenciaActivoFijoDetalle = db.TransferenciaActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado)
                    .Include(c => c.UbicacionActivoFijoOrigen).ThenInclude(c=> c.Empleado).ThenInclude(c=> c.Persona)
                    .Include(c => c.UbicacionActivoFijoOrigen).ThenInclude(c => c.Bodega).ThenInclude(c => c.Sucursal)
                    .Include(c => c.UbicacionActivoFijoOrigen).ThenInclude(c => c.Empleado).ThenInclude(c => c.Dependencia).ThenInclude(c => c.Sucursal)
                    .Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c => c.Bodega).ThenInclude(c => c.Sucursal)
                    .Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c => c.Empleado).ThenInclude(c => c.Dependencia).ThenInclude(c => c.Sucursal)
                    .Where(c => c.IdTransferenciaActivoFijo == transferenciaActivoFijo.IdTransferenciaActivoFijo);

                var listadoIdsTransferenciaActivoFijoDetalle = predicadoTransferenciaActivoFijoDetalle != null ? await listadoTransferenciaActivoFijoDetalle.Where(predicadoTransferenciaActivoFijoDetalle).ToListAsync() : await listadoTransferenciaActivoFijoDetalle.ToListAsync();
                foreach (var item in listadoIdsTransferenciaActivoFijoDetalle)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true, incluirAltasActivoFijo: true, incluirClaimsTransferencia: incluirClaimsTransferencia);
                    if ((recepcionActivoFijoDetalle != null && !item.IsComponente) || (recepcionActivoFijoDetalle != null && recepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Alta))
                    {
                        transferenciaActivoFijo.TransferenciaActivoFijoDetalle.Add(new TransferenciaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdTransferenciaActivoFijo = item.IdTransferenciaActivoFijo,
                            IdUbicacionActivoFijoDestino = item.IdUbicacionActivoFijoDestino,
                            IdUbicacionActivoFijoOrigen = item.IdUbicacionActivoFijoOrigen,
                            IdCodigoActivoFijo = item.IdCodigoActivoFijo,
                            UbicacionActivoFijoOrigen = clonacionService.ClonarUbicacionActivoFijo(item.UbicacionActivoFijoOrigen),
                            UbicacionActivoFijoDestino = clonacionService.ClonarUbicacionActivoFijo(item.UbicacionActivoFijoDestino),
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            CodigoActivoFijo = clonacionService.ClonarCodigoActivoFijo(await db.CodigoActivoFijo.FirstOrDefaultAsync(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo))
                        });
                        if (recepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Alta)
                            transferenciaActivoFijo.IsEditar = true;
                    }
                }
                var transferenciaActivoFijoDetalle = transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault();
                transferenciaActivoFijo.SucursalOrigen = transferenciaActivoFijoDetalle?.UbicacionActivoFijoOrigen?.Empleado?.Dependencia?.Sucursal ?? transferenciaActivoFijoDetalle.UbicacionActivoFijoOrigen.Bodega.Sucursal;
                transferenciaActivoFijo.SucursalDestino = transferenciaActivoFijoDetalle?.UbicacionActivoFijoDestino?.Empleado?.Dependencia?.Sucursal ?? transferenciaActivoFijoDetalle.UbicacionActivoFijoDestino.Bodega.Sucursal;
                return new Response { IsSuccess = transferenciaActivoFijo != null, Message = transferenciaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = transferenciaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerInventarioActivoFijo(int id, Expression<Func<InventarioActivoFijo, bool>> predicado = null)
        {
            try
            {
                var inventarioAFBD = db.InventarioActivoFijo.Include(c => c.Estado);
                var inventarioActivoFijo = clonacionService.ClonarInventarioActivoFijo(predicado != null ? await inventarioAFBD.Where(predicado).FirstOrDefaultAsync(c => c.IdInventarioActivoFijo == id) : await inventarioAFBD.FirstOrDefaultAsync(c => c.IdInventarioActivoFijo == id));
                var listadoIdsInventarioActivoFijoDetalle = await db.InventarioActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdInventarioActivoFijo == inventarioActivoFijo.IdInventarioActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Alta).ToListAsync();
                inventarioActivoFijo.InventarioActivoFijoDetalle = new List<InventarioActivoFijoDetalle>();
                foreach (var item in listadoIdsInventarioActivoFijoDetalle)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true, incluirAltasActivoFijo: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        inventarioActivoFijo.InventarioActivoFijoDetalle.Add(new InventarioActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdInventarioActivoFijo = inventarioActivoFijo.IdInventarioActivoFijo,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            Constatado = item.Constatado
                        });
                    }
                }
                return new Response { IsSuccess = inventarioActivoFijo != null, Message = inventarioActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = inventarioActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerMovilizacionActivoFijo(int id, Expression<Func<MovilizacionActivoFijo, bool>> predicado = null)
        {
            try
            {
                var movilizacionAFBD = db.MovilizacionActivoFijo
                    .Include(c => c.EmpleadoResponsable).ThenInclude(c => c.Persona)
                    .Include(c => c.EmpleadoResponsable).ThenInclude(c=> c.Dependencia).ThenInclude(c=> c.Sucursal)
                    .Include(c => c.EmpleadoSolicita).ThenInclude(c => c.Persona)
                    .Include(c => c.EmpleadoSolicita).ThenInclude(c => c.Dependencia).ThenInclude(c => c.Sucursal)
                    .Include(c => c.EmpleadoAutorizado).ThenInclude(c => c.Persona)
                    .Include(c => c.EmpleadoAutorizado).ThenInclude(c => c.Dependencia).ThenInclude(c => c.Sucursal)
                    .Include(c => c.MotivoTraslado);

                var movilizacionActivoFijo = clonacionService.ClonarMovilizacionActivoFijo(predicado != null ? await movilizacionAFBD.Where(predicado).FirstOrDefaultAsync(c => c.IdMovilizacionActivoFijo == id) : await movilizacionAFBD.FirstOrDefaultAsync(c => c.IdMovilizacionActivoFijo == id));
                var listadoIdsMovilizacionActivoFijoDetalle = await db.MovilizacionActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdMovilizacionActivoFijo == movilizacionActivoFijo.IdMovilizacionActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Alta).ToListAsync();
                movilizacionActivoFijo.MovilizacionActivoFijoDetalle = new List<MovilizacionActivoFijoDetalle>();
                foreach (var item in listadoIdsMovilizacionActivoFijoDetalle)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true, incluirAltasActivoFijo: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        movilizacionActivoFijo.MovilizacionActivoFijoDetalle.Add(new MovilizacionActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdMovilizacionActivoFijo = movilizacionActivoFijo.IdMovilizacionActivoFijo,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            Observaciones = item.Observaciones,
                            Componentes = await ObtenerComponentesRecepcionActivoFijo(recepcionActivoFijoDetalle)
                        });
                    }
                }
                return new Response { IsSuccess = movilizacionActivoFijo != null, Message = movilizacionActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = movilizacionActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        #endregion

        #region Eliminar
        private void EliminarSeccionAlta(int idRecepcionActivoFijoDetalle)
        {
            EliminarMantenimientoPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarRevalorizacionPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarDepreciacionPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarTransferenciaPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarInventarioPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarMovilizacionPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarProcesoJudicialPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
            EliminarAltaPorIdRecepcionActivoFijoDetalle(idRecepcionActivoFijoDetalle);
        }
        private void EliminarTransferenciaPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var transferenciaActivoFijoDetalle = db.TransferenciaActivoFijoDetalle
                            .Include(c => c.TransferenciaActivoFijo)
                            .Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)
                            .GroupBy(c=> c.IdTransferenciaActivoFijo);

            if (transferenciaActivoFijoDetalle.Count() > 0)
            {
                foreach (var item in transferenciaActivoFijoDetalle)
                {
                    int idTransferenciaActivoFijo = item.Key;
                    foreach (var item1 in item)
                    {
                        db.TransferenciaActivoFijoDetalle.Remove(db.TransferenciaActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle && c.IdTransferenciaActivoFijo == idTransferenciaActivoFijo));

                        if (db.AltaActivoFijoDetalle.Count(c => c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle && c.IdUbicacionActivoFijo == item1.IdUbicacionActivoFijoOrigen) == 0)
                            db.UbicacionActivoFijo.Remove(db.UbicacionActivoFijo.FirstOrDefault(c => c.IdUbicacionActivoFijo == item1.IdUbicacionActivoFijoOrigen));

                        if (db.AltaActivoFijoDetalle.Count(c => c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle && c.IdUbicacionActivoFijo == item1.IdUbicacionActivoFijoDestino) == 0)
                            db.UbicacionActivoFijo.Remove(db.UbicacionActivoFijo.FirstOrDefault(c => c.IdUbicacionActivoFijo == item1.IdUbicacionActivoFijoDestino));

                        if (item1.IdCodigoActivoFijo != null)
                            db.CodigoActivoFijo.Remove(db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == item1.IdCodigoActivoFijo));

                        db.SaveChanges();
                    }
                    if (db.TransferenciaActivoFijoDetalle.Count(c => c.IdTransferenciaActivoFijo == idTransferenciaActivoFijo) == 0)
                    {
                        db.TransferenciaActivoFijo.Remove(db.TransferenciaActivoFijo.FirstOrDefault(c => c.IdTransferenciaActivoFijo == idTransferenciaActivoFijo));
                        db.SaveChanges();
                    }
                }
            }
        }
        private void EliminarAltaPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var altaActivoFijoDetalle = db.AltaActivoFijoDetalle
                .Include(c => c.AltaActivoFijo).ThenInclude(c=> c.FacturaActivoFijo)
                .Include(c=> c.UbicacionActivoFijo)
                .Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);

            if (altaActivoFijoDetalle.Count() > 0)
            {
                int idAltaActivoFijo = altaActivoFijoDetalle.FirstOrDefault().IdAltaActivoFijo;
                int? idFacturaActivoFijo = altaActivoFijoDetalle.FirstOrDefault().AltaActivoFijo.IdFacturaActivoFijo;

                foreach (var item in altaActivoFijoDetalle)
                {
                    ActualizarEstadoDetalleActivoFijoComponente(item.IdRecepcionActivoFijoDetalle, db.Estado.FirstOrDefault(c => c.Nombre.ToUpper() == Estados.Recepcionado));
                    db.AltaActivoFijoDetalle.Remove(db.AltaActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdAltaActivoFijo == idAltaActivoFijo));
                    db.UbicacionActivoFijo.Remove(db.UbicacionActivoFijo.FirstOrDefault(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo));

                    if (item.IsComponente)
                        db.ComponenteActivoFijo.Remove(db.ComponenteActivoFijo.LastOrDefault(c => c.IdRecepcionActivoFijoDetalleComponente == item.IdRecepcionActivoFijoDetalle));
                    db.SaveChanges();
                }                
                EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdAltaActivoFijo == idAltaActivoFijo));
                if (db.AltaActivoFijoDetalle.Count(c => c.IdAltaActivoFijo == idAltaActivoFijo) == 0)
                {
                    db.AltaActivoFijo.Remove(db.AltaActivoFijo.FirstOrDefault(c => c.IdAltaActivoFijo == idAltaActivoFijo));
                    db.SaveChanges();
                }

                if (idFacturaActivoFijo != null)
                {
                    EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdFacturaActivoFijo == idFacturaActivoFijo));
                    db.FacturaActivoFijo.Remove(db.FacturaActivoFijo.FirstOrDefault(c => c.IdFacturaActivoFijo == idFacturaActivoFijo));
                    db.SaveChanges();
                }
            }
        }
        private void EliminarRecepcionActivoFijo(int idRecepcionActivoFijo)
        {
            EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdRecepcionActivoFijo == idRecepcionActivoFijo));
            db.RecepcionActivoFijo.Remove(db.RecepcionActivoFijo.FirstOrDefault(c=> c.IdRecepcionActivoFijo == idRecepcionActivoFijo));
            db.SaveChanges();
        }
        private void EliminarRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.RecepcionActivoFijoDetalle.Remove(db.RecepcionActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.SaveChanges();
        }
        private void EliminarActivoFijo(int idActivoFijo)
        {
            EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdActivoFijo == idActivoFijo));
            db.ActivoFijo.Remove(db.ActivoFijo.FirstOrDefault(c=> c.IdActivoFijo == idActivoFijo));
            db.SaveChanges();
        }
        private void EliminarCodigoActivoFijo(int idCodigoActivoFijo)
        {
            db.CodigoActivoFijo.Remove(db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == idCodigoActivoFijo));
            db.SaveChanges();
        }
        private void EliminarBajaPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var bajaActivoFijoDetalle = db.BajaActivoFijoDetalle
                .Include(c => c.BajaActivoFijo)
                .Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)
                .GroupBy(c=> c.IdBajaActivoFijo);

            if (bajaActivoFijoDetalle.Count() > 0)
            {
                foreach (var item in bajaActivoFijoDetalle)
                {
                    int idBajaActivoFijo = item.Key;
                    foreach (var item1 in item)
                    {
                        db.BajaActivoFijoDetalle.Remove(db.BajaActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle && c.IdBajaActivoFijo == idBajaActivoFijo));
                        db.SaveChanges();
                    }

                    if (db.BajaActivoFijoDetalle.Count(c => c.IdBajaActivoFijo == idBajaActivoFijo) == 0)
                    {
                        db.BajaActivoFijo.Remove(db.BajaActivoFijo.FirstOrDefault(c => c.IdBajaActivoFijo == idBajaActivoFijo));
                        db.SaveChanges();
                    }
                }
                
            }
        }
        private void EliminarInventarioPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var inventarioActivoFijoDetalle = db.InventarioActivoFijoDetalle
                .Include(c => c.InventarioActivoFijo)
                .Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)
                .GroupBy(c=> c.IdInventarioActivoFijo);

            if (inventarioActivoFijoDetalle.Count() > 0)
            {
                foreach (var item in inventarioActivoFijoDetalle)
                {
                    int idInventarioActivoFijo = item.Key;
                    foreach (var item1 in item)
                    {
                        db.InventarioActivoFijoDetalle.Remove(db.InventarioActivoFijoDetalle.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle && c.IdInventarioActivoFijo == idInventarioActivoFijo));
                        db.SaveChanges();
                    }

                    if (db.InventarioActivoFijoDetalle.Count(c=> c.IdInventarioActivoFijo == idInventarioActivoFijo) == 0)
                    {
                        db.InventarioActivoFijo.Remove(db.InventarioActivoFijo.FirstOrDefault(c => c.IdInventarioActivoFijo == idInventarioActivoFijo));
                        db.SaveChanges();
                    }
                }
            }
        }
        private void EliminarMovilizacionPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var movilizacionActivoFijoDetalle = db.MovilizacionActivoFijoDetalle
                .Include(c => c.MovilizacionActivoFijo)
                .Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)
                .GroupBy(c=> c.IdMovilizacionActivoFijo);

            if (movilizacionActivoFijoDetalle.Count() > 0)
            {
                foreach (var item in movilizacionActivoFijoDetalle)
                {
                    int idMovilizacionActivoFijo = item.Key;
                    foreach (var item1 in item)
                    {
                        db.MovilizacionActivoFijoDetalle.Remove(db.MovilizacionActivoFijoDetalle.FirstOrDefault(c => c.IdMovilizacionActivoFijo == idMovilizacionActivoFijo && c.IdRecepcionActivoFijoDetalle == item1.IdRecepcionActivoFijoDetalle));
                        db.SaveChanges();
                    }

                    if (db.MovilizacionActivoFijoDetalle.Count(c => c.IdMovilizacionActivoFijo == idMovilizacionActivoFijo) == 0)
                    {
                        db.MovilizacionActivoFijo.Remove(db.MovilizacionActivoFijo.FirstOrDefault(c => c.IdMovilizacionActivoFijo == idMovilizacionActivoFijo));
                        db.SaveChanges();
                    }
                }
            }
        }
        private void EliminarListadoDocumentoActivoFijo(IEnumerable<DocumentoActivoFijo> listaDocumentosActivoFijo)
        {
            db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);
            foreach (var item in listaDocumentosActivoFijo)
                uploadFileService.DeleteFile(item.Url);
            db.SaveChanges();
        }
        private void EliminarProcesoJudicialPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var procesosJudiciales = db.ProcesoJudicialActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
            foreach (var item in procesosJudiciales)
            {
                EliminarListadoDocumentoActivoFijo(db.DocumentoActivoFijo.Where(c => c.IdProcesoJudicialActivoFijo == item.IdProcesoJudicialActivoFijo));
                db.ProcesoJudicialActivoFijo.Remove(db.ProcesoJudicialActivoFijo.FirstOrDefault(c=> c.IdProcesoJudicialActivoFijo == item.IdProcesoJudicialActivoFijo && c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle));
                db.SaveChanges();
            }
        }
        private void EliminarRevalorizacionPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            db.RevalorizacionActivoFijo.RemoveRange(db.RevalorizacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.SaveChanges();
        }
        private void EliminarMantenimientoPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            db.MantenimientoActivoFijo.RemoveRange(db.MantenimientoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.SaveChanges();
        }
        private void EliminarUbicacionPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            db.UbicacionActivoFijo.RemoveRange(db.UbicacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.SaveChanges();
        }
        private void EliminarDepreciacionPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            db.DepreciacionActivoFijo.RemoveRange(db.DepreciacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
            db.SaveChanges();
        }
        private void EliminarPolizaSeguroPorIdRecepcionActivoFijo(int idRecepcionActivoFijo)
        {
            db.PolizaSeguroActivoFijo.Remove(db.PolizaSeguroActivoFijo.FirstOrDefault(c=> c.IdRecepcionActivoFijo == idRecepcionActivoFijo));
            db.SaveChanges();
        }
        private void EliminarRecepcionActivoFijoDetalleEdificioPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var recepcionActivoFijoDetalleEdificio = db.RecepcionActivoFijoDetalleEdificio.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
            if (recepcionActivoFijoDetalleEdificio != null)
            {
                db.RecepcionActivoFijoDetalleEdificio.Remove(recepcionActivoFijoDetalleEdificio);
                db.SaveChanges();
            }
        }
        private void EliminarRecepcionActivoFijoDetalleVehiculoPorIdRecepcionActivoFijoDetalle(int idRecepcionActivoFijoDetalle)
        {
            var recepcionActivoFijoDetalleVehiculo = db.RecepcionActivoFijoDetalleVehiculo.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
            if (recepcionActivoFijoDetalleVehiculo != null)
            {
                db.RecepcionActivoFijoDetalleVehiculo.Remove(recepcionActivoFijoDetalleVehiculo);
                db.SaveChanges();
            }
        }
        #endregion

        #region Exportar a PDF
        [HttpPost]
        [Route("PDFTransferenciaCambioCustodio")]
        public async Task<byte[]> ExportarPDFTransferenciaCambioCustodio([FromBody] int idTransferenciaActivoFijo)
        {
            try
            {
                var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                var empleado = await db.Empleado.FirstOrDefaultAsync(c => c.IdEmpleado == claimTransfer.IdEmpleado);

                var response = await GetTransferenciaActivoFijo(idTransferenciaActivoFijo);
                var transferenciaActivoFijo = response.Resultado as TransferenciaActivoFijo;

                MemoryStream memoryStream = new MemoryStream();
                PdfWriter pdfWritter = new PdfWriter(memoryStream);
                PdfDocument pdfDocument = new PdfDocument(pdfWritter);

                PageSize pageSize = PageSize.A4.Rotate();
                float width = CssUtils.ParseAbsoluteLength("" + pageSize.GetWidth());

                pdfDocument.SetTagged();
                pdfDocument.SetDefaultPageSize(pageSize);

                ConverterProperties props = new ConverterProperties();
                MediaDeviceDescription mediaDescription = new MediaDeviceDescription(MediaType.SCREEN);
                mediaDescription.SetWidth(width);
                props.SetMediaDeviceDescription(mediaDescription);

                var eventHandler = new CambioCustodioHandler(pdfMethodsService, transferenciaActivoFijo.FechaTransferencia, transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault(), empleado.NombreUsuario);
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, eventHandler);

                Document document = new Document(pdfDocument);
                document.SetBottomMargin(70);

                Table table = new Table(new float[] { 230, 90, 90, 90, 90, 90, 90 });
                table.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 72);

                pdfMethodsService.AdicionarValorCelda("BANCO DE DESARROLLO DEL ECUADOR B.P.", table, colspan: 7, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeArriba: false, pintarBordeAbajo: false, isBold: true, fontSize: 10, isHeaderCell: true);
                pdfMethodsService.AdicionarValorCelda("ACTA ENTREGA RECEPCIÓN DE BIENES.", table, colspan: 7, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeArriba: false, pintarBordeAbajo: false, isBold: true, fontSize: 10, isHeaderCell: true);
                pdfMethodsService.AdicionarValorCelda("REGLAMENTO UTILIZACIÓN Y CONTROL DE LOS BIENES DEL SECTOR PÚBLICO", table, colspan: 7, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeArriba: false, pintarBordeAbajo: false, isBold: true, fontSize: 10, isHeaderCell: true);

                pdfMethodsService.AdicionarCeldaHeader("NOMBRE DE ACTIVO", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("CLASE DE ACTIVO", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("SUCURSAL", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("CÓDIGO CONCATENADO", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("AT_MARCA", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("AT_MODELO", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);
                pdfMethodsService.AdicionarCeldaHeader("AT_SERIE", table, textAlignment: TextAlignment.LEFT, isHeaderCell: true);

                Image imagenCuadrado = new Image(ImageDataFactory.Create(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, $"images\\cuadrado.png"))).SetWidth(7);
                pdfMethodsService.AdicionarValorCelda($"{transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault().UbicacionActivoFijoDestino.Empleado.Persona.Nombres} {transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault().UbicacionActivoFijoDestino.Empleado.Persona.Apellidos}", table, textAlignment: TextAlignment.LEFT, pintarBordeArriba: false, pintarBordeAbajo: false, colspan: 7, backgroundColor: new DeviceRgb(241, 237, 252), imagen: imagenCuadrado, isBold: true);

                var igrouping = transferenciaActivoFijo.TransferenciaActivoFijoDetalle.GroupBy(c => c.RecepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.CategoriaActivoFijo.Nombre);
                foreach (var item in igrouping)
                {
                    pdfMethodsService.AdicionarValorCelda(item.Key, table, textAlignment: TextAlignment.LEFT, colspan: 7, pintarBordeArriba: false, pintarBordeAbajo: false, backgroundColor: new DeviceRgb(241, 237, 252), paddingLeft: 9, isBold: true, imagen: imagenCuadrado);
                    foreach (var item1 in item)
                    {
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.ActivoFijo.Nombre, table, TextAlignment.LEFT, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.Nombre, table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.SucursalActual.Nombre, table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.CodigoActivoFijo.Codigosecuencial, table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.ActivoFijo.Modelo.Marca.Nombre, table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle.ActivoFijo.Modelo.Nombre, table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeDerecho: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                        pdfMethodsService.AdicionarValorCelda(item1.RecepcionActivoFijoDetalle?.Serie ?? "-", table, TextAlignment.LEFT, pintarBordeIzquierdo: false, pintarBordeAbajo: false, pintarBordeArriba: false, paddingLeft: 18);
                    }
                    pdfMethodsService.AdicionarValorCelda($"SUBTOTAL: {item.Count()}", table, colspan: 7, textAlignment: TextAlignment.RIGHT, pintarBordeAbajo: false, pintarBordeArriba: false, backgroundColor: new DeviceRgb(241, 237, 252), isBold: true);
                }
                pdfMethodsService.AdicionarValorCelda($"TOTAL: {igrouping.Count()}", table, colspan: 7, textAlignment: TextAlignment.RIGHT, pintarBordeArriba: false, backgroundColor: new DeviceRgb(241, 237, 252), isBold: true);
                document.Add(table);

                document.Close();
                pdfDocument.Close();
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }

        [HttpPost]
        [Route("PDFTransferenciaCambioUbicacionSucursales")]
        public async Task<byte[]> ExportarPDFTransferenciaCambioUbicacion([FromBody] int idTransferenciaActivoFijo)
        {
            try
            {
                return await ExportarPDFTransferenciaCambioCustodio(idTransferenciaActivoFijo);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new byte[0];
            }
        }
        #endregion

        #region Exportar a Excel
        [HttpPost]
        [Route("ExcelMovilizacion")]
        public async Task<byte[]> PostExcelMovilizacion([FromBody] int idMovilizacionActivoFijo)
        {
            try
            {
                var response = await GetMovilizacionActivoFijo(idMovilizacionActivoFijo);
                var movilizacionActivoFijo = response.Resultado as MovilizacionActivoFijo;

                var fontArial12 = excelMethodsService.ArialFont(12);
                var fontArial14 = excelMethodsService.ArialFont(14);
                var fontArial16 = excelMethodsService.ArialFont(16);
                var fontArial20 = excelMethodsService.ArialFont(20);

                var controlBienes = "CONTROL DE BIENES";
                var usuarioFinal = "USUARIO FINAL";

                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    double cantWorkSheet = (movilizacionActivoFijo.MovilizacionActivoFijoDetalle.Count / 6);
                    cantWorkSheet = movilizacionActivoFijo.MovilizacionActivoFijoDetalle.Count % 6 == 0 ? cantWorkSheet : cantWorkSheet + 1;

                    int posMovilizacionTabla = 0;
                    int posMovilizacionTablaAnterior = 0;
                    for (int i = 0; i < cantWorkSheet; i++)
                    {
                        ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add($"HOJA{i+1}");
                        Func<ExcelWorksheet, int, int, string, int> accion = (ws, fila, columna, footer) =>
                        {
                            var filaOriginal = fila;
                            var columnaOriginal = columna;

                            fila = footer == usuarioFinal ? fila + 1 : fila + 2;
                            columna += 2;

                            var excelRange = excelMethodsService.Ajustar(ws, controlBienes, fila, columna, fila, columna + 11, font: fontArial20, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "AUTORIZACIÓN PARA TRASLADAR BIENES FUERA DE LA INSTITUCIÓN", fila, columna, fila, columna + 11, font: fontArial16, isBold: true, excelHorizontalAlignment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, isMerge: true);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 14.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "AUTORIZADO A:", fila, columna, font: fontArial14, isBold: true);
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "CÉDULA DE CIUDADANÍA:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo?.EmpleadoResponsable?.Persona?.Identificacion ?? String.Empty, fila, columna + 2, fila, columna + 4, font: fontArial14, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "FECHA:", fila, columna + 6, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo.FechaSalida.ToString("dd/MM/yyyy"), fila, columna + 8, fila, columna + 10, font: fontArial14, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "NOMBRE:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            string usuarioEmpleadoResponsable = movilizacionActivoFijo?.EmpleadoResponsable?.NombreUsuario ?? String.Empty;
                            if (!String.IsNullOrEmpty(usuarioEmpleadoResponsable))
                                usuarioEmpleadoResponsable = $" ({usuarioEmpleadoResponsable})";

                            excelRange = excelMethodsService.Ajustar(ws, $"{movilizacionActivoFijo.EmpleadoResponsable.Persona.Nombres} {movilizacionActivoFijo.EmpleadoResponsable.Persona.Apellidos}{usuarioEmpleadoResponsable}", fila, columna + 2, fila, columna + 4, font: fontArial12, isMerge: true);
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "SUCURSAL:", fila, columna + 6, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo?.EmpleadoResponsable?.Dependencia?.Sucursal?.Nombre ?? String.Empty, fila, columna + 8, fila, columna + 10, font: fontArial12, isMerge: true);
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "ÁREA REQUIRIENTE:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo?.EmpleadoResponsable?.Dependencia?.Nombre ?? String.Empty, fila, columna + 2, fila, columna + 4, font: fontArial12, isMerge: true);
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "JEFE INMEDIATO:", fila, columna + 6, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));

                            string usuarioEmpleadoSolicita = movilizacionActivoFijo?.EmpleadoSolicita?.NombreUsuario ?? String.Empty;
                            if (!String.IsNullOrEmpty(usuarioEmpleadoSolicita))
                                usuarioEmpleadoSolicita = $" ({usuarioEmpleadoSolicita})";

                            excelRange = excelMethodsService.Ajustar(ws, $"{movilizacionActivoFijo.EmpleadoSolicita.Persona.Nombres} {movilizacionActivoFijo.EmpleadoSolicita.Persona.Apellidos}{usuarioEmpleadoSolicita}", fila, columna + 8, fila, columna + 10, font: fontArial12, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "MOTIVO DEL TRASLADO:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo.MotivoTraslado.Descripcion, fila, columna + 2, fila, columna + 11, font: fontArial14, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "AUTORIZACIÓN:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 37.50D;

                            excelRange = excelMethodsService.Ajustar(ws, "Se autoriza la salida y movilización de los bienes de larga duración de propiedad del Banco del Desarrollo del Ecuador B.P., detallados a continuación, para uso exclusivo de actividades oficiales e institucionales:", fila, columna + 2, fila, columna + 11, font: fontArial14, isMerge: true, isWrapText: true);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "CÓDIGO INSTITUCIONAL", fila, columna, fila, columna + 1, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "DESCRIPCIÓN", fila, columna + 2, fila, columna + 5, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "MARCA", fila, columna + 6, font: fontArial14, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelRange = excelMethodsService.Ajustar(ws, "MODELO", fila, columna + 7, fila, columna + 8, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "No. SERIE", fila, columna + 9, font: fontArial14, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "ACCESORIOS", fila, columna + 10, font: fontArial14, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = excelMethodsService.Ajustar(ws, "OBSERVACIONES", fila, columna + 11, font: fontArial14, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            excelRange = ws.Cells[fila, columna, fila, columna + 11];
                            excelRange.Worksheet.Row(fila).Height = 35.25D;
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            var posMovilizacionTablaAux = posMovilizacionTabla;
                            var filaAux = fila;
                            for (int j = posMovilizacionTablaAux; j < (posMovilizacionTablaAux + 6); j++)
                            {
                                var movilizacionActivoFijoDetalle = movilizacionActivoFijo.MovilizacionActivoFijoDetalle.ElementAtOrDefault(j);
                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle?.CodigoActivoFijo?.Codigosecuencial ?? String.Empty, fila, columna, fila, columna + 1, font: fontArial14, isMerge: true, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle?.ActivoFijo?.Nombre ?? String.Empty, fila, columna + 2, fila, columna + 5, font: fontArial14, isMerge: true, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle?.ActivoFijo?.Modelo?.Marca?.Nombre ?? String.Empty, fila, columna + 6, font: fontArial14, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle?.ActivoFijo?.Modelo?.Nombre ?? String.Empty, fila, columna + 7, fila, columna + 8, font: fontArial14, isMerge: true, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle?.Serie ?? String.Empty, fila, columna + 9, font: fontArial14, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle != null ? ObtenerComponentesRecepcionActivoFijo(movilizacionActivoFijoDetalle?.RecepcionActivoFijoDetalle).Result : String.Empty, fila, columna + 10, font: fontArial14, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijoDetalle?.Observaciones ?? String.Empty, fila, columna + 11, font: fontArial14, isWrapText: true);
                                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                excelRange.Worksheet.Row(fila).Height = 30.75D;

                                posMovilizacionTabla++;
                                fila++;
                            }

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 19.50D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "FECHA DE SALIDA:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo?.FechaSalida.ToString("dd/MM/yyyy") ?? String.Empty, fila, columna + 2, font: fontArial14, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 8.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "FECHA DE RETORNO:", fila, columna, font: fontArial14, isBold: true);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 30D;

                            excelRange = excelMethodsService.Ajustar(ws, movilizacionActivoFijo?.FechaRetorno.ToString("dd/MM/yyyy") ?? String.Empty, fila, columna + 2, font: fontArial14, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            ws.Cells[fila, columna].Worksheet.Row(fila).Height = 29.25D;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "SOLICITADO POR:", fila, columna + 2, font: fontArial14, isBold: true);
                            excelRange = excelMethodsService.Ajustar(ws, "RETIRADO POR:", fila, columna + 6, font: fontArial14, isBold: true);
                            excelRange = excelMethodsService.Ajustar(ws, "AUTORIZADO POR:", fila, columna + 10, font: fontArial14, isBold: true);
                            excelRange.Worksheet.Row(fila).Height = 19.50D;
                            fila += 3;

                            excelRange = excelMethodsService.Ajustar(ws, $"{movilizacionActivoFijo.EmpleadoSolicita.Persona.Nombres} {movilizacionActivoFijo.EmpleadoSolicita.Persona.Apellidos}{usuarioEmpleadoSolicita}", fila, columna, fila, columna + 3, font: fontArial14, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Worksheet.Row(fila).Height = 19.50D;

                            excelRange = excelMethodsService.Ajustar(ws, $"{movilizacionActivoFijo.EmpleadoResponsable.Persona.Nombres} {movilizacionActivoFijo.EmpleadoResponsable.Persona.Apellidos}{usuarioEmpleadoResponsable}", fila, columna + 4, fila, columna + 8, font: fontArial14, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);

                            string usuarioEmpleadoAutoriza = movilizacionActivoFijo?.EmpleadoAutorizado?.NombreUsuario ?? String.Empty;
                            if (!String.IsNullOrEmpty(usuarioEmpleadoAutoriza))
                                usuarioEmpleadoAutoriza = $" ({usuarioEmpleadoAutoriza})";
                            excelRange = excelMethodsService.Ajustar(ws, $"{movilizacionActivoFijo.EmpleadoAutorizado.Persona.Nombres} {movilizacionActivoFijo.EmpleadoAutorizado.Persona.Apellidos}{usuarioEmpleadoAutoriza}", fila, columna + 9, fila, columna + 11, font: fontArial14, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            ws.Cells[fila, columna + 10].Style.Border.Bottom.Style = ExcelBorderStyle.Dashed;
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "JEFE INMEDIATO", fila, columna, fila, columna + 3, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Worksheet.Row(fila).Height = 19.50D;
                            excelRange = excelMethodsService.Ajustar(ws, "CUSTODIO RESPONSABLE", fila, columna + 4, fila, columna + 8, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange = excelMethodsService.Ajustar(ws, controlBienes, fila, columna + 9, fila, columna + 11, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "DIRECCIÓN DE BIENES Y SERVICIO", fila, columna, fila, columna + 3, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange = excelMethodsService.Ajustar(ws, "DIRECCIÓN DE BIENES Y SERVICIO", fila, columna + 4, fila, columna + 8, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange = excelMethodsService.Ajustar(ws, "DIRECCIÓN DE BIENES Y SERVICIO", fila, columna + 9, fila, columna + 11, font: fontArial14, isBold: true, isMerge: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Worksheet.Row(fila).Height = 19.50D;
                            fila += 3;

                            excelRange = excelMethodsService.Ajustar(ws, "APLICACIÓN DE NORMATIVA:", fila, columna, font: fontArial14, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange = excelMethodsService.Ajustar(ws, "- Reglamento General para la administración, utilización y control de los bienes y existencias del sector público.", fila, columna + 2, fila, columna + 11, font: fontArial12, isBold: true, isMerge: true);
                            fila++;
                            excelRange = excelMethodsService.Ajustar(ws, "- Normas Técnicas de Control Interno.", fila, columna + 2, fila, columna + 10, font: fontArial12, isBold: true, isMerge: true);

                            excelRange = excelMethodsService.Ajustar(ws, footer, fila, columna + 11, font: fontArial12, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center);
                            excelRange.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                            excelRange.Worksheet.Row(fila).Height = 20.25D;
                            fila++;

                            ws.Cells[filaOriginal, columnaOriginal].Worksheet.Row(filaOriginal).Height = footer == controlBienes ? 19.50D : 9.75D;

                            if (footer == controlBienes)
                                ws.Cells[filaOriginal, columnaOriginal + 1].Worksheet.Row(filaOriginal + 1).Height = 9.75D;

                            ws.Cells[filaOriginal, columnaOriginal].Worksheet.Column(columnaOriginal).Width = 2.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 1].Worksheet.Column(columnaOriginal + 1).Width = 2.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 2].Worksheet.Column(columnaOriginal + 2).Width = 39.84D;
                            ws.Cells[filaOriginal, columnaOriginal + 3].Worksheet.Column(columnaOriginal + 3).Width = 1.45D;
                            ws.Cells[filaOriginal, columnaOriginal + 4].Worksheet.Column(columnaOriginal + 4).Width = 25.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 5].Worksheet.Column(columnaOriginal + 5).Width = 9.84D;
                            ws.Cells[filaOriginal, columnaOriginal + 6].Worksheet.Column(columnaOriginal + 6).Width = 22.84D;
                            ws.Cells[filaOriginal, columnaOriginal + 7].Worksheet.Column(columnaOriginal + 7).Width = 4.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 8].Worksheet.Column(columnaOriginal + 8).Width = 25.27D;
                            ws.Cells[filaOriginal, columnaOriginal + 9].Worksheet.Column(columnaOriginal + 9).Width = 1.53D;
                            ws.Cells[filaOriginal, columnaOriginal + 10].Worksheet.Column(columnaOriginal + 10).Width = 18.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 11].Worksheet.Column(columnaOriginal + 11).Width = 22.84D;
                            ws.Cells[filaOriginal, columnaOriginal + 12].Worksheet.Column(columnaOriginal + 12).Width = 27.99D;
                            ws.Cells[filaOriginal, columnaOriginal + 13].Worksheet.Column(columnaOriginal + 13).Width = 24.70D;
                            ws.Cells[filaOriginal, columnaOriginal + 14].Worksheet.Column(columnaOriginal + 14).Width = 2.84D;
                            ws.Cells[filaOriginal, columnaOriginal + 15].Worksheet.Column(columnaOriginal + 15).Width = 3.27D;
                            ws.Cells[filaOriginal, columnaOriginal + 16].Worksheet.Column(columnaOriginal + 16).Width = 3.27D;
                            ws.Cells[filaOriginal, columnaOriginal + 17].Worksheet.Column(columnaOriginal + 17).Width = 3.27D;

                            var finfo = new System.IO.FileInfo(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "images\\logoExcelBDE.png"));
                            var picture = ws.Drawings.AddPicture($"Imagen_{filaOriginal}", finfo);
                            picture.SetSize(250, 95);
                            picture.SetPosition(footer == usuarioFinal ? filaOriginal - 1 : filaOriginal, 0, columnaOriginal + 12, -60);

                            ws.Cells[footer == usuarioFinal ? filaOriginal : filaOriginal + 1, columnaOriginal + 1, fila, 15].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                            return fila;
                        };

                        Action<ExcelWorksheet, int, int> accionDetalles = (ws, fila, columna) =>
                        {
                            var filaOriginal = fila;
                            var columnaOriginal = columna;

                            var excelRange = excelMethodsService.Ajustar(ws, "APLICACIÓN DE NORMATIVA LEGAL VIGENTE.", fila, columna, fila + 1, columna + 9, font: fontArial16, isBold: true, excelHorizontalAlignment: ExcelHorizontalAlignment.Center, isMerge: true);
                            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                            fila += 2;

                            excelRange = excelMethodsService.Ajustar(ws, "Reglamento General para la administración, utilización y control de los bienes y existencias del sector publico.", fila, columna, fila + 1, columna + 5, font: fontArial14, isBold: true, isMerge: true, isWrapText: true, isUnderLine: true, isItalic: true, excelVerticalAlignment: ExcelVerticalAlignment.Bottom);
                            excelRange = excelMethodsService.Ajustar(ws, "Normas Técnicas de Control Interno.", fila, columna + 7, fila + 1, columna + 9, font: fontArial14, isBold: true, isMerge: true, isWrapText: true, isUnderLine: true, isItalic: true);
                            fila += 2;

                            excelRange = excelMethodsService.Ajustar(ws, "Art. 2.- De las personas y entidades responsables.", fila, columna, fila, columna + 5, font: fontArial14, isBold: true, isMerge: true, isItalic: true, excelVerticalAlignment: ExcelVerticalAlignment.Bottom);
                            excelRange = excelMethodsService.Ajustar(ws, "406-08-Uso de los bienes de larga duración", fila, columna + 7, fila, columna + 9, font: fontArial14, isBold: true, isMerge: true, isItalic: true, excelVerticalAlignment: ExcelVerticalAlignment.Bottom);
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, $"“El presente reglamento rige para todas las entidades y organismos descritos en el artículo 1 del presente reglamento, servidoras y servidores, obreras y obreros del sector público, a cuyo cargo se encuentre la custodia, uso y control de los bienes del Estado.{System.Environment.NewLine}{System.Environment.NewLine}Por tanto, no habrá servidora y servidor que por razón de su cargo, función o jerarquía se encuentre exenta o exento del cumplimiento de las disposiciones del presente reglamento, de conformidad a lo previsto en el artículo 233 de la Constitución de la República del Ecuador.{System.Environment.NewLine}{System.Environment.NewLine}Para efectos de aplicación de este reglamento, las siguientes personas son las responsables de la administración, registro, control, cuidado y uso de los bienes de cada entidad:{System.Environment.NewLine}{System.Environment.NewLine}Guardalmacén ó Administradora o Administrador de Bienes.- Será el responsable administrativo del control en la inspección, recepción, registro, custodia, distribución, conservación y baja de los bienes.Usuario Final ó Custodio Responsable.- Será el responsable del cuidado, uso, custodia y conservación de los bienes asignados, servidora o servidor de las entidades y organismos del sector público y aquel que por efectos de acuerdos o convenios, se encuentre prestando servicios en la entidad, al cual se le haya asignado bienes para el desempeño de sus funciones y los que por delegación expresa se agreguen a su cuidado.” (…).", fila, columna, fila + 15, columna + 5, font: fontArial14, isMerge: true, isWrapText: true, excelVerticalAlignment: ExcelVerticalAlignment.Top);
                            excelRange = excelMethodsService.Ajustar(ws, $"“En cada entidad pública los bienes de larga duración se utilizarán únicamente en las labores institucionales y por ningún motivo para fines personales, políticos, electorales, religiosos u otras actividades particulares.{System.Environment.NewLine}{System.Environment.NewLine}Solamente el personal autorizado debe tener acceso a los bienes de la institución, debiendo asumir la responsabilidad por su buen uso y conservación.{System.Environment.NewLine}{System.Environment.NewLine}Cada servidora o servidor será responsable del uso, custodia y conservación de los bienes de larga duración que hayan sido entregados para el desempeño de sus funciones, dejando constancia escrita de su recepción; y por ningún motivo serán utilizados para otros fines que no sean los institucionales. El daño, pérdida o destrucción del bien por negligencia comprobada o mal uso, no imputable al deterioro normal de las cosas, será responsabilidad del servidor que lo tiene a su guarda.{System.Environment.NewLine}{System.Environment.NewLine}En el caso de bienes que son utilización indistintamente por varias personas, es responsabilidad del Jefe de la Unidad Administrativa, definir los aspectos relativos a su uso, custodia y verificación, de manera que estos sean utilizados correctamente.{System.Environment.NewLine}{System.Environment.NewLine}El daño, pérdida o destrucción del bien por negligencia comprobada o mal uso, no imputable al deterioro normal de las cosas, será responsabilidad del servidor que lo tiene a su cargo.{System.Environment.NewLine}{System.Environment.NewLine}Los cambios que se produzcan y que alteren la ubicación y naturaleza de los bienes, serán reportados a la dirección correspondiente, por el personal responsable del uso y custodia de los mismos, para que se adopten los correctivos que cada caso requiera.”.", fila, columna + 7, fila + 29, columna + 9, font: fontArial14, isMerge: true, isWrapText: true, excelVerticalAlignment: ExcelVerticalAlignment.Top);
                            fila += 16;

                            excelRange = excelMethodsService.Ajustar(ws, "Art. 16.- Utilización de los bienes y existencias.", fila, columna, fila, columna + 5, font: fontArial14, isBold: true, isMerge: true, isItalic: true, excelVerticalAlignment: ExcelVerticalAlignment.Bottom);
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "“Los bienes y existencias de las entidades y organismos del sector público, se utilizarán únicamente para los fines propios de la Institución. Es prohibido el uso de dichos bienes y existencias para fines políticos, electorales, doctrinarios o religiosos o para actividades particulares y/o extrañas al servicio público o al objetivo misional de la institución”.", fila, columna, fila + 3, columna + 5, font: fontArial14, isMerge: true, isWrapText: true, excelVerticalAlignment: ExcelVerticalAlignment.Top);
                            fila += 4;

                            excelRange = excelMethodsService.Ajustar(ws, "Art. 87.- Denuncia.", fila, columna, fila, columna + 5, font: fontArial14, isBold: true, isMerge: true, isItalic: true, excelVerticalAlignment: ExcelVerticalAlignment.Bottom);
                            fila++;

                            excelRange = excelMethodsService.Ajustar(ws, "“Cuando alguno de los bienes hubiere desaparecido, por hurto, robo, abigeato o por cualquier causa semejante, la servidora o servidor encargado de su custodia, comunicará inmediatamente por escrito este hecho al Guardalmacén o a quien haga sus veces, al jefe inmediato y a la máxima autoridad de la institución o su delegado, con todos los pormenores que fueren del caso, inmediatamente conocido el hecho.” (…).", fila, columna, fila + 5, columna + 5, font: fontArial14, isMerge: true, isWrapText: true, excelVerticalAlignment: ExcelVerticalAlignment.Top);
                            fila += 6;

                            ws.Cells[filaOriginal, columnaOriginal].Worksheet.Column(columnaOriginal).Width = 42.48D;
                            ws.Cells[filaOriginal, columnaOriginal + 1].Worksheet.Column(columnaOriginal + 1).Width = 11.35D;
                            ws.Cells[filaOriginal, columnaOriginal + 2].Worksheet.Column(columnaOriginal + 2).Width = 11.35D;
                            ws.Cells[filaOriginal, columnaOriginal + 3].Worksheet.Column(columnaOriginal + 3).Width = 11.35D;
                            ws.Cells[filaOriginal, columnaOriginal + 4].Worksheet.Column(columnaOriginal + 4).Width = 22.10D;
                            ws.Cells[filaOriginal, columnaOriginal + 5].Worksheet.Column(columnaOriginal + 5).Width = 16.60D;
                            ws.Cells[filaOriginal, columnaOriginal + 6].Worksheet.Column(columnaOriginal + 6).Width = 2.60D;
                            ws.Cells[filaOriginal, columnaOriginal + 7].Worksheet.Column(columnaOriginal + 7).Width = 11.35D;
                            ws.Cells[filaOriginal, columnaOriginal + 8].Worksheet.Column(columnaOriginal + 8).Width = 11.35D;
                            ws.Cells[filaOriginal, columnaOriginal + 9].Worksheet.Column(columnaOriginal + 9).Width = 69.48D;
                            ws.Cells[filaOriginal, columnaOriginal + 10].Worksheet.Column(columnaOriginal + 10).Width = 3.23D;
                            ws.Cells[filaOriginal, columnaOriginal + 11].Worksheet.Column(columnaOriginal + 11).Width = 3.35D;
                        };

                        var row = accion(objWorksheet, 1, 1, controlBienes);
                        accionDetalles(objWorksheet, 2, 19);
                        row++;
                        objWorksheet.Cells[row, 1].Worksheet.Row(row).Height = 42D;

                        var excellRange = excelMethodsService.Ajustar(objWorksheet, String.Empty, row, 1, row, 16, isMerge: true);
                        excellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Dashed;
                        row++;
                        
                        excellRange = excelMethodsService.Ajustar(objWorksheet, String.Empty, row, 1, row, 16, isMerge: true);
                        objWorksheet.Cells[row, 1].Worksheet.Row(row).Height = 42D;
                        row++;

                        posMovilizacionTabla = posMovilizacionTablaAnterior;
                        accionDetalles(objWorksheet, row, 19);
                        row = accion(objWorksheet, row, 1, usuarioFinal);

                        objWorksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                        objWorksheet.PrinterSettings.BottomMargin = 0;
                        objWorksheet.PrinterSettings.FooterMargin = 0;
                        objWorksheet.PrinterSettings.HeaderMargin = 0;
                        objWorksheet.PrinterSettings.LeftMargin = .30M;
                        objWorksheet.PrinterSettings.RightMargin = 0;
                        objWorksheet.PrinterSettings.TopMargin = 0;
                        objWorksheet.PrinterSettings.Scale = 41;
                        objWorksheet.View.ZoomScale = 70;
                        objWorksheet.PrinterSettings.HorizontalCentered = true;
                        objWorksheet.PrinterSettings.VerticalCentered = true;
                        posMovilizacionTablaAnterior = posMovilizacionTabla;
                    }
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