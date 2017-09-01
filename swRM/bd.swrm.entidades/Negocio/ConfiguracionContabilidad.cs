using System;
using System.Collections.Generic;

namespace bd.swrm.entidades.Negocio
{
    public partial class ConfiguracionContabilidad
    {
        public ConfiguracionContabilidad()
        {
            MotivoAsiento = new HashSet<MotivoAsiento>();
        }

        public int IdConfiguracionContabilidad { get; set; }
        public int? IdCatalogoCuentaD { get; set; }
        public int? IdCatalogoCuentaH { get; set; }
        public decimal ValorD { get; set; }
        public decimal ValorH { get; set; }

        public virtual ICollection<MotivoAsiento> MotivoAsiento { get; set; }
        public virtual CatalogoCuenta IdCatalogoCuentaDNavigation { get; set; }
        public virtual CatalogoCuenta IdCatalogoCuentaHNavigation { get; set; }
    }
}
