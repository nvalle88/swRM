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
using Microsoft.EntityFrameworkCore;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/DetalleFactura")]
    public class DetalleFacturaController : Controller
    {
        private readonly SwRMDbContext db;

        public DetalleFacturaController(SwRMDbContext db)
        {
            this.db = db;
        }

        // GET: api/ListarDetallesFactura
        [HttpGet]
        [Route("ListarDetallesFactura")]
        public async Task<List<DetalleFactura>> GetDetalleFactura()
        {
            try
            {
                return await db.DetalleFactura.OrderBy(x => x.IdDetalleFactura).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<DetalleFactura>();
            }
        }

        // GET: api/DetalleFactura/5
        [HttpGet("{id}")]
        public async Task<Response> GetDetalleFactura([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido",
                    };
                }

                var detalleFactura = await db.DetalleFactura.SingleOrDefaultAsync(m => m.IdDetalleFactura == id);

                if (detalleFactura == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No encontrado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Resultado = detalleFactura,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // PUT: api/DetalleFactura/5
        [HttpPut("{id}")]
        public async Task<Response> PutDetalleFactura([FromRoute] int id, [FromBody] DetalleFactura detallefactura)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var detallefacturaActualizar = await db.DetalleFactura.Where(x => x.IdDetalleFactura == id).FirstOrDefaultAsync();
                if (detallefacturaActualizar != null)
                {
                    try
                    {
                        detallefacturaActualizar.Precio = detallefactura.Precio;
                        detallefacturaActualizar.Cantidad = detallefactura.Cantidad;
                        detallefacturaActualizar.IdFactura = detallefactura.IdFactura;
                        detallefacturaActualizar.IdArticulo = detallefactura.IdArticulo;
                        db.DetalleFactura.Update(detallefacturaActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwRm),
                            ExceptionTrace = ex,
                            Message = "Se ha producido una exepci�n",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Error ",
                        };
                    }
                }




                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe"
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepci�n"
                };
            }
        }

        // POST: api/DetalleFactura
        [HttpPost]
        [Route("InsertarDetalleFactura")]
        public async Task<Response> PostDetalleFactura([FromBody] DetalleFactura detalleFactura)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var respuesta = Existe(detalleFactura);
                if (!respuesta.IsSuccess)
                {
                    db.DetalleFactura.Add(detalleFactura);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "OK"
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "OK"
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // DELETE: api/DetalleFactura/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteDetalleFactura([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido ",
                    };
                }

                var respuesta = await db.DetalleFactura.SingleOrDefaultAsync(m => m.IdDetalleFactura == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.DetalleFactura.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = "Eliminado ",
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwRm),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        private bool DetalleFacturaExists(int idArticulo, int idFactura)
        {
            return db.DetalleFactura.Any(e => e.IdArticulo == idArticulo && e.IdFactura == idFactura);
        }

        public Response Existe(DetalleFactura detalleFactura)
        {
            var loglevelrespuesta = db.DetalleFactura.Where(p => p.IdFactura == detalleFactura.IdFactura && p.IdArticulo == detalleFactura.IdArticulo).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = String.Format("Existe un detalle de factura para el art�culo {0} y la factura {1}", detalleFactura?.Articulo?.Nombre ?? "<Sin nombre>", detalleFactura?.Factura?.Numero ?? "<Sin nombre>"),
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
