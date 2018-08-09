﻿using bd.swrm.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.swrm.entidades.ObjectTransfer
{
    public class CambioUbicacionSucursalViewModel
    {
        public int IdTransferenciaActivoFijo { get; set; }

        [Display(Name = "Sucursal de origen:")]
        [Required(ErrorMessage = "Debe seleccionar la {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int IdSucursalOrigen { get; set; }
        public virtual Sucursal SucursalOrigen { get; set; }

        [Display(Name = "Custodio que entrega:")]
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleadoEntrega { get; set; }
        public virtual Empleado EmpleadoEntrega { get; set; }

        [Display(Name = "Responsable de envío:")]
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleadoResponsableEnvio { get; set; }
        public virtual Empleado EmpleadoResponsableEnvio { get; set; }

        [Display(Name = "Sucursal de destino:")]
        [Required(ErrorMessage = "Debe seleccionar la {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int IdSucursalDestino { get; set; }
        public virtual Sucursal SucursalDestino { get; set; }

        [Display(Name = "Custodio que recibe:")]
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleadoRecibe { get; set; }
        public virtual Empleado EmpleadoRecibe { get; set; }

        [Display(Name = "Responsable de recibo:")]
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleadoResponsableRecibo { get; set; }
        public virtual Empleado EmpleadoResponsableRecibo { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Fecha de transferencia:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FechaTransferencia { get; set; }

        [Display(Name = "Observaciones:")]
        public string Observaciones { get; set; }

        public ICollection<int> ListadoIdRecepcionActivoFijoDetalle { get; set; }
    }
}
