using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class ConfiguracionContabilidad
    {
        public ConfiguracionContabilidad()
        {
            MotivoAsiento = new HashSet<MotivoAsiento>();
        }

        [Key]
        public int IdConfiguracionContabilidad { get; set; }

        [Display(Name = "Cuenta Debe:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdCatalogoCuentaD { get; set; }
        public virtual CatalogoCuenta CatalogoCuentaD { get; set; }

        [Display(Name = "Cuenta Haber")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdCatalogoCuentaH { get; set; }
        public virtual CatalogoCuenta CatalogoCuentaH { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Cuenta Debe:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorD { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Cuenta haber:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorH { get; set; }

        public virtual ICollection<MotivoAsiento> MotivoAsiento { get; set; }
    }
}
