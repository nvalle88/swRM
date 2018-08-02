namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ClaseArticulo
    {
        [Key]
        public int IdClaseArticulo { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Clase de art�culo:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Tipo de art�culo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdTipoArticulo { get; set; }

        public virtual TipoArticulo TipoArticulo { get; set; }

        public virtual ICollection<SubClaseArticulo> SubClaseArticulo { get; set; }
    }
}
