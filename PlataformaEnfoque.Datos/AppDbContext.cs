using Microsoft.EntityFrameworkCore;
using PlataformaEnfoque.Datos.Entidades;

namespace PlataformaEnfoque.Datos;

public class AppDbContext(DbContextOptions<AppDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<SesionUsuario> Sesiones => Set<SesionUsuario>();
    public DbSet<TokenUsuario> Tokens => Set<TokenUsuario>();
    public DbSet<CorreoEnCola> CorreosEnCola => Set<CorreoEnCola>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Indice unico: la base de datos tambien impide dos usuarios con el mismo correo (RF-CA-01).
        mb.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
        mb.Entity<SesionUsuario>().HasIndex(s => s.Token).IsUnique();
        mb.Entity<TokenUsuario>().HasIndex(t => t.Valor);
    }
}