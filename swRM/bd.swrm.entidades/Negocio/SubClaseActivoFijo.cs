namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
    public partial class SubClaseActivoFijo
    {
        [Key]
        public int IdSubClaseActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Subclase de activo fijo:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Clase de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar la {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IdClaseActivoFijo { get; set; }
        public virtual ClaseActivoFijo ClaseActivoFijo { get; set; }

        [Display(Name = "Subramo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdSubramo { get; set; }
        public virtual Subramo Subramo { get; set; }

        public virtual ICollection<ActivoFijo> ActivoFijo { get; set; }
    }
}
