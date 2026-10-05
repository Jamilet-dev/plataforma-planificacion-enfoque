using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Datos.Entidades;
using PlataformaEnfoque.Servicios.Seguridad;

namespace PlataformaEnfoque.Servicios.Cuentas;

public class ServicioRegistro(AppDbContext db)
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

        await db.SaveChangesAsync();
    }
}
