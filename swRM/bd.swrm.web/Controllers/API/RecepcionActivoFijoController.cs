using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Utiles;
using bd.swrm.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/RecepcionActivoFijo")]
    public class RecepcionActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;

        public RecepcionActivoFijoController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/RecepcionActivoFijo
        [HttpGet]
        [Route("ListarRecepcionActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijo()
        {
            try
            {
                return await db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Empleado)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.MotivoRecepcion)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.SubClaseActivoFijo).ThenInclude(c=> c.ClaseActivoFijo).ThenInclude(c=> c.TipoActivoFijo)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c=> c.LibroActivoFijo).ThenInclude(c=> c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.LibroActivoFijo).ThenInclude(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c => c.ActivoFijo).ThenInclude(c=> c.UnidadMedida)
                    .Include(c => c.ActivoFijo).ThenInclude(c=> c.Modelo).ThenInclude(c=> c.Marca)
                    .Include(c=> c.ActivoFijo).ThenInclude(c=> c.CodigoActivoFijo)
                    .ToListAsync();
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
                return new List<RecepcionActivoFijoDetalle>();
            }
        }

        // POST: api/Articulo
        [HttpPost]
        [Route("InsertarRecepcionActivoFijo")]
        public async Task<Response> PostArticulo([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var respuesta = Existe(recepcionActivoFijoDetalle);
                if (!respuesta.IsSuccess)
                {
                    db.RecepcionActivoFijoDetalle.Add(recepcionActivoFijoDetalle);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };

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
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        public Response Existe(RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            var nombreActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Nombre.ToUpper().TrimEnd().TrimStart();
            var serieActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Serie.ToUpper().TrimEnd().TrimStart();
            var ubicacionActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Ubicacion.ToUpper().TrimEnd().TrimStart();
            var modeloActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdModelo;
            var unidadMedidaActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdUnidadMedida;
            var fondoRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.Fondo.ToUpper().TrimStart().TrimEnd();
            var ordenCompraRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.OrdenCompra.ToUpper().TrimStart().TrimEnd();

            var loglevelrespuesta = db.RecepcionActivoFijoDetalle.Where(p => p.ActivoFijo.Nombre.ToUpper().TrimStart().TrimEnd() == nombreActivoFijo
            && p.ActivoFijo.Serie.ToUpper().TrimStart().TrimEnd() == serieActivoFijo
            && p.ActivoFijo.Ubicacion.ToUpper().TrimStart().TrimEnd() == ubicacionActivoFijo
            && p.ActivoFijo.IdModelo == modeloActivoFijo
            && p.ActivoFijo.IdUnidadMedida == unidadMedidaActivoFijo
            && p.RecepcionActivoFijo.Fondo == fondoRecepcion
            && p.RecepcionActivoFijo.OrdenCompra == ordenCompraRecepcion).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.ExisteRegistro,
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = loglevelrespuesta,
            };
        }
    }
}