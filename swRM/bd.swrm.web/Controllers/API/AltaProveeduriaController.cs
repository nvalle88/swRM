using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swrm.datos;
using bd.swrm.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.Enumeradores;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.ObjectTranfer;
using bd.swrm.entidades.Enumeradores;
using bd.log.guardar.Utiles;
using bd.swrm.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/AltaProveeduria")]
    public class AltaProveeduriaController : Controller
    {
        private readonly SwRMDbContext db;

        public AltaProveeduriaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarPaises
        [HttpGet]
        [Route("ListarAltasProveeduria")]
        public async Task<List<AltaProveeduria>> GetAltasProveeduria()
        {
            try
            {
                return await db.AltaProveeduria.OrderBy(x => x.FechaAlta).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<AltaProveeduria>();
            }
        }

        // GET: api/AltaProveeduria/5
        [HttpGet("{id}")]
        public async Task<Response> GetAltaProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var AltaProveeduria = await db.AltaProveeduria.SingleOrDefaultAsync(m => m.IdAlta == id);

                if (AltaProveeduria == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = AltaProveeduria,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
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

        // PUT: api/AltaProveeduria/5
        [HttpPut("{id}")]
        public async Task<Response> PutAltaProveeduria([FromRoute] int id, [FromBody] AltaProveeduria AltaProveeduria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var AltaProveeduriaActualizar = await db.AltaProveeduria.Where(x => x.IdArticulo == id).FirstOrDefaultAsync();
                if (AltaProveeduriaActualizar != null)
                {
                    try
                    {
                        AltaProveeduriaActualizar.Acreditacion = AltaProveeduria.Acreditacion;
                        AltaProveeduriaActualizar.FechaAlta = AltaProveeduria.FechaAlta;
                        AltaProveeduriaActualizar.IdProveedor = AltaProveeduria.IdProveedor;

                        db.AltaProveeduria.Update(AltaProveeduriaActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = Mensaje.ModeloInvalido,
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwRm),
                            ExceptionTrace = ex.Message,
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
                
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion
                };
            }
        }

        // POST: api/AltaProveeduria
        [HttpPost]
        [Route("InsertarAltaProveeduria")]
        public async Task<Response> PostAltaProveeduria([FromBody] AltaProveeduria AltaProveeduria)
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

                var respuesta = Existe(AltaProveeduria);
                if (!respuesta.IsSuccess)
                {
                    db.AltaProveeduria.Add(AltaProveeduria);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = AltaProveeduria
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
                    ExceptionTrace = ex.Message,
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

        // DELETE: api/AltaProveeduria/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteAltaProveeduria([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var respuesta = await db.AltaProveeduria.SingleOrDefaultAsync(m => m.IdArticulo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.AltaProveeduria.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex.Message,
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

        public Response Existe(AltaProveeduria AltaProveeduria)
        {
            var bdd = AltaProveeduria.IdArticulo;
            var bdd_fecha = AltaProveeduria.FechaAlta;
            var AltaProveeduriaRespuesta = db.AltaProveeduria.Where(p => p.IdArticulo == bdd && p.FechaAlta == bdd_fecha).FirstOrDefault();
            if (AltaProveeduriaRespuesta != null)
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
                Resultado = AltaProveeduriaRespuesta,
            };
        }
    }
}