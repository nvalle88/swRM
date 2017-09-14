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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/CatalogoCuenta")]
    public class CatalogoCuentaController : Controller
    {
        private readonly SwRMDbContext db;

        public CatalogoCuentaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarCatalogosCuenta
        [HttpGet]
        [Route("ListarCatalogosCuenta")]
        public async Task<List<CatalogoCuenta>> GetCatalogoCuenta()
        {
            try
            {
                return await db.CatalogoCuenta.OrderBy(x => x.Codigo).ToListAsync();
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
                return new List<CatalogoCuenta>();
            }
        }

        // GET: api/CatalogosCuenta/5
        [HttpGet("{id}")]
        public async Task<Response> GetCatalogosCuenta([FromRoute] int id)
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

                var catalogoCuenta = await db.CatalogoCuenta.SingleOrDefaultAsync(m => m.IdCatalogoCuenta == id);

                if (catalogoCuenta == null)
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
                    Resultado = catalogoCuenta,
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

        // PUT: api/CatalogoCuenta/5
        [HttpPut("{id}")]
        public async Task<Response> PutCatalogoCuenta([FromRoute] int id, [FromBody] CatalogoCuenta catalogoCuenta)
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

                var catalogoCuentaActualizar = await db.CatalogoCuenta.Where(x => x.IdCatalogoCuenta == id).FirstOrDefaultAsync();
                if (catalogoCuentaActualizar != null)
                {
                    try
                    {
                        catalogoCuentaActualizar.Codigo = catalogoCuenta.Codigo;
                        catalogoCuentaActualizar.IdCatalogoCuentaHijo = catalogoCuenta.IdCatalogoCuentaHijo;
                        db.CatalogoCuenta.Update(catalogoCuentaActualizar);
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

        // POST: api/CatalogoCuenta
        [HttpPost]
        [Route("InsertarCatalogoCuenta")]
        public async Task<Response> PostCatalogoCuenta([FromBody] CatalogoCuenta catalogoCuenta)
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

                var respuesta = Existe(catalogoCuenta);
                if (!respuesta.IsSuccess)
                {
                    db.CatalogoCuenta.Add(catalogoCuenta);
                    await db.SaveChangesAsync();

                    if (catalogoCuenta.IdCatalogoCuentaHijo == 0)
                    {
                        catalogoCuenta = db.CatalogoCuenta.FirstOrDefault();
                        if (catalogoCuenta != null)
                        {
                            catalogoCuenta.IdCatalogoCuentaHijo = catalogoCuenta.IdCatalogoCuenta;
                            await db.SaveChangesAsync();
                        }
                    }

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

        // DELETE: api/CatalogoCuenta/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteCatalogoCuenta([FromRoute] int id)
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

                var respuesta = await db.CatalogoCuenta.SingleOrDefaultAsync(m => m.IdCatalogoCuenta == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.CatalogoCuenta.Remove(respuesta);
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

        public Response Existe(CatalogoCuenta catalogoCuenta)
        {
            var bdd = catalogoCuenta.Codigo.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.CatalogoCuenta.Where(p => p.Codigo.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
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
