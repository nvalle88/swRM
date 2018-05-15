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
        [Route("ListarActivoFijoPorEstado/{estado}")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorEstado(string estado)
        {
            return await ListarActivosFijos(predicadoRecepcionActivoFijoDetalle: c => c.Estado.Nombre == estado);
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

        [HttpGet("{id}/{estado}")]
        public async Task<Response> GetActivoFijoPorEstado([FromRoute]int id, [FromRoute] string estado)
        {
            return await ObtenerActivoFijo(id, predicadoDetalleActivoFijo: c => c.Estado.Nombre == estado);
        }

        [HttpGet("DetalleActivoFijo/{id}")]
        public async Task<Response> GetDetalleActivoFijo([FromRoute] int id)
        {
            return await ObtenerDetalleActivoFijo(id);
        }

        [HttpGet("DetalleActivoFijoPorEstado/{id}/{estado}")]
        public async Task<Response> GetDetalleActivoFijo([FromRoute] int id, [FromRoute] string estado)
        {
            return await ObtenerDetalleActivoFijo(id, predicadoDetalleActivoFijo: c => c.Estado.Nombre == estado);
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
                    var response = await ObtenerDetalleActivoFijo(item.idRecepcionActivoFijoDetalle);
                    if (response.IsSuccess)
                        lista.Add(new RecepcionActivoFijoDetalleSeleccionado { RecepcionActivoFijoDetalle = response.Resultado as RecepcionActivoFijoDetalle, Seleccionado = item.seleccionado });
                }
            }
            catch (Exception)
            {
                return new List<RecepcionActivoFijoDetalleSeleccionado>();
            }
            return lista;
        }

        [HttpPost]
        [Route("AsignarPoliza")]
        public async Task<Response> AsignarPoliza([FromBody] PolizaSeguroActivoFijo polizaSeguroActivoFijo)
        {
            try
            {
                if (String.IsNullOrEmpty(polizaSeguroActivoFijo.NumeroPoliza))
                    return new Response { IsSuccess = false, Message = "Debe introducir el Número de Póliza." };

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
        [Route("ListarDetallesActivoFijo")]
        public async Task<Response> GetDetallesActivoFijo([FromBody] int[] idsRecepcionActivoFijoDetalle)
        {
            try
            {
                var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                foreach (var item in idsRecepcionActivoFijoDetalle)
                {
                    var response = await ObtenerDetalleActivoFijo(item);
                    if (response.IsSuccess)
                        listaRecepcionActivoFijoDetalle.Add(response.Resultado as RecepcionActivoFijoDetalle);
                }
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = listaRecepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
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
                    var activoFijo = new ActivoFijo { IdSubClaseActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubClaseActivoFijo, IdCodigoActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdCodigoActivoFijo, CodigoActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.CodigoActivoFijo, IdModelo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdModelo, Nombre = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Nombre, ValorCompra = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.ValorCompra, Depreciacion = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Depreciacion };
                    db.ActivoFijo.Add(activoFijo);
                    db.PolizaSeguroActivoFijo.Add(new PolizaSeguroActivoFijo { IdActivo = activoFijo.IdActivoFijo, IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdSubramo, IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro });

                    var recepcionActivoFijo = new RecepcionActivoFijo { IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor, IdMotivoRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoRecepcion, FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion, Cantidad = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Cantidad, ValidacionTecnica = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.ValidacionTecnica, IdFondoFinanciamiento = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdFondoFinanciamiento, OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra };
                    db.RecepcionActivoFijo.Add(recepcionActivoFijo);
                    await db.SaveChangesAsync();
                    
                    for (int i = 0; i < listaRecepcionActivoFijoDetalleTransfer.Count; i++)
                    {
                        var ubicacionActivoFijo = listaRecepcionActivoFijoDetalleTransfer[i].UbicacionActivoFijo.FirstOrDefault();
                        var recepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle { IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo, IdActivoFijo = activoFijo.IdActivoFijo, IdEstado = listaRecepcionActivoFijoDetalleTransfer[i].IdEstado, Serie = listaRecepcionActivoFijoDetalleTransfer[i].Serie, NumeroChasis = listaRecepcionActivoFijoDetalleTransfer[i].NumeroChasis, NumeroMotor = listaRecepcionActivoFijoDetalleTransfer[i].NumeroMotor, Placa = listaRecepcionActivoFijoDetalleTransfer[i].Placa, NumeroClaveCatastral = listaRecepcionActivoFijoDetalleTransfer[i].NumeroClaveCatastral };
                        db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                        db.UbicacionActivoFijo.Add(new UbicacionActivoFijo { IdEmpleado = ubicacionActivoFijo.IdEmpleado, IdBodega = ubicacionActivoFijo.IdBodega, IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle, IdLibroActivoFijo = ubicacionActivoFijo.IdLibroActivoFijo, FechaUbicacion = ubicacionActivoFijo.FechaUbicacion });
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
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El Número de chasis: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroChasis.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroChasis.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroChasis", Valor = "El Número de chasis: ya existe." });
                    }
                }

                if (!String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroMotor))
                {
                    if (recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle == 0)
                    {
                        if (await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroMotor.ToUpper().Trim()))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El Número de motor: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroMotor.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroMotor.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroMotor", Valor = "El Número de motor: ya existe." });
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
                            listaPropiedadValorErrores.Add(new PropiedadValor { Propiedad = "NumeroClaveCatastral", Valor = "El Número de clave catastral: ya existe." });
                    }
                    else
                    {
                        if (await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroClaveCatastral.ToUpper().Trim() == recepcionActivoFijoDetalle.NumeroClaveCatastral.ToUpper().Trim()).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle))
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
                    var listaDetalleActivosFijosEliminar = await db.RecepcionActivoFijoDetalle.Include(c=> c.Estado).Where(c => c.Estado.Nombre == Estados.Recepcionado && c.IdActivoFijo == id).Select(c=> c.IdRecepcionActivoFijoDetalle).Except(listaRecepcionActivoFijoDetalleTransfer.Where(c=> c.IdRecepcionActivoFijoDetalle > 0).Select(c=> c.IdRecepcionActivoFijoDetalle)).ToListAsync();
                    foreach (var item in listaDetalleActivosFijosEliminar)
                    {
                        await DeleteDetalleActivoFijo(item);
                        listaRecepcionActivoFijoDetalleTransfer.Remove(listaRecepcionActivoFijoDetalleTransfer.FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == item));
                    }

                    var activoFijoActualizar = await db.ActivoFijo.Include(c => c.CodigoActivoFijo).FirstOrDefaultAsync(c => c.IdActivoFijo == id);
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
                        var ubicacionActivoFijo = item.UbicacionActivoFijo.FirstOrDefault();
                        if (item.IdRecepcionActivoFijoDetalle == 0)
                        {
                            var nuevaRecepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle { IdRecepcionActivoFijo = item.IdRecepcionActivoFijo, IdActivoFijo = id, IdEstado = item.IdEstado, Serie = item.Serie, NumeroChasis = item.NumeroChasis, NumeroMotor = item.NumeroMotor, Placa = item.Placa, NumeroClaveCatastral = item.NumeroClaveCatastral };
                            db.RecepcionActivoFijoDetalle.Add(nuevaRecepcionActivoFijoDetalle);
                            db.UbicacionActivoFijo.Add(new UbicacionActivoFijo { IdEmpleado = ubicacionActivoFijo.IdEmpleado, IdBodega = ubicacionActivoFijo.IdBodega, IdLibroActivoFijo = ubicacionActivoFijo.IdLibroActivoFijo, FechaUbicacion = ubicacionActivoFijo.FechaUbicacion, IdRecepcionActivoFijoDetalle = nuevaRecepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle });
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                            if (recepcionActivoFijoDetalleActualizar != null)
                            {
                                recepcionActivoFijoDetalleActualizar.IdRecepcionActivoFijo = item.IdRecepcionActivoFijo;
                                recepcionActivoFijoDetalleActualizar.IdActivoFijo = item.IdActivoFijo;
                                recepcionActivoFijoDetalleActualizar.IdEstado = item.IdEstado;
                                recepcionActivoFijoDetalleActualizar.Serie = item.Serie;
                                recepcionActivoFijoDetalleActualizar.NumeroChasis = item.NumeroChasis;
                                recepcionActivoFijoDetalleActualizar.NumeroMotor = item.NumeroMotor;
                                recepcionActivoFijoDetalleActualizar.Placa = item.Placa;
                                recepcionActivoFijoDetalleActualizar.NumeroClaveCatastral = item.NumeroClaveCatastral;
                            }
                            db.Update(recepcionActivoFijoDetalleActualizar);

                            var ubicacionActivoFijoActualizar = await db.UbicacionActivoFijo.FirstOrDefaultAsync(c => c.IdUbicacionActivoFijo == ubicacionActivoFijo.IdUbicacionActivoFijo);
                            if (ubicacionActivoFijoActualizar != null)
                            {
                                ubicacionActivoFijoActualizar.IdEmpleado = ubicacionActivoFijo.IdEmpleado;
                                ubicacionActivoFijoActualizar.IdBodega = ubicacionActivoFijo.IdBodega;
                                ubicacionActivoFijoActualizar.IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle;
                                ubicacionActivoFijoActualizar.IdLibroActivoFijo = ubicacionActivoFijo.IdLibroActivoFijo;
                                ubicacionActivoFijoActualizar.FechaUbicacion = ubicacionActivoFijo.FechaUbicacion;
                            }
                            db.Update(ubicacionActivoFijoActualizar);
                            await db.SaveChangesAsync();
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
                db.AltaActivoFijo.RemoveRange(db.AltaActivoFijo.Include(c=> c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));
                db.BajaActivoFijo.RemoveRange(db.BajaActivoFijo.Include(c => c.RecepcionActivoFijoDetalle).Where(c => c.RecepcionActivoFijoDetalle.IdActivoFijo == respuesta.IdActivoFijo));

                var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdActivoFijo == respuesta.IdActivoFijo).ToListAsync();
                db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);

                var listaRecepcionActivoFijoDetalle = db.RecepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == respuesta.IdActivoFijo);
                int idRecepcionActivoFijo = listaRecepcionActivoFijoDetalle.FirstOrDefault().IdRecepcionActivoFijo;

                db.RecepcionActivoFijoDetalle.RemoveRange(listaRecepcionActivoFijoDetalle);
                db.RecepcionActivoFijo.Remove(db.RecepcionActivoFijo.FirstOrDefault(c=> c.IdRecepcionActivoFijo == idRecepcionActivoFijo));
                db.ActivoFijo.Remove(respuesta);
                db.CodigoActivoFijo.Remove(db.CodigoActivoFijo.FirstOrDefault(c => c.IdCodigoActivoFijo == respuesta.IdCodigoActivoFijo));
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

        [HttpDelete("EliminarDetalleActivoFijo/{id}")]
        public async Task<Response> DeleteDetalleActivoFijo([FromRoute] int id)
        {
            try
            {
                var respuesta = await db.RecepcionActivoFijoDetalle.Include(c => c.ActivoFijo).ThenInclude(c => c.CodigoActivoFijo).Include(c => c.RecepcionActivoFijo).SingleOrDefaultAsync(m => m.IdRecepcionActivoFijoDetalle == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                int contDetallesActivoFijo = db.RecepcionActivoFijoDetalle.Count(c => c.IdActivoFijo == respuesta.IdActivoFijo);
                if (contDetallesActivoFijo == 1)
                {
                    if (respuesta.ActivoFijo.CodigoActivoFijo != null)
                        db.CodigoActivoFijo.Remove(respuesta.ActivoFijo.CodigoActivoFijo);
                }

                db.ComponenteActivoFijo.RemoveRange(db.ComponenteActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalleComponente == respuesta.IdActivoFijo || c.IdRecepcionActivoFijoDetalleOrigen == respuesta.IdRecepcionActivoFijoDetalle));
                db.UbicacionActivoFijo.RemoveRange(db.UbicacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.DepreciacionActivoFijo.RemoveRange(db.DepreciacionActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.MantenimientoActivoFijo.RemoveRange(db.MantenimientoActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.TransferenciaActivoFijo.RemoveRange(db.TransferenciaActivoFijo.Where(c => c.IdUbicacionActivoFijoOrigen == respuesta.IdRecepcionActivoFijoDetalle || c.IdUbicacionActivoFijoDestino == respuesta.IdRecepcionActivoFijoDetalle));
                db.AltaActivoFijo.RemoveRange(db.AltaActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));
                db.BajaActivoFijo.RemoveRange(db.BajaActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == respuesta.IdRecepcionActivoFijoDetalle));

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

        #region Datos Comunes
        private IQueryable<ActivoFijo> ObtenerDatosActivoFijo()
        {
            return db.ActivoFijo
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.CodigoActivoFijo)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.Subramo).ThenInclude(c => c.Ramo)
                    .Include(c => c.PolizaSeguroActivoFijo).ThenInclude(c => c.CompaniaSeguro);
        }
        private IQueryable<RecepcionActivoFijoDetalle> ObtenerListadoDetallesActivosFijos(int idActivoFijo)
        {
            return db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c => c.Estado).Include(c => c.AltaActivoFijo)
                    .Include(c => c.BajaActivoFijo)
                    .Where(c => c.IdActivoFijo == idActivoFijo)
                    .OrderBy(c => c.Estado.Nombre)
                    .OrderBy(c => c.RecepcionActivoFijo.FechaRecepcion)
                    .OrderBy(c => c.AltaActivoFijo.FechaAlta)
                    .OrderBy(c => c.BajaActivoFijo.FechaBaja);
        }
        private async Task<UbicacionActivoFijo> ObtenerUbicacionActualDetalleActivoFijo(int idRecepcionActivoFijoDetalle)
        {
            return await db.UbicacionActivoFijo
                    .Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.Empleado).ThenInclude(c => c.Persona)
                    .Include(c => c.Bodega)
                    .LastOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle);
        }
        private RecepcionActivoFijoDetalle ObtenerDetalleActivoFijo(RecepcionActivoFijoDetalle rafdOld, UbicacionActivoFijo ubicacionActivoFijoDetalle)
        {
            return new RecepcionActivoFijoDetalle
            {
                IdRecepcionActivoFijoDetalle = rafdOld.IdRecepcionActivoFijoDetalle,
                IdActivoFijo = rafdOld.IdActivoFijo,
                IdRecepcionActivoFijo = rafdOld.IdRecepcionActivoFijo,
                NumeroChasis = rafdOld.NumeroChasis,
                NumeroMotor = rafdOld.NumeroMotor,
                Placa = rafdOld.Placa,
                NumeroClaveCatastral = rafdOld.NumeroClaveCatastral,
                Serie = rafdOld.Serie,
                Estado = new Estado { Nombre = rafdOld.Estado.Nombre },
                UbicacionActivoFijoActual = ubicacionActivoFijoDetalle,
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
                SubClaseActivoFijo = new SubClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.Nombre, IdClaseActivoFijo = activoFijo.SubClaseActivoFijo.IdClaseActivoFijo, ClaseActivoFijo = new ClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.Nombre, IdTipoActivoFijo = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo, TipoActivoFijo = new TipoActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.TipoActivoFijo.Nombre } } },
                CodigoActivoFijo = new CodigoActivoFijo { IdCodigoActivoFijo = activoFijo.IdCodigoActivoFijo, Codigosecuencial = activoFijo.CodigoActivoFijo.Codigosecuencial, CodigoBarras = activoFijo.CodigoActivoFijo.CodigoBarras },
                Modelo = new Modelo { Nombre = activoFijo.Modelo.Nombre, IdMarca = activoFijo.Modelo.IdMarca, Marca = new Marca { Nombre = activoFijo.Modelo.Marca.Nombre } },
                PolizaSeguroActivoFijo = new PolizaSeguroActivoFijo { NumeroPoliza = activoFijo.PolizaSeguroActivoFijo.NumeroPoliza, IdSubramo = activoFijo.PolizaSeguroActivoFijo.IdSubramo, IdCompaniaSeguro = activoFijo.PolizaSeguroActivoFijo.IdCompaniaSeguro, Subramo = new Subramo { Nombre = activoFijo.PolizaSeguroActivoFijo.Subramo.Nombre, IdRamo = activoFijo.PolizaSeguroActivoFijo.Subramo.IdRamo, Ramo = new Ramo { Nombre = activoFijo.PolizaSeguroActivoFijo.Subramo.Ramo.Nombre } }, CompaniaSeguro = new CompaniaSeguro { Nombre = activoFijo.PolizaSeguroActivoFijo.CompaniaSeguro.Nombre } },
                RecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle
            };
        }
        #endregion

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
                        listaRecepcionActivoFijoDetalleAF.ForEach(async c => listaRecepcionActivoFijoDetalle.Add(ObtenerDetalleActivoFijo(c, await ObtenerUbicacionActualDetalleActivoFijo(c.IdRecepcionActivoFijoDetalle))));
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
                        listaRecepcionActivoFijoDetalleAF.ForEach(async c => listaRecepcionActivoFijoDetalle.Add(ObtenerDetalleActivoFijo(c, await ObtenerUbicacionActualDetalleActivoFijo(c.IdRecepcionActivoFijoDetalle))));
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
        private async Task<Response> ObtenerDetalleActivoFijo(int idRecepcionActivoFijoDetalle, Expression<Func<RecepcionActivoFijoDetalle, bool>> predicadoDetalleActivoFijo = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var recepcionActivoFijoDetalleBD = db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.FondoFinanciamiento)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.CodigoActivoFijo)
                    .Include(c => c.ActivoFijo)
                    .Include(c => c.Estado)
                    .Include(c => c.RecepcionActivoFijo.Proveedor.Factura)
                    .Include(c=> c.AltaActivoFijo).ThenInclude(c=> c.Factura)
                    .Include(c=> c.BajaActivoFijo);

                var recepcionActivoFijoDetalle = await (predicadoDetalleActivoFijo != null ? recepcionActivoFijoDetalleBD.Where(predicadoDetalleActivoFijo).SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle) : recepcionActivoFijoDetalleBD.SingleOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle));
                recepcionActivoFijoDetalle.UbicacionActivoFijoActual = await ObtenerUbicacionActualDetalleActivoFijo(recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                return new Response { IsSuccess = recepcionActivoFijoDetalle != null, Message = recepcionActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
    }
}