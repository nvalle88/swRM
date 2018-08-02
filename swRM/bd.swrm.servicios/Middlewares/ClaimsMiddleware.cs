﻿using bd.swrm.entidades.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bd.swrm.servicios.Middlewares
{
    public class ClaimsMiddleware
    {
        private RequestDelegate nextDelegate;

        public ClaimsMiddleware(RequestDelegate next)
        {
            nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var claimsTransfer = new ClaimsTransfer();
            var valorIdSucursal = ObtenerHeader(httpContext, "IdSucursal");
            if (!String.IsNullOrEmpty(valorIdSucursal))
                claimsTransfer.IdSucursal = int.Parse(valorIdSucursal);

            var valorIdDependencia = ObtenerHeader(httpContext, "IdDependencia");
            if (!String.IsNullOrEmpty(valorIdDependencia))
                claimsTransfer.IdDependencia = int.Parse(valorIdDependencia);

            var valorIdEmpleado = ObtenerHeader(httpContext, "IdEmpleado");
            if (!String.IsNullOrEmpty(valorIdEmpleado))
                claimsTransfer.IdEmpleado = int.Parse(valorIdEmpleado);

            var valorIsAdminNacionalProveeduria = ObtenerHeader(httpContext, "IsAdminNacionalProveeduria");
            if (!String.IsNullOrEmpty(valorIsAdminNacionalProveeduria))
                claimsTransfer.IsAdminNacionalProveeduria = bool.Parse(valorIsAdminNacionalProveeduria);

            var valorIsAdminZonalProveeduria = ObtenerHeader(httpContext, "IsAdminZonalProveeduria");
            if (!String.IsNullOrEmpty(valorIsAdminZonalProveeduria))
                claimsTransfer.IsAdminZonalProveeduria = bool.Parse(valorIsAdminZonalProveeduria);

            var valorIsFuncionarioSolicitante = ObtenerHeader(httpContext, "IsFuncionarioSolicitante");
            if (!String.IsNullOrEmpty(valorIsFuncionarioSolicitante))
                claimsTransfer.IsFuncionarioSolicitante = bool.Parse(valorIsFuncionarioSolicitante);

            var valorIsAdminAF = ObtenerHeader(httpContext, "IsAdminAF");
            if (!String.IsNullOrEmpty(valorIsAdminAF))
                claimsTransfer.IsAdminAF = bool.Parse(valorIsAdminAF);

            var valorIsEncargadoSeguros = ObtenerHeader(httpContext, "IsEncargadoSeguros");
            if (!String.IsNullOrEmpty(valorIsEncargadoSeguros))
                claimsTransfer.IsEncargadoSeguros = bool.Parse(valorIsEncargadoSeguros);

            httpContext.Items.Add("ClaimsTransfer", claimsTransfer);
            await nextDelegate.Invoke(httpContext);
        }

        private string ObtenerHeader(HttpContext httpContext, string key)
        {
            try
            {
                StringValues valor;
                httpContext.Request.Headers.TryGetValue(key, out valor);
                var arrValor = valor.ToArray();

                if (arrValor.Length > 0)
                    return arrValor[0];
            }
            catch (Exception)
            { }
            return null;
        }
    }
}
