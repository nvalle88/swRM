using bd.swrm.entidades.Utils;
using bd.swrm.servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bd.swrm.servicios.Servicios
{
    public class ClaimsTransferService : IClaimsTransfer
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClaimsTransferService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ClaimsTransfer ObtenerClaimsTransferHttpContext()
        {
            try
            {
                return (ClaimsTransfer)httpContextAccessor.HttpContext.Items.FirstOrDefault(c => c.Key.ToString() == "ClaimsTransfer").Value;
            }
            catch (Exception)
            {
                return new ClaimsTransfer();
            }
        }
    }
}
