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
    [Route("api/RecepcionArticulo")]
    public class RecepcionArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public RecepcionArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarRecepcionArticulos
        [HttpGet]
        [Route("ListarRecepcionArticulos")]
        public async Task<List<RecepcionArticulos>> GetRecepcionArticulo()
        {
            try
            {
                return await db.RecepcionArticulos.ToListAsync();
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
                return new List<RecepcionArticulos>();
            }
        }

        // GET: api/RecepcionArticulo/5
        [HttpGet("{id}")]
        public async Task<Response> GetRecepcionArticulo([FromRoute] int id)
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

                var recepcionArticulo = await db.RecepcionArticulos.SingleOrDefaultAsync(m => m.IdRecepcionArticulos == id);

                if (recepcionArticulo == null)
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
                    Resultado = recepcionArticulo,
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

        // PUT: api/RecepcionArticulo/5
        [HttpPut("{id}")]
        public async Task<Response> PutRecepcionArticulo([FromRoute] int id, [FromBody] RecepcionArticulos recepcionArticulo)
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

                var recepcionArticuloActualizar = await db.RecepcionArticulos.Where(x => x.IdRecepcionArticulos == id).FirstOrDefaultAsync();
                if (recepcionArticuloActualizar != null)
                {
                    try
                    {
                        recepcionArticuloActualizar.Fecha = recepcionArticulo.Fecha;
                        recepcionArticuloActualizar.Cantidad = recepcionArticulo.Cantidad;
                        recepcionArticuloActualizar.IdEmpleado = recepcionArticulo.IdEmpleado;
                        recepcionArticuloActualizar.IdArticulo = recepcionArticulo.IdArticulo;
                        recepcionArticuloActualizar.IdMaestroArticuloSucursal = recepcionArticulo.IdMaestroArticuloSucursal;
                        recepcionArticuloActualizar.IdProveedor = recepcionArticulo.IdProveedor;
                        db.RecepcionArticulos.Update(recepcionArticuloActualizar);
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

        // POST: api/RecepcionArticulo
        [HttpPost]
        [Route("InsertarRecepcionArticulo")]
        public async Task<Response> PostRecepcionArticulo([FromBody] RecepcionArticulos recepcionArticulo)
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

                db.Entry(recepcionArticulo.Articulo).State = EntityState.Unchanged;
                db.Entry(recepcionArticulo.MaestroArticuloSucursal).State = EntityState.Unchanged;
                db.Entry(recepcionArticulo).State = EntityState.Added;
                await db.SaveChangesAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio
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

        // DELETE: api/RecepcionArticulo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteRecepcionArticulo([FromRoute] int id)
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

                var respuesta = await db.RecepcionArticulos.SingleOrDefaultAsync(m => m.IdRecepcionArticulos == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.RecepcionArticulos.Remove(respuesta);
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
    }
}