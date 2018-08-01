using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.Utils
{
    public static class Mensaje
    {
        public static string Excepcion { get { return "Ha ocurrido una Excepción"; } }
        public static string ExisteRegistro { get { return "Existe un registro de igual información"; } }
        public static string Satisfactorio { get { return "La acción se ha realizado satisfactoriamente"; } }
        public static string Error { get { return "Ha ocurrido un error inesperado"; } }
        public static string RegistroNoEncontrado { get { return "El registro solicitado no se ha encontrado"; } }
        public static string ModeloInvalido { get { return "El Módelo es inválido"; } }
        public static string BorradoNoSatisfactorio { get { return "No es posible eliminar el registro, existen relaciones que dependen de él"; } }
        public static string CarpetaActivoFijoDocumento { get { return "ActivoFijoDocumentos"; } }
    }

    public static class Estados
    {
        public static string Recepcionado { get { return "RECEPCIONADO"; } }
        public static string ValidacionTecnica { get { return "VALIDACIÓN TÉCNICA"; } }
        public static string Desaprobado { get { return "DESAPROBADO"; } }
        public static string Alta { get { return "ALTA"; } }
        public static string Baja { get { return "BAJA"; } }
        public static string Creada { get { return "CREADO"; } }
        public static string Aceptada { get { return "ACEPTADO"; } }
        public static string EnTramite { get { return "EN TRÁMITE"; } }
        public static string Procesada { get { return "PROCESADA"; } }
        public static string Solicitado { get { return "SOLICITADO"; } }
        public static string Despachado { get { return "DESPACHADO"; } }
    }

    public static class MotivosTransferencia
    {
        public static string CambioCustodio { get { return "CAMBIO DE CUSTODIO"; } }
        public static string CambioUbicacion { get { return "CAMBIO DE UBICACIÓN"; } }
        public static string PrestamoUsoExterno { get { return "PRÉSTAMO DE USO EXTERNO"; } }
        public static string PrestamoUsoInterno { get { return "PRÉSTAMO DE USO INTERNO"; } }
        public static string TransferenciaBodegas { get { return "TRANSFERENCIA ENTRE BODEGAS"; } }
    }

    public static class MotivosAlta
    {
        public static string Adicion { get { return "ADICIÓN"; } }
    }

    public static class ADMI_Grupos
    {
        public static string AdminNacionalProveeduria { get { return "Admin Nacional de Proveeduría"; } }
        public static string AdminZonalProveeduria { get { return "Admin Zonal de Proveeduría"; } }
        public static string FuncionarioSolicitante { get { return "Funcionario solicitante"; } }
    }
}
