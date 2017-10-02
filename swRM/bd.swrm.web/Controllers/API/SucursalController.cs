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
    [Route("api/Sucursal")]
    public class SucursalController : Controller
    {
        private readonly SwRMDbContext db;

        public SucursalController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarSucursales
        [HttpGet]
        [Route("ListarSucursales")]
        public async Task<List<Sucursal>> GetSucursal()
        {
            try
            {
                return await db.Sucursal.OrderBy(x => x.Nombre).Include(x=> x.Ciudad).ThenInclude(x=> x.Provincia).ThenInclude(x=> x.Pais).ToListAsync();
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
                return new List<Sucursal>();
            }
        }

        // GET: api/Sucursal/5
        [HttpGet("{id}")]
        public async Task<Response> GetSucursal([FromRoute] int id)
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

                var sucursal = await db.Sucursal.SingleOrDefaultAsync(m => m.IdSucursal == id);
                sucursal.Ciudad = await db.Ciudad.SingleOrDefaultAsync(c => c.IdCiudad == sucursal.IdCiudad);
                sucursal.Ciudad.Provincia = await db.Provincia.SingleOrDefaultAsync(c => c.IdProvincia == sucursal.Ciudad.IdProvincia);
                sucursal.Ciudad.Provincia.Pais = await db.Pais.SingleOrDefaultAsync(c => c.IdPais == sucursal.Ciudad.Provincia.IdPais);

                if (sucursal == null)
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
                    Resultado = sucursal,
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

        // PUT: api/Sucursal/5
        [HttpPut("{id}")]
        public async Task<Response> PutSucursal([FromRoute] int id, [FromBody] Sucursal Sucursal)
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

                var SucursalActualizar = await db.Sucursal.Where(x => x.IdSucursal == id).FirstOrDefaultAsync();
                if (SucursalActualizar != null)
                {
                    try
                    {
                        SucursalActualizar.Nombre = Sucursal.Nombre;
                        SucursalActualizar.IdCiudad = Sucursal.IdCiudad;
                        db.Sucursal.Update(SucursalActualizar);
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

        // POST: api/Sucursal
        [HttpPost]
        [Route("InsertarSucursal")]
        public async Task<Response> PostSucursal([FromBody] Sucursal sucursal)
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

                var respuesta = Existe(sucursal);
                if (!respuesta.IsSuccess)
                {
                    db.Entry(sucursal.Ciudad).State = EntityState.Unchanged;
                    db.Sucursal.Add(sucursal);
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

        // DELETE: api/Sucursal/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSucursal([FromRoute] int id)
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

                var respuesta = await db.Sucursal.SingleOrDefaultAsync(m => m.IdSucursal == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.Sucursal.Remove(respuesta);
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

        public Response Existe(Sucursal Sucursal)
        {
            var bdd = Sucursal.Nombre.ToUpper().TrimEnd().TrimStart();
            var SucursalRespuesta = db.Sucursal.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd && p.IdCiudad == Sucursal.IdCiudad).FirstOrDefault();
            if (SucursalRespuesta != null)
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
                Resultado = SucursalRespuesta,
            };
        }
    }
}