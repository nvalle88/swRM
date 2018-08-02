namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
    public partial class TipoArticulo
    {
        [Key]
        public int IdTipoArticulo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Tipo de art�culo:")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<ClaseArticulo> ClaseArticulo { get; set; }
    }
}
