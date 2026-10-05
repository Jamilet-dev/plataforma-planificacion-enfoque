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

// >>> para siguiente commit <<<