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
using bd.swrm.entidades.ObjectTransfer;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/DocumentoActivoFijo")]
    public class DocumentoActivoFijoController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;

        public DocumentoActivoFijoController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
        }

        [HttpGet]
        [Route("ListarDocumentos")]
        public async Task<List<DocumentoActivoFijo>> GetDocumentoActivoFijo()
        {
            try
            {
                return await db.DocumentoActivoFijo.Include(c=> c.ActivoFijo).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<DocumentoActivoFijo>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetDocumentoActivoFijo([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var documentoActivoFijo = await db.DocumentoActivoFijo.Include(c=> c.ActivoFijo).SingleOrDefaultAsync(m => m.IdDocumentoActivoFijo == id);
                return new Response { IsSuccess = documentoActivoFijo != null, Message = documentoActivoFijo != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = documentoActivoFijo };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<Response> PostDocumentoActivoFijo([FromBody] DocumentoActivoFijoTransfer documentoActivoFijoTransfer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.DocumentoActivoFijo.AnyAsync(c => c.Nombre.ToUpper().Trim() == documentoActivoFijoTransfer.Nombre.ToUpper().Trim()))
                {
                    var documentoActivoFijo = await InsertarDocumentoActivoFijo(new DocumentoActivoFijo
                    {
                        Nombre = documentoActivoFijoTransfer.Nombre,
                        Fecha = DateTime.Now,
                        IdActivoFijo = documentoActivoFijoTransfer.IdActivoFijo,
                        IdRecepcionActivoFijoDetalle = documentoActivoFijoTransfer.IdRecepcionActivoFijoDetalle,
                        IdAltaActivoFijo = documentoActivoFijoTransfer.IdAltaActivoFijo,
                        IdFacturaActivoFijo = documentoActivoFijoTransfer.IdFacturaActivoFijo
                    });
                    string extensionFile = uploadFileService.FileExtension(documentoActivoFijoTransfer.Nombre);
                    await uploadFileService.UploadFile(documentoActivoFijoTransfer.Fichero, Mensaje.CarpetaActivoFijoDocumento, $"{documentoActivoFijo.IdDocumentoActivoFijo}{extensionFile}");

                    var seleccionado = await db.DocumentoActivoFijo.FindAsync(documentoActivoFijo.IdDocumentoActivoFijo);
                    seleccionado.Url = $"{Mensaje.CarpetaActivoFijoDocumento}/{documentoActivoFijo.IdDocumentoActivoFijo}{extensionFile}";
                    db.DocumentoActivoFijo.Update(seleccionado);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
                }
                return new Response { IsSuccess = false, Message = Mensaje.ExisteRegistro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("GetFile")]
        public async Task<Response> GetDocumentoActivoFijo([FromBody] DocumentoActivoFijo documentoActivoFijo)
        {
            try
            {
                var respuestaFile = uploadFileService.GetFileDocumentoActivoFijo(Mensaje.CarpetaActivoFijoDocumento, documentoActivoFijo.Nombre);
                return new Response { IsSuccess = respuestaFile != null, Message = respuestaFile != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = respuestaFile };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutDocumentoActivoFijo([FromRoute] int id, [FromBody]DocumentoActivoFijo documentoActivoFijo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.DocumentoActivoFijo.Where(c => c.Nombre.ToUpper().Trim() == documentoActivoFijo.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdDocumentoActivoFijo != documentoActivoFijo.IdDocumentoActivoFijo))
                {
                    var documentoActivoFijoActualizar = await db.DocumentoActivoFijo.FirstOrDefaultAsync(x => x.IdDocumentoActivoFijo == id);
                    if (documentoActivoFijoActualizar != null)
                    {
                        try
                        {
                            documentoActivoFijoActualizar.Nombre = documentoActivoFijo.Nombre;
                            db.DocumentoActivoFijo.Update(documentoActivoFijoActualizar);
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
        public async Task<Response> DeleteDocumentoActivoFijo([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.DocumentoActivoFijo.SingleOrDefaultAsync(m => m.IdDocumentoActivoFijo == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                var respuestaFile = uploadFileService.DeleteFile(Mensaje.CarpetaActivoFijoDocumento, $"{id}{uploadFileService.FileExtension(respuesta.Nombre)}");
                db.DocumentoActivoFijo.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        private async Task<DocumentoActivoFijo> InsertarDocumentoActivoFijo(DocumentoActivoFijo documentoActivoFijo)
        {
            db.DocumentoActivoFijo.Add(documentoActivoFijo);
            await db.SaveChangesAsync();
            return documentoActivoFijo;
        }
    }
}