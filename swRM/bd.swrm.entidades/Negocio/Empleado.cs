namespace bd.swrm.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Empleado
    {
        public Empleado()
        {
            AjusteInventarioArticulos = new HashSet<AjusteInventarioArticulos>();
            Bodega = new HashSet<Bodega>();
            TransferenciasActivoFijoEmpleadoResponsableEnvio = new HashSet<TransferenciaActivoFijo>();
            TransferenciasActivoFijoEmpleadoResponsableRecibo = new HashSet<TransferenciaActivoFijo>();
            UbicacionActivoFijo = new HashSet<UbicacionActivoFijo>();
            MantenimientoActivoFijo = new HashSet<MantenimientoActivoFijo>();
            MovilizacionesActivoFijoEmpleadoAutorizado = new HashSet<MovilizacionActivoFijo>();
            MovilizacionesActivoFijoEmpleadoResponsable = new HashSet<MovilizacionActivoFijo>();
            MovilizacionesActivoFijoEmpleadoSolicita = new HashSet<MovilizacionActivoFijo>();
            OrdenCompraEmpleadosDevolucion = new HashSet<OrdenCompra>();
            OrdenCompraEmpleadosResponsables = new HashSet<OrdenCompra>();
            RequerimientoArticulos = new HashSet<RequerimientoArticulos>();
            SalidaArticulosEmpleadosDespacho = new HashSet<SalidaArticulos>();
            SalidaArticulosEmpleadosRealizanBaja = new HashSet<SalidaArticulos>();
        }

        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de ingreso:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de ingreso al sector p�blico:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaIngresoSectorPublico { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "�Trabaj� en la Superintendencia de InstitucionesFinancieras?")]
        public bool TrabajoSuperintendenciaBanco { get; set; }

        public bool FondosReservas { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "�Declaraci�n jurada?")]
        public bool DeclaracionJurada { get; set; }
        
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

        [Display(Name = "Dependencia:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdDependencia { get; set; }
        public virtual Dependencia Dependencia { get; set; }

        [Display(Name = "Nombre de usuario:")]
        public string NombreUsuario { get; set; }

        public virtual ICollection<AjusteInventarioArticulos> AjusteInventarioArticulos { get; set; }
        public virtual ICollection<Bodega> Bodega { get; set; }
        public virtual ICollection<TransferenciaActivoFijo> TransferenciasActivoFijoEmpleadoResponsableEnvio { get; set; }
        public virtual ICollection<TransferenciaActivoFijo> TransferenciasActivoFijoEmpleadoResponsableRecibo { get; set; }
        public virtual ICollection<UbicacionActivoFijo> UbicacionActivoFijo { get; set; }
        public virtual ICollection<MantenimientoActivoFijo> MantenimientoActivoFijo { get; set; }
        public virtual ICollection<MovilizacionActivoFijo> MovilizacionesActivoFijoEmpleadoAutorizado { get; set; }
        public virtual ICollection<MovilizacionActivoFijo> MovilizacionesActivoFijoEmpleadoResponsable { get; set; }
        public virtual ICollection<MovilizacionActivoFijo> MovilizacionesActivoFijoEmpleadoSolicita { get; set; }
        public virtual ICollection<OrdenCompra> OrdenCompraEmpleadosResponsables { get; set; }
        public virtual ICollection<OrdenCompra> OrdenCompraEmpleadosDevolucion { get; set; }
        public virtual ICollection<RequerimientoArticulos> RequerimientoArticulos { get; set; }
        public virtual ICollection<SalidaArticulos> SalidaArticulosEmpleadosDespacho { get; set; }
        public virtual ICollection<SalidaArticulos> SalidaArticulosEmpleadosRealizanBaja { get; set; }
    }
}
