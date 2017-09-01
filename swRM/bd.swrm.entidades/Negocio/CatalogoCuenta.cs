using System;
using System.Collections.Generic;

namespace bd.swrm.entidades.Negocio
{
    public partial class CatalogoCuenta
    {
        public CatalogoCuenta()
        {
            ConfiguracionContabilidadIdCatalogoCuentaDNavigation = new HashSet<ConfiguracionContabilidad>();
            ConfiguracionContabilidadIdCatalogoCuentaHNavigation = new HashSet<ConfiguracionContabilidad>();
        }

        public int IdCatalogoCuenta { get; set; }
        public int IdCatalogoCuentaHijo { get; set; }
        public string Codigo { get; set; }

        public virtual ICollection<ConfiguracionContabilidad> ConfiguracionContabilidadIdCatalogoCuentaDNavigation { get; set; }
        public virtual ICollection<ConfiguracionContabilidad> ConfiguracionContabilidadIdCatalogoCuentaHNavigation { get; set; }
        public virtual CatalogoCuenta IdCatalogoCuentaHijoNavigation { get; set; }
        public virtual ICollection<CatalogoCuenta> InverseIdCatalogoCuentaHijoNavigation { get; set; }
    }
}
