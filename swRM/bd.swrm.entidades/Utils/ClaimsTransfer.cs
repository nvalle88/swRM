using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.Utils
{
    public class ClaimsTransfer
    {
        public int? IdSucursal { get; set; }
        public int? IdDependencia { get; set; }
        public int? IdEmpleado { get; set; }
        public bool IsAdminNacionalProveeduria { get; set; }
        public bool IsAdminZonalProveeduria { get; set; }
        public bool IsFuncionarioSolicitante { get; set; }
    }
}
