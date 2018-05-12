namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CodigoActivoFijo
    {
        public CodigoActivoFijo()
        {
            ActivoFijo = new HashSet<ActivoFijo>();
            TransferenciaActivoFijo = new HashSet<TransferenciaActivoFijo>();
        }

        [Key]
        public int IdCodigoActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Código secuencial:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Codigosecuencial { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Código de barras:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CodigoBarras { get; set; }

        [NotMapped]
        public string CAF { get; set; }

        [NotMapped]
        public string SUBCAF { get; set; }

        [NotMapped]
        public string SUC { get; set; }

        //Propiedades Virtuales Referencias a otras clases
        public virtual ICollection<ActivoFijo> ActivoFijo { get; set; }

        public virtual ICollection<TransferenciaActivoFijo> TransferenciaActivoFijo { get; set; }
    }
}
