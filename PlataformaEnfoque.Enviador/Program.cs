using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Datos.Entidades;

// Las credenciales NUNCA van en el codigo ni en el repositorio: solo variables de entorno (RD-10).
static string Leer(string nombre) =>
    Environment.GetEnvironmentVariable(nombre)
    ?? throw new InvalidOperationException($"Falta la variable de entorno {nombre}");

var cadenaConexion = Leer("PLATAFORMA_DB");
var host = Leer("SMTP_HOST");
var puerto = int.Parse(Leer("SMTP_PUERTO"));
var usuarioSmtp = Leer("SMTP_USUARIO");
var claveSmtp = Leer("SMTP_CLAVE");
var remitente = Leer("SMTP_REMITENTE");

var opciones = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(cadenaConexion).Options;
using var db = new AppDbContext(opciones);

using var smtp = new SmtpClient(host, puerto)
{
    EnableSsl = true,
    Credentials = new NetworkCredential(usuarioSmtp, claveSmtp)
};

// Idempotente: solo toma los Pendiente. Como cada envio se marca Enviado al instante,
// ejecutar este programa dos veces seguidas no repite ningun correo (RF-NOT-12).
// Importante: no lo ejecutes dos veces AL MISMO TIEMPO.
var pendientes = await db.CorreosEnCola
    .Where(c => c.Estado == EstadoCorreo.Pendiente)
    .OrderBy(c => c.Id)
    .ToListAsync();

Console.WriteLine($"Correos pendientes: {pendientes.Count}");

foreach (var correo in pendientes)
{
    try
    {
        smtp.Send(new MailMessage(remitente, correo.Destinatario, correo.Asunto, correo.Cuerpo));
        correo.Estado = EstadoCorreo.Enviado;
        correo.EnviadoEn = DateTime.UtcNow;
        // Se guarda correo por correo: si el siguiente falla, los ya enviados quedan marcados.
        await db.SaveChangesAsync();
        Console.WriteLine($"Enviado: {correo.Id} a {correo.Destinatario}");
    }
    catch (Exception ex)
    {
        // Sigue pendiente; se reintenta en la proxima ejecucion. (Reintentos formales llegan en la semana 11.)
        Console.WriteLine($"No se pudo enviar el correo {correo.Id}: {ex.Message}");
    }
}