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
- En la carpeta de solucion general:
dotnet build
- En la carpeta de la API:
dotnet run
```

Por defecto la API queda escuchando en `https://localhost:XXXX` (revisa la consola al ejecutar `dotnet run` para el puerto exacto, o el archivo `launchSettings.json`).

## Notas

- Si usas variables de entorno o `appsettings.Development.json`, asegúrate de configurarlas antes de correr el proyecto (ver `.gitignore`, ese archivo no se versiona).
