# Bitacora de uso del agente - Practica 1

Herramienta: Claude (Anthropic), en el chat de claude.ai.
Fecha: 4 de octubre de 2026.

## Entrada 1: planificacion de la practica
- Que le pedi: el enunciado de la Practica 1 y el contexto de mi proyecto, para que me guiara paso a paso en el orden de ramas y commits.
- Que respondio: un plan de 7 ramas (base, registro, correo, sesion, administracion, contrasenas, maquina de estados, README) con un commit por cambio, y el codigo de cada uno.
- Que revise o decidi yo: lei el plan contra la rubrica y comprobe que cubria los cuatro pull requests minimos (registro, sesion, recuperacion, administracion).
- Donde se equivoco: asumio que yo usaba Visual Studio y me dio pasos con menus y comandos de PowerShell. Yo trabajo con VS Code y Git Bash, asi que tuve que pedirle que lo rehiciera.

## Entrada 2: arquitectura
- Que le pedi: si el proyecto iba a usar arquitectura en capas.
- Que respondio: una division en Datos, Servicios y Api, mas un programa de consola aparte para enviar correos.
- Que decidi yo: acepte esa estructura porque coincide con las responsabilidades de la semana 2 (cada pieza una sola responsabilidad).

## Entrada 3: cambio de entorno
- Que le pedi: reorganizar la guia porque no podia instalar Visual Studio por falta de espacio.
- Que respondio: pasos solo con el SDK de .NET, VS Code y Git Bash.
- Donde se equivoco: me dio comandos de PowerShell (por ejemplo variables con $env:) que no funcionan en Git Bash. Le avise y los corrigio a la sintaxis de Git Bash.

## Entrada 4: mensajes de commit
- Que le pedi: que los mensajes de commit sonaran naturales, fueran atomicos y estuvieran en orden.
- Que respondio: mensajes cortos en imperativo con el requisito en el cuerpo, un commit por cambio.
- Que revise yo: comprobe que cada asunto tuviera menos de 50 caracteres y que cada commit compilara antes de confirmarlo.

## Entrada 5: entrega de las instrucciones
- Que le pedi: que me diera el codigo directamente en el chat.
- Donde se equivoco: me las habia dejado en archivos externos que no pude usar bien. Pedi que las pusiera en el chat.

## Errores que me salieron al ejecutar (completar con los mios)
- Error: Al ejecutar las pruebas rapidas de registro con el comando `curl`, me arrojo el error `Invoke-WebRequest : No se puede enlazar el parametro 'Headers'`. Esto paso porque el comando `curl` en la terminal de PowerShell de Windows actua como un alias de un comando nativo que no procesa los parametros `-H` ni `-d` en formato de texto plano.
- Como lo resolvi: Abri una ventana independiente de Git Bash (donde el ejecutable `curl` de Linux si funciona de forma nativa) y volvi a lanzar las peticiones hacia `http://localhost:5000/api/cuentas/registro`.

- Error: Intentar usar el mismo nombre de la primera rama que ya habia subido a GitHub al momento de crear el segundo Pull Request.
- Como lo resolvi: El agente me advirtio que repetir el titulo exacto causaria conflictos en el historial de entregas en la nube, por lo que decidimos cambiar el titulo a uno mucho mas preciso enfocado en la infraestructura de persistencia.

## Lo que aprendi / verifique por mi cuenta
- Probe el registro con curl en Git Bash y verifique la precision de las respuestas del servidor: la primera peticion ingreso con un codigo de estado exitoso `200 OK`, el segundo intento con el mismo correo fue rechazado con un `409 Conflict` (verificando el requisito RF-CA-01) y una contrasena de 5 caracteres devolvio de manera controlada un `400 Bad Request` (verificando el requisito RF-CA-14).
- Comprendi la diferencia tecnica entre PowerShell y Git Bash al momento de interactuar con endpoints mediante peticiones HTTP en entornos de desarrollo Windows.
- Aprendi el concepto criptografico de la "Sal" (Salt): entendi que es un bloque de datos aleatorio y unico generado por cada usuario que se mezcla con su clave antes de calcular el Hash. Gracias a esto, verifique que si dos personas eligen la misma contrasena, sus valores finales guardados en la base de datos de SQL Server seguiran siendo completamente diferentes, cumpliendo con la regla de de diseno RD-05.
- Confirme la importancia de hacer commits atomicos y de empaquetar de forma conjunta las modificaciones de multiples capas (Servicios, DTOs, Controladores y Program) para asegurar que el repositorio nunca quede en un estado roto o incompilable.
