namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class CatalogoCuenta
    {
        [Key]
        public int IdCatalogoCuenta { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "C�digo:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Codigo { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Cat�logo de cuenta:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdCatalogoCuentaHijo { get; set; }
        public virtual CatalogoCuenta CatalogoCuentaReferencia { get; set; }

        public virtual ICollection<CatalogoCuenta> CatalogoCuentas { get; set; }

    }
}
