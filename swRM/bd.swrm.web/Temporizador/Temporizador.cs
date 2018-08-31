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
using bd.swrm.entidades.Constantes;

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

        #region Depreciación de Activos Fijos
        public static void InicializarTemporizadorDepreciacion()
        {
            DateTime fechaEjecucionTimer = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ConstantesTimerDepreciacion.Hora, ConstantesTimerDepreciacion.Minutos, ConstantesTimerDepreciacion.Segundos);
            TimeSpan tiempoEspera = new TimeSpan();

            if (DateTime.Now > fechaEjecucionTimer)
            {
                var fechaMannana = fechaEjecucionTimer.AddDays(1);
                tiempoEspera = fechaMannana - DateTime.Now;
            }
            else
                tiempoEspera = fechaEjecucionTimer - DateTime.Now;
            
            InicializarTemporizador(timerDepreciacion, async () =>
            {
                await DepreciacionActivosFijosAlta();
                await ExistenciaMaestroArticuloSucursal();
            }, tiempoEspera, new TimeSpan(24, 0, 0));
        }
        private static async Task<DepreciacionActivoFijo> InsertarDepreciacionActivoFijo(decimal valorCompra, decimal depreciacionAcumulada, decimal valorResidual, DateTime ultimaFechaDepreciacionAlta, int idRecepcionActivoFijoDetalle)
        {
            var nuevaDepreciacionActivoFijo = new DepreciacionActivoFijo
            {
                ValorCompra = valorCompra,
                FechaDepreciacion = ultimaFechaDepreciacionAlta.AddMonths(1),
                DepreciacionAcumulada = depreciacionAcumulada,
                IdRecepcionActivoFijoDetalle = idRecepcionActivoFijoDetalle,
                IsRevalorizacion = false
            };

            if (nuevaDepreciacionActivoFijo.ValorResidual <= 0)
                nuevaDepreciacionActivoFijo.ValorResidual = 1;

            db.DepreciacionActivoFijo.Add(nuevaDepreciacionActivoFijo);
            await db.SaveChangesAsync();

            nuevaDepreciacionActivoFijo.ValorResidual = valorResidual;
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
                var listaRecepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.CategoriaActivoFijo)
                    .Include(c=> c.ActivoFijo).ThenInclude(c=> c.RecepcionActivoFijoDetalle)
                    .Where(c => c.Estado.Nombre == Estados.Alta && c.ActivoFijo.Depreciacion).Include(c => c.DepreciacionActivoFijo)
                    .ToListAsync();
                foreach (var recepcionActivoFijoDetalle in listaRecepcionActivoFijoDetalle)
                {
                    var ultimaDepreciacion = recepcionActivoFijoDetalle?.DepreciacionActivoFijo.OrderByDescending(c => c.FechaDepreciacion).FirstOrDefault();
                    decimal valorCompraRevalorizacion = ultimaDepreciacion != null ? ultimaDepreciacion.ValorCompra : recepcionActivoFijoDetalle.ActivoFijo.ValorCompra / recepcionActivoFijoDetalle.ActivoFijo.RecepcionActivoFijoDetalle.Count;
                    var porCientoDepreciacionAnual = recepcionActivoFijoDetalle.ActivoFijo.SubClaseActivoFijo.ClaseActivoFijo.CategoriaActivoFijo.PorCientoDepreciacionAnual;
                    var totalDepreciacionAnual = (valorCompraRevalorizacion * porCientoDepreciacionAnual) / 100;
                    var indiceDepreciacionMensual = totalDepreciacionAnual / 12;
                    
                    if (ultimaDepreciacion != null)
                    {
                        ultimaDepreciacion.ValorResidual = ultimaDepreciacion.ValorCompra - ultimaDepreciacion.DepreciacionAcumulada;
                        if ((ultimaDepreciacion.FechaDepreciacion.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                        {
                            if (ultimaDepreciacion.ValorResidual > 1)
                            {
                                var nuevaDepreciacionActivoFijo = await InsertarDepreciacionActivoFijo(valorCompraRevalorizacion, (ultimaDepreciacion.DepreciacionAcumulada + indiceDepreciacionMensual), (ultimaDepreciacion.ValorResidual - indiceDepreciacionMensual), ultimaDepreciacion.FechaDepreciacion, recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                                await CalculoRecursivoDepreciacion(nuevaDepreciacionActivoFijo, indiceDepreciacionMensual);
                            }
                        }
                    }
                    else
                    {
                        var recepcionActivoFijoDetalleAltaActivoFijo = db.AltaActivoFijoDetalle.Include(c => c.AltaActivoFijo).ThenInclude(c => c.FacturaActivoFijo).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                        if ((recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FechaAlta.Subtract(DateTime.Now).TotalDays) * (-1) >= 30)
                        {
                            var nuevaDepreciacionActivoFijo = await InsertarDepreciacionActivoFijo(valorCompraRevalorizacion, (indiceDepreciacionMensual), (valorCompraRevalorizacion - indiceDepreciacionMensual), recepcionActivoFijoDetalleAltaActivoFijo.AltaActivoFijo.FechaAlta, recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                            await CalculoRecursivoDepreciacion(nuevaDepreciacionActivoFijo, indiceDepreciacionMensual);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
            }
        }
        public static decimal ObtenerValorCompraRealActivoFijo(RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var ultimaDepreciacionActivoFijo = db.DepreciacionActivoFijo.OrderByDescending(c => c.FechaDepreciacion).FirstOrDefault(c => c.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle);
                return ultimaDepreciacionActivoFijo != null ? (ultimaDepreciacionActivoFijo.ValorCompra - ultimaDepreciacionActivoFijo.DepreciacionAcumulada) : recepcionActivoFijoDetalle.ActivoFijo.ValorCompra / db.RecepcionActivoFijoDetalle.Count(c=> c.IdActivoFijo == recepcionActivoFijoDetalle.IdActivoFijo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Maestro de artículo de sucursal
        private static async Task ExistenciaMaestroArticuloSucursal()
        {
            try
            {
                var maestrosArticulosSucursal = await db.MaestroArticuloSucursal.Where(c => c.Habilitado).ToListAsync();
                foreach (var item in maestrosArticulosSucursal)
                {
                    if (item.FechaSinExistencia != null)
                    {
                        if (item.FechaSinExistencia.Value.AddMonths(6) < DateTime.Now)
                        {
                            var inventarioArticulo = await db.InventarioArticulos.Include(c=> c.Bodega).Where(c=> c.Bodega.IdSucursal == item.IdSucursal && c.IdMaestroArticuloSucursal == item.IdMaestroArticuloSucursal).ToListAsync();
                            if (inventarioArticulo.Count == 0 || (inventarioArticulo.Sum(c=> c.Cantidad) == 0))
                            {
                                item.Habilitado = false;
                                item.FechaSinExistencia = null;
                            }
                        }
                    }
                    else
                        item.FechaSinExistencia = DateTime.Now;
                }
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
            }
        }
        #endregion
    }
}
