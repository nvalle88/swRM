﻿namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Dependencia
    {
        [Key]
        public int IdDependencia { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Dependencia:")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "La {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Dependencia padre:")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdDependenciaPadre { get; set; }
        public virtual Dependencia DependenciaPadre { get; set; }

        [Display(Name = "Sucursal:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int IdSucursal { get; set; }
        public virtual Sucursal Sucursal { get; set; }

        [Display(Name = "Bodega:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdBodega { get; set; }
        public virtual Bodega Bodega { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual ICollection<Dependencia> InverseDependenciaPadreIdDependenciaNavigation { get; set; }
    }
}
