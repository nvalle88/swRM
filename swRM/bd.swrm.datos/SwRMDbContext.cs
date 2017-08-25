using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using bd.swrm.entidades.Negocio;

namespace bd.swrm.datos
{
    public partial class SwRMDbContext : DbContext
    {
        public SwRMDbContext(DbContextOptions<SwRMDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<AccionPersonal> AccionPersonal { get; set; }
        public virtual DbSet<ActividadesAnalisisOcupacional> ActividadesAnalisisOcupacional { get; set; }
        public virtual DbSet<ActividadesEsenciales> ActividadesEsenciales { get; set; }
        public virtual DbSet<ActividadesGestionCambio> ActividadesGestionCambio { get; set; }
        public virtual DbSet<ActivoFijo> ActivoFijo { get; set; }
        public virtual DbSet<AdministracionTalentoHumano> AdministracionTalentoHumano { get; set; }
        public virtual DbSet<AnoExperiencia> AnoExperiencia { get; set; }
        public virtual DbSet<AprobacionViatico> AprobacionViatico { get; set; }
        public virtual DbSet<AreaConocimiento> AreaConocimiento { get; set; }
        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<AvanceGestionCambio> AvanceGestionCambio { get; set; }
        public virtual DbSet<BrigadaSso> BrigadaSso { get; set; }
        public virtual DbSet<BrigadaSsorol> BrigadaSsorol { get; set; }
        public virtual DbSet<CandidatoConcurso> CandidatoConcurso { get; set; }
        public virtual DbSet<Canditato> Canditato { get; set; }
        public virtual DbSet<Capacitacion> Capacitacion { get; set; }
        public virtual DbSet<CapacitacionAreaConocimiento> CapacitacionAreaConocimiento { get; set; }
        public virtual DbSet<CapacitacionEncuesta> CapacitacionEncuesta { get; set; }
        public virtual DbSet<CapacitacionModalidad> CapacitacionModalidad { get; set; }
        public virtual DbSet<CapacitacionPlanificacion> CapacitacionPlanificacion { get; set; }
        public virtual DbSet<CapacitacionPregunta> CapacitacionPregunta { get; set; }
        public virtual DbSet<CapacitacionProveedor> CapacitacionProveedor { get; set; }
        public virtual DbSet<CapacitacionRecibida> CapacitacionRecibida { get; set; }
        public virtual DbSet<CapacitacionRespuesta> CapacitacionRespuesta { get; set; }
        public virtual DbSet<CapacitacionTemario> CapacitacionTemario { get; set; }
        public virtual DbSet<CapacitacionTemarioProveedor> CapacitacionTemarioProveedor { get; set; }
        public virtual DbSet<CapacitacionTipoPregunta> CapacitacionTipoPregunta { get; set; }
        public virtual DbSet<CatalogoCuenta> CatalogoCuenta { get; set; }
        public virtual DbSet<Ciudad> Ciudad { get; set; }
        public virtual DbSet<ClaseActivoFijo> ClaseActivoFijo { get; set; }
        public virtual DbSet<ClaseArticulo> ClaseArticulo { get; set; }
        public virtual DbSet<CodigoActivoFijo> CodigoActivoFijo { get; set; }
        public virtual DbSet<ComportamientoObservable> ComportamientoObservable { get; set; }
        public virtual DbSet<ConfiguracionContabilidad> ConfiguracionContabilidad { get; set; }
        public virtual DbSet<ConfiguracionViatico> ConfiguracionViatico { get; set; }
        public virtual DbSet<ConfirmacionLectura> ConfirmacionLectura { get; set; }
        public virtual DbSet<ConocimientosAdicionales> ConocimientosAdicionales { get; set; }
        public virtual DbSet<DatosBancarios> DatosBancarios { get; set; }
        public virtual DbSet<DeclaracionPatrimonioPersonal> DeclaracionPatrimonioPersonal { get; set; }
        public virtual DbSet<DenominacionCompetencia> DenominacionCompetencia { get; set; }
        public virtual DbSet<Dependencia> Dependencia { get; set; }
        public virtual DbSet<DependenciaDocumento> DependenciaDocumento { get; set; }
        public virtual DbSet<DepreciacionActivoFijo> DepreciacionActivoFijo { get; set; }
        public virtual DbSet<Destreza> Destreza { get; set; }
        public virtual DbSet<DetalleExamenInduccion> DetalleExamenInduccion { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }
        public virtual DbSet<DocumentoInformacionInstitucional> DocumentoInformacionInstitucional { get; set; }
        public virtual DbSet<DocumentosCargados> DocumentosCargados { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<EmpleadoActivoFijo> EmpleadoActivoFijo { get; set; }
        public virtual DbSet<EmpleadoContactoEmergencia> EmpleadoContactoEmergencia { get; set; }
        public virtual DbSet<EmpleadoFamiliar> EmpleadoFamiliar { get; set; }
        public virtual DbSet<EmpleadoFormularioCapacitacion> EmpleadoFormularioCapacitacion { get; set; }
        public virtual DbSet<EmpleadoIe> EmpleadoIe { get; set; }
        public virtual DbSet<EmpleadoImpuestoRenta> EmpleadoImpuestoRenta { get; set; }
        public virtual DbSet<EmpleadoMovimiento> EmpleadoMovimiento { get; set; }
        public virtual DbSet<EmpleadoNepotismo> EmpleadoNepotismo { get; set; }
        public virtual DbSet<EmpleadoSaldoVacaciones> EmpleadoSaldoVacaciones { get; set; }
        public virtual DbSet<EmpleadosFormularioDevengacion> EmpleadosFormularioDevengacion { get; set; }
        public virtual DbSet<EscalaEvaluacionTotal> EscalaEvaluacionTotal { get; set; }
        public virtual DbSet<EscalaGrados> EscalaGrados { get; set; }
        public virtual DbSet<EspecificidadExperiencia> EspecificidadExperiencia { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<Estudio> Estudio { get; set; }
        public virtual DbSet<Etnia> Etnia { get; set; }
        public virtual DbSet<Eval001> Eval001 { get; set; }
        public virtual DbSet<EvaluacionActividadesPuestoTrabajo> EvaluacionActividadesPuestoTrabajo { get; set; }
        public virtual DbSet<EvaluacionActividadesPuestoTrabajoDetalle> EvaluacionActividadesPuestoTrabajoDetalle { get; set; }
        public virtual DbSet<EvaluacionActividadesPuestoTrabajoFactor> EvaluacionActividadesPuestoTrabajoFactor { get; set; }
        public virtual DbSet<EvaluacionCompetenciasTecnicasPuesto> EvaluacionCompetenciasTecnicasPuesto { get; set; }
        public virtual DbSet<EvaluacionCompetenciasTecnicasPuestoDetalle> EvaluacionCompetenciasTecnicasPuestoDetalle { get; set; }
        public virtual DbSet<EvaluacionCompetenciasTecnicasPuestoFactor> EvaluacionCompetenciasTecnicasPuestoFactor { get; set; }
        public virtual DbSet<EvaluacionCompetenciasUniversales> EvaluacionCompetenciasUniversales { get; set; }
        public virtual DbSet<EvaluacionCompetenciasUniversalesDetalle> EvaluacionCompetenciasUniversalesDetalle { get; set; }
        public virtual DbSet<EvaluacionCompetenciasUniversalesFactor> EvaluacionCompetenciasUniversalesFactor { get; set; }
        public virtual DbSet<EvaluacionConocimiento> EvaluacionConocimiento { get; set; }
        public virtual DbSet<EvaluacionConocimientoDetalle> EvaluacionConocimientoDetalle { get; set; }
        public virtual DbSet<EvaluacionConocimientoFactor> EvaluacionConocimientoFactor { get; set; }
        public virtual DbSet<EvaluacionInducion> EvaluacionInducion { get; set; }
        public virtual DbSet<EvaluacionTrabajoEquipoIniciativaLiderazgo> EvaluacionTrabajoEquipoIniciativaLiderazgo { get; set; }
        public virtual DbSet<EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle> EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle { get; set; }
        public virtual DbSet<EvaluacionTrabajoEquipoIniciativaLiderazgoFactor> EvaluacionTrabajoEquipoIniciativaLiderazgoFactor { get; set; }
        public virtual DbSet<Evaluador> Evaluador { get; set; }
        public virtual DbSet<Exepciones> Exepciones { get; set; }
        public virtual DbSet<ExperienciaLaboralRequerida> ExperienciaLaboralRequerida { get; set; }
        public virtual DbSet<Factor> Factor { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<FacturaViatico> FacturaViatico { get; set; }
        public virtual DbSet<FaseConcurso> FaseConcurso { get; set; }
        public virtual DbSet<FondoFinanciamiento> FondoFinanciamiento { get; set; }
        public virtual DbSet<FormularioAnalisisOcupacional> FormularioAnalisisOcupacional { get; set; }
        public virtual DbSet<FormularioCapacitacion> FormularioCapacitacion { get; set; }
        public virtual DbSet<FormularioDevengacion> FormularioDevengacion { get; set; }
        public virtual DbSet<FormulasRmu> FormulasRmu { get; set; }
        public virtual DbSet<FrecuenciaAplicacion> FrecuenciaAplicacion { get; set; }
        public virtual DbSet<GastoRubro> GastoRubro { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<GrupoOcupacional> GrupoOcupacional { get; set; }
        public virtual DbSet<ImpuestoRentaParametros> ImpuestoRentaParametros { get; set; }
        public virtual DbSet<Indicador> Indicador { get; set; }
        public virtual DbSet<IndiceOcupacional> IndiceOcupacional { get; set; }
        public virtual DbSet<IndiceOcupacionalActividadesEsenciales> IndiceOcupacionalActividadesEsenciales { get; set; }
        public virtual DbSet<IndiceOcupacionalAreaConocimiento> IndiceOcupacionalAreaConocimiento { get; set; }
        public virtual DbSet<IndiceOcupacionalCapacitaciones> IndiceOcupacionalCapacitaciones { get; set; }
        public virtual DbSet<IndiceOcupacionalComportamientoObservable> IndiceOcupacionalComportamientoObservable { get; set; }
        public virtual DbSet<IndiceOcupacionalConocimientosAdicionales> IndiceOcupacionalConocimientosAdicionales { get; set; }
        public virtual DbSet<IndiceOcupacionalEstudio> IndiceOcupacionalEstudio { get; set; }
        public virtual DbSet<IndiceOcupacionalModalidadPartida> IndiceOcupacionalModalidadPartida { get; set; }
        public virtual DbSet<InformeUath> InformeUath { get; set; }
        public virtual DbSet<InformeViatico> InformeViatico { get; set; }
        public virtual DbSet<IngresoEgresoRmu> IngresoEgresoRmu { get; set; }
        public virtual DbSet<InstitucionFinanciera> InstitucionFinanciera { get; set; }
        public virtual DbSet<InstruccionFormal> InstruccionFormal { get; set; }
        public virtual DbSet<ItemViatico> ItemViatico { get; set; }
        public virtual DbSet<ItinerarioViatico> ItinerarioViatico { get; set; }
        public virtual DbSet<LibroActivoFijo> LibroActivoFijo { get; set; }
        public virtual DbSet<Liquidacion> Liquidacion { get; set; }
        public virtual DbSet<MaestroArticuloSucursal> MaestroArticuloSucursal { get; set; }
        public virtual DbSet<MaestroDetalleArticulo> MaestroDetalleArticulo { get; set; }
        public virtual DbSet<MantenimientoActivoFijo> MantenimientoActivoFijo { get; set; }
        public virtual DbSet<ManualPuesto> ManualPuesto { get; set; }
        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<MaterialApoyo> MaterialApoyo { get; set; }
        public virtual DbSet<Mision> Mision { get; set; }
        public virtual DbSet<MisionIndiceOcupacional> MisionIndiceOcupacional { get; set; }
        public virtual DbSet<ModalidadPartida> ModalidadPartida { get; set; }
        public virtual DbSet<Modelo> Modelo { get; set; }
        public virtual DbSet<ModosScializacion> ModosScializacion { get; set; }
        public virtual DbSet<MotivoAsiento> MotivoAsiento { get; set; }
        public virtual DbSet<MotivoRecepcion> MotivoRecepcion { get; set; }
        public virtual DbSet<Nacionalidad> Nacionalidad { get; set; }
        public virtual DbSet<NacionalidadIndigena> NacionalidadIndigena { get; set; }
        public virtual DbSet<Nivel> Nivel { get; set; }
        public virtual DbSet<NivelConocimiento> NivelConocimiento { get; set; }
        public virtual DbSet<NivelDesarrollo> NivelDesarrollo { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<PaquetesInformaticos> PaquetesInformaticos { get; set; }
        public virtual DbSet<ParametrosGenerales> ParametrosGenerales { get; set; }
        public virtual DbSet<Parentesco> Parentesco { get; set; }
        public virtual DbSet<Parroquia> Parroquia { get; set; }
        public virtual DbSet<PartidasFase> PartidasFase { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PersonaCapacitacion> PersonaCapacitacion { get; set; }
        public virtual DbSet<PersonaDiscapacidad> PersonaDiscapacidad { get; set; }
        public virtual DbSet<PersonaEnfermedad> PersonaEnfermedad { get; set; }
        public virtual DbSet<PersonaEstudio> PersonaEstudio { get; set; }
        public virtual DbSet<PersonaPaquetesInformaticos> PersonaPaquetesInformaticos { get; set; }
        public virtual DbSet<PlanGestionCambio> PlanGestionCambio { get; set; }
        public virtual DbSet<PlanificacionHe> PlanificacionHe { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<PreguntaRespuesta> PreguntaRespuesta { get; set; }
        public virtual DbSet<Proceso> Proceso { get; set; }
        public virtual DbSet<ProcesoDetalle> ProcesoDetalle { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Provincia> Provincia { get; set; }
        public virtual DbSet<Provisiones> Provisiones { get; set; }
        public virtual DbSet<RealizaExamenInduccion> RealizaExamenInduccion { get; set; }
        public virtual DbSet<RecepcionActivoFijo> RecepcionActivoFijo { get; set; }
        public virtual DbSet<RecepcionActivoFijoDetalle> RecepcionActivoFijoDetalle { get; set; }
        public virtual DbSet<RecepcionArticulos> RecepcionArticulos { get; set; }
        public virtual DbSet<RegimenLaboral> RegimenLaboral { get; set; }
        public virtual DbSet<RegistroEntradaSalida> RegistroEntradaSalida { get; set; }
        public virtual DbSet<RelacionLaboral> RelacionLaboral { get; set; }
        public virtual DbSet<RelacionesInternasExternas> RelacionesInternasExternas { get; set; }
        public virtual DbSet<RelacionesInternasExternasIndiceOcupacional> RelacionesInternasExternasIndiceOcupacional { get; set; }
        public virtual DbSet<Relevancia> Relevancia { get; set; }
        public virtual DbSet<RequisitosNoCumple> RequisitosNoCumple { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<Rmu> Rmu { get; set; }
        public virtual DbSet<RolPagoDetalle> RolPagoDetalle { get; set; }
        public virtual DbSet<RolPagos> RolPagos { get; set; }
        public virtual DbSet<RolPuesto> RolPuesto { get; set; }
        public virtual DbSet<Rubro> Rubro { get; set; }
        public virtual DbSet<RubroLiquidacion> RubroLiquidacion { get; set; }
        public virtual DbSet<Sexo> Sexo { get; set; }
        public virtual DbSet<SituacionPropuesta> SituacionPropuesta { get; set; }
        public virtual DbSet<SolicitudAcumulacionDecimos> SolicitudAcumulacionDecimos { get; set; }
        public virtual DbSet<SolicitudAnticipo> SolicitudAnticipo { get; set; }
        public virtual DbSet<SolicitudCertificadoPersonal> SolicitudCertificadoPersonal { get; set; }
        public virtual DbSet<SolicitudHorasExtras> SolicitudHorasExtras { get; set; }
        public virtual DbSet<SolicitudLiquidacionHaberes> SolicitudLiquidacionHaberes { get; set; }
        public virtual DbSet<SolicitudModificacionFichaEmpleado> SolicitudModificacionFichaEmpleado { get; set; }
        public virtual DbSet<SolicitudPermiso> SolicitudPermiso { get; set; }
        public virtual DbSet<SolicitudPlanificacionVacaciones> SolicitudPlanificacionVacaciones { get; set; }
        public virtual DbSet<SolicitudProveduria> SolicitudProveduria { get; set; }
        public virtual DbSet<SolicitudProveduriaDetalle> SolicitudProveduriaDetalle { get; set; }
        public virtual DbSet<SolicitudVacaciones> SolicitudVacaciones { get; set; }
        public virtual DbSet<SolicitudViatico> SolicitudViatico { get; set; }
        public virtual DbSet<SubClaseActivoFijo> SubClaseActivoFijo { get; set; }
        public virtual DbSet<SubClaseArticulo> SubClaseArticulo { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<TablaDepreciacion> TablaDepreciacion { get; set; }
        public virtual DbSet<Temporal> Temporal { get; set; }
        public virtual DbSet<TipoAccionPersonal> TipoAccionPersonal { get; set; }
        public virtual DbSet<TipoActivoFijo> TipoActivoFijo { get; set; }
        public virtual DbSet<TipoArticulo> TipoArticulo { get; set; }
        public virtual DbSet<TipoCertificado> TipoCertificado { get; set; }
        public virtual DbSet<TipoConsurso> TipoConsurso { get; set; }
        public virtual DbSet<TipoDiscapacidad> TipoDiscapacidad { get; set; }
        public virtual DbSet<TipoDiscapacidadSustituto> TipoDiscapacidadSustituto { get; set; }
        public virtual DbSet<TipoEnfermedad> TipoEnfermedad { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }
        public virtual DbSet<TipoMovimientoInterno> TipoMovimientoInterno { get; set; }
        public virtual DbSet<TipoNombramiento> TipoNombramiento { get; set; }
        public virtual DbSet<TipoPermiso> TipoPermiso { get; set; }
        public virtual DbSet<TipoProvision> TipoProvision { get; set; }
        public virtual DbSet<TipoRmu> TipoRmu { get; set; }
        public virtual DbSet<TipoSangre> TipoSangre { get; set; }
        public virtual DbSet<TipoTransporte> TipoTransporte { get; set; }
        public virtual DbSet<TipoViatico> TipoViatico { get; set; }
        public virtual DbSet<Titulo> Titulo { get; set; }
        public virtual DbSet<TrabajoEquipoIniciativaLiderazgo> TrabajoEquipoIniciativaLiderazgo { get; set; }
        public virtual DbSet<TranferenciaArticulo> TranferenciaArticulo { get; set; }
        public virtual DbSet<TransferenciaActivoFijo> TransferenciaActivoFijo { get; set; }
        public virtual DbSet<TransferenciaActivoFijoDetalle> TransferenciaActivoFijoDetalle { get; set; }
        public virtual DbSet<TrayectoriaLaboral> TrayectoriaLaboral { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
        public virtual DbSet<ValidacionInmediatoSuperior> ValidacionInmediatoSuperior { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccionPersonal>(entity =>
            {
                entity.HasKey(e => e.IdAccionPersonal)
                    .HasName("PK188");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15463");

                entity.HasIndex(e => e.IdTipoAccionPersonal)
                    .HasName("Ref305462");

                entity.Property(e => e.IdAccionPersonal)
                    .HasColumnName("idAccionPersonal")
                    .HasColumnType("char(10)");

                entity.Property(e => e.Explicacion).HasColumnType("text");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdTipoAccionPersonal).HasColumnName("idTipoAccionPersonal");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Solicitud).HasColumnType("text");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.AccionPersonal)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado463");

                entity.HasOne(d => d.IdTipoAccionPersonalNavigation)
                    .WithMany(p => p.AccionPersonal)
                    .HasForeignKey(d => d.IdTipoAccionPersonal)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoAccionPersonal462");
            });

            modelBuilder.Entity<ActividadesAnalisisOcupacional>(entity =>
            {
                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("Ref107307");

                entity.Property(e => e.Actividades).HasColumnType("varchar(250)");

                entity.Property(e => e.IdFormularioAnalisisOcupacional).HasColumnName("idFormularioAnalisisOcupacional");

                entity.HasOne(d => d.IdFormularioAnalisisOcupacionalNavigation)
                    .WithMany(p => p.ActividadesAnalisisOcupacional)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFormularioAnalisisOcupacional307");
            });

            modelBuilder.Entity<ActividadesEsenciales>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<ActividadesGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdActividadesGestionCambio)
                    .HasName("PK260");

                entity.HasIndex(e => e.IdPlanGestionCambio)
                    .HasName("Ref262401");

                entity.Property(e => e.Descripcion).HasColumnType("char(10)");

                entity.HasOne(d => d.IdPlanGestionCambioNavigation)
                    .WithMany(p => p.ActividadesGestionCambio)
                    .HasForeignKey(d => d.IdPlanGestionCambio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPlanGestionCambio401");
            });

            modelBuilder.Entity<ActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdActivoFijo)
                    .HasName("PK89");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("Ref3129");

                entity.HasIndex(e => e.IdCodigoActivoFijo)
                    .HasName("Ref90131");

                entity.HasIndex(e => e.IdLibroActivoFijo)
                    .HasName("Ref91132");

                entity.HasIndex(e => e.IdModelo)
                    .HasName("Ref135217");

                entity.HasIndex(e => e.IdSubClaseActivoFijo)
                    .HasName("Ref88127");

                entity.HasIndex(e => e.IdUnidadMedida)
                    .HasName("Ref84128");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");

                entity.Property(e => e.IdCodigoActivoFijo).HasColumnName("idCodigoActivoFijo");

                entity.Property(e => e.IdLibroActivoFijo).HasColumnName("idLibroActivoFijo");

                entity.Property(e => e.IdModelo).HasColumnName("idModelo");

                entity.Property(e => e.IdSubClaseActivoFijo).HasColumnName("idSubClaseActivoFijo");

                entity.Property(e => e.IdUnidadMedida).HasColumnName("idUnidadMedida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Serie)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ValorCompra).HasColumnType("decimal");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdCiudad)
                    .HasConstraintName("RefCiudad129");

                entity.HasOne(d => d.IdCodigoActivoFijoNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdCodigoActivoFijo)
                    .HasConstraintName("RefCodigoActivoFijo131");

                entity.HasOne(d => d.IdLibroActivoFijoNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdLibroActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefLibroActivoFijo132");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("RefModelo217");

                entity.HasOne(d => d.IdSubClaseActivoFijoNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdSubClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSubClaseActivoFijo127");

                entity.HasOne(d => d.IdUnidadMedidaNavigation)
                    .WithMany(p => p.ActivoFijo)
                    .HasForeignKey(d => d.IdUnidadMedida)
                    .HasConstraintName("RefUnidadMedida128");
            });

            modelBuilder.Entity<AdministracionTalentoHumano>(entity =>
            {
                entity.HasIndex(e => e.EmpleadoResponsable)
                    .HasName("Ref15323");

                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("Ref107317");

                entity.HasIndex(e => e.IdRolPuesto)
                    .HasName("Ref63315");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdFormularioAnalisisOcupacional).HasColumnName("idFormularioAnalisisOcupacional");

                entity.Property(e => e.IdRolPuesto).HasColumnName("idRolPuesto");

                entity.HasOne(d => d.EmpleadoResponsableNavigation)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.EmpleadoResponsable)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado323");

                entity.HasOne(d => d.IdFormularioAnalisisOcupacionalNavigation)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFormularioAnalisisOcupacional317");

                entity.HasOne(d => d.IdRolPuestoNavigation)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.IdRolPuesto)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRolPuesto315");
            });

            modelBuilder.Entity<AnoExperiencia>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("varchar(25)");
            });

            modelBuilder.Entity<AprobacionViatico>(entity =>
            {
                entity.HasKey(e => e.IdAprobacionViatico)
                    .HasName("PK254");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15392");

                entity.HasIndex(e => e.IdSolicitudViatico)
                    .HasName("Ref77390");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdSolicitudViatico).HasColumnName("idSolicitudViatico");

                entity.Property(e => e.ValorAdescontar)
                    .HasColumnName("ValorADescontar")
                    .HasColumnType("char(10)");

                entity.Property(e => e.ValorJustificado).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.AprobacionViatico)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado392");

                entity.HasOne(d => d.IdSolicitudViaticoNavigation)
                    .WithMany(p => p.AprobacionViatico)
                    .HasForeignKey(d => d.IdSolicitudViatico)
                    .HasConstraintName("RefSolicitudViatico390");
            });

            modelBuilder.Entity<AreaConocimiento>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK99");

                entity.HasIndex(e => e.IdModelo)
                    .HasName("Ref135219");

                entity.HasIndex(e => e.IdSubClaseArticulo)
                    .HasName("Ref98141");

                entity.HasIndex(e => e.IdUnidadMedida)
                    .HasName("Ref84142");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdModelo).HasColumnName("idModelo");

