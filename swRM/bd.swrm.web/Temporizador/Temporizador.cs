using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Utiles;
using bd.swrm.entidades.Utils;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace bd.swrm.web.Temporizador
{
    public class Temporizador
    {
        private static SwRMDbContext db;
        public static Timer timerDepreciacion { get; set; }

        public static void InicializarTemporizador(Timer timer, Action accion, TimeSpan tiempoEsperaFuncionCallBack, TimeSpan periodoEsperaFuncionCallBack)
        {
            db = db ?? CreateDbContext();
            timer = new Timer((c) => {
                accion();
            }, accion, tiempoEsperaFuncionCallBack, periodoEsperaFuncionCallBack);
        }

        public static async Task ComprobarActivosFijosAlta()
        {
            try
            {
                var lista = await db.RecepcionActivoFijoDetalle.Where(c => c.Estado.Nombre == "Alta").Include(c => c.ActivoFijo.DepreciacionActivoFijo).ToListAsync();
                if (lista.Count == 0)
                    timerDepreciacion.Dispose();
                else
                {
                    foreach (var item in lista)
                    {
                        var depreciacionActivoFijo = item?.ActivoFijo?.DepreciacionActivoFijo?.FirstOrDefault();
                        if (depreciacionActivoFijo != null)
                        {
                            var ll = depreciacionActivoFijo.FechaDepreciacion.Subtract(DateTime.Now).TotalDays;
                            if ((depreciacionActivoFijo.FechaDepreciacion.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                            {
                                //Buscar el indice (indiceDepreciacion) de cada uno de los Activos Fijos contra la Tabla TablaDepreciacion
                                if (depreciacionActivoFijo.ValorResidual > 1)
                                {
                                    var indiceDepreciacion = await db.TablaDepreciacion.FirstOrDefaultAsync();
                                    depreciacionActivoFijo.DepreciacionAcumulada += indiceDepreciacion.IndiceDepreciacion;
                                    depreciacionActivoFijo.FechaDepreciacion = DateTime.Now;
                                    depreciacionActivoFijo.ValorResidual -= depreciacionActivoFijo.DepreciacionAcumulada;
                                    if (depreciacionActivoFijo.ValorResidual <= 0)
                                        depreciacionActivoFijo.ValorResidual = 1;
                                    db.Entry(depreciacionActivoFijo.ActivoFijo).State = EntityState.Detached;
                                    db.Entry(depreciacionActivoFijo).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",
                });
            }
        }

        private static SwRMDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<SwRMDbContext>();

            var connectionString = configuration.GetConnectionString("SwRMConnection");

            builder.UseSqlServer(connectionString);

            builder.ConfigureWarnings(w =>
                w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            return new SwRMDbContext(builder.Options);
        }

        public static void InicializarTemporizadorDepreciacion()
        {
            InicializarTemporizador(timerDepreciacion, async () => {
                await ComprobarActivosFijosAlta();
            }, new TimeSpan(0, 0, 5), new TimeSpan(24, 0, 0));
        }
    }
}
