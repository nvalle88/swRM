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
    [Route("api/CompaniaSeguro")]
    public class CompaniaSeguroController : Controller
    {
        private readonly SwRMDbContext db;
        private readonly IUploadFileService uploadFileService;

        public CompaniaSeguroController(SwRMDbContext db, IUploadFileService uploadFileService)
        {
            this.db = db;
            this.uploadFileService = uploadFileService;
        }

        [HttpGet]
        [Route("ListarCompaniaSeguro")]
        public async Task<List<CompaniaSeguro>> GetCompaniaSeguro()
        {
            try
            {
                return await db.CompaniaSeguro.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<CompaniaSeguro>();
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetCompaniaSeguro([FromRoute]int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var companiaSeguro = await db.CompaniaSeguro.SingleOrDefaultAsync(m => m.IdCompaniaSeguro == id);
                return new Response { IsSuccess = companiaSeguro != null, Message = companiaSeguro != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = companiaSeguro };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("InsertarCompaniaSeguro")]
        public async Task<Response> PostCompaniaSeguro([FromBody]CompaniaSeguro companiaSeguro)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CompaniaSeguro.AnyAsync(c => c.Nombre.ToUpper().Trim() == companiaSeguro.Nombre.ToUpper().Trim()))
                {
                    db.CompaniaSeguro.Add(companiaSeguro);
                    await db.SaveChangesAsync();
                    return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = companiaSeguro };
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
        public async Task<Response> PutCompaniaSeguro([FromRoute] int id, [FromBody]CompaniaSeguro companiaSeguro)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.CompaniaSeguro.Where(c => c.Nombre.ToUpper().Trim() == companiaSeguro.Nombre.ToUpper().Trim()).AnyAsync(c => c.IdCompaniaSeguro != companiaSeguro.IdCompaniaSeguro))
                {
                    var companiaSeguroActualizar = await db.CompaniaSeguro.Where(x => x.IdCompaniaSeguro == id).FirstOrDefaultAsync();
                    if (companiaSeguroActualizar != null)
                    {
                        try
                        {
                            companiaSeguroActualizar.Nombre = companiaSeguro.Nombre;
                            companiaSeguroActualizar.FechaInicioVigencia = companiaSeguro.FechaInicioVigencia;
                            companiaSeguroActualizar.FechaFinVigencia = companiaSeguro.FechaFinVigencia;
                            db.CompaniaSeguro.Update(companiaSeguroActualizar);
                            await db.SaveChangesAsync();
                            return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = companiaSeguroActualizar };
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
        public async Task<Response> DeleteCompaniaSeguro([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.CompaniaSeguro.SingleOrDefaultAsync(m => m.IdCompaniaSeguro == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                using (var transaction = db.Database.BeginTransaction())
                {
                    var listaDocumentosActivoFijo = await db.DocumentoActivoFijo.Where(c => c.IdCompaniaSeguro == respuesta.IdCompaniaSeguro).ToListAsync();
                    db.DocumentoActivoFijo.RemoveRange(listaDocumentosActivoFijo);
                    await db.SaveChangesAsync();

                    db.CompaniaSeguro.Remove(respuesta);
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
    }
}