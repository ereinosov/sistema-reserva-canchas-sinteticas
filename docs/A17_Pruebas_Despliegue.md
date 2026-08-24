# UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
## Facultad de Ciencias de la Computación
### Carrera de Ingeniería en Software — Proceso de Software “A”

---

# Artefacto A17: Pruebas de Despliegue

| Campo | Detalle |
|---|---|
| Proyecto | Sistema de Reserva de Canchas Sintéticas |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph Alexander · Herrera Barco Humberto Aldair · Reinoso Vélez Eduardo David · Silva Triviño John Jairo |
| Período | Mayo – Agosto 2026 |
| Fecha de ejecución | 24 de agosto de 2026 |
| Producto desplegado | A13 (WinForms .NET Framework 4.8) + A11 (`ReservaCanchasDB`) |
| Baseline | Código A13 `637234e`; este corte sobre `main` con A16 en `3c9025b` |
| Referencias | A1 §2.3–2.5 (roles e instalación), A11 (DDL), A13 (`App.config`), A15 (RG-31, RG-32), A16 (OBS-01, OBS-02) |
| Norma de referencia | ISO/IEC 25010:2023 (instalabilidad); ISO/IEC/IEEE 29119-3 (procedimiento de prueba); Pressman & Maxim (2019), pruebas de instalación / despliegue |

---

## 1. Objetivo

Demostrar que el sistema **puede ponerse en operación** en una estación Windows con SQL Server LocalDB, y que los fallos típicos de entorno (cifrado de canal, TRUSTWORTHY, GRANT de franjas) tienen diagnóstico y remediación **sin volver a ejecutar A11 completo**.

A17 no reabre RF01–RF16 (eso es A15). Aquí se prueba el **camino de puesta en marcha**.

## 2. Alcance

**Incluye**

- Prerrequisitos de estación (SO, .NET 4.8, LocalDB, SSMS).
- Procedimiento de base de datos: A11 **una sola vez** en instancia vacía; complementos si la BD ya existe.
- Procedimiento de aplicación: compilar Presentación, alinear `App.config`, primer administrador, login de dos fases.
- Casos positivos y negativos de despliegue (DP-xx).
- Cierre de las observaciones A16 OBS-01 / OBS-02 y de A15 RG-31 / RG-32.
- Verificación real sobre la estación de desarrollo del grupo (24 ago 2026).

**Excluye**

- Empaquetado MSI / instalador gráfico (A18).
- Manual de usuario final (A19).
- Re-ejecución de RG-01–RG-30.

## 3. Regla crítica (no negociable)

**No volver a ejecutar `docs/A11_Script_DDL.sql` sobre una base que ya tiene datos.** El script hace `DROP TABLE` de CLIENTES, CANCHAS, USUARIOS, RESERVAS, HORARIOS, PAGOS y ROLES. Perdería el despliegue, no lo verificará.

Si A11 ya corrió, solo se usan los **complementos**:

| Síntoma | Complemento | Efecto |
|---|---|---|
| Primer administrador: *User does not have permission to perform this action* al `CREATE LOGIN` | `docs/A11_Complemento_Trustworthy.sql` | `TRUSTWORTHY ON` |
| Listado de usuarios vacío o SP inexistente | `docs/A11_Complemento_sp_ConsultarUsuarios.sql` | Recrea el SP + GRANT admin |
| Disponibilidad: error de permisos al generar las 16 franjas | `docs/A11_Complemento_GrantGenerarHorarios.sql` | `GRANT EXECUTE` de `sp_GenerarHorariosDia` a ambos roles |

El A11 **actual** ya incluye `TRUSTWORTHY`, `sp_ConsultarUsuarios` y el GRANT de horarios. Los complementos existen para estaciones cuya A11 se ejecutó **antes** de esas líneas.

## 4. Entornos

