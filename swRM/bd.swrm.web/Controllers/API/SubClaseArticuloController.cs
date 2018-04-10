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
    [Route("api/SubClaseArticulo")]
    public class SubClaseArticuloController : Controller
    {
        private readonly SwRMDbContext db;

        public SubClaseArticuloController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarSubClaseArticulos
        [HttpGet]
        [Route("ListarSubClaseArticulos")]
        public async Task<List<SubClaseArticulo>> GetSubClaseArticulo()
        {
            try
            {
                return await db.SubClaseArticulo.OrderBy(x => x.Nombre).Include(c=> c.ClaseArticulo).ThenInclude(c=> c.TipoArticulo).ToListAsync();
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
                return new List<SubClaseArticulo>();
            }
        }

        // GET: api/SubClaseArticulo/5
        [HttpGet("{id}")]
        public async Task<Response> GetSubClaseArticulo([FromRoute] int id)
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

                var subClaseArticulo = await db.SubClaseArticulo.SingleOrDefaultAsync(m => m.IdSubClaseArticulo == id);
                subClaseArticulo.ClaseArticulo = await db.ClaseArticulo.SingleOrDefaultAsync(c => c.IdClaseArticulo == subClaseArticulo.IdClaseArticulo);
                subClaseArticulo.ClaseArticulo.TipoArticulo = await db.TipoArticulo.SingleOrDefaultAsync(c => c.IdTipoArticulo == subClaseArticulo.ClaseArticulo.IdTipoArticulo);

                if (subClaseArticulo == null)
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
                    Resultado = subClaseArticulo,
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

        // PUT: api/SubClaseArticulo/5
        [HttpPut("{id}")]
        public async Task<Response> PutSubClaseArticulo([FromRoute] int id, [FromBody] SubClaseArticulo SubClaseArticulo)
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

                var SubClaseArticuloActualizar = await db.SubClaseArticulo.Where(x => x.IdSubClaseArticulo == id).FirstOrDefaultAsync();
                if (SubClaseArticuloActualizar != null)
                {
                    try
                    {
                        SubClaseArticuloActualizar.Nombre = SubClaseArticulo.Nombre;
                        db.SubClaseArticulo.Update(SubClaseArticuloActualizar);
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

        // POST: api/SubClaseArticulo
        [HttpPost]
        [Route("InsertarSubClaseArticulo")]
        public async Task<Response> PostSubClaseArticulo([FromBody] SubClaseArticulo SubClaseArticulo)
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

                var respuesta = Existe(SubClaseArticulo);
                if (!respuesta.IsSuccess)
                {
                    db.SubClaseArticulo.Add(SubClaseArticulo);
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

        // DELETE: api/SubClaseArticulo/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteSubClaseArticulo([FromRoute] int id)
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

                var respuesta = await db.SubClaseArticulo.SingleOrDefaultAsync(m => m.IdSubClaseArticulo == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.SubClaseArticulo.Remove(respuesta);
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

        private bool SubClaseArticuloExists(string nombre)
        {
            return db.SubClaseArticulo.Any(e => e.Nombre == nombre);
        }

        public Response Existe(SubClaseArticulo SubClaseArticulo)
        {
            var bdd = SubClaseArticulo.Nombre.ToUpper().TrimEnd().TrimStart();
            var SubClaseArticuloRespuesta = db.SubClaseArticulo.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            if (SubClaseArticuloRespuesta != null)
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
                Resultado = SubClaseArticuloRespuesta,
            };
        }
    }
}
