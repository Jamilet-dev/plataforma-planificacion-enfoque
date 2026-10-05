# plataforma-planificacion-enfoque

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) o superior instalado.
- Verifica tu versión con:

```bash
  dotnet --version
```

## Clonar el repositorio

```bash
git clone <URL-del-repositorio>
cd <nombre-carpeta-repo>
```

## Instalar dependencias

Dentro de la carpeta de la API:

```bash
cd [carpeta-de-la-api]
dotnet restore
```

## Ejecutar el proyecto

```bash
- Desde la carpeta que contiene la solucion general:
dotnet build
- Desde la carpeta que contiene la API:
dotnet run
```

Por defecto la API queda escuchando en `https://localhost:XXXX` (revisa la consola al ejecutar `dotnet run` para el puerto exacto, o el archivo `launchSettings.json`).

## Notas

- Si usas variables de entorno o `appsettings.Development.json`, asegúrate de configurarlas antes de correr el proyecto (ver `.gitignore`, ese archivo no se versiona).

## Variables de Entorno Necesarias
El proyecto requiere las siguientes variables de entorno para el envío de correos (cola mínima):
- `SMTP_HOST`: Dirección del servidor SMTP (ej: ://gmail.com).
- `SMTP_PORT`: Puerto del servidor SMTP (ej: 587).
- `SMTP_USER`: Usuario o correo electrónico de autenticación.
- `SMTP_PASS`: Contraseña de aplicación o credencial del servidor.

## Cómo verificar los Criterios de Aceptación
- **Registro y Activación:** Registra un usuario en el endpoint `/api/auth/register`. El usuario se creará inactivo. Intenta iniciar sesión y será rechazado. Ejecuta el componente `PlataformaEnfoque.Enviador` para procesar la cola por SMTP y recibirás el correo real de activación.
- **Bloqueo de Sesión:** Intenta iniciar sesión 5 veces seguidas con credenciales incorrectas en `/api/auth/login`. Al 6to intento, el sistema bloqueará la cuenta por 15 minutos de forma controlada.
- **Recuperación de Contraseña:** Solicita la recuperación en `/api/auth/recover`. Llegará un código de un solo uso por correo tras procesar la cola.
- **Restricción de Roles:** Crea una petición manual HTTP hacia un endpoint de Administrador (ej: listar usuarios) usando un token de usuario Estándar; el servidor devolverá un rechazo explícito (Forbidden).
- **Máquina de Estados:** La estructura de la máquina de estados se encuentra documentada en la tabla del archivo `docs/maquina-de-estados.md`.
