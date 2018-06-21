using bd.swrm.entidades.Utils;
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

            claimsTransfer.NombreSucursal = ObtenerHeader(httpContext, "NombreSucursal");
            
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
