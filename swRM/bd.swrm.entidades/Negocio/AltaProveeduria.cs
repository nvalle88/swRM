using System;
using System.Collections.Generic;

namespace bd.swrm.entidades.Negocio
{
    public partial class AltaProveeduria
    {
        public int IdArticulo { get; set; }
        public int? IdProveedor { get; set; }
        public byte[] Acreditacion { get; set; }
        public DateTime? FechaAlta { get; set; }
        public int IdAlta { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; }

        public virtual ICollection<FacturasPorAltaProveeduria> FacturasPorAltaProveeduria { get; set; }
    }
}
