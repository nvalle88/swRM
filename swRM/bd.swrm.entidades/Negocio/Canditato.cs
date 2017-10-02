namespace bd.swrm.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Canditato
    {
        [Key]
        public int IdCanditato { get; set; }

        public virtual ICollection<Persona> Persona { get; set; }
    }
}
