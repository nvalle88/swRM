using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.Negocio
{
    public partial class ExistenciaArticuloProveeduria
    {
        public int IdArticulo { get; set; }
        public int Existencia { get; set; }

        public virtual Articulo Articulo { get; set; }
    }
}
