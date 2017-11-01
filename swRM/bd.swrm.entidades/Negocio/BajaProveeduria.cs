using System;
using System.Collections.Generic;

namespace bd.swrm.entidades.Negocio
{
    public partial class BajaProveeduria
    {
        public int IdArticulo { get; set; }
        public int? IdProveedor { get; set; }
        public int IdBaja { get; set; }
        public DateTime? FechaBaja { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; }
    }
}
