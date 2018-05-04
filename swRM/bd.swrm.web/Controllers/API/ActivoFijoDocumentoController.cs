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
    [Route("api/ActivoFijoDocumento")]
    public class ActivoFijoDocumentoController : Controller
    {
        private readonly IUploadFileService uploadFileService;
        private readonly SwRMDbContext db;

        public ActivoFijoDocumentoController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
            this.db = db;
        }

        [HttpGet]
        [Route("ListarDocumentos")]
        public async Task<List<ActivoFijoDocumento>> GetActivoFijoDocumento()
        {
            try
            {
                return await db.ActivoFijoDocumento.Include(c=> c.ActivoFijo).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<ActivoFijoDocumento>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetActivoFijoDocumento([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var activoFijoDocumento = await db.ActivoFijoDocumento.Include(c=> c.ActivoFijo).SingleOrDefaultAsync(m => m.IdActivoFijoDocumento == id);
                return new Response { IsSuccess = activoFijoDocumento != null, Message = activoFijoDocumento != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = activoFijoDocumento };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<Response> PostActivoFijoDocumento([FromBody] ActivoFijoDocumentoTransfer activoFijoDocumentoTransfer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivoFijoDocumento.AnyAsync(c => c.Nombre.ToUpper().Trim() == activoFijoDocumentoTransfer.Nombre.ToUpper().Trim()))
                {
                    var activoFijoDocumento = await InsertarActivoFijoDocumento(new ActivoFijoDocumento { Nombre = activoFijoDocumentoTransfer.Nombre, Fecha = DateTime.Now, IdActivo = activoFijoDocumentoTransfer.IdActivoFijo });
                    string extensionFile = uploadFileService.FileExtension(activoFijoDocumentoTransfer.Nombre);
                    await uploadFileService.UploadFile(activoFijoDocumentoTransfer.Fichero, "ActivoFijoDocumentos", $"{activoFijoDocumento.IdActivoFijoDocumento}{extensionFile}");

                    var seleccionado = await db.ActivoFijoDocumento.FindAsync(activoFijoDocumento.IdActivoFijoDocumento);
                    seleccionado.Url = $"ActivoFijoDocumentos/{activoFijoDocumento.IdActivoFijoDocumento}{extensionFile}";
                    db.ActivoFijoDocumento.Update(seleccionado);
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
        public async Task<Response> GetActivoFijoDocumento([FromBody] ActivoFijoDocumento activoFijoDocumento)
        {
            try
            {
                var respuestaFile = uploadFileService.GetFileActivoFijoDocumento("ActivoFijoDocumentos", activoFijoDocumento.Nombre);
                return new Response { IsSuccess = respuestaFile != null, Message = respuestaFile != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = respuestaFile };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPut("{id}")]
        public async Task<Response> PutActivoFijoDocumento([FromRoute] int id, [FromBody]ActivoFijoDocumento activoFijoDocumento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.ActivoFijoDocumento.Where(c => c.Nombre.ToUpper().Trim() == activoFijoDocumento.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdActivoFijoDocumento != activoFijoDocumento.IdActivoFijoDocumento))
                {
                    var activoFijoDocumentoActualizar = await db.ActivoFijoDocumento.FirstOrDefaultAsync(x => x.IdActivoFijoDocumento == id);
                    if (activoFijoDocumentoActualizar != null)
                    {
                        try
                        {
                            activoFijoDocumentoActualizar.Nombre = activoFijoDocumento.Nombre;
                            db.ActivoFijoDocumento.Update(activoFijoDocumentoActualizar);
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
        public async Task<Response> DeleteDocumentoInformacionInstitucional([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.ActivoFijoDocumento.SingleOrDefaultAsync(m => m.IdActivoFijoDocumento == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                var respuestaFile = uploadFileService.DeleteFile("ActivoFijoDocumentos", $"{id}{uploadFileService.FileExtension(respuesta.Nombre)}");
                db.ActivoFijoDocumento.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        private async Task<ActivoFijoDocumento> InsertarActivoFijoDocumento(ActivoFijoDocumento activoFijoDocumento)
        {
            db.ActivoFijoDocumento.Add(activoFijoDocumento);
            await db.SaveChangesAsync();
            return activoFijoDocumento;
        }
    }
}