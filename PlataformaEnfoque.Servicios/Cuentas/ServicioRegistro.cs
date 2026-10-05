using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Datos.Entidades;
using PlataformaEnfoque.Servicios.Seguridad;

namespace PlataformaEnfoque.Servicios.Cuentas;

public class ServicioRegistro(AppDbContext db, ServicioTokens tokens, ServicioCola cola)
{
    public async Task RegistrarAsync(string? correo, string? contrasena)
    {
        ValidadorEntrada.ExigirCorreoValido(correo);
        ValidadorEntrada.ExigirContrasenaValida(contrasena);
        var correoNormal = ValidadorEntrada.NormalizarCorreo(correo!);

        if (await db.Usuarios.AnyAsync(u => u.Correo == correoNormal))
            throw new ReglaNegocioException("Ese correo ya esta registrado.", 409);

        var (hash, sal) = HashContrasena.Hashear(contrasena!);
        var usuario = new Usuario
        {
            Correo = correoNormal,
            HashContrasena = hash,
            Sal = sal,
            Activo = false // nace inactivo (RF-CA-15)
        };
        db.Usuarios.Add(usuario);

        await EmitirEnlaceAsync(usuario);

        // Usuario, token y correo en cola se guardan juntos en una sola operacion.
        await db.SaveChangesAsync();
    }

    private async Task EmitirEnlaceAsync(Usuario usuario)
    {
        var token = await tokens.EmitirAsync(usuario, TipoToken.Activacion, TimeSpan.FromHours(24));
        var urlBase = Environment.GetEnvironmentVariable("PLATAFORMA_URL_BASE") ?? "http://localhost:5000";
        cola.Encolar(
            usuario.Correo,
            "Activa tu cuenta",
            $"Abre este enlace para activar tu cuenta (vence en 24 horas):\n{urlBase}/api/cuentas/activar?token={token.Valor}");
    }
}