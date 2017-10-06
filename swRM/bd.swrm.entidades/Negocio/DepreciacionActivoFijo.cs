namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public partial class DepreciacionActivoFijo
    {
        [Key]
        public int IdDepreciacionActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de depreciaci�n:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaDepreciacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Depreciaci�n acumulada:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal DepreciacionAcumulada { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Valor Residual:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorResidual { get; set; }

        //Propiedades Virtuales Referencias a otras clases
        [Display(Name = "Activo fijo:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdActivoFijo { get; set; }
        public virtual ActivoFijo ActivoFijo { get; set; }
    }
}
