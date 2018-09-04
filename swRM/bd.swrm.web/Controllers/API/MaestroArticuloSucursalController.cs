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
using bd.swrm.entidades.Comparadores;

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
        
        [HttpGet]
        [Route("ListarMaestroArticuloSucursal")]
        public async Task<List<MaestroArticuloSucursal>> GetMaestroArticuloSucursal()
        {
            try
            {
                var listadoMaestroArticuloSucursal = await db.MaestroArticuloSucursal
                    .Include(c => c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.Articulo)
                    .OrderBy(c=> c.IdSucursal)
                    .OrderBy(c=> c.Articulo)
                    .OrderBy(c => c.Minimo).ThenBy(c => c.Maximo).ToListAsync();

                foreach (var item in listadoMaestroArticuloSucursal)
                    item.ValorActual = await ObtenerValorActual(item.IdMaestroArticuloSucursal);
                return listadoMaestroArticuloSucursal;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MaestroArticuloSucursal>();
            }
        }

        [HttpGet]
        [Route("ListarMaestroArticuloSucursalPorSucursal/{idSucursal}")]
        public async Task<List<MaestroArticuloSucursal>> GetMaestroArticuloSucursalPorSucursal(int idSucursal)
        {
            try
            {
                var listadoMaestroArticuloSucursal = await db.MaestroArticuloSucursal.Where(c=> c.IdSucursal == idSucursal)
                    .Include(c => c.Sucursal).ThenInclude(c => c.Ciudad).ThenInclude(c => c.Provincia).ThenInclude(c => c.Pais)
                    .Include(c=> c.Articulo)
                    .OrderBy(c => c.IdSucursal)
                    .OrderBy(c => c.Articulo)
                    .OrderBy(c => c.Minimo).ThenBy(c => c.Maximo).ToListAsync();
                
                foreach (var item in listadoMaestroArticuloSucursal)
                    item.ValorActual = await ObtenerValorActual(item.IdMaestroArticuloSucursal);
                return listadoMaestroArticuloSucursal;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<MaestroArticuloSucursal>();
            }
        }

        public async Task<decimal> ObtenerValorActual(int idMaestroArticuloSucursal)
        {
            try
            {
                var listaValorUnitariosOrdenCompraDetalles = await db.OrdenCompraDetalles.Where(c => c.IdMaestroArticuloSucursal == idMaestroArticuloSucursal).ToListAsync();
                return listaValorUnitariosOrdenCompraDetalles.Count > 0 ? listaValorUnitariosOrdenCompraDetalles.Average(c=> c.ValorUnitario) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpGet("{id}")]
        public async Task<Response> GetMaestroArticuloSucursal([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var maestroArticuloSucursal = await db.MaestroArticuloSucursal
                    .Include(c=> c.Sucursal).ThenInclude(c=> c.Ciudad).ThenInclude(c=> c.Provincia).ThenInclude(c=> c.Pais)
                    .Include(c=> c.Articulo)
                    .SingleOrDefaultAsync(m => m.IdMaestroArticuloSucursal == id);

                if (maestroArticuloSucursal != null)
                    maestroArticuloSucursal.ValorActual = await ObtenerValorActual(maestroArticuloSucursal.IdMaestroArticuloSucursal);

                return new Response { IsSuccess = maestroArticuloSucursal != null, Message = maestroArticuloSucursal != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = maestroArticuloSucursal };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        [HttpPost]
        [Route("ObtenerGrupoArticulo")]
        public async Task<Response> PostGrupoArticulo([FromBody] int idArticulo)
        {
            try
            {
                var articulo = await db.Articulo.Include(c=> c.SubClaseArticulo).ThenInclude(c=> c.ClaseArticulo).FirstOrDefaultAsync(c => c.IdArticulo == idArticulo);
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio, Resultado = PutCeros(articulo.SubClaseArticulo.ClaseArticulo.IdClaseArticulo.ToString(), 3) };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        private string PutCeros(string texto, int cantidadCeros)
        {
            int lengthTexto = texto.Length;
            while (lengthTexto < cantidadCeros)
            {
                texto = "0" + texto;
                lengthTexto++;
            }
            return texto;
        }

        [HttpPut("{id}")]
        public async Task<Response> PutMaestroArticuloSucursal([FromRoute] int id, [FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MaestroArticuloSucursal.Where(p => (p.IdSucursal == maestroArticuloSucursal.IdSucursal && p.IdArticulo == maestroArticuloSucursal.IdArticulo) || p.CodigoArticulo == maestroArticuloSucursal.CodigoArticulo).AnyAsync(c => c.IdMaestroArticuloSucursal != maestroArticuloSucursal.IdMaestroArticuloSucursal))
                {
                    var maestroArticuloSucursalActualizar = await db.MaestroArticuloSucursal.Where(x => x.IdMaestroArticuloSucursal == id).FirstOrDefaultAsync();
                    if (maestroArticuloSucursalActualizar != null)
                    {
                        try
                        {
                            maestroArticuloSucursalActualizar.Minimo = maestroArticuloSucursal.Minimo;
                            maestroArticuloSucursalActualizar.Maximo = maestroArticuloSucursal.Maximo;
                            maestroArticuloSucursalActualizar.IdSucursal = maestroArticuloSucursal.IdSucursal;
                            maestroArticuloSucursalActualizar.IdArticulo = maestroArticuloSucursal.IdArticulo;
                            maestroArticuloSucursalActualizar.CodigoArticulo = $"{PutCeros(maestroArticuloSucursal.GrupoArticulo, 3)}.{PutCeros(maestroArticuloSucursal.CodigoArticulo, 5)}";

                            if (!maestroArticuloSucursalActualizar.Habilitado && maestroArticuloSucursal.Habilitado)
                                maestroArticuloSucursalActualizar.FechaSinExistencia = DateTime.Now;

                            if (maestroArticuloSucursalActualizar.Habilitado && !maestroArticuloSucursal.Habilitado)
                                maestroArticuloSucursalActualizar.FechaSinExistencia = null;

                            maestroArticuloSucursalActualizar.Habilitado = maestroArticuloSucursal.Habilitado;

                            db.MaestroArticuloSucursal.Update(maestroArticuloSucursalActualizar);
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

        [HttpPut("EditarFechaExistenciaHabilitado/{id}")]
        public async Task<Response> PutFechaSinExistenciaHabilitado([FromRoute] int id, [FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
        {
            try
            {
                var maestroArticuloSucursalActualizar = await db.MaestroArticuloSucursal.Where(x => x.IdMaestroArticuloSucursal == id).FirstOrDefaultAsync();
                if (maestroArticuloSucursalActualizar != null)
                {
                    maestroArticuloSucursalActualizar.Habilitado = maestroArticuloSucursal.Habilitado;
                    maestroArticuloSucursalActualizar.FechaSinExistencia = maestroArticuloSucursal.FechaSinExistencia;
                }
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpPut("EditarValorActual/{id}")]
        public async Task<Response> PutValorActual([FromRoute] int id, [FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
        {
            try
            {
                var maestroArticuloSucursalActualizar = await db.MaestroArticuloSucursal.Where(x => x.IdMaestroArticuloSucursal == id).FirstOrDefaultAsync();
                if (maestroArticuloSucursalActualizar != null)
                    maestroArticuloSucursalActualizar.ValorActual = maestroArticuloSucursal.ValorActual;

                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Mensaje.Excepcion };
            }
        }

        [HttpPost]
        [Route("InsertarMaestroArticuloSucursal")]
        public async Task<Response> PostMaestroArticuloSucursal([FromBody] MaestroArticuloSucursal maestroArticuloSucursal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.MaestroArticuloSucursal.AnyAsync(p => (p.IdSucursal == maestroArticuloSucursal.IdSucursal && p.IdArticulo == maestroArticuloSucursal.IdArticulo) || p.CodigoArticulo == maestroArticuloSucursal.CodigoArticulo))
                {
                    maestroArticuloSucursal.CodigoArticulo = $"{PutCeros(maestroArticuloSucursal.GrupoArticulo, 3)}.{PutCeros(maestroArticuloSucursal.CodigoArticulo, 5)}";

                    if (maestroArticuloSucursal.Habilitado)
                        maestroArticuloSucursal.FechaSinExistencia = DateTime.Now;

                    db.MaestroArticuloSucursal.Add(maestroArticuloSucursal);
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
        
        [HttpDelete("{id}")]
        public async Task<Response> DeleteMaestroArticuloSucursal([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.MaestroArticuloSucursal.SingleOrDefaultAsync(m => m.IdMaestroArticuloSucursal == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.MaestroArticuloSucursal.Remove(respuesta);
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
}