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
    [Route("api/ActivosFijos")]
    public class ActivosFijosController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;
        private readonly IEmailSender emailSender;
        private readonly IClaimsTransfer claimsTransfer;

        public ActivosFijosController(SwRMDbContext db, IUploadFileService uploadFileService, IEmailSender emailSender, IClaimsTransfer claimsTransfer, IHttpContextAccessor httpContextAccessor)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
            this.emailSender = emailSender;
            this.claimsTransfer = claimsTransfer;
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
        [Route("ListarTransferenciasCambiosUbicacionCreadasSolicitudActivosFijos")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciasCambiosUbicacionCreadasSolicitudActivosFijos()
        {
            return await ListarTransferenciasActivoFijo(new TransferenciaEstadoTransfer { MotivoTransferencia = MotivosTransferencia.CambioUbicacion, Estado = Estados.Creada });
        }

        [HttpGet]
        [Route("ListarTransferenciasCambiosUbicacionAceptadasActivosFijos")]
        public async Task<List<TransferenciaActivoFijo>> GetTransferenciasCambiosUbicacionAceptadasActivosFijos()
        {
            return await ListarTransferenciasActivoFijo(new TransferenciaEstadoTransfer { MotivoTransferencia = MotivosTransferencia.CambioUbicacion, Estado = Estados.Aceptada });
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
        public async Task<List<ActivoFijo>> GetActivosFijosConPoliza()
        {
            return await ListarActivosFijos(predicadoActivoFijo: c=> c.PolizaSeguroActivoFijo.NumeroPoliza != null);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoSinPoliza")]
        public async Task<List<ActivoFijo>> GetActivosFijosSinPoliza()
        {
            return await ListarActivosFijos(predicadoActivoFijo: c => c.PolizaSeguroActivoFijo.NumeroPoliza == null);
        }

        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijos([FromRoute]int id)
        {
            return await ObtenerActivoFijo(id);
        }

        [HttpGet]
        [Route("ObtenerAltaActivosFijos/{id}")]
        public async Task<Response> GetAltaActivoFijo([FromRoute] int id)
        {
            return await ObtenerAltaActivoFijo(id);
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
            return await ObtenerTransferenciaActivoFijo(id);
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
            return idActivoFijoEstadosTransfer.Estados.Count > 0 ? await ObtenerActivoFijo(idActivoFijoEstadosTransfer.Id, predicadoDetalleActivoFijo: c=> idActivoFijoEstadosTransfer.Estados.Contains(c.Estado.Nombre)) : await ObtenerActivoFijo(idActivoFijoEstadosTransfer.Id);
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
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).Where(c => idRecepcionActivoFijoDetalleSeleccionadoEstado.Estados.Contains(c.Estado.Nombre)).ToListAsync();
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
            return await ListarActivosFijos(predicadoRecepcionActivoFijoDetalle: c => estados.Contains(c.Estado.Nombre));
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
                .Where(c=> (c.Estado.Nombre == Estados.Recepcionado || c.Estado.Nombre == Estados.Alta)
                && c.RecepcionActivoFijo.MotivoAlta.Descripcion == "Adici�n"
                && c.Estado.Nombre != Estados.ValidacionTecnica
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
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => (c.Estado.Nombre == Estados.Recepcionado || (c.Estado.Nombre == Estados.Alta && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x=> x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))) && c.RecepcionActivoFijo.MotivoAlta.Descripcion != "Adici�n").ToListAsync();
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
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => c.Estado.Nombre == Estados.Alta || (c.Estado.Nombre == Estados.Baja && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x => x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))).ToListAsync();
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
                var listaRecepcionActivoFijoDetalleBD = ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true, incluirAltasActivoFijo: true).Where(c => (c.Estado.Nombre == Estados.Alta));
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
            return await ListarActivosFijosPorAgrupacionSucursalNombre(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre == estado);
        }

        [HttpPost]
        [Route("ListarActivosFijosPorAgrupacionDepreciacion")]
        public async Task<List<ActivoFijo>> PostActivosFijosPorAgrupacionPorEstadoDepreciacion()
        {
            return await ListarActivosFijosPorAgrupacionSucursalNombre(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre == Estados.Alta && c.ActivoFijo.Depreciacion);
        }

        [HttpPost]
        [Route("AsignarPolizaSeguro")]
        public async Task<Response> AsignarPoliza([FromBody] ActivoFijo activoFijo)
        {
            try
            {
                if (String.IsNullOrEmpty(activoFijo?.PolizaSeguroActivoFijo?.NumeroPoliza))
                    return new Response { IsSuccess = false, Message = "Debe introducir el N�mero de P�liza." };

                var polizaSeguroActivoFijoActualizar = await db.PolizaSeguroActivoFijo.FirstOrDefaultAsync(c=> c.IdActivo == activoFijo.IdActivoFijo);
                if (polizaSeguroActivoFijoActualizar != null)
                {
                    if (!(polizaSeguroActivoFijoActualizar.NumeroPoliza == null ? await db.PolizaSeguroActivoFijo.AnyAsync(c => c.NumeroPoliza == activoFijo.PolizaSeguroActivoFijo.NumeroPoliza) : await db.PolizaSeguroActivoFijo.Where(c => c.NumeroPoliza == activoFijo.PolizaSeguroActivoFijo.NumeroPoliza).AnyAsync(c => c.IdActivo != activoFijo.IdActivoFijo)))
                    {
                        try
                        {
                            polizaSeguroActivoFijoActualizar.NumeroPoliza = activoFijo.PolizaSeguroActivoFijo.NumeroPoliza;
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
                    var activoFijo = new ActivoFijo
                    {
                        IdSubClaseActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubClaseActivoFijo,
                        IdModelo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdModelo,
                        Nombre = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Nombre,
                        ValorCompra = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.ValorCompra,
                        Depreciacion = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Depreciacion
                    };
                    db.ActivoFijo.Add(activoFijo);
                    db.PolizaSeguroActivoFijo.Add(new PolizaSeguroActivoFijo
                    {
                        IdActivo = activoFijo.IdActivoFijo,
                        IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdSubramo,
                        IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro
                    });

                    var recepcionActivoFijo = new RecepcionActivoFijo
                    {
                        IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor,
                        IdMotivoAlta = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoAlta,
                        FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion,
                        Cantidad = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Cantidad,
                        ValidacionTecnica = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.ValidacionTecnica,
                        IdFondoFinanciamiento = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdFondoFinanciamiento,
                        OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra
                    };
                    db.RecepcionActivoFijo.Add(recepcionActivoFijo);
                    await db.SaveChangesAsync();
                    
                    for (int i = 0; i < listaRecepcionActivoFijoDetalleTransfer.Count; i++)
                    {
                        var codigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = listaRecepcionActivoFijoDetalleTransfer[i].CodigoActivoFijo.Codigosecuencial };
                        db.CodigoActivoFijo.Add(codigoActivoFijo);
                        await db.SaveChangesAsync();
                    
                        var ubicacionActivoFijo = listaRecepcionActivoFijoDetalleTransfer[i].UbicacionActivoFijoActual;
                        var recepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle
                        {
                            IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo,
                            IdActivoFijo = activoFijo.IdActivoFijo,
                            IdEstado = listaRecepcionActivoFijoDetalleTransfer[i].IdEstado,
                            IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                            Serie = listaRecepcionActivoFijoDetalleTransfer[i].Serie,
                            NumeroChasis = listaRecepcionActivoFijoDetalleTransfer[i].NumeroChasis,
                            NumeroMotor = listaRecepcionActivoFijoDetalleTransfer[i].NumeroMotor,
                            Placa = listaRecepcionActivoFijoDetalleTransfer[i].Placa,
                            NumeroClaveCatastral = listaRecepcionActivoFijoDetalleTransfer[i].NumeroClaveCatastral
                        };
                        db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                        db.UbicacionActivoFijo.Add(new UbicacionActivoFijo
                        {
                            IdEmpleado = ubicacionActivoFijo.IdEmpleado,
                            IdBodega = ubicacionActivoFijo.IdBodega,
                            IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle,
                            IdLibroActivoFijo = ubicacionActivoFijo.IdLibroActivoFijo,
                            FechaUbicacion = ubicacionActivoFijo.FechaUbicacion
                        });
                        await db.SaveChangesAsync();

                        foreach (var item in listaRecepcionActivoFijoDetalleTransfer[i].ComponentesActivoFijoOrigen)
                            item.IdRecepcionActivoFijoDetalleOrigen = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle;
                        db.ComponenteActivoFijo.AddRange(listaRecepcionActivoFijoDetalleTransfer[i].ComponentesActivoFijoOrigen);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                    var response = await ObtenerActivoFijo(activoFijo.IdActivoFijo);
                    if (response.IsSuccess)
                    {
                        var activoFijoBD = (ActivoFijo)response.Resultado;
                        await emailSender.SendEmailAsync("carlos.avila8909@gmail.com", "Nueva recepci�n de Activos Fijos.",
                        $@"Se han recepcionado nuevos Activos Fijos en el sistema de Recursos Materiales con los siguientes datos: \n \n
                            Tipo de activo fijo: {activoFijoBD.SubClaseActivoFijo.ClaseActivoFijo.TipoActivoFijo.Nombre}, \n \n
                            Clase de activo fijo: {activoFijoBD.SubClaseActivoFijo.ClaseActivoFijo.Nombre}, \n \n
                            Subclase de activo fijo: {activoFijoBD.SubClaseActivoFijo.Nombre}, \n \n
                            Sucursal: {activoFijoBD.RecepcionActivoFijoDetalle.FirstOrDefault().SucursalActual.Nombre}, \n \n
                            Proveedor: {activoFijoBD.RecepcionActivoFijoDetalle.FirstOrDefault().RecepcionActivoFijo.Proveedor.Nombre} {activoFijoBD.RecepcionActivoFijoDetalle.FirstOrDefault().RecepcionActivoFijo.Proveedor.Apellidos}, \n \n
                            Fecha de recepci�n: {recepcionActivoFijo.FechaRecepcion.ToString("dd-MM-yyyy hh:mm tt")}, \n \n
                            Activo fijo: {activoFijoBD.Nombre}, \n \n
                            Cantidad: {recepcionActivoFijo.Cantidad}");
                    }
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

                    var estadoAlta = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Alta);
                    foreach (var item in altaActivoFijo.AltaActivoFijoDetalle)
                    {
                        var nuevaUbicacionActivoFijo = new UbicacionActivoFijo
                        {
                            IdEmpleado = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdEmpleado,
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdLibroActivoFijo = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdLibroActivoFijo,
                            FechaUbicacion = nuevaAltaActivoFijo.FechaAlta
                        };
                        db.UbicacionActivoFijo.Add(nuevaUbicacionActivoFijo);
                        await db.SaveChangesAsync();

                        db.AltaActivoFijoDetalle.Add(new AltaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdAltaActivoFijo = nuevaAltaActivoFijo.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = item.IdTipoUtilizacionAlta,
                            IdUbicacionActivoFijo = nuevaUbicacionActivoFijo.IdUbicacionActivoFijo
                        });
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c=> c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        rafd.IdEstado = estadoAlta.IdEstado;
                        rafd.RecepcionActivoFijo.Cantidad--;
                        await db.SaveChangesAsync();
                        await GestionarComponentesRecepcionActivoFijoDetalle(item.RecepcionActivoFijoDetalle, Estados.Alta);
                    }
                    Temporizador.Temporizador.InicializarTemporizadorDepreciacion();
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

                    var estadoBaja = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Baja);
                    foreach (var item in bajaActivoFijo.BajaActivoFijoDetalle)
                    {
                        db.BajaActivoFijoDetalle.Add(new BajaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdBajaActivoFijo = nuevaBajaActivoFijo.IdBajaActivoFijo
                        });
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        rafd.IdEstado = estadoBaja.IdEstado;
                        await db.SaveChangesAsync();

                        var listadoComponentes = await db.ComponenteActivoFijo.Include(c=> c.RecepcionActivoFijoDetalleComponente).Where(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle).ToListAsync();
                        foreach (var comp in listadoComponentes)
                        {
                            comp.RecepcionActivoFijoDetalleComponente.IdEstado = estadoBaja.IdEstado;
                            await db.SaveChangesAsync();
                        }
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
                var ultimaUbicacionOrigen = await db.UbicacionActivoFijo.LastOrDefaultAsync(c => c.IdEmpleado == idEmpleadoEntrega);
                if (ultimaUbicacionOrigen != null)
                {
                    var nuevaUbicacionDestino = new UbicacionActivoFijo
                    {
                        IdEmpleado = idEmpleadoRecibe,
                        IdLibroActivoFijo = ultimaUbicacionOrigen.IdLibroActivoFijo,
                        FechaUbicacion = transferenciaActivoFijo.FechaTransferencia,
                        IdRecepcionActivoFijoDetalle = item
                    };
                    db.UbicacionActivoFijo.Add(nuevaUbicacionDestino);
                    await db.SaveChangesAsync();

                    db.TransferenciaActivoFijoDetalle.Add(new TransferenciaActivoFijoDetalle
                    {
                        IdRecepcionActivoFijoDetalle = item,
                        IdTransferenciaActivoFijo = transferenciaActivoFijo.IdTransferenciaActivoFijo,
                        IdUbicacionActivoFijoOrigen = ultimaUbicacionOrigen.IdUbicacionActivoFijo,
                        IdUbicacionActivoFijoDestino = nuevaUbicacionDestino.IdUbicacionActivoFijo
                    });
                    await db.SaveChangesAsync();
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
                    var nuevaTransferenciaActivoFijo = await InsertarTransferenciaActivoFijo(Estados.Aceptada, MotivosTransferencia.CambioCustodio);
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
                var listaRecepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Include(c=> c.RecepcionActivoFijo).Where(c => c.IdActivoFijo == aprobacionActivoFijoTransfer.IdActivoFijo).ToListAsync();
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    item.Estado = await db.Estado.SingleOrDefaultAsync(c => c.Nombre == aprobacionActivoFijoTransfer.NuevoEstadoActivoFijo);
                    item.IdEstado = item.Estado.IdEstado;
                    item.RecepcionActivoFijo.ValidacionTecnica = aprobacionActivoFijoTransfer.ValidacionTecnica;
                    db.RecepcionActivoFijoDetalle.Update(item);
                    db.RecepcionActivoFijo.Update(item.RecepcionActivoFijo);
                    await db.SaveChangesAsync();
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
                            var transferenciasActivoFijoDetalle = await db.TransferenciaActivoFijoDetalle.Include(c=> c.RecepcionActivoFijoDetalle).ThenInclude(c=> c.CodigoActivoFijo).Include(c => c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c => c.LibroActivoFijo).Where(c => c.IdTransferenciaActivoFijo == transferenciaActivoFijoTransfer.IdTransferenciaActivoFijo).ToListAsync();
                            foreach (var item in transferenciasActivoFijoDetalle)
                            {
                                var ultimaTransferencia = db.TransferenciaActivoFijoDetalle.Include(c => c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).LastOrDefault(c => c.UbicacionActivoFijoDestino.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdCodigoActivoFijo != null);
                                var ultimoCodigoActivoFijo = ultimaTransferencia != null ? ultimaTransferencia.CodigoActivoFijo : item.RecepcionActivoFijoDetalle.CodigoActivoFijo;

                                var arrCodigoActivoFijo = ultimoCodigoActivoFijo.Codigosecuencial.Split('.').ToList();
                                string codigoFinal = String.Join(".", arrCodigoActivoFijo.GetRange(1, arrCodigoActivoFijo.Count - 1));
                                var nuevoCodigoSecuencial = $"{item.UbicacionActivoFijoDestino.LibroActivoFijo.IdSucursal}.{codigoFinal}";

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

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroChasis))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroChasis.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroChasis.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El N�mero de chasis: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroChasis.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroChasis.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El N�mero de chasis: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroMotor))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroMotor.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El N�mero de motor: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroMotor.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El N�mero de motor: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.Placa))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.Placa.ToUpper().Trim() == recepcionActivoFijoDetalle.Placa.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Placa", Valor = "La Placa: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.Placa.ToUpper().Trim() == recepcionActivoFijoDetalle.Placa.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "Placa", Valor = "La Placa: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroClaveCatastral))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroClaveCatastral.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroClaveCatastral.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroClaveCatastral", Valor = "El N�mero de clave catastral: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroClaveCatastral.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroClaveCatastral.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroClaveCatastral", Valor = "El N�mero de clave catastral: ya existe." });
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
                    var listaDetalleActivosFijosEliminar = await db.RecepcionActivoFijoDetalle.Include(c=> c.Estado).Where(c => c.Estado.Nombre == Estados.Recepcionado && c.IdActivoFijo == id).Select(c=> c.IdRecepcionActivoFijoDetalle).Except(listaRecepcionActivoFijoDetalleTransfer.Where(c=> c.IdRecepcionActivoFijoDetalle > 0).Select(c=> c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                    foreach (var item in listaDetalleActivosFijosEliminar)
                    {
                        await DeleteDetalleActivoFijo(item);
                        listaRecepcionActivoFijoDetalleTransfer.Remove(listaRecepcionActivoFijoDetalleTransfer.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item));
                    }
                    var activoFijoActualizar = await db.ActivoFijo.FirstOrDefaultAsync(c => c.IdActivoFijo == id);
                    if (activoFijoActualizar != null)
                    {
                        activoFijoActualizar.Nombre = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Nombre;
                        activoFijoActualizar.ValorCompra = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.ValorCompra;
                        activoFijoActualizar.IdSubClaseActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubClaseActivoFijo;
                        activoFijoActualizar.IdModelo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdModelo;
                        activoFijoActualizar.Depreciacion = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Depreciacion;
                        db.ActivoFijo.Update(activoFijoActualizar);
                    }
                    var recepcionActivoFijoActualizar = await db.RecepcionActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo);
                    if (recepcionActivoFijoActualizar != null)
                    {
                        recepcionActivoFijoActualizar.FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion;
                        recepcionActivoFijoActualizar.Cantidad = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Cantidad;
                        recepcionActivoFijoActualizar.ValidacionTecnica = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.ValidacionTecnica;
                        recepcionActivoFijoActualizar.IdFondoFinanciamiento = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdFondoFinanciamiento;
                        recepcionActivoFijoActualizar.OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra;
                        recepcionActivoFijoActualizar.IdMotivoAlta = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoAlta;
                        recepcionActivoFijoActualizar.IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor;
                        db.RecepcionActivoFijo.Update(recepcionActivoFijoActualizar);
                    }
                    var polizaSeguroActivoFijoActualizar = await db.PolizaSeguroActivoFijo.FirstOrDefaultAsync(c => c.IdActivo == id);
                    if (polizaSeguroActivoFijoActualizar != null)
                    {
                        polizaSeguroActivoFijoActualizar.IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdSubramo;
                        polizaSeguroActivoFijoActualizar.IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro;
                        db.PolizaSeguroActivoFijo.Update(polizaSeguroActivoFijoActualizar);
                    }
                    await db.SaveChangesAsync();
                    foreach (var item in listaRecepcionActivoFijoDetalleTransfer)
                    {
                        if (item.IdRecepcionActivoFijoDetalle == 0)
                        {
                            var codigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = item.CodigoActivoFijo.Codigosecuencial };
                            db.CodigoActivoFijo.Add(codigoActivoFijo);
                            await db.SaveChangesAsync();

                            var nuevaRecepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle
                            {
                                IdRecepcionActivoFijo = item.IdRecepcionActivoFijo,
                                IdActivoFijo = id,
                                IdEstado = item.IdEstado,
                                IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                                Serie = item.Serie,
                                NumeroChasis = item.NumeroChasis,
                                NumeroMotor = item.NumeroMotor,
                                Placa = item.Placa,
                                NumeroClaveCatastral = item.NumeroClaveCatastral
                            };
                            db.RecepcionActivoFijoDetalle.Add(nuevaRecepcionActivoFijoDetalle);
                            db.UbicacionActivoFijo.Add(new UbicacionActivoFijo
                            {
                                IdEmpleado = item.UbicacionActivoFijoActual.IdEmpleado,
                                IdBodega = item.UbicacionActivoFijoActual.IdBodega,
                                IdLibroActivoFijo = item.UbicacionActivoFijoActual.IdLibroActivoFijo,
                                FechaUbicacion = item.UbicacionActivoFijoActual.FechaUbicacion,
                                IdRecepcionActivoFijoDetalle = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle
                            });
                            await db.SaveChangesAsync();
                            foreach (var comp in item.ComponentesActivoFijoOrigen)
                            {
                                comp.IdRecepcionActivoFijoDetalleOrigen = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle;
                                db.ComponenteActivoFijo.Add(comp);
                            }
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var codigoActivoFijoActualizar = await db.CodigoActivoFijo.FirstOrDefaultAsync(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo);
                            if (codigoActivoFijoActualizar != null)
                            {
                                codigoActivoFijoActualizar.Codigosecuencial = item.CodigoActivoFijo.Codigosecuencial;
                                db.CodigoActivoFijo.Update(codigoActivoFijoActualizar);
                            }

                            var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                            if (recepcionActivoFijoDetalleActualizar != null)
                            {
                                recepcionActivoFijoDetalleActualizar.IdRecepcionActivoFijo = item.IdRecepcionActivoFijo;
                                recepcionActivoFijoDetalleActualizar.IdActivoFijo = item.IdActivoFijo;
                                recepcionActivoFijoDetalleActualizar.IdEstado = item.IdEstado;
                                recepcionActivoFijoDetalleActualizar.IdCodigoActivoFijo = item.IdCodigoActivoFijo;
                                recepcionActivoFijoDetalleActualizar.Serie = item.Serie;
                                recepcionActivoFijoDetalleActualizar.NumeroChasis = item.NumeroChasis;
                                recepcionActivoFijoDetalleActualizar.NumeroMotor = item.NumeroMotor;
                                recepcionActivoFijoDetalleActualizar.Placa = item.Placa;
                                recepcionActivoFijoDetalleActualizar.NumeroClaveCatastral = item.NumeroClaveCatastral;
                                db.Update(recepcionActivoFijoDetalleActualizar);
                            }
                            
                            var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.UbicacionActivoFijoActual.IdUbicacionActivoFijo);
                            if (ubicacionActivoFijoActualizar != null)
                            {
                                ubicacionActivoFijoActualizar.IdEmpleado = item.UbicacionActivoFijoActual.IdEmpleado;
                                ubicacionActivoFijoActualizar.IdBodega = item.UbicacionActivoFijoActual.IdBodega;
                                ubicacionActivoFijoActualizar.IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle;
                                ubicacionActivoFijoActualizar.IdLibroActivoFijo = item.UbicacionActivoFijoActual.IdLibroActivoFijo;
                                ubicacionActivoFijoActualizar.FechaUbicacion = item.UbicacionActivoFijoActual.FechaUbicacion;
                                db.Update(ubicacionActivoFijoActualizar);
                            }
                            await GestionarComponentesRecepcionActivoFijoDetalle(item);
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = listaRecepcionActivoFijoDetalleTransfer };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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
                            var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo);
                            if (ubicacionActivoFijoActualizar != null)
                            {
                                ubicacionActivoFijoActualizar.IdEmpleado = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdEmpleado;
                                ubicacionActivoFijoActualizar.IdLibroActivoFijo = item.RecepcionActivoFijoDetalle.UbicacionActivoFijoActual.IdLibroActivoFijo;
                                db.UbicacionActivoFijo.Update(ubicacionActivoFijoActualizar);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = altaActivoFijo };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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

                        var transferenciasActivosFijosDetalles = await db.TransferenciaActivoFijoDetalle.Include(c=> c.UbicacionActivoFijoDestino).Where(c => c.IdTransferenciaActivoFijo == id).ToListAsync();
                        foreach (var item in transferenciasActivosFijosDetalles)
                        {
                            item.UbicacionActivoFijoDestino.IdLibroActivoFijo = cambioUbicacionSucursalViewModel.IdLibroActivoFijoDestino;
                            await db.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = cambioUbicacionSucursalViewModel };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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
                            var inventarioActivoFijoDetalle = await db.InventarioActivoFijoDetalle.SingleOrDefaultAsync(c => c.IdInventarioActivoFijo == item.IdInventarioActivoFijo && c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
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
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
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
                            var movilizacionActivoFijoDetalle = await db.MovilizacionActivoFijoDetalle.SingleOrDefaultAsync(c => c.IdMovilizacionActivoFijo == item.IdMovilizacionActivoFijo && c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
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
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteActivosFijos([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivoFijo.SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.ComponenteActivoFijo.RemoveRange(db.ComponenteActivoFijo.Include(c=> c.RecepcionActivoFijoDetalleOrigen).Include(c=> c.RecepcionActivoFijoDetalleComponente).Where(c => c.RecepcionActivoFijoDetalleOrigen.IdActivoFijo == respuesta.IdActivoFijo || c.RecepcionActivoFijoDetalleComponente.IdActivoFijo == respuesta.IdActivoFijo));
                db.DepreciacionActivoFijo.RemoveRange(db.DepreciacionActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.MantenimientoActivoFijo.RemoveRange(db.MantenimientoActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.PolizaSeguroActivoFijo.Remove(db.PolizaSeguroActivoFijo.FirstOrDefault(c => c.IdActivo == respuesta.IdActivoFijo));

                var transferenciaActivoFijoDetalle = db.TransferenciaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in transferenciaActivoFijoDetalle)
                {
                    var listadoTransferencia = db.TransferenciaActivoFijo.Where(c => c.IdTransferenciaActivoFijo == item.IdTransferenciaActivoFijo);
                    db.TransferenciaActivoFijoDetalle.Remove(item);
                    if (listadoTransferencia.Count() == 1)
                        db.TransferenciaActivoFijo.Remove(listadoTransferencia.FirstOrDefault());
                }

                var altaActivoFijoDetalle = db.AltaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.AltaActivoFijo).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in altaActivoFijoDetalle)
                {
                    var listadoAltasRecepcion = db.AltaActivoFijo.Where(c => c.IdAltaActivoFijo == item.IdAltaActivoFijo);
                    db.AltaActivoFijoDetalle.Remove(item);
                    if (listadoAltasRecepcion.Count() == 1)
                    {
                        db.FacturaActivoFijo.Remove(listadoAltasRecepcion.FirstOrDefault().FacturaActivoFijo);
                        db.AltaActivoFijo.Remove(listadoAltasRecepcion.FirstOrDefault());
                    }
                }

                var bajaActivoFijoDetalle = db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.BajaActivoFijo).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in bajaActivoFijoDetalle)
                {
                    var listadoBajasRecepcion = db.BajaActivoFijo.Where(c => c.IdBajaActivoFijo == item.IdBajaActivoFijo);
                    db.BajaActivoFijoDetalle.Remove(item);
                    if (listadoBajasRecepcion.Count() == 1)
                        db.BajaActivoFijo.Remove(listadoBajasRecepcion.FirstOrDefault());
                }

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdActivoFijo == respuesta.IdActivoFijo).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                var listaRecepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle.Include(c=> c.CodigoActivoFijo).Where(c => c.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    db.RecepcionActivoFijoDetalle.Remove(item);
                    db.CodigoActivoFijo.Remove(item.CodigoActivoFijo);
                }

                db.UbicacionActivoFijo.RemoveRange(db.UbicacionActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.RecepcionActivoFijo.Remove(db.RecepcionActivoFijo.FirstOrDefault(c=> c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalle.FirstOrDefault().IdRecepcionActivoFijo));
                db.ActivoFijo.Remove(respuesta);
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
        [Route("EliminarDetalleActivoFijo/{id}")]
        public async Task<Response> DeleteDetalleActivoFijo([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.RecepcionActivoFijoDetalle.Include(c => c.ActivoFijo).Include(c => c.RecepcionActivoFijo).SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                int contDetallesActivoFijo = db.RecepcionActivoFijoDetalle.Count(c => c.IdActivoFijo == respuesta.IdActivoFijo);
                if (contDetallesActivoFijo == 1)
                {
                    if (respuesta.CodigoActivoFijo != null)
                        db.CodigoActivoFijo.Remove(respuesta.CodigoActivoFijo);
                }

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
                db.AltaActivoFijoDetalle.Remove(altaActivoFijoDetalle);
                db.FacturaActivoFijo.Remove(altaActivoFijoDetalle.AltaActivoFijo.FacturaActivoFijo);
                db.AltaActivoFijo.Remove(altaActivoFijoDetalle.AltaActivoFijo);

                var bajaActivoFijoDetalle = db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.BajaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                db.BajaActivoFijoDetalle.Remove(bajaActivoFijoDetalle);
                db.BajaActivoFijo.Remove(bajaActivoFijoDetalle.BajaActivoFijo);

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                if (contDetallesActivoFijo == 1)
                {
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
        [Route("EliminarMovilizacionActivoFijo/{id}")]
        public async Task<Response> DeleteMovilizacionActivoFijo([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.MovilizacionActivoFijo.SingleOrDefaultAsync(m => m.IdMovilizacionActivoFijo == id);
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

        #region IQueryable<T> Datos Comunes
        private IQueryable<ActivoFijo> ObtenerDatosActivoFijo()
        {
            return db.ActivoFijo
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c=> c.CategoriaActivoFijo)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.Subramo).ThenInclude(c => c.Ramo)
                    .Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.CompaniaSeguro)
                    .OrderBy(c=> c.IdSubClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.IdClaseActivoFijo)
                    .ThenBy(c=> c.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo)
                    .ThenBy(c=> c.Nombre);
        }
        private IQueryable<RecepcionActivoFijoDetalle> ObtenerListadoDetallesActivosFijos(int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null)
        {
            var recepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoAlta)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c => c.Estado)
                    .OrderBy(c => c.RecepcionActivoFijo.IdProveedor)
                    .ThenBy(c => c.RecepcionActivoFijo.FechaRecepcion)
                    .ThenBy(c => c.Serie)
                    .ThenBy(c => c.NumeroChasis)
                    .ThenBy(c => c.NumeroMotor)
                    .ThenBy(c => c.Placa)
                    .ThenBy(c => c.NumeroClaveCatastral)
                    .ThenBy(c => c.RecepcionActivoFijo.MotivoAlta)
                    .ThenBy(c => c.RecepcionActivoFijo.FondoFinanciamiento)
                    .ThenBy(c => c.Estado.Nombre);

            var claimsTransferencia = claimsTransfer.ObtenerClaimsTransferHttpContext();
            foreach (var item in recepcionActivoFijoDetalle)
            {
                item.UbicacionActivoFijoActual = ObtenerUbicacionActivoFijoActual(db.UbicacionActivoFijo
                    .Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.Bodega)
                    .LastOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle));
                item.UbicacionActivoFijo.Clear();
                item.SucursalActual = item.UbicacionActivoFijoActual.LibroActivoFijo.Sucursal;

                var ultimaTransferencia = db.TransferenciaActivoFijoDetalle.Include(c=> c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).LastOrDefault(c => c.UbicacionActivoFijoDestino.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle && c.IdCodigoActivoFijo != null);
                item.CodigoActivoFijo = ObtenerCodigoActivoFijoFinal(ultimaTransferencia != null ? ultimaTransferencia.CodigoActivoFijo : db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo));

                if (incluirAltasActivoFijo != null)
                {
                    if ((bool)incluirAltasActivoFijo)
                    {
                        var recepcionActivoFijoDetalleAltaActivoFijo = db.AltaActivoFijoDetalle.Include(c => c.AltaActivoFijo).ThenInclude(c => c.MotivoAlta).Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        if (recepcionActivoFijoDetalleAltaActivoFijo != null)
                        {
                            item.AltaActivoFijoActual = ObtenerAltaActivoFijoActual(recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo);
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
                            item.BajaActivoFijoActual = ObtenerBajaActivoFijoActual(recepcionActivoFijoDetalleBajaActivoFijo.BajaActivoFijo);
                            item.BajaActivoFijoActual.BajaActivoFijoDetalle.Clear();
                            item.BajaActivoFijoDetalle.Clear();
                        }
                    }
                }

                if (incluirActivoFijo != null)
                {
                    if ((bool)incluirActivoFijo)
                        item.ActivoFijo = ObtenerActivoFijoFinal(ObtenerDatosActivoFijo().FirstOrDefault(c=> c.IdActivoFijo == item.IdActivoFijo), new List<RecepcionActivoFijoDetalle>());
                }
            }
            recepcionActivoFijoDetalle = recepcionActivoFijoDetalle.OrderBy(c => c.UbicacionActivoFijoActual.IdBodega).ThenBy(c => c.UbicacionActivoFijoActual.IdEmpleado);
            
            if (claimsTransferencia != null && claimsTransferencia.IdSucursal != null && claimsTransferencia.IdSucursal > 0)
                return idActivoFijo != null ? recepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == idActivoFijo && c.SucursalActual.IdSucursal == claimsTransferencia.IdSucursal) : recepcionActivoFijoDetalle.Where(c => c.SucursalActual.IdSucursal == claimsTransferencia.IdSucursal);

            return idActivoFijo != null ? recepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == idActivoFijo) : recepcionActivoFijoDetalle;
        }
        #endregion

        #region Objetos de Clonaci�n Comunes
        private RecepcionActivoFijoDetalle ObtenerRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle rafdOld, bool? incluirComponentes = null, bool? incluirActivoFijo = null)
        {
            var recepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle
            {
                IdRecepcionActivoFijoDetalle = rafdOld.IdRecepcionActivoFijoDetalle,
                IdActivoFijo = rafdOld.IdActivoFijo,
                IdRecepcionActivoFijo = rafdOld.IdRecepcionActivoFijo,
                IdCodigoActivoFijo = rafdOld.IdCodigoActivoFijo,
                NumeroChasis = rafdOld.NumeroChasis,
                NumeroMotor = rafdOld.NumeroMotor,
                Placa = rafdOld.Placa,
                NumeroClaveCatastral = rafdOld.NumeroClaveCatastral,
                Serie = rafdOld.Serie,
                Estado = new Estado { Nombre = rafdOld.Estado.Nombre },
                UbicacionActivoFijoActual = rafdOld.UbicacionActivoFijoActual,
                SucursalActual = rafdOld.SucursalActual,
                AltaActivoFijoActual = rafdOld.AltaActivoFijoActual,
                BajaActivoFijoActual = rafdOld.BajaActivoFijoActual,
                CodigoActivoFijo = rafdOld.CodigoActivoFijo,
                RecepcionActivoFijo = new RecepcionActivoFijo
                {
                    FechaRecepcion = rafdOld.RecepcionActivoFijo.FechaRecepcion,
                    Cantidad = rafdOld.RecepcionActivoFijo.Cantidad,
                    ValidacionTecnica = rafdOld.RecepcionActivoFijo.ValidacionTecnica,
                    IdFondoFinanciamiento = rafdOld.RecepcionActivoFijo.IdFondoFinanciamiento,
                    FondoFinanciamiento = new FondoFinanciamiento { Nombre = rafdOld.RecepcionActivoFijo.FondoFinanciamiento.Nombre },
                    OrdenCompra = rafdOld.RecepcionActivoFijo.OrdenCompra,
                    IdMotivoAlta = rafdOld.RecepcionActivoFijo.IdMotivoAlta,
                    MotivoAlta = new MotivoAlta { Descripcion = rafdOld.RecepcionActivoFijo.MotivoAlta.Descripcion },
                    IdProveedor = rafdOld.RecepcionActivoFijo.IdProveedor,
                    Proveedor = new Proveedor { Nombre = rafdOld.RecepcionActivoFijo.Proveedor.Nombre, Apellidos = rafdOld.RecepcionActivoFijo.Proveedor.Apellidos }
                }
            };
            if (incluirComponentes != null)
            {
                if ((bool)incluirComponentes)
                    recepcionActivoFijoDetalle.ComponentesActivoFijoOrigen = ObtenerListadoComponenteActivoFijo(db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == rafdOld.IdRecepcionActivoFijoDetalle).ToList());
            }
            if (incluirActivoFijo != null)
            {
                if ((bool)incluirActivoFijo)
                    recepcionActivoFijoDetalle.ActivoFijo = ObtenerActivoFijoFinal(rafdOld.ActivoFijo, new List<RecepcionActivoFijoDetalle>());
            }
            return recepcionActivoFijoDetalle;
        }
        private ActivoFijo ObtenerActivoFijoFinal(ActivoFijo activoFijo, List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalle)
        {
            return new ActivoFijo
            {
                IdActivoFijo = activoFijo.IdActivoFijo,
                Nombre = activoFijo.Nombre,
                ValorCompra = activoFijo.ValorCompra,
                Depreciacion = activoFijo.Depreciacion,
                IdSubClaseActivoFijo = activoFijo.IdSubClaseActivoFijo,
                IdModelo = activoFijo.IdModelo,
                SubClaseActivoFijo = new SubClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.Nombre, IdClaseActivoFijo = activoFijo.SubClaseActivoFijo.IdClaseActivoFijo, ClaseActivoFijo = new ClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.Nombre, IdCategoriaActivoFijo = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.IdCategoriaActivoFijo, CategoriaActivoFijo = new CategoriaActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.CategoriaActivoFijo.Nombre }, IdTipoActivoFijo = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo, TipoActivoFijo = new TipoActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.TipoActivoFijo.Nombre } } },
                Modelo = new Modelo { Nombre = activoFijo.Modelo.Nombre, IdMarca = activoFijo.Modelo.IdMarca, Marca = new Marca { Nombre = activoFijo.Modelo.Marca.Nombre } },
                PolizaSeguroActivoFijo = new PolizaSeguroActivoFijo { NumeroPoliza = activoFijo.PolizaSeguroActivoFijo.NumeroPoliza, IdSubramo = activoFijo.PolizaSeguroActivoFijo.IdSubramo, IdCompaniaSeguro = activoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro, Subramo = new Subramo { Nombre = activoFijo.PolizaSeguroActivoFijo.Subramo.Nombre, IdRamo = activoFijo.PolizaSeguroActivoFijo.Subramo.IdRamo, Ramo = new Ramo { Nombre = activoFijo.PolizaSeguroActivoFijo.Subramo.Ramo.Nombre } }, CompaniaSeguro = new CompaniaSeguro { Nombre = activoFijo.PolizaSeguroActivoFijo.CompaniaSeguro.Nombre } },
                RecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle
            };
        }
        private UbicacionActivoFijo ObtenerUbicacionActivoFijoActual(UbicacionActivoFijo ubicacionActivoFijo)
        {
            return new UbicacionActivoFijo
            {
                IdUbicacionActivoFijo = ubicacionActivoFijo.IdUbicacionActivoFijo,
                IdEmpleado = ubicacionActivoFijo.IdEmpleado,
                IdBodega = ubicacionActivoFijo.IdBodega,
                IdRecepcionActivoFijoDetalle = ubicacionActivoFijo.IdRecepcionActivoFijoDetalle,
                IdLibroActivoFijo = ubicacionActivoFijo.IdLibroActivoFijo,
                FechaUbicacion = ubicacionActivoFijo.FechaUbicacion,
                LibroActivoFijo = ubicacionActivoFijo.LibroActivoFijo != null ? new LibroActivoFijo
                {
                    IdSucursal = ubicacionActivoFijo.LibroActivoFijo.IdSucursal,
                    Sucursal = ubicacionActivoFijo.LibroActivoFijo.Sucursal != null ? new Sucursal
                    {
                        IdSucursal = ubicacionActivoFijo.LibroActivoFijo.Sucursal.IdSucursal,
                        Nombre = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Nombre,
                        IdCiudad = ubicacionActivoFijo.LibroActivoFijo.Sucursal.IdCiudad,
                        Ciudad = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad != null ? new Ciudad
                        {
                            Nombre = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Nombre,
                            IdProvincia = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.IdProvincia,
                            Provincia = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia != null ? new Provincia
                            {
                                Nombre = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Nombre,
                                IdPais = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.IdPais,
                                Pais = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Pais != null ? new Pais
                                {
                                    Nombre = ubicacionActivoFijo.LibroActivoFijo.Sucursal.Ciudad.Provincia.Pais.Nombre
                                } : null
                            } : null
                        } : null
                    } : null
                } : null,
                Bodega = ubicacionActivoFijo.Bodega != null ? new Bodega
                {
                    Nombre = ubicacionActivoFijo.Bodega.Nombre
                } : null,
                Empleado = ObtenerEmpleadoFinal(ubicacionActivoFijo?.Empleado)
            };
        }
        private List<ComponenteActivoFijo> ObtenerListadoComponenteActivoFijo(List<ComponenteActivoFijo> listaComponenteActivoFijo)
        {
            var nuevaListaComponenteActivoFijo = new List<ComponenteActivoFijo>();
            listaComponenteActivoFijo.ForEach(c => nuevaListaComponenteActivoFijo.Add(new ComponenteActivoFijo
            {
                IdComponenteActivoFijo = c.IdComponenteActivoFijo,
                IdRecepcionActivoFijoDetalleOrigen = c.IdRecepcionActivoFijoDetalleOrigen,
                IdRecepcionActivoFijoDetalleComponente = c.IdRecepcionActivoFijoDetalleComponente
            }));
            return nuevaListaComponenteActivoFijo;
        }
        private AltaActivoFijo ObtenerAltaActivoFijoActual(AltaActivoFijo altaActivoFijo)
        {
            return altaActivoFijo != null ? new AltaActivoFijo
            {
                IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                FechaAlta = altaActivoFijo.FechaAlta,
                IdMotivoAlta = altaActivoFijo.IdMotivoAlta,
                IdFacturaActivoFijo = altaActivoFijo.IdFacturaActivoFijo,
                MotivoAlta = new MotivoAlta { Descripcion = altaActivoFijo.MotivoAlta.Descripcion },
                FacturaActivoFijo = altaActivoFijo.FacturaActivoFijo != null ? new FacturaActivoFijo { NumeroFactura = altaActivoFijo.FacturaActivoFijo.NumeroFactura, FechaFactura = altaActivoFijo.FacturaActivoFijo.FechaFactura } : null,
                AltaActivoFijoDetalle = altaActivoFijo.AltaActivoFijoDetalle
            } : null;
        }
        private BajaActivoFijo ObtenerBajaActivoFijoActual(BajaActivoFijo bajaActivoFijo)
        {
            return bajaActivoFijo != null ? new BajaActivoFijo
            {
                IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo,
                FechaBaja = bajaActivoFijo.FechaBaja,
                IdMotivoBaja = bajaActivoFijo.IdMotivoBaja,
                MemoOficioResolucion = bajaActivoFijo.MemoOficioResolucion,
                MotivoBaja = new MotivoBaja { Nombre = bajaActivoFijo.MotivoBaja.Nombre },
                BajaActivoFijoDetalle = bajaActivoFijo.BajaActivoFijoDetalle
            } : null;
        }
        private TransferenciaActivoFijo ObtenerTransferenciaActivoFijoFinal(TransferenciaActivoFijo transferenciaActivoFijo)
        {
            return transferenciaActivoFijo != null ? new TransferenciaActivoFijo
            {
                IdTransferenciaActivoFijo = transferenciaActivoFijo.IdTransferenciaActivoFijo,
                IdEmpleadoResponsableEnvio = transferenciaActivoFijo.IdEmpleadoResponsableEnvio,
                IdEmpleadoResponsableRecibo = transferenciaActivoFijo.IdEmpleadoResponsableRecibo,
                FechaTransferencia = transferenciaActivoFijo.FechaTransferencia,
                Observaciones = transferenciaActivoFijo.Observaciones,
                IdMotivoTransferencia = transferenciaActivoFijo.IdMotivoTransferencia,
                IdEstado = transferenciaActivoFijo.IdEstado,
                Estado = new Estado { Nombre = transferenciaActivoFijo.Estado.Nombre },
                MotivoTransferencia = new MotivoTransferencia { Motivo_Transferencia = transferenciaActivoFijo.MotivoTransferencia.Motivo_Transferencia }
            } : null;
        }
        private InventarioActivoFijo ObtenerInventarioActivoFijoFinal(InventarioActivoFijo inventarioActivoFijo)
        {
            return inventarioActivoFijo != null ? new InventarioActivoFijo
            {
                IdInventarioActivoFijo = inventarioActivoFijo.IdInventarioActivoFijo,
                FechaCorteInventario = inventarioActivoFijo.FechaCorteInventario,
                FechaInforme = inventarioActivoFijo.FechaInforme,
                NumeroInforme = inventarioActivoFijo.NumeroInforme,
                IdEstado = inventarioActivoFijo.IdEstado,
                InventarioManual = inventarioActivoFijo.InventarioManual,
                Estado = new Estado { Nombre = inventarioActivoFijo.Estado.Nombre },
                InventarioActivoFijoDetalle = inventarioActivoFijo.InventarioActivoFijoDetalle
            } : null;
        }
        private MovilizacionActivoFijo ObtenerMovilizacionActivoFijoFinal(MovilizacionActivoFijo movilizacionActivoFijo)
        {
            return movilizacionActivoFijo != null ? new MovilizacionActivoFijo
            {
                IdMovilizacionActivoFijo = movilizacionActivoFijo.IdMovilizacionActivoFijo,
                IdEmpleadoResponsable = movilizacionActivoFijo.IdEmpleadoResponsable,
                IdEmpleadoSolicita = movilizacionActivoFijo.IdEmpleadoSolicita,
                IdEmpleadoAutorizado = movilizacionActivoFijo.IdEmpleadoAutorizado,
                FechaSalida = movilizacionActivoFijo.FechaSalida,
                FechaRetorno = movilizacionActivoFijo.FechaRetorno,
                IdMotivoTraslado = movilizacionActivoFijo.IdMotivoTraslado,
                MotivoTraslado = new MotivoTraslado { Descripcion = movilizacionActivoFijo.MotivoTraslado.Descripcion },
                EmpleadoResponsable = ObtenerEmpleadoFinal(movilizacionActivoFijo.EmpleadoResponsable),
                EmpleadoSolicita = ObtenerEmpleadoFinal(movilizacionActivoFijo.EmpleadoSolicita),
                EmpleadoAutorizado = ObtenerEmpleadoFinal(movilizacionActivoFijo.EmpleadoAutorizado),
                MovilizacionActivoFijoDetalle = movilizacionActivoFijo.MovilizacionActivoFijoDetalle
            } : null;
        }
        private CodigoActivoFijo ObtenerCodigoActivoFijoFinal(CodigoActivoFijo codigoActivoFijo)
        {
            return codigoActivoFijo != null ? new CodigoActivoFijo
            {
                IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                Codigosecuencial = codigoActivoFijo.Codigosecuencial
            } : null;
        }
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
                        listaActivosFijos.Add(ObtenerActivoFijoFinal(item, listaRecepcionActivoFijoDetalle));
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
                        listaActivosFijos.Add(ObtenerActivoFijoFinal(activoFijo, listaRecepcionActivoFijoDetalle));
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
        private async Task GestionarComponentesRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle item, string nuevoEstado = null)
        {
            var listaIdsComponentesBD = await db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle).Select(c => c.IdRecepcionActivoFijoDetalleComponente).ToListAsync();
            var listaExcept = listaIdsComponentesBD.Except(item.ComponentesActivoFijoOrigen.Select(c => c.IdRecepcionActivoFijoDetalleComponente).ToList());
            foreach (var comp in item.ComponentesActivoFijoOrigen)
            {
                if (!await db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle).AnyAsync(c => c.IdRecepcionActivoFijoDetalleComponente == comp.IdRecepcionActivoFijoDetalleComponente))
                {
                    comp.IdRecepcionActivoFijoDetalleOrigen = item.IdRecepcionActivoFijoDetalle;
                    db.ComponenteActivoFijo.Add(comp);
                }
                if (!String.IsNullOrEmpty(nuevoEstado))
                {
                    if (nuevoEstado == Estados.Alta)
                    {
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == comp.IdRecepcionActivoFijoDetalleComponente);
                        var estadoDB = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Alta);
                        rafd.IdEstado = estadoDB.IdEstado;
                        rafd.RecepcionActivoFijo.Cantidad--;
                    }
                }
            }
            foreach (var idcomp in listaExcept)
            {
                db.ComponenteActivoFijo.Remove(await db.ComponenteActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle && c.IdRecepcionActivoFijoDetalleComponente == idcomp));
                var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idcomp);
                var estadoDB = await db.Estado.FirstOrDefaultAsync(c => c.Nombre == Estados.Recepcionado);
                rafd.IdEstado = estadoDB.IdEstado;
                rafd.RecepcionActivoFijo.Cantidad++;
            }
            await db.SaveChangesAsync();
        }
        private async Task<List<TransferenciaActivoFijo>> ListarTransferenciasActivoFijo(TransferenciaEstadoTransfer transferenciaEstadoTransfer, Expression<Func<TransferenciaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var lista = new List<TransferenciaActivoFijo>();
                var idsTransferencias = db.TransferenciaActivoFijo.Include(c => c.MotivoTransferencia).Include(c => c.Estado).Where(c => c.MotivoTransferencia.Motivo_Transferencia == transferenciaEstadoTransfer.MotivoTransferencia && c.Estado.Nombre == transferenciaEstadoTransfer.Estado);
                var listadoIdsTransferencias = await (predicado != null ? idsTransferencias.Where(predicado).Select(c=> c.IdTransferenciaActivoFijo).ToListAsync() : idsTransferencias.Select(c => c.IdTransferenciaActivoFijo).ToListAsync());

                foreach (var item in listadoIdsTransferencias)
                {
                    var response = await ObtenerTransferenciaActivoFijo(item);
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
        private async Task<List<AltaActivoFijo>> ListarAltasActivoFijo(Expression<Func<AltaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var lista = new List<AltaActivoFijo>();
                var idsAltas = await (predicado != null ? db.AltaActivoFijo.Where(predicado).Select(c => c.IdAltaActivoFijo).ToListAsync() : db.AltaActivoFijo.Select(c => c.IdAltaActivoFijo).ToListAsync());
                foreach (var item in idsAltas)
                {
                    var response = await ObtenerAltaActivoFijo(item);
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
                var activoFijo = await (predicadoActivoFijo != null ? activoFijoBD.Where(predicadoActivoFijo).SingleOrDefaultAsync(m => m.IdActivoFijo == id) : activoFijoBD.SingleOrDefaultAsync(m => m.IdActivoFijo == id));
                if (activoFijo != null)
                {
                    var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                    var listaRecepcionActivoFijoDetalleAFBD = ObtenerListadoDetallesActivosFijos(activoFijo.IdActivoFijo);
                    var listaRecepcionActivoFijoDetalleAF = await (predicadoDetalleActivoFijo != null ? listaRecepcionActivoFijoDetalleAFBD.Where(predicadoDetalleActivoFijo).ToListAsync() : listaRecepcionActivoFijoDetalleAFBD.ToListAsync());

                    if (listaRecepcionActivoFijoDetalleAF.Count > 0)
                    {
                        listaRecepcionActivoFijoDetalleAF.ForEach(c => listaRecepcionActivoFijoDetalle.Add(ObtenerRecepcionActivoFijoDetalle(c, incluirComponentes: true)));
                        activoFijo = ObtenerActivoFijoFinal(activoFijo, listaRecepcionActivoFijoDetalle);
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
        private async Task<RecepcionActivoFijoDetalle> ObtenerDetalleActivoFijo(int idRecepcionActivoFijoDetalle, int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null, bool? incluirComponentes = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                var recepcionActivoFijoDetalleBD = ObtenerListadoDetallesActivosFijos(idActivoFijo: idActivoFijo, incluirActivoFijo: incluirActivoFijo, incluirAltasActivoFijo: incluirAltasActivoFijo, incluirBajasActivoFijo: incluirBajasActivoFijo);
                return ObtenerRecepcionActivoFijoDetalle(await (predicadoDetalleActivoFijo != null ? recepcionActivoFijoDetalleBD.Where(predicadoDetalleActivoFijo).SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle) : recepcionActivoFijoDetalleBD.SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle)), incluirComponentes, incluirActivoFijo: incluirActivoFijo);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private async Task<Response> ObtenerAltaActivoFijo(int id, Expression<Func<AltaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var altaActivoFijoBD = db.AltaActivoFijo.Include(x => x.FacturaActivoFijo).Include(c => c.MotivoAlta);
                var altaActivoFijo = ObtenerAltaActivoFijoActual(predicado != null ? await altaActivoFijoBD.Where(predicado).SingleOrDefaultAsync(c => c.IdAltaActivoFijo == id) : await altaActivoFijoBD.SingleOrDefaultAsync(c => c.IdAltaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleAltaActivoFijo = await db.AltaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdAltaActivoFijo == altaActivoFijo.IdAltaActivoFijo).ToListAsync();
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
                            UbicacionActivoFijo = ObtenerUbicacionActivoFijoActual(await db.UbicacionActivoFijo.Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).Include(c => c.Bodega).Include(c => c.Empleado).ThenInclude(c => c.Persona).FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo))
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
                var bajaActivoFijo = ObtenerBajaActivoFijoActual(predicado != null ? await bajaActivoFijoBD.Where(predicado).SingleOrDefaultAsync(c => c.IdBajaActivoFijo == id) : await bajaActivoFijoBD.SingleOrDefaultAsync(c => c.IdBajaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo = await db.BajaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdBajaActivoFijo == bajaActivoFijo.IdBajaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Baja).ToListAsync();
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
        private async Task<Response> ObtenerTransferenciaActivoFijo(int id, Expression<Func<TransferenciaActivoFijo, bool>> predicado = null)
        {
            try
            {
                var transferenciaBD = db.TransferenciaActivoFijo.Include(c => c.Estado).Include(c => c.MotivoTransferencia);
                var transferenciaActivoFijo = ObtenerTransferenciaActivoFijoFinal(predicado != null ? await transferenciaBD.Where(predicado).SingleOrDefaultAsync(c => c.IdTransferenciaActivoFijo == id) : await transferenciaBD.SingleOrDefaultAsync(c => c.IdTransferenciaActivoFijo == id));

                if (transferenciaActivoFijo.IdEmpleadoResponsableEnvio != null)
                    transferenciaActivoFijo.EmpleadoResponsableEnvio = ObtenerEmpleadoFinal(await db.Empleado.Include(c => c.Persona).FirstOrDefaultAsync(c => c.IdEmpleado == transferenciaActivoFijo.IdEmpleadoResponsableEnvio));

                if (transferenciaActivoFijo.IdEmpleadoResponsableRecibo != null)
                    transferenciaActivoFijo.EmpleadoResponsableRecibo = ObtenerEmpleadoFinal(await db.Empleado.Include(c => c.Persona).FirstOrDefaultAsync(c => c.IdEmpleado == transferenciaActivoFijo.IdEmpleadoResponsableRecibo));

                var listadoIdsTransferenciaActivoFijoDetalle = await db.TransferenciaActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdTransferenciaActivoFijo == transferenciaActivoFijo.IdTransferenciaActivoFijo).ToListAsync();
                foreach (var item in listadoIdsTransferenciaActivoFijoDetalle)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true, incluirAltasActivoFijo: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        transferenciaActivoFijo.TransferenciaActivoFijoDetalle.Add(new TransferenciaActivoFijoDetalle
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdTransferenciaActivoFijo = item.IdTransferenciaActivoFijo,
                            IdUbicacionActivoFijoDestino = item.IdUbicacionActivoFijoDestino,
                            IdUbicacionActivoFijoOrigen = item.IdUbicacionActivoFijoOrigen,
                            IdCodigoActivoFijo = item.IdCodigoActivoFijo,
                            UbicacionActivoFijoOrigen = ObtenerUbicacionActivoFijoActual(await db.UbicacionActivoFijo.Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).Include(c => c.Bodega).Include(c => c.Empleado).ThenInclude(c => c.Persona).FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijoOrigen)),
                            UbicacionActivoFijoDestino = ObtenerUbicacionActivoFijoActual(await db.UbicacionActivoFijo.Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).Include(c => c.Bodega).Include(c => c.Empleado).ThenInclude(c => c.Persona).FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijoDestino)),
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            CodigoActivoFijo = ObtenerCodigoActivoFijoFinal(await db.CodigoActivoFijo.SingleOrDefaultAsync(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo))
                        });
                    }
                }
                transferenciaActivoFijo.SucursalOrigen = transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault().UbicacionActivoFijoOrigen.LibroActivoFijo.Sucursal;
                transferenciaActivoFijo.SucursalDestino = transferenciaActivoFijo.TransferenciaActivoFijoDetalle.FirstOrDefault().UbicacionActivoFijoDestino.LibroActivoFijo.Sucursal;
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
                var inventarioActivoFijo = ObtenerInventarioActivoFijoFinal(predicado != null ? await inventarioAFBD.Where(predicado).SingleOrDefaultAsync(c => c.IdInventarioActivoFijo == id) : await inventarioAFBD.SingleOrDefaultAsync(c => c.IdInventarioActivoFijo == id));
                var listadoIdsInventarioActivoFijoDetalle = await db.InventarioActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdInventarioActivoFijo == inventarioActivoFijo.IdInventarioActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Alta).ToListAsync();
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

                var movilizacionActivoFijo = ObtenerMovilizacionActivoFijoFinal(predicado != null ? await movilizacionAFBD.Where(predicado).SingleOrDefaultAsync(c => c.IdMovilizacionActivoFijo == id) : await movilizacionAFBD.SingleOrDefaultAsync(c => c.IdMovilizacionActivoFijo == id));
                var listadoIdsMovilizacionActivoFijoDetalle = await db.MovilizacionActivoFijoDetalle.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdMovilizacionActivoFijo == movilizacionActivoFijo.IdMovilizacionActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Alta).ToListAsync();
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