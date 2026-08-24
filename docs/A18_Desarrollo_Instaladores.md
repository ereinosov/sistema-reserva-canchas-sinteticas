# UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
## Facultad de Ciencias de la Computación
### Carrera de Ingeniería en Software — Proceso de Software “A”

---

# Artefacto A18: Desarrollo de Instaladores

| Campo | Detalle |
|---|---|
| Proyecto | Sistema de Reserva de Canchas Sintéticas |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph Alexander · Herrera Barco Humberto Aldair · Reinoso Vélez Eduardo David · Silva Triviño John Jairo |
| Período | Mayo – Agosto 2026 |
| Fecha | 24 de agosto de 2026 |
| Versión del paquete | 1.0.0 |
| Producto empaquetado | A13 Release (`SistemaCanchas.Presentacion`, net48) |
| Baseline | Código A13 `637234e`; A17 en `2e8256e` |
| Referencias | A11 (SQL, no ejecutado por el Setup), A12 §10 / RNF12 (DPAPI), A16 OBS-01, A17 (Encrypt=False, no rehacer A11) |
| Norma de referencia | ISO/IEC 25010:2023 (instalabilidad); Microsoft, *ClickOnce vs. Windows Installer*; Inno Setup 6 (empaquetado Windows) |

---

## 1. Objetivo

Entregar un **paquete de instalación** que copie la aplicación WinForms, sus dependencias y los scripts SQL de apoyo, sin sustituir a SSMS ni destruir datos. El instalador cierra lo que A17 dejó pendiente: distribución de archivos, accesos directos, desinstalación y cifrado de la cadena de arranque **en la estación destino** (RNF12 / A16 OBS-01).

## 2. Decisión de diseño

| Alternativa | Por qué no es la principal |
|---|---|
| ClickOnce | Piensa en actualizaciones desde URL y en perfiles de usuario; no copia scripts SQL de servidor ni se alinea con un laboratorio LocalDB administrado en SSMS. |
| WiX / MSI | Correcto en empresa; exige el SDK de WiX, ausente en esta estación, y un autoría XML desproporcionada para un único exe de escritorio. |
| Solo copiar `bin\Debug` | No es instalador: mezcla PDB, no crea desinstalación ni comprueba .NET 4.8. |

**Elegido:** empaquetado **Inno Setup 6** (asistente gráfico, `Setup.exe`) más un **instalador PowerShell equivalente** que no depende de Inno. Ambos consumen el mismo payload Release. Esta estación **no tiene ISCC.exe**; el payload y el script `.iss` quedan listos para generar el Setup en cualquier PC con Inno 6. El PowerShell sí es ejecutable aquí.

El instalador **nunca** ejecuta `A11_Script_DDL.sql` (A17: `DROP TABLE`).

## 3. Contenido del paquete (evidencia 24 ago 2026)

Comando:

```
dotnet publish src/SistemaCanchas.Presentacion/SistemaCanchas.Presentacion.csproj -c Release -f net48 -o setup/payload
```

Archivos de ejecución (sin PDB ni XML de documentación):

| Archivo | Rol |
|---|---|
| `SistemaCanchas.Presentacion.exe` | Entrada WinForms |
| `SistemaCanchas.Presentacion.exe.config` | Cadena bootstrap, `Encrypt=False` |
| `SistemaCanchas.Negocio.dll` | Reglas RN01–RN13 |
| `SistemaCanchas.Datos.dll` | ADO.NET / SP A11 |
| `SistemaCanchas.Entidades.dll` | POCOs |
| `BCrypt.Net-Next.dll` | Hash de aplicación, costo 12 |
| `System.Buffers.dll`, `System.Memory.dll`, `System.Numerics.Vectors.dll`, `System.Runtime.CompilerServices.Unsafe.dll` | Dependencias transitivas de BCrypt |

Se añaden al paquete (no salen de `publish`):

