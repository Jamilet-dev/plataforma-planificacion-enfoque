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

        public const string MensajeReenvio =
        "Si el correo esta registrado y pendiente de activar, recibiras un nuevo enlace.";

    public async Task<string> ReenviarAsync(string? correo)
    {
        ValidadorEntrada.ExigirCorreoValido(correo);
        var correoNormal = ValidadorEntrada.NormalizarCorreo(correo!);
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Correo == correoNormal);

        // Solo si nunca confirmo su correo. Asi un usuario desactivado por un admin
        // no puede reactivarse solo pidiendo otro enlace.
        if (usuario != null && !usuario.CorreoConfirmado)
        {
            await EmitirEnlaceAsync(usuario); // invalida el enlace anterior
            await db.SaveChangesAsync();
        }

        // La MISMA respuesta exista o no el correo: no revela quien esta registrado.
        return MensajeReenvio;
    }

        public async Task ActivarAsync(string? valorToken)
    {
        // ConsumirAsync ya rechaza si el token se uso o vencio, y en ese caso no se cambia nada.
        var token = await tokens.ConsumirAsync(valorToken, TipoToken.Activacion);
        token.Usuario.Activo = true;
        token.Usuario.CorreoConfirmado = true;
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