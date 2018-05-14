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
    [Route("api/Proveedor")]
    public class ProveedorController : Controller
    {
        private readonly SwRMDbContext db;

        public ProveedorController(SwRMDbContext db)
        {
            this.db = db;
        }
        
        [HttpGet]
        [Route("ListarProveedores")]
        public async Task<List<Proveedor>> GetProveedor()
        {
            try
            {
                return await db.Proveedor.OrderBy(x => x.Nombre).Include(x => x.Factura).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new List<Proveedor>();
            }
        }
        
        [HttpGet("{id}")]
        public async Task<Response> GetProveedor([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var proveedor = await db.Proveedor.Include(x => x.Factura).SingleOrDefaultAsync(m => m.IdProveedor == id);
                return new Response { IsSuccess = proveedor != null, Message = proveedor != null ? Mensaje.Satisfactorio : Mensaje.RegistroNoEncontrado, Resultado = proveedor };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }
        
        [HttpPut("{id}")]
        public async Task<Response> PutProveedor([FromRoute] int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Proveedor.Where(c => c.Codigo.ToUpper().Trim() == proveedor.Codigo.ToUpper().Trim()).AnyAsync(c => c.IdProveedor != proveedor.IdProveedor))
                {
                    var ProveedorActualizar = await db.Proveedor.Where(x => x.IdProveedor == id).FirstOrDefaultAsync();
                    if (ProveedorActualizar != null)
                    {
                        try
                        {
                            ProveedorActualizar.Nombre = proveedor.Nombre;
                            ProveedorActualizar.Apellidos = proveedor.Apellidos;
                            ProveedorActualizar.Identificacion = proveedor.Identificacion;
                            ProveedorActualizar.Direccion = proveedor.Direccion;
                            ProveedorActualizar.Codigo = proveedor.Codigo;
                            ProveedorActualizar.Activo = proveedor.Activo;
                            ProveedorActualizar.LineaServicio = proveedor.LineaServicio;
                            ProveedorActualizar.RazonSocial = proveedor.RazonSocial;
                            ProveedorActualizar.Telefono = proveedor.Telefono;
                            ProveedorActualizar.Email = proveedor.Email;
                            ProveedorActualizar.Cargo = proveedor.Cargo;
                            ProveedorActualizar.Observaciones = proveedor.Observaciones;
                            ProveedorActualizar.Emision = proveedor.Emision;
                            db.Proveedor.Update(ProveedorActualizar);
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
        
        [HttpPost]
        [Route("InsertarProveedor")]
        public async Task<Response> PostProveedor([FromBody] Proveedor proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                if (!await db.Proveedor.AnyAsync(c => c.Codigo.ToUpper().Trim() == proveedor.Codigo.ToUpper().Trim()))
                {
                    db.Proveedor.Add(proveedor);
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
        public async Task<Response> DeleteProveedor([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new Response { IsSuccess = false, Message = Mensaje.ModeloInvalido };

                var respuesta = await db.Proveedor.SingleOrDefaultAsync(m => m.IdProveedor == id);
                if (respuesta == null)
                    return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };

                db.Proveedor.Remove(respuesta);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, Message = Mensaje.Satisfactorio };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.SwRm), ExceptionTrace = ex.Message, Message = Mensaje.Excepcion, LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "" });
                return new Response { IsSuccess = false, Message = Mensaje.Error };
            }
        }

        public Response Existe(Proveedor Proveedor)
        {
            var bdd = Proveedor.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.Proveedor.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            return new Response { IsSuccess = loglevelrespuesta != null, Message = loglevelrespuesta != null ? Mensaje.ExisteRegistro : String.Empty, Resultado = loglevelrespuesta };
        }
    }
}
