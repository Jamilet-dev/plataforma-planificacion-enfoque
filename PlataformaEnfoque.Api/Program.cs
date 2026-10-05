using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Servicios;

var builder = WebApplication.CreateBuilder(args);

// La cadena de conexion viene de una variable de entorno, nunca del codigo (RD-10).
var cadenaConexion = Environment.GetEnvironmentVariable("PLATAFORMA_DB")
    ?? throw new InvalidOperationException("Falta la variable de entorno PLATAFORMA_DB");

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(cadenaConexion, b => b.MigrationsAssembly("PlataformaEnfoque.Datos")));

builder.Services.AddControllers();

builder.Services.AddScoped<PlataformaEnfoque.Servicios.Cuentas.ServicioRegistro>();


// >>> AQUI SE IRAN AGREGANDO LOS SERVICIOS EN CADA RAMA <<<

var app = builder.Build();

// Crea o actualiza la base de datos al arrancar. Los datos se guardan en SQL Server,
// por eso los usuarios sobreviven a reiniciar la aplicacion (RD-09).
using (var alcance = app.Services.CreateScope())
{
    alcance.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

// Manejo central de errores (RD-08): el usuario nunca ve trazas ni consultas.
app.UseExceptionHandler(errorApp => errorApp.Run(async contexto =>
{
    var error = contexto.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (error is ReglaNegocioException regla)
    {
        contexto.Response.StatusCode = regla.Codigo;
        await contexto.Response.WriteAsJsonAsync(new { mensaje = regla.Message });
    }
    else
    {
        contexto.Response.StatusCode = 500;
        await contexto.Response.WriteAsJsonAsync(new { mensaje = "Ocurrio un error inesperado." });
    }
}));

app.MapControllers();
app.Run();