using bd.swrm.entidades.Negocio;
using bd.swrm.servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.Servicios
{
    public class ClonacionService : IClonacion
    {
        public Articulo ClonarArticulo(Articulo articulo)
        {
            try
            {
                return articulo != null ? new Articulo
                {
                    IdArticulo = articulo.IdArticulo,
                    IdSubClaseArticulo = articulo.IdSubClaseArticulo,
                    IdUnidadMedida = articulo.IdUnidadMedida,
                    IdModelo = articulo.IdModelo,
                    Nombre = articulo.Nombre,
                    SubClaseArticulo = ClonarSubclaseArticulo(articulo?.SubClaseArticulo),
                    UnidadMedida = ClonarUnidadMedida(articulo?.UnidadMedida),
                    Modelo = ClonarModelo(articulo?.Modelo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Bodega ClonarBodega(Bodega bodega)
        {
            try
            {
                return bodega != null ? new Bodega
                {
                    IdBodega = bodega.IdBodega,
                    Nombre = bodega.Nombre,
                    IdSucursal = bodega.IdSucursal,
                    Sucursal = ClonarSucursal(bodega?.Sucursal)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ClaseArticulo ClonarClaseArticulo(ClaseArticulo claseArticulo)
        {
            try
            {
                return claseArticulo != null ? new ClaseArticulo
                {
                    IdClaseArticulo = claseArticulo.IdClaseArticulo,
                    Nombre = claseArticulo.Nombre,
                    IdTipoArticulo = claseArticulo.IdTipoArticulo,
                    TipoArticulo = ClonarTipoArticulo(claseArticulo?.TipoArticulo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Empleado ClonarEmpleado(Empleado empleado)
        {
            try
            {
                return empleado != null ? new Empleado
                {
                    IdEmpleado = empleado.IdEmpleado,
                    IdPersona = empleado.IdPersona,
                    IdDependencia = empleado.IdDependencia,
                    Persona = ClonarPersona(empleado?.Persona),
                    Dependencia = ClonarDependencia(empleado?.Dependencia)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Estado ClonarEstado(Estado estado)
        {
            try
            {
                return estado != null ? new Estado
                {
                    IdEstado = estado.IdEstado,
                    Nombre = estado.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FacturaActivoFijo ClonarFacturaActivoFijo(FacturaActivoFijo facturaActivoFijo)
        {
            try
            {
                return facturaActivoFijo != null ? new FacturaActivoFijo
                {
                    IdFacturaActivoFijo = facturaActivoFijo.IdFacturaActivoFijo,
                    FechaFactura = facturaActivoFijo.FechaFactura,
                    NumeroFactura = facturaActivoFijo.NumeroFactura
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MaestroArticuloSucursal ClonarMaestroArticuloSucursal(MaestroArticuloSucursal maestroArticuloSucursal)
        {
            try
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
                    Sucursal = ClonarSucursal(maestroArticuloSucursal?.Sucursal),
                    Articulo = ClonarArticulo(maestroArticuloSucursal?.Articulo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Marca ClonarMarca(Marca marca)
        {
            try
            {
                return marca != null ? new Marca
                {
                    IdMarca = marca.IdMarca,
                    Nombre = marca.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Modelo ClonarModelo(Modelo modelo)
        {
            try
            {
                return modelo != null ? new Modelo
                {
                    IdModelo = modelo.IdModelo,
                    Nombre = modelo.Nombre,
                    IdMarca = modelo.IdMarca,
                    Marca = ClonarMarca(modelo?.Marca)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MotivoRecepcionArticulos ClonarMotivoRecepcionArticulos(MotivoRecepcionArticulos motivoRecepcionArticulos)
        {
            try
            {
                return motivoRecepcionArticulos != null ? new MotivoRecepcionArticulos
                {
                    IdMotivoRecepcionArticulos = motivoRecepcionArticulos.IdMotivoRecepcionArticulos,
                    Descripcion = motivoRecepcionArticulos.Descripcion
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public OrdenCompra ClonarOrdenCompra(OrdenCompra ordenCompra)
        {
            try
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
                    MotivoRecepcionArticulos = ClonarMotivoRecepcionArticulos(ordenCompra?.MotivoRecepcionArticulos),
                    Estado = ClonarEstado(ordenCompra?.Estado),
                    Factura = ClonarFacturaActivoFijo(ordenCompra?.Factura),
                    EmpleadoResponsable = ClonarEmpleado(ordenCompra?.EmpleadoResponsable),
                    EmpleadoDevolucion = ClonarEmpleado(ordenCompra?.EmpleadoDevolucion),
                    Bodega = ClonarBodega(ordenCompra?.Bodega),
                    Proveedor = ClonarProveedor(ordenCompra?.Proveedor),
                    OrdenCompraDetalles = ordenCompra.OrdenCompraDetalles
                } : null;

                if (nuevaOrdenCompra != null)
                {
                    nuevaOrdenCompra.Factura.DocumentoActivoFijo = ordenCompra.Factura.DocumentoActivoFijo;
                    foreach (var item in ordenCompra.OrdenCompraDetalles)
                    {
                        item.OrdenCompra = null;
                        item.MaestroArticuloSucursal = ClonarMaestroArticuloSucursal(item?.MaestroArticuloSucursal);
                    }
                }
                return nuevaOrdenCompra;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Persona ClonarPersona(Persona persona)
        {
            try
            {
                return persona != null ? new Persona
                {
                    IdPersona = persona.IdPersona,
                    Nombres = persona.Nombres,
                    Apellidos = persona.Apellidos
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Proveedor ClonarProveedor(Proveedor proveedor)
        {
            try
            {
                return proveedor != null ? new Proveedor
                {
                    IdProveedor = proveedor.IdProveedor,
                    RepresentanteLegal = proveedor.RepresentanteLegal,
                    PersonaContacto = proveedor.PersonaContacto,
                    RazonSocial = proveedor.RazonSocial,
                    Direccion = proveedor.Direccion,
                    Identificacion = proveedor.Identificacion
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SubClaseArticulo ClonarSubclaseArticulo(SubClaseArticulo subClaseArticulo)
        {
            try
            {
                return subClaseArticulo != null ? new SubClaseArticulo
                {
                    IdSubClaseArticulo = subClaseArticulo.IdSubClaseArticulo,
                    Nombre = subClaseArticulo.Nombre,
                    IdClaseArticulo = subClaseArticulo.IdClaseArticulo,
                    ClaseArticulo = ClonarClaseArticulo(subClaseArticulo?.ClaseArticulo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Sucursal ClonarSucursal(Sucursal sucursal)
        {
            try
            {
                return sucursal != null ? new Sucursal
                {
                    IdSucursal = sucursal.IdSucursal,
                    Nombre = sucursal.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TipoArticulo ClonarTipoArticulo(TipoArticulo tipoArticulo)
        {
            try
            {
                return tipoArticulo != null ? new TipoArticulo
                {
                    IdTipoArticulo = tipoArticulo.IdTipoArticulo,
                    Nombre = tipoArticulo.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UnidadMedida ClonarUnidadMedida(UnidadMedida unidadMedida)
        {
            try
            {
                return unidadMedida != null ? new UnidadMedida
                {
                    IdUnidadMedida = unidadMedida.IdUnidadMedida,
                    Nombre = unidadMedida.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RecepcionActivoFijoDetalle ClonarRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle rafdOld)
        {
            try
            {
                return rafdOld != null ? new RecepcionActivoFijoDetalle
                {
                    IdRecepcionActivoFijoDetalle = rafdOld.IdRecepcionActivoFijoDetalle,
                    IdActivoFijo = rafdOld.IdActivoFijo,
                    IdRecepcionActivoFijo = rafdOld.IdRecepcionActivoFijo,
                    IdCodigoActivoFijo = rafdOld.IdCodigoActivoFijo,
                    IdEstado = rafdOld.IdEstado,
                    Serie = rafdOld.Serie,
                    Estado = ClonarEstado(rafdOld.Estado),
                    UbicacionActivoFijoActual = rafdOld.UbicacionActivoFijoActual,
                    SucursalActual = rafdOld.SucursalActual,
                    AltaActivoFijoActual = rafdOld.AltaActivoFijoActual,
                    BajaActivoFijoActual = rafdOld.BajaActivoFijoActual,
                    CodigoActivoFijo = rafdOld.CodigoActivoFijo,
                    RecepcionActivoFijo = ClonarRecepcionActivoFijo(rafdOld?.RecepcionActivoFijo),
                    RecepcionActivoFijoDetalleEdificio = ClonarRecepcionActivoFijoDetalleEdificio(rafdOld?.RecepcionActivoFijoDetalleEdificio),
                    RecepcionActivoFijoDetalleVehiculo = ClonarRecepcionActivoFijoDetalleVehiculo(rafdOld?.RecepcionActivoFijoDetalleVehiculo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RecepcionActivoFijoDetalleEdificio ClonarRecepcionActivoFijoDetalleEdificio(RecepcionActivoFijoDetalleEdificio recepcionActivoFijoDetalleEdificio)
        {
            try
            {
                return recepcionActivoFijoDetalleEdificio != null ? new RecepcionActivoFijoDetalleEdificio
                {
                    IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalleEdificio.IdRecepcionActivoFijoDetalle,
                    NumeroClaveCatastral = recepcionActivoFijoDetalleEdificio.NumeroClaveCatastral
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RecepcionActivoFijoDetalleVehiculo ClonarRecepcionActivoFijoDetalleVehiculo(RecepcionActivoFijoDetalleVehiculo recepcionActivoFijoDetalleVehiculo)
        {
            try
            {
                return recepcionActivoFijoDetalleVehiculo != null ? new RecepcionActivoFijoDetalleVehiculo
                {
                    IdRecepcionActivoFijoDetalle = recepcionActivoFijoDetalleVehiculo.IdRecepcionActivoFijoDetalle,
                    NumeroChasis = recepcionActivoFijoDetalleVehiculo.NumeroChasis,
                    NumeroMotor = recepcionActivoFijoDetalleVehiculo.NumeroMotor,
                    Placa = recepcionActivoFijoDetalleVehiculo.Placa
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RecepcionActivoFijo ClonarRecepcionActivoFijo(RecepcionActivoFijo recepcionActivoFijo)
        {
            try
            {
                return recepcionActivoFijo != null ? new RecepcionActivoFijo
                {
                    IdRecepcionActivoFijo = recepcionActivoFijo.IdRecepcionActivoFijo,
                    FechaRecepcion = recepcionActivoFijo.FechaRecepcion,
                    IdFondoFinanciamiento = recepcionActivoFijo.IdFondoFinanciamiento,
                    FondoFinanciamiento = ClonarFondoFinanciamiento(recepcionActivoFijo?.FondoFinanciamiento),
                    OrdenCompra = recepcionActivoFijo.OrdenCompra,
                    IdMotivoAlta = recepcionActivoFijo.IdMotivoAlta,
                    MotivoAlta = ClonarMotivoAlta(recepcionActivoFijo?.MotivoAlta),
                    IdProveedor = recepcionActivoFijo.IdProveedor,
                    Proveedor = ClonarProveedor(recepcionActivoFijo?.Proveedor),
                    PolizaSeguroActivoFijo = ClonarPolizaSeguroActivoFijo(recepcionActivoFijo?.PolizaSeguroActivoFijo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MotivoAlta ClonarMotivoAlta(MotivoAlta motivoAlta)
        {
            try
            {
                return motivoAlta != null ? new MotivoAlta
                {
                    IdMotivoAlta = motivoAlta.IdMotivoAlta,
                    Descripcion = motivoAlta.Descripcion
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MotivoBaja ClonarMotivoBaja(MotivoBaja motivoBaja)
        {
            try
            {
                return motivoBaja != null ? new MotivoBaja
                {
                    IdMotivoBaja = motivoBaja.IdMotivoBaja,
                    Nombre = motivoBaja.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MotivoTransferencia ClonarMotivoTransferencia(MotivoTransferencia motivoTransferencia)
        {
            try
            {
                return motivoTransferencia != null ? new MotivoTransferencia
                {
                    IdMotivoTransferencia = motivoTransferencia.IdMotivoTransferencia,
                    Motivo_Transferencia = motivoTransferencia.Motivo_Transferencia
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MotivoTraslado ClonarMotivoTraslado(MotivoTraslado motivoTraslado)
        {
            try
            {
                return motivoTraslado != null ? new MotivoTraslado
                {
                    IdMotivoTraslado = motivoTraslado.IdMotivoTraslado,
                    Descripcion = motivoTraslado.Descripcion
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FondoFinanciamiento ClonarFondoFinanciamiento(FondoFinanciamiento fondoFinanciamiento)
        {
            try
            {
                return fondoFinanciamiento != null ? new FondoFinanciamiento
                {
                    IdFondoFinanciamiento = fondoFinanciamiento.IdFondoFinanciamiento,
                    Nombre = fondoFinanciamiento.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PolizaSeguroActivoFijo ClonarPolizaSeguroActivoFijo(PolizaSeguroActivoFijo polizaSeguroActivoFijo)
        {
            try
            {
                return polizaSeguroActivoFijo != null ? new PolizaSeguroActivoFijo
                {
                    IdRecepcionActivoFijo = polizaSeguroActivoFijo.IdRecepcionActivoFijo,
                    IdCompaniaSeguro = polizaSeguroActivoFijo.IdCompaniaSeguro,
                    NumeroPoliza = polizaSeguroActivoFijo.NumeroPoliza,
                    CompaniaSeguro = ClonarCompaniaSeguro(polizaSeguroActivoFijo?.CompaniaSeguro)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CodigoActivoFijo ClonarCodigoActivoFijo(CodigoActivoFijo codigoActivoFijo)
        {
            try
            {
                return codigoActivoFijo != null ? new CodigoActivoFijo
                {
                    IdCodigoActivoFijo = codigoActivoFijo.IdCodigoActivoFijo,
                    Codigosecuencial = codigoActivoFijo.Codigosecuencial
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CompaniaSeguro ClonarCompaniaSeguro(CompaniaSeguro companiaSeguro)
        {
            try
            {
                return companiaSeguro != null ? new CompaniaSeguro
                {
                    IdCompaniaSeguro = companiaSeguro.IdCompaniaSeguro,
                    Nombre = companiaSeguro.Nombre,
                    FechaInicioVigencia = companiaSeguro.FechaInicioVigencia,
                    FechaFinVigencia = companiaSeguro.FechaFinVigencia
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Subramo ClonarSubramo(Subramo subramo)
        {
            try
            {
                return subramo != null ? new Subramo
                {
                    IdSubramo = subramo.IdSubramo,
                    Nombre = subramo.Nombre,
                    IdRamo = subramo.IdRamo,
                    Ramo = ClonarRamo(subramo?.Ramo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Ramo ClonarRamo(Ramo ramo)
        {
            try
            {
                return ramo != null ? new Ramo
                {
                    IdRamo = ramo.IdRamo,
                    Nombre = ramo.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActivoFijo ClonarActivoFijo(ActivoFijo activoFijo, List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalle)
        {
            try
            {
                return activoFijo != null ? new ActivoFijo
                {
                    IdActivoFijo = activoFijo.IdActivoFijo,
                    Nombre = activoFijo.Nombre,
                    ValorCompra = activoFijo.ValorCompra,
                    Depreciacion = activoFijo.Depreciacion,
                    IdSubClaseActivoFijo = activoFijo.IdSubClaseActivoFijo,
                    IdModelo = activoFijo.IdModelo,
                    ValidacionTecnica = activoFijo.ValidacionTecnica,
                    SubClaseActivoFijo = ClonarSubclaseActivoFijo(activoFijo?.SubClaseActivoFijo),
                    Modelo = ClonarModelo(activoFijo?.Modelo),
                    RecepcionActivoFijoDetalle = listaRecepcionActivoFijoDetalle
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TipoActivoFijo ClonarTipoActivoFijo(TipoActivoFijo tipoActivoFijo)
        {
            try
            {
                return tipoActivoFijo != null ? new TipoActivoFijo
                {
                    IdTipoActivoFijo = tipoActivoFijo.IdTipoActivoFijo,
                    Nombre = tipoActivoFijo.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ClaseActivoFijo ClonarClaseActivoFijo(ClaseActivoFijo claseActivoFijo)
        {
            try
            {
                return claseActivoFijo != null ? new ClaseActivoFijo
                {
                    IdClaseActivoFijo = claseActivoFijo.IdClaseActivoFijo,
                    IdCategoriaActivoFijo = claseActivoFijo.IdCategoriaActivoFijo,
                    IdTipoActivoFijo = claseActivoFijo.IdTipoActivoFijo,
                    Nombre = claseActivoFijo.Nombre,
                    CategoriaActivoFijo = ClonarCategoriaActivoFijo(claseActivoFijo?.CategoriaActivoFijo),
                    TipoActivoFijo = ClonarTipoActivoFijo(claseActivoFijo?.TipoActivoFijo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SubClaseActivoFijo ClonarSubclaseActivoFijo(SubClaseActivoFijo subClaseActivoFijo)
        {
            try
            {
                return subClaseActivoFijo != null ? new SubClaseActivoFijo
                {
                    IdSubClaseActivoFijo = subClaseActivoFijo.IdSubClaseActivoFijo,
                    IdClaseActivoFijo = subClaseActivoFijo.IdClaseActivoFijo,
                    IdSubramo = subClaseActivoFijo.IdSubramo,
                    Nombre = subClaseActivoFijo.Nombre,
                    ClaseActivoFijo = ClonarClaseActivoFijo(subClaseActivoFijo?.ClaseActivoFijo),
                    Subramo = ClonarSubramo(subClaseActivoFijo?.Subramo)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CategoriaActivoFijo ClonarCategoriaActivoFijo(CategoriaActivoFijo categoriaActivoFijo)
        {
            try
            {
                return categoriaActivoFijo != null ? new CategoriaActivoFijo
                {
                    IdCategoriaActivoFijo = categoriaActivoFijo.IdCategoriaActivoFijo,
                    Nombre = categoriaActivoFijo.Nombre,
                    AnosVidaUtil = categoriaActivoFijo.AnosVidaUtil,
                    PorCientoDepreciacionAnual = categoriaActivoFijo.PorCientoDepreciacionAnual
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UbicacionActivoFijo ClonarUbicacionActivoFijo(UbicacionActivoFijo ubicacionActivoFijo)
        {
            try
            {
                return ubicacionActivoFijo != null ? new UbicacionActivoFijo
                {
                    IdUbicacionActivoFijo = ubicacionActivoFijo.IdUbicacionActivoFijo,
                    IdEmpleado = ubicacionActivoFijo.IdEmpleado,
                    IdBodega = ubicacionActivoFijo.IdBodega,
                    IdRecepcionActivoFijoDetalle = ubicacionActivoFijo.IdRecepcionActivoFijoDetalle,
                    FechaUbicacion = ubicacionActivoFijo.FechaUbicacion,
                    Confirmacion = ubicacionActivoFijo.Confirmacion,
                    Bodega = ClonarBodega(ubicacionActivoFijo?.Bodega),
                    Empleado = ClonarEmpleado(ubicacionActivoFijo?.Empleado)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ComponenteActivoFijo> ClonarListadoComponenteActivoFijo(List<ComponenteActivoFijo> listaComponenteActivoFijo)
        {
            try
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
            catch (Exception)
            {
                return new List<ComponenteActivoFijo>();
            }
        }

        public AltaActivoFijo ClonarAltaActivoFijo(AltaActivoFijo altaActivoFijo)
        {
            try
            {
                return altaActivoFijo != null ? new AltaActivoFijo
                {
                    IdAltaActivoFijo = altaActivoFijo.IdAltaActivoFijo,
                    FechaAlta = altaActivoFijo.FechaAlta,
                    FechaPago = altaActivoFijo.FechaPago,
                    IdMotivoAlta = altaActivoFijo.IdMotivoAlta,
                    IdFacturaActivoFijo = altaActivoFijo.IdFacturaActivoFijo,
                    MotivoAlta = ClonarMotivoAlta(altaActivoFijo?.MotivoAlta),
                    FacturaActivoFijo = ClonarFacturaActivoFijo(altaActivoFijo?.FacturaActivoFijo),
                    AltaActivoFijoDetalle = altaActivoFijo.AltaActivoFijoDetalle
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public BajaActivoFijo ClonarBajaActivoFijo(BajaActivoFijo bajaActivoFijo)
        {
            try
            {
                return bajaActivoFijo != null ? new BajaActivoFijo
                {
                    IdBajaActivoFijo = bajaActivoFijo.IdBajaActivoFijo,
                    FechaBaja = bajaActivoFijo.FechaBaja,
                    IdMotivoBaja = bajaActivoFijo.IdMotivoBaja,
                    MemoOficioResolucion = bajaActivoFijo.MemoOficioResolucion,
                    MotivoBaja = ClonarMotivoBaja(bajaActivoFijo?.MotivoBaja),
                    BajaActivoFijoDetalle = bajaActivoFijo.BajaActivoFijoDetalle
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TransferenciaActivoFijo ClonarTransferenciaActivoFijo(TransferenciaActivoFijo transferenciaActivoFijo)
        {
            try
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
                    Estado = ClonarEstado(transferenciaActivoFijo?.Estado),
                    MotivoTransferencia = ClonarMotivoTransferencia(transferenciaActivoFijo?.MotivoTransferencia)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public InventarioActivoFijo ClonarInventarioActivoFijo(InventarioActivoFijo inventarioActivoFijo)
        {
            try
            {
                return inventarioActivoFijo != null ? new InventarioActivoFijo
                {
                    IdInventarioActivoFijo = inventarioActivoFijo.IdInventarioActivoFijo,
                    FechaCorteInventario = inventarioActivoFijo.FechaCorteInventario,
                    FechaInforme = inventarioActivoFijo.FechaInforme,
                    NumeroInforme = inventarioActivoFijo.NumeroInforme,
                    IdEstado = inventarioActivoFijo.IdEstado,
                    InventarioManual = inventarioActivoFijo.InventarioManual,
                    Estado = ClonarEstado(inventarioActivoFijo?.Estado),
                    InventarioActivoFijoDetalle = inventarioActivoFijo.InventarioActivoFijoDetalle
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MovilizacionActivoFijo ClonarMovilizacionActivoFijo(MovilizacionActivoFijo movilizacionActivoFijo)
        {
            try
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
                    MotivoTraslado = ClonarMotivoTraslado(movilizacionActivoFijo?.MotivoTraslado),
                    EmpleadoResponsable = ClonarEmpleado(movilizacionActivoFijo?.EmpleadoResponsable),
                    EmpleadoSolicita = ClonarEmpleado(movilizacionActivoFijo?.EmpleadoSolicita),
                    EmpleadoAutorizado = ClonarEmpleado(movilizacionActivoFijo?.EmpleadoAutorizado),
                    MovilizacionActivoFijoDetalle = movilizacionActivoFijo.MovilizacionActivoFijoDetalle
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dependencia ClonarDependencia(Dependencia dependencia)
        {
            try
            {
                return dependencia != null ? new Dependencia
                {
                    IdDependencia = dependencia.IdDependencia,
                    IdSucursal = dependencia.IdSucursal,
                    Nombre = dependencia.Nombre,
                    Sucursal = ClonarSucursal(dependencia?.Sucursal),
                    IdBodega = dependencia.IdBodega,
                    Bodega = ClonarBodega(dependencia?.Bodega)
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<DocumentoActivoFijo> ClonarListadoDocumentoActivoFijo(List<DocumentoActivoFijo> listaDocumentoActivoFijo)
        {
            try
            {
                var nuevaListaDocumentoActivoFijo = new List<DocumentoActivoFijo>();
                foreach (var item in listaDocumentoActivoFijo)
                {
                    nuevaListaDocumentoActivoFijo.Add(new DocumentoActivoFijo
                    {
                        IdDocumentoActivoFijo = item.IdDocumentoActivoFijo,
                        Nombre = item.Nombre,
                        Fecha = item.Fecha,
                        Url = item.Url,
                        IdActivoFijo = item.IdActivoFijo,
                        IdRecepcionActivoFijoDetalle = item.IdRecepcionActivoFijoDetalle,
                        IdAltaActivoFijo = item.IdAltaActivoFijo,
                        IdFacturaActivoFijo = item.IdFacturaActivoFijo,
                        IdProcesoJudicialActivoFijo = item.IdProcesoJudicialActivoFijo,
                        IdRecepcionActivoFijo = item.IdRecepcionActivoFijo,
                        IdCompaniaSeguro = item.IdCompaniaSeguro
                    });
                }
                return nuevaListaDocumentoActivoFijo;
            }
            catch (Exception)
            {
                return new List<DocumentoActivoFijo>();
            }
        }

        public TipoUtilizacionAlta ClonarTipoUtilizacionAlta(TipoUtilizacionAlta tipoUtilizacionAlta)
        {
            try
            {
                return tipoUtilizacionAlta != null ? new TipoUtilizacionAlta
                {
                    IdTipoUtilizacionAlta = tipoUtilizacionAlta.IdTipoUtilizacionAlta,
                    Nombre = tipoUtilizacionAlta.Nombre
                } : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
