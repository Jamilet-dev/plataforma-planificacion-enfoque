using System.Security.Cryptography;

namespace PlataformaEnfoque.Servicios.Seguridad;

public static class HashContrasena
{
    private const int Iteraciones = 100_000;
    private const int TamanoHash = 32;

    public static (string Hash, string Sal) Hashear(string contrasena)
    {
        // La sal es un valor aleatorio DISTINTO por usuario. Por eso dos usuarios con la
        // misma contrasena terminan con valores guardados diferentes (RD-05).
        byte[] sal = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(sal));
    }

    public static bool Verificar(string contrasena, string hashGuardado, string salGuardada)
    {
        byte[] sal = Convert.FromBase64String(salGuardada);
        byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);
        // FixedTimeEquals compara en tiempo constante para que nadie pueda adivinar el hash midiendo tiempos.
        return CryptographicOperations.FixedTimeEquals(calculado, Convert.FromBase64String(hashGuardado));
    }
}