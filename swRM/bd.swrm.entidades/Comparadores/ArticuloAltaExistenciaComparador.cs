using bd.swrm.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.Comparadores
{
    public class ArticuloAltaExistenciaComparador : IEqualityComparer<RecepcionArticulos>
    {
        public bool Equals(RecepcionArticulos x, RecepcionArticulos y)
        {
            try
            {
                return x.IdArticulo == y.IdArticulo;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetHashCode(RecepcionArticulos obj)
        {
            return obj.IdArticulo.GetHashCode();
        }
    }
}
