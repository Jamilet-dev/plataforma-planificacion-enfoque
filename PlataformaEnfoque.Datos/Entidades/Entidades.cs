using System.ComponentModel.DataAnnotations;

namespace PlataformaEnfoque.Datos.Entidades;

public enum Rol { Estandar = 0, Administrador = 1 }
public enum TipoToken { Activacion = 0, Recuperacion = 1 }
public enum EstadoCorreo { Pendiente = 0, Enviado = 1 }

public class Usuario
{
    public int Id { get; set; }
    [MaxLength(200)] public string Correo { get; set; } = string.Empty;
    public string HashContrasena { get; set; } = string.Empty;
    public string Sal { get; set; } = string.Empty;
    public Rol Rol { get; set; } = Rol.Estandar;
    // Activo = puede iniciar sesion. Nace en false y se pone true al abrir el enlace.
    public bool Activo { get; set; }
    // Separado de Activo a proposito: si un admin desactiva a alguien, NO debe poder
    // reactivarse solo pidiendo otro enlace de activacion.
    public bool CorreoConfirmado { get; set; }
    public int IntentosFallidos { get; set; }
    public DateTime? BloqueadoHasta { get; set; }
}

public class SesionUsuario
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    [MaxLength(64)] public string Token { get; set; } = string.Empty;
    public DateTime CreadaEn { get; set; } = DateTime.UtcNow;
}

// Sirve para los dos casos: enlace de activacion y codigo de recuperacion.
public class TokenUsuario
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public TipoToken Tipo { get; set; }
    [MaxLength(64)] public string Valor { get; set; } = string.Empty;
    public DateTime Vencimiento { get; set; }
    public bool Usado { get; set; }
}

public class CorreoEnCola
{
    public int Id { get; set; }
    [MaxLength(200)] public string Destinatario { get; set; } = string.Empty;
    [MaxLength(200)] public string Asunto { get; set; } = string.Empty;
    public string Cuerpo { get; set; } = string.Empty;
    public EstadoCorreo Estado { get; set; } = EstadoCorreo.Pendiente;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EnviadoEn { get; set; }
}