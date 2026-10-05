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

    [HttpGet("activar")]
    public async Task<IActionResult> Activar([FromQuery] string? token)
    {
        await registro.ActivarAsync(token);
        return Ok(new { mensaje = "Cuenta activada. Ya puedes iniciar sesion." });
    }
}