                entity.Property(e => e.IdSubClaseArticulo).HasColumnName("idSubClaseArticulo");

                entity.Property(e => e.IdUnidadMedida).HasColumnName("idUnidadMedida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("RefModelo219");

                entity.HasOne(d => d.IdSubClaseArticuloNavigation)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdSubClaseArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSubClaseArticulo141");

                entity.HasOne(d => d.IdUnidadMedidaNavigation)
                    .WithMany(p => p.Articulo)
                    .HasForeignKey(d => d.IdUnidadMedida)
                    .HasConstraintName("RefUnidadMedida142");
            });

            modelBuilder.Entity<AvanceGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdAvanceGestionCambio)
                    .HasName("PK263");

                entity.HasIndex(e => e.IdActividadesGestionCambio)
                    .HasName("Ref260402");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.IdActividadesGestionCambioNavigation)
                    .WithMany(p => p.AvanceGestionCambio)
                    .HasForeignKey(d => d.IdActividadesGestionCambio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefActividadesGestionCambio402");
            });

            modelBuilder.Entity<BrigadaSso>(entity =>
            {
                entity.HasKey(e => e.IdBrigadaSso)
                    .HasName("PK66");

                entity.ToTable("BrigadaSSO");

                entity.Property(e => e.IdBrigadaSso).HasColumnName("idBrigadaSSO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<BrigadaSsorol>(entity =>
            {
                entity.HasKey(e => e.IdBrigadaSsorol)
                    .HasName("PK67");

                entity.ToTable("BrigadaSSORol");

                entity.HasIndex(e => e.IdBrigadaSso)
                    .HasName("Ref6696");

                entity.Property(e => e.IdBrigadaSsorol).HasColumnName("idBrigadaSSORol");

                entity.Property(e => e.IdBrigadaSso).HasColumnName("idBrigadaSSO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdBrigadaSsoNavigation)
                    .WithMany(p => p.BrigadaSsorol)
                    .HasForeignKey(d => d.IdBrigadaSso)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefBrigadaSSO96");
            });

            modelBuilder.Entity<CandidatoConcurso>(entity =>
            {
                entity.HasKey(e => e.IdCandidatoConcurso)
                    .HasName("PK270");

                entity.HasIndex(e => e.IdCanditato)
                    .HasName("Ref267419");

                entity.HasIndex(e => e.IdPartidasFase)
                    .HasName("Ref273430");

                entity.Property(e => e.CodigoSocioEmpleo).HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdCanditatoNavigation)
                    .WithMany(p => p.CandidatoConcurso)
                    .HasForeignKey(d => d.IdCanditato)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCanditato419");

                entity.HasOne(d => d.IdPartidasFaseNavigation)
                    .WithMany(p => p.CandidatoConcurso)
                    .HasForeignKey(d => d.IdPartidasFase)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPartidasFase430");
            });

            modelBuilder.Entity<Canditato>(entity =>
            {
                entity.HasKey(e => e.IdCanditato)
                    .HasName("PK267");
            });

            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacion)
                    .HasName("PK12_1");

                entity.Property(e => e.IdCapacitacion).HasColumnName("idCapacitacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<CapacitacionAreaConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionAreaConocimiento)
                    .HasName("PK169");

                entity.Property(e => e.IdCapacitacionAreaConocimiento).HasColumnName("idCapacitacionAreaConocimiento");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<CapacitacionEncuesta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionEncuesta)
                    .HasName("PK171");

                entity.HasIndex(e => e.IdCapacitacionRecibida)
                    .HasName("Ref315470");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15485");

                entity.Property(e => e.IdCapacitacionEncuesta)
                    .HasColumnName("idCapacitacionEncuesta")
                    .HasColumnType("char(10)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IdCapacitacionRecibida).HasColumnName("idCapacitacionRecibida");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdCapacitacionRecibidaNavigation)
                    .WithMany(p => p.CapacitacionEncuesta)
                    .HasForeignKey(d => d.IdCapacitacionRecibida)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionRecibida470");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.CapacitacionEncuesta)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado485");
            });

            modelBuilder.Entity<CapacitacionModalidad>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionModalidad)
                    .HasName("PK178");

                entity.Property(e => e.IdCapacitacionModalidad).HasColumnName("idCapacitacionModalidad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<CapacitacionPlanificacion>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionPlanificacion)
                    .HasName("PK167");

                entity.HasIndex(e => e.IdCapacitacionModalidad)
                    .HasName("Ref322478");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("Ref313479");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15484");

                entity.Property(e => e.IdCapacitacionPlanificacion).HasColumnName("idCapacitacionPlanificacion");

                entity.Property(e => e.IdCapacitacionModalidad).HasColumnName("idCapacitacionModalidad");

                entity.Property(e => e.IdCapacitacionTemario).HasColumnName("idCapacitacionTemario");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.Presupuesto).HasColumnType("decimal");

                entity.HasOne(d => d.IdCapacitacionModalidadNavigation)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdCapacitacionModalidad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionModalidad478");

                entity.HasOne(d => d.IdCapacitacionTemarioNavigation)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionTemario479");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado484");
            });

            modelBuilder.Entity<CapacitacionPregunta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionPregunta)
                    .HasName("PK172");

                entity.HasIndex(e => e.IdCapacitacionEncuesta)
                    .HasName("Ref316471");

                entity.HasIndex(e => e.IdCapacitacionTipoPregunta)
                    .HasName("Ref319473");

                entity.Property(e => e.IdCapacitacionPregunta).HasColumnName("idCapacitacionPregunta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.IdCapacitacionEncuesta)
                    .IsRequired()
                    .HasColumnName("idCapacitacionEncuesta")
                    .HasColumnType("char(10)");

                entity.Property(e => e.IdCapacitacionTipoPregunta).HasColumnName("idCapacitacionTipoPregunta");

                entity.HasOne(d => d.IdCapacitacionEncuestaNavigation)
                    .WithMany(p => p.CapacitacionPregunta)
                    .HasForeignKey(d => d.IdCapacitacionEncuesta)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionEncuesta471");

                entity.HasOne(d => d.IdCapacitacionTipoPreguntaNavigation)
                    .WithMany(p => p.CapacitacionPregunta)
                    .HasForeignKey(d => d.IdCapacitacionTipoPregunta)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionTipoPregunta473");
            });

            modelBuilder.Entity<CapacitacionProveedor>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionProveedor)
                    .HasName("PK175");

                entity.HasIndex(e => e.IdCapacitacionRecibida)
                    .HasName("Ref315476");

                entity.Property(e => e.IdCapacitacionProveedor).HasColumnName("idCapacitacionProveedor");

                entity.Property(e => e.Direccion).HasColumnType("varchar(40)");

                entity.Property(e => e.IdCapacitacionRecibida).HasColumnName("idCapacitacionRecibida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Telefono).HasColumnType("varchar(20)");

                entity.HasOne(d => d.IdCapacitacionRecibidaNavigation)
                    .WithMany(p => p.CapacitacionProveedor)
                    .HasForeignKey(d => d.IdCapacitacionRecibida)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionRecibida476");
            });

            modelBuilder.Entity<CapacitacionRecibida>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionRecibida)
                    .HasName("PK170");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("Ref313481");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15483");

                entity.Property(e => e.IdCapacitacionRecibida).HasColumnName("idCapacitacionRecibida");

                entity.Property(e => e.IdCapacitacionTemario).HasColumnName("idCapacitacionTemario");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdCapacitacionTemarioNavigation)
                    .WithMany(p => p.CapacitacionRecibida)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionTemario481");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.CapacitacionRecibida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado483");
            });

            modelBuilder.Entity<CapacitacionRespuesta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionRespuesta)
                    .HasName("PK173");

                entity.HasIndex(e => e.IdCapacitacionPregunta)
                    .HasName("Ref317472");

                entity.Property(e => e.IdCapacitacionRespuesta).HasColumnName("idCapacitacionRespuesta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(60)");

                entity.Property(e => e.IdCapacitacionPregunta).HasColumnName("idCapacitacionPregunta");

                entity.HasOne(d => d.IdCapacitacionPreguntaNavigation)
                    .WithMany(p => p.CapacitacionRespuesta)
                    .HasForeignKey(d => d.IdCapacitacionPregunta)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionPregunta472");
            });

            modelBuilder.Entity<CapacitacionTemario>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTemario)
                    .HasName("PK168");

                entity.HasIndex(e => e.IdCapacitacionAreaConocimiento)
                    .HasName("Ref314480");

                entity.Property(e => e.IdCapacitacionTemario).HasColumnName("idCapacitacionTemario");

                entity.Property(e => e.IdCapacitacionAreaConocimiento).HasColumnName("idCapacitacionAreaConocimiento");

                entity.Property(e => e.Tema)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.HasOne(d => d.IdCapacitacionAreaConocimientoNavigation)
                    .WithMany(p => p.CapacitacionTemario)
                    .HasForeignKey(d => d.IdCapacitacionAreaConocimiento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionAreaConocimiento480");
            });

            modelBuilder.Entity<CapacitacionTemarioProveedor>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTemarioProveedor)
                    .HasName("PK177");

                entity.HasIndex(e => e.IdCapacitacionModalidad)
                    .HasName("Ref322477");

                entity.HasIndex(e => e.IdCapacitacionProveedor)
                    .HasName("Ref320475");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("Ref313474");

                entity.Property(e => e.IdCapacitacionTemarioProveedor).HasColumnName("idCapacitacionTemarioProveedor");

                entity.Property(e => e.Costo).HasColumnType("decimal");

                entity.Property(e => e.IdCapacitacionModalidad).HasColumnName("idCapacitacionModalidad");

                entity.Property(e => e.IdCapacitacionProveedor).HasColumnName("idCapacitacionProveedor");

                entity.Property(e => e.IdCapacitacionTemario).HasColumnName("idCapacitacionTemario");

                entity.HasOne(d => d.IdCapacitacionModalidadNavigation)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionModalidad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionModalidad477");

                entity.HasOne(d => d.IdCapacitacionProveedorNavigation)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionProveedor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionProveedor475");

                entity.HasOne(d => d.IdCapacitacionTemarioNavigation)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacionTemario474");
            });

            modelBuilder.Entity<CapacitacionTipoPregunta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTipoPregunta)
                    .HasName("PK174");

                entity.Property(e => e.IdCapacitacionTipoPregunta).HasColumnName("idCapacitacionTipoPregunta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(10)");
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

            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.HasKey(e => e.IdCiudad)
                    .HasName("PK3");

                entity.HasIndex(e => e.IdProvincia)
                    .HasName("Ref13");

                entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");

                entity.Property(e => e.IdProvincia).HasColumnName("idProvincia");

                entity.Property(e => e.Name).HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdProvinciaNavigation)
                    .WithMany(p => p.Ciudad)
                    .HasForeignKey(d => d.IdProvincia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefProvincia3");
            });

            modelBuilder.Entity<ClaseActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdClaseActivoFijo)
                    .HasName("PK87");

                entity.HasIndex(e => e.IdTablaDepreciacion)
                    .HasName("Ref85124");

                entity.HasIndex(e => e.IdTipoActivoFijo)
                    .HasName("Ref86123");

                entity.Property(e => e.IdClaseActivoFijo).HasColumnName("idClaseActivoFijo");

                entity.Property(e => e.IdTablaDepreciacion).HasColumnName("idTablaDepreciacion");

                entity.Property(e => e.IdTipoActivoFijo).HasColumnName("idTipoActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdTablaDepreciacionNavigation)
                    .WithMany(p => p.ClaseActivoFijo)
                    .HasForeignKey(d => d.IdTablaDepreciacion)
                    .HasConstraintName("RefTablaDepreciacion124");

                entity.HasOne(d => d.IdTipoActivoFijoNavigation)
                    .WithMany(p => p.ClaseActivoFijo)
                    .HasForeignKey(d => d.IdTipoActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoActivoFijo123");
            });

            modelBuilder.Entity<ClaseArticulo>(entity =>
            {
                entity.HasKey(e => e.IdClaseArticulo)
                    .HasName("PK97");

                entity.HasIndex(e => e.IdTipoArticulo)
                    .HasName("Ref96139");

                entity.Property(e => e.IdClaseArticulo).HasColumnName("idClaseArticulo");

                entity.Property(e => e.IdTipoArticulo).HasColumnName("idTipoArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdTipoArticuloNavigation)
                    .WithMany(p => p.ClaseArticulo)
                    .HasForeignKey(d => d.IdTipoArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoArticulo139");
            });

            modelBuilder.Entity<CodigoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdCodigoActivoFijo)
                    .HasName("PK90");

                entity.Property(e => e.IdCodigoActivoFijo).HasColumnName("idCodigoActivoFijo");

                entity.Property(e => e.CodigoBarras)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Codigosecuencial)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ComportamientoObservable>(entity =>
            {
                entity.HasIndex(e => e.DenominacionCompetenciaId)
                    .HasName("Ref203326");

                entity.HasIndex(e => e.NivelId)
                    .HasName("Ref205325");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.HasOne(d => d.DenominacionCompetencia)
                    .WithMany(p => p.ComportamientoObservable)
                    .HasForeignKey(d => d.DenominacionCompetenciaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefDenominacionCompetencia326");

                entity.HasOne(d => d.Nivel)
                    .WithMany(p => p.ComportamientoObservable)
                    .HasForeignKey(d => d.NivelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefNivel325");
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

            modelBuilder.Entity<ConfiguracionViatico>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracionViatico)
                    .HasName("PK255");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref51398");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.PorCientoAjustificar)
                    .HasColumnName("PorCientoAJustificar")
                    .HasColumnType("char(10)");

                entity.Property(e => e.ValorEntregadoPorDia).HasColumnType("decimal");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.ConfiguracionViatico)
                    .HasForeignKey(d => d.IdDependencia)
                    .HasConstraintName("RefDependencia398");
            });

            modelBuilder.Entity<ConfirmacionLectura>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15363");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.ConfirmacionLectura)
                    .HasForeignKey(d => d.IdEmpleado)
                    .HasConstraintName("RefEmpleado363");
            });

            modelBuilder.Entity<ConocimientosAdicionales>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<DatosBancarios>(entity =>
            {
                entity.HasKey(e => e.IdDatosBancarios)
                    .HasName("PK24");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref1534");

                entity.HasIndex(e => e.IdInstitucionFinanciera)
                    .HasName("Ref2333");

                entity.Property(e => e.IdDatosBancarios).HasColumnName("idDatosBancarios");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdInstitucionFinanciera).HasColumnName("idInstitucionFinanciera");

                entity.Property(e => e.NumeroCuenta)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.DatosBancarios)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado34");

                entity.HasOne(d => d.IdInstitucionFinancieraNavigation)
                    .WithMany(p => p.DatosBancarios)
                    .HasForeignKey(d => d.IdInstitucionFinanciera)
                    .HasConstraintName("RefInstitucionFinanciera33");
            });

            modelBuilder.Entity<DeclaracionPatrimonioPersonal>(entity =>
            {
                entity.HasKey(e => e.IdDeclaracionPatrimonioPersonal)
                    .HasName("PK106");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15152");

                entity.Property(e => e.IdDeclaracionPatrimonioPersonal).HasColumnName("idDeclaracionPatrimonioPersonal");

                entity.Property(e => e.FechaDeclaracion).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.PromedioMensualIngresos).HasColumnType("decimal");

                entity.Property(e => e.PromedioMensualOtrosIngresos).HasColumnType("decimal");

                entity.Property(e => e.TotalActivosAnioActual).HasColumnType("decimal");

                entity.Property(e => e.TotalActivosAnioAnterior).HasColumnType("decimal");

                entity.Property(e => e.TotalPasivosAnioActual).HasColumnType("decimal");

                entity.Property(e => e.TotalPasivosAnioAnterior).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.DeclaracionPatrimonioPersonal)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado152");
            });

            modelBuilder.Entity<DenominacionCompetencia>(entity =>
            {
                entity.Property(e => e.Definicion).HasColumnType("varchar(1000)");

                entity.Property(e => e.Nombre).HasColumnType("varchar(250)");
            });

            modelBuilder.Entity<Dependencia>(entity =>
            {
                entity.HasKey(e => e.IdDependencia)
                    .HasName("PK51");

                entity.HasIndex(e => e.IdDependenciaPadre)
                    .HasName("Ref5190");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("Ref18413");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.IdDependenciaPadre).HasColumnName("idDependenciaPadre");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdDependenciaPadreNavigation)
                    .WithMany(p => p.InverseIdDependenciaPadreNavigation)
                    .HasForeignKey(d => d.IdDependenciaPadre)
                    .HasConstraintName("RefDependencia90");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.Dependencia)
                    .HasForeignKey(d => d.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSucursal413");
            });

            modelBuilder.Entity<DependenciaDocumento>(entity =>
            {
                entity.HasIndex(e => e.DocumentosSubidos)
                    .HasName("Ref243372");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref51373");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.HasOne(d => d.DocumentosSubidosNavigation)
                    .WithMany(p => p.DependenciaDocumento)
                    .HasForeignKey(d => d.DocumentosSubidos)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefDocumentosCargados372");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.DependenciaDocumento)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefDependencia373");
            });

            modelBuilder.Entity<DepreciacionActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdDepreciacionActivoFijo)
                    .HasName("PK93");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("Ref89136");

                entity.Property(e => e.IdDepreciacionActivoFijo).HasColumnName("idDepreciacionActivoFijo");

                entity.Property(e => e.DepreciacionAcumulada).HasColumnType("decimal");

                entity.Property(e => e.FechaDepreciacion).HasColumnType("date");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.HasOne(d => d.IdActivoFijoNavigation)
                    .WithMany(p => p.DepreciacionActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefActivoFijo136");
            });

            modelBuilder.Entity<Destreza>(entity =>
            {
                entity.HasKey(e => e.IdDestreza)
                    .HasName("PK40");

                entity.Property(e => e.IdDestreza).HasColumnName("idDestreza");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<DetalleExamenInduccion>(entity =>
            {
                entity.HasIndex(e => e.PreguntaId)
                    .HasName("Ref232360");

                entity.HasIndex(e => e.RealizaExamenInduccionId)
                    .HasName("Ref235358");

                entity.HasIndex(e => e.RespuestaId)
                    .HasName("Ref233361");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.PreguntaId)
                    .HasConstraintName("RefPregunta360");

                entity.HasOne(d => d.RealizaExamenInduccion)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.RealizaExamenInduccionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRealizaExamenInduccion358");

                entity.HasOne(d => d.Respuesta)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.RespuestaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRespuesta361");
            });

            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.HasKey(e => e.IdDetalleFactura)
                    .HasName("PK142");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("Ref99255");

                entity.HasIndex(e => e.IdFactura)
                    .HasName("Ref141247");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.Precio).HasColumnType("decimal");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.DetalleFactura)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefArticulo255");

                entity.HasOne(d => d.IdFacturaNavigation)
                    .WithMany(p => p.DetalleFactura)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFactura247");
            });

            modelBuilder.Entity<DocumentoInformacionInstitucional>(entity =>
            {
                entity.HasKey(e => e.IdDocumentoInformacionInstitucional)
                    .HasName("PK105");

                entity.Property(e => e.IdDocumentoInformacionInstitucional).HasColumnName("idDocumentoInformacionInstitucional");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnType("varchar(1024)");
            });

            modelBuilder.Entity<DocumentosCargados>(entity =>
            {
                entity.HasKey(e => e.DocumentosSubidos)
                    .HasName("PK243");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15371");

                entity.Property(e => e.Are).HasColumnType("char(10)");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.FechaCaducidad).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.NombreArchivo).HasColumnType("text");

                entity.Property(e => e.Ubicacion).HasColumnType("text");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.DocumentosCargados)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado371");
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado)
                    .HasName("PK15");

                entity.HasIndex(e => e.IdCiudadLugarNacimiento)
                    .HasName("Ref349");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref51298");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref1728");

                entity.HasIndex(e => e.IdProvinciaLugarSufragio)
                    .HasName("Ref150");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.FechaIngreso).HasColumnType("date");

                entity.Property(e => e.FechaIngresoSectorPublico).HasColumnType("date");

                entity.Property(e => e.IdCiudadLugarNacimiento).HasColumnName("idCiudadLugarNacimiento");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdProvinciaLugarSufragio).HasColumnName("idProvinciaLugarSufragio");

                entity.Property(e => e.IngresosOtraActividad).HasColumnType("text");

                entity.HasOne(d => d.IdCiudadLugarNacimientoNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdCiudadLugarNacimiento)
                    .HasConstraintName("RefCiudad49");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefDependencia298");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("RefPersona28");

                entity.HasOne(d => d.IdProvinciaLugarSufragioNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdProvinciaLugarSufragio)
                    .HasConstraintName("RefProvincia50");
            });

            modelBuilder.Entity<EmpleadoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoActivoFijo)
                    .HasName("PK92");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("Ref89135");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15134");

                entity.Property(e => e.IdEmpleadoActivoFijo).HasColumnName("idEmpleadoActivoFijo");

                entity.Property(e => e.FechaAsignacion).HasColumnType("date");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdActivoFijoNavigation)
                    .WithMany(p => p.EmpleadoActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefActivoFijo135");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado134");
            });

            modelBuilder.Entity<EmpleadoContactoEmergencia>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleadoContactoEmergencia, e.IdPersona })
                    .HasName("PK25");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref1537");

                entity.HasIndex(e => e.IdParentesco)
                    .HasName("Ref2038");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref1735");

                entity.Property(e => e.IdEmpleadoContactoEmergencia)
                    .HasColumnName("idEmpleadoContactoEmergencia")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdParentesco).HasColumnName("idParentesco");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado37");

                entity.HasOne(d => d.IdParentescoNavigation)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdParentesco)
                    .HasConstraintName("RefParentesco38");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPersona35");
            });

            modelBuilder.Entity<EmpleadoFamiliar>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleadoFamiliar, e.IdPersona })
                    .HasName("PK19");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref1521");

                entity.HasIndex(e => e.IdParentesco)
                    .HasName("Ref2036");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref1720");

                entity.Property(e => e.IdEmpleadoFamiliar)
                    .HasColumnName("idEmpleadoFamiliar")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdParentesco).HasColumnName("idParentesco");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado21");

                entity.HasOne(d => d.IdParentescoNavigation)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdParentesco)
                    .HasConstraintName("RefParentesco36");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPersona20");
            });

            modelBuilder.Entity<EmpleadoFormularioCapacitacion>(entity =>
            {
                entity.HasIndex(e => e.AporbadoPor)
                    .HasName("Ref15369");

                entity.HasIndex(e => e.FormularioCapacitacionId)
                    .HasName("Ref238365");

                entity.HasIndex(e => e.IdServidor)
                    .HasName("Ref15366");

                entity.HasIndex(e => e.ResponsableArea)
                    .HasName("Ref15367");

                entity.HasIndex(e => e.RevisadoPor)
                    .HasName("Ref15368");

                entity.HasOne(d => d.AporbadoPorNavigation)
                    .WithMany(p => p.EmpleadoFormularioCapacitacionAporbadoPorNavigation)
                    .HasForeignKey(d => d.AporbadoPor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado369");

                entity.HasOne(d => d.FormularioCapacitacion)
                    .WithMany(p => p.EmpleadoFormularioCapacitacion)
                    .HasForeignKey(d => d.FormularioCapacitacionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFormularioCapacitacion365");

                entity.HasOne(d => d.IdServidorNavigation)
                    .WithMany(p => p.EmpleadoFormularioCapacitacionIdServidorNavigation)
                    .HasForeignKey(d => d.IdServidor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado366");

                entity.HasOne(d => d.ResponsableAreaNavigation)
                    .WithMany(p => p.EmpleadoFormularioCapacitacionResponsableAreaNavigation)
                    .HasForeignKey(d => d.ResponsableArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado367");

                entity.HasOne(d => d.RevisadoPorNavigation)
                    .WithMany(p => p.EmpleadoFormularioCapacitacionRevisadoPorNavigation)
                    .HasForeignKey(d => d.RevisadoPor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado368");
            });

            modelBuilder.Entity<EmpleadoIe>(entity =>
            {
                entity.HasKey(e => e.IdEmpeladoIe)
                    .HasName("PK153");

                entity.ToTable("EmpleadoIE");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15439");

                entity.HasIndex(e => e.IdIngresoEgresoRmu)
                    .HasName("Ref282434");

                entity.Property(e => e.IdEmpeladoIe).HasColumnName("idEmpeladoIE");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdIngresoEgresoRmu).HasColumnName("idIngresoEgresoRMU");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoIe)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado439");

                entity.HasOne(d => d.IdIngresoEgresoRmuNavigation)
                    .WithMany(p => p.EmpleadoIe)
                    .HasForeignKey(d => d.IdIngresoEgresoRmu)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIngresoEgresoRMU434");
            });

            modelBuilder.Entity<EmpleadoImpuestoRenta>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoImpuestoRenta)
                    .HasName("PK81");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15120");

                entity.Property(e => e.IdEmpleadoImpuestoRenta).HasColumnName("idEmpleadoImpuestoRenta");

                entity.Property(e => e.FechaDesde).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IngresoTotal).HasColumnType("decimal");

                entity.Property(e => e.OtrosIngresos).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoImpuestoRenta)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado120");
            });

            modelBuilder.Entity<EmpleadoMovimiento>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoMovimiento)
                    .HasName("PK126");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15414");

                entity.HasIndex(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("Ref71195");

                entity.HasIndex(e => e.IdTipoMovimientoInterno)
                    .HasName("Ref127193");

                entity.Property(e => e.IdEmpleadoMovimiento).HasColumnName("idEmpleadoMovimiento");

                entity.Property(e => e.FechaDesde).HasColumnType("date");

                entity.Property(e => e.FechaHasta).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdIndiceOcupacionalModalidadPartida).HasColumnName("idIndiceOcupacionalModalidadPartida");

                entity.Property(e => e.IdTipoMovimientoInterno).HasColumnName("idTipoMovimientoInterno");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado414");

                entity.HasOne(d => d.IdIndiceOcupacionalModalidadPartidaNavigation)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdIndiceOcupacionalModalidadPartida)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacionalModalidadPartida195");

                entity.HasOne(d => d.IdTipoMovimientoInternoNavigation)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdTipoMovimientoInterno)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoMovimientoInterno193");
            });

            modelBuilder.Entity<EmpleadoNepotismo>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoNepotismo)
                    .HasName("PK123");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15176");

                entity.HasIndex(e => e.IdEmpleadoFamiliar)
                    .HasName("Ref15177");

                entity.Property(e => e.IdEmpleadoNepotismo).HasColumnName("idEmpleadoNepotismo");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEmpleadoFamiliar).HasColumnName("idEmpleadoFamiliar");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoNepotismoIdEmpleadoNavigation)
                    .HasForeignKey(d => d.IdEmpleado)
                    .HasConstraintName("RefEmpleado176");

                entity.HasOne(d => d.IdEmpleadoFamiliarNavigation)
                    .WithMany(p => p.EmpleadoNepotismoIdEmpleadoFamiliarNavigation)
                    .HasForeignKey(d => d.IdEmpleadoFamiliar)
                    .HasConstraintName("RefEmpleado177");
            });

            modelBuilder.Entity<EmpleadoSaldoVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoSaldoVacaciones)
                    .HasName("PK74");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15112");

                entity.Property(e => e.IdEmpleadoSaldoVacaciones).HasColumnName("idEmpleadoSaldoVacaciones");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.SaldoDias).HasColumnType("decimal");

                entity.Property(e => e.SaldoMonetario).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoSaldoVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado112");
            });

            modelBuilder.Entity<EmpleadosFormularioDevengacion>(entity =>
            {
                entity.HasKey(e => e.EmpleadosFormularioDevengacionIs)
                    .HasName("PK246");

                entity.HasIndex(e => e.FormularioDevengacionId)
                    .HasName("Ref245376");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15377");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.FormularioDevengacion)
                    .WithMany(p => p.EmpleadosFormularioDevengacion)
                    .HasForeignKey(d => d.FormularioDevengacionId)
                    .HasConstraintName("RefFormularioDevengacion376");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadosFormularioDevengacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado377");
            });

            modelBuilder.Entity<EscalaEvaluacionTotal>(entity =>
            {
                entity.HasKey(e => e.IdEscalaEvaluacionTotal)
                    .HasName("PK50");

                entity.Property(e => e.IdEscalaEvaluacionTotal).HasColumnName("idEscalaEvaluacionTotal");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(1024)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PorcientoDesde).HasColumnType("decimal");

                entity.Property(e => e.PorcientoHasta).HasColumnType("decimal");
            });

            modelBuilder.Entity<EscalaGrados>(entity =>
            {
                entity.HasKey(e => e.IdEscalaGrados)
                    .HasName("PK60");

                entity.HasIndex(e => e.IdGrupoOcupacional)
                    .HasName("Ref6195");

                entity.Property(e => e.IdEscalaGrados).HasColumnName("idEscalaGrados");

                entity.Property(e => e.IdGrupoOcupacional).HasColumnName("idGrupoOcupacional");

                entity.Property(e => e.Remuneracion).HasColumnType("decimal");

                entity.HasOne(d => d.IdGrupoOcupacionalNavigation)
                    .WithMany(p => p.EscalaGrados)
                    .HasForeignKey(d => d.IdGrupoOcupacional)
                    .HasConstraintName("RefGrupoOcupacional95");
            });

            modelBuilder.Entity<EspecificidadExperiencia>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK73");

                entity.HasIndex(e => new { e.IdEstado, e.IdSolicitudCertificadoPersonal })
                    .HasName("Ref103220");

                entity.Property(e => e.IdEstado)
                    .HasColumnName("idEstado")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdSolicitudCertificadoPersonal).HasColumnName("idSolicitudCertificadoPersonal");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Id)
                    .WithMany(p => p.Estado)
                    .HasForeignKey(d => new { d.IdSolicitudCertificadoPersonal, d.IdEstado })
                    .HasConstraintName("RefSolicitudCertificadoPersonal220");
            });

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCivil)
                    .HasName("PK8");

                entity.Property(e => e.IdEstadoCivil).HasColumnName("idEstadoCivil");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Estudio>(entity =>
            {
                entity.HasKey(e => e.IdEstudio)
                    .HasName("PK16_1");

                entity.Property(e => e.IdEstudio).HasColumnName("idEstudio");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Etnia>(entity =>
            {
                entity.HasKey(e => e.IdEtnia)
                    .HasName("PK10");

                entity.Property(e => e.IdEtnia).HasColumnName("idEtnia");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<Eval001>(entity =>
            {
                entity.HasKey(e => e.IdEval001)
                    .HasName("PK49");

                entity.HasIndex(e => e.EvaluadorId)
                    .HasName("Ref182301");

                entity.HasIndex(e => e.IdEmpleadoEvaluado)
                    .HasName("Ref1576");

                entity.HasIndex(e => e.IdEscalaEvaluacionTotal)
                    .HasName("Ref5078");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("Ref3379");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("Ref3881");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("Ref4382");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("Ref3580");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("Ref4683");

                entity.Property(e => e.IdEval001).HasColumnName("idEval001");

                entity.Property(e => e.FechaEvaluacionDesde).HasColumnType("date");

                entity.Property(e => e.FechaEvaluacionHasta).HasColumnType("date");

                entity.Property(e => e.FechaRegistro).HasColumnType("date");

                entity.Property(e => e.IdEmpleadoEvaluado).HasColumnName("idEmpleadoEvaluado");

                entity.Property(e => e.IdEscalaEvaluacionTotal).HasColumnName("idEscalaEvaluacionTotal");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajo).HasColumnName("idEvaluacionActividadesPuestoTrabajo");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuesto).HasColumnName("idEvaluacionCompetenciasTecnicasPuesto");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversales).HasColumnName("idEvaluacionCompetenciasUniversales");

                entity.Property(e => e.IdEvaluacionConocimiento).HasColumnName("idEvaluacionConocimiento");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.HasOne(d => d.Evaluador)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.EvaluadorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluador301");

                entity.HasOne(d => d.IdEmpleadoEvaluadoNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEmpleadoEvaluado)
                    .HasConstraintName("RefEmpleado76");

                entity.HasOne(d => d.IdEscalaEvaluacionTotalNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEscalaEvaluacionTotal)
                    .HasConstraintName("RefEscalaEvaluacionTotal78");

                entity.HasOne(d => d.IdEvaluacionActividadesPuestoTrabajoNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo)
                    .HasConstraintName("RefEvaluacionActividadesPuestoTrabajo79");

                entity.HasOne(d => d.IdEvaluacionCompetenciasTecnicasPuestoNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasConstraintName("RefEvaluacionCompetenciasTecnicasPuesto81");

                entity.HasOne(d => d.IdEvaluacionCompetenciasUniversalesNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales)
                    .HasConstraintName("RefEvaluacionCompetenciasUniversales82");

                entity.HasOne(d => d.IdEvaluacionConocimientoNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento)
                    .HasConstraintName("RefEvaluacionConocimiento80");

                entity.HasOne(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgoNavigation)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasConstraintName("RefEvaluacionTrabajoEquipoIniciativaLiderazgo83");
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajo>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("PK33");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajo).HasColumnName("idEvaluacionActividadesPuestoTrabajo");

                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajoDetalle)
                    .HasName("PK34");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("Ref3356");

                entity.HasIndex(e => e.IdIndicador)
                    .HasName("Ref3157");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajoDetalle).HasColumnName("idEvaluacionActividadesPuestoTrabajoDetalle");

                entity.Property(e => e.DescripcionActividad)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajo).HasColumnName("idEvaluacionActividadesPuestoTrabajo");

                entity.Property(e => e.IdIndicador).HasColumnName("idIndicador");

                entity.HasOne(d => d.IdEvaluacionActividadesPuestoTrabajoNavigation)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionActividadesPuestoTrabajo56");

                entity.HasOne(d => d.IdIndicadorNavigation)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoDetalle)
                    .HasForeignKey(d => d.IdIndicador)
                    .HasConstraintName("RefIndicador57");
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajoFactor)
                    .HasName("PK128");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("Ref33207");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("Ref32206");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajoFactor).HasColumnName("idEvaluacionActividadesPuestoTrabajoFactor");

                entity.Property(e => e.IdEvaluacionActividadesPuestoTrabajo).HasColumnName("idEvaluacionActividadesPuestoTrabajo");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.HasOne(d => d.IdEvaluacionActividadesPuestoTrabajoNavigation)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoFactor)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo)
                    .HasConstraintName("RefEvaluacionActividadesPuestoTrabajo207");

                entity.HasOne(d => d.IdFactorNavigation)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .HasConstraintName("RefFactor206");
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuesto>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("PK38");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuesto).HasColumnName("idEvaluacionCompetenciasTecnicasPuesto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuestoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasName("PK39");

                entity.HasIndex(e => e.IdDestreza)
                    .HasName("Ref4063");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("Ref3862");

                entity.HasIndex(e => e.IdNivelDesarrollo)
                    .HasName("Ref4265");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("Ref4164");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuestoDetalle).HasColumnName("idEvaluacionCompetenciasTecnicasPuestoDetalle");

                entity.Property(e => e.IdDestreza).HasColumnName("idDestreza");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuesto).HasColumnName("idEvaluacionCompetenciasTecnicasPuesto");

                entity.Property(e => e.IdNivelDesarrollo).HasColumnName("idNivelDesarrollo");

                entity.Property(e => e.IdRelevancia).HasColumnName("idRelevancia");

                entity.HasOne(d => d.IdDestrezaNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdDestreza)
                    .HasConstraintName("RefDestreza63");

                entity.HasOne(d => d.IdEvaluacionCompetenciasTecnicasPuestoNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionCompetenciasTecnicasPuesto62");

                entity.HasOne(d => d.IdNivelDesarrolloNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdNivelDesarrollo)
                    .HasConstraintName("RefNivelDesarrollo65");

                entity.HasOne(d => d.IdRelevanciaNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdRelevancia)
                    .HasConstraintName("RefRelevancia64");
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuestoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasName("PK130");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("Ref38209");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("Ref32201");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuestoFactor).HasColumnName("idEvaluacionCompetenciasTecnicasPuestoFactor");

                entity.Property(e => e.IdEvaluacionCompetenciasTecnicasPuesto).HasColumnName("idEvaluacionCompetenciasTecnicasPuesto");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.HasOne(d => d.IdEvaluacionCompetenciasTecnicasPuestoNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasConstraintName("RefEvaluacionCompetenciasTecnicasPuesto209");

                entity.HasOne(d => d.IdFactorNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFactor201");
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversales>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("PK43");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversales).HasColumnName("idEvaluacionCompetenciasUniversales");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversalesDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversalesDetalle)
                    .HasName("PK44");

                entity.HasIndex(e => e.IdDestreza)
                    .HasName("Ref4068");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("Ref4367");

                entity.HasIndex(e => e.IdFrecuenciaAplicacion)
                    .HasName("Ref4570");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("Ref4169");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversalesDetalle).HasColumnName("idEvaluacionCompetenciasUniversalesDetalle");

                entity.Property(e => e.IdDestreza).HasColumnName("idDestreza");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversales).HasColumnName("idEvaluacionCompetenciasUniversales");

                entity.Property(e => e.IdFrecuenciaAplicacion).HasColumnName("idFrecuenciaAplicacion");

                entity.Property(e => e.IdRelevancia).HasColumnName("idRelevancia");

                entity.HasOne(d => d.IdDestrezaNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdDestreza)
                    .HasConstraintName("RefDestreza68");

                entity.HasOne(d => d.IdEvaluacionCompetenciasUniversalesNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionCompetenciasUniversales67");

                entity.HasOne(d => d.IdFrecuenciaAplicacionNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdFrecuenciaAplicacion)
                    .HasConstraintName("RefFrecuenciaAplicacion70");

                entity.HasOne(d => d.IdRelevanciaNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdRelevancia)
                    .HasConstraintName("RefRelevancia69");
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversalesFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversalesFactor)
                    .HasName("PK131");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("Ref43210");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("Ref32203");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversalesFactor).HasColumnName("idEvaluacionCompetenciasUniversalesFactor");

                entity.Property(e => e.IdEvaluacionCompetenciasUniversales).HasColumnName("idEvaluacionCompetenciasUniversales");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.HasOne(d => d.IdEvaluacionCompetenciasUniversalesNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesFactor)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales)
                    .HasConstraintName("RefEvaluacionCompetenciasUniversales210");

                entity.HasOne(d => d.IdFactorNavigation)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFactor203");
            });

            modelBuilder.Entity<EvaluacionConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimiento)
                    .HasName("PK35");

                entity.Property(e => e.IdEvaluacionConocimiento).HasColumnName("idEvaluacionConocimiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<EvaluacionConocimientoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimientoDetalle)
                    .HasName("PK36");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("Ref3558");

                entity.HasIndex(e => e.IdNivelConocimiento)
                    .HasName("Ref3759");

                entity.Property(e => e.IdEvaluacionConocimientoDetalle).HasColumnName("idEvaluacionConocimientoDetalle");

                entity.Property(e => e.IdEvaluacionConocimiento).HasColumnName("idEvaluacionConocimiento");

                entity.Property(e => e.IdNivelConocimiento).HasColumnName("idNivelConocimiento");

                entity.HasOne(d => d.IdEvaluacionConocimientoNavigation)
                    .WithMany(p => p.EvaluacionConocimientoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionConocimiento58");

                entity.HasOne(d => d.IdNivelConocimientoNavigation)
                    .WithMany(p => p.EvaluacionConocimientoDetalle)
                    .HasForeignKey(d => d.IdNivelConocimiento)
                    .HasConstraintName("RefNivelConocimiento59");
            });

            modelBuilder.Entity<EvaluacionConocimientoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimientoFactor)
                    .HasName("PK129");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("Ref35208");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("Ref32199");

                entity.Property(e => e.IdEvaluacionConocimientoFactor).HasColumnName("idEvaluacionConocimientoFactor");

                entity.Property(e => e.IdEvaluacionConocimiento).HasColumnName("idEvaluacionConocimiento");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.HasOne(d => d.IdEvaluacionConocimientoNavigation)
                    .WithMany(p => p.EvaluacionConocimientoFactor)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento)
                    .HasConstraintName("RefEvaluacionConocimiento208");

                entity.HasOne(d => d.IdFactorNavigation)
                    .WithMany(p => p.EvaluacionConocimientoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFactor199");
            });

            modelBuilder.Entity<EvaluacionInducion>(entity =>
            {
                entity.HasKey(e => e.EvaluacionInduccinId)
                    .HasName("PK231");

                entity.Property(e => e.MinimoAprobar).HasColumnName("MInimoAprobar");

                entity.Property(e => e.Nombre).HasColumnType("char(10)");
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgo>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("PK46");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ObservacionesJefeInmediato).HasColumnType("text");
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasName("PK47");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("Ref4671");

                entity.HasIndex(e => e.IdFrecuenciaAplicacion)
                    .HasName("Ref4574");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("Ref4173");

                entity.HasIndex(e => e.IdTrabajoEquipoIniciativaLiderazgo)
                    .HasName("Ref4872");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgoDetalle).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgoDetalle");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.IdFrecuenciaAplicacion).HasColumnName("idFrecuenciaAplicacion");

                entity.Property(e => e.IdRelevancia).HasColumnName("idRelevancia");

                entity.Property(e => e.IdTrabajoEquipoIniciativaLiderazgo).HasColumnName("idTrabajoEquipoIniciativaLiderazgo");

                entity.HasOne(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgoNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasConstraintName("RefEvaluacionTrabajoEquipoIniciativaLiderazgo71");

                entity.HasOne(d => d.IdFrecuenciaAplicacionNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdFrecuenciaAplicacion)
                    .HasConstraintName("RefFrecuenciaAplicacion74");

                entity.HasOne(d => d.IdRelevanciaNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdRelevancia)
                    .HasConstraintName("RefRelevancia73");

                entity.HasOne(d => d.IdTrabajoEquipoIniciativaLiderazgoNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdTrabajoEquipoIniciativaLiderazgo)
                    .HasConstraintName("RefTrabajoEquipoIniciativaLiderazgo72");
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgoFactor)
                    .HasName("PK132");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("Ref46211");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("Ref32205");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgoFactor).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgoFactor");

                entity.Property(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo).HasColumnName("idEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.HasOne(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgoNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoFactor)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasConstraintName("RefEvaluacionTrabajoEquipoIniciativaLiderazgo211");

                entity.HasOne(d => d.IdFactorNavigation)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFactor205");
            });

            modelBuilder.Entity<Evaluador>(entity =>
            {
                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref51299");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15300");

                entity.Property(e => e.EvaluadorId).ValueGeneratedNever();

                entity.Property(e => e.Ano).HasColumnType("date");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.Evaluador)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefDependencia299");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Evaluador)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado300");
            });

            modelBuilder.Entity<Exepciones>(entity =>
            {
                entity.HasIndex(e => e.ValidacionJefeId)
                    .HasName("Ref193324");

                entity.Property(e => e.Detalle).HasColumnType("varchar(250)");

                entity.HasOne(d => d.ValidacionJefe)
                    .WithMany(p => p.Exepciones)
                    .HasForeignKey(d => d.ValidacionJefeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefValidacionInmediatoSuperior324");
            });

            modelBuilder.Entity<ExperienciaLaboralRequerida>(entity =>
            {
                entity.HasIndex(e => e.AnoExperienciaId)
                    .HasName("Ref228351");

                entity.HasIndex(e => e.EspecificidadExperienciaId)
                    .HasName("Ref229350");

                entity.HasIndex(e => e.IdEstudio)
                    .HasName("Ref214352");

                entity.HasIndex(e => e.IndiceOcupacionalCapacitaciones)
                    .HasName("Ref226349");

                entity.Property(e => e.IdEstudio).HasColumnName("idEstudio");

                entity.HasOne(d => d.AnoExperiencia)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.AnoExperienciaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefAnoExperiencia351");

                entity.HasOne(d => d.EspecificidadExperiencia)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.EspecificidadExperienciaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEspecificidadExperiencia350");

                entity.HasOne(d => d.IdEstudioNavigation)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEstudio352");

                entity.HasOne(d => d.IndiceOcupacionalCapacitacionesNavigation)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IndiceOcupacionalCapacitaciones)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacionalCapacitaciones349");
            });

            modelBuilder.Entity<Factor>(entity =>
            {
                entity.HasKey(e => e.IdFactor)
                    .HasName("PK32");

                entity.Property(e => e.IdFactor).HasColumnName("idFactor");

                entity.Property(e => e.Porciento).HasColumnType("decimal");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.IdFactura)
                    .HasName("PK141");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("Ref100246");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("Ref113245");

                entity.Property(e => e.IdMaestroArticuloSucursal).HasColumnName("idMaestroArticuloSucursal");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.Property(e => e.Numerro).HasColumnType("varchar(30)");

                entity.HasOne(d => d.IdMaestroArticuloSucursalNavigation)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMaestroArticuloSucursal246");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefProveedor245");
            });

            modelBuilder.Entity<FacturaViatico>(entity =>
            {
                entity.HasKey(e => e.IdFacturaViatico)
                    .HasName("PK253");

                entity.HasIndex(e => e.AprobadoPor)
                    .HasName("Ref15397");

                entity.HasIndex(e => e.IdItemViatico)
                    .HasName("Ref252388");

                entity.HasIndex(e => e.ItinerarioViaticoId)
                    .HasName("Ref251389");

                entity.Property(e => e.FechaAprobacion).HasColumnType("date");

                entity.Property(e => e.FechaFactura).HasColumnType("date");

                entity.Property(e => e.NumeroFactura)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.Property(e => e.ValorTotalAprobacion).HasColumnType("date");

                entity.Property(e => e.ValorTotalFactura).HasColumnType("decimal");

                entity.HasOne(d => d.AprobadoPorNavigation)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.AprobadoPor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado397");

                entity.HasOne(d => d.IdItemViaticoNavigation)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.IdItemViatico)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefItemViatico388");

                entity.HasOne(d => d.ItinerarioViatico)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.ItinerarioViaticoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefItinerarioViatico389");
            });

            modelBuilder.Entity<FaseConcurso>(entity =>
            {
                entity.HasKey(e => e.IdFaseConcurso)
                    .HasName("PK272");

                entity.HasIndex(e => e.IdTipoConsurso)
                    .HasName("Ref268428");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.FechaFin).HasColumnType("date");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.Nomvre)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.IdTipoConsursoNavigation)
                    .WithMany(p => p.FaseConcurso)
                    .HasForeignKey(d => d.IdTipoConsurso)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoConsurso428");
            });

            modelBuilder.Entity<FondoFinanciamiento>(entity =>
            {
                entity.HasKey(e => e.IdFondoFinanciamiento)
                    .HasName("PK59");

                entity.Property(e => e.IdFondoFinanciamiento).HasColumnName("idFondoFinanciamiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FormularioAnalisisOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("PK107");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15153");

                entity.Property(e => e.IdFormularioAnalisisOcupacional).HasColumnName("idFormularioAnalisisOcupacional");

                entity.Property(e => e.FechaRegistro).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.MisionPuesto)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.FormularioAnalisisOcupacional)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado153");
            });

            modelBuilder.Entity<FormularioCapacitacion>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<FormularioDevengacion>(entity =>
            {
                entity.HasIndex(e => e.AnalistaDesarrolloInstitucional)
                    .HasName("Ref15381");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15379");

                entity.HasIndex(e => e.ModosScializacionId)
                    .HasName("Ref247375");

                entity.HasIndex(e => e.ResponsableArea)
                    .HasName("Ref15380");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.ModoSociali).HasColumnType("char(10)");

                entity.HasOne(d => d.AnalistaDesarrolloInstitucionalNavigation)
                    .WithMany(p => p.FormularioDevengacionAnalistaDesarrolloInstitucionalNavigation)
                    .HasForeignKey(d => d.AnalistaDesarrolloInstitucional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado381");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.FormularioDevengacionIdEmpleadoNavigation)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado379");

                entity.HasOne(d => d.ModosScializacion)
                    .WithMany(p => p.FormularioDevengacion)
                    .HasForeignKey(d => d.ModosScializacionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefModosScializacion375");

                entity.HasOne(d => d.ResponsableAreaNavigation)
                    .WithMany(p => p.FormularioDevengacionResponsableAreaNavigation)
                    .HasForeignKey(d => d.ResponsableArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado380");
            });

            modelBuilder.Entity<FormulasRmu>(entity =>
            {
                entity.HasKey(e => e.IdFormulaRmu)
                    .HasName("PK180");

                entity.ToTable("FormulasRMU");

                entity.Property(e => e.IdFormulaRmu)
                    .HasColumnName("idFormulaRMU")
                    .HasColumnType("char(10)");

                entity.Property(e => e.Formula).HasColumnType("varchar(80)");
            });

            modelBuilder.Entity<FrecuenciaAplicacion>(entity =>
            {
                entity.HasKey(e => e.IdFrecuenciaAplicacion)
                    .HasName("PK45");

                entity.Property(e => e.IdFrecuenciaAplicacion).HasColumnName("idFrecuenciaAplicacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<GastoRubro>(entity =>
            {
                entity.HasKey(e => e.IdGastoRubro)
                    .HasName("PK82");

                entity.HasIndex(e => e.IdEmpleadoImpuestoRenta)
                    .HasName("Ref81121");

                entity.HasIndex(e => e.IdRubro)
                    .HasName("Ref83122");

                entity.Property(e => e.IdGastoRubro).HasColumnName("idGastoRubro");

                entity.Property(e => e.GastoProyectado).HasColumnType("decimal");

                entity.Property(e => e.IdEmpleadoImpuestoRenta).HasColumnName("idEmpleadoImpuestoRenta");

                entity.Property(e => e.IdRubro).HasColumnName("idRubro");

                entity.HasOne(d => d.IdEmpleadoImpuestoRentaNavigation)
                    .WithMany(p => p.GastoRubro)
                    .HasForeignKey(d => d.IdEmpleadoImpuestoRenta)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleadoImpuestoRenta121");

                entity.HasOne(d => d.IdRubroNavigation)
                    .WithMany(p => p.GastoRubro)
                    .HasForeignKey(d => d.IdRubro)
                    .HasConstraintName("RefRubro122");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero)
                    .HasName("PK7");

                entity.Property(e => e.IdGenero).HasColumnName("idGenero");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<GrupoOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdGrupoOcupacional)
                    .HasName("PK61");

                entity.Property(e => e.IdGrupoOcupacional).HasColumnName("idGrupoOcupacional");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ImpuestoRentaParametros>(entity =>
            {
                entity.HasKey(e => e.IdImpuestoRentaParametros)
                    .HasName("PK80");

                entity.Property(e => e.IdImpuestoRentaParametros).HasColumnName("idImpuestoRentaParametros");

                entity.Property(e => e.ExcesoHasta).HasColumnType("decimal");

                entity.Property(e => e.FraccionBasica).HasColumnType("decimal");
            });

            modelBuilder.Entity<Indicador>(entity =>
            {
                entity.HasKey(e => e.IdIndicador)
                    .HasName("PK31");

                entity.Property(e => e.IdIndicador).HasColumnName("idIndicador");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<IndiceOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacional)
                    .HasName("PK69");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref5198");

                entity.HasIndex(e => e.IdEscalaGrados)
                    .HasName("Ref60101");

                entity.HasIndex(e => e.IdManualPuesto)
                    .HasName("Ref7099");

                entity.HasIndex(e => e.IdRolPuesto)
                    .HasName("Ref63100");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.IdEscalaGrados).HasColumnName("idEscalaGrados");

                entity.Property(e => e.IdManualPuesto).HasColumnName("idManualPuesto");

                entity.Property(e => e.IdRolPuesto).HasColumnName("idRolPuesto");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdDependencia)
                    .HasConstraintName("RefDependencia98");

                entity.HasOne(d => d.IdEscalaGradosNavigation)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdEscalaGrados)
                    .HasConstraintName("RefEscalaGrados101");

                entity.HasOne(d => d.IdManualPuestoNavigation)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdManualPuesto)
                    .HasConstraintName("RefManualPuesto99");

                entity.HasOne(d => d.IdRolPuestoNavigation)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdRolPuesto)
                    .HasConstraintName("RefRolPuesto100");
            });

            modelBuilder.Entity<IndiceOcupacionalActividadesEsenciales>(entity =>
            {
                entity.HasIndex(e => e.ActividadesEsencialesId)
                    .HasName("Ref221341");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69344");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.ActividadesEsenciales)
                    .WithMany(p => p.IndiceOcupacionalActividadesEsenciales)
                    .HasForeignKey(d => d.ActividadesEsencialesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefActividadesEsenciales341");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalActividadesEsenciales)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional344");
            });

            modelBuilder.Entity<IndiceOcupacionalAreaConocimiento>(entity =>
            {
                entity.HasIndex(e => e.AreaConocimientoId)
                    .HasName("Ref219339");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69340");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.AreaConocimiento)
                    .WithMany(p => p.IndiceOcupacionalAreaConocimiento)
                    .HasForeignKey(d => d.AreaConocimientoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefAreaConocimiento339");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalAreaConocimiento)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional340");
            });

            modelBuilder.Entity<IndiceOcupacionalCapacitaciones>(entity =>
            {
                entity.HasKey(e => e.IndiceOcupacionalCapacitaciones1)
                    .HasName("PK226");

                entity.HasIndex(e => e.IdCapacitacion)
                    .HasName("Ref213346");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69345");

                entity.Property(e => e.IndiceOcupacionalCapacitaciones1).HasColumnName("IndiceOcupacionalCapacitaciones");

                entity.Property(e => e.IdCapacitacion).HasColumnName("idCapacitacion");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.IdCapacitacionNavigation)
                    .WithMany(p => p.IndiceOcupacionalCapacitaciones)
                    .HasForeignKey(d => d.IdCapacitacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacion346");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalCapacitaciones)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional345");
            });

            modelBuilder.Entity<IndiceOcupacionalComportamientoObservable>(entity =>
            {
                entity.HasKey(e => e.IndiceOcupacionalComportamientoObservable1)
                    .HasName("PK227");

                entity.HasIndex(e => e.ComportamientoObservableId)
                    .HasName("Ref204347");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69348");

                entity.Property(e => e.IndiceOcupacionalComportamientoObservable1).HasColumnName("IndiceOcupacionalComportamientoObservable");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.ComportamientoObservable)
                    .WithMany(p => p.IndiceOcupacionalComportamientoObservable)
                    .HasForeignKey(d => d.ComportamientoObservableId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefComportamientoObservable347");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalComportamientoObservable)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional348");
            });

            modelBuilder.Entity<IndiceOcupacionalConocimientosAdicionales>(entity =>
            {
                entity.HasIndex(e => e.ConocimientosAdicionalesId)
                    .HasName("Ref222342");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69343");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.ConocimientosAdicionales)
                    .WithMany(p => p.IndiceOcupacionalConocimientosAdicionales)
                    .HasForeignKey(d => d.ConocimientosAdicionalesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefConocimientosAdicionales342");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalConocimientosAdicionales)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional343");
            });

            modelBuilder.Entity<IndiceOcupacionalEstudio>(entity =>
            {
                entity.HasIndex(e => e.IdEstudio)
                    .HasName("Ref214338");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69337");

                entity.Property(e => e.IdEstudio).HasColumnName("idEstudio");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.IdEstudioNavigation)
                    .WithMany(p => p.IndiceOcupacionalEstudio)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEstudio338");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalEstudio)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional337");
            });

            modelBuilder.Entity<IndiceOcupacionalModalidadPartida>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("PK71");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15189");

                entity.HasIndex(e => e.IdFondoFinanciamiento)
                    .HasName("Ref59104");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69103");

                entity.HasIndex(e => e.IdModalidadPartida)
                    .HasName("Ref62105");

                entity.HasIndex(e => e.IdTipoNombramiento)
                    .HasName("Ref58106");

                entity.Property(e => e.IdIndiceOcupacionalModalidadPartida).HasColumnName("idIndiceOcupacionalModalidadPartida");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdFondoFinanciamiento).HasColumnName("idFondoFinanciamiento");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.Property(e => e.IdModalidadPartida).HasColumnName("idModalidadPartida");

                entity.Property(e => e.IdTipoNombramiento).HasColumnName("idTipoNombramiento");

                entity.Property(e => e.SalarioReal).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado189");

                entity.HasOne(d => d.IdFondoFinanciamientoNavigation)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdFondoFinanciamiento)
                    .HasConstraintName("RefFondoFinanciamiento104");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional103");

                entity.HasOne(d => d.IdModalidadPartidaNavigation)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdModalidadPartida)
                    .HasConstraintName("RefModalidadPartida105");

                entity.HasOne(d => d.IdTipoNombramientoNavigation)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdTipoNombramiento)
                    .HasConstraintName("RefTipoNombramiento106");
            });

            modelBuilder.Entity<InformeUath>(entity =>
            {
                entity.ToTable("InformeUATH");

                entity.HasIndex(e => e.AdministracionTalentoHumanoId)
                    .HasName("Ref198321");

                entity.HasIndex(e => e.ManualPuestoDestino)
                    .HasName("Ref70319");

                entity.HasIndex(e => e.ManualPuestoOrigen)
                    .HasName("Ref70318");

                entity.Property(e => e.InformeUathid).HasColumnName("InformeUATHId");

                entity.HasOne(d => d.AdministracionTalentoHumano)
                    .WithMany(p => p.InformeUath)
                    .HasForeignKey(d => d.AdministracionTalentoHumanoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefAdministracionTalentoHumano321");

                entity.HasOne(d => d.ManualPuestoDestinoNavigation)
                    .WithMany(p => p.InformeUathManualPuestoDestinoNavigation)
                    .HasForeignKey(d => d.ManualPuestoDestino)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefManualPuesto319");

                entity.HasOne(d => d.ManualPuestoOrigenNavigation)
                    .WithMany(p => p.InformeUathManualPuestoOrigenNavigation)
                    .HasForeignKey(d => d.ManualPuestoOrigen)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefManualPuesto318");
            });

            modelBuilder.Entity<InformeViatico>(entity =>
            {
                entity.HasKey(e => e.IdInformeViatico)
                    .HasName("PK257");

                entity.HasIndex(e => e.ItinerarioViaticoId)
                    .HasName("Ref251396");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.HasOne(d => d.ItinerarioViatico)
                    .WithMany(p => p.InformeViatico)
                    .HasForeignKey(d => d.ItinerarioViaticoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefItinerarioViatico396");
            });

            modelBuilder.Entity<IngresoEgresoRmu>(entity =>
            {
                entity.HasKey(e => e.IdIngresoEgresoRmu)
                    .HasName("PK152");

                entity.ToTable("IngresoEgresoRMU");

                entity.HasIndex(e => e.IdFormulaRmu)
                    .HasName("Ref280437");

                entity.Property(e => e.IdIngresoEgresoRmu).HasColumnName("idIngresoEgresoRMU");

                entity.Property(e => e.CuentaContable)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IdFormulaRmu)
                    .HasColumnName("idFormulaRMU")
                    .HasColumnType("char(10)");

                entity.HasOne(d => d.IdFormulaRmuNavigation)
                    .WithMany(p => p.IngresoEgresoRmu)
                    .HasForeignKey(d => d.IdFormulaRmu)
                    .HasConstraintName("RefFormulasRMU437");
            });

            modelBuilder.Entity<InstitucionFinanciera>(entity =>
            {
                entity.HasKey(e => e.IdInstitucionFinanciera)
                    .HasName("PK23");

                entity.Property(e => e.IdInstitucionFinanciera).HasColumnName("idInstitucionFinanciera");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<InstruccionFormal>(entity =>
            {
                entity.HasKey(e => e.IdInstruccionFormal)
                    .HasName("PK64");

                entity.Property(e => e.IdInstruccionFormal).HasColumnName("idInstruccionFormal");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ItemViatico>(entity =>
            {
                entity.HasKey(e => e.IdItemViatico)
                    .HasName("PK252");

                entity.Property(e => e.Descipcion).HasColumnType("char(10)");
            });

            modelBuilder.Entity<ItinerarioViatico>(entity =>
            {
                entity.HasIndex(e => e.IdCiudad)
                    .HasName("Ref3386");

                entity.HasIndex(e => e.IdPais)
                    .HasName("Ref2387");

                entity.HasIndex(e => e.IdSolicitudViatico)
                    .HasName("Ref77384");

                entity.HasIndex(e => e.TipoTransporteId)
                    .HasName("Ref249385");

                entity.Property(e => e.Descripcion).HasColumnType("char(10)");

                entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.IdSolicitudViatico).HasColumnName("idSolicitudViatico");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCiudad386");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPais387");

                entity.HasOne(d => d.IdSolicitudViaticoNavigation)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdSolicitudViatico)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSolicitudViatico384");

                entity.HasOne(d => d.TipoTransporte)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.TipoTransporteId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoTransporte385");
            });

            modelBuilder.Entity<LibroActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdLibroActivoFijo)
                    .HasName("PK91");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("Ref18133");

                entity.Property(e => e.IdLibroActivoFijo).HasColumnName("idLibroActivoFijo");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.LibroActivoFijo)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("RefSucursal133");
            });

            modelBuilder.Entity<Liquidacion>(entity =>
            {
                entity.HasKey(e => e.IdLiquidacion)
                    .HasName("PK182_1");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15465");

                entity.HasIndex(e => e.IdRubroLiquidacion)
                    .HasName("Ref308464");

                entity.Property(e => e.IdLiquidacion)
                    .HasColumnName("idLiquidacion")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdRubroLiquidacion).HasColumnName("idRubroLiquidacion");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Liquidacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado465");

                entity.HasOne(d => d.IdRubroLiquidacionNavigation)
                    .WithMany(p => p.Liquidacion)
                    .HasForeignKey(d => d.IdRubroLiquidacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRubroLiquidacion464");
            });

            modelBuilder.Entity<MaestroArticuloSucursal>(entity =>
            {
                entity.HasKey(e => e.IdMaestroArticuloSucursal)
                    .HasName("PK100");

                entity.HasIndex(e => e.IdSucursal)
                    .HasName("Ref18143");

                entity.Property(e => e.IdMaestroArticuloSucursal).HasColumnName("idMaestroArticuloSucursal");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.MaestroArticuloSucursal)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("RefSucursal143");
            });

            modelBuilder.Entity<MaestroDetalleArticulo>(entity =>
            {
                entity.HasKey(e => e.IdMaestroDetalleArticulo)
                    .HasName("PK138");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("Ref99226");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("Ref100225");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdMaestroArticuloSucursal).HasColumnName("idMaestroArticuloSucursal");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.MaestroDetalleArticulo)
                    .HasForeignKey(d => d.IdArticulo)
                    .HasConstraintName("RefArticulo226");

                entity.HasOne(d => d.IdMaestroArticuloSucursalNavigation)
                    .WithMany(p => p.MaestroDetalleArticulo)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .HasConstraintName("RefMaestroArticuloSucursal225");
            });

            modelBuilder.Entity<MantenimientoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdMantenimientoActivoFijo)
                    .HasName("PK94");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("Ref89138");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15137");

                entity.Property(e => e.IdMantenimientoActivoFijo).HasColumnName("idMantenimientoActivoFijo");

                entity.Property(e => e.FechaDesde).HasColumnType("date");

                entity.Property(e => e.FechaHasta).HasColumnType("date");

                entity.Property(e => e.FechaMantenimiento).HasColumnType("date");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdActivoFijoNavigation)
                    .WithMany(p => p.MantenimientoActivoFijo)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefActivoFijo138");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.MantenimientoActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado137");
            });

            modelBuilder.Entity<ManualPuesto>(entity =>
            {
                entity.HasKey(e => e.IdManualPuesto)
                    .HasName("PK70");

                entity.Property(e => e.IdManualPuesto).HasColumnName("idManualPuesto");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK136");

                entity.Property(e => e.IdMarca).HasColumnName("idMarca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<MaterialApoyo>(entity =>
            {
                entity.HasIndex(e => e.FormularioDevengacionId)
                    .HasName("Ref245378");

                entity.Property(e => e.NombreDocumento).HasColumnType("text");

                entity.Property(e => e.Ubicacion).HasColumnType("text");

                entity.HasOne(d => d.FormularioDevengacion)
                    .WithMany(p => p.MaterialApoyo)
                    .HasForeignKey(d => d.FormularioDevengacionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFormularioDevengacion378");
            });

            modelBuilder.Entity<Mision>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<MisionIndiceOcupacional>(entity =>
            {
                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69328");

                entity.HasIndex(e => e.MisionId)
                    .HasName("Ref209327");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.MisionIndiceOcupacional)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional328");

                entity.HasOne(d => d.Mision)
                    .WithMany(p => p.MisionIndiceOcupacional)
                    .HasForeignKey(d => d.MisionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMision327");
            });

            modelBuilder.Entity<ModalidadPartida>(entity =>
            {
                entity.HasKey(e => e.IdModalidadPartida)
                    .HasName("PK62");

                entity.HasIndex(e => e.IdRelacionLaboral)
                    .HasName("Ref5797");

                entity.Property(e => e.IdModalidadPartida).HasColumnName("idModalidadPartida");

                entity.Property(e => e.IdRelacionLaboral).HasColumnName("idRelacionLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdRelacionLaboralNavigation)
                    .WithMany(p => p.ModalidadPartida)
                    .HasForeignKey(d => d.IdRelacionLaboral)
                    .HasConstraintName("RefRelacionLaboral97");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.HasKey(e => e.IdModelo)
                    .HasName("PK135");

                entity.HasIndex(e => e.IdMarca)
                    .HasName("Ref136218");

                entity.Property(e => e.IdModelo).HasColumnName("idModelo");

                entity.Property(e => e.IdMarca).HasColumnName("idMarca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Modelo)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMarca218");
            });

            modelBuilder.Entity<ModosScializacion>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("varchar(50)");
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

            modelBuilder.Entity<MotivoRecepcion>(entity =>
            {
                entity.HasKey(e => e.IdMotivoRecepcion)
                    .HasName("PK114");

                entity.Property(e => e.IdMotivoRecepcion).HasColumnName("idMotivoRecepcion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidad)
                    .HasName("PK26");

                entity.Property(e => e.IdNacionalidad).HasColumnName("idNacionalidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<NacionalidadIndigena>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidadIndigena)
                    .HasName("PK11");

                entity.HasIndex(e => e.IdEtnia)
                    .HasName("Ref106");

                entity.Property(e => e.IdNacionalidadIndigena).HasColumnName("idNacionalidadIndigena");

                entity.Property(e => e.IdEtnia).HasColumnName("idEtnia");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdEtniaNavigation)
                    .WithMany(p => p.NacionalidadIndigena)
                    .HasForeignKey(d => d.IdEtnia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEtnia6");
            });

            modelBuilder.Entity<Nivel>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<NivelConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdNivelConocimiento)
                    .HasName("PK37");

                entity.Property(e => e.IdNivelConocimiento).HasColumnName("idNivelConocimiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<NivelDesarrollo>(entity =>
            {
                entity.HasKey(e => e.IdNivelDesarrollo)
                    .HasName("PK42");

                entity.Property(e => e.IdNivelDesarrollo).HasColumnName("idNivelDesarrollo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais)
                    .HasName("PK2");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PaquetesInformaticos>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("varchar(50)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ParametrosGenerales>(entity =>
            {
                entity.HasKey(e => e.IdParametrosGenerales)
                    .HasName("PK119");

                entity.Property(e => e.IdParametrosGenerales).HasColumnName("idParametrosGenerales");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Parentesco>(entity =>
            {
                entity.HasKey(e => e.IdParentesco)
                    .HasName("PK20");

                entity.Property(e => e.IdParentesco).HasColumnName("idParentesco");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Parroquia>(entity =>
            {
                entity.HasKey(e => e.IdParish)
                    .HasName("PK5");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("Ref3174");

                entity.Property(e => e.IdParish).HasColumnName("idParish");

                entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.Parroquia)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCiudad174");
            });

            modelBuilder.Entity<PartidasFase>(entity =>
            {
                entity.HasKey(e => e.IdPartidasFase)
                    .HasName("PK273");

                entity.HasIndex(e => e.IdFaseConcurso)
                    .HasName("Ref272427");

                entity.HasIndex(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("Ref71424");

                entity.Property(e => e.IdIndiceOcupacionalModalidadPartida).HasColumnName("idIndiceOcupacionalModalidadPartida");

                entity.HasOne(d => d.IdFaseConcursoNavigation)
                    .WithMany(p => p.PartidasFase)
                    .HasForeignKey(d => d.IdFaseConcurso)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFaseConcurso427");

                entity.HasOne(d => d.IdIndiceOcupacionalModalidadPartidaNavigation)
                    .WithMany(p => p.PartidasFase)
                    .HasForeignKey(d => d.IdIndiceOcupacionalModalidadPartida)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacionalModalidadPartida424");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PK134");

                entity.HasIndex(e => e.IdTipoPermiso)
                    .HasName("Ref133215");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.IdTipoPermiso).HasColumnName("idTipoPermiso");

                entity.HasOne(d => d.IdTipoPermisoNavigation)
                    .WithMany(p => p.Permiso)
                    .HasForeignKey(d => d.IdTipoPermiso)
                    .HasConstraintName("RefTipoPermiso215");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK17");

                entity.HasIndex(e => e.IdCanditato)
                    .HasName("Ref267421");

                entity.HasIndex(e => e.IdEstadoCivil)
                    .HasName("Ref8181");

                entity.HasIndex(e => e.IdEtnia)
                    .HasName("Ref10182");

                entity.HasIndex(e => e.IdGenero)
                    .HasName("Ref740");

                entity.HasIndex(e => e.IdNacionalidad)
                    .HasName("Ref2643");

                entity.HasIndex(e => e.IdSexo)
                    .HasName("Ref639");

                entity.HasIndex(e => e.IdTipoIdentificacion)
                    .HasName("Ref124179");

                entity.HasIndex(e => e.IdTipoSangre)
                    .HasName("Ref9180");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CorreoPrivado).HasColumnType("varchar(100)");

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.IdEstadoCivil).HasColumnName("idEstadoCivil");

                entity.Property(e => e.IdEtnia).HasColumnName("idEtnia");

                entity.Property(e => e.IdGenero).HasColumnName("idGenero");

                entity.Property(e => e.IdNacionalidad).HasColumnName("idNacionalidad");

                entity.Property(e => e.IdSexo).HasColumnName("idSexo");

                entity.Property(e => e.IdTipoIdentificacion).HasColumnName("idTipoIdentificacion");

                entity.Property(e => e.IdTipoSangre).HasColumnName("idTipoSangre");

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.LugarTrabajo).HasColumnType("varchar(100)");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TelefonoCasa).HasColumnType("char(15)");

                entity.Property(e => e.TelefonoPrivado).HasColumnType("varchar(15)");

                entity.HasOne(d => d.IdCanditatoNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdCanditato)
                    .HasConstraintName("RefCanditato421");

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .HasConstraintName("RefEstadoCivil181");

                entity.HasOne(d => d.IdEtniaNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdEtnia)
                    .HasConstraintName("RefEtnia182");

                entity.HasOne(d => d.IdGeneroNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdGenero)
                    .HasConstraintName("RefGenero40");

                entity.HasOne(d => d.IdNacionalidadNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdNacionalidad)
                    .HasConstraintName("RefNacionalidad43");

                entity.HasOne(d => d.IdSexoNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdSexo)
                    .HasConstraintName("RefSexo39");

                entity.HasOne(d => d.IdTipoIdentificacionNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdTipoIdentificacion)
                    .HasConstraintName("RefTipoIdentificacion179");

                entity.HasOne(d => d.IdTipoSangreNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdTipoSangre)
                    .HasConstraintName("RefTipoSangre180");
            });

            modelBuilder.Entity<PersonaCapacitacion>(entity =>
            {
                entity.HasKey(e => e.IdPersonaCapacitacion)
                    .HasName("PK148");

                entity.HasIndex(e => e.IdCapacitacion)
                    .HasName("Ref213333");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref17334");

                entity.Property(e => e.IdPersonaCapacitacion).HasColumnName("idPersonaCapacitacion");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdCapacitacion).HasColumnName("idCapacitacion");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.Observaciones).HasColumnType("varchar(100)");

                entity.HasOne(d => d.IdCapacitacionNavigation)
                    .WithMany(p => p.PersonaCapacitacion)
                    .HasForeignKey(d => d.IdCapacitacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCapacitacion333");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.PersonaCapacitacion)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPersona334");
            });

            modelBuilder.Entity<PersonaDiscapacidad>(entity =>
            {
                entity.HasKey(e => e.IdPersonaDiscapacidad)
                    .HasName("PK22");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref17186");

                entity.HasIndex(e => e.IdTipoDiscapacidad)
                    .HasName("Ref1332");

                entity.Property(e => e.IdPersonaDiscapacidad).HasColumnName("idPersonaDiscapacidad");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdTipoDiscapacidad).HasColumnName("idTipoDiscapacidad");

                entity.Property(e => e.NumeroCarnet)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.PersonaDiscapacidad)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("RefPersona186");

                entity.HasOne(d => d.IdTipoDiscapacidadNavigation)
                    .WithMany(p => p.PersonaDiscapacidad)
                    .HasForeignKey(d => d.IdTipoDiscapacidad)
                    .HasConstraintName("RefTipoDiscapacidad32");
            });

            modelBuilder.Entity<PersonaEnfermedad>(entity =>
            {
                entity.HasKey(e => e.IdPersonaEnfermedad)
                    .HasName("PK21");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref17185");

                entity.HasIndex(e => e.IdTipoEnfermedad)
                    .HasName("Ref1431");

                entity.Property(e => e.IdPersonaEnfermedad).HasColumnName("idPersonaEnfermedad");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdTipoEnfermedad).HasColumnName("idTipoEnfermedad");

                entity.Property(e => e.InstitucionEmite)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.PersonaEnfermedad)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("RefPersona185");

                entity.HasOne(d => d.IdTipoEnfermedadNavigation)
                    .WithMany(p => p.PersonaEnfermedad)
                    .HasForeignKey(d => d.IdTipoEnfermedad)
                    .HasConstraintName("RefTipoEnfermedad31");
            });

            modelBuilder.Entity<PersonaEstudio>(entity =>
            {
                entity.HasKey(e => e.IdPersonaEstudio)
                    .HasName("PK147");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref17335");

                entity.HasIndex(e => e.IdTitulo)
                    .HasName("Ref217332");

                entity.Property(e => e.IdPersonaEstudio)
                    .HasColumnName("idPersonaEstudio")
                    .ValueGeneratedNever();

                entity.Property(e => e.FechaGraduado).HasColumnType("date");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.IdTitulo).HasColumnName("idTitulo");

                entity.Property(e => e.Observaciones).HasColumnType("varchar(100)");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.PersonaEstudio)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPersona335");

                entity.HasOne(d => d.IdTituloNavigation)
                    .WithMany(p => p.PersonaEstudio)
                    .HasForeignKey(d => d.IdTitulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTitulo332");
            });

            modelBuilder.Entity<PersonaPaquetesInformaticos>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15314");

                entity.HasIndex(e => e.PaquetesInformaticosId)
                    .HasName("Ref195310");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.PersonaPaquetesInformaticos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado314");

                entity.HasOne(d => d.PaquetesInformaticos)
                    .WithMany(p => p.PersonaPaquetesInformaticos)
                    .HasForeignKey(d => d.PaquetesInformaticosId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPaquetesInformaticos310");
            });

            modelBuilder.Entity<PlanGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdPlanGestionCambio)
                    .HasName("PK262");

                entity.HasIndex(e => e.AprobadoPor)
                    .HasName("Ref15404");

                entity.HasIndex(e => e.RealizadoPor)
                    .HasName("Ref15403");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.FechaFin).HasColumnType("date");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.Titulo).HasColumnType("varchar(250)");

                entity.HasOne(d => d.AprobadoPorNavigation)
                    .WithMany(p => p.PlanGestionCambioAprobadoPorNavigation)
                    .HasForeignKey(d => d.AprobadoPor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado404");

                entity.HasOne(d => d.RealizadoPorNavigation)
                    .WithMany(p => p.PlanGestionCambioRealizadoPorNavigation)
                    .HasForeignKey(d => d.RealizadoPor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado403");
            });

            modelBuilder.Entity<PlanificacionHe>(entity =>
            {
                entity.HasKey(e => e.IdPlanificacionHe)
                    .HasName("PK179");

                entity.ToTable("PlanificacionHE");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15443");

                entity.HasIndex(e => e.IdEmpleadoAprueba)
                    .HasName("Ref15445");

                entity.Property(e => e.IdPlanificacionHe).HasColumnName("idPlanificacionHE");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEmpleadoAprueba).HasColumnName("idEmpleadoAprueba");

                entity.Property(e => e.NoHoras).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.PlanificacionHeIdEmpleadoNavigation)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado443");

                entity.HasOne(d => d.IdEmpleadoApruebaNavigation)
                    .WithMany(p => p.PlanificacionHeIdEmpleadoApruebaNavigation)
                    .HasForeignKey(d => d.IdEmpleadoAprueba)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado445");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasIndex(e => e.EvaluacionInduccinId)
                    .HasName("Ref231353");

                entity.Property(e => e.Nombre).HasColumnType("varchar(500)");

                entity.HasOne(d => d.EvaluacionInduccin)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.EvaluacionInduccinId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionInducion353");
            });

            modelBuilder.Entity<PreguntaRespuesta>(entity =>
            {
                entity.HasIndex(e => e.PreguntaId)
                    .HasName("Ref232354");

                entity.HasIndex(e => e.RespuestaId)
                    .HasName("Ref233355");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.PreguntaRespuesta)
                    .HasForeignKey(d => d.PreguntaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPregunta354");

                entity.HasOne(d => d.Respuesta)
                    .WithMany(p => p.PreguntaRespuesta)
                    .HasForeignKey(d => d.RespuestaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRespuesta355");
            });

            modelBuilder.Entity<Proceso>(entity =>
            {
                entity.HasKey(e => e.IdProceso)
                    .HasName("PK54");

                entity.Property(e => e.IdProceso).HasColumnName("idProceso");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ProcesoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdProcesoDetalle)
                    .HasName("PK55");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref5192");

                entity.HasIndex(e => e.IdProceso)
                    .HasName("Ref5491");

                entity.Property(e => e.IdProcesoDetalle).HasColumnName("idProcesoDetalle");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.IdProceso).HasColumnName("idProceso");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.ProcesoDetalle)
                    .HasForeignKey(d => d.IdDependencia)
                    .HasConstraintName("RefDependencia92");

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany(p => p.ProcesoDetalle)
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefProceso91");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor)
                    .HasName("PK113");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.Property(e => e.Identificacion).HasColumnType("varchar(20)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.IdProvincia)
                    .HasName("PK1");

                entity.HasIndex(e => e.IdPais)
                    .HasName("Ref22");

                entity.Property(e => e.IdProvincia).HasColumnName("idProvincia");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Provincia)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPais2");
            });

            modelBuilder.Entity<Provisiones>(entity =>
            {
                entity.HasKey(e => e.IdProvisiones)
                    .HasName("PK160");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15444");

                entity.HasIndex(e => e.IdTipoProvision)
                    .HasName("Ref287441");

                entity.Property(e => e.IdProvisiones).HasColumnName("idProvisiones");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdTipoProvision).HasColumnName("idTipoProvision");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Provisiones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado444");

                entity.HasOne(d => d.IdTipoProvisionNavigation)
                    .WithMany(p => p.Provisiones)
                    .HasForeignKey(d => d.IdTipoProvision)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoProvision441");
            });

            modelBuilder.Entity<RealizaExamenInduccion>(entity =>
            {
                entity.HasIndex(e => e.EvaluacionInduccinId)
                    .HasName("Ref231356");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15357");

                entity.Property(e => e.Calificacion).HasColumnType("decimal");

                entity.Property(e => e.Fecha).HasColumnType("char(10)");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.EvaluacionInduccin)
                    .WithMany(p => p.RealizaExamenInduccion)
                    .HasForeignKey(d => d.EvaluacionInduccinId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEvaluacionInducion356");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RealizaExamenInduccion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado357");
            });

            modelBuilder.Entity<RecepcionActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionActivoFijo)
                    .HasName("PK112");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15164");

                entity.HasIndex(e => e.IdLibroActivoFijo)
                    .HasName("Ref91163");

                entity.HasIndex(e => e.IdMotivoRecepcion)
                    .HasName("Ref114162");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("Ref113160");

                entity.HasIndex(e => e.IdSubClaseActivoFijo)
                    .HasName("Ref88161");

                entity.Property(e => e.IdRecepcionActivoFijo).HasColumnName("idRecepcionActivoFijo");

                entity.Property(e => e.Cantidad).HasColumnType("decimal");

                entity.Property(e => e.FechaRecepcion).HasColumnType("date");

                entity.Property(e => e.Fondo).HasColumnType("varchar(50)");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdLibroActivoFijo).HasColumnName("idLibroActivoFijo");

                entity.Property(e => e.IdMotivoRecepcion).HasColumnName("idMotivoRecepcion");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.Property(e => e.IdSubClaseActivoFijo).HasColumnName("idSubClaseActivoFijo");

                entity.Property(e => e.OrdenCompra).HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado164");

                entity.HasOne(d => d.IdLibroActivoFijoNavigation)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdLibroActivoFijo)
                    .HasConstraintName("RefLibroActivoFijo163");

                entity.HasOne(d => d.IdMotivoRecepcionNavigation)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdMotivoRecepcion)
                    .HasConstraintName("RefMotivoRecepcion162");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefProveedor160");

                entity.HasOne(d => d.IdSubClaseActivoFijoNavigation)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdSubClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSubClaseActivoFijo161");
            });

            modelBuilder.Entity<RecepcionActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionActivoFijoDetalle)
                    .HasName("PK115");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("Ref89166");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73167");

                entity.HasIndex(e => e.IdRecepcionActivoFijo)
                    .HasName("Ref112165");

                entity.Property(e => e.IdRecepcionActivoFijoDetalle).HasColumnName("idRecepcionActivoFijoDetalle");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdRecepcionActivoFijo).HasColumnName("idRecepcionActivoFijo");

                entity.Property(e => e.NumeroPoliza).HasColumnType("varchar(20)");

                entity.HasOne(d => d.IdActivoFijoNavigation)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .HasConstraintName("RefActivoFijo166");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado167");

                entity.HasOne(d => d.IdRecepcionActivoFijoNavigation)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdRecepcionActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRecepcionActivoFijo165");
            });

            modelBuilder.Entity<RecepcionArticulos>(entity =>
            {
                entity.HasKey(e => e.IdRecepcionArticulos)
                    .HasName("PK139");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("Ref99240");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15237");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("Ref100241");

                entity.HasIndex(e => e.IdProveedor)
                    .HasName("Ref113244");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdMaestroArticuloSucursal).HasColumnName("idMaestroArticuloSucursal");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefArticulo240");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado237");

                entity.HasOne(d => d.IdMaestroArticuloSucursalNavigation)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMaestroArticuloSucursal241");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefProveedor244");
            });

            modelBuilder.Entity<RegimenLaboral>(entity =>
            {
                entity.HasKey(e => e.IdRegimenLaboral)
                    .HasName("PK56");

                entity.Property(e => e.IdRegimenLaboral).HasColumnName("idRegimenLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RegistroEntradaSalida>(entity =>
            {
                entity.HasKey(e => e.IdRegistroEntradaSalida)
                    .HasName("PK187");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15440");

                entity.Property(e => e.IdRegistroEntradaSalida)
                    .HasColumnName("idRegistroEntradaSalida")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RegistroEntradaSalida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado440");
            });

            modelBuilder.Entity<RelacionLaboral>(entity =>
            {
                entity.HasKey(e => e.IdRelacionLaboral)
                    .HasName("PK57");

                entity.HasIndex(e => e.IdRegimenLaboral)
                    .HasName("Ref5693");

                entity.Property(e => e.IdRelacionLaboral).HasColumnName("idRelacionLaboral");

                entity.Property(e => e.IdRegimenLaboral).HasColumnName("idRegimenLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdRegimenLaboralNavigation)
                    .WithMany(p => p.RelacionLaboral)
                    .HasForeignKey(d => d.IdRegimenLaboral)
                    .HasConstraintName("RefRegimenLaboral93");
            });

            modelBuilder.Entity<RelacionesInternasExternas>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");
            });

            modelBuilder.Entity<RelacionesInternasExternasIndiceOcupacional>(entity =>
            {
                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("Ref69330");

                entity.HasIndex(e => e.RelacionesInternasExternasId)
                    .HasName("Ref211329");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.IdIndiceOcupacionalNavigation)
                    .WithMany(p => p.RelacionesInternasExternasIndiceOcupacional)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefIndiceOcupacional330");

                entity.HasOne(d => d.RelacionesInternasExternas)
                    .WithMany(p => p.RelacionesInternasExternasIndiceOcupacional)
                    .HasForeignKey(d => d.RelacionesInternasExternasId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRelacionesInternasExternas329");
            });

            modelBuilder.Entity<Relevancia>(entity =>
            {
                entity.HasKey(e => e.IdRelevancia)
                    .HasName("PK41");

                entity.Property(e => e.IdRelevancia).HasColumnName("idRelevancia");

                entity.Property(e => e.ComportamientoObservable)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RequisitosNoCumple>(entity =>
            {
                entity.HasIndex(e => e.AdministracionTalentoHumanoId)
                    .HasName("Ref198316");

                entity.Property(e => e.Detalle).HasColumnType("varchar(250)");

                entity.HasOne(d => d.AdministracionTalentoHumano)
                    .WithMany(p => p.RequisitosNoCumple)
                    .HasForeignKey(d => d.AdministracionTalentoHumanoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefAdministracionTalentoHumano316");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("varchar(250)");
            });

            modelBuilder.Entity<Rmu>(entity =>
            {
                entity.HasKey(e => e.IdRmu)
                    .HasName("PK150");

                entity.ToTable("RMU");

                entity.HasIndex(e => e.IdEmpeladoIe)
                    .HasName("Ref283435");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15438");

                entity.HasIndex(e => e.IdTipoRmu)
                    .HasName("Ref284436");

                entity.Property(e => e.IdRmu).HasColumnName("idRMU");

                entity.Property(e => e.IdEmpeladoIe).HasColumnName("idEmpeladoIE");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdTipoRmu).HasColumnName("idTipoRMU");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpeladoIeNavigation)
                    .WithMany(p => p.Rmu)
                    .HasForeignKey(d => d.IdEmpeladoIe)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleadoIE435");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Rmu)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado438");

                entity.HasOne(d => d.IdTipoRmuNavigation)
                    .WithMany(p => p.Rmu)
                    .HasForeignKey(d => d.IdTipoRmu)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoRMU436");
            });

            modelBuilder.Entity<RolPagoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdRolPagoDetalle)
                    .HasName("PK79");

                entity.HasIndex(e => e.IdRolPagos)
                    .HasName("Ref78119");

                entity.Property(e => e.IdRolPagoDetalle).HasColumnName("idRolPagoDetalle");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IdRolPagos).HasColumnName("idRolPagos");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.IdRolPagosNavigation)
                    .WithMany(p => p.RolPagoDetalle)
                    .HasForeignKey(d => d.IdRolPagos)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRolPagos119");
            });

            modelBuilder.Entity<RolPagos>(entity =>
            {
                entity.HasKey(e => e.IdRolPagos)
                    .HasName("PK78");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15118");

                entity.Property(e => e.IdRolPagos).HasColumnName("idRolPagos");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.SaldoFinal).HasColumnType("decimal");

                entity.Property(e => e.SaldoInicial).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RolPagos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado118");
            });

            modelBuilder.Entity<RolPuesto>(entity =>
            {
                entity.HasKey(e => e.IdRolPuesto)
                    .HasName("PK63");

                entity.Property(e => e.IdRolPuesto).HasColumnName("idRolPuesto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Rubro>(entity =>
            {
                entity.HasKey(e => e.IdRubro)
                    .HasName("PK83");

                entity.Property(e => e.IdRubro).HasColumnName("idRubro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TasaPorcentualMaxima).HasColumnType("decimal");
            });

            modelBuilder.Entity<RubroLiquidacion>(entity =>
            {
                entity.HasKey(e => e.IdRubroLiquidacion)
                    .HasName("PK183");

                entity.Property(e => e.IdRubroLiquidacion).HasColumnName("idRubroLiquidacion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.IdSexo)
                    .HasName("PK6");

                entity.Property(e => e.IdSexo).HasColumnName("idSexo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<SituacionPropuesta>(entity =>
            {
                entity.HasKey(e => e.IdSituacionPropuesta)
                    .HasName("PK258");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("Ref51400");

                entity.Property(e => e.Ano).HasColumnType("date");

                entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.HasOne(d => d.IdDependenciaNavigation)
                    .WithMany(p => p.SituacionPropuesta)
                    .HasForeignKey(d => d.IdDependencia)
                    .HasConstraintName("RefDependencia400");
            });

            modelBuilder.Entity<SolicitudAcumulacionDecimos>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudAcumulacionDecimos)
                    .HasName("PK110");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15158");

                entity.Property(e => e.IdSolicitudAcumulacionDecimos).HasColumnName("idSolicitudAcumulacionDecimos");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudAcumulacionDecimos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado158");
            });

            modelBuilder.Entity<SolicitudAnticipo>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudAnticipo)
                    .HasName("PK109");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15156");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73157");

                entity.Property(e => e.IdSolicitudAnticipo).HasColumnName("idSolicitudAnticipo");

                entity.Property(e => e.CantidadCancelada).HasColumnType("decimal");

                entity.Property(e => e.CantidadSolicitada).HasColumnType("decimal");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudAnticipo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado156");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudAnticipo)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado157");
            });

            modelBuilder.Entity<SolicitudCertificadoPersonal>(entity =>
            {
                entity.HasKey(e => new { e.IdSolicitudCertificadoPersonal, e.IdEstado })
                    .HasName("PK103");

                entity.HasIndex(e => e.IdEmpleadoDirigidoA)
                    .HasName("Ref15151");

                entity.HasIndex(e => e.IdEmpleadoSolicitante)
                    .HasName("Ref15150");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73223");

                entity.HasIndex(e => e.IdTipoCertificado)
                    .HasName("Ref104149");

                entity.Property(e => e.IdSolicitudCertificadoPersonal)
                    .HasColumnName("idSolicitudCertificadoPersonal")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleadoDirigidoA).HasColumnName("idEmpleadoDirigidoA");

                entity.Property(e => e.IdEmpleadoSolicitante).HasColumnName("idEmpleadoSolicitante");

                entity.Property(e => e.IdTipoCertificado).HasColumnName("idTipoCertificado");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.HasOne(d => d.IdEmpleadoDirigidoANavigation)
                    .WithMany(p => p.SolicitudCertificadoPersonalIdEmpleadoDirigidoANavigation)
                    .HasForeignKey(d => d.IdEmpleadoDirigidoA)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado151");

                entity.HasOne(d => d.IdEmpleadoSolicitanteNavigation)
                    .WithMany(p => p.SolicitudCertificadoPersonalIdEmpleadoSolicitanteNavigation)
                    .HasForeignKey(d => d.IdEmpleadoSolicitante)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado150");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudCertificadoPersonal)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEstado223");

                entity.HasOne(d => d.IdTipoCertificadoNavigation)
                    .WithMany(p => p.SolicitudCertificadoPersonal)
                    .HasForeignKey(d => d.IdTipoCertificado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoCertificado149");
            });

            modelBuilder.Entity<SolicitudHorasExtras>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15306");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.Observaciones).HasColumnType("varchar(250)");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudHorasExtras)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado306");
            });

            modelBuilder.Entity<SolicitudLiquidacionHaberes>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudLiquidacionHaberes)
                    .HasName("PK111");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15159");

                entity.Property(e => e.IdSolicitudLiquidacionHaberes).HasColumnName("idSolicitudLiquidacionHaberes");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.TotalDecimoCuarto).HasColumnType("decimal");

                entity.Property(e => e.TotalDecimoTercero).HasColumnType("decimal");

                entity.Property(e => e.TotalDesahucio).HasColumnType("decimal");

                entity.Property(e => e.TotalDespidoIntempestivo).HasColumnType("decimal");

                entity.Property(e => e.TotalFondoReserva).HasColumnType("decimal");

                entity.Property(e => e.TotalVacaciones).HasColumnType("decimal");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudLiquidacionHaberes)
                    .HasForeignKey(d => d.IdEmpleado)
                    .HasConstraintName("RefEmpleado159");
            });

            modelBuilder.Entity<SolicitudModificacionFichaEmpleado>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudModificacionFichaEmpleado)
                    .HasName("PK108");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15154");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73155");

                entity.Property(e => e.IdSolicitudModificacionFichaEmpleado).HasColumnName("idSolicitudModificacionFichaEmpleado");

                entity.Property(e => e.Apellidos).HasColumnType("varchar(50)");

                entity.Property(e => e.CapacitacionesRecibidas).HasColumnType("text");

                entity.Property(e => e.CargasFamiliares).HasColumnType("text");

                entity.Property(e => e.Direccion).HasColumnType("varchar(1024)");

                entity.Property(e => e.ExperienciaLaboral).HasColumnType("text");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.FormacionAcademica).HasColumnType("text");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.Nombres).HasColumnType("varchar(50)");

                entity.Property(e => e.TelefonoCasa).HasColumnType("varchar(15)");

                entity.Property(e => e.TelefonoPrivado).HasColumnType("varchar(20)");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudModificacionFichaEmpleado)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado154");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudModificacionFichaEmpleado)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado155");
            });

            modelBuilder.Entity<SolicitudPermiso>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudPermiso)
                    .HasName("PK76");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15113");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73114");

                entity.HasIndex(e => e.IdPermiso)
                    .HasName("Ref134214");

                entity.Property(e => e.IdSolicitudPermiso).HasColumnName("idSolicitudPermiso");

                entity.Property(e => e.FechaDesde).HasColumnType("char(10)");

                entity.Property(e => e.FechaHasta).HasColumnType("char(10)");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.Motivo).HasColumnType("text");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado113");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado114");

                entity.HasOne(d => d.IdPermisoNavigation)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdPermiso)
                    .HasConstraintName("RefPermiso214");
            });

            modelBuilder.Entity<SolicitudPlanificacionVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudPlanificacionVacaciones)
                    .HasName("PK72");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15110");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73303");

                entity.Property(e => e.IdSolicitudPlanificacionVacaciones).HasColumnName("idSolicitudPlanificacionVacaciones");

                entity.Property(e => e.FechaDesde).HasColumnType("date");

                entity.Property(e => e.FechaHasta).HasColumnType("date");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudPlanificacionVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado110");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudPlanificacionVacaciones)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado303");
            });

            modelBuilder.Entity<SolicitudProveduria>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduria)
                    .HasName("PK101");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15145");

                entity.Property(e => e.IdSolicitudProveduria).HasColumnName("idSolicitudProveduria");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudProveduria)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado145");
            });

            modelBuilder.Entity<SolicitudProveduriaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduriaDetalle)
                    .HasName("PK102");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("Ref99243");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73232");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("Ref100242");

                entity.HasIndex(e => e.IdSolicitudProveduria)
                    .HasName("Ref101228");

                entity.Property(e => e.IdSolicitudProveduriaDetalle).HasColumnName("idSolicitudProveduriaDetalle");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdMaestroArticuloSucursal).HasColumnName("idMaestroArticuloSucursal");

                entity.Property(e => e.IdSolicitudProveduria).HasColumnName("idSolicitudProveduria");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefArticulo243");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEstado232");

                entity.HasOne(d => d.IdMaestroArticuloSucursalNavigation)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdMaestroArticuloSucursal)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMaestroArticuloSucursal242");

                entity.HasOne(d => d.IdSolicitudProveduriaNavigation)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdSolicitudProveduria)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefSolicitudProveduria228");
            });

            modelBuilder.Entity<SolicitudVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudVacaciones)
                    .HasName("PK75");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15111");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73115");

                entity.Property(e => e.IdSolicitudVacaciones).HasColumnName("idSolicitudVacaciones");

                entity.Property(e => e.FechaDesde).HasColumnType("date");

                entity.Property(e => e.FechaHasta).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado111");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudVacaciones)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado115");
            });

            modelBuilder.Entity<SolicitudViatico>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudViatico)
                    .HasName("PK77");

                entity.HasIndex(e => e.IdConfiguracionViatico)
                    .HasName("Ref255393");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15117");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("Ref73116");

                entity.HasIndex(e => e.TipoViaticoId)
                    .HasName("Ref250383");

                entity.Property(e => e.IdSolicitudViatico).HasColumnName("idSolicitudViatico");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.ValorEstimado).HasColumnType("decimal");

                entity.HasOne(d => d.IdConfiguracionViaticoNavigation)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdConfiguracionViatico)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefConfiguracionViatico393");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado117");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("RefEstado116");

                entity.HasOne(d => d.TipoViatico)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.TipoViaticoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTipoViatico383");
            });

            modelBuilder.Entity<SubClaseActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdSubClaseActivoFijo)
                    .HasName("PK88");

                entity.HasIndex(e => e.IdClaseActivoFijo)
                    .HasName("Ref87125");

                entity.Property(e => e.IdSubClaseActivoFijo).HasColumnName("idSubClaseActivoFijo");

                entity.Property(e => e.IdClaseActivoFijo).HasColumnName("idClaseActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdClaseActivoFijoNavigation)
                    .WithMany(p => p.SubClaseActivoFijo)
                    .HasForeignKey(d => d.IdClaseActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefClaseActivoFijo125");
            });

            modelBuilder.Entity<SubClaseArticulo>(entity =>
            {
                entity.HasKey(e => e.IdSubClaseArticulo)
                    .HasName("PK98");

                entity.HasIndex(e => e.IdClaseArticulo)
                    .HasName("Ref97140");

                entity.Property(e => e.IdSubClaseArticulo).HasColumnName("idSubClaseArticulo");

                entity.Property(e => e.IdClaseArticulo).HasColumnName("idClaseArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdClaseArticuloNavigation)
                    .WithMany(p => p.SubClaseArticulo)
                    .HasForeignKey(d => d.IdClaseArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefClaseArticulo140");
            });

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasKey(e => e.IdSucursal)
                    .HasName("PK18");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("Ref314");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");

                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.Sucursal)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefCiudad14");
            });

            modelBuilder.Entity<TablaDepreciacion>(entity =>
            {
                entity.HasKey(e => e.IdTablaDepreciacion)
                    .HasName("PK85");

                entity.Property(e => e.IdTablaDepreciacion).HasColumnName("idTablaDepreciacion");

                entity.Property(e => e.IndiceDepreciacion).HasColumnType("decimal");
            });

            modelBuilder.Entity<Temporal>(entity =>
            {
                entity.HasKey(e => e.IdTemporal)
                    .HasName("PK65");

                entity.Property(e => e.IdTemporal).HasColumnName("idTemporal");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoAccionPersonal>(entity =>
            {
                entity.HasKey(e => e.IdTipoAccionPersonal)
                    .HasName("PK247_1");

                entity.Property(e => e.IdTipoAccionPersonal).HasColumnName("idTipoAccionPersonal");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<TipoActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdTipoActivoFijo)
                    .HasName("PK86");

                entity.Property(e => e.IdTipoActivoFijo).HasColumnName("idTipoActivoFijo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoArticulo>(entity =>
            {
                entity.HasKey(e => e.IdTipoArticulo)
                    .HasName("PK96");

                entity.Property(e => e.IdTipoArticulo).HasColumnName("idTipoArticulo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoCertificado>(entity =>
            {
                entity.HasKey(e => e.IdTipoCertificado)
                    .HasName("PK104");

                entity.Property(e => e.IdTipoCertificado).HasColumnName("idTipoCertificado");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(1024)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoConsurso>(entity =>
            {
                entity.HasKey(e => e.IdTipoConsurso)
                    .HasName("PK268");

                entity.Property(e => e.Descripcion).HasColumnType("varchar(250)");

                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoDiscapacidad>(entity =>
            {
                entity.HasKey(e => e.IdTipoDiscapacidad)
                    .HasName("PK13");

                entity.Property(e => e.IdTipoDiscapacidad).HasColumnName("idTipoDiscapacidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoDiscapacidadSustituto>(entity =>
            {
                entity.HasKey(e => e.IdTipoDiscapacidadSustituto)
                    .HasName("PK68");

                entity.Property(e => e.IdTipoDiscapacidadSustituto).HasColumnName("idTipoDiscapacidadSustituto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoEnfermedad>(entity =>
            {
                entity.HasKey(e => e.IdTipoEnfermedad)
                    .HasName("PK14");

                entity.Property(e => e.IdTipoEnfermedad).HasColumnName("idTipoEnfermedad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoIdentificacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoIdentificacion)
                    .HasName("PK124");

                entity.Property(e => e.IdTipoIdentificacion).HasColumnName("idTipoIdentificacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoMovimientoInterno>(entity =>
            {
                entity.HasKey(e => e.IdTipoMovimientoInterno)
                    .HasName("PK127");

                entity.Property(e => e.IdTipoMovimientoInterno).HasColumnName("idTipoMovimientoInterno");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoNombramiento>(entity =>
            {
                entity.HasKey(e => e.IdTipoNombramiento)
                    .HasName("PK58");

                entity.HasIndex(e => e.IdRelacionLaboral)
                    .HasName("Ref5794");

                entity.Property(e => e.IdTipoNombramiento).HasColumnName("idTipoNombramiento");

                entity.Property(e => e.IdRelacionLaboral).HasColumnName("idRelacionLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdRelacionLaboralNavigation)
                    .WithMany(p => p.TipoNombramiento)
                    .HasForeignKey(d => d.IdRelacionLaboral)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefRelacionLaboral94");
            });

            modelBuilder.Entity<TipoPermiso>(entity =>
            {
                entity.HasKey(e => e.IdTipoPermiso)
                    .HasName("PK133");

                entity.Property(e => e.IdTipoPermiso).HasColumnName("idTipoPermiso");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoProvision>(entity =>
            {
                entity.HasKey(e => e.IdTipoProvision)
                    .HasName("PK161");

                entity.Property(e => e.IdTipoProvision).HasColumnName("idTipoProvision");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<TipoRmu>(entity =>
            {
                entity.HasKey(e => e.IdTipoRmu)
                    .HasName("PK155");

                entity.ToTable("TipoRMU");

                entity.Property(e => e.IdTipoRmu).HasColumnName("idTipoRMU");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("char(10)");
            });

            modelBuilder.Entity<TipoSangre>(entity =>
            {
                entity.HasKey(e => e.IdTipoSangre)
                    .HasName("PK9");

                entity.Property(e => e.IdTipoSangre).HasColumnName("idTipoSangre");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoTransporte>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TipoViatico>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Titulo>(entity =>
            {
                entity.HasKey(e => e.IdTitulo)
                    .HasName("PK149");

                entity.HasIndex(e => e.IdEstudio)
                    .HasName("Ref214331");

                entity.Property(e => e.IdTitulo).HasColumnName("idTitulo");

                entity.Property(e => e.IdEstudio).HasColumnName("idEstudio");

                entity.HasOne(d => d.IdEstudioNavigation)
                    .WithMany(p => p.Titulo)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEstudio331");
            });

            modelBuilder.Entity<TrabajoEquipoIniciativaLiderazgo>(entity =>
            {
                entity.HasKey(e => e.IdTrabajoEquipoIniciativaLiderazgo)
                    .HasName("PK48");

                entity.Property(e => e.IdTrabajoEquipoIniciativaLiderazgo).HasColumnName("idTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TranferenciaArticulo>(entity =>
            {
                entity.HasKey(e => e.IdTranferenciaArticulo)
                    .HasName("PK144");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("Ref99258");

                entity.HasIndex(e => e.IdEmpleadoEnvia)
                    .HasName("Ref15259");

                entity.HasIndex(e => e.IdMaestroArticuloEnvia)
                    .HasName("Ref100257");

                entity.HasIndex(e => e.IdMaestroRecibe)
                    .HasName("Ref100262");

                entity.HasIndex(e => e.IdempleadoRecibe)
                    .HasName("Ref15263");

                entity.Property(e => e.IdTranferenciaArticulo).ValueGeneratedNever();

                entity.Property(e => e.Fecha).HasColumnType("char(10)");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdEmpleadoEnvia).HasColumnName("idEmpleadoEnvia");

                entity.Property(e => e.IdMaestroArticuloEnvia).HasColumnName("idMaestroArticuloEnvia");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.TranferenciaArticulo)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefArticulo258");

                entity.HasOne(d => d.IdEmpleadoEnviaNavigation)
                    .WithMany(p => p.TranferenciaArticuloIdEmpleadoEnviaNavigation)
                    .HasForeignKey(d => d.IdEmpleadoEnvia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado259");

                entity.HasOne(d => d.IdMaestroArticuloEnviaNavigation)
                    .WithMany(p => p.TranferenciaArticuloIdMaestroArticuloEnviaNavigation)
                    .HasForeignKey(d => d.IdMaestroArticuloEnvia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMaestroArticuloSucursal257");

                entity.HasOne(d => d.IdMaestroRecibeNavigation)
                    .WithMany(p => p.TranferenciaArticuloIdMaestroRecibeNavigation)
                    .HasForeignKey(d => d.IdMaestroRecibe)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefMaestroArticuloSucursal262");

                entity.HasOne(d => d.IdempleadoRecibeNavigation)
                    .WithMany(p => p.TranferenciaArticuloIdempleadoRecibeNavigation)
                    .HasForeignKey(d => d.IdempleadoRecibe)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado263");
            });

            modelBuilder.Entity<TransferenciaActivoFijo>(entity =>
            {
                entity.HasKey(e => e.IdTransferenciaActivoFijo)
                    .HasName("PK117");

                entity.HasIndex(e => e.IdEmpleadoRegistra)
                    .HasName("Ref15169");

                entity.HasIndex(e => e.IdEmpleadoResponsableEnvio)
                    .HasName("Ref15170");

                entity.HasIndex(e => e.IdEmpleadoResponsableRecibo)
                    .HasName("Ref15171");

                entity.Property(e => e.IdTransferenciaActivoFijo).HasColumnName("idTransferenciaActivoFijo");

                entity.Property(e => e.Destino)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FechaTransferencia).HasColumnType("date");

                entity.Property(e => e.IdEmpleadoRegistra).HasColumnName("idEmpleadoRegistra");

                entity.Property(e => e.IdEmpleadoResponsableEnvio).HasColumnName("idEmpleadoResponsableEnvio");

                entity.Property(e => e.IdEmpleadoResponsableRecibo).HasColumnName("idEmpleadoResponsableRecibo");

                entity.Property(e => e.Observaciones).HasColumnType("text");

                entity.Property(e => e.Origen).HasColumnType("varchar(50)");

                entity.HasOne(d => d.IdEmpleadoRegistraNavigation)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoRegistraNavigation)
                    .HasForeignKey(d => d.IdEmpleadoRegistra)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado169");

                entity.HasOne(d => d.IdEmpleadoResponsableEnvioNavigation)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoResponsableEnvioNavigation)
                    .HasForeignKey(d => d.IdEmpleadoResponsableEnvio)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado170");

                entity.HasOne(d => d.IdEmpleadoResponsableReciboNavigation)
                    .WithMany(p => p.TransferenciaActivoFijoIdEmpleadoResponsableReciboNavigation)
                    .HasForeignKey(d => d.IdEmpleadoResponsableRecibo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado171");
            });

            modelBuilder.Entity<TransferenciaActivoFijoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdTransferenciaActivoFijoDetalle)
                    .HasName("PK118");

                entity.HasIndex(e => e.IdActivoFijo)
                    .HasName("Ref89173");

                entity.HasIndex(e => e.IdTransferenciaActivoFijo)
                    .HasName("Ref117172");

                entity.Property(e => e.IdTransferenciaActivoFijoDetalle).HasColumnName("idTransferenciaActivoFijoDetalle");

                entity.Property(e => e.IdActivoFijo).HasColumnName("idActivoFijo");

                entity.Property(e => e.IdTransferenciaActivoFijo).HasColumnName("idTransferenciaActivoFijo");

                entity.HasOne(d => d.IdActivoFijoNavigation)
                    .WithMany(p => p.TransferenciaActivoFijoDetalle)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .HasConstraintName("RefActivoFijo173");

                entity.HasOne(d => d.IdTransferenciaActivoFijoNavigation)
                    .WithMany(p => p.TransferenciaActivoFijoDetalle)
                    .HasForeignKey(d => d.IdTransferenciaActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefTransferenciaActivoFijo172");
            });

            modelBuilder.Entity<TrayectoriaLaboral>(entity =>
            {
                entity.HasKey(e => e.IdTrayectoriaLaboral)
                    .HasName("PK271");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("Ref17422");

                entity.Property(e => e.DescripcionFunciones).HasColumnType("text");

                entity.Property(e => e.Empresa).HasColumnType("varchar(100)");

                entity.Property(e => e.FechaFin).HasColumnType("date");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.IdPersona).HasColumnName("idPersona");

                entity.Property(e => e.PuestoTrabajo).HasColumnType("varchar(250)");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.TrayectoriaLaboral)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefPersona422");
            });

            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.HasKey(e => e.IdUnidadMedida)
                    .HasName("PK84");

                entity.Property(e => e.IdUnidadMedida).HasColumnName("idUnidadMedida");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ValidacionInmediatoSuperior>(entity =>
            {
                entity.HasKey(e => e.ValidacionJefeId)
                    .HasName("PK193");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("Ref15309");

                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("Ref107308");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");

                entity.Property(e => e.IdFormularioAnalisisOcupacional).HasColumnName("idFormularioAnalisisOcupacional");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.ValidacionInmediatoSuperior)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefEmpleado309");

                entity.HasOne(d => d.IdFormularioAnalisisOcupacionalNavigation)
                    .WithMany(p => p.ValidacionInmediatoSuperior)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RefFormularioAnalisisOcupacional308");
            });
        }
    }
}