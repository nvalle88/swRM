namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BajaActivoFijo
    {
        [Key]
        public int IdBajaActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Fecha de baja:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FechaBaja { get; set; }

        [Display(Name = "N�mero de memo, oficio o resoluci�n:")]
        [Required(ErrorMessage = "Debe introducir el {0}")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string MemoOficioResolucion { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Motivo de baja:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdMotivoBaja { get; set; }
        public virtual MotivoBaja MotivoBaja { get; set; }

        public virtual ICollection<BajaActivoFijoDetalle> BajaActivoFijoDetalle { get; set; }
    }
}
