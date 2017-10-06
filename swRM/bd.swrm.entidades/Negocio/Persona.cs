﻿

namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de nacimiento:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Identificación:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombres:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellidos:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono privado:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono de la Casa:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoCasa { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Correo electrónico privado:")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El {0} no cumple con el formáto establecido")]
        public string CorreoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Lugar de trabajo:")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string LugarTrabajo { get; set; }


        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Estado civil:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdEstadoCivil { get; set; }
        public virtual EstadoCivil EstadoCivil { get; set; }

        [Display(Name = "Etnia:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdEtnia { get; set; }
        public virtual Etnia Etnia { get; set; }

        [Display(Name = "Género:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdGenero { get; set; }
        public virtual Genero Genero { get; set; }

        [Display(Name = "Nacionalidad:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public virtual Nacionalidad Nacionalidad { get; set; }
        public int? IdNacionalidad { get; set; }

        [Display(Name = "Sub clase de activo fijo:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdSexo { get; set; }
        public virtual Sexo Sexo { get; set; }

        [Display(Name = "Tipo de identificación:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdTipoIdentificacion { get; set; }
        public virtual TipoIdentificacion TipoIdentificacion { get; set; }

        [Display(Name = "Tipo de sangre:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdTipoSangre { get; set; }
        public virtual TipoSangre TipoSangre { get; set; }

        [Display(Name = "Candidato:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdCanditato { get; set; }
        public virtual Canditato Canditato { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}
