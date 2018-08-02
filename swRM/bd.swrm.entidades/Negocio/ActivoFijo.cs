namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ActivoFijo
    {
        public ActivoFijo()
        {
            DocumentosActivoFijo = new HashSet<DocumentoActivoFijo>();
            RecepcionActivoFijoDetalle = new HashSet<RecepcionActivoFijoDetalle>();
        }

        [Key]
        public int IdActivoFijo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Activo fijo:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Valor de compra:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorCompra { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "�Depreciaci�n?")]
        public bool Depreciacion { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "�Validacion t�cnica?")]
        public bool ValidacionTecnica { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Subclase de activo fijo:")]
        [Required(ErrorMessage = "Debe seleccionar la {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IdSubClaseActivoFijo { get; set; }
        public virtual SubClaseActivoFijo SubClaseActivoFijo { get; set; }

        [Display(Name = "Modelo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdModelo { get; set; }
        public virtual Modelo Modelo { get; set; }

        public virtual ICollection<DocumentoActivoFijo> DocumentosActivoFijo { get; set; }

        public virtual ICollection<RecepcionActivoFijoDetalle> RecepcionActivoFijoDetalle { get; set; }
    }
}