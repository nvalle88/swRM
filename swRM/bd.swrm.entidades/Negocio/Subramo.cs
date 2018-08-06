﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class Subramo
    {
        public Subramo()
        {
            SubClaseActivoFijo = new HashSet<SubClaseActivoFijo>();
        }

        [Key]
        public int IdSubramo { get; set; }

        [Display(Name = "Ramo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdRamo { get; set; }
        public virtual Ramo Ramo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Subramo:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }
        
        public virtual ICollection<SubClaseActivoFijo> SubClaseActivoFijo { get; set; }
    }
}
