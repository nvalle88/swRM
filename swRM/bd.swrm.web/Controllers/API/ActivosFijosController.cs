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

        public ActivosFijosController(SwRMDbContext db, IUploadFileService uploadFileService, IEmailSender emailSender, IClaimsTransfer claimsTransfer, IHttpContextAccessor httpContextAccessor, IClonacion clonacionService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
            this.emailSender = emailSender;
            this.claimsTransfer = claimsTransfer;
            this.clonacionService = clonacionService;
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
        public async Task<Response> PostDetalleActivoFijoParaInventario([FromBody] string codigoSecuencial)
        {
            try
            {
                var recepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).FirstOrDefaultAsync(c => c.CodigoActivoFijo.Codigosecuencial == codigoSecuencial), incluirActivoFijo: true);
                return new Response { IsSuccess = recepcionActivoFijoDetalle != null, Message = recepcionActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
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
        [Route("DetallesActivoFijoSeleccionadoPorEstadoAlta")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorEstadoAlta([FromBody] IdRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => (c.Estado.Nombre.ToUpper() == Estados.Recepcionado || (c.Estado.Nombre.ToUpper() == Estados.Alta && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x=> x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))) && c.RecepcionActivoFijo.MotivoAlta.Descripcion.ToUpper() != MotivosAlta.Adicion).ToListAsync();
                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
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
        [Route("DetallesActivoFijoSeleccionadoPorEmpleado")]
        public async Task<List<RecepcionActivoFijoDetalleSeleccionado>> PostDetallesActivoFijoSeleccionadoPorEmpleado([FromBody] CambioCustodioViewModel cambioCustodioViewModel)
        {
            try
            {
                var lista = new List<RecepcionActivoFijoDetalleSeleccionado>();
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).Where(c => c.UbicacionActivoFijoActual.IdEmpleado == cambioCustodioViewModel.IdEmpleadoEntrega).ToListAsync();

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
                        IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.PolizaSeguroActivoFijo.IdSubramo,
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
                        .Include(c=> c.PolizaSeguroActivoFijo).ThenInclude(c=> c.Subramo).ThenInclude(c=> c.Ramo)
                        .Include(c=> c.PolizaSeguroActivoFijo).ThenInclude(c=> c.CompaniaSeguro)
                        .Include(c=> c.MotivoAlta)
                        .Include(c=> c.Proveedor)
                        .Include(c=> c.FondoFinanciamiento)
                        .FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == recepcionActivoFijo.IdRecepcionActivoFijo);
                    var claimTransfer = claimsTransfer.ObtenerClaimsTransferHttpContext();
                    var sucursal = await db.Sucursal.FirstOrDefaultAsync(c => c.IdSucursal == claimTransfer.IdSucursal);

                    await emailSender.SendEmailAsync(ConstantesCorreo.CorreoEncargadoSeguro, "Nueva recepción de Activos Fijos.",
                    $@"Se ha guardado una recepción de Activos Fijos en el sistema de Recursos Materiales con los siguientes datos: \n \n
                            Motivo de recepción: {recepcionAF.MotivoAlta.Descripcion}, \n \n
                            Proveedor: {recepcionAF.Proveedor.Nombre} {recepcionAF.Proveedor.Apellidos}, \n \n
                            Fondo de financiamiento: {recepcionAF.FondoFinanciamiento.Nombre}, \n \n
                            Fecha de recepción: {recepcionAF.FechaRecepcion.ToString("dd-MM-yyyy hh:mm tt")}, \n \n
                            Orden de compra: {recepcionAF.OrdenCompra}, \n \n
                            Sucursal: {sucursal.Nombre}, \n \n
                            Ramo: {recepcionAF.PolizaSeguroActivoFijo.Subramo.Ramo.Nombre}, \n \n
                            Subramo: {recepcionAF.PolizaSeguroActivoFijo.Subramo.Nombre}, \n \n
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
                        polizaSeguroActivoFijoActualizar.IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.PolizaSeguroActivoFijo.IdSubramo;
                        polizaSeguroActivoFijoActualizar.IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro;
                        db.PolizaSeguroActivoFijo.Update(polizaSeguroActivoFijoActualizar);
                    }
                    await db.SaveChangesAsync();

                    var igrouping = listaRecepcionActivoFijoDetalleTransfer.GroupBy(c => new { c.ActivoFijo.IdSubClaseActivoFijo, c.ActivoFijo.IdModelo, c.ActivoFijo.Nombre }).ToList();
                    if (igrouping.Count > 0)
                    {
                        var listaActivosFijosEliminar = await db.ActivoFijo.Include(c => c.RecepcionActivoFijoDetalle)
                        .Where(c => c.RecepcionActivoFijoDetalle.Count > 0 && c.RecepcionActivoFijoDetalle.FirstOrDefault().IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo)
                        .Select(c => c.IdActivoFijo)
                        .Except(listaRecepcionActivoFijoDetalleTransfer.Where(c => c.IdActivoFijo > 0).Select(c => c.IdActivoFijo).Distinct()).ToListAsync();
                        foreach (var item in listaActivosFijosEliminar)
                            await DeleteActivosFijos(item);
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

                                if (listadoRafd[i].RecepcionActivoFijoDetalleEdificio != null)
                                {
                                    var nuevaRecepcionActivoFijoDetalleEdificio = new RecepcionActivoFijoDetalleEdificio
                                    {
                                        IdRecepcionActivoFijoDetalle = listadoRafd[i].IdRecepcionActivoFijoDetalle,
                                        NumeroClaveCatastral = listadoRafd[i].RecepcionActivoFijoDetalleEdificio.NumeroClaveCatastral
                                    };
                                    db.RecepcionActivoFijoDetalleEdificio.Add(nuevaRecepcionActivoFijoDetalleEdificio);
                                }

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
                                }

                                var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == listadoRafd[i].UbicacionActivoFijoActual.IdUbicacionActivoFijo && c.Confirmacion);
                                if (ubicacionActivoFijoActualizar != null)
                                {
                                    ubicacionActivoFijoActualizar.IdEmpleado = listadoRafd[i].UbicacionActivoFijoActual.IdEmpleado;
                                    ubicacionActivoFijoActualizar.IdBodega = listadoRafd[i].UbicacionActivoFijoActual.IdBodega;
                                    ubicacionActivoFijoActualizar.IdRecepcionActivoFijoDetalle = listadoRafd[i].IdRecepcionActivoFijoDetalle;
                                    ubicacionActivoFijoActualizar.FechaUbicacion = listadoRafd[i].UbicacionActivoFijoActual.FechaUbicacion;
                                    db.Update(ubicacionActivoFijoActualizar);
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
                    .Include(c => c.ActivoFijo)
                    .Include(c => c.RecepcionActivoFijo)
                    .Include(c => c.RecepcionActivoFijo.PolizaSeguroActivoFijo)
                    .Include(c => c.RecepcionActivoFijoDetalleEdificio)
                    .Include(c => c.RecepcionActivoFijoDetalleVehiculo)
                    .Include(c => c.CodigoActivoFijo)
                    .FirstOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);

                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.CodigoActivoFijo.Remove(respuesta.CodigoActivoFijo);
                db.ComponenteActivoFijo.RemoveRange(db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleComponente == respuesta.IdActivoFijo || c.IdRecepcionActivoFijoDetalleOrigen == respuesta.IdRecepcionActivoFijoDetalle));
                db.UbicacionActivoFijo.RemoveRange(db.UbicacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.DepreciacionActivoFijo.RemoveRange(db.DepreciacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.MantenimientoActivoFijo.RemoveRange(db.MantenimientoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));

                var transferenciaActivoFijoDetalle = db.TransferenciaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                foreach (var item in transferenciaActivoFijoDetalle)
                {
                    db.TransferenciaActivoFijoDetalle.Remove(item);
                    db.TransferenciaActivoFijo.Remove(item.TransferenciaActivoFijo);
                }

                var altaActivoFijoDetalle = db.AltaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.AltaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                if (altaActivoFijoDetalle != null)
                {
                    db.AltaActivoFijoDetalle.Remove(altaActivoFijoDetalle);
                    db.FacturaActivoFijo.Remove(altaActivoFijoDetalle.AltaActivoFijo.FacturaActivoFijo);
                    db.AltaActivoFijo.Remove(altaActivoFijoDetalle.AltaActivoFijo);
                }

                var bajaActivoFijoDetalle = db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.BajaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                if (bajaActivoFijoDetalle != null)
                {
                    db.BajaActivoFijoDetalle.Remove(bajaActivoFijoDetalle);
                    db.BajaActivoFijo.Remove(bajaActivoFijoDetalle.BajaActivoFijo);
                }

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                if (respuesta.RecepcionActivoFijoDetalleEdificio != null)
                    db.RecepcionActivoFijoDetalleEdificio.Remove(respuesta.RecepcionActivoFijoDetalleEdificio);

                if (respuesta.RecepcionActivoFijoDetalleVehiculo != null)
                    db.RecepcionActivoFijoDetalleVehiculo.Remove(respuesta.RecepcionActivoFijoDetalleVehiculo);

                if (db.RecepcionActivoFijoDetalle.Count(c => c.IdRecepcionActivoFijo == respuesta.IdRecepcionActivoFijo) == 1)
                {
                    db.PolizaSeguroActivoFijo.Remove(respuesta.RecepcionActivoFijo.PolizaSeguroActivoFijo);
                    db.ActivoFijo.Remove(respuesta.ActivoFijo);
                    db.RecepcionActivoFijo.Remove(respuesta.RecepcionActivoFijo);
                }

                db.RecepcionActivoFijoDetalle.Remove(respuesta);
                await db.SaveChangesAsync();

                foreach (var item in listaDocumentosActivoFijo)
                    uploadFileService.DeleteFile(item.Url);
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
                    var idsRecepcionesActivosFijosDetalles = await db.RecepcionActivoFijoDetalle.Where(c => c.IdRecepcionActivoFijo == id).Select(c => c.IdRecepcionActivoFijoDetalle).ToListAsync();
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
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .OrderBy(c=> c.IdSubClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.IdClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo)
                    .ThenBy(c=> c.Nombre);
        }
        private IQueryable<RecepcionActivoFijoDetalle> ObtenerListadoDetallesActivosFijos(int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null, bool incluirClaimsTransferencia = true)
        {
            var recepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoAlta)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c=> c.RecepcionActivoFijo).ThenInclude(c=> c.PolizaSeguroActivoFijo).ThenInclude(c => c.Subramo).ThenInclude(c => c.Ramo)
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
                    if (isAltaInsertar)
                    {
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                        rafd.IdEstado = (await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Alta)).IdEstado;
                        await db.SaveChangesAsync();
                    }

                    if (existeComponente)
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
                if (isBajaInsertar)
                {
                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                    rafd.IdEstado = (await db.Estado.FirstOrDefaultAsync(c => c.Nombre.ToUpper() == Estados.Baja)).IdEstado;
                    await db.SaveChangesAsync();

                    db.BajaActivoFijoDetalle.Add(new BajaActivoFijoDetalle
                    {
                        IdBajaActivoFijo = bajaActivoFijoDetalle.IdBajaActivoFijo,
                        IdRecepcionActivoFijoDetalle = comp.IdRecepcionActivoFijoDetalleComponente
                    });
                    await db.SaveChangesAsync();
                }
                Task.Run(async () => {
                    if (ubicacionComponente != null)
                        ubicacionComponente.IdRecepcionActivoFijoDetalle = 0;

                    var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).Include(c=> c.ComponentesActivoFijoOrigen).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
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
                var recepcionActivoFijoBD = db.RecepcionActivoFijo.Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.Subramo).ThenInclude(c => c.Ramo).Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.CompaniaSeguro).Include(c => c.MotivoAlta).Include(c => c.Proveedor).Include(c => c.FondoFinanciamiento);
                var recepcionActivoFijo = clonacionService.ClonarRecepcionActivoFijo(predicado != null ? await recepcionActivoFijoBD.Where(predicado).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == id) : await recepcionActivoFijoBD.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == id));
                recepcionActivoFijo.RecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();

                var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true, incluirBajasActivoFijo: true).Where(c=> c.IdRecepcionActivoFijo == id);
                var listaRecepcionActivoFijoDetalleAF = await (predicadoDetalleActivoFijo != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoDetalleActivoFijo).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());

                foreach (var item in listaRecepcionActivoFijoDetalleAF)
                    recepcionActivoFijo.RecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true));

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
        private async Task<Response> ObtenerAltaActivoFijo(int id, Expression<Func<AltaActivoFijo, bool>> predicado = null, bool? incluirComponentes = false)
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
                        var tipoUtilizacionAlta = await db.TipoUtilizacionAlta.FirstOrDefaultAsync(c => c.IdTipoUtilizacionAlta == item.IdTipoUtilizacionAlta);
                        altaActivoFijo.AltaActivoFijoDetalle.Add(new AltaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = item.IdTipoUtilizacionAlta,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            IdUbicacionActivoFijo = item.IdUbicacionActivoFijo,
                            TipoUtilizacionAlta = new TipoUtilizacionAlta { Nombre = tipoUtilizacionAlta.Nombre },
                            UbicacionActivoFijo = clonacionService.ClonarUbicacionActivoFijo(await db.UbicacionActivoFijo.Include(c => c.Bodega).ThenInclude(c=> c.Sucursal).Include(c => c.Empleado).ThenInclude(c => c.Persona).Include(c=> c.Empleado).ThenInclude(c=> c.Dependencia).ThenInclude(c=> c.Sucursal).FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo && c.Confirmacion))
                        });
                    }
                }
                return new Response { IsSuccess = altaActivoFijo != null, Message = altaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = altaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        private async Task<Response> ObtenerBajaActivoFijo(int id, Expression<Func<BajaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var bajaActivoFijoBD = db.BajaActivoFijo.Include(c => c.MotivoBaja);
                var bajaActivoFijo = clonacionService.ClonarBajaActivoFijo(predicado != null ? await bajaActivoFijoBD.Where(predicado).FirstOrDefaultAsync(c => c.IdBajaActivoFijo == id) : await bajaActivoFijoBD.FirstOrDefaultAsync(c => c.IdBajaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo = await db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdBajaActivoFijo == bajaActivoFijo.IdBajaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre.ToUpper() == Estados.Baja).ToListAsync();
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
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle
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
                    if (recepcionActivoFijoDetalle != null)
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
                    .Include(c => c.EmpleadoSolicita).ThenInclude(c => c.Persona)
                    .Include(c => c.EmpleadoAutorizado).ThenInclude(c => c.Persona)
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
    }
}