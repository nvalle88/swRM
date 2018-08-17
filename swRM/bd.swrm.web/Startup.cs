using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using bd.swrm.datos;
using System;
using bd.swrm.servicios.Interfaces;
using bd.swrm.servicios.Servicios;
using bd.swrm.entidades.Constantes;
using bd.swrm.servicios.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace bd.swrm.web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<SwRMDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SwRMConnection")));
            services.AddSingleton<IUploadFileService, UploadFileService>();
            services.AddSingleton<IClonacion, ClonacionService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddSingleton<IClaimsTransfer, ClaimsTransferService>();
            services.AddSingleton<IPdfMethods, PdfMethodsService>();
            services.AddSingleton<IExcelMethods, ExcelMethodsService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Constantes de correo
            ConstantesCorreo.Smtp = Configuration.GetSection("Smtp").Value;
            ConstantesCorreo.PrimaryPort = Configuration.GetSection("PrimaryPort").Value;
            ConstantesCorreo.SecureSocketOptions = Configuration.GetSection("SecureSocketOptions").Value;
            ConstantesCorreo.CorreoRM = Configuration.GetSection("CorreoRM").Value;
            ConstantesCorreo.PasswordCorreo = Configuration.GetSection("PasswordCorreo").Value;
            ConstantesCorreo.NameFrom = Configuration.GetSection("NameFrom").Value;
            ConstantesCorreo.MensajeCorreoSuperior = Configuration.GetSection("MensajeCorreoSuperior").Value;
            ConstantesCorreo.CorreoEncargadoSeguro = Configuration.GetSection("CorreoEncargadoSeguro").Value;

            //Constantes de función de depreciación
            ConstantesTimerDepreciacion.Hora = int.Parse(Configuration.GetSection("Hora").Value);
            ConstantesTimerDepreciacion.Minutos = int.Parse(Configuration.GetSection("Minutos").Value);
            ConstantesTimerDepreciacion.Segundos = int.Parse(Configuration.GetSection("Segundos").Value);

            Temporizador.Temporizador.InicializarTemporizadorDepreciacion();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<ClaimsMiddleware>();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //serviceScope.ServiceProvider.GetService<LogDbContext>().Database.Migrate();
                    //serviceScope.ServiceProvider.GetService<SwCompartidoDbContext>().EnsureSeedData();
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
