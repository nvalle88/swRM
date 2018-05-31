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
using bd.swrm.servicios.Interfaces;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ProcesoJudicialActivoFijo")]
    public class ProcesoJudicialActivoFijoController : Controller
    {
        private readonly SwRMDbContext db;
        private readonly IUploadFileService uploadFileService;

        public ProcesoJudicialActivoFijoController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.db = db;
            this.uploadFileService = uploadFileService;
        }

        [HttpGet]
        [Route("ListarProcesoJudicialActivoFijo")]
        public async Task<List<ProcesoJudicialActivoFijo>> GetProcesoJudicialActivoFijo()
        {
            try
            {
                return await db.ProcesoJudicialActivoFijo.OrderBy(x => x.NumeroDenuncia).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ProcesoJudicialActivoFijo>();
            }
        }

        [HttpPost]
        [Route("ListarProcesoJudicialActivoFijoPorIdDetalleActivoFijo")]
        public async Task<List<ProcesoJudicialActivoFijo>> GetMantenimientosActivoFijoPorIdDetalleActivoFijo([FromBody] int idRecepcionActivoFijoDetalle)
        {
            try
            {
                return await db.ProcesoJudicialActivoFijo.Where(c => c.IdRecepcionActivoFijoDetalle == idRecepcionActivoFijoDetalle).OrderBy(x => x.NumeroDenuncia).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ProcesoJudicialActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetProcesoJudicialActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var procesoJudicialActivoFijo = await db.ProcesoJudicialActivoFijo.SingleOrDefaultAsync(m => m.IdProcesoJudicialActivoFijo == id);
                return new Response { IsSuccess = procesoJudicialActivoFijo != null, Message = procesoJudicialActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = procesoJudicialActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarProcesoJudicialActivoFijo")]
        public async Task<Response> PostProcesoJudicialActivoFijo([FromBody]ProcesoJudicialActivoFijo procesoJudicialActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ProcesoJudicialActivoFijo.AnyAsync(c => c.NumeroDenuncia.ToUpper().Trim() == procesoJudicialActivoFijo.NumeroDenuncia.ToUpper().Trim()))
                {
                    db.ProcesoJudicialActivoFijo.Add(procesoJudicialActivoFijo);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = procesoJudicialActivoFijo };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutProcesoJudicialActivoFijo([FromRoute] int id, [FromBody]ProcesoJudicialActivoFijo procesoJudicialActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ProcesoJudicialActivoFijo.Where(c => c.NumeroDenuncia.ToUpper().Trim() == procesoJudicialActivoFijo.NumeroDenuncia.ToUpper().Trim()).AnyAsync(c => c.IdProcesoJudicialActivoFijo != procesoJudicialActivoFijo.IdProcesoJudicialActivoFijo))
                {
                    var procesoJudicialActivoFijoActualizar = await db.ProcesoJudicialActivoFijo.Where(x => x.IdProcesoJudicialActivoFijo == id).FirstOrDefaultAsync();
                    if (procesoJudicialActivoFijoActualizar != null)
                    {
                        try
                        {
                            procesoJudicialActivoFijoActualizar.NumeroDenuncia = procesoJudicialActivoFijo.NumeroDenuncia;
                            procesoJudicialActivoFijoActualizar.IdRecepcionActivoFijoDetalle = procesoJudicialActivoFijo.IdRecepcionActivoFijoDetalle;
                            db.ProcesoJudicialActivoFijo.Update(procesoJudicialActivoFijoActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                        }
                        catch (Exception ex)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                            return new Response { IsSuccess = false, Message = Mensaje.Error };
                        }
                    }
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteProcesoJudicialActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ProcesoJudicialActivoFijo.SingleOrDefaultAsync(m => m.IdProcesoJudicialActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                using (var transaction = db.Database.BeginTransaction())
                {
                    var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdProcesoJudicialActivoFijo == respuesta.IdProcesoJudicialActivoFijo).ToListAsync();
                    db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);
                    await db.SaveChangesAsync();

                    db.ProcesoJudicialActivoFijo.Remove(respuesta);
                    await db.SaveChangesAsync();

                    foreach (var item in listaDocumentosActivoFijo)
                        uploadFileService.DeleteFile(item.Url);

                    transaction.Commit();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(ProcesoJudicialActivoFijo procesoJudicialActivoFijo)
        {
            var bdd = procesoJudicialActivoFijo.NumeroDenuncia.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.ProcesoJudicialActivoFijo.Where(p => p.NumeroDenuncia.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}