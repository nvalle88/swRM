﻿namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TipoIdentificacion
    {
        [Key]
        public int IdTipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "tipo de identificación:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        public virtual ICollection<Persona> Persona { get; set; }
    }
}
