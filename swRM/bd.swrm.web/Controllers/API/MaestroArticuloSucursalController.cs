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
    [Route("api/MaestroArticuloSucursal")]
    public class MaestroArticuloSucursalController : Controller
    {
        private readonly SwRMDbContext db;

        public MaestroArticuloSucursalController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/MaestroArticuloSucursal
        [HttpGet]
        [Route("ListarMaestroArticuloSucursal")]
        public async Task<List<MaestroArticuloSucursal>> GetMaestroArticuloSucursal()
        {
            try
            {
                return await db.MaestroArticuloSucursal.OrderBy(c=> c.Minimo).ThenBy(c=> c.Maximo).Include(c => c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais).ToListAsync();
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
                return new List<MaestroArticuloSucursal>();
            }
        }

        // GET: api/MaestroArticuloSucursal/5
        [HttpGet("{id}")]
        public async Task<Response> GetMaestroArticuloSucursal([FromRoute] int id)
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

                var maestroArticuloSucursal = await db.MaestroArticuloSucursal.SingleOrDefaultAsync(m => m.IdMaestroArticuloSucursal == id);
                maestroArticuloSucursal.Sucursal = await db.Sucursal.SingleOrDefaultAsync(c => c.IdSucursal == maestroArticuloSucursal.IdSucursal);
                maestroArticuloSucursal.Sucursal.Ciudad = await db.Ciudad.SingleOrDefaultAsync(c => c.IdCiudad == maestroArticuloSucursal.Sucursal.IdCiudad);
                maestroArticuloSucursal.Sucursal.Ciudad.Provincia = await db.Provincia.SingleOrDefaultAsync(c => c.IdProvincia == maestroArticuloSucursal.Sucursal.Ciudad.IdProvincia);
                maestroArticuloSucursal.Sucursal.Ciudad.Provincia.Pais = await db.Pais.SingleOrDefaultAsync(c => c.IdPais == maestroArticuloSucursal.Sucursal.Ciudad.Provincia.IdPais);

                if (maestroArticuloSucursal == null)
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
                    Resultado = maestroArticuloSucursal,
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

        // PUT: api/MaestroArticuloSucursal/5
        [HttpPut("{id}")]
        public async Task<Response> PutMaestroArticuloSucursal([FromRoute] int id, [FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
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

                var maestroArticuloSucursalActualizar = await db.MaestroArticuloSucursal.Where(x => x.IdMaestroArticuloSucursal == id).FirstOrDefaultAsync();
                if (maestroArticuloSucursalActualizar != null)
                {
                    try
                    {
                        maestroArticuloSucursalActualizar.Minimo = maestroArticuloSucursal.Minimo;
                        maestroArticuloSucursalActualizar.Maximo = maestroArticuloSucursal.Maximo;
                        maestroArticuloSucursalActualizar.IdSucursal = maestroArticuloSucursal.IdSucursal;
                        db.MaestroArticuloSucursal.Update(maestroArticuloSucursalActualizar);
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

        // POST: api/MaestroArticuloSucursal
        [HttpPost]
        [Route("InsertarMaestroArticuloSucursal")]
        public async Task<Response> PostMaestroArticuloSucursal([FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
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

                var respuesta = Existe(maestroArticuloSucursal);
                if (!respuesta.IsSuccess)
                {
                    db.Entry(maestroArticuloSucursal.Sucursal).State = EntityState.Unchanged;
                    db.MaestroArticuloSucursal.Add(maestroArticuloSucursal);
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

        // DELETE: api/MaestroArticuloSucursal/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteMaestroArticuloSucursal([FromRoute] int id)
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

                var respuesta = await db.MaestroArticuloSucursal.SingleOrDefaultAsync(m => m.IdMaestroArticuloSucursal == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.MaestroArticuloSucursal.Remove(respuesta);
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

        public Response Existe(MaestroArticuloSucursal maestroArticuloSucursal)
        {
            var MaestroArticuloSucursalRespuesta = db.MaestroArticuloSucursal.Where(p => p.Minimo == maestroArticuloSucursal.Minimo && p.Maximo == maestroArticuloSucursal.Maximo && p.IdSucursal == maestroArticuloSucursal.IdSucursal).FirstOrDefault();
            if (MaestroArticuloSucursalRespuesta != null)
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
                Resultado = MaestroArticuloSucursalRespuesta,
            };
        }
    }
}