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
            timer = new Timer((c) => { accion(); }, accion, tiempoEsperaFuncionCallBack, periodoEsperaFuncionCallBack);
        }

        public static async Task DepreciacionActivosFijosAlta()
        {
            Action<decimal, decimal, int> insertarDepreciacionActivoFijo = async (depreciacionAcumulada, valorResidual, idRecepcionActivoFijoDetalle) =>
            {
                var nuevaDepreciacionActivoFijo = new DepreciacionActivoFijo
                {
                    FechaDepreciacion = DateTime.Now,
                    DepreciacionAcumulada = depreciacionAcumulada,
                    ValorResidual = valorResidual,
                    IdRecepcionActivoFijoDetalle = idRecepcionActivoFijoDetalle
                };

                if (nuevaDepreciacionActivoFijo.ValorResidual <= 0)
                    nuevaDepreciacionActivoFijo.ValorResidual = 1;

                db.DepreciacionActivoFijo.Add(nuevaDepreciacionActivoFijo);
                await db.SaveChangesAsync();
            };

            try
            {
                var listaRecepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Where(c => c.Estado.Nombre == Estados.Alta && c.ActivoFijo.Depreciacion).Include(c => c.DepreciacionActivoFijo.OrderByDescending(p=> p.FechaDepreciacion)).ToListAsync();
                if (listaRecepcionActivoFijoDetalle.Count == 0)
                    timerDepreciacion.Dispose();
                else
                {
                    foreach (var recepcionActivoFijoDetalle in listaRecepcionActivoFijoDetalle)
                    {
                        var ultimaDepreciacion = recepcionActivoFijoDetalle?.DepreciacionActivoFijo.FirstOrDefault();
                        if (ultimaDepreciacion != null)
                        {
                            if ((ultimaDepreciacion.FechaDepreciacion.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                            {
                                if (ultimaDepreciacion.ValorResidual > 1)
                                    insertarDepreciacionActivoFijo((ultimaDepreciacion.DepreciacionAcumulada + recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.TablaDepreciacion.IndiceDepreciacion), (ultimaDepreciacion.ValorResidual - recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.TablaDepreciacion.IndiceDepreciacion), recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                            }
                        }
                        else
                        {
                            var recepcionActivoFijoDetalleAltaActivoFijo = await db.RecepcionActivoFijoDetalleAltaActivoFijo
                                .Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo)
                                .Where(c=> c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle)
                                .FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijo);
                            if ((recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FechaAlta.Subtract(DateTime.Now).TotalDays) *(-1) >= 30)
                                insertarDepreciacionActivoFijo((recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.TablaDepreciacion.IndiceDepreciacion), (recepcionActivoFijoDetalle.ActivoFijo.ValorCompra - recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.TablaDepreciacion.IndiceDepreciacion), recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
            }
        }

        private static SwRMDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<SwRMDbContext>();
            builder.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            return new SwRMDbContext(builder.Options);
        }

        public static void InicializarTemporizadorDepreciacion()
        {
            InicializarTemporizador(timerDepreciacion, async () => { await DepreciacionActivosFijosAlta(); }, new TimeSpan(0, 0, 5), new TimeSpan(24, 0, 0));
        }
    }
}
