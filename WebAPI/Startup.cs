using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAoniken.Data;
using TestAoniken.Servicios;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public IConfiguration Configuration { get; }

    // Constructor que recibe la configuración del archivo appsettings.json
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Configuración de los servicios necesarios para la aplicación
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        // Configurar DbContext con SQL Server utilizando Entity Framework Core
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IPublicacionService, PublicacionService>();
        services.AddScoped<IEmailService, EmailService>(); // Mail Service +++++

        // Configurar servicios personalizados
        services.AddScoped<IPublicacionService, PublicacionService>();

        // Configurar Swagger para la documentación de la API
        services.AddSwaggerGen();
    }

    // Configuración del pipeline de la aplicación
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configuración para el entorno de desarrollo
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // Configuración para el entorno de producción
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Forzar redirección HTTPS
        app.UseHttpsRedirection();

        // Habilitar archivos estáticos
        app.UseStaticFiles();

        // Configurar enrutamiento
        app.UseRouting();

        // Habilitar autorización
        app.UseAuthorization();

        // Configurar los endpoints para los controladores
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
