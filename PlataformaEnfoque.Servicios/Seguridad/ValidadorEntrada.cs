using System.Text.RegularExpressions;

namespace PlataformaEnfoque.Servicios.Seguridad;

public static class ValidadorEntrada
{
    // Forma basica: algo@algo.algo, sin espacios.
    private static readonly Regex FormaCorreo = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static void ExigirCorreoValido(string? correo)
    {
        if (string.IsNullOrWhiteSpace(correo) || !FormaCorreo.IsMatch(correo.Trim()))
            throw new ReglaNegocioException("El correo no es valido.");
    }

    // Al menos 8 caracteres, con letras y numeros.
    public static void ExigirContrasenaValida(string? contrasena)
    {
        if (string.IsNullOrEmpty(contrasena) || contrasena.Length < 8
            || !contrasena.Any(char.IsLetter) || !contrasena.Any(char.IsDigit))
            throw new ReglaNegocioException("La contrasena debe tener al menos 8 caracteres, con letras y numeros.");
    }

    public static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
}