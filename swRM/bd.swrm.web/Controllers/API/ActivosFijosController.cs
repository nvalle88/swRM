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
        [Route("ListarActivosFijosRecepcion")]
        public async Task<List<ActivoFijo>> GetActivosFijos()
        {
            try
            {
                var listaActivoFijoRecepcion = new List<ActivoFijo>();
                var listaActivoFijo = await db.ActivoFijo
                    .Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.UnidadMedida)
                    .Include(c => c.CodigoActivoFijo)
                    .Include(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.Subramo).ThenInclude(c => c.Ramo)
                    .Include(c => c.CompaniaSeguro)
                    .ToListAsync();

                foreach (var item in listaActivoFijo)
                {
                    var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                    var listaRecepcionActivoFijoDetalleAF = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor).Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion).Include(c => c.Estado).Where(c => c.IdActivoFijo == item.IdActivoFijo).ToListAsync();
                    foreach (var rafd in listaRecepcionActivoFijoDetalleAF)
                    {
                        var ubicacionActivoFijoDetalle = await db.UbicacionActivoFijo.Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais).Include(c => c.Empleado).ThenInclude(c => c.Persona).Include(c => c.Bodega).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == rafd.IdRecepcionActivoFijoDetalle);
                        listaRecepcionActivoFijoDetalle.Add(new RecepcionActivoFijoDetalle { NumeroPoliza = rafd.NumeroPoliza, Serie = rafd.Serie, NumeroChasis = rafd.NumeroChasis, NumeroMotor = rafd.NumeroMotor, Placa = rafd.Placa, NumeroClaveCatastral = rafd.NumeroClaveCatastral, Estado = new Estado { Nombre = rafd.Estado.Nombre }, RecepcionActivoFijo = new RecepcionActivoFijo { FechaRecepcion = rafd.RecepcionActivoFijo.FechaRecepcion, Cantidad = rafd.RecepcionActivoFijo.Cantidad, ValidacionTecnica = rafd.RecepcionActivoFijo.ValidacionTecnica, Fondo = rafd.RecepcionActivoFijo.Fondo, OrdenCompra = rafd.RecepcionActivoFijo.OrdenCompra, MotivoRecepcion = new MotivoRecepcion { Descripcion = rafd.RecepcionActivoFijo.MotivoRecepcion.Descripcion }, Proveedor = new Proveedor { Nombre = rafd.RecepcionActivoFijo.Proveedor.Nombre, Apellidos = rafd.RecepcionActivoFijo.Proveedor.Apellidos } }, UbicacionActivoFijo = new List<UbicacionActivoFijo> { new UbicacionActivoFijo { Bodega = ubicacionActivoFijoDetalle.Bodega != null ? new Bodega { Nombre = ubicacionActivoFijoDetalle.Bodega.Nombre } : null, Empleado = ubicacionActivoFijoDetalle.Empleado != null ? new Empleado { Persona = new Persona { Nombres = ubicacionActivoFijoDetalle.Empleado.Persona.Nombres, Apellidos = ubicacionActivoFijoDetalle.Empleado.Persona.Apellidos } } : null, LibroActivoFijo = new LibroActivoFijo { IdSucursal = ubicacionActivoFijoDetalle.LibroActivoFijo.IdSucursal, Sucursal = new Sucursal { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Nombre, Ciudad = new Ciudad { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Nombre, Provincia = new Provincia { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Provincia.Nombre, Pais = new Pais { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Provincia.Pais.Nombre } } } } } } } } );
                    }
                    listaActivoFijoRecepcion.Add(new ActivoFijo
                    {
                        IdActivoFijo = item.IdActivoFijo,
                        Nombre = item.Nombre,
                        ValorCompra = item.ValorCompra,
                        Depreciacion = item.Depreciacion,
                        SubClaseActivoFijo = new SubClaseActivoFijo { Nombre = item.SubClaseActivoFijo.Nombre, ClaseActivoFijo = new ClaseActivoFijo { Nombre = item.SubClaseActivoFijo.ClaseActivoFijo.Nombre, TipoActivoFijo = new TipoActivoFijo { Nombre = item.SubClaseActivoFijo.ClaseActivoFijo.TipoActivoFijo.Nombre } } },
                        UnidadMedida = new UnidadMedida { Nombre = item.UnidadMedida.Nombre },
                        CodigoActivoFijo = new CodigoActivoFijo { Codigosecuencial = item.CodigoActivoFijo.Codigosecuencial, CodigoBarras = item.CodigoActivoFijo.CodigoBarras },
                        Modelo = new Modelo { Nombre = item.Modelo.Nombre, Marca = new Marca { Nombre = item.Modelo.Marca.Nombre } },
                        Subramo = new Subramo { Nombre = item.Subramo.Nombre, Ramo = new Ramo { Nombre = item.Subramo.Ramo.Nombre } },
                        CompaniaSeguro = new CompaniaSeguro { Nombre = item.CompaniaSeguro.Nombre },
                        RecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle
                    });
                }
                return listaActivoFijoRecepcion;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijo>();
            }
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoPorEstado/{estado}")]
        public async Task<List<ActivoFijo>> GetActivosFijosPorEstado(string estado)
        {
            var listaActivoFijo = await GetActivosFijos();
            return listaActivoFijo.Where(c => c.RecepcionActivoFijoDetalle.FirstOrDefault().Estado.Nombre == estado).ToList();
        }

        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijos([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activoFijo = await db.ActivoFijo.Include(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo).Include(c => c.UnidadMedida).Include(c => c.CodigoActivoFijo).Include(c => c.Modelo).ThenInclude(c => c.Marca).Include(c=> c.Subramo).ThenInclude(c=> c.Ramo).Include(c=> c.CompaniaSeguro).SingleOrDefaultAsync(m => m.IdActivoFijo == id);
                if (activoFijo != null)
                {
                    var listaRecepcionActivoFijoDetalle = new List<RecepcionActivoFijoDetalle>();
                    var listaRecepcionActivoFijoDetalleAF = await db.RecepcionActivoFijoDetalle.Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor).Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion).Include(c => c.Estado).Where(c => c.IdActivoFijo == activoFijo.IdActivoFijo).ToListAsync();
                    foreach (var item in listaRecepcionActivoFijoDetalleAF)
                    {
                        var ubicacionActivoFijoDetalle = await db.UbicacionActivoFijo.Include(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais).Include(c=> c.Empleado).ThenInclude(c=> c.Persona).Include(c=> c.Bodega).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == item.IdRecepcionActivoFijoDetalle);
                        listaRecepcionActivoFijoDetalle.Add(new RecepcionActivoFijoDetalle { IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle, IdActivoFijo = item.IdActivoFijo, IdRecepcionActivoFijo = item.IdRecepcionActivoFijo, NumeroChasis = item.NumeroChasis, NumeroMotor = item.NumeroMotor, Placa = item.Placa, NumeroClaveCatastral = item.NumeroClaveCatastral, Serie = item.Serie, NumeroPoliza = item.NumeroPoliza, Estado = new Estado { Nombre = item.Estado.Nombre }, RecepcionActivoFijo = new RecepcionActivoFijo { FechaRecepcion = item.RecepcionActivoFijo.FechaRecepcion, Cantidad = item.RecepcionActivoFijo.Cantidad, ValidacionTecnica = item.RecepcionActivoFijo.ValidacionTecnica, Fondo = item.RecepcionActivoFijo.Fondo, OrdenCompra = item.RecepcionActivoFijo.OrdenCompra, IdMotivoRecepcion = item.RecepcionActivoFijo.IdMotivoRecepcion, MotivoRecepcion = new MotivoRecepcion { Descripcion = item.RecepcionActivoFijo.MotivoRecepcion.Descripcion }, IdProveedor = item.RecepcionActivoFijo.IdProveedor, Proveedor = new Proveedor { Nombre = item.RecepcionActivoFijo.Proveedor.Nombre, Apellidos = item.RecepcionActivoFijo.Proveedor.Apellidos } }, UbicacionActivoFijo = new List<UbicacionActivoFijo> { new UbicacionActivoFijo { IdUbicacionActivoFijo = ubicacionActivoFijoDetalle.IdUbicacionActivoFijo, IdBodega = ubicacionActivoFijoDetalle.IdBodega, Bodega = ubicacionActivoFijoDetalle.Bodega != null ? new Bodega { Nombre = ubicacionActivoFijoDetalle.Bodega.Nombre } : null, IdEmpleado = ubicacionActivoFijoDetalle.IdEmpleado, Empleado = ubicacionActivoFijoDetalle.Empleado != null ? new Empleado { Persona = new Persona { Nombres = ubicacionActivoFijoDetalle.Empleado.Persona.Nombres, Apellidos = ubicacionActivoFijoDetalle.Empleado.Persona.Apellidos } } : null, IdLibroActivoFijo = ubicacionActivoFijoDetalle.IdLibroActivoFijo, LibroActivoFijo = new LibroActivoFijo { IdSucursal = ubicacionActivoFijoDetalle.LibroActivoFijo.IdSucursal, Sucursal = new Sucursal { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Nombre, IdCiudad = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.IdCiudad, Ciudad = new Ciudad { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Nombre, IdProvincia = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.IdProvincia, Provincia = new Provincia { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Provincia.Nombre, IdPais = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Provincia.IdPais, Pais = new Pais { Nombre = ubicacionActivoFijoDetalle.LibroActivoFijo.Sucursal.Ciudad.Provincia.Pais.Nombre } } } } } } } });
                    }
                    activoFijo = new ActivoFijo
                    {
                        IdActivoFijo = activoFijo.IdActivoFijo,
                        Nombre = activoFijo.Nombre,
                        ValorCompra = activoFijo.ValorCompra,
                        Depreciacion = activoFijo.Depreciacion,
                        IdSubClaseActivoFijo = activoFijo.IdSubClaseActivoFijo,
                        IdUnidadMedida = activoFijo.IdUnidadMedida,
                        IdModelo = activoFijo.IdModelo,
                        IdSubramo = activoFijo.IdSubramo,
                        IdCompaniaSeguro = activoFijo.IdCompaniaSeguro,
                        SubClaseActivoFijo = new SubClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.Nombre, IdClaseActivoFijo = activoFijo.SubClaseActivoFijo.IdClaseActivoFijo, ClaseActivoFijo = new ClaseActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.Nombre, IdTipoActivoFijo = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.IdTipoActivoFijo, TipoActivoFijo = new TipoActivoFijo { Nombre = activoFijo.SubClaseActivoFijo.ClaseActivoFijo.TipoActivoFijo.Nombre } } },
                        UnidadMedida = new UnidadMedida { Nombre = activoFijo.UnidadMedida.Nombre },
                        CodigoActivoFijo = new CodigoActivoFijo { IdCodigoActivoFijo = activoFijo.IdCodigoActivoFijo, Codigosecuencial = activoFijo.CodigoActivoFijo.Codigosecuencial, CodigoBarras = activoFijo.CodigoActivoFijo.CodigoBarras },
                        Modelo = new Modelo { Nombre = activoFijo.Modelo.Nombre, IdMarca = activoFijo.Modelo.IdMarca, Marca = new Marca { Nombre = activoFijo.Modelo.Marca.Nombre } },
                        Subramo = new Subramo { Nombre = activoFijo.Subramo.Nombre, IdRamo = activoFijo.Subramo.IdRamo, Ramo = new Ramo { Nombre = activoFijo.Subramo.Ramo.Nombre } },
                        CompaniaSeguro = new CompaniaSeguro { Nombre = activoFijo.CompaniaSeguro.Nombre },
                        RecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle
                    };
                }
                return new Response { IsSuccess = activoFijo != null, Message = activoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activoFijo };
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
                    var activoFijo = new ActivoFijo { IdSubClaseActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubClaseActivoFijo, IdUnidadMedida = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdUnidadMedida, IdCodigoActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdCodigoActivoFijo, CodigoActivoFijo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.CodigoActivoFijo, IdModelo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdModelo, Nombre = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Nombre, ValorCompra = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.ValorCompra, Depreciacion = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Depreciacion, IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubramo, IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdCompaniaSeguro };
                    db.ActivoFijo.Add(activoFijo);
                    var recepcionActivoFijo = new RecepcionActivoFijo { IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor, IdMotivoRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoRecepcion, FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion, Cantidad = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Cantidad, ValidacionTecnica = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.ValidacionTecnica, Fondo = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Fondo, OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra };
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

        [HttpPut("{id}")]
        public async Task<Response> PutActivosFijos([FromRoute] int id, [FromBody] List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalleTransfer)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var listaDetalleActivosFijosEliminar = await db.RecepcionActivoFijoDetalle.Where(c => c.IdActivoFijo == id).Select(c=> c.IdRecepcionActivoFijoDetalle).Except(listaRecepcionActivoFijoDetalleTransfer.Where(c=> c.IdRecepcionActivoFijoDetalle > 0).Select(c=> c.IdRecepcionActivoFijoDetalle)).ToListAsync();
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
                        activoFijoActualizar.IdUnidadMedida = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdUnidadMedida;
                        activoFijoActualizar.IdModelo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdModelo;
                        activoFijoActualizar.IdSubramo = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdSubramo;
                        activoFijoActualizar.IdCompaniaSeguro = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.IdCompaniaSeguro;
                        activoFijoActualizar.Depreciacion = listaRecepcionActivoFijoDetalleTransfer[0].ActivoFijo.Depreciacion;
                        db.ActivoFijo.Update(activoFijoActualizar);
                    }
                    var recepcionActivoFijoActualizar = await db.RecepcionActivoFijo.FirstOrDefaultAsync(c => c.IdRecepcionActivoFijo == listaRecepcionActivoFijoDetalleTransfer[0].IdRecepcionActivoFijo);
                    if (recepcionActivoFijoActualizar != null)
                    {
                        recepcionActivoFijoActualizar.FechaRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.FechaRecepcion;
                        recepcionActivoFijoActualizar.Cantidad = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Cantidad;
                        recepcionActivoFijoActualizar.ValidacionTecnica = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.ValidacionTecnica;
                        recepcionActivoFijoActualizar.Fondo = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.Fondo;
                        recepcionActivoFijoActualizar.OrdenCompra = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.OrdenCompra;
                        recepcionActivoFijoActualizar.IdMotivoRecepcion = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdMotivoRecepcion;
                        recepcionActivoFijoActualizar.IdProveedor = listaRecepcionActivoFijoDetalleTransfer[0].RecepcionActivoFijo.IdProveedor;
                        db.RecepcionActivoFijo.Update(recepcionActivoFijoActualizar);
                    }

                    if (activoFijoActualizar == null || recepcionActivoFijoActualizar == null)
                        return new Response { IsSuccess = false, Message = Mensaje.Error };

                    foreach (var item in listaRecepcionActivoFijoDetalleTransfer)
                    {
                        var ubicacionActivoFijo = item.UbicacionActivoFijo.FirstOrDefault();
                        if (item.IdRecepcionActivoFijoDetalle == 0)
                        {
                            var nuevaRecepcionActivoFijoDetalle = new RecepcionActivoFijoDetalle { IdRecepcionActivoFijo = item.IdRecepcionActivoFijo, IdActivoFijo = id, IdEstado = item.IdEstado, Serie = item.Serie, NumeroPoliza = item.NumeroPoliza, NumeroChasis = item.NumeroChasis, NumeroMotor = item.NumeroMotor, Placa = item.Placa, NumeroClaveCatastral = item.NumeroClaveCatastral };
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
                                recepcionActivoFijoDetalleActualizar.NumeroPoliza = item.NumeroPoliza;
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
    }
}
