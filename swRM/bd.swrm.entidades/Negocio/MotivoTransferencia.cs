﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.swrm.entidades.Negocio
{
    public partial class MotivoTransferencia
    {
        [Key]
        public int IdMotivoTransferencia { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Motivo de Transferencia:")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El {0} no puede tener menos de {2} y más de {1}")]
        public string Motivo_Transferencia { get; set; }

        public virtual ICollection<TransferenciaActivoFijo> TransferenciaActivoFijo { get; set; }
    }
}
