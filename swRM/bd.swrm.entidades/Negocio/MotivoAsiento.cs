﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class MotivoAsiento
    {
        [Key]
        public int IdMotivoAsiento { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Motivo de asiento:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        [Display(Name = "Configuración de contabilidad:")]
        [Required(ErrorMessage = "Debe seleccionar la {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IdConfiguracionContabilidad { get; set; }

        public virtual ConfiguracionContabilidad ConfiguracionContabilidad { get; set; }
    }
}
