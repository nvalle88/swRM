﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.swrm.entidades.Negocio
{
    public partial class OrdenCompra
    {
        public OrdenCompra()
        {
            OrdenCompraDetalles = new HashSet<OrdenCompraDetalles>();
            OrdenCompraRecepcionArticulos = new HashSet<OrdenCompraRecepcionArticulos>();
        }

        [Key]
        public int IdOrdenCompra { get; set; }

        [Display(Name = "Proveedor:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdProveedor { get; set; }
        public virtual Proveedor Proveedor { get; set; }

        public string Codigo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de salida:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Estado:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdEstado { get; set; }
        public virtual Estado Estado { get; set; }

        [Display(Name = "Empleado responsable:")]
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int IdEmpleadoResponsable { get; set; }
        public virtual Empleado EmpleadoResponsable { get; set; }

        [Display(Name = "Factura:")]
        [Required(ErrorMessage = "Debe seleccionar la {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IdFacturaActivoFijo { get; set; }
        public virtual FacturaActivoFijo Factura { get; set; }

        public virtual ICollection<OrdenCompraDetalles> OrdenCompraDetalles { get; set; }
        public virtual ICollection<OrdenCompraRecepcionArticulos> OrdenCompraRecepcionArticulos { get; set; }
    }
}
