namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Marca
    {
        [Key]
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Marca:")]
        [StringLength(200,MinimumLength =2,ErrorMessage ="La {0} no puede tener menos de {2} y m�s de {1}")]
        public string Nombre { get; set; }

        public virtual ICollection<Modelo> Modelo { get; set; }
    }
}
