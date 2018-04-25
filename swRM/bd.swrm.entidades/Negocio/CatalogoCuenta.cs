using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class CatalogoCuenta
    {
        public CatalogoCuenta()
        {
            ConfiguracionContabilidadIdCatalogoCuentaDNavigation = new HashSet<ConfiguracionContabilidad>();
            ConfiguracionContabilidadIdCatalogoCuentaHNavigation = new HashSet<ConfiguracionContabilidad>();
        }

        [Key]
        public int IdCatalogoCuenta { get; set; }

        [Display(Name = "Catálogo de cuenta:")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdCatalogoCuentaHijo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Código:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        [RegularExpression(@"^[-A-Z0-9a-z-]*$", ErrorMessage = "El {0} tiene que ser alfanumérico.")]
        public string Codigo { get; set; }

        public virtual ICollection<ConfiguracionContabilidad> ConfiguracionContabilidadIdCatalogoCuentaDNavigation { get; set; }
        public virtual ICollection<ConfiguracionContabilidad> ConfiguracionContabilidadIdCatalogoCuentaHNavigation { get; set; }
        public virtual CatalogoCuenta CatalogoCuentaHijo { get; set; }
        public virtual ICollection<CatalogoCuenta> CatalogosCuenta { get; set; }
    }
}
