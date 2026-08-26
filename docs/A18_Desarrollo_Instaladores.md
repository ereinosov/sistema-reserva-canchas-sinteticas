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
| Fecha | 25 de agosto de 2026 |
| Versión del paquete | 1.0.0 |
| Referencias | A11, A12, A16, A17 |

---

## 1. Qué entrega este artefacto

Un paquete que copia el programa de escritorio, las DLL y el script SQL de apoyo. **No** crea la base ni borra datos. Quien instala sigue A17: A11 una vez, en SSMS, si la instancia está vacía.

## 2. Por qué Inno Setup + PowerShell

ClickOnce apunta a actualizaciones por URL y no encaja con un laboratorio LocalDB. WiX pide un SDK que no está en esta estación. Copiar `bin\Debug` a mano no es un instalador (mezcla PDB y no desinstala).

Quedó **Inno Setup 6** (Setup gráfico) y un **script PowerShell** por si Inno no está instalado. Los dos usan el mismo payload Release. El instalador **nunca** ejecuta `A11_Script_DDL.sql`.

## 3. Qué va en el paquete

```
dotnet publish src/SistemaCanchas.Presentacion/SistemaCanchas.Presentacion.csproj -c Release -f net48 -o setup/payload
```

| Archivo | Para qué |
|---|---|
| `SistemaCanchas.Presentacion.exe` (+ `.config`) | Programa y cadena bootstrap (`Encrypt=False`) |
| DLL de Negocio, Datos, Entidades | Capas |
| `BCrypt.Net-Next.dll` y dependencias | Hash de la clave de aplicación |
| `docs/A11_Script_DDL.sql` | Único script SQL, copiado a `{app}\sql\` |
| `LEAME_INSTALACION.txt`, `ProtegerCadenaConexion.ps1`, desinstalador | Texto y cifrado DPAPI en destino |

`setup/payload/` y `setup/output/` están en `.gitignore`. Se versionan los scripts de `setup/`.

## 4. Qué hace y qué no hace

| Acción | Sí / No |
|---|---|
| Comprobar .NET 4.8 | Sí |
| Copiar a `%ProgramFiles%\SistemaCanchas` | Sí |
| No pisar un `.config` ya editado al actualizar | Sí |
| Acceso en menú Inicio | Sí |
| Ejecutar A11 | **No** |
| Crear o borrar `aes.key` | **No** (la app la crea; al desinstalar se deja) |
| Borrar `ReservaCanchasDB` | **No** |

## 5. Cifrar la cadena en el PC instalado

En el laboratorio la clave de `login_bootstrap` es la de A11. En otra estación:

1. Cambiar el login en SQL Server.
2. Poner el mismo valor en el `.config` (`Encrypt=False` si es LocalDB).
3. Como administrador: `ProtegerCadenaConexion.ps1` (`aspnet_regiis -pef connectionStrings`).

Eso cifra en **esa** máquina. Copiar el `.config` ya cifrado a otro PC no sirve.

## 6. Cómo generar e instalar

Desde la raíz del repo:

```
.\setup\Compilar-Instalador.ps1
```

Publica Release, copia `A11_Script_DDL.sql` a `setup\payload\sql` y, si hay Inno 6, genera `setup\output\SistemaCanchas_Setup_1.0.0.exe`.

Sin Inno: `.\setup\Instalar-SistemaCanchas.ps1` (administrador).

Después, en SSMS, `{app}\sql\A11_Script_DDL.sql` **solo si la base no existe**.

## 7. Comprobación

| ID | Criterio | Resultado |
|---|---|---|
| INS-01 | `dotnet publish` Release termina bien | OK |
| INS-02 | El `.config` lleva LocalDB y `Encrypt=False` | OK |
| INS-03 | Van las DLL de capas y BCrypt | OK |
| INS-04 | El `.iss` no ejecuta SQL ni borra `aes.key` | OK |
| INS-05 | El PowerShell pide administrador y .NET 4.8 | OK |
| INS-06 | Actualizar no pisa el `.config` existente | OK |
| INS-07 | Solo se copia un SQL (`A11_Script_DDL.sql`) | OK |

## 8. Veredicto

**Aprobado.** Hay instalador gráfico (cuando Inno está) e instalador PowerShell. El SQL queda como archivo de apoyo, no se corre solo.

## 9. Cambios del documento

| Versión | Fecha | Qué cambió |
|---|---|---|
| 1.0 | 24 ago 2026 | Payload, Inno y PowerShell |
| 1.1 | 25 ago 2026 | El paquete lleva un solo script SQL |

---

*Sigue A19 (Manual de Usuario).*
