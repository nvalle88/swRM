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


        public virtual DbSet<bd.swrm.entidades.Negocio.AccionPersonal> AccionPersonal { get; set; }
        public virtual DbSet<ActividadesAnalisisOcupacional> ActividadesAnalisisOcupacional { get; set; }
        public virtual DbSet<ActividadesEsenciales> ActividadesEsenciales { get; set; }
        public virtual DbSet<ActividadesGestionCambio> ActividadesGestionCambio { get; set; }
        public virtual DbSet<AdministracionTalentoHumano> AdministracionTalentoHumano { get; set; }
        public virtual DbSet<AnoExperiencia> AnoExperiencia { get; set; }
        public virtual DbSet<AprobacionViatico> AprobacionViatico { get; set; }
        public virtual DbSet<AreaConocimiento> AreaConocimiento { get; set; }
        public virtual DbSet<Articulo> Articulo { get; set; }
    
        public virtual DbSet<AvanceGestionCambio> AvanceGestionCambio { get; set; }
        public virtual DbSet<BrigadaSSO> BrigadaSSO { get; set; }
        public virtual DbSet<BrigadaSSORol> BrigadaSsorol { get; set; }
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
        public virtual DbSet<Ciudad> Ciudad { get; set; }
        public virtual DbSet<ClaseActivoFijo> ClaseActivoFijo { get; set; }
        public virtual DbSet<ClaseArticulo> ClaseArticulo { get; set; }
        public virtual DbSet<CodigoActivoFijo> CodigoActivoFijo { get; set; }
        public virtual DbSet<ComportamientoObservable> ComportamientoObservable { get; set; }
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
        public virtual DbSet<DocumentosParentescodos> DocumentosParentescodos { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<EmpleadoActivoFijo> EmpleadoActivoFijo { get; set; }
        public virtual DbSet<EmpleadoContactoEmergencia> EmpleadoContactoEmergencia { get; set; }
        public virtual DbSet<EmpleadoFamiliar> EmpleadoFamiliar { get; set; }
        public virtual DbSet<EmpleadoFormularioCapacitacion> EmpleadoFormularioCapacitacion { get; set; }
        public virtual DbSet<EmpleadoIE> EmpleadoIE { get; set; }
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
        public virtual DbSet<FormulasRMU> FormulasRMU { get; set; }
        public virtual DbSet<FrecuenciaAplicacion> FrecuenciaAplicacion { get; set; }
        public virtual DbSet<GastoRubro> GastoRubro { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<GrupoOcupacional> GrupoOcupacional { get; set; }
        public virtual DbSet<Indicador> Indicador { get; set; }
        public virtual DbSet<IndiceOcupacional> IndiceOcupacional { get; set; }
        public virtual DbSet<IndiceOcupacionalActividadesEsenciales> IndiceOcupacionalActividadesEsenciales { get; set; }
        public virtual DbSet<IndiceOcupacionalAreaConocimiento> IndiceOcupacionalAreaConocimiento { get; set; }
        public virtual DbSet<IndiceOcupacionalCapacitaciones> IndiceOcupacionalCapacitaciones { get; set; }
        public virtual DbSet<IndiceOcupacionalComportamientoObservable> IndiceOcupacionalComportamientoObservable { get; set; }
        public virtual DbSet<IndiceOcupacionalConocimientosAdicionales> IndiceOcupacionalConocimientosAdicionales { get; set; }
        public virtual DbSet<IndiceOcupacionalEstudio> IndiceOcupacionalEstudio { get; set; }
        public virtual DbSet<IndiceOcupacionalModalidadPartida> IndiceOcupacionalModalidadPartida { get; set; }
        public virtual DbSet<InformeUATH> InformeUATH { get; set; }
        public virtual DbSet<InformeViatico> InformeViatico { get; set; }
        public virtual DbSet<IngresoEgresoRMU> IngresoEgresoRMU { get; set; }
        public virtual DbSet<InstitucionFinanciera> InstitucionFinanciera { get; set; }
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
        public virtual DbSet<MotivoRecepcion> MotivoRecepcion { get; set; }
        public virtual DbSet<Nacionalidad> Nacionalidad { get; set; }
        public virtual DbSet<NacionalidadIndigena> NacionalidadIndigena { get; set; }
        public virtual DbSet<Nivel> Nivel { get; set; }
        public virtual DbSet<NivelConocimiento> NivelConocimiento { get; set; }
        public virtual DbSet<NivelDesarrollo> NivelDesarrollo { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<PaquetesInformaticos> PaquetesInformaticos { get; set; }
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
        public virtual DbSet<PlanificacionHE> PlanificacionHE { get; set; }
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
        public virtual DbSet<RMU> RMU { get; set; }
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
        public virtual DbSet<TipoAccionPersonal> TipoAccionPersonal { get; set; }
        public virtual DbSet<TipoActivoFijo> TipoActivoFijo { get; set; }
        public virtual DbSet<TipoArticulo> TipoArticulo { get; set; }
        public virtual DbSet<TipoCertificado> TipoCertificado { get; set; }
        public virtual DbSet<TipoConcurso> TipoConcurso { get; set; }
        public virtual DbSet<TipoDiscapacidad> TipoDiscapacidad { get; set; }
        public virtual DbSet<TipoEnfermedad> TipoEnfermedad { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }
        public virtual DbSet<TipoMovimientoInterno> TipoMovimientoInterno { get; set; }
        public virtual DbSet<TipoNombramiento> TipoNombramiento { get; set; }
        public virtual DbSet<TipoPermiso> TipoPermiso { get; set; }
        public virtual DbSet<TipoProvision> TipoProvision { get; set; }
        public virtual DbSet<TipoRMU> TipoRMU { get; set; }
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
                    .HasName("PK_AccionPersonal");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_AccionPersonal_IdEmpleado");

                entity.HasIndex(e => e.IdTipoAccionPersonal)
                    .HasName("IX_AccionPersonal_IdTipoAccionPersonal");

                entity.Property(e => e.Explicacion).IsRequired();

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Solicitud).IsRequired();

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AccionPersonal)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoAccionPersonal)
                    .WithMany(p => p.AccionPersonal)
                    .HasForeignKey(d => d.IdTipoAccionPersonal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ActividadesAnalisisOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdActividadesAnalisisOcupacional)
                    .HasName("PK_ActividadesAnalisisOcupacional");

                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("IX_ActividadesAnalisisOcupacional_IdFormularioAnalisisOcupacional");

                entity.HasOne(d => d.FormularioAnalisisOcupacional)
                    .WithMany(p => p.ActividadesAnalisisOcupacional)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ActividadesEsenciales>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ActividadesGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdActividadesGestionCambio)
                    .HasName("PK_ActividadesGestionCambio");

                entity.HasIndex(e => e.IdPlanGestionCambio)
                    .HasName("IX_ActividadesGestionCambio_IdPlanGestionCambio");

                entity.Property(e => e.Descripcion).IsRequired();

                entity.HasOne(d => d.PlanGestionCambio)
                    .WithMany(p => p.ActividadesGestionCambio)
                    .HasForeignKey(d => d.IdPlanGestionCambio)
                    .OnDelete(DeleteBehavior.Restrict);
            });

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

            modelBuilder.Entity<AdministracionTalentoHumano>(entity =>
            {
                entity.HasKey(e => e.IdAdministracionTalentoHumano)
                    .HasName("PK_AdministracionTalentoHumano");

                entity.HasIndex(e => e.EmpleadoResponsable)
                    .HasName("IX_AdministracionTalentoHumano_EmpleadoIdEmpleado");

                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("IX_AdministracionTalentoHumano_idFormularioAnalisisOcupacional");

                entity.HasIndex(e => e.IdRolPuesto)
                    .HasName("IX_AdministracionTalentoHumano_idRolPuesto");

                entity.Property(e => e.IdFormularioAnalisisOcupacional).HasColumnName("idFormularioAnalisisOcupacional");

                entity.Property(e => e.IdRolPuesto).HasColumnName("idRolPuesto");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.EmpleadoResponsable);

                entity.HasOne(d => d.FormularioAnalisisOcupacional)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RolPuesto)
                    .WithMany(p => p.AdministracionTalentoHumano)
                    .HasForeignKey(d => d.IdRolPuesto)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnoExperiencia>(entity =>
            {
                entity.HasKey(e => e.IdAnoExperiencia)
                    .HasName("PK_AnoExperiencia");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AprobacionViatico>(entity =>
            {
                entity.HasKey(e => e.IdAprobacionViatico)
                    .HasName("PK_AprobacionViatico");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_AprobacionViatico_IdEmpleado");

                entity.HasIndex(e => e.IdSolicitudViatico)
                    .HasName("IX_AprobacionViatico_IdSolicitudViatico");
                                       
                entity.Property(e => e.ValorADescontar)
                    .HasColumnName("ValorADescontar")
                    .HasColumnType("decimal");

                entity.Property(e => e.ValorJustificado).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AprobacionViatico)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SolicitudViatico)
                    .WithMany(p => p.AprobacionViatico)
                    .HasForeignKey(d => d.IdSolicitudViatico)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AreaConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdAreaConocimiento)
                    .HasName("PK_AreaConocimiento");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
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


            modelBuilder.Entity<AvanceGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdAvanceGestionCambio)
                    .HasName("PK_AvanceGestionCambio");

                entity.HasIndex(e => e.IdActividadesGestionCambio)
                    .HasName("IX_AvanceGestionCambio_IdActividadesGestionCambio");

                entity.HasOne(d => d.ActividadesGestionCambio)
                    .WithMany(p => p.AvanceGestionCambio)
                    .HasForeignKey(d => d.IdActividadesGestionCambio)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BrigadaSSO>(entity =>
            {
                entity.HasKey(e => e.IdBrigadaSSO)
                    .HasName("PK_BrigadaSSO");

                entity.ToTable("BrigadaSSO");

                entity.Property(e => e.IdBrigadaSSO).HasColumnName("IdBrigadaSSO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BrigadaSSORol>(entity =>
            {
                entity.HasKey(e => e.IdBrigadaSSORol)
                    .HasName("PK_BrigadaSSORol");

                entity.ToTable("BrigadaSSORol");

                entity.HasIndex(e => e.IdBrigadaSSO)
                    .HasName("IX_BrigadaSSORol_IdBrigadaSSO");

                entity.Property(e => e.IdBrigadaSSORol).HasColumnName("IdBrigadaSSORol");

                entity.Property(e => e.IdBrigadaSSO).HasColumnName("IdBrigadaSSO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.BrigadaSSO)
                    .WithMany(p => p.BrigadaSSORol)
                    .HasForeignKey(d => d.IdBrigadaSSO)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CandidatoConcurso>(entity =>
            {
                entity.HasKey(e => e.IdCandidatoConcurso)
                    .HasName("PK_CandidatoConcurso");

                entity.HasIndex(e => e.IdCanditato)
                    .HasName("IX_CandidatoConcurso_IdCanditato");

                entity.HasIndex(e => e.IdPartidasFase)
                    .HasName("IX_CandidatoConcurso_IdPartidasFase");

                entity.Property(e => e.CodigoSocioEmpleo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Canditato)
                    .WithMany(p => p.CandidatoConcurso)
                    .HasForeignKey(d => d.IdCanditato)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.PartidasFase)
                    .WithMany(p => p.CandidatoConcurso)
                    .HasForeignKey(d => d.IdPartidasFase)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Canditato>(entity =>
            {
                entity.HasKey(e => e.IdCanditato)
                    .HasName("PK_Canditato");
            });

            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacion)
                    .HasName("PK_Capacitacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<CapacitacionAreaConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionAreaConocimiento)
                    .HasName("PK_CapacitacionAreaConocimiento");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<CapacitacionEncuesta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionEncuesta)
                    .HasName("PK_CapacitacionEncuesta");

                entity.HasIndex(e => e.IdCapacitacionRecibida)
                    .HasName("IX_CapacitacionEncuesta_IdCapacitacionRecibida");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_CapacitacionEncuesta_IdEmpleado");

                entity.Property(e => e.IdCapacitacionEncuesta).HasMaxLength(450);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.CapacitacionRecibida)
                    .WithMany(p => p.CapacitacionEncuesta)
                    .HasForeignKey(d => d.IdCapacitacionRecibida)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.CapacitacionEncuesta)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionModalidad>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionModalidad)
                    .HasName("PK_CapacitacionModalidad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<CapacitacionPlanificacion>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionPlanificacion)
                    .HasName("PK_CapacitacionPlanificacion");

                entity.HasIndex(e => e.IdCapacitacionModalidad)
                    .HasName("IX_CapacitacionPlanificacion_IdCapacitacionModalidad");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("IX_CapacitacionPlanificacion_IdCapacitacionTemario");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_CapacitacionPlanificacion_IdEmpleado");

                entity.Property(e => e.Presupuesto).HasColumnType("decimal");

                entity.HasOne(d => d.CapacitacionModalidad)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdCapacitacionModalidad)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CapacitacionTemario)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.CapacitacionPlanificacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionPregunta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionPregunta)
                    .HasName("PK_CapacitacionPregunta");

                entity.HasIndex(e => e.IdCapacitacionEncuesta)
                    .HasName("IX_CapacitacionPregunta_IdCapacitacionEncuesta");

                entity.HasIndex(e => e.IdCapacitacionTipoPregunta)
                    .HasName("IX_CapacitacionPregunta_IdCapacitacionTipoPregunta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.IdCapacitacionEncuesta).HasMaxLength(450);

                entity.HasOne(d => d.CapacitacionEncuesta)
                    .WithMany(p => p.CapacitacionPregunta)
                    .HasForeignKey(d => d.IdCapacitacionEncuesta);

                entity.HasOne(d => d.CapacitacionTipoPregunta)
                    .WithMany(p => p.CapacitacionPregunta)
                    .HasForeignKey(d => d.IdCapacitacionTipoPregunta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionProveedor>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionProveedor)
                    .HasName("PK_CapacitacionProveedor");

                entity.HasIndex(e => e.IdCapacitacionRecibida)
                    .HasName("IX_CapacitacionProveedor_IdCapacitacionRecibida");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasOne(d => d.CapacitacionRecibida)
                    .WithMany(p => p.CapacitacionProveedor)
                    .HasForeignKey(d => d.IdCapacitacionRecibida)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionRecibida>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionRecibida)
                    .HasName("PK_CapacitacionRecibida");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("IX_CapacitacionRecibida_IdCapacitacionTemario");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_CapacitacionRecibida_IdEmpleado");

                entity.HasOne(d => d.CapacitacionTemario)
                    .WithMany(p => p.CapacitacionRecibida)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.CapacitacionRecibida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionRespuesta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionRespuesta)
                    .HasName("PK_CapacitacionRespuesta");

                entity.HasIndex(e => e.IdCapacitacionPregunta)
                    .HasName("IX_CapacitacionRespuesta_IdCapacitacionPregunta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.CapacitacionPregunta)
                    .WithMany(p => p.CapacitacionRespuesta)
                    .HasForeignKey(d => d.IdCapacitacionPregunta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionTemario>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTemario)
                    .HasName("PK_CapacitacionTemario");

                entity.HasIndex(e => e.IdCapacitacionAreaConocimiento)
                    .HasName("IX_CapacitacionTemario_IdCapacitacionAreaConocimiento");

                entity.Property(e => e.Tema)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.CapacitacionAreaConocimiento)
                    .WithMany(p => p.CapacitacionTemario)
                    .HasForeignKey(d => d.IdCapacitacionAreaConocimiento)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionTemarioProveedor>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTemarioProveedor)
                    .HasName("PK_CapacitacionTemarioProveedor");

                entity.HasIndex(e => e.IdCapacitacionModalidad)
                    .HasName("IX_CapacitacionTemarioProveedor_IdCapacitacionModalidad");

                entity.HasIndex(e => e.IdCapacitacionProveedor)
                    .HasName("IX_CapacitacionTemarioProveedor_IdCapacitacionProveedor");

                entity.HasIndex(e => e.IdCapacitacionTemario)
                    .HasName("IX_CapacitacionTemarioProveedor_IdCapacitacionTemario");

                entity.Property(e => e.Costo).HasColumnType("decimal");

                entity.HasOne(d => d.CapacitacionModalidad)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionModalidad)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CapacitacionProveedor)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionProveedor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CapacitacionTemario)
                    .WithMany(p => p.CapacitacionTemarioProveedor)
                    .HasForeignKey(d => d.IdCapacitacionTemario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CapacitacionTipoPregunta>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacionTipoPregunta)
                    .HasName("PK_CapacitacionTipoPregunta");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
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

            modelBuilder.Entity<ComportamientoObservable>(entity =>
            {
                entity.HasIndex(e => e.DenominacionCompetenciaId)
                    .HasName("IX_ComportamientoObservable_DenominacionCompetenciaId");

                entity.HasIndex(e => e.NivelId)
                    .HasName("IX_ComportamientoObservable_NivelId");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DenominacionCompetencia)
                    .WithMany(p => p.ComportamientoObservable)
                    .HasForeignKey(d => d.DenominacionCompetenciaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Nivel)
                    .WithMany(p => p.ComportamientoObservable)
                    .HasForeignKey(d => d.NivelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConfiguracionViatico>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracionViatico)
                    .HasName("PK_ConfiguracionViatico");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_ConfiguracionViatico_IdDependencia");

                entity.Property(e => e.PorCientoAJustificar).HasColumnName("PorCientoAJustificar");

                entity.Property(e => e.ValorEntregadoPorDia).HasColumnType("decimal");

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.ConfiguracionViatico)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConfirmacionLectura>(entity =>
            {
                entity.HasKey(e => e.IdConfirmacionLectura)
                    .HasName("PK_ConfirmacionLectura");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_ConfirmacionLectura_IdEmpleado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.ConfirmacionLectura)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConocimientosAdicionales>(entity =>
            {
                entity.HasKey(e => e.IdConocimientosAdicionales)
                    .HasName("PK_ConocimientosAdicionales");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<DatosBancarios>(entity =>
            {
                entity.HasKey(e => e.IdDatosBancarios)
                    .HasName("PK_DatosBancarios");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_DatosBancarios_IdEmpleado");

                entity.HasIndex(e => e.IdInstitucionFinanciera)
                    .HasName("IX_DatosBancarios_IdInstitucionFinanciera");

                entity.Property(e => e.NumeroCuenta)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.DatosBancarios)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.InstitucionFinanciera)
                    .WithMany(p => p.DatosBancarios)
                    .HasForeignKey(d => d.IdInstitucionFinanciera)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DeclaracionPatrimonioPersonal>(entity =>
            {
                entity.HasKey(e => e.IdDeclaracionPatrimonioPersonal)
                    .HasName("PK_DeclaracionPatrimonioPersonal");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_DeclaracionPatrimonioPersonal_IdEmpleado");

                entity.Property(e => e.PromedioMensualIngresos).HasColumnType("decimal");

                entity.Property(e => e.PromedioMensualOtrosIngresos).HasColumnType("decimal");

                entity.Property(e => e.TotalActivosAnioActual).HasColumnType("decimal");

                entity.Property(e => e.TotalActivosAnioAnterior).HasColumnType("decimal");

                entity.Property(e => e.TotalPasivosAnioActual).HasColumnType("decimal");

                entity.Property(e => e.TotalPasivosAnioAnterior).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.DeclaracionPatrimonioPersonal)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DenominacionCompetencia>(entity =>
            {
                entity.HasKey(e => e.IdDenominacionCompetencia)
                    .HasName("PK_DenominacionCompetencia");

                entity.Property(e => e.Definicion).IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);
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
                    .WithMany(p => p.Dependencia1)
                    .HasForeignKey(d => d.IdDependenciaPadre);

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.Dependencia)
                    .HasForeignKey(d => d.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DependenciaDocumento>(entity =>
            {
                entity.HasKey(e => e.IdDependenciaDocumento)
                    .HasName("PK_DependenciaDocumento");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_DependenciaDocumento_IdDependencia");

                entity.HasIndex(e => e.IdDocumentosSubidos)
                    .HasName("IX_DependenciaDocumento_IdDocumentosSubidos");

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.DependenciaDocumento)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.DocumentosParentescodos)
                    .WithMany(p => p.DependenciaDocumento)
                    .HasForeignKey(d => d.IdDocumentosSubidos)
                    .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<Destreza>(entity =>
            {
                entity.HasKey(e => e.IdDestreza)
                    .HasName("PK_Destreza");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<DetalleExamenInduccion>(entity =>
            {
                entity.HasKey(e => e.IdDetalleExamenInduccion)
                    .HasName("PK_DetalleExamenInduccion");

                entity.HasIndex(e => e.PreguntaId)
                    .HasName("IX_DetalleExamenInduccion_PreguntaId");

                entity.HasIndex(e => e.RealizaExamenInduccionId)
                    .HasName("IX_DetalleExamenInduccion_RealizaExamenInduccionId");

                entity.HasIndex(e => e.RespuestaId)
                    .HasName("IX_DetalleExamenInduccion_RespuestaId");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.PreguntaId);

                entity.HasOne(d => d.RealizaExamenInduccion)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.RealizaExamenInduccionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Respuesta)
                    .WithMany(p => p.DetalleExamenInduccion)
                    .HasForeignKey(d => d.RespuestaId)
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

            modelBuilder.Entity<DocumentosParentescodos>(entity =>
            {
                entity.HasKey(e => e.IdDocumentosSubidos)
                    .HasName("PK_DocumentosParentescodos");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_DocumentosParentescodos_IdEmpleado");

                entity.Property(e => e.Are)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Ubicacion).IsRequired();

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.DocumentosParentescodos)
                    .HasForeignKey(d => d.IdEmpleado)
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

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_Empleado_IdPersona");

                entity.HasIndex(e => e.IdProvinciaLugarSufragio)
                    .HasName("IX_Empleado_ProvinciaSufragioIdProvincia");

                entity.Property(e => e.IngresosOtraActividad)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.CiudadNacimiento)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdCiudadLugarNacimiento);

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);

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

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoContactoEmergencia>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoContactoEmergencia)
                    .HasName("PK_EmpleadoContactoEmergencia");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoContactoEmergencia_IdEmpleado");

                entity.HasIndex(e => e.IdParentesco)
                    .HasName("IX_EmpleadoContactoEmergencia_IdParentesco");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_EmpleadoContactoEmergencia_IdPersona");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Parentesco)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdParentesco)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.EmpleadoContactoEmergencia)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoFamiliar>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoFamiliar)
                    .HasName("PK_EmpleadoFamiliar");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoFamiliar_IdEmpleado");

                entity.HasIndex(e => e.IdParentesco)
                    .HasName("IX_EmpleadoFamiliar_IdParentesco");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_EmpleadoFamiliar_IdPersona");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Parentesco)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdParentesco)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.EmpleadoFamiliar)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoFormularioCapacitacion>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoFormularioCapacitacion)
                    .HasName("PK_EmpleadoFormularioCapacitacion");

                entity.HasIndex(e => e.IdFormularioCapacitacion)
                    .HasName("IX_EmpleadoFormularioCapacitacion_IdFormularioCapacitacion");

                entity.HasIndex(e => e.IdServidor)
                    .HasName("IX_EmpleadoFormularioCapacitacion_ServidorIdEmpleado");

                entity.HasOne(d => d.FormularioCapacitacion)
                    .WithMany(p => p.EmpleadoFormularioCapacitacion)
                    .HasForeignKey(d => d.IdFormularioCapacitacion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Servidor)
                    .WithMany(p => p.EmpleadoFormularioCapacitacion)
                    .HasForeignKey(d => d.IdServidor);
            });

            modelBuilder.Entity<EmpleadoIE>(entity =>
            {
                entity.HasKey(e => e.IdEmpeladoIE)
                    .HasName("PK_EmpleadoIE");

                entity.ToTable("EmpleadoIE");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoIE_IdEmpleado");

                entity.HasIndex(e => e.IdIngresoEgresoRMU)
                    .HasName("IX_EmpleadoIE_IdIngresoEgresoRMU");

                entity.Property(e => e.IdEmpeladoIE).HasColumnName("IdEmpeladoIE");

                entity.Property(e => e.IdIngresoEgresoRMU).HasColumnName("IdIngresoEgresoRMU");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoIE)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IngresoEgresoRMU)
                    .WithMany(p => p.EmpleadoIE)
                    .HasForeignKey(d => d.IdIngresoEgresoRMU)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoImpuestoRenta>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoImpuestoRenta)
                    .HasName("PK_EmpleadoImpuestoRenta");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoImpuestoRenta_IdEmpleado");

                entity.Property(e => e.IngresoTotal).HasColumnType("decimal");

                entity.Property(e => e.OtrosIngresos).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoImpuestoRenta)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoMovimiento>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoMovimiento)
                    .HasName("PK_EmpleadoMovimiento");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoMovimiento_IdEmpleado");

                entity.HasIndex(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("IX_EmpleadoMovimiento_IdIndiceOcupacionalModalidadPartida");

                entity.HasIndex(e => e.IdTipoMovimientoInterno)
                    .HasName("IX_EmpleadoMovimiento_IdTipoMovimientoInterno");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacionalModalidadPartida)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdIndiceOcupacionalModalidadPartida)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoMovimientoInterno)
                    .WithMany(p => p.EmpleadoMovimiento)
                    .HasForeignKey(d => d.IdTipoMovimientoInterno)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoNepotismo>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoNepotismo)
                    .HasName("PK_EmpleadoNepotismo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoNepotismo_IdEmpleado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoNepotismo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoSaldoVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadoSaldoVacaciones)
                    .HasName("PK_EmpleadoSaldoVacaciones");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadoSaldoVacaciones_IdEmpleado");

                entity.Property(e => e.SaldoDias).HasColumnType("decimal");

                entity.Property(e => e.SaldoMonetario).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadoSaldoVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadosFormularioDevengacion>(entity =>
            {
                entity.HasKey(e => e.IdEmpleadosFormularioDevengacion)
                    .HasName("PK_EmpleadosFormularioDevengacion");

                entity.HasIndex(e => e.FormularioDevengacionId)
                    .HasName("IX_EmpleadosFormularioDevengacion_FormularioDevengacionId");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_EmpleadosFormularioDevengacion_IdEmpleado");

                entity.HasOne(d => d.FormularioDevengacion)
                    .WithMany(p => p.EmpleadosFormularioDevengacion)
                    .HasForeignKey(d => d.FormularioDevengacionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.EmpleadosFormularioDevengacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EscalaEvaluacionTotal>(entity =>
            {
                entity.HasKey(e => e.IdEscalaEvaluacionTotal)
                    .HasName("PK_EscalaEvaluacionTotal");

                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PorcientoDesde).HasColumnType("decimal");

                entity.Property(e => e.PorcientoHasta).HasColumnType("decimal");
            });

            modelBuilder.Entity<EscalaGrados>(entity =>
            {
                entity.HasKey(e => e.IdEscalaGrados)
                    .HasName("PK_EscalaGrados");

                entity.HasIndex(e => e.IdGrupoOcupacional)
                    .HasName("IX_EscalaGrados_IdGrupoOcupacional");

                entity.Property(e => e.Remuneracion).HasColumnType("decimal");

                entity.HasOne(d => d.GrupoOcupacional)
                    .WithMany(p => p.EscalaGrados)
                    .HasForeignKey(d => d.IdGrupoOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EspecificidadExperiencia>(entity =>
            {
                entity.HasKey(e => e.IdEspecificidadExperiencia)
                    .HasName("PK_EspecificidadExperiencia");

                entity.Property(e => e.Descripcion)
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

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCivil)
                    .HasName("PK_EstadoCivil");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Estudio>(entity =>
            {
                entity.HasKey(e => e.IdEstudio)
                    .HasName("PK_Estudio");

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

            modelBuilder.Entity<Eval001>(entity =>
            {
                entity.HasKey(e => e.IdEval001)
                    .HasName("PK_Eval001");

                entity.HasIndex(e => e.IdEmpleadoEvaluado)
                    .HasName("IX_Eval001_EmpleadoIdEmpleado");

                entity.HasIndex(e => e.IdEscalaEvaluacionTotal)
                    .HasName("IX_Eval001_IdEscalaEvaluacionTotal");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("IX_Eval001_IdEvaluacionActividadesPuestoTrabajo");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("IX_Eval001_IdEvaluacionCompetenciasTecnicasPuesto");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("IX_Eval001_IdEvaluacionCompetenciasUniversales");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("IX_Eval001_IdEvaluacionConocimiento");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("IX_Eval001_IdEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.HasIndex(e => e.IdEvaluador)
                    .HasName("IX_Eval001_IdEvaluador");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEmpleadoEvaluado);

                entity.HasOne(d => d.EscalaEvaluacionTotal)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEscalaEvaluacionTotal);

                entity.HasOne(d => d.EvaluacionActividadesPuestoTrabajo)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo);

                entity.HasOne(d => d.EvaluacionCompetenciasTecnicasPuesto)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto);

                entity.HasOne(d => d.EvaluacionCompetenciasUniversales)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales);

                entity.HasOne(d => d.EvaluacionConocimiento)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento);

                entity.HasOne(d => d.EvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo);

                entity.HasOne(d => d.Evaluador)
                    .WithMany(p => p.Eval001)
                    .HasForeignKey(d => d.IdEvaluador)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajo>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("PK_EvaluacionActividadesPuestoTrabajo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajoDetalle)
                    .HasName("PK_EvaluacionActividadesPuestoTrabajoDetalle");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("IX_EvaluacionActividadesPuestoTrabajoDetalle_IdEvaluacionActividadesPuestoTrabajo");

                entity.HasIndex(e => e.IdIndicador)
                    .HasName("IX_EvaluacionActividadesPuestoTrabajoDetalle_IdIndicador");

                entity.Property(e => e.DescripcionActividad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EvaluacionActividadesPuestoTrabajo)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Indicador)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoDetalle)
                    .HasForeignKey(d => d.IdIndicador);
            });

            modelBuilder.Entity<EvaluacionActividadesPuestoTrabajoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionActividadesPuestoTrabajoFactor)
                    .HasName("PK_EvaluacionActividadesPuestoTrabajoFactor");

                entity.HasIndex(e => e.IdEvaluacionActividadesPuestoTrabajo)
                    .HasName("IX_EvaluacionActividadesPuestoTrabajoFactor_IdEvaluacionActividadesPuestoTrabajo");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("IX_EvaluacionActividadesPuestoTrabajoFactor_IdFactor");

                entity.HasOne(d => d.EvaluacionActividadesPuestoTrabajo)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoFactor)
                    .HasForeignKey(d => d.IdEvaluacionActividadesPuestoTrabajo);

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.EvaluacionActividadesPuestoTrabajoFactor)
                    .HasForeignKey(d => d.IdFactor);
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuesto>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("PK_EvaluacionCompetenciasTecnicasPuesto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuestoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasName("PK_EvaluacionCompetenciasTecnicasPuestoDetalle");

                entity.HasIndex(e => e.IdDestreza)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoDetalle_IdDestreza");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoDetalle_IdEvaluacionCompetenciasTecnicasPuesto");

                entity.HasIndex(e => e.IdNivelDesarrollo)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoDetalle_IdNivelDesarrollo");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoDetalle_IdRelevancia");

                entity.HasOne(d => d.Destreza)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdDestreza);

                entity.HasOne(d => d.EvaluacionCompetenciasTecnicasPuesto)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.NivelDesarrollo)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdNivelDesarrollo);

                entity.HasOne(d => d.Relevancia)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoDetalle)
                    .HasForeignKey(d => d.IdRelevancia);
            });

            modelBuilder.Entity<EvaluacionCompetenciasTecnicasPuestoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasName("PK_EvaluacionCompetenciasTecnicasPuestoFactor");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasTecnicasPuesto)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoFactor_IdEvaluacionCompetenciasTecnicasPuesto");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("IX_EvaluacionCompetenciasTecnicasPuestoFactor_IdFactor");

                entity.HasOne(d => d.EvaluacionCompetenciasTecnicasPuesto)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasTecnicasPuesto);

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.EvaluacionCompetenciasTecnicasPuestoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversales>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("PK_EvaluacionCompetenciasUniversales");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversalesDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversalesDetalle)
                    .HasName("PK_EvaluacionCompetenciasUniversalesDetalle");

                entity.HasIndex(e => e.IdDestreza)
                    .HasName("IX_EvaluacionCompetenciasUniversalesDetalle_IdDestreza");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("IX_EvaluacionCompetenciasUniversalesDetalle_IdEvaluacionCompetenciasUniversales");

                entity.HasIndex(e => e.IdFrecuenciaAplicacion)
                    .HasName("IX_EvaluacionCompetenciasUniversalesDetalle_IdFrecuenciaAplicacion");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("IX_EvaluacionCompetenciasUniversalesDetalle_IdRelevancia");

                entity.HasOne(d => d.Destreza)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdDestreza);

                entity.HasOne(d => d.EvaluacionCompetenciasUniversales)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EvaluacionCompUnivlesDetalle_EvaluacionCompsUni_IdEvalnCompsUniversales");

                entity.HasOne(d => d.FrecuenciaAplicacion)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdFrecuenciaAplicacion);

                entity.HasOne(d => d.Relevancia)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesDetalle)
                    .HasForeignKey(d => d.IdRelevancia);
            });

            modelBuilder.Entity<EvaluacionCompetenciasUniversalesFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionCompetenciasUniversalesFactor)
                    .HasName("PK_EvaluacionCompetenciasUniversalesFactor");

                entity.HasIndex(e => e.IdEvaluacionCompetenciasUniversales)
                    .HasName("IX_EvaluacionCompetenciasUniversalesFactor_IdEvaluacionCompetenciasUniversales");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("IX_EvaluacionCompetenciasUniversalesFactor_IdFactor");

                entity.HasOne(d => d.EvaluacionCompetenciasUniversales)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesFactor)
                    .HasForeignKey(d => d.IdEvaluacionCompetenciasUniversales)
                    .HasConstraintName("FK_EvaluacionCompetenciasUniversalesFactor_EvaluacionCompetenciasUniversales_IdEvaCompUnives");

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.EvaluacionCompetenciasUniversalesFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EvaluacionConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimiento)
                    .HasName("PK_EvaluacionConocimiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EvaluacionConocimientoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimientoDetalle)
                    .HasName("PK_EvaluacionConocimientoDetalle");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("IX_EvaluacionConocimientoDetalle_IdEvaluacionConocimiento");

                entity.HasIndex(e => e.IdNivelConocimiento)
                    .HasName("IX_EvaluacionConocimientoDetalle_IdNivelConocimiento");

                entity.HasOne(d => d.EvaluacionConocimiento)
                    .WithMany(p => p.EvaluacionConocimientoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.NivelConocimiento)
                    .WithMany(p => p.EvaluacionConocimientoDetalle)
                    .HasForeignKey(d => d.IdNivelConocimiento);
            });

            modelBuilder.Entity<EvaluacionConocimientoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionConocimientoFactor)
                    .HasName("PK_EvaluacionConocimientoFactor");

                entity.HasIndex(e => e.IdEvaluacionConocimiento)
                    .HasName("IX_EvaluacionConocimientoFactor_IdEvaluacionConocimiento");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("IX_EvaluacionConocimientoFactor_IdFactor");

                entity.HasOne(d => d.EvaluacionConocimiento)
                    .WithMany(p => p.EvaluacionConocimientoFactor)
                    .HasForeignKey(d => d.IdEvaluacionConocimiento);

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.EvaluacionConocimientoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EvaluacionInducion>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionInduccion)
                    .HasName("PK_EvaluacionInducion");

                entity.Property(e => e.MinimoAprobar).HasColumnName("MinimoAprobar");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgo>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("PK_EvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ObservacionesJefeInmediato).IsRequired();
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasName("PK_EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle_IdEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.HasIndex(e => e.IdFrecuenciaAplicacion)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle_IdFrecuenciaAplicacion");

                entity.HasIndex(e => e.IdRelevancia)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle_IdRelevancia");

                entity.HasIndex(e => e.IdTrabajoEquipoIniciativaLiderazgo)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle_IdTrabajoEquipoIniciativaLiderazgo");

                entity.HasOne(d => d.EvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasConstraintName("FK_EvaluacionTrabajoEquIniLidDetall_EvalTrabEquiIniLid_IdEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.HasOne(d => d.FrecuenciaAplicacion)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdFrecuenciaAplicacion);

                entity.HasOne(d => d.Relevancia)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdRelevancia);

                entity.HasOne(d => d.TrabajoEquipoIniciativaLiderazgo)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoDetalle)
                    .HasForeignKey(d => d.IdTrabajoEquipoIniciativaLiderazgo);
            });

            modelBuilder.Entity<EvaluacionTrabajoEquipoIniciativaLiderazgoFactor>(entity =>
            {
                entity.HasKey(e => e.IdEvalTjoEquiInicLidFac)
                    .HasName("PK_EvaluacionTrabajoEquipoIniciativaLiderazgoFactor");

                entity.HasIndex(e => e.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoFactor_IdEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.HasIndex(e => e.IdFactor)
                    .HasName("IX_EvaluacionTrabajoEquipoIniciativaLiderazgoFactor_IdFactor");

                entity.HasOne(d => d.EvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoFactor)
                    .HasForeignKey(d => d.IdEvaluacionTrabajoEquipoIniciativaLiderazgo)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EvalTrabEqIniLidFac_EvaTrabEquiIniLid_IdEvaluacionTrabajoEquipoIniciativaLiderazgo");

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.EvaluacionTrabajoEquipoIniciativaLiderazgoFactor)
                    .HasForeignKey(d => d.IdFactor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Evaluador>(entity =>
            {
                entity.HasKey(e => e.IdEvaluador)
                    .HasName("PK_Evaluador");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_Evaluador_IdDependencia");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_Evaluador_IdEmpleado");

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.Evaluador)
                    .HasForeignKey(d => d.IdDependencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Evaluador)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Exepciones>(entity =>
            {
                entity.HasKey(e => e.IdExepciones)
                    .HasName("PK_Exepciones");

                entity.HasIndex(e => e.ValidacionJefeId)
                    .HasName("IX_Exepciones_ValidacionInmediatoSuperiorIdValidacionJefe");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.ValidacionInmediatoSuperior)
                    .WithMany(p => p.Exepciones)
                    .HasForeignKey(d => d.ValidacionJefeId);
            });

            modelBuilder.Entity<ExperienciaLaboralRequerida>(entity =>
            {
                entity.HasKey(e => e.IdExperienciaLaboralRequerida)
                    .HasName("PK_ExperienciaLaboralRequerida");

                entity.HasIndex(e => e.IdAnoExperiencia)
                    .HasName("IX_ExperienciaLaboralRequerida_IdAnoExperiencia");

                entity.HasIndex(e => e.IdEspecificidadExperiencia)
                    .HasName("IX_ExperienciaLaboralRequerida_IdEspecificidadExperiencia");

                entity.HasIndex(e => e.IdEstudio)
                    .HasName("IX_ExperienciaLaboralRequerida_IdEstudio");

                entity.HasIndex(e => e.IdIndiceOcupacionalCapacitaciones)
                    .HasName("IX_ExperienciaLaboralRequerida_IdIndiceOcupacionalCapacitaciones");

                entity.HasOne(d => d.AnoExperiencia)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IdAnoExperiencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.EspecificidadExperiencia)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IdEspecificidadExperiencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estudio)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacionalCapacitaciones)
                    .WithMany(p => p.ExperienciaLaboralRequerida)
                    .HasForeignKey(d => d.IdIndiceOcupacionalCapacitaciones)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Factor>(entity =>
            {
                entity.HasKey(e => e.IdFactor)
                    .HasName("PK_Factor");

                entity.Property(e => e.Porciento).HasColumnType("decimal");
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

            modelBuilder.Entity<FacturaViatico>(entity =>
            {
                entity.HasKey(e => e.IdFacturaViatico)
                    .HasName("PK_FacturaViatico");

                entity.HasIndex(e => e.AprobadoPor)
                    .HasName("IX_FacturaViatico_EmpleadoIdEmpleado");

                entity.HasIndex(e => e.IdItemViatico)
                    .HasName("IX_FacturaViatico_IdItemViatico");

                entity.HasIndex(e => e.ItinerarioViaticoId)
                    .HasName("IX_FacturaViatico_ItinerarioViaticoId");

                entity.Property(e => e.NumeroFactura)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Observaciones).IsRequired();

                entity.Property(e => e.ValorTotalAprobacion).HasColumnType("decimal");

                entity.Property(e => e.ValorTotalFactura).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.AprobadoPor);

                entity.HasOne(d => d.ItemViatico)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.IdItemViatico)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ItinerarioViatico)
                    .WithMany(p => p.FacturaViatico)
                    .HasForeignKey(d => d.ItinerarioViaticoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FaseConcurso>(entity =>
            {
                entity.HasKey(e => e.IdFaseConcurso)
                    .HasName("PK_FaseConcurso");

                entity.HasIndex(e => e.IdTipoConcurso)
                    .HasName("IX_FaseConcurso_IdTipoConcurso");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TipoConcurso)
                    .WithMany(p => p.FaseConcurso)
                    .HasForeignKey(d => d.IdTipoConcurso)
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

            modelBuilder.Entity<FormularioAnalisisOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("PK_FormularioAnalisisOcupacional");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_FormularioAnalisisOcupacional_IdEmpleado");

                entity.Property(e => e.MisionPuesto).IsRequired();

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.FormularioAnalisisOcupacional)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FormularioCapacitacion>(entity =>
            {
                entity.HasKey(e => e.IdFormularioCapacitacion)
                    .HasName("PK_FormularioCapacitacion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FormularioDevengacion>(entity =>
            {
                entity.HasKey(e => e.IdFormularioDevengacion)
                    .HasName("PK_FormularioDevengacion");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_FormularioDevengacion_IdEmpleado");


                entity.HasIndex(e => e.AnalistaDesarrolloInstitucional)
                    .HasName("IX_FormularioDevengacion_IdEmpleadoDesarrolloInstitucional");

                entity.HasIndex(e => e.ResponsableArea)
                    .HasName("IX_FormularioDevengacion_IdEmpleadoResponsableArea");

                entity.HasIndex(e => e.ModosScializacionId)
                    .HasName("IX_FormularioDevengacion_ModosScializacionId");

                entity.Property(e => e.ModoSocial)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.FormularioDevengacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.EmpleadoDesarrolloInstitucional)
                    .WithMany(p => p.FormularioDevengacion1)
                    .HasForeignKey(d => d.AnalistaDesarrolloInstitucional)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.EmpleadoResponsableArea)
                   .WithMany(p => p.FormularioDevengacion2)
                   .HasForeignKey(d => d.ResponsableArea)
                   .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(d => d.ModosScializacion)
                    .WithMany(p => p.FormularioDevengacion)
                    .HasForeignKey(d => d.ModosScializacionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FormulasRMU>(entity =>
            {
                entity.HasKey(e => e.IdFormulaRMU)
                    .HasName("PK_FormulasRMU");

                entity.ToTable("FormulasRMU");

                entity.Property(e => e.IdFormulaRMU).HasColumnName("IdFormulaRMU");

                entity.Property(e => e.Formula)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<FrecuenciaAplicacion>(entity =>
            {
                entity.HasKey(e => e.IdFrecuenciaAplicacion)
                    .HasName("PK_FrecuenciaAplicacion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<GastoRubro>(entity =>
            {
                entity.HasKey(e => e.IdGastoRubro)
                    .HasName("PK_GastoRubro");

                entity.HasIndex(e => e.IdEmpleadoImpuestoRenta)
                    .HasName("IX_GastoRubro_IdEmpleadoImpuestoRenta");

                entity.HasIndex(e => e.IdRubro)
                    .HasName("IX_GastoRubro_IdRubro");

                entity.Property(e => e.GastoProyectado).HasColumnType("decimal");

                entity.HasOne(d => d.EmpleadoImpuestoRenta)
                    .WithMany(p => p.GastoRubro)
                    .HasForeignKey(d => d.IdEmpleadoImpuestoRenta)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Rubro)
                    .WithMany(p => p.GastoRubro)
                    .HasForeignKey(d => d.IdRubro);
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero)
                    .HasName("PK_Genero");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<GrupoOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdGrupoOcupacional)
                    .HasName("PK_GrupoOcupacional");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Indicador>(entity =>
            {
                entity.HasKey(e => e.IdIndicador)
                    .HasName("PK_Indicador");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<IndiceOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacional)
                    .HasName("PK_IndiceOcupacional");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_IndiceOcupacional_IdDependencia");

                entity.HasIndex(e => e.IdEscalaGrados)
                    .HasName("IX_IndiceOcupacional_IdEscalaGrados");

                entity.HasIndex(e => e.IdManualPuesto)
                    .HasName("IX_IndiceOcupacional_IdManualPuesto");

                entity.HasIndex(e => e.IdRolPuesto)
                    .HasName("IX_IndiceOcupacional_IdRolPuesto");

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdDependencia);

                entity.HasOne(d => d.EscalaGrados)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdEscalaGrados);

                entity.HasOne(d => d.ManualPuesto)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdManualPuesto);

                entity.HasOne(d => d.RolPuesto)
                    .WithMany(p => p.IndiceOcupacional)
                    .HasForeignKey(d => d.IdRolPuesto);
            });

            modelBuilder.Entity<IndiceOcupacionalActividadesEsenciales>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalActividadesEsenciales)
                    .HasName("PK_IndiceOcupacionalActividadesEsenciales");

                entity.HasIndex(e => e.IdActividadesEsenciales)
                    .HasName("IX_IndiceOcupacionalActividadesEsenciales_IdActividadesEsenciales");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalActividadesEsenciales_IdIndiceOcupacional");

                entity.HasOne(d => d.ActividadesEsenciales)
                    .WithMany(p => p.IndiceOcupacionalActividadesEsenciales)
                    .HasForeignKey(d => d.IdActividadesEsenciales);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalActividadesEsenciales)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalAreaConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalAreaConocimiento)
                    .HasName("PK_IndiceOcupacionalAreaConocimiento");

                entity.HasIndex(e => e.IdAreaConocimiento)
                    .HasName("IX_IndiceOcupacionalAreaConocimiento_IdAreaConocimiento");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalAreaConocimiento_IdIndiceOcupacional");

                entity.HasOne(d => d.AreaConocimiento)
                    .WithMany(p => p.IndiceOcupacionalAreaConocimiento)
                    .HasForeignKey(d => d.IdAreaConocimiento)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalAreaConocimiento)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalCapacitaciones>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalCapacitaciones)
                    .HasName("PK_IndiceOcupacionalCapacitaciones");

                entity.HasIndex(e => e.IdCapacitacion)
                    .HasName("IX_IndiceOcupacionalCapacitaciones_IdCapacitacion");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalCapacitaciones_IdIndiceOcupacional");

                entity.HasOne(d => d.Capacitacion)
                    .WithMany(p => p.IndiceOcupacionalCapacitaciones)
                    .HasForeignKey(d => d.IdCapacitacion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalCapacitaciones)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalComportamientoObservable>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalComportamientoObservable)
                    .HasName("PK_IndiceOcupacionalComportamientoObservable");

                entity.HasIndex(e => e.IdComportamientoObservable)
                    .HasName("IX_IndiceOcupacionalComportamientoObservable_ComportamientoObservableId");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalComportamientoObservable_IdIndiceOcupacional");

                entity.HasOne(d => d.ComportamientoObservable)
                    .WithMany(p => p.IndiceOcupacionalComportamientoObservable)
                    .HasForeignKey(d => d.IdComportamientoObservable);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalComportamientoObservable)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalConocimientosAdicionales>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalConocimientosAdicionales)
                    .HasName("PK_IndiceOcupacionalConocimientosAdicionales");

                entity.HasIndex(e => e.IdConocimientosAdicionales)
                    .HasName("IX_IndiceOcupacionalConocimientosAdicionales_IdConocimientosAdicionales");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalConocimientosAdicionales_IdIndiceOcupacional");

                entity.HasOne(d => d.ConocimientosAdicionales)
                    .WithMany(p => p.IndiceOcupacionalConocimientosAdicionales)
                    .HasForeignKey(d => d.IdConocimientosAdicionales)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalConocimientosAdicionales)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalEstudio>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalEstudio)
                    .HasName("PK_IndiceOcupacionalEstudio");

                entity.HasIndex(e => e.IdEstudio)
                    .HasName("IX_IndiceOcupacionalEstudio_IdEstudio");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalEstudio_IdIndiceOcupacional");

                entity.HasOne(d => d.Estudio)
                    .WithMany(p => p.IndiceOcupacionalEstudio)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalEstudio)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IndiceOcupacionalModalidadPartida>(entity =>
            {
                entity.HasKey(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("PK_IndiceOcupacionalModalidadPartida");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_IndiceOcupacionalModalidadPartida_IdEmpleado");

                entity.HasIndex(e => e.IdFondoFinanciamiento)
                    .HasName("IX_IndiceOcupacionalModalidadPartida_IdFondoFinanciamiento");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_IndiceOcupacionalModalidadPartida_IdIndiceOcupacional");

                entity.HasIndex(e => e.IdModalidadPartida)
                    .HasName("IX_IndiceOcupacionalModalidadPartida_IdModalidadPartida");

                entity.HasIndex(e => e.IdTipoNombramiento)
                    .HasName("IX_IndiceOcupacionalModalidadPartida_IdTipoNombramiento");

                entity.Property(e => e.SalarioReal).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FondoFinanciamiento)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdFondoFinanciamiento);

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ModalidadPartida)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdModalidadPartida);

                entity.HasOne(d => d.TipoNombramiento)
                    .WithMany(p => p.IndiceOcupacionalModalidadPartida)
                    .HasForeignKey(d => d.IdTipoNombramiento);
            });

            modelBuilder.Entity<InformeUATH>(entity =>
            {
                entity.HasKey(e => e.IdInformeUATH)
                    .HasName("PK_InformeUATH");

                entity.ToTable("InformeUATH");

                entity.HasIndex(e => e.IdAdministracionTalentoHumano)
                    .HasName("IX_InformeUATH_IdAdministracionTalentoHumano");

                entity.HasIndex(e => e.IdManualPuestoOrigen)
                    .HasName("IX_InformeUATH_IdManualPuestoOrigen");

                entity.HasIndex(e => e.IdManualPuestoDestino)
                   .HasName("IX_InformeUATH_IdManualPuestoDestino");

                entity.Property(e => e.IdInformeUATH).HasColumnName("IdInformeUATH");

                entity.HasOne(d => d.AdministracionTalentoHumano)
                    .WithMany(p => p.InformeUATH)
                    .HasForeignKey(d => d.IdAdministracionTalentoHumano)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ManualPuestoOrigen)
                    .WithMany(p => p.InformeUATH1)
                    .HasForeignKey(d => d.IdManualPuestoOrigen)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ManualPuestoDestino)
                   .WithMany(p => p.InformeUATH)
                   .HasForeignKey(d => d.IdManualPuestoDestino)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InformeViatico>(entity =>
            {
                entity.HasKey(e => e.IdInformeViatico)
                    .HasName("PK_InformeViatico");

                entity.HasIndex(e => e.IdItinerarioViatico)
                    .HasName("IX_InformeViatico_IdItinerarioViatico");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.ItinerarioViatico)
                    .WithMany(p => p.InformeViatico)
                    .HasForeignKey(d => d.IdItinerarioViatico)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IngresoEgresoRMU>(entity =>
            {
                entity.HasKey(e => e.IdIngresoEgresoRMU)
                    .HasName("PK_IngresoEgresoRMU");

                entity.ToTable("IngresoEgresoRMU");

                entity.HasIndex(e => e.IdFormulaRMU)
                    .HasName("IX_IngresoEgresoRMU_FormulasRMUIdFormulaRMU");

                entity.Property(e => e.IdIngresoEgresoRMU).HasColumnName("IdIngresoEgresoRMU");

                entity.Property(e => e.CuentaContable)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);

              

                entity.Property(e => e.IdFormulaRMU).HasColumnName("IdFormulaRMU");

                entity.HasOne(d => d.FormulasRMU)
                    .WithMany(p => p.IngresoEgresoRMU)
                    .HasForeignKey(d => d.IdFormulaRMU);
            });

            modelBuilder.Entity<InstitucionFinanciera>(entity =>
            {
                entity.HasKey(e => e.IdInstitucionFinanciera)
                    .HasName("PK_InstitucionFinanciera");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ItemViatico>(entity =>
            {
                entity.HasKey(e => e.IdItemViatico)
                    .HasName("PK_ItemViatico");

                entity.Property(e => e.Descipcion)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ItinerarioViatico>(entity =>
            {
                entity.HasKey(e => e.IdItinerarioViatico)
                    .HasName("PK_ItinerarioViatico");

                entity.HasIndex(e => e.IdCiudad)
                    .HasName("IX_ItinerarioViatico_IdCiudad");

                entity.HasIndex(e => e.IdPais)
                    .HasName("IX_ItinerarioViatico_IdPais");

                entity.HasIndex(e => e.IdSolicitudViatico)
                    .HasName("IX_ItinerarioViatico_IdSolicitudViatico");

                entity.HasIndex(e => e.IdTipoTransporte)
                    .HasName("IX_ItinerarioViatico_IdTipoTransporte");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Ciudad)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdCiudad)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Pais)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SolicitudViatico)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdSolicitudViatico)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoTransporte)
                    .WithMany(p => p.ItinerarioViatico)
                    .HasForeignKey(d => d.IdTipoTransporte)
                    .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<Liquidacion>(entity =>
            {
                entity.HasKey(e => e.IdLiquidacion)
                    .HasName("PK_Liquidacion");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_Liquidacion_IdEmpleado");

                entity.HasIndex(e => e.IdRubroLiquidacion)
                    .HasName("IX_Liquidacion_IdRubroLiquidacion");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Liquidacion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RubroLiquidacion)
                    .WithMany(p => p.Liquidacion)
                    .HasForeignKey(d => d.IdRubroLiquidacion)
                    .OnDelete(DeleteBehavior.Restrict);
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

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.MantenimientoActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ManualPuesto>(entity =>
            {
                entity.HasKey(e => e.IdManualPuesto)
                    .HasName("PK_ManualPuesto");

                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK_Marca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MaterialApoyo>(entity =>
            {
                entity.HasKey(e => e.IdMaterialApoyo)
                    .HasName("PK_MaterialApoyo");

                entity.HasIndex(e => e.FormularioDevengacionId)
                    .HasName("IX_MaterialApoyo_FormularioDevengacionId");

                entity.HasOne(d => d.FormularioDevengacion)
                    .WithMany(p => p.MaterialApoyo)
                    .HasForeignKey(d => d.FormularioDevengacionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Mision>(entity =>
            {
                entity.HasKey(e => e.IdMision)
                    .HasName("PK_Mision");

                entity.Property(e => e.Descripcion).IsRequired();
            });

            modelBuilder.Entity<MisionIndiceOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdMisionIndiceOcupacional)
                    .HasName("PK_MisionIndiceOcupacional");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_MisionIndiceOcupacional_idIndiceOcupacional");

                entity.HasIndex(e => e.IdMision)
                    .HasName("IX_MisionIndiceOcupacional_IdMision");

                entity.Property(e => e.IdIndiceOcupacional).HasColumnName("idIndiceOcupacional");

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.MisionIndiceOcupacional)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Mision)
                    .WithMany(p => p.MisionIndiceOcupacional)
                    .HasForeignKey(d => d.IdMision)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModalidadPartida>(entity =>
            {
                entity.HasKey(e => e.IdModalidadPartida)
                    .HasName("PK_ModalidadPartida");

                entity.HasIndex(e => e.IdRelacionLaboral)
                    .HasName("IX_ModalidadPartida_IdRelacionLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.RelacionLaboral)
                    .WithMany(p => p.ModalidadPartida)
                    .HasForeignKey(d => d.IdRelacionLaboral)
                    .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<ModosScializacion>(entity =>
            {
                entity.HasKey(e => e.IdModosScializacion)
                    .HasName("PK_ModosScializacion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MotivoRecepcion>(entity =>
            {
                entity.HasKey(e => e.IdMotivoRecepcion)
                    .HasName("PK_MotivoRecepcion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidad)
                    .HasName("PK_Nacionalidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<NacionalidadIndigena>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidadIndigena)
                    .HasName("PK_NacionalidadIndigena");

                entity.HasIndex(e => e.IdEtnia)
                    .HasName("IX_NacionalidadIndigena_IdEtnia");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Etnia)
                    .WithMany(p => p.NacionalidadIndigena)
                    .HasForeignKey(d => d.IdEtnia)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Nivel>(entity =>
            {
                entity.HasKey(e => e.IdNivel)
                    .HasName("PK_Nivel");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<NivelConocimiento>(entity =>
            {
                entity.HasKey(e => e.IdNivelConocimiento)
                    .HasName("PK_NivelConocimiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<NivelDesarrollo>(entity =>
            {
                entity.HasKey(e => e.IdNivelDesarrollo)
                    .HasName("PK_NivelDesarrollo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais)
                    .HasName("PK_Pais");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaquetesInformaticos>(entity =>
            {
                entity.HasKey(e => e.IdPaquetesInformaticos)
                    .HasName("PK_PaquetesInformaticos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Parentesco>(entity =>
            {
                entity.HasKey(e => e.IdParentesco)
                    .HasName("PK_Parentesco");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
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

            modelBuilder.Entity<PartidasFase>(entity =>
            {
                entity.HasKey(e => e.IdPartidasFase)
                    .HasName("PK_PartidasFase");

                entity.HasIndex(e => e.IdFaseConcurso)
                    .HasName("IX_PartidasFase_IdFaseConcurso");

                entity.HasIndex(e => e.IdIndiceOcupacionalModalidadPartida)
                    .HasName("IX_PartidasFase_IdIndiceOcupacionalModalidadPartida");

                entity.HasOne(d => d.FaseConcurso)
                    .WithMany(p => p.PartidasFase)
                    .HasForeignKey(d => d.IdFaseConcurso)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IndiceOcupacionalModalidadPartida)
                    .WithMany(p => p.PartidasFase)
                    .HasForeignKey(d => d.IdIndiceOcupacionalModalidadPartida)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PK_Permiso");

                entity.HasIndex(e => e.IdTipoPermiso)
                    .HasName("IX_Permiso_IdTipoPermiso");

                entity.HasOne(d => d.TipoPermiso)
                    .WithMany(p => p.Permiso)
                    .HasForeignKey(d => d.IdTipoPermiso)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK_Persona");

                entity.HasIndex(e => e.IdCanditato)
                    .HasName("IX_Persona_IdCanditato");

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

                entity.HasOne(d => d.Canditato)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdCanditato);

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

            modelBuilder.Entity<PersonaCapacitacion>(entity =>
            {
                entity.HasKey(e => e.IdPersonaCapacitacion)
                    .HasName("PK_PersonaCapacitacion");

                entity.HasIndex(e => e.IdCapacitacion)
                    .HasName("IX_PersonaCapacitacion_IdCapacitacion");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_PersonaCapacitacion_IdPersona");

                entity.Property(e => e.Observaciones)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Capacitacion)
                    .WithMany(p => p.PersonaCapacitacion)
                    .HasForeignKey(d => d.IdCapacitacion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.PersonaCapacitacion)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PersonaDiscapacidad>(entity =>
            {
                entity.HasKey(e => e.IdPersonaDiscapacidad)
                    .HasName("PK_PersonaDiscapacidad");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_PersonaDiscapacidad_IdPersona");

                entity.HasIndex(e => e.IdTipoDiscapacidad)
                    .HasName("IX_PersonaDiscapacidad_IdTipoDiscapacidad");

                entity.Property(e => e.NumeroCarnet)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.PersonaDiscapacidad)
                    .HasForeignKey(d => d.IdPersona);

                entity.HasOne(d => d.TipoDiscapacidad)
                    .WithMany(p => p.PersonaDiscapacidad)
                    .HasForeignKey(d => d.IdTipoDiscapacidad);
            });

            modelBuilder.Entity<PersonaEnfermedad>(entity =>
            {
                entity.HasKey(e => e.IdPersonaEnfermedad)
                    .HasName("PK_PersonaEnfermedad");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_PersonaEnfermedad_IdPersona");

                entity.HasIndex(e => e.IdTipoEnfermedad)
                    .HasName("IX_PersonaEnfermedad_IdTipoEnfermedad");

                entity.Property(e => e.InstitucionEmite)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.PersonaEnfermedad)
                    .HasForeignKey(d => d.IdPersona);

                entity.HasOne(d => d.TipoEnfermedad)
                    .WithMany(p => p.PersonaEnfermedad)
                    .HasForeignKey(d => d.IdTipoEnfermedad);
            });

            modelBuilder.Entity<PersonaEstudio>(entity =>
            {
                entity.HasKey(e => e.IdPersonaEstudio)
                    .HasName("PK_PersonaEstudio");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_PersonaEstudio_IdPersona");

                entity.HasIndex(e => e.IdTitulo)
                    .HasName("IX_PersonaEstudio_IdTitulo");

                entity.Property(e => e.Observaciones)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.PersonaEstudio)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Titulo)
                    .WithMany(p => p.PersonaEstudio)
                    .HasForeignKey(d => d.IdTitulo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PersonaPaquetesInformaticos>(entity =>
            {
                entity.HasKey(e => e.IdPersonaPaquetesInformaticos)
                    .HasName("PK_PersonaPaquetesInformaticos");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_PersonaPaquetesInformaticos_IdEmpleado");

                entity.HasIndex(e => e.IdPaquetesInformaticos)
                    .HasName("IX_PersonaPaquetesInformaticos_IdPaquetesInformaticos");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.PersonaPaquetesInformaticos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.PaquetesInformaticos)
                    .WithMany(p => p.PersonaPaquetesInformaticos)
                    .HasForeignKey(d => d.IdPaquetesInformaticos)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PlanGestionCambio>(entity =>
            {
                entity.HasKey(e => e.IdPlanGestionCambio)
                    .HasName("PK_PlanGestionCambio");

                entity.HasIndex(e => e.RealizadoPor)
                    .HasName("IX_PlanGestionCambio_EmpleadoIdRealizadoPor");

                entity.HasIndex(e => e.AprobadoPor)
                   .HasName("IX_PlanGestionCambio_EmpleadoIdAprobadoPor");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.EmpleadoAprobadoPor)
                    .WithMany(p => p.PlanGestionCambio)
                    .HasForeignKey(d => d.AprobadoPor);

                entity.HasOne(d => d.EmpleadoRealizadoPor)
                   .WithMany(p => p.PlanGestionCambio1)
                   .HasForeignKey(d => d.RealizadoPor);
            });

            modelBuilder.Entity<PlanificacionHE>(entity =>
            {
                entity.HasKey(e => e.IdPlanificacionHE)
                    .HasName("PK_PlanificacionHE");

                entity.ToTable("PlanificacionHE");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_PlanificacionHE_IdEmpleado");

                entity.Property(e => e.IdPlanificacionHE).HasColumnName("IdPlanificacionHE");

                entity.Property(e => e.NoHoras).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.PlanificacionHE)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasKey(e => e.IdPregunta)
                    .HasName("PK_Pregunta");

                entity.HasIndex(e => e.IdEvaluacionInduccion)
                    .HasName("IX_Pregunta_IdEvaluacionInduccion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);
                                     
                entity.HasOne(d => d.EvaluacionInducion)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.IdEvaluacionInduccion)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PreguntaRespuesta>(entity =>
            {
                entity.HasKey(e => e.IdPreguntaRespuesta)
                    .HasName("PK_PreguntaRespuesta");

                entity.HasIndex(e => e.IdPregunta)
                    .HasName("IX_PreguntaRespuesta_IdPregunta");

                entity.HasIndex(e => e.IdRespuesta)
                    .HasName("IX_PreguntaRespuesta_IdRespuesta");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.PreguntaRespuesta)
                    .HasForeignKey(d => d.IdPregunta)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Respuesta)
                    .WithMany(p => p.PreguntaRespuesta)
                    .HasForeignKey(d => d.IdRespuesta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Proceso>(entity =>
            {
                entity.HasKey(e => e.IdProceso)
                    .HasName("PK_Proceso");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProcesoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdProcesoDetalle)
                    .HasName("PK_ProcesoDetalle");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_ProcesoDetalle_IdDependencia");

                entity.HasIndex(e => e.IdProceso)
                    .HasName("IX_ProcesoDetalle_IdProceso");

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.ProcesoDetalle)
                    .HasForeignKey(d => d.IdDependencia);

                entity.HasOne(d => d.Proceso)
                    .WithMany(p => p.ProcesoDetalle)
                    .HasForeignKey(d => d.IdProceso)
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

            modelBuilder.Entity<Provisiones>(entity =>
            {
                entity.HasKey(e => e.IdProvisiones)
                    .HasName("PK_Provisiones");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_Provisiones_IdEmpleado");

                entity.HasIndex(e => e.IdTipoProvision)
                    .HasName("IX_Provisiones_IdTipoProvision");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Provisiones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoProvision)
                    .WithMany(p => p.Provisiones)
                    .HasForeignKey(d => d.IdTipoProvision)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RealizaExamenInduccion>(entity =>
            {
                entity.HasKey(e => e.IdRealizaExamenInduccion)
                    .HasName("PK_RealizaExamenInduccion");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RealizaExamenInduccion_IdEmpleado");

                entity.HasIndex(e => e.IdEvaluacionInduccion)
                    .HasName("IX_RealizaExamenInduccion_IdEvaluacionInduccion");

                entity.Property(e => e.Calificacion).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RealizaExamenInduccion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.EvaluacionInducion)
                    .WithMany(p => p.RealizaExamenInduccion)
                    .HasForeignKey(d => d.IdEvaluacionInduccion)
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

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RecepcionActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

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

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_RecepcionActivoFijoDetalle_IdEstado");

                entity.HasIndex(e => e.IdRecepcionActivoFijo)
                    .HasName("IX_RecepcionActivoFijoDetalle_IdRecepcionActivoFijo");

                entity.Property(e => e.NumeroPoliza)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivoFijo)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdActivoFijo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.RecepcionActivoFijoDetalle)
                    .HasForeignKey(d => d.IdEstado)
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

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RecepcionArticulos)
                    .HasForeignKey(d => d.IdEmpleado)
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

            modelBuilder.Entity<RegimenLaboral>(entity =>
            {
                entity.HasKey(e => e.IdRegimenLaboral)
                    .HasName("PK_RegimenLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RegistroEntradaSalida>(entity =>
            {
                entity.HasKey(e => e.IdRegistroEntradaSalida)
                    .HasName("PK_RegistroEntradaSalida");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RegistroEntradaSalida_IdEmpleado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RegistroEntradaSalida)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RelacionLaboral>(entity =>
            {
                entity.HasKey(e => e.IdRelacionLaboral)
                    .HasName("PK_RelacionLaboral");

                entity.HasIndex(e => e.IdRegimenLaboral)
                    .HasName("IX_RelacionLaboral_IdRegimenLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.RegimenLaboral)
                    .WithMany(p => p.RelacionLaboral)
                    .HasForeignKey(d => d.IdRegimenLaboral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RelacionesInternasExternas>(entity =>
            {
                entity.HasKey(e => e.IdRelacionesInternasExternas)
                    .HasName("PK_RelacionesInternasExternas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RelacionesInternasExternasIndiceOcupacional>(entity =>
            {
                entity.HasKey(e => e.IdRelacionesInternasExternasIndiceOcupacional)
                    .HasName("PK_RelacionesInternasExternasIndiceOcupacional");

                entity.HasIndex(e => e.IdIndiceOcupacional)
                    .HasName("IX_RelacionesInternasExternasIndiceOcupacional_IdIndiceOcupacional");

                entity.HasIndex(e => e.RelacionesInternasExternasId)
                    .HasName("IX_RelacionesInternasExternasIndiceOcupacional_RelacionesInternasExternasId");

                entity.HasOne(d => d.IndiceOcupacional)
                    .WithMany(p => p.RelacionesInternasExternasIndiceOcupacional)
                    .HasForeignKey(d => d.IdIndiceOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RelacionesInternasExternas)
                    .WithMany(p => p.RelacionesInternasExternasIndiceOcupacional)
                    .HasForeignKey(d => d.RelacionesInternasExternasId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Relevancia>(entity =>
            {
                entity.HasKey(e => e.IdRelevancia)
                    .HasName("PK_Relevancia");

                entity.Property(e => e.ComportamientoObservable)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RequisitosNoCumple>(entity =>
            {
                entity.HasKey(e => e.IdRequisitosNoCumple)
                    .HasName("PK_RequisitosNoCumple");

                entity.HasIndex(e => e.AdministracionTalentoHumanoId)
                    .HasName("IX_RequisitosNoCumple_AdministracionTalentoHumanoId");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.AdministracionTalentoHumano)
                    .WithMany(p => p.RequisitosNoCumple)
                    .HasForeignKey(d => d.AdministracionTalentoHumanoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta)
                    .HasName("PK_Respuesta");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<RMU>(entity =>
            {
                entity.HasKey(e => e.IdRMU)
                    .HasName("PK_RMU");

                entity.ToTable("RMU");

                entity.HasIndex(e => e.IdEmpeladoIE)
                    .HasName("IX_RMU_IdEmpeladoIE");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RMU_IdEmpleado");

                entity.HasIndex(e => e.IdTipoRMU)
                    .HasName("IX_RMU_IdTipoRMU");

                entity.Property(e => e.IdRMU).HasColumnName("IdRMU");

                entity.Property(e => e.IdEmpeladoIE).HasColumnName("IdEmpeladoIE");

                entity.Property(e => e.IdTipoRMU).HasColumnName("IdTipoRMU");

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.EmpleadoIE)
                    .WithMany(p => p.RMU)
                    .HasForeignKey(d => d.IdEmpeladoIE)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RMU)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoRMU)
                    .WithMany(p => p.RMU)
                    .HasForeignKey(d => d.IdTipoRMU)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RolPagoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdRolPagoDetalle)
                    .HasName("PK_RolPagoDetalle");

                entity.HasIndex(e => e.IdRolPagos)
                    .HasName("IX_RolPagoDetalle_IdRolPagos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Valor).HasColumnType("decimal");

                entity.HasOne(d => d.RolPagos)
                    .WithMany(p => p.RolPagoDetalle)
                    .HasForeignKey(d => d.IdRolPagos)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RolPagos>(entity =>
            {
                entity.HasKey(e => e.IdRolPagos)
                    .HasName("PK_RolPagos");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_RolPagos_IdEmpleado");

                entity.Property(e => e.SaldoFinal).HasColumnType("decimal");

                entity.Property(e => e.SaldoInicial).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.RolPagos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RolPuesto>(entity =>
            {
                entity.HasKey(e => e.IdRolPuesto)
                    .HasName("PK_RolPuesto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Rubro>(entity =>
            {
                entity.HasKey(e => e.IdRubro)
                    .HasName("PK_Rubro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TasaPorcentualMaxima).HasColumnType("decimal");
            });

            modelBuilder.Entity<RubroLiquidacion>(entity =>
            {
                entity.HasKey(e => e.IdRubroLiquidacion)
                    .HasName("PK_RubroLiquidacion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.IdSexo)
                    .HasName("PK_Sexo");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SituacionPropuesta>(entity =>
            {
                entity.HasKey(e => e.IdSituacionPropuesta)
                    .HasName("PK_SituacionPropuesta");

                entity.HasIndex(e => e.IdDependencia)
                    .HasName("IX_SituacionPropuesta_IdDependencia");

                entity.Property(e => e.Observaciones)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.HasOne(d => d.Dependencia)
                    .WithMany(p => p.SituacionPropuesta)
                    .HasForeignKey(d => d.IdDependencia);
            });

            modelBuilder.Entity<SolicitudAcumulacionDecimos>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudAcumulacionDecimos)
                    .HasName("PK_SolicitudAcumulacionDecimos");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudAcumulacionDecimos_IdEmpleado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudAcumulacionDecimos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SolicitudAnticipo>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudAnticipo)
                    .HasName("PK_SolicitudAnticipo");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudAnticipo_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudAnticipo_IdEstado");

                entity.Property(e => e.CantidadCancelada).HasColumnType("decimal");

                entity.Property(e => e.CantidadSolicitada).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudAnticipo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudAnticipo)
                    .HasForeignKey(d => d.IdEstado);
            });

            modelBuilder.Entity<SolicitudCertificadoPersonal>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudCertificadoPersonal)
                    .HasName("PK_SolicitudCertificadoPersonal");

                entity.HasIndex(e => e.IdEmpleadoSolicitante)
                    .HasName("IX_SolicitudCertificadoPersonal_EmpleadoSolicitanteIdEmpleadoSolicitante");

                entity.HasIndex(e => e.IdEmpleadoDirigidoA)
                   .HasName("IX_SolicitudCertificadoPersonal_EmpleadoSolicitanteIdEmpleadoDirigidoA");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudCertificadoPersonal_IdEstado");

                entity.HasIndex(e => e.IdTipoCertificado)
                    .HasName("IX_SolicitudCertificadoPersonal_IdTipoCertificado");

                entity.Property(e => e.Observaciones).HasMaxLength(20);

                entity.HasOne(d => d.EmpleadoSolicitante)
                    .WithMany(p => p.SolicitudCertificadoPersonal)
                    .HasForeignKey(d => d.IdEmpleadoSolicitante);

                entity.HasOne(d => d.EmpleadoDirigidoA)
                   .WithMany(p => p.SolicitudCertificadoPersonal1)
                   .HasForeignKey(d => d.IdEmpleadoDirigidoA);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudCertificadoPersonal)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TipoCertificado)
                    .WithMany(p => p.SolicitudCertificadoPersonal)
                    .HasForeignKey(d => d.IdTipoCertificado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SolicitudHorasExtras>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudHorasExtras)
                    .HasName("PK_SolicitudHorasExtras");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudHorasExtras_IdEmpleado");

                entity.Property(e => e.Observaciones)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudHorasExtras)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SolicitudLiquidacionHaberes>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudLiquidacionHaberes)
                    .HasName("PK_SolicitudLiquidacionHaberes");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudLiquidacionHaberes_IdEmpleado");

                entity.Property(e => e.TotalDecimoCuarto).HasColumnType("decimal");

                entity.Property(e => e.TotalDecimoTercero).HasColumnType("decimal");

                entity.Property(e => e.TotalDesahucio).HasColumnType("decimal");

                entity.Property(e => e.TotalDespidoIntempestivo).HasColumnType("decimal");

                entity.Property(e => e.TotalFondoReserva).HasColumnType("decimal");

                entity.Property(e => e.TotalVacaciones).HasColumnType("decimal");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudLiquidacionHaberes)
                    .HasForeignKey(d => d.IdEmpleado);
            });

            modelBuilder.Entity<SolicitudModificacionFichaEmpleado>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudModificacionFichaEmpleado)
                    .HasName("PK_SolicitudModificacionFichaEmpleado");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudModificacionFichaEmpleado_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudModificacionFichaEmpleado_IdEstado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudModificacionFichaEmpleado)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudModificacionFichaEmpleado)
                    .HasForeignKey(d => d.IdEstado);
            });

            modelBuilder.Entity<SolicitudPermiso>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudPermiso)
                    .HasName("PK_SolicitudPermiso");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudPermiso_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudPermiso_IdEstado");

                entity.HasIndex(e => e.IdPermiso)
                    .HasName("IX_SolicitudPermiso_IdPermiso");

                entity.Property(e => e.Motivo)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasOne(d => d.Permiso)
                    .WithMany(p => p.SolicitudPermiso)
                    .HasForeignKey(d => d.IdPermiso);
            });

            modelBuilder.Entity<SolicitudPlanificacionVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudPlanificacionVacaciones)
                    .HasName("PK_SolicitudPlanificacionVacaciones");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudPlanificacionVacaciones_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudPlanificacionVacaciones_IdEstado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudPlanificacionVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudPlanificacionVacaciones)
                    .HasForeignKey(d => d.IdEstado);
            });

            modelBuilder.Entity<SolicitudProveduria>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduria)
                    .HasName("PK_SolicitudProveduria");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudProveduria_IdEmpleado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudProveduria)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SolicitudProveduriaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudProveduriaDetalle)
                    .HasName("PK_SolicitudProveduriaDetalle");

                entity.HasIndex(e => e.IdArticulo)
                    .HasName("IX_SolicitudProveduriaDetalle_IdArticulo");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudProveduriaDetalle_IdEstado");

                entity.HasIndex(e => e.IdMaestroArticuloSucursal)
                    .HasName("IX_SolicitudProveduriaDetalle_IdMaestroArticuloSucursal");

                entity.HasIndex(e => e.IdSolicitudProveduria)
                    .HasName("IX_SolicitudProveduriaDetalle_IdSolicitudProveduria");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudProveduriaDetalle)
                    .HasForeignKey(d => d.IdEstado)
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

            modelBuilder.Entity<SolicitudVacaciones>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudVacaciones)
                    .HasName("PK_SolicitudVacaciones");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudVacaciones_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudVacaciones_IdEstado");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudVacaciones)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudVacaciones)
                    .HasForeignKey(d => d.IdEstado);
            });

            modelBuilder.Entity<SolicitudViatico>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudViatico)
                    .HasName("PK_SolicitudViatico");

                entity.HasIndex(e => e.IdConfiguracionViatico)
                    .HasName("IX_SolicitudViatico_IdConfiguracionViatico");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_SolicitudViatico_IdEmpleado");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_SolicitudViatico_IdEstado");

                entity.HasIndex(e => e.IdTipoViatico)
                    .HasName("IX_SolicitudViatico_IdTipoViatico");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ValorEstimado).HasColumnType("decimal");

                entity.HasOne(d => d.ConfiguracionViatico)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdConfiguracionViatico)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasOne(d => d.TipoViatico)
                    .WithMany(p => p.SolicitudViatico)
                    .HasForeignKey(d => d.IdTipoViatico)
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

            modelBuilder.Entity<TipoAccionPersonal>(entity =>
            {
                entity.HasKey(e => e.IdTipoAccionPersonal)
                    .HasName("PK_TipoAccionPersonal");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
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

            modelBuilder.Entity<TipoCertificado>(entity =>
            {
                entity.HasKey(e => e.IdTipoCertificado)
                    .HasName("PK_TipoCertificado");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoConcurso>(entity =>
            {
                entity.HasKey(e => e.IdTipoConcurso)
                    .HasName("PK_TipoConcurso");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoDiscapacidad>(entity =>
            {
                entity.HasKey(e => e.IdTipoDiscapacidad)
                    .HasName("PK_TipoDiscapacidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoEnfermedad>(entity =>
            {
                entity.HasKey(e => e.IdTipoEnfermedad)
                    .HasName("PK_TipoEnfermedad");

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

            modelBuilder.Entity<TipoMovimientoInterno>(entity =>
            {
                entity.HasKey(e => e.IdTipoMovimientoInterno)
                    .HasName("PK_TipoMovimientoInterno");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoNombramiento>(entity =>
            {
                entity.HasKey(e => e.IdTipoNombramiento)
                    .HasName("PK_TipoNombramiento");

                entity.HasIndex(e => e.IdRelacionLaboral)
                    .HasName("IX_TipoNombramiento_IdRelacionLaboral");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.RelacionLaboral)
                    .WithMany(p => p.TipoNombramiento)
                    .HasForeignKey(d => d.IdRelacionLaboral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TipoPermiso>(entity =>
            {
                entity.HasKey(e => e.IdTipoPermiso)
                    .HasName("PK_TipoPermiso");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoProvision>(entity =>
            {
                entity.HasKey(e => e.IdTipoProvision)
                    .HasName("PK_TipoProvision");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoRMU>(entity =>
            {
                entity.HasKey(e => e.IdTipoRMU)
                    .HasName("PK_TipoRMU");

                entity.ToTable("TipoRMU");

                entity.Property(e => e.IdTipoRMU).HasColumnName("IdTipoRMU");

                entity.Property(e => e.Descripcion)
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

            modelBuilder.Entity<TipoTransporte>(entity =>
            {
                entity.HasKey(e => e.IdTipoTransporte)
                    .HasName("PK_TipoTransporte");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<TipoViatico>(entity =>
            {
                entity.HasKey(e => e.IdTipoViatico)
                    .HasName("PK_TipoViatico");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Titulo>(entity =>
            {
                entity.HasKey(e => e.IdTitulo)
                    .HasName("PK_Titulo");

                entity.HasIndex(e => e.IdEstudio)
                    .HasName("IX_Titulo_IdEstudio");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Estudio)
                    .WithMany(p => p.Titulo)
                    .HasForeignKey(d => d.IdEstudio)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TrabajoEquipoIniciativaLiderazgo>(entity =>
            {
                entity.HasKey(e => e.IdTrabajoEquipoIniciativaLiderazgo)
                    .HasName("PK_TrabajoEquipoIniciativaLiderazgo");

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

                entity.HasOne(d => d.EmpleadoRecibe)
                    .WithMany(p => p.TranferenciaArticulo)
                    .HasForeignKey(d => d.IdEmpleadoRecibe);

                entity.HasOne(d => d.EmpleadoEnvia)
                    .WithMany(p => p.TranferenciaArticulo1)
                    .HasForeignKey(d => d.IdEmpleadoEnvia);

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

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.TransferenciaActivoFijo)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<TrayectoriaLaboral>(entity =>
            {
                entity.HasKey(e => e.IdTrayectoriaLaboral)
                    .HasName("PK_TrayectoriaLaboral");

                entity.HasIndex(e => e.IdPersona)
                    .HasName("IX_TrayectoriaLaboral_IdPersona");

                entity.Property(e => e.Empresa).HasMaxLength(100);

                entity.Property(e => e.PuestoTrabajo).HasMaxLength(250);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.TrayectoriaLaboral)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.HasKey(e => e.IdUnidadMedida)
                    .HasName("PK_UnidadMedida");
            });

            modelBuilder.Entity<ValidacionInmediatoSuperior>(entity =>
            {
                entity.HasKey(e => e.IdValidacionJefe)
                    .HasName("PK_ValidacionInmediatoSuperior");

                entity.HasIndex(e => e.IdEmpleado)
                    .HasName("IX_ValidacionInmediatoSuperior_IdEmpleado");

                entity.HasIndex(e => e.IdFormularioAnalisisOcupacional)
                    .HasName("IX_ValidacionInmediatoSuperior_IdFormularioAnalisisOcupacional");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.ValidacionInmediatoSuperior)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FormularioAnalisisOcupacional)
                    .WithMany(p => p.ValidacionInmediatoSuperior)
                    .HasForeignKey(d => d.IdFormularioAnalisisOcupacional)
                    .OnDelete(DeleteBehavior.Restrict);
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






