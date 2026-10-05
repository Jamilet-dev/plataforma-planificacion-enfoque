using System.Security.Cryptography;

namespace PlataformaEnfoque.Servicios.Seguridad;

public static class GeneradorToken
{
    // 32 bytes aleatorios = 64 caracteres hexadecimales. Imposible de adivinar.
    public static string Nuevo() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
}