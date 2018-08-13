using bd.swrm.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.Interfaces
{
    public interface IClonacion
    {
        Empleado ClonarEmpleado(Empleado empleado);
        Persona ClonarPersona(Persona persona);
        Proveedor ClonarProveedor(Proveedor proveedor);
        MotivoRecepcionArticulos ClonarMotivoRecepcionArticulos(MotivoRecepcionArticulos motivoRecepcionArticulos);
        Estado ClonarEstado(Estado estado);
        FacturaActivoFijo ClonarFacturaActivoFijo(FacturaActivoFijo facturaActivoFijo);
        Bodega ClonarBodega(Bodega bodega);
        Sucursal ClonarSucursal(Sucursal sucursal);
        TipoArticulo ClonarTipoArticulo(TipoArticulo tipoArticulo);
        ClaseArticulo ClonarClaseArticulo(ClaseArticulo claseArticulo);
        SubClaseArticulo ClonarSubclaseArticulo(SubClaseArticulo subClaseArticulo);
        UnidadMedida ClonarUnidadMedida(UnidadMedida unidadMedida);
        Marca ClonarMarca(Marca marca);
        Modelo ClonarModelo(Modelo modelo);
        Articulo ClonarArticulo(Articulo articulo);
        MaestroArticuloSucursal ClonarMaestroArticuloSucursal(MaestroArticuloSucursal maestroArticuloSucursal);
        OrdenCompra ClonarOrdenCompra(OrdenCompra ordenCompra);
        RecepcionActivoFijo ClonarRecepcionActivoFijo(RecepcionActivoFijo recepcionActivoFijo);
        RecepcionActivoFijoDetalle ClonarRecepcionActivoFijoDetalle(RecepcionActivoFijoDetalle rafdOld);
        RecepcionActivoFijoDetalleEdificio ClonarRecepcionActivoFijoDetalleEdificio(RecepcionActivoFijoDetalleEdificio recepcionActivoFijoDetalleEdificio);
        RecepcionActivoFijoDetalleVehiculo ClonarRecepcionActivoFijoDetalleVehiculo(RecepcionActivoFijoDetalleVehiculo recepcionActivoFijoDetalleVehiculo);
        MotivoAlta ClonarMotivoAlta(MotivoAlta motivoAlta);
        MotivoBaja ClonarMotivoBaja(MotivoBaja motivoBaja);
        MotivoTransferencia ClonarMotivoTransferencia(MotivoTransferencia motivoTransferencia);
        MotivoTraslado ClonarMotivoTraslado(MotivoTraslado motivoTraslado);
        FondoFinanciamiento ClonarFondoFinanciamiento(FondoFinanciamiento fondoFinanciamiento);
        PolizaSeguroActivoFijo ClonarPolizaSeguroActivoFijo(PolizaSeguroActivoFijo polizaSeguroActivoFijo);
        CodigoActivoFijo ClonarCodigoActivoFijo(CodigoActivoFijo codigoActivoFijo);
        CompaniaSeguro ClonarCompaniaSeguro(CompaniaSeguro companiaSeguro);
        Subramo ClonarSubramo(Subramo subramo);
        Ramo ClonarRamo(Ramo ramo);
        ActivoFijo ClonarActivoFijo(ActivoFijo activoFijo, List<RecepcionActivoFijoDetalle> listaRecepcionActivoFijoDetalle);
        TipoActivoFijo ClonarTipoActivoFijo(TipoActivoFijo tipoActivoFijo);
        ClaseActivoFijo ClonarClaseActivoFijo(ClaseActivoFijo claseActivoFijo);
        SubClaseActivoFijo ClonarSubclaseActivoFijo(SubClaseActivoFijo subClaseActivoFijo);
        CategoriaActivoFijo ClonarCategoriaActivoFijo(CategoriaActivoFijo categoriaActivoFijo);
        UbicacionActivoFijo ClonarUbicacionActivoFijo(UbicacionActivoFijo ubicacionActivoFijo);
        List<ComponenteActivoFijo> ClonarListadoComponenteActivoFijo(List<ComponenteActivoFijo> listaComponenteActivoFijo);
        AltaActivoFijo ClonarAltaActivoFijo(AltaActivoFijo altaActivoFijo);
        BajaActivoFijo ClonarBajaActivoFijo(BajaActivoFijo bajaActivoFijo);
        TransferenciaActivoFijo ClonarTransferenciaActivoFijo(TransferenciaActivoFijo transferenciaActivoFijo);
        InventarioActivoFijo ClonarInventarioActivoFijo(InventarioActivoFijo inventarioActivoFijo);
        MovilizacionActivoFijo ClonarMovilizacionActivoFijo(MovilizacionActivoFijo movilizacionActivoFijo);
        Dependencia ClonarDependencia(Dependencia dependencia);
        List<DocumentoActivoFijo> ClonarListadoDocumentoActivoFijo(List<DocumentoActivoFijo> listaDocumentoActivoFijo);
        TipoUtilizacionAlta ClonarTipoUtilizacionAlta(TipoUtilizacionAlta tipoUtilizacionAlta);
    }
}
