﻿using System;
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
        public static string Recepcionado { get { return "Recepcionado"; } }
        public static string ValidacionTecnica { get { return "Validación Técnica"; } }
        public static string Desaprobado { get { return "Desaprobado"; } }
        public static string Alta { get { return "Alta"; } }
        public static string Baja { get { return "Baja"; } }
        public static string Creada { get { return "Creado"; } }
        public static string Aceptada { get { return "Aceptado"; } }
        public static string EnTramite { get { return "En trámite"; } }
        public static string Procesada { get { return "Procesada"; } }
        public static string Solicitado { get { return "Solicitado"; } }
        public static string Despachado { get { return "Despachado"; } }
    }

    public static class MotivosTransferencia
    {
        public static string CambioCustodio { get { return "Cambio de Custodio"; } }
        public static string CambioUbicacion { get { return "Cambio de Ubicación"; } }
        public static string PrestamoUsoExterno { get { return "Préstamo de Uso Externo"; } }
        public static string PrestamoUsoInterno { get { return "Préstamo de Uso Interno"; } }
        public static string TransferenciaBodegas { get { return "Transferencia entre Bodegas"; } }
    }

    public static class ADMI_Grupos
    {
        public static string AdminNacionalProveeduria { get { return "Admin Nacional de Proveeduría"; } }
        public static string AdminZonalProveeduria { get { return "Admin Zonal de Proveeduría"; } }
        public static string FuncionarioSolicitante { get { return "Funcionario solicitante"; } }
    }
}