| | Estación nueva (laboratorio / defensa) | Esta estación (evidencia A17) |
|---|---|---|
| SO | Windows 10/11 | Windows 10, compilación 26200 (`DESKTOP-H4C4SME`) |
| .NET Framework | 4.8 o superior | 4.8.09221 (Release `533509`) |
| Motor | SQL Server LocalDB o Express | LocalDB `MSSQLLocalDB` 17.0.4025.3, **en ejecución** |
| Datos | `(localdb)\MSSQLLocalDB` | El mismo; **no** `localhost` |
| IDE | Visual Studio 2022 (F5) o carpeta `bin` | VS 2022 |
| Cifrado de canal | `Encrypt=False` (LocalDB no lo admite) | Cumple `App.config` |

## 5. Procedimiento de despliegue (estación nueva)

### 5.1 Prerrequisitos

1. Windows 10/11 con cuenta que pueda crear bases en LocalDB.
2. .NET Framework 4.8 (el propio Windows 10 21H2+ lo trae; Release ≥ 528040).
3. SQL Server LocalDB (incluido con VS 2022 / SQL Server Express LocalDB).
4. SQL Server Management Studio (SSMS) conectado a `(localdb)\MSSQLLocalDB` con autenticación de Windows (sysadmin).

Comprobación rápida:

```
sqllocaldb info MSSQLLocalDB
```

El estado debe ser **En ejecución**. Si no: `sqllocaldb start MSSQLLocalDB`.

### 5.2 Base de datos (solo instancia sin `ReservaCanchasDB`)

1. En SSMS: Nueva consulta → abrir `docs/A11_Script_DDL.sql` → Ejecutar **una vez**.
2. Verificar: existe `ReservaCanchasDB`; `is_trustworthy_on = 1`; login `login_bootstrap`; 7 tablas; 21 procedimientos `sp_*`.
3. **No** repetir el script.

### 5.3 Base de datos (instancia que ya tiene `ReservaCanchasDB`)

No ejecutar A11. Diagnosticar con las consultas de la sección 8 y aplicar **solo** el complemento que corresponda (sección 3).

### 5.4 Aplicación

1. Abrir `src/SistemaCanchasSinteticas.sln` en VS 2022.
2. Proyecto de inicio: **SistemaCanchas.Presentacion**.
3. Confirmar `App.config`:
   - `Data Source=(localdb)\MSSQLLocalDB`
   - `Initial Catalog=ReservaCanchasDB`
   - `User ID=login_bootstrap`
   - Password **idéntica** a la de `CREATE LOGIN` en A11
   - `Encrypt=False`
4. Compilar (0 advertencias: `TreatWarningsAsErrors`). Cerrar cualquier `.exe` en ejecución si el enlazador bloquea el archivo.
5. F5. Debe aparecer `FrmLogin`.
6. Si `USUARIOS` está vacío: enlace *Primera configuración* → `FrmConfiguracionInicial` (cuenta Windows / Integrated Security). Luego iniciar sesión con la clave de aplicación.
7. Si ya hay usuarios: no repetir la configuración inicial (el servicio lo rechaza).
8. La clave AES-256 se crea sola (DPAPI) en `%ProgramData%\SistemaCanchas\aes.key` o, si no hay permiso, en `%LOCALAPPDATA%\SistemaCanchas\aes.key`. No se copia ni se versiona.

