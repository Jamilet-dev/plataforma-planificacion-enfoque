using Microsoft.AspNetCore.Mvc;
using PlataformaEnfoque.Servicios.Cuentas;

namespace PlataformaEnfoque.Api.Controllers;

[ApiController]
[Route("api/cuentas")]
public class CuentasController(ServicioRegistro registro) : ControllerBase
{
    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        await registro.RegistrarAsync(dto.Correo, dto.Contrasena);
        return Ok(new { mensaje = "Cuenta creada. Revisa tu correo para activarla." });
    }
}
