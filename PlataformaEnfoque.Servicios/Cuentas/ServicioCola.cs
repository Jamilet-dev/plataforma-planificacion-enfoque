using PlataformaEnfoque.Datos;
using PlataformaEnfoque.Datos.Entidades;

namespace PlataformaEnfoque.Servicios.Cuentas;

public class ServicioCola(AppDbContext db)
{
    // Solo AGREGA la fila. No llama a SaveChanges a proposito: asi el correo y la operacion
    // que lo origino se guardan juntos, o no se guarda ninguno. Nunca se envia correo aqui:
    // eso lo hace el Enviador aparte (RF-NOT-08).
    public void Encolar(string destinatario, string asunto, string cuerpo)
    {
        db.CorreosEnCola.Add(new CorreoEnCola
        {
            Destinatario = destinatario,
            Asunto = asunto,
            Cuerpo = cuerpo
        });
    }
}