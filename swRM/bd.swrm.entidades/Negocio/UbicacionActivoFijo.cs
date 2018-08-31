using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd.swrm.entidades.Negocio
{
    public partial class UbicacionActivoFijo
    {
        public UbicacionActivoFijo()
        {
            AltaActivoFijoDetalle = new HashSet<AltaActivoFijoDetalle>();
            TransferenciasActivoFijoDetalleDestino = new HashSet<TransferenciaActivoFijoDetalle>();
            TransferenciasActivoFijoDetalleOrigen = new HashSet<TransferenciaActivoFijoDetalle>();
        }

        [Key]
        public int IdUbicacionActivoFijo { get; set; }

        [Display(Name = "Custodio:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int? IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }

        [Display(Name = "Bodega:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int? IdBodega { get; set; }
        public virtual Bodega Bodega { get; set; }

        [Display(Name = "Detalle de recepción de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdRecepcionActivoFijoDetalle { get; set; }
        public virtual RecepcionActivoFijoDetalle RecepcionActivoFijoDetalle { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Fecha de ubicación:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FechaUbicacion { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "¿Confirmación?")]
        public bool Confirmacion { get; set; }

        [NotMapped]
        [Display(Name = "Motivo de ubicación:")]
        public string MotivoUbicacion { get; set; }

        public virtual ICollection<AltaActivoFijoDetalle> AltaActivoFijoDetalle { get; set; }
        public virtual ICollection<TransferenciaActivoFijoDetalle> TransferenciasActivoFijoDetalleDestino { get; set; }
        public virtual ICollection<TransferenciaActivoFijoDetalle> TransferenciasActivoFijoDetalleOrigen { get; set; }
    }
}
