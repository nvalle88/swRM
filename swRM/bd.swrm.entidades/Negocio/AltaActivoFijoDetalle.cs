﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd.swrm.entidades.Negocio
{
    public partial class AltaActivoFijoDetalle
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Detalle de recepción de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdRecepcionActivoFijoDetalle { get; set; }
        public virtual RecepcionActivoFijoDetalle RecepcionActivoFijoDetalle { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Alta de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdAltaActivoFijo { get; set; }
        public virtual AltaActivoFijo AltaActivoFijo { get; set; }

        [Display(Name = "Tipo de utilización:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdTipoUtilizacionAlta { get; set; }
        public virtual TipoUtilizacionAlta TipoUtilizacionAlta { get; set; }

        [Display(Name = "Ubicación de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar la {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IdUbicacionActivoFijo { get; set; }
        public virtual UbicacionActivoFijo UbicacionActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "¿Es Componente?")]
        public bool IsComponente { get; set; }

        [NotMapped]
        [Display(Name = "Componentes")]
        public string Componentes { get; set; }
    }
}
