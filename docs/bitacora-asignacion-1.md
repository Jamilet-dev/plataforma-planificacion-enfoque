# Bitácora de sesión con el agente — Asignación 1

Programación III · TDS-007 · Jamilet · Agente: Claude (claude.ai)

## Tarea 1: qué faltaba para entregar

**Qué le pedí:** subí el enunciado de la asignación y pegué el texto de los tres PR de mi compañero fusionados en mi repo. Escribí: "YA REALICE LOS PULL REQUEST Y MI COMPAÑERO TAMBIEN, QUIERO HACER LO QUE FALTA PARA ENTREGAR".

**Qué me devolvió:** una lista de pendientes (revisión de los tres PR con comentario en línea y veredicto, verificación del índice y del historial, bitácora, tag y Moodle) y observaciones sobre los diffs: una posible `S` suelta en la plantilla, placeholders en el README, y que el PR #4 no mostraba la limpieza de `bin/` y `obj/` que prometía su descripción.

## Tarea 2: verificar mi repo con comandos

**Qué le pedí:** "EREALMENTE NOSE DE GIT Y PARA ENTREGAR LA TAREA ME QUEDA POCO TIEMPO... GUIAME PASO A PASO".

**Qué me devolvió:** comandos para la terminal de VS Code (`git ls-files`, `git log -G`, `git show`).

**Resultados que obtuve:**
- `git ls-files "*bin/*" "*obj/*"`: sin resultados.
- `git ls-files "*appsettings*"`: sin resultados.
- `git log --oneline --all -- "*bin/*" "*obj/*"`: sin resultados.

## Tarea 3: comentarios y veredictos de revisión

**Qué le pedí:** "ajusta el comentario tipo como los de cesar".

**Qué me devolvió:** un comentario corto sobre la línea concreta más un veredicto con el formato "Veredicto: Approve. ...", que usé en la pestaña Files changed de los tres PR.

## Error del agente (caso principal)

**Qué dijo:** al revisar el PR #4, el agente escribió: "Hay patrones duplicados (`[Bb]in/` y `bin/`, `*.user` dos veces) y encabezados repetidos".

**Por qué estaba mal:** el texto que le pegué venía de copiar la página con las líneas borradas (-) y las agregadas (+) mezcladas, y el agente las leyó como un solo archivo.

**Cómo se detectó:** subí el PDF de la pestaña Files changed del PR #4, donde las líneas - y + aparecen en columnas separadas. Ahí se vio que no había duplicados (`*.user` solo cambió de lugar).

**Cómo se corrigió:** el agente respondió "Me equivoqué en la revisión anterior..." y retiró el comentario. No lo incluí en mi revisión del PR.

## Caso menor: falso positivo

`git log --oneline -G"[Pp]assword|[Ss]ecret|[Aa]pi[Kk]ey"` devolvió `72174fc refactor: se modificó el anterior .gitignore para incluir otros archivos`. El agente indicó que probablemente era un falso positivo por el comentario "secretos". Lo verifiqué con `git show 72174fc | Select-String -Pattern "secret|password|apikey"`, que solo mostró `+## Archivos de entorno / secretos`. No hay credenciales en el historial.