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

        private static SwRMDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<SwRMDbContext>();
            builder.UseSqlServer(Startup.Configuration.GetConnectionString("SwRMConnection"));
            builder.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            return new SwRMDbContext(builder.Options);
        }

        public static void InicializarTemporizadorDepreciacion()
        {
            InicializarTemporizador(timerDepreciacion, async () => { await DepreciacionActivosFijosAlta(); }, new TimeSpan(0, 0, 5), new TimeSpan(24, 0, 0));
        }

        #region Depreciación de Activos Fijos
        private static async Task<DepreciacionActivoFijo> InsertarDepreciacionActivoFijo(decimal valorCompra, decimal depreciacionAcumulada, decimal valorResidual, DateTime ultimaFechaDepreciacionAlta, int idRecepcionActivoFijoDetalle)
        {
            var nuevaDepreciacionActivoFijo = new DepreciacionActivoFijo
            {
                ValorCompra = valorCompra,
                FechaDepreciacion = ultimaFechaDepreciacionAlta.AddMonths(1),
                DepreciacionAcumulada = depreciacionAcumulada,
                ValorResidual = valorResidual,
                IdRecepcionActivoFijoDetalle = idRecepcionActivoFijoDetalle
            };

            if (nuevaDepreciacionActivoFijo.ValorResidual <= 0)
                nuevaDepreciacionActivoFijo.ValorResidual = 1;

            db.DepreciacionActivoFijo.Add(nuevaDepreciacionActivoFijo);
            await db.SaveChangesAsync();
            return nuevaDepreciacionActivoFijo;
        }

        private static async Task CalculoRecursivoDepreciacion(DepreciacionActivoFijo ultimaDepreciacion, decimal indiceDepreciacionMensual)
        {
            if (ultimaDepreciacion != null)
            {
                if ((ultimaDepreciacion.FechaDepreciacion.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                {
                    if (ultimaDepreciacion.ValorResidual > 1)
                    {
                        var nuevaDepreciacionActivoFijo = await InsertarDepreciacionActivoFijo(ultimaDepreciacion.ValorCompra, (ultimaDepreciacion.DepreciacionAcumulada + indiceDepreciacionMensual), (ultimaDepreciacion.ValorResidual - indiceDepreciacionMensual), ultimaDepreciacion.FechaDepreciacion, ultimaDepreciacion.IdRecepcionActivoFijoDetalle);
                        await CalculoRecursivoDepreciacion(nuevaDepreciacionActivoFijo, indiceDepreciacionMensual);
                    }
                }
            }
        }

        public static async Task DepreciacionActivosFijosAlta()
        {
            try
            {
                var listaRecepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Include(c => c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.CategoriaActivoFijo).Where(c => c.Estado.Nombre == Estados.Alta && c.ActivoFijo.Depreciacion).Include(c => c.DepreciacionActivoFijo).ToListAsync();
                if (listaRecepcionActivoFijoDetalle.Count == 0)
                    timerDepreciacion.Dispose();
                else
                {
                    foreach (var recepcionActivoFijoDetalle in listaRecepcionActivoFijoDetalle)
                    {
                        var porCientoDepreciacionAnual = recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.CategoriaActivoFijo.PorCientoDepreciacionAnual;
                        var totalDepreciacionAnual = (recepcionActivoFijoDetalle.ActivoFijo.ValorCompra * porCientoDepreciacionAnual) / 100;
                        var indiceDepreciacionMensual = totalDepreciacionAnual / 12;

                        var ultimaDepreciacion = recepcionActivoFijoDetalle?.DepreciacionActivoFijo.OrderByDescending(c => c.FechaDepreciacion).FirstOrDefault();
                        if (ultimaDepreciacion != null)
                        {
                            if ((ultimaDepreciacion.FechaDepreciacion.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                            {
                                if (ultimaDepreciacion.ValorResidual > 1)
                                {
                                    var nuevaDepreciacionActivoFijo = await InsertarDepreciacionActivoFijo(ultimaDepreciacion.ValorCompra, (ultimaDepreciacion.DepreciacionAcumulada + indiceDepreciacionMensual), (ultimaDepreciacion.ValorResidual - indiceDepreciacionMensual), ultimaDepreciacion.FechaDepreciacion, recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                                    await CalculoRecursivoDepreciacion(nuevaDepreciacionActivoFijo, indiceDepreciacionMensual);
                                }
                            }
                        }
                        else
                        {
                            var recepcionActivoFijoDetalleAltaActivoFijo = db.AltaActivoFijoDetalle.Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                            if ((recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FechaAlta.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                            {
                                var nuevaDepreciacionActivoFijo = await InsertarDepreciacionActivoFijo(recepcionActivoFijoDetalle.ActivoFijo.ValorCompra, (indiceDepreciacionMensual), (recepcionActivoFijoDetalle.ActivoFijo.ValorCompra - indiceDepreciacionMensual), recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FechaAlta, recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                                await CalculoRecursivoDepreciacion(nuevaDepreciacionActivoFijo, indiceDepreciacionMensual);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
            }
        }
        #endregion
    }
}