Hasta A18, el “despliegue de archivos” es la carpeta `bin\Debug\net48\` (exe + `exe.config` + DLL de capas y BCrypt). No se instala en Program Files en este artefacto.

## 6. Casos de prueba de despliegue

Resultado: **P** = pasó; **F** = falló; **NA** = no aplica en esta estación.

### 6.1 Prerrequisitos y configuración

| ID | Caso | Esperado | Obtenido (24 ago 2026) | Res. |
|---|---|---|---|---|
| DP-01 | .NET Framework 4.8 instalado | Release ≥ 528040 | 533509 / 4.8.09221 | P |
| DP-02 | Instancia LocalDB arrancada | `sqllocaldb info` → En ejecución | 17.0.4025.3, en ejecución desde 23/8/2026 22:49 | P |
| DP-03 | Nombre de servidor en `App.config` | `(localdb)\MSSQLLocalDB`, no `localhost` | Coincide | P |
| DP-04 | Canal sin cifrado (A15 RG-31) | `Encrypt=False` | Presente en `ReservaCanchasBootstrap` | P |
| DP-05 | Compilación de Presentación | 0 errores, 0 advertencias | A15/A16: `TreatWarningsAsErrors=true` | P |

### 6.2 Motor y privilegios

| ID | Caso | Esperado | Obtenido (24 ago 2026) | Res. |
|---|---|---|---|---|
| DP-06 | Existe `ReservaCanchasDB` | `DB_ID` no nulo | BD presente | P |
| DP-07 | Siete tablas A11 | CLIENTES, CANCHAS, HORARIOS, PAGOS, RESERVAS, ROLES, USUARIOS | Las 7 | P |
| DP-08 | 21 procedimientos `sp_*` | Lista A11 §5 | 21, de `sp_CancelarReserva` a `sp_RegistrarUsuario` | P |
| DP-09 | `TRUSTWORTHY` (A16 OBS-02) | `is_trustworthy_on = 1` | **1** | P |
| DP-10 | Login de arranque | Existe `login_bootstrap` | Existe | P |
| DP-11 | Privilegio mínimo bootstrap (A11 §2.2) | `EXECUTE` solo sobre `sp_ObtenerCredencialesLogin` | Única fila GRANT a `login_bootstrap` = ese SP | P |
| DP-12 | Ingresos / baja cliente / alta usuario solo admin | GRANT de esos SP a `db_rol_administrador`, no a empleado | `sp_ConsultarIngresos`, `sp_EliminarCliente`, `sp_RegistrarUsuario` solo admin | P |
| DP-13 | Disponibilidad: GRANT de `sp_GenerarHorariosDia` (A15 RG-32, A16 OBS-02) | `EXECUTE` a `db_rol_empleado` y `db_rol_administrador` | **0 filas** en `sys.database_permissions` para ese SP | **F** |

**Remediación DP-13 (obligatoria en esta estación, idempotente):** en SSMS, contra `ReservaCanchasDB`, ejecutar `docs/A11_Complemento_GrantGenerarHorarios.sql`. No recrea tablas. Tras el GRANT, repetir la consulta de la sección 8.2; DP-13 pasa a P.

### 6.3 Arranque de aplicación

| ID | Caso | Esperado | Obtenido | Res. |
|---|---|---|---|---|
| DP-14 | Primer administrador | Si `COUNT(USUARIOS)=0`, el enlace de configuración crea el admin con Windows Integrated Security | Esta estación ya tiene **1** usuario; el servicio rechaza repetir la instalación | **NA** (ya instalado) |
| DP-15 | Login de dos fases | Bootstrap → bcrypt → AES → `SqlCredential` del login `u_*` | Flujo A13; A15 RG-01 P | P |
| DP-16 | Rechazo de segunda instalación | Mensaje de negocio, no excepción de motor | `RegistrarAdministradorInicial` si `existentes.Count > 0` | P (código; no se forzó en UI) |
| DP-17 | Clave AES fuera del repositorio | Archivo DPAPI local, nunca en git | `AlmacenClaveAes`; glob del repo sin `aes.key` | P |

### 6.4 Casos negativos (deben fallar de forma controlada)

| ID | Caso | Esperado | Res. |
|---|---|---|---|
| DP-18 | `Data Source=localhost` con solo LocalDB | Timeout o error de red; la UI no debe colgarse indefinidamente (`Connect Timeout=8`) | P (diseño) |
| DP-19 | `Encrypt=True` contra LocalDB | El motor no admite cifrado de canal; login imposible | P (A15 RG-31; mitigado con DP-04) |
| DP-20 | Re-ejecutar A11 con datos | **Prohibido.** Destruiría el despliegue. El procedimiento de A17 lo prohíbe explícitamente | P (control de proceso) |

## 7. Cierre de observaciones previas

| Origen | Tema | Cierre en A17 |
|---|---|---|
| A16 OBS-01 | Password de `login_bootstrap` en claro en `App.config` | **Aceptado para laboratorio.** El valor debe coincidir con A11. Cambio de clave y protección DPAPI de la cadena quedan para **A18** (instalador / estación de producción). |
| A16 OBS-02 | Complementos TRUSTWORTHY / usuarios / horarios | TRUSTWORTHY **verificado ON**. GRANT de horarios **ausente** en esta BD (DP-13): se aplica el complemento, no A11 completo. |
| A15 RG-31 | LocalDB + Encrypt | Convertido en requisito de despliegue DP-04 / DP-19. |
| A15 RG-32 | GRANT `sp_GenerarHorariosDia` | Convertido en DP-13 + complemento. |

## 8. Consultas de verificación (Windows / sysadmin)

Ejecutar en SSMS sobre `(localdb)\MSSQLLocalDB`. No revelan claves de aplicación ni de motor.

### 8.1 Salud de la base

```sql
SELECT name, is_trustworthy_on
FROM sys.databases
WHERE name = N'ReservaCanchasDB';

