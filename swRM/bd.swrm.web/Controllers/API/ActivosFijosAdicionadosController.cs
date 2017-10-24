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

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ActivosFijosAdicionados")]
    public class ActivosFijosAdicionadosController : Controller
    {
        private readonly SwRMDbContext db;

        public ActivosFijosAdicionadosController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ActivosFijosAdicionados
        [HttpGet]
        [Route("ListarActivosFijosAdicionados")]
        public async Task<List<ActivosFijosAdicionados>> GetActivosFijosAdicionados()
        {
            try
            {
                return await db.ActivosFijosAdicionados.OrderBy(x => x.idAdicion).Include(x => x.ActivoFijo).ToListAsync();
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
                return new List<ActivosFijosAdicionados>();
            }
        }

        // GET: api/ActivosFijosAdicionados/5
        [HttpGet("{id}")]
        public async Task<Response> GetActivosFijosAdicionados([FromRoute]int id)
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

                var _ActivosFijosAdicionados = await db.ActivosFijosAdicionados.SingleOrDefaultAsync(m => m.idAdicion == id);

                if (_ActivosFijosAdicionados == null)
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
                    Resultado = _ActivosFijosAdicionados,
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

        // POST: api/ActivosFijosAdicionados
        [HttpPost]
        [Route("InsertarActivosFijosAdicionados")]
        public async Task<Response> PostMarca([FromBody]ActivosFijosAdicionados _ActivosFijosAdicionados)
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

                var respuesta = Existe(_ActivosFijosAdicionados);
                if (!respuesta.IsSuccess)
                {
                    db.ActivosFijosAdicionados.Add(_ActivosFijosAdicionados);
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

        // PUT: api/ActivosFijosAdicionados/5
        [HttpPut("{id}")]
        public async Task<Response> PutActivosFijosAdicionados([FromRoute] int id, [FromBody]ActivosFijosAdicionados _ActivosFijosAdicionados)
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

                var _ActivosFijosAdicionadosActualizar = await db.ActivosFijosAdicionados.Where(x => x.idAdicion == id).FirstOrDefaultAsync();
                if (_ActivosFijosAdicionadosActualizar != null)
                {
                    try
                    {
                        _ActivosFijosAdicionadosActualizar.idActivoFijoOrigen = _ActivosFijosAdicionados.idActivoFijoOrigen;
                        _ActivosFijosAdicionadosActualizar.idActivoFijoDestino = _ActivosFijosAdicionados.idActivoFijoDestino;
                        
                        db.ActivosFijosAdicionados.Update(_ActivosFijosAdicionadosActualizar);
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
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteActivosFijosAdicionados([FromRoute] int id)
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

                var respuesta = await db.ActivosFijosAdicionados.SingleOrDefaultAsync(m => m.idAdicion == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.ActivosFijosAdicionados.Remove(respuesta);
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

        private bool ActivosFijosAdicionadosExists(int id)
        {
            return db.ActivosFijosAdicionados.Any(e => e.idAdicion == id);
        }

        public Response Existe(ActivosFijosAdicionados _ActivosFijosAdicionados)
        {
            var bdd = _ActivosFijosAdicionados.idActivoFijoOrigen;
            var _bdd = _ActivosFijosAdicionados.idActivoFijoDestino;
            var loglevelrespuesta = db.ActivosFijosAdicionados.Where(p => p.idActivoFijoOrigen == bdd && p.idActivoFijoDestino == _bdd).FirstOrDefault();
            
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
