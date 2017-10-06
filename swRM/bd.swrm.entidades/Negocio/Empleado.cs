namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de ingreso:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de ingreso al sector p�blico:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaIngresoSectorPublico { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "�Trabaj� en la Superintendencia de InstitucionesFinancieras?")]
        public bool TrabajoSuperintendenciaBanco { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nepotismo:")]
        public bool Nepotismo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "�Declaraci�n jurada?")]
        public bool DeclaracionJurada { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingreso por otra actividad:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string IngresosOtraActividad { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Mesees de imposici�n:")]
        [Range(0, 600, ErrorMessage = "El {0} debe estar entre {1} y {2} ")]
        public int MesesImposiciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "D�as de imposici�n:")]
        [Range(0, 31, ErrorMessage = "El {0} debe estar entre {1} y {2} ")]
        public int DiasImposiciones { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Persona:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPersona { get; set; }
        public virtual Persona Persona { get; set; }

        [Display(Name = "Ciudad de nacimiento:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdCiudadLugarNacimiento { get; set; }
        public virtual Ciudad CiudadNacimiento { get; set; }

        [Display(Name = "Provincia de sufragio:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProvinciaLugarSufragio { get; set; }
        public virtual Provincia ProvinciaSufragio { get; set; }
        public virtual ICollection<TransferenciaActivoFijo> TransferenciaActivoFijo { get; set; }

        [Display(Name = "Dependencia:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdDependencia { get; set; }
        public virtual Dependencia Dependencia { get; set; }

    }
}
