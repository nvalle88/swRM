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
        { }

        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<ActivoFijo> ActivoFijo { get; set; }
        public virtual DbSet<ActivoFijoComponentes> ActivoFijoComponentes { get; set; }
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
        public virtual DbSet<SolicitudProveeduria> SolicitudProveeduria { get; set; }
        public virtual DbSet<SolicitudProveeduriaDetalle> SolicitudProveeduriaDetalle { get; set; }
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
        public virtual DbSet<MotivoBaja> MotivoBaja { get; set; }
        public virtual DbSet<BajaActivoFijoDetalle> BajaActivoFijoDetalle { get; set; }
        public virtual DbSet<AltaActivoFijoDetalle> AltaActivoFijoDetalle { get; set; }
        public virtual DbSet<MotivoTransferencia> MotivoTransferencia { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<Etnia> Etnia { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Nacionalidad> Nacionalidad { get; set; }
        public virtual DbSet<Sexo> Sexo { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }
        public virtual DbSet<TipoSangre> TipoSangre { get; set; }
        public virtual DbSet<Dependencia> Dependencia { get; set; }
        public virtual DbSet<AltaProveeduria> AltaProveeduria { get; set; }
        public virtual DbSet<BajaProveeduria> BajaProveeduria { get; set; }
        public virtual DbSet<FacturasPorAltaProveeduria> FacturasPorAltaProveeduria { get; set; }
        public virtual DbSet<ExistenciaArticuloProveeduria> ExistenciaArticuloProveeduria { get; set; }

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

            modelBuilder.Entity<ActivoFijoComponentes>(entity =>
            {
                entity.HasKey(e => e.IdAdicion)
                    .HasName("PK_ActivoFijoComponentes_1");

                entity.HasOne(d => d.ActivoFijoComponente)
                    .WithMany(p => p.ActivosFijosComponentes)
                    .HasForeignKey(d => d.IdActivoFijoComponente)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ActivoFijoComponentes_ActivoFijo1");

                entity.HasOne(d => d.ActivoFijoOrigen)
                    .WithMany(p => p.ActivosFijosOrigen)
                    .HasForeignKey(d => d.IdActivoFijoOrigen)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ActivoFijoComponentes_ActivoFijo");
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

                entity.HasOne(d => d.CatalogoCuentaD)
                    .WithMany(p => p.ConfiguracionContabilidadIdCatalogoCuentaDNavigation)
                    .HasForeignKey(d => d.IdCatalogoCuentaD)
                    .HasConstraintName("RefCatalogoCuenta467");

                entity.HasOne(d => d.CatalogoCuentaH)
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

                entity.HasOne(d => d.ConfiguracionContabilidad)
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

                entity.HasOne(d => d.CatalogoCuentaHijo)
                    .WithMany(p => p.CatalogosCuenta)
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

                entity.Property(e => e.ValorResidual).HasColumnType("decimal");

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

                entity.HasIndex(e => e.IdDependencia)
                      .HasName("IX_Empleado_IdDependencia");

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

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<SolicitudProveeduria>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveeduria)
                    .HasName("PK_SolicitudProveeduria");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudProveeduria_IdEmpleado");
            });

            modelBuilder.Entity<SolicitudProveeduriaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveeduriaDetalle)
                    .HasName("PK_SolicitudProveeduriaDetalle");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_SolicitudProveeduriaDetalle_IdArticulo");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_SolicitudProveeduriaDetalle_IdMaestroArticuloSucursal");

                entity.HasIndex(e => e.IdSolicitudProveeduria)
                    .HasName("IX_SolicitudProveeduriaDetalle_IdSolicitudProveeduria");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.SolicitudProveeduriaDetalle)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.MaestroArticuloSucursal)
                    .WithMany(p => p.SolicitudProveeduriaDetalle)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SolicitudProveeduria)
                    .WithMany(p => p.SolicitudProveeduriaDetalle)
                    .HasForeignKey(d => d.IdSolicitudProveeduria)
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

                entity.HasIndex(e => e.IdEmpleadoRegistra)
                    .HasName("Ref15169");

                entity.HasIndex(e => e.IdEmpleadoResponsableEnvio)
                    .HasName("IX_TransferenciaActivoFijo_IdEmpleado");

                entity.HasIndex(e => e.IdEmpleadoResponsableRecibo)
                    .HasName("Ref15171");

                entity.Property(e => e.Destino)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdMotivoTransferencia).HasColumnName("idMotivoTransferencia");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.Property(e => e.Origen).HasMaxLength(50);

                entity.HasOne(d => d.EmpleadoRecibo)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoReciboNavigation)
                    .HasForeignKey(d => d.IdEmpleadoRecibo)
                    .HasConstraintName("FK_TransferenciaActivoFijo_Empleado3");

                entity.HasOne(d => d.EmpleadoRegistra)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoRegistraNavigation)
                    .HasForeignKey(d => d.IdEmpleadoRegistra)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TransferenciaActivoFijo_Empleado");

                entity.HasOne(d => d.EmpleadoResponsableEnvio)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoResponsableEnvioNavigation)
                    .HasForeignKey(d => d.IdEmpleadoResponsableEnvio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TransferenciaActivoFijo_Empleado1");

                entity.HasOne(d => d.EmpleadoResponsableRecibo)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoResponsableReciboNavigation)
                    .HasForeignKey(d => d.IdEmpleadoResponsableRecibo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TransferenciaActivoFijo_Empleado2");

                entity.HasOne(d => d.MotivoTransferencia)
                    .WithMany(p => p.TransferenciaActivoFijo)
                    .HasForeignKey(d => d.IdMotivoTransferencia)
                    .HasConstraintName("FK_TransferenciaActivoFijo_MotivoTransferencia");
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

                entity.HasIndex(e => e.IdEstadoCivil)
                    .HasName("IX_Persona_IdEstadoCivil");

                entity.HasIndex(e => e.IdEtnia)
                    .HasName("IX_Persona_IdEtnia");

                entity.HasIndex(e => e.IdGenero)
                    .HasName("IX_Persona_IdGenero");

                entity.HasIndex(e => e.IdNacionalidad)
                    .HasName("IX_Persona_IdNacionalidad");

                entity.HasIndex(e => e.IdSexo)
                    .HasName("IX_Persona_IdSexo");

                entity.HasIndex(e => e.IdTipoIdentificacion)
                    .HasName("IX_Persona_IdTipoIdentificacion");

                entity.HasIndex(e => e.IdTipoSangre)
                    .HasName("IX_Persona_IdTipoSangre");

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

                entity.HasOne(d => d.EstadoCivil)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdEstadoCivil);

                entity.HasOne(d => d.Etnia)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdEtnia);

                entity.HasOne(d => d.Genero)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdGenero);

                entity.HasOne(d => d.Nacionalidad)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdNacionalidad);

                entity.HasOne(d => d.Sexo)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdSexo);

                entity.HasOne(d => d.TipoIdentificacion)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdTipoIdentificacion);

                entity.HasOne(d => d.TipoSangre)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdTipoSangre);
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

            modelBuilder.Entity<MotivoBaja>(entity =>
            {
                entity.HasKey(e => e.IdMotivoBaja)
                    .HasName("PK_ActivoFijoMotivoBaja");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<BajaActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijoBaja)
                    .HasName("PK_ActivosFijosBaja");

                entity.Property(e => e.FechaBaja).HasColumnType("datetime");

                entity.Property(e => e.MemoOficioResolucion)
                    .HasColumnName("memoOficioResolucion")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.BajaActivosFijosDetalles)
                    .HasForeignKey(d => d.IdActivo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BajaActivoFijoDetalle_ActivoFijo");

                entity.HasOne(d => d.MotivoBaja)
                    .WithMany(p => p.BajaActivosFijosDetalles)
                    .HasForeignKey(d => d.IdMotivoBaja)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ActivosFijosBaja_ActivoFijoMotivoBaja");
            }); 

            modelBuilder.Entity<AltaActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijoAlta)
                    .HasName("PK_ActivosFijosAlta_1");

                entity.Property(e => e.FechaAlta).HasColumnType("datetime");

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.AltaActivosFijosDetalles)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_AltaActivoFijoDetalle_ActivoFijo");

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.AltaActivosFijosDetalles)
                    .HasForeignKey(d => d.IdFactura)
                    .HasConstraintName("FK_ActivosFijosAlta_Factura");
            });

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCivil)
                    .HasName("PK_EstadoCivil");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Etnia>(entity =>
            {
                entity.HasKey(e => e.IdEtnia)
                    .HasName("PK_Etnia");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero)
                    .HasName("PK_Genero");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidad)
                    .HasName("PK_Nacionalidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.IdSexo)
                    .HasName("PK_Sexo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoIdentificacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoIdentificacion)
                    .HasName("PK_TipoIdentificacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoSangre>(entity =>
            {
                entity.HasKey(e => e.IdTipoSangre)
                    .HasName("PK_TipoSangre");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Dependencia>(entity =>
            {
                entity.HasKey(e => e.IdDependencia)
                    .HasName("PK_Dependencia");

                entity.HasIndex(e => e.IdDependenciaPadre)
                    .HasName("IX_Dependencia_DependenciaPadreIdDependencia");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("IX_Dependencia_IdSucursal");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.DependenciaPadre)
                    .WithMany(p => p.InverseDependenciaPadreIdDependenciaNavigation)
                    .HasForeignKey(d => d.IdDependenciaPadre);

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.Dependencia)
                    .HasForeignKey(d => d.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MotivoTransferencia>(entity =>
            {
                entity.HasKey(e => e.IdMotivoTransferencia)
                    .HasName("PK_MotivoTransferencia");

                entity.Property(e => e.IdMotivoTransferencia).HasColumnName("idMotivoTransferencia");

                entity.Property(e => e.Motivo_Transferencia)
                    .HasColumnName("Motivo_Transferencia")
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AltaProveeduria>(entity =>
            {
                entity.HasKey(e => e.IdAlta)
                    .HasName("PK_AltaProveeduria_1");

                entity.Property(e => e.IdAlta).HasColumnName("idAlta");

                entity.Property(e => e.Acreditacion).HasColumnName("acreditacion");

                entity.Property(e => e.FechaAlta)
                    .HasColumnName("fechaAlta")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.AltaProveeduria)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IdRecepcionArticulosNavigation)
                    .WithMany(p => p.AltaProveeduria)
                    .HasForeignKey(d => d.IdRecepcionArticulos)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_AltaProveeduria_RecepcionArticulos");
            });

            modelBuilder.Entity<BajaProveeduria>(entity =>
            {
                entity.HasKey(e => e.IdBaja)
                    .HasName("PK_BajaProveeduria");

                entity.Property(e => e.IdBaja).HasColumnName("idBaja");

                entity.Property(e => e.FechaBaja)
                    .HasColumnName("fechaBaja")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.BajaProveeduria)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BajaProveeduria_Articulo");
            });

            modelBuilder.Entity<FacturasPorAltaProveeduria>(entity =>
            {
                entity.HasKey(e => e.IdFacturasPorAlta)
                    .HasName("PK_FacturasPorAltaProveeduria");

                entity.Property(e => e.IdFacturasPorAlta).HasColumnName("idFacturasPorAlta");

                entity.Property(e => e.IdAlta).HasColumnName("idAlta");

                entity.Property(e => e.NumeroFactura).HasMaxLength(30);

                entity.HasOne(d => d.IdAltaNavigation)
                    .WithMany(p => p.FacturasPorAltaProveeduria)
                    .HasForeignKey(d => d.IdAlta)
                    .HasConstraintName("FK_FacturasPorAltaProveeduria_AltaProveeduria");
            });
            
            modelBuilder.Entity<ExistenciaArticuloProveeduria>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK_ExistenciaArticuloProveeduria");

                entity.Property(e => e.IdArticulo).ValueGeneratedNever();

                entity.Property(e => e.Existencia).HasColumnName("existencia");

                entity.HasOne(d => d.Articulo)
                    .WithOne(p => p.ExistenciaArticuloProveeduria)
                    .HasForeignKey<ExistenciaArticuloProveeduria>(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ExistenciaArticuloProveeduria_Articulo");
            });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }            
        }
    }
}