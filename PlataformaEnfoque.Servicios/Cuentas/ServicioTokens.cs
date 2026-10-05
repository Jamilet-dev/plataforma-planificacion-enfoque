using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Datos.Entidades;
using PlataformaEnfoque.Servicios.Seguridad;

namespace PlataformaEnfoque.Servicios.Cuentas;

public class ServicioTokens(AppDbContext db)
{
    public async Task<TokenUsuario> EmitirAsync(Usuario usuario, TipoToken tipo, TimeSpan duracion)
    {
        // Un usuario nuevo todavia no tiene Id (vale 0), asi que no tiene tokens previos que invalidar.
        if (usuario.Id != 0)
        {
            // Al emitir uno nuevo, los anteriores del mismo tipo dejan de servir (RF-CA-17).
            var previos = await db.Tokens
                .Where(t => t.UsuarioId == usuario.Id && t.Tipo == tipo && !t.Usado)
                .ToListAsync();
            foreach (var previo in previos) previo.Usado = true;
        }

        var token = new TokenUsuario
        {
            Usuario = usuario, // se enlaza por objeto; EF pone el UsuarioId al guardar
            Tipo = tipo,
            Valor = GeneradorToken.Nuevo(),
            Vencimiento = DateTime.UtcNow.Add(duracion)
        };
        db.Tokens.Add(token);
        return token;
    }

    // Valida y "quema" el token. Si no existe, ya se uso o vencio: rechaza con el mismo mensaje.
    public async Task<TokenUsuario> ConsumirAsync(string? valor, TipoToken tipo)
    {
        TokenUsuario? token = null;
        if (!string.IsNullOrEmpty(valor))
            token = await db.Tokens.Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Valor == valor && t.Tipo == tipo);

        if (token == null || token.Usado || token.Vencimiento < DateTime.UtcNow)
            throw new ReglaNegocioException("El enlace o codigo no es valido o ya vencio.");

        token.Usado = true; // un solo uso
        return token;
    }
}