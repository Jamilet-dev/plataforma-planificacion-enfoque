namespace PlataformaEnfoque.Servicios;

// Error "esperado" (ej: correo repetido). La Api lo convierte en una respuesta limpia, sin trazas (RD-08).
public class ReglaNegocioException(string mensaje, int codigoHttp = 400) : Exception(mensaje)
{
    public int Codigo { get; } = codigoHttp;
}