﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class CompaniaSeguro
    {
        public CompaniaSeguro()
        {
            ActivoFijo = new HashSet<ActivoFijo>();
        }

        [Key]
        public int IdCompaniaSeguro { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Compañía de seguro:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Fecha de inicio de vigencia:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicioVigencia { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Fecha de fin de vigencia:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinVigencia { get; set; }

        public virtual ICollection<ActivoFijo> ActivoFijo { get; set; }
    }
}
