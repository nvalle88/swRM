using bd.swrm.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.ObjectTransfer
{
    public class ArticuloProveedoresTransfer
    {
        public Articulo Articulo { get; set; }
        public List<Proveedor> ListadoProveedores { get; set; }
    }
}
