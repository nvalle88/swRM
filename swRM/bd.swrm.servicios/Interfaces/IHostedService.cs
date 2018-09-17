using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bd.swrm.servicios.Interfaces
{
    public interface IHostedService
    {
        Task StartAsync();
        Task StopAsync();
    }
}