| Recurso | Destino instalado |
|---|---|
| `docs/A11_Script_DDL.sql` y tres complementos | `{app}\sql\` |
| `setup/LEAME_INSTALACION.txt` | `{app}` |
| `ProtegerCadenaConexion.ps1` | `{app}` (RNF12) |
| `Desinstalar-SistemaCanchas.ps1` | `{app}` |

`setup/payload/` y `setup/output/` están en `.gitignore` (binarios). Se versionan los **scripts** de A18.

## 4. Comportamiento del instalador

| Acción | Sí / No | Motivo |
|---|---|---|
| Comprobar .NET 4.8 (Release ≥ 528040) | Sí | A17 DP-01 |
| Copiar exe, DLL y `.config` a `%ProgramFiles%\SistemaCanchas` | Sí | Destino estándar |
| Conservar `.config` existente en actualización (`onlyifdoesntexist`) | Sí | No pisar cadena ya cifrada o clave cambiada |
| Crear acceso en menú Inicio (y escritorio opcional) | Sí | Arranque del usuario |
| Ejecutar A11 o complementos SQL | **No** | Evita DROP TABLE y requiere sysadmin consciente (A17) |
| Crear `aes.key` | **No** | Lo crea la app al primer arranque (DPAPI de estación) |
| Borrar `aes.key` al desinstalar | **No** | Sin esa clave no se descifra `clave_bd_enc` |
| Borrar `ReservaCanchasDB` | **No** | El motor no es parte del producto de UI |
| Cifrar `connectionStrings` en el payload del repo | **No** | DPAPI no es portable; se cifra **después** de instalar |

## 5. Cifrado de la cadena en destino (cierre A16 OBS-01)

En laboratorio la clave de `login_bootstrap` coincide con A11 (documentada). En una estación de producción:

1. Cambiar la clave del login en SQL Server.
2. Editar `{app}\SistemaCanchas.Presentacion.exe.config` (mismo valor; `Encrypt=False` si es LocalDB).
3. Ejecutar como administrador:

```
powershell -NoProfile -ExecutionPolicy Bypass -File "C:\Program Files\SistemaCanchas\ProtegerCadenaConexion.ps1"
```

El script llama a `aspnet_regiis -pef connectionStrings` con `DataProtectionConfigurationProvider`. `ConfigurationManager` de .NET 4.8 descifra al leer; **A13 no se modifica**. Si se copia el `.config` ya cifrado a otro PC, fallará: hay que volver a proteger en esa máquina.

## 6. Cómo generar e instalar

Desde la raíz del repositorio (PowerShell):

```
Set-ExecutionPolicy -Scope Process Bypass
.\setup\Compilar-Instalador.ps1
```

Eso publica Release, copia SQL a `setup\payload\sql` y, si existe Inno Setup 6, produce:

`setup\output\SistemaCanchas_Setup_1.0.0.exe`

Sin Inno:

```
.\setup\Instalar-SistemaCanchas.ps1
```

(requiere administrador). Opcional: `-ProtegerCadena` para RNF12 en el mismo paso.

Desinstalación: Panel de control (Inno) o `Desinstalar-SistemaCanchas.ps1`.

Post-instalación SQL: abrir `{app}\sql\` en SSMS y seguir A17 (A11 una vez **o** solo complementos).

## 7. Estructura versionada

```
/setup
  SistemaCanchas.iss              Script Inno Setup 6
  Compilar-Instalador.ps1         publish + ISCC
  Instalar-SistemaCanchas.ps1     instalador sin Inno
  Desinstalar-SistemaCanchas.ps1
  ProtegerCadenaConexion.ps1      DPAPI en destino
  LEAME_INSTALACION.txt           texto del asistente / carpeta {app}
```

`AppId` Inno (estable, permite actualizar): `{8F3A91C2-6B47-4E1D-9C58-7A2D0E4B1F63}`.

## 8. Lista de comprobación del paquete

| ID | Criterio | Resultado 24 ago 2026 |
|---|---|---|
| INS-01 | `dotnet publish` Release net48 termina 0 | OK; payload en `setup/payload` |
| INS-02 | El `.config` publicado lleva `Encrypt=False` y `(localdb)\MSSQLLocalDB` | OK |
| INS-03 | BCrypt y DLL de capas presentes | OK (lista §3) |
| INS-04 | El `.iss` no ejecuta SQL ni borra `aes.key` | OK (comentario y ausencia de `[UninstallDelete]` sobre ProgramData) |
| INS-05 | PowerShell de instalación exige administrador y .NET 4.8 | OK (`#Requires -RunAsAdministrator`, Release ≥ 528040) |
| INS-06 | Actualización no pisa `.config` existente | OK (`onlyifdoesntexist` en Inno) |
| INS-07 | ISCC no está en esta estación | Registrado: Setup.exe se genera cuando haya Inno 6; el payload y el `.iss` son el producto compilable |
| INS-08 | OBS-01: procedimiento de cambio de clave + DPAPI | OK (§5), no se cifra el repo |

## 9. Veredicto

**APROBADO.** Existe un instalador de producto (Inno + PowerShell), un payload Release reproducible y reglas explícitas alineadas con A17. El `Setup.exe` gráfico es el binario de Inno; su ausencia aquí es de **herramienta de compilación**, no de diseño: el script `SistemaCanchas.iss` es el artefacto compilable.

Queda autorizado el paso a **A19 Manual de Usuario** (operación del sistema ya instalado, sin jerga RF/RN en la guía del empleado).

## 10. Cómo repetir este artefacto

1. `.\setup\Compilar-Instalador.ps1`
2. Comprobar que `setup\payload` contiene el exe, el `.config` y las DLL de la §3.
3. Con Inno 6: comprobar `setup\output\SistemaCanchas_Setup_1.0.0.exe` e instalarlo en un PC de prueba **sin** ejecutar A11 si la BD ya existe.
4. Confirmar que desinstalar deja `%ProgramData%\SistemaCanchas\aes.key`.

## 11. Control de cambios del artefacto

| Versión | Fecha | Descripción |
|---|---|---|
| 1.0 | 24 ago 2026 | Payload Release, Inno Setup 6, instalador PowerShell y cifrado DPAPI en destino |

---

*Fin del artefacto A18. El siguiente artefacto del ciclo es A19 (Manual de Usuario).*