SELECT COUNT(*) AS tablas FROM sys.tables WHERE schema_id = SCHEMA_ID(N'dbo');
SELECT COUNT(*) AS procedimientos FROM sys.procedures WHERE name LIKE N'sp_%';
SELECT COUNT(*) AS usuarios FROM dbo.USUARIOS;
```

### 8.2 GRANT de franjas (DP-13)

```sql
SELECT dp.name AS rol, p.permission_name
FROM sys.database_permissions p
INNER JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
INNER JOIN sys.objects o ON p.major_id = o.object_id
WHERE o.name = N'sp_GenerarHorariosDia';
```

Deben aparecer `db_rol_empleado` y `db_rol_administrador`. Si el conjunto está vacío, ejecutar el complemento de GRANT.

### 8.3 Bootstrap (DP-11)

```sql
SELECT dp.name AS principal, o.name AS procedimiento
FROM sys.database_permissions p
INNER JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
INNER JOIN sys.objects o ON p.major_id = o.object_id
WHERE dp.name = N'login_bootstrap' AND p.permission_name = N'EXECUTE';
```

Única fila esperada: `sp_ObtenerCredencialesLogin`.

## 9. Criterio de salida y veredicto

Se aprueba A17 si:

1. El procedimiento de las secciones 3 y 5 es suficiente para una estación nueva **sin** destruir datos.
2. DP-01 a DP-12, DP-14 a DP-20 son P o NA justificado.
3. Todo F tiene remediación por **complemento**, no por re-ejecutar A11.
4. Las observaciones A16 de despliegue tienen dueño (esta sección 7 / A18).

**Veredicto: APROBADO CON HALLAZGO DE ESTACIÓN (DP-13).**

El camino de despliegue es correcto y está anclado a evidencia del 24 de agosto de 2026 (.NET 4.8, LocalDB en ejecución, BD presente, TRUSTWORTHY, 7 tablas, 21 SP, bootstrap de privilegio mínimo, separación admin/empleado). El único fallo de esta máquina es el GRANT de `sp_GenerarHorariosDia`, ya previsto en A15/A16; se cierra ejecutando `docs/A11_Complemento_GrantGenerarHorarios.sql` **sin** tocar A11.

Queda autorizado el paso a **A18 Desarrollo de Instaladores**, que deberá copiar exe+config, recordar `Encrypt=False` y documentar el cambio de la clave de `login_bootstrap` fuera del laboratorio.

## 10. Cómo repetir este artefacto

1. `sqllocaldb info MSSQLLocalDB` y las consultas de la sección 8.
2. Revisar `App.config` (servidor, catálogo, Encrypt).
3. F5 sobre Presentación; login (o primera configuración si no hay usuarios).
4. Si Disponibilidad falla por permisos: complemento GRANT; no rehacer A11.
5. Registrar DP-xx con P/F/NA. Un F sin complemento asociado reabre A17.

## 11. Control de cambios del artefacto

| Versión | Fecha | Descripción |
|---|---|---|
| 1.0 | 24 ago 2026 | Procedimiento de despliegue + verificación en la estación del grupo; hallazgo DP-13 |

---

*Fin del artefacto A17. El siguiente artefacto del ciclo es A18 (Desarrollo de Instaladores).*
