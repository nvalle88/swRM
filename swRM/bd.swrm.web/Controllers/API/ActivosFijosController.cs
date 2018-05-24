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

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ActivosFijos")]
    public class ActivosFijosController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;

        public ActivosFijosController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
        }

        [HttpGet]
        [Route("ListarActivosFijos")]
        public async Task<List<ActivoFijo>> GetActivosFijos()
        {
            try
            {
                return await ListarActivosFijos();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }

        [HttpGet]
        [Route("ListarActivosFijosPorAgrupacion")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorAgrupacion()
        {
            try
            {
                return await ListarActivosFijosPorAgrupacion();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }

        [HttpGet]
        [Route("ListarAltasActivosFijos")]
        public async Task<List<AltaActivoFijo>> GetAltasActivosFijos()
        {
            try
            {
                var lista = new List<AltaActivoFijo>();
                var idsAltas = await db.AltaActivoFijo.Select(c => c.IdAltaActivoFijo).ToListAsync();
                foreach (var item in idsAltas)
                {
                    var response = await GetAltaActivoFijo(item);
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

        [HttpGet]
        [Route("ListarBajasActivosFijos")]
        public async Task<List<BajaActivoFijo>> GetBajasActivosFijos()
        {
            try
            {
                var lista = new List<BajaActivoFijo>();
                var idsAltas = await db.BajaActivoFijo.Select(c => c.IdBajaActivoFijo).ToListAsync();
                foreach (var item in idsAltas)
                {
                    var response = await GetBajaActivoFijo(item);
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

        [HttpGet]
        [Route("ListarRecepcionActivoFijoConPoliza")]
        public async Task<List<ActivoFijo>> GetActivosFijosConPoliza()
        {
            return await ListarActivosFijos(predicadoActivoFijo: c=> c.PolizaSeguroActivoFijo.NumeroPoliza != null, predicadoRecepcionActivoFijoDetalle: c=> c.Estado.Nombre == Estados.Recepcionado);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoSinPoliza")]
        public async Task<List<ActivoFijo>> GetActivosFijosSinPoliza()
        {
            return await ListarActivosFijos(predicadoActivoFijo: c => c.PolizaSeguroActivoFijo.NumeroPoliza == null, predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre == Estados.Recepcionado);
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
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };
                
                var altaActivoFijo = ObtenerAltaActivoFijoActual(await db.AltaActivoFijo.Include(x => x.FacturaActivoFijo).Include(c => c.MotivoAlta).SingleOrDefaultAsync(m => m.IdAltaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleAltaActivoFijo = await db.RecepcionActivoFijoDetalleAltaActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).ThenInclude(c=> c.Estado).Where(c => c.IdAltaActivoFijo == altaActivoFijo.IdAltaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Alta).ToListAsync();
                altaActivoFijo.RecepcionActivoFijoDetalleAltaActivoFijo = new List<RecepcionActivoFijoDetalleAltaActivoFijo>();
                foreach (var item in listadoIdsRecepcionActivoFijoDetalleAltaActivoFijo)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        altaActivoFijo.RecepcionActivoFijoDetalleAltaActivoFijo.Add(new RecepcionActivoFijoDetalleAltaActivoFijo
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                            IdTipoUtilizacionAlta = item.IdTipoUtilizacionAlta,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle,
                            IdUbicacionActivoFijo = item.IdUbicacionActivoFijo,
                            TipoUtilizacionAlta = await db.TipoUtilizacionAlta.FirstOrDefaultAsync(c=> c.IdTipoUtilizacionAlta == item.IdTipoUtilizacionAlta),
                            UbicacionActivoFijo = ObtenerUbicacionActivoFijoActual(await db.UbicacionActivoFijo
                            .Include(c=> c.LibroActivoFijo).ThenInclude(c=> c.Sucursal)
                            .Include(c=> c.Bodega)
                            .Include(c=> c.Empleado).ThenInclude(c=> c.Persona)
                            .FirstOrDefaultAsync(c=> c.IdUbicacionActivoFijo == item.IdUbicacionActivoFijo))
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

        [HttpGet]
        [Route("ObtenerBajaActivosFijos/{id}")]
        public async Task<Response> GetBajaActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var bajaActivoFijo = ObtenerBajaActivoFijoActual(await db.BajaActivoFijo.Include(c => c.MotivoBaja).SingleOrDefaultAsync(m => m.IdBajaActivoFijo == id));
                var listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo = await db.RecepcionActivoFijoDetalleBajaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).ThenInclude(c => c.Estado).Where(c => c.IdBajaActivoFijo == bajaActivoFijo.IdBajaActivoFijo && c.RecepcionActivoFijoDetalle.Estado.Nombre == Estados.Baja).ToListAsync();
                bajaActivoFijo.RecepcionActivoFijoDetalleBajaActivoFijo = new List<RecepcionActivoFijoDetalleBajaActivoFijo>();
                foreach (var item in listadoIdsRecepcionActivoFijoDetalleBajaActivoFijo)
                {
                    var recepcionActivoFijoDetalle = await ObtenerDetalleActivoFijo(item.IdRecepcionActivoFijoDetalle, incluirActivoFijo: true, incluirComponentes: true);
                    if (recepcionActivoFijoDetalle != null)
                    {
                        bajaActivoFijo.RecepcionActivoFijoDetalleBajaActivoFijo.Add(new RecepcionActivoFijoDetalleBajaActivoFijo
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo,
                            RecepcionActivoFijoDetalle = recepcionActivoFijoDetalle
                        });
                    }
                }
                return new Response { IsSuccess = bajaActivoFijo != null, Message = bajaActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = bajaActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ObtenerActivoFijoPorEstado")]
        public async Task<Response> GetActivoFijoPorEstado([FromBody] IdEstadosTransfer idActivoFijoEstadosTransfer)
        {
            return await ObtenerActivoFijo(idActivoFijoEstadosTransfer.Id, predicadoDetalleActivoFijo: c=> idActivoFijoEstadosTransfer.Estados.Contains(c.Estado.Nombre));//c => c.Estado.Nombre == idActivoFijoEstadosTransfer.Estado);
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
        [Route("ListarActivoFijoPorEstado")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorEstado([FromBody] List<string> estados)
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
                var listaIdsExcluirTablaComponenteActivoFijo = await db.ComponenteActivoFijo.Select(c => c.IdRecepcionActivoFijoDetalleOrigen).Except(idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle)).ToListAsync();
                var listaIdsRAFDSeleccionados = idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.ListaIdRecepcionActivoFijoDetalleSeleccionado.Select(c => c.idRecepcionActivoFijoDetalle);
                
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c=> c.Estado.Nombre == Estados.Recepcionado || c.Estado.Nombre == Estados.Alta).ToListAsync();
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    if (!idRecepcionActivoFijoDetalleSeleccionadoIdsComponentesExcluir.IdsComponentesExcluir.Contains(item.IdRecepcionActivoFijoDetalle) && !listaIdsExcluirTablaComponenteActivoFijo.Contains(item.IdRecepcionActivoFijoDetalle))
                    {
                        lista.Add(new RecepcionActivoFijoDetalleSeleccionado
                        {
                            RecepcionActivoFijoDetalle = ObtenerRecepcionActivoFijoDetalle(item, incluirComponentes: true, incluirActivoFijo: true),
                            Seleccionado = listaIdsRAFDSeleccionados.Contains(item.IdRecepcionActivoFijoDetalle)
                        });
                    }
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
                var listaRecepcionActivoFijoDetalle = await ObtenerListadoDetallesActivosFijos(incluirActivoFijo: true).Where(c => c.Estado.Nombre == Estados.Recepcionado || (c.Estado.Nombre == Estados.Alta && idRecepcionActivoFijoDetalleSeleccionadoIdsInicialesAltaBaja.ListaIdRecepcionActivoFijoDetalleSeleccionadoInicialesAltaBaja.Select(x=> x.idRecepcionActivoFijoDetalle).Contains(c.IdRecepcionActivoFijoDetalle))).ToListAsync();
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
        [Route("ListarActivosFijosPorAgrupacionPorEstado")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorAgrupacionPorEstado([FromBody] string estado)
        {
            return await ListarActivosFijosPorAgrupacion(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre == estado);
        }

        [HttpPost]
        [Route("AsignarPoliza")]
        public async Task<Response> AsignarPoliza([FromBody] PolizaSeguroActivoFijo polizaSeguroActivoFijo)
        {
            try
            {
                if (String.IsNullOrEmpty(polizaSeguroActivoFijo.NumeroPoliza))
                    return new Response { IsSuccess = false, Message = "Debe introducir el N�mero de P�liza." };

                var polizaSeguroActivoFijoActualizar = await db.PolizaSeguroActivoFijo.FirstOrDefaultAsync(c=> c.IdActivo == polizaSeguroActivoFijo.IdActivo);
                if (polizaSeguroActivoFijoActualizar != null)
                {
                    if (!(polizaSeguroActivoFijoActualizar.NumeroPoliza == null ? await db.PolizaSeguroActivoFijo.AnyAsync(c => c.NumeroPoliza == polizaSeguroActivoFijo.NumeroPoliza) : await db.PolizaSeguroActivoFijo.Where(c => c.NumeroPoliza == polizaSeguroActivoFijo.NumeroPoliza).AnyAsync(c => c.IdActivo != polizaSeguroActivoFijo.IdActivo)))
                    {
                        try
                        {
                            polizaSeguroActivoFijoActualizar.NumeroPoliza = polizaSeguroActivoFijo.NumeroPoliza;
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
                        IdMotivoRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoRecepcion,
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
                    foreach (var item in altaActivoFijo.RecepcionActivoFijoDetalleAltaActivoFijo)
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

                        db.RecepcionActivoFijoDetalleAltaActivoFijo.Add(new RecepcionActivoFijoDetalleAltaActivoFijo
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
                        await GestionarComponentesRecepcionActivoFijoDetalle(item.RecepcionActivoFijoDetalle);
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
                    foreach (var item in bajaActivoFijo.RecepcionActivoFijoDetalleBajaActivoFijo)
                    {
                        db.RecepcionActivoFijoDetalleBajaActivoFijo.Add(new RecepcionActivoFijoDetalleBajaActivoFijo
                        {
                            IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                            IdBajaActivoFijo = nuevaBajaActivoFijo.IdBajaActivoFijo
                        });
                        var rafd = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        rafd.IdEstado = estadoBaja.IdEstado;
                        await db.SaveChangesAsync();
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

        [HttpPost]
        [Route("AprobacionActivoFijo")]
        public async Task<Response> PostDesaprobacionActivoFijo([FromBody] AprobacionActivoFijoTransfer aprobacionActivoFijoTransfer)
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
                        recepcionActivoFijoActualizar.IdMotivoRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoRecepcion;
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

                        foreach (var item in altaActivoFijo.RecepcionActivoFijoDetalleAltaActivoFijo)
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
                db.UbicacionActivoFijo.RemoveRange(db.UbicacionActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.DepreciacionActivoFijo.RemoveRange(db.DepreciacionActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.MantenimientoActivoFijo.RemoveRange(db.MantenimientoActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.TransferenciaActivoFijo.RemoveRange(db.TransferenciaActivoFijo.Include(c=> c.UbicacionActivoFijoOrigen).ThenInclude(c=> c.RecepcionActivoFijoDetalle).Include(c => c.UbicacionActivoFijoDestino).ThenInclude(c => c.RecepcionActivoFijoDetalle).Where(c => c.UbicacionActivoFijoOrigen.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo || c.UbicacionActivoFijoDestino.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.PolizaSeguroActivoFijo.Remove(db.PolizaSeguroActivoFijo.FirstOrDefault(c => c.IdActivo == respuesta.IdActivoFijo));

                var recepcionActivoFijoDetalleAltaActivoFijo = db.RecepcionActivoFijoDetalleAltaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.AltaActivoFijo).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in recepcionActivoFijoDetalleAltaActivoFijo)
                {
                    var listadoAltasRecepcion = db.AltaActivoFijo.Where(c => c.IdAltaActivoFijo == item.IdAltaActivoFijo);
                    if (listadoAltasRecepcion.Count() == 1)
                    {
                        db.FacturaActivoFijo.Remove(listadoAltasRecepcion.FirstOrDefault().FacturaActivoFijo);
                        db.AltaActivoFijo.Remove(listadoAltasRecepcion.FirstOrDefault());
                    }
                    db.RecepcionActivoFijoDetalleAltaActivoFijo.Remove(item);
                }

                var recepcionActivoFijoDetalleBajaActivoFijo = db.RecepcionActivoFijoDetalleBajaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.BajaActivoFijo).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in recepcionActivoFijoDetalleBajaActivoFijo)
                {
                    var listadoBajasRecepcion = db.BajaActivoFijo.Where(c => c.IdBajaActivoFijo == item.IdBajaActivoFijo);
                    if (listadoBajasRecepcion.Count() == 1)
                        db.BajaActivoFijo.Remove(listadoBajasRecepcion.FirstOrDefault());
                    db.RecepcionActivoFijoDetalleBajaActivoFijo.Remove(item);
                }

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdActivoFijo == respuesta.IdActivoFijo).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                var listaRecepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle.Include(c=> c.CodigoActivoFijo).Where(c => c.IdActivoFijo == respuesta.IdActivoFijo);
                foreach (var item in listaRecepcionActivoFijoDetalle)
                {
                    db.RecepcionActivoFijoDetalle.Remove(item);
                    db.CodigoActivoFijo.Remove(item.CodigoActivoFijo);
                }
                
                db.RecepcionActivoFijo.Remove(db.RecepcionActivoFijo.FirstOrDefault(c=> c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalle.FirstOrDefault().IdRecepcionActivoFijo));
                db.ActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();

                foreach (var item in listaDocumentosActivoFijo)
                    uploadFileService.DeleteFile(Mensaje.CarpetaActivoFijoDocumento, item.Url);
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
                db.TransferenciaActivoFijo.RemoveRange(db.TransferenciaActivoFijo.Where(c => c.IdUbicacionActivoFijoOrigen == respuesta.IdRecepcionActivoFijoDetalle || c.IdUbicacionActivoFijoDestino == respuesta.IdRecepcionActivoFijoDetalle));

                var recepcionActivoFijoDetalleAltaActivoFijo = db.RecepcionActivoFijoDetalleAltaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.AltaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                db.FacturaActivoFijo.Remove(recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FacturaActivoFijo);
                db.AltaActivoFijo.Remove(recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo);
                db.RecepcionActivoFijoDetalleAltaActivoFijo.Remove(recepcionActivoFijoDetalleAltaActivoFijo);

                var recepcionActivoFijoDetalleBajaActivoFijo = db.RecepcionActivoFijoDetalleBajaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Include(c => c.BajaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle);
                db.BajaActivoFijo.Remove(recepcionActivoFijoDetalleBajaActivoFijo.BajaActivoFijo);
                db.RecepcionActivoFijoDetalleBajaActivoFijo.Remove(recepcionActivoFijoDetalleBajaActivoFijo);

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
                    uploadFileService.DeleteFile(Mensaje.CarpetaActivoFijoDocumento, item.Url);
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
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c => c.Estado)
                    .OrderBy(c => c.RecepcionActivoFijo.IdProveedor)
                    .ThenBy(c => c.RecepcionActivoFijo.FechaRecepcion)
                    .ThenBy(c => c.Serie)
                    .ThenBy(c => c.NumeroChasis)
                    .ThenBy(c => c.NumeroMotor)
                    .ThenBy(c => c.Placa)
                    .ThenBy(c => c.NumeroClaveCatastral)
                    .ThenBy(c => c.RecepcionActivoFijo.MotivoRecepcion)
                    .ThenBy(c => c.RecepcionActivoFijo.FondoFinanciamiento)
                    .ThenBy(c => c.Estado.Nombre);

            foreach (var item in recepcionActivoFijoDetalle)
            {
                item.UbicacionActivoFijoActual = ObtenerUbicacionActivoFijoActual(db.UbicacionActivoFijo
                    .Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.Bodega)
                    .LastOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle));
                item.UbicacionActivoFijo.Clear();
                item.SucursalActual = item.UbicacionActivoFijoActual.LibroActivoFijo.Sucursal;

                var ultimaTransferencia = db.TransferenciaActivoFijo.Include(c=> c.CodigoActivoFijo).Include(c => c.UbicacionActivoFijoDestino).LastOrDefault(c => c.UbicacionActivoFijoDestino.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                item.CodigoActivoFijo = ultimaTransferencia != null ? ultimaTransferencia.CodigoActivoFijo : db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == item.IdCodigoActivoFijo);
                item.CodigoActivoFijo.RecepcionActivoFijoDetalle.Clear();
                item.CodigoActivoFijo.TransferenciaActivoFijo.Clear();

                if (incluirAltasActivoFijo != null)
                {
                    if ((bool)incluirAltasActivoFijo)
                    {
                        var recepcionActivoFijoDetalleAltaActivoFijo = db.RecepcionActivoFijoDetalleAltaActivoFijo.Include(c => c.AltaActivoFijo).ThenInclude(c => c.MotivoAlta).Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        if (recepcionActivoFijoDetalleAltaActivoFijo != null)
                        {
                            item.AltaActivoFijoActual = ObtenerAltaActivoFijoActual(recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo);
                            item.AltaActivoFijoActual.RecepcionActivoFijoDetalleAltaActivoFijo.Clear();
                            item.RecepcionActivoFijoDetalleAltaActivoFijo.Clear();
                        }
                    }
                }

                if (incluirBajasActivoFijo != null)
                {
                    if ((bool)incluirBajasActivoFijo)
                    {
                        var recepcionActivoFijoDetalleBajaActivoFijo = db.RecepcionActivoFijoDetalleBajaActivoFijo.Include(c => c.BajaActivoFijo).ThenInclude(c=> c.MotivoBaja).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        if (recepcionActivoFijoDetalleBajaActivoFijo != null)
                        {
                            item.BajaActivoFijoActual = ObtenerBajaActivoFijoActual(recepcionActivoFijoDetalleBajaActivoFijo.BajaActivoFijo);
                            item.BajaActivoFijoActual.RecepcionActivoFijoDetalleBajaActivoFijo.Clear();
                            item.RecepcionActivoFijoDetalleBajaActivoFijo.Clear();
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
                    IdMotivoRecepcion = rafdOld.RecepcionActivoFijo.IdMotivoRecepcion,
                    MotivoRecepcion = new MotivoRecepcion { Descripcion = rafdOld.RecepcionActivoFijo.MotivoRecepcion.Descripcion },
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
                Empleado = ubicacionActivoFijo.Empleado != null ? new Empleado
                {
                    Persona = ubicacionActivoFijo.Empleado.Persona != null ? new Persona
                    {
                        IdPersona = ubicacionActivoFijo.Empleado.Persona.IdPersona,
                        Nombres = ubicacionActivoFijo.Empleado.Persona.Nombres,
                        Apellidos = ubicacionActivoFijo.Empleado.Persona.Apellidos
                    } : null
                } : null
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
            return new AltaActivoFijo
            {
                IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                FechaAlta = altaActivoFijo.FechaAlta,
                IdMotivoAlta = altaActivoFijo.IdMotivoAlta,
                IdFacturaActivoFijo = altaActivoFijo.IdFacturaActivoFijo,
                MotivoAlta = new MotivoAlta { Descripcion = altaActivoFijo.MotivoAlta.Descripcion },
                FacturaActivoFijo = altaActivoFijo.FacturaActivoFijo != null ? new FacturaActivoFijo { NumeroFactura = altaActivoFijo.FacturaActivoFijo.NumeroFactura, FechaFactura = altaActivoFijo.FacturaActivoFijo.FechaFactura } : null,
                RecepcionActivoFijoDetalleAltaActivoFijo = altaActivoFijo.RecepcionActivoFijoDetalleAltaActivoFijo
            };
        }
        private BajaActivoFijo ObtenerBajaActivoFijoActual(BajaActivoFijo bajaActivoFijo)
        {
            return new BajaActivoFijo
            {
                IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo,
                FechaBaja = bajaActivoFijo.FechaBaja,
                IdMotivoBaja = bajaActivoFijo.IdMotivoBaja,
                MemoOficioResolucion = bajaActivoFijo.MemoOficioResolucion,
                MotivoBaja = new MotivoBaja { Nombre = bajaActivoFijo.MotivoBaja.Nombre },
                RecepcionActivoFijoDetalleBajaActivoFijo = bajaActivoFijo.RecepcionActivoFijoDetalleBajaActivoFijo
            };
        }
        #endregion

        #region M�todos
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
        private async Task<List<ActivoFijo>> ListarActivosFijosPorAgrupacion(int? idActivoFijo = null, bool? incluirAltasActivoFijo = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoRecepcionActivoFijoDetalle = null)
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
        private async Task<Response> ObtenerActivoFijo(int id, Expression<Func<ActivoFijo, bool>> predicadoActivoFijo = null, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

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
        private async Task<RecepcionActivoFijoDetalle> ObtenerDetalleActivoFijo(int idRecepcionActivoFijoDetalle, int? idActivoFijo = null, bool? incluirActivoFijo = null, bool? incluirAltasActivoFijo = null, bool? incluirBajasActivoFijo = null, bool? incluirComponentes = null, Expression < Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
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
        private async Task GestionarComponentesRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle item)
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
            }
            foreach (var idcomp in listaExcept)
                db.ComponenteActivoFijo.Remove(await db.ComponenteActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalleOrigen == item.IdRecepcionActivoFijoDetalle && c.IdRecepcionActivoFijoDetalleComponente == idcomp));
            await db.SaveChangesAsync();
        }
        #endregion
    }
}