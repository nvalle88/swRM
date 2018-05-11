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
using System.Linq.Expressions;
using bd.swrm.servicios.Interfaces;
using bd.swrm.entidades.ObjectTransfer;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/RecepcionActivoFijo")]
    public class RecepcionActivoFijoController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;

        public RecepcionActivoFijoController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarRecepcionActivoFijo")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijo()
        {
            return await ListadoRecepcionActivoFijo();
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoPorEstado/{estado}")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoPorEstado(string estado)
        {
            return await ListadoRecepcionActivoFijo(c=> c.Estado.Nombre == estado);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoConPoliza")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoConPoliza()
        {
            return await ListadoRecepcionActivoFijo(c => c.Estado.Nombre == "Recepcionado" && c.NumeroPoliza != null);
        }

        [HttpGet]
        [Route("ListarRecepcionActivoFijoSinPoliza")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetRecepcionActivoFijoSinPoliza()
        {
            return await ListadoRecepcionActivoFijo(c => c.Estado.Nombre == "Recepcionado" && c.NumeroPoliza == null);
        }

        [HttpGet]
        [Route("BienesReporte")]
        public async Task<List<RecepcionActivoFijoDetalle>> GetBienesReporte()
        {
            var listaRecepcionActivoFijo = await ListadoRecepcionActivoFijo(c => c.Estado.Nombre != "Validación Técnica" && c.Estado.Nombre != "Desaprobado");
            return listaRecepcionActivoFijo;
        }

        private async Task<List<RecepcionActivoFijoDetalle>> ListadoRecepcionActivoFijo(Expression<Func<RecepcionActivoFijoDetalle, bool>> predicado = null)
        {
            try
            {
                var lista = (from recAFD in db.RecepcionActivoFijoDetalle
                              join recAF in db.RecepcionActivoFijo on recAFD.IdRecepcionActivoFijo equals recAF.IdRecepcionActivoFijo
                              join af in db.ActivoFijo on recAFD.IdActivoFijo equals af.IdActivoFijo
                              join est in db.Estado on recAFD.IdEstado equals est.IdEstado
                              join prov in db.Proveedor on recAF.IdProveedor equals prov.IdProveedor
                              join codAF in db.CodigoActivoFijo on af.IdCodigoActivoFijo equals codAF.IdCodigoActivoFijo
                              join motRec in db.MotivoRecepcion on recAF.IdMotivoRecepcion equals motRec.IdMotivoRecepcion
                              join um in db.UnidadMedida on af.IdUnidadMedida equals um.IdUnidadMedida
                              join mod in db.Modelo on af.IdModelo equals mod.IdModelo
                              join marca in db.Marca on mod.IdMarca equals marca.IdMarca
                              select new RecepcionActivoFijoDetalle
                              {
                                  IdRecepcionActivoFijoDetalle = recAFD.IdRecepcionActivoFijoDetalle,
                                  IdActivoFijo = recAFD.IdActivoFijo,
                                  IdEstado = recAFD.IdEstado,
                                  IdRecepcionActivoFijo = recAFD.IdRecepcionActivoFijo,
                                  NumeroPoliza = recAFD.NumeroPoliza,
                                  RecepcionActivoFijo = new RecepcionActivoFijo { Fondo = recAF.Fondo, OrdenCompra = recAF.OrdenCompra, Cantidad = recAF.Cantidad, ValidacionTecnica = recAF.ValidacionTecnica, FechaRecepcion = recAF.FechaRecepcion, IdProveedor = recAF.IdProveedor, Proveedor = new Proveedor { IdProveedor = prov.IdProveedor, Nombre = prov.Nombre, Apellidos = prov.Apellidos } }
                              });
                return await (predicado != null ? lista.Where(predicado).ToListAsync() : lista.ToListAsync());
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<RecepcionActivoFijoDetalle>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetRecepcionActivoFijoDetalle([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var recepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.Proveedor)
                    .Include(c => c.RecepcionActivoFijo).ThenInclude(c => c.MotivoRecepcion)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.SubClaseActivoFijo).ThenInclude(c => c.ClaseActivoFijo).ThenInclude(c => c.TipoActivoFijo)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.UnidadMedida)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.Modelo).ThenInclude(c => c.Marca)
                    .Include(c => c.ActivoFijo).ThenInclude(c => c.CodigoActivoFijo)
                    .Include(c => c.ActivoFijo)
                    .Include(c => c.Estado)
                    .Include(c => c.RecepcionActivoFijo.Proveedor.Factura)
                    .Where(c=> c.IdRecepcionActivoFijoDetalle == id).SingleOrDefaultAsync();

                recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.SUBCAF = recepcionActivoFijoDetalle?.ActivoFijo?.SubClaseActivoFijo?.ClaseActivoFijo?.TipoActivoFijo?.Nombre;
                recepcionActivoFijoDetalle.ActivoFijo.CodigoActivoFijo.CAF = recepcionActivoFijoDetalle?.ActivoFijo?.SubClaseActivoFijo?.ClaseActivoFijo?.Nombre;
                return new Response { IsSuccess = recepcionActivoFijoDetalle != null, Message = recepcionActivoFijoDetalle != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijoDetalle };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpGet("EstadoActivoFijo/{id}")]
        public async Task<Response> GetEstadoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var recepcionActivoFijoDetalle = await db.RecepcionActivoFijoDetalle.Include(c => c.Estado).FirstOrDefaultAsync(c => c.IdRecepcionActivoFijoDetalle == id);
                return new Response { IsSuccess = recepcionActivoFijoDetalle != null && recepcionActivoFijoDetalle.Estado != null, Message = recepcionActivoFijoDetalle != null && recepcionActivoFijoDetalle.Estado != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = recepcionActivoFijoDetalle?.Estado };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("EstadoActivoFijo/{id}")]
        public async Task<Response> PutModificarEstadoActivoFijo([FromRoute] int id, [FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.Where(x => x.IdRecepcionActivoFijoDetalle == id).FirstOrDefaultAsync();
                if (recepcionActivoFijoDetalleActualizar != null)
                {
                    try
                    {
                        recepcionActivoFijoDetalleActualizar.Estado = await db.Estado.SingleOrDefaultAsync(c => c.Nombre == recepcionActivoFijoDetalle.Estado.Nombre);
                        recepcionActivoFijoDetalleActualizar.IdEstado = recepcionActivoFijoDetalleActualizar.Estado.IdEstado;
                        db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
                        await db.SaveChangesAsync();
                        return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                        return new Response { IsSuccess = false, Message = Mensaje.Error };
                    }
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpPost]
        [Route("AsignarPoliza")]
        public async Task<Response> AsignarPoliza([FromBody] RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            try
            {
                if (String.IsNullOrEmpty(recepcionActivoFijoDetalle.NumeroPoliza))
                    return new Response { IsSuccess = false, Message = "Debe introducir el Número de Póliza." };

                var recepcionActivoFijoDetalleActualizar = await db.RecepcionActivoFijoDetalle.Where(x => x.IdRecepcionActivoFijoDetalle == recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle).FirstOrDefaultAsync();
                if (recepcionActivoFijoDetalleActualizar != null)
                {
                    if (!(recepcionActivoFijoDetalleActualizar.NumeroPoliza == null ? await db.RecepcionActivoFijoDetalle.AnyAsync(c => c.NumeroPoliza == recepcionActivoFijoDetalle.NumeroPoliza) : await db.RecepcionActivoFijoDetalle.Where(c => c.NumeroPoliza == recepcionActivoFijoDetalle.NumeroPoliza).AnyAsync(c => c.IdRecepcionActivoFijoDetalle != recepcionActivoFijoDetalle.IdRecepcionActivoFijoDetalle)))
                    {
                        try
                        {
                            recepcionActivoFijoDetalleActualizar.NumeroPoliza = recepcionActivoFijoDetalle.NumeroPoliza;
                            db.RecepcionActivoFijoDetalle.Update(recepcionActivoFijoDetalleActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                            return new Response { IsSuccess = false, Message = Mensaje.Error };
                        }
                    }
                    return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        public Response Existe(RecepcionActivoFijoDetalle recepcionActivoFijoDetalle)
        {
            var nombreActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.Nombre.ToUpper().TrimEnd().TrimStart();
            var modeloActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdModelo;
            var unidadMedidaActivoFijo = recepcionActivoFijoDetalle.ActivoFijo.IdUnidadMedida;
            var fondoRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.Fondo.ToUpper().TrimStart().TrimEnd();
            var ordenCompraRecepcion = recepcionActivoFijoDetalle.RecepcionActivoFijo.OrdenCompra.ToUpper().TrimStart().TrimEnd();

            var loglevelrespuesta = db.RecepcionActivoFijoDetalle.Where(p => p.ActivoFijo.Nombre.ToUpper().TrimStart().TrimEnd() == nombreActivoFijo
            && p.ActivoFijo.IdModelo == modeloActivoFijo
            && p.ActivoFijo.IdUnidadMedida == unidadMedidaActivoFijo
            && p.RecepcionActivoFijo.Fondo == fondoRecepcion
            && p.RecepcionActivoFijo.OrdenCompra == ordenCompraRecepcion).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}