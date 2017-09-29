using bd.swrm.entidades.Negocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace bd.swrm.datos
{
    public class SwRMDbContext : DbContext
    {

        public SwRMDbContext(DbContextOptions<SwRMDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<ActivoFijo> ActivoFijo { get; set; }
        public virtual DbSet<Ciudad> Ciudad { get; set; }
        public virtual DbSet<ClaseActivoFijo> ClaseActivoFijo { get; set; }
        public virtual DbSet<ClaseArticulo> ClaseArticulo { get; set; }
        public virtual DbSet<CodigoActivoFijo> CodigoActivoFijo { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<DepreciacionActivoFijo> DepreciacionActivoFijo { get; set; }
        public virtual DbSet<ConfiguracionContabilidad> ConfiguracionContabilidad { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }
        public virtual DbSet<EmpleadoActivoFijo> EmpleadoActivoFijo { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<FondoFinanciamiento> FondoFinanciamiento { get; set; }
        public virtual DbSet<LibroActivoFijo> LibroActivoFijo { get; set; }
        public virtual DbSet<CatalogoCuenta> CatalogoCuenta { get; set; }
        public virtual DbSet<MotivoAsiento> MotivoAsiento { get; set; }
        public virtual DbSet<MaestroArticuloSucursal> MaestroArticuloSucursal { get; set; }
        public virtual DbSet<MaestroDetalleArticulo> MaestroDetalleArticulo { get; set; }
        public virtual DbSet<MantenimientoActivoFijo> MantenimientoActivoFijo { get; set; }
        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<Modelo> Modelo { get; set; }
        public virtual DbSet<MotivoRecepcion> MotivoRecepcion { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parroquia> Parroquia { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Provincia> Provincia { get; set; }
        public virtual DbSet<RecepcionActivoFijo> RecepcionActivoFijo { get; set; }
        public virtual DbSet<RecepcionActivoFijoDetalle> RecepcionActivoFijoDetalle { get; set; }
        public virtual DbSet<RecepcionArticulos> RecepcionArticulos { get; set; }
        public virtual DbSet<SolicitudProveduria> SolicitudProveduria { get; set; }
        public virtual DbSet<SolicitudProveduriaDetalle> SolicitudProveduriaDetalle { get; set; }
        public virtual DbSet<SubClaseActivoFijo> SubClaseActivoFijo { get; set; }
        public virtual DbSet<SubClaseArticulo> SubClaseArticulo { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<TablaDepreciacion> TablaDepreciacion { get; set; }
        public virtual DbSet<TipoActivoFijo> TipoActivoFijo { get; set; }
        public virtual DbSet<TipoArticulo> TipoArticulo { get; set; }
        public virtual DbSet<TranferenciaArticulo> TranferenciaArticulo { get; set; }
        public virtual DbSet<TransferenciaActivoFijo> TransferenciaActivoFijo { get; set; }
        public virtual DbSet<TransferenciaActivoFijoDetalle> TransferenciaActivoFijoDetalle { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<ActivoFijoMotivoBaja> ActivoFijoMotivoBaja { get; set; }
        public virtual DbSet<ActivosFijosBaja> ActivosFijosBaja { get; set; }
        public virtual DbSet<ActivosFijosAlta> ActivosFijosAlta { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijo)
                    .HasName("PK_ActivoFijo");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("IX_ActivoFijo_IdCiudad");

                entity.HasIndex(e => e.IdCodigoActivoFijo)
                    .HasName("IX_ActivoFijo_IdCodigoActivoFijo");

                entity.HasIndex(e => e.IdLibroActivoFijo)
                    .HasName("IX_ActivoFijo_IdLibroActivoFijo");

                entity.HasIndex(e => e.IdModelo)
                    .HasName("IX_ActivoFijo_IdModelo");

                entity.HasIndex(e => e.IdSubClaseActivoFijo)
                    .HasName("IX_ActivoFijo_IdSubClaseActivoFijo");

                entity.HasIndex(e => e.IdUnidadMedida)
                    .HasName("IX_ActivoFijo_IdUnidadMedida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Serie)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ValorCompra).HasColumnType("decimal");

                entity.HasOne(d => d.Ciudad)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CodigoActivoFijo)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdCodigoActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.LibroActivoFijo)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdLibroActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Modelo)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SubClaseActivoFijo)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdSubClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.UnidadMedida)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdUnidadMedida)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConfiguracionContabilidad>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracionContabilidad)
                    .HasName("PK162");

                entity.HasIndex(e => e.IdCatalogoCuentaD)
                    .HasName("Ref310467");

                entity.HasIndex(e => e.IdCatalogoCuentaH)
                    .HasName("Ref310468");

                entity.Property(e => e.IdConfiguracionContabilidad).HasColumnName("idConfiguracionContabilidad");

                entity.Property(e => e.IdCatalogoCuentaD).HasColumnName("idCatalogoCuentaD");

                entity.Property(e => e.IdCatalogoCuentaH).HasColumnName("idCatalogoCuentaH");

                entity.Property(e => e.ValorD).HasColumnType("decimal");

                entity.Property(e => e.ValorH).HasColumnType("decimal");

                entity.HasOne(d => d.IdCatalogoCuentaDNavigation)
                    .WithMany(p => p.ConfiguracionContabilidadIdCatalogoCuentaDNavigation)
                    .HasForeignKey(d => d.IdCatalogoCuentaD)
                    .HasConstraintName("RefCatalogoCuenta467");

                entity.HasOne(d => d.IdCatalogoCuentaHNavigation)
                    .WithMany(p => p.ConfiguracionContabilidadIdCatalogoCuentaHNavigation)
                    .HasForeignKey(d => d.IdCatalogoCuentaH)
                    .HasConstraintName("RefCatalogoCuenta468");
            });


            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK_Articulo");

                entity.HasIndex(e => e.IdModelo)
                    .HasName("IX_Articulo_IdModelo");

                entity.HasIndex(e => e.IdSubClaseArticulo)
                    .HasName("IX_Articulo_IdSubClaseArticulo");

                entity.HasIndex(e => e.IdUnidadMedida)
                    .HasName("IX_Articulo_IdUnidadMedida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Modelo)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdModelo);

                entity.HasOne(d => d.SubClaseArticulo)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdSubClaseArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.UnidadMedida)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdUnidadMedida);
            });



            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.HasKey(e => e.IdCiudad)
                    .HasName("PK_Ciudad");

                entity.HasIndex(e => e.IdProvincia)
                    .HasName("IX_Ciudad_IdProvincia");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.Ciudad)
                    .HasForeignKey(d => d.IdProvincia)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MotivoAsiento>(entity =>
            {
                entity.HasKey(e => e.IdMotivoAsiento)
                    .HasName("PK165");

                entity.HasIndex(e => e.IdConfiguracionContabilidad)
                    .HasName("Ref309469");

                entity.Property(e => e.IdMotivoAsiento).HasColumnName("idMotivoAsiento");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IdConfiguracionContabilidad).HasColumnName("idConfiguracionContabilidad");

                entity.HasOne(d => d.IdConfiguracionContabilidadNavigation)
                    .WithMany(p => p.MotivoAsiento)
                    .HasForeignKey(d => d.IdConfiguracionContabilidad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefConfiguracionContabilidad469");
            });

            modelBuilder.Entity<ClaseActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdClaseActivoFijo)
                    .HasName("PK_ClaseActivoFijo");

                entity.HasIndex(e => e.IdTablaDepreciacion)
                    .HasName("IX_ClaseActivoFijo_IdTablaDepreciacion");

                entity.HasIndex(e => e.IdTipoActivoFijo)
                    .HasName("IX_ClaseActivoFijo_IdTipoActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.TablaDepreciacion)
                    .WithMany(p => p.ClaseActivoFijo)
                    .HasForeignKey(d => d.IdTablaDepreciacion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoActivoFijo)
                    .WithMany(p => p.ClaseActivoFijo)
                    .HasForeignKey(d => d.IdTipoActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CatalogoCuenta>(entity =>
            {
                entity.HasKey(e => e.IdCatalogoCuenta)
                    .HasName("PK163");

                entity.HasIndex(e => e.IdCatalogoCuentaHijo)
                    .HasName("Ref310466");

                entity.Property(e => e.IdCatalogoCuenta).HasColumnName("idCatalogoCuenta");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnType("char(10)");

                entity.Property(e => e.IdCatalogoCuentaHijo).HasColumnName("idCatalogoCuentaHijo");

                entity.HasOne(d => d.IdCatalogoCuentaHijoNavigation)
                    .WithMany(p => p.InverseIdCatalogoCuentaHijoNavigation)
                    .HasForeignKey(d => d.IdCatalogoCuentaHijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCatalogoCuenta466");
            });

            modelBuilder.Entity<ClaseArticulo>(entity =>
            {
                entity.HasKey(e => e.IdClaseArticulo)
                    .HasName("PK_ClaseArticulo");

                entity.HasIndex(e => e.IdTipoArticulo)
                    .HasName("IX_ClaseArticulo_IdTipoArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.TipoArticulo)
                    .WithMany(p => p.ClaseArticulo)
                    .HasForeignKey(d => d.IdTipoArticulo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CodigoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdCodigoActivoFijo)
                    .HasName("PK_CodigoActivoFijo");

                entity.Property(e => e.CodigoBarras)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Codigosecuencial)
                    .IsRequired()
                    .HasMaxLength(20);
            });


            modelBuilder.Entity<DepreciacionActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdDepreciacionActivoFijo)
                    .HasName("PK_DepreciacionActivoFijo");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("IX_DepreciacionActivoFijo_IdActivoFijo");

                entity.Property(e => e.DepreciacionAcumulada).HasColumnType("decimal");

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.DepreciacionActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.HasKey(e => e.IdDetalleFactura)
                    .HasName("PK_DetalleFactura");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_DetalleFactura_IdArticulo");

                entity.HasIndex(e => e.IdFactura)
                    .HasName("IX_DetalleFactura_IdFactura");

                entity.Property(e => e.Precio).HasColumnType("decimal");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.DetalleFactura)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.DetalleFactura)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado)
                    .HasName("PK_Empleado");

                entity.HasIndex(e => e.IdCiudadLugarNacimiento)
                    .HasName("IX_Empleado_CiudadNacimientoIdCiudad");

              

                entity.HasIndex(e => e.IdProvinciaLugarSufragio)
                    .HasName("IX_Empleado_ProvinciaSufragioIdProvincia");

                entity.Property(e => e.IngresosOtraActividad)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.CiudadNacimiento)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdCiudadLugarNacimiento);
                
                entity.HasOne(d => d.ProvinciaSufragio)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdProvinciaLugarSufragio);
            });

            modelBuilder.Entity<EmpleadoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoActivoFijo)
                    .HasName("PK_EmpleadoActivoFijo");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("IX_EmpleadoActivoFijo_IdActivoFijo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoActivoFijo_IdEmpleado");

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.EmpleadoActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

                
            });




            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.IdFactura)
                    .HasName("PK_Factura");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_Factura_IdMaestroArticuloSucursal");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("IX_Factura_IdProveedor");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.MaestroArticuloSucursal)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Proveedor)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<FondoFinanciamiento>(entity =>
            {
                entity.HasKey(e => e.IdFondoFinanciamiento)
                    .HasName("PK_FondoFinanciamiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LibroActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdLibroActivoFijo)
                    .HasName("PK_LibroActivoFijo");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("IX_LibroActivoFijo_IdSucursal");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.LibroActivoFijo)
                    .HasForeignKey(d => d.IdSucursal);
            });

            modelBuilder.Entity<MaestroArticuloSucursal>(entity =>
            {
                entity.HasKey(e => e.IdMaestroArticuloSucursal)
                    .HasName("PK_MaestroArticuloSucursal");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("IX_MaestroArticuloSucursal_IdSucursal");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.MaestroArticuloSucursal)
                    .HasForeignKey(d => d.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MaestroDetalleArticulo>(entity =>
            {
                entity.HasKey(e => e.IdMaestroDetalleArticulo)
                    .HasName("PK_MaestroDetalleArticulo");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_MaestroDetalleArticulo_IdArticulo");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_MaestroDetalleArticulo_IdMaestroArticuloSucursal");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.MaestroDetalleArticulo)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.MaestroArticuloSucursal)
                    .WithMany(p => p.MaestroDetalleArticulo)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MantenimientoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdMantenimientoActivoFijo)
                    .HasName("PK_MantenimientoActivoFijo");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("IX_MantenimientoActivoFijo_IdActivoFijo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_MantenimientoActivoFijo_IdEmpleado");

                entity.Property(e => e.Observaciones)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.MantenimientoActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK_Marca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.HasKey(e => e.IdModelo)
                    .HasName("PK_Modelo");

                entity.HasIndex(e => e.IdMarca)
                    .HasName("IX_Modelo_IdMarca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Modelo)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<MotivoRecepcion>(entity =>
            {
                entity.HasKey(e => e.IdMotivoRecepcion)
                    .HasName("PK_MotivoRecepcion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });



            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais)
                    .HasName("PK_Pais");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<Parroquia>(entity =>
            {
                entity.HasKey(e => e.IdParroquia)
                    .HasName("PK_Parroquia");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("IX_Parroquia_IdCiudad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Ciudad)
                    .WithMany(p => p.Parroquia)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor)
                    .HasName("PK_Proveedor");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.IdProvincia)
                    .HasName("PK_Provincia");

                entity.HasIndex(e => e.IdPais)
                    .HasName("IX_Provincia_IdPais");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Pais)
                    .WithMany(p => p.Provincia)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<RecepcionActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionActivoFijo)
                    .HasName("PK_RecepcionActivoFijo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RecepcionActivoFijo_IdEmpleado");

                entity.HasIndex(e => e.IdLibroActivoFijo)
                    .HasName("IX_RecepcionActivoFijo_IdLibroActivoFijo");

                entity.HasIndex(e => e.IdMotivoRecepcion)
                    .HasName("IX_RecepcionActivoFijo_IdMotivoRecepcion");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("IX_RecepcionActivoFijo_IdProveedor");

                entity.HasIndex(e => e.IdSubClaseActivoFijo)
                    .HasName("IX_RecepcionActivoFijo_IdSubClaseActivoFijo");

                entity.Property(e => e.Cantidad).HasColumnType("decimal");

                entity.Property(e => e.Fondo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OrdenCompra)
                    .IsRequired()
                    .HasMaxLength(100);

               
                entity.HasOne(d => d.LibroActivoFijo)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdLibroActivoFijo);

                entity.HasOne(d => d.MotivoRecepcion)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdMotivoRecepcion);

                entity.HasOne(d => d.Proveedor)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SubClaseActivoFijo)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdSubClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RecepcionActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionActivoFijoDetalle)
                    .HasName("PK_RecepcionActivoFijoDetalle");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("IX_RecepcionActivoFijoDetalle_IdActivoFijo");

               
                entity.HasIndex(e => e.IdRecepcionActivoFijo)
                    .HasName("IX_RecepcionActivoFijoDetalle_IdRecepcionActivoFijo");

                entity.Property(e => e.NumeroPoliza)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

               
                entity.HasOne(d => d.RecepcionActivoFijo)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdRecepcionActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RecepcionArticulos>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionArticulos)
                    .HasName("PK_RecepcionArticulos");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_RecepcionArticulos_IdArticulo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RecepcionArticulos_IdEmpleado");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_RecepcionArticulos_IdMaestroArticuloSucursal");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("IX_RecepcionArticulos_IdProveedor");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

               
                entity.HasOne(d => d.MaestroArticuloSucursal)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Proveedor)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<SolicitudProveduria>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduria)
                    .HasName("PK_SolicitudProveduria");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudProveduria_IdEmpleado");

               
            });

            modelBuilder.Entity<SolicitudProveduriaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduriaDetalle)
                    .HasName("PK_SolicitudProveduriaDetalle");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_SolicitudProveduriaDetalle_IdArticulo");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_SolicitudProveduriaDetalle_IdMaestroArticuloSucursal");

                entity.HasIndex(e => e.IdSolicitudProveduria)
                    .HasName("IX_SolicitudProveduriaDetalle_IdSolicitudProveduria");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

              

                entity.HasOne(d => d.MaestroArticuloSucursal)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SolicitudProveduria)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdSolicitudProveduria)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<SubClaseActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdSubClaseActivoFijo)
                    .HasName("PK_SubClaseActivoFijo");

                entity.HasIndex(e => e.IdClaseActivoFijo)
                    .HasName("IX_SubClaseActivoFijo_IdClaseActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.ClaseActivoFijo)
                    .WithMany(p => p.SubClaseActivoFijo)
                    .HasForeignKey(d => d.IdClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SubClaseArticulo>(entity =>
            {
                entity.HasKey(e => e.IdSubClaseArticulo)
                    .HasName("PK_SubClaseArticulo");

                entity.HasIndex(e => e.IdClaseArticulo)
                    .HasName("IX_SubClaseArticulo_IdClaseArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.ClaseArticulo)
                    .WithMany(p => p.SubClaseArticulo)
                    .HasForeignKey(d => d.IdClaseArticulo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasKey(e => e.IdSucursal)
                    .HasName("PK_Sucursal");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("IX_Sucursal_IdCiudad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Ciudad)
                    .WithMany(p => p.Sucursal)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TablaDepreciacion>(entity =>
            {
                entity.HasKey(e => e.IdTablaDepreciacion)
                    .HasName("PK_TablaDepreciacion");

                entity.Property(e => e.IndiceDepreciacion).HasColumnType("decimal");
            });


            modelBuilder.Entity<TipoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdTipoActivoFijo)
                    .HasName("PK_TipoActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoArticulo>(entity =>
            {
                entity.HasKey(e => e.IdTipoArticulo)
                    .HasName("PK_TipoArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });


            modelBuilder.Entity<TranferenciaArticulo>(entity =>
            {
                entity.HasKey(e => e.IdTranferenciaArticulo)
                    .HasName("PK_TranferenciaArticulo");

                entity.HasIndex(e => e.IdEmpleadoEnvia)
                    .HasName("IX_TranferenciaArticulo_EmpleadoIdIdEmpleadoEnvia");

                entity.HasIndex(e => e.IdEmpleadoRecibe)
                    .HasName("IX_TranferenciaArticulo_EmpleadoId");


                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_TranferenciaArticulo_IdArticulo");

                entity.HasIndex(e => e.IdMaestroArticuloEnvia)
                    .HasName("IX_TranferenciaArticulo_MaestroArticuloSucursalIdMaestroArticuloEnvia");

                entity.HasIndex(e => e.IdMaestroArticuloRecibe)
                   .HasName("IX_TranferenciaArticulo_MaestroArticuloSucursalIdMaestroArticuloRecibe");

                entity.Property(e => e.Fecha).HasMaxLength(10);


                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.TranferenciaArticulo)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.MaestroArticuloSucursalRecibe)
                    .WithMany(p => p.TranferenciaArticulo)
                    .HasForeignKey(d => d.IdMaestroArticuloRecibe);

                entity.HasOne(d => d.MaestroArticuloSucursalEnvia)
                  .WithMany(p => p.TranferenciaArticulo1)
                  .HasForeignKey(d => d.IdMaestroArticuloEnvia);
            });

            modelBuilder.Entity<TransferenciaActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdTransferenciaActivoFijo)
                    .HasName("PK_TransferenciaActivoFijo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_TransferenciaActivoFijo_IdEmpleado");

                entity.Property(e => e.Destino)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Origen).HasMaxLength(50);
            });

            modelBuilder.Entity<TransferenciaActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdTransferenciaActivoFijoDetalle)
                    .HasName("PK_TransferenciaActivoFijoDetalle");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("IX_TransferenciaActivoFijoDetalle_IdActivoFijo");

                entity.HasIndex(e => e.IdTransferenciaActivoFijo)
                    .HasName("IX_TransferenciaActivoFijoDetalle_IdTransferenciaActivoFijo");

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.TransferenciaActivoFijoDetalle)
                    .HasForeignKey(d => d.IdActivoFijo);

                entity.HasOne(d => d.TransferenciaActivoFijo)
                    .WithMany(p => p.TransferenciaActivoFijoDetalle)
                    .HasForeignKey(d => d.IdTransferenciaActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK_Persona");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CorreoPrivado).IsRequired();

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LugarTrabajo)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TelefonoCasa)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TelefonoPrivado)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK_Estado");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });


            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.HasKey(e => e.IdUnidadMedida)
                    .HasName("PK_UnidadMedida");
            });

            modelBuilder.Entity<ActivoFijoMotivoBaja>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijoMotivoBaja)
                    .HasName("PK_ActivoFijoMotivoBaja");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ActivosFijosBaja>(entity =>
            {
                entity.HasKey(e => e.IdBaja)
                    .HasName("PK_ActivosFijosBaja");

                entity.Property(e => e.FechaBaja)
                    .IsRequired();

                entity.HasOne(d => d.ActivoFijoMotivoBaja)
                    .WithMany(p => p.ActivosFijosBaja)
                    .HasForeignKey(d => d.IdMotivoBaja)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ActivoFijo)
                    .WithOne(b => b.ActivosFijosBaja)
                    .HasForeignKey<ActivosFijosBaja>(b => b.IdActivo);

            });

            modelBuilder.Entity<ActivosFijosAlta>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijo)
                    .HasName("PK_ActivoFijo");

                entity.Property(e => e.FechaAlta)
                    .IsRequired();

                entity.HasOne(a => a.ActivoFijo)
                    .WithOne(b => b.ActivosFijosAlta)
                    .HasForeignKey<ActivosFijosAlta>(b => b.IdActivoFijo);
                
             });

           


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{

        //    foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    base.OnModelCreating(builder);

        //    // Customize the ASP.NET Identity model and override the defaults if needed.
        //    // For example, you can rename the ASP.NET Identity table names and more.
        //    // Add your customizations after calling base.OnModelCreating(builder);
        //}


        public void EnsureSeedData()
        {

        }

      
    }

}






