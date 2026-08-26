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
| Fecha | 25 de agosto de 2026 |
| Producto | WinForms .NET 4.8 + base `ReservaCanchasDB` |
| Referencias | A1, A11, A13, A15, A16 |

---

## 1. Qué se prueba aquí

Que el sistema se puede **poner a andar** en un Windows con LocalDB: prerrequisitos, script de base, `App.config` y primer administrador. Los RF01–RF16 ya se cubrieron en A15.

## 2. Regla importante

**No vuelva a ejecutar `docs/A11_Script_DDL.sql` si la base ya tiene datos.** El script hace `DROP TABLE` y se pierde todo.

Ese archivo es el **único** script SQL del proyecto. Trae TRUSTWORTHY, `QUOTED_IDENTIFIER`, horarios por cancha, reservas de varias franjas y todos los GRANT. No hay scripts “complemento”.

Si la base ya existe y está desactualizada, hay que decidir con el grupo: o se migra a mano, o se recrea en un laboratorio vacío. En defensa / PC nuevo: instancia vacía + A11 una vez.

## 3. Entorno de esta corrida

| Elemento | Valor |
|---|---|
| SO | Windows 10 |
| .NET Framework | 4.8 |
| Motor | LocalDB `(localdb)\MSSQLLocalDB` |
| Cifrado de canal | `Encrypt=False` (LocalDB no lo admite) |

## 4. Pasos en un PC nuevo

1. Windows 10/11, .NET 4.8, LocalDB y SSMS.
2. `sqllocaldb start MSSQLLocalDB` si no está en ejecución.
3. En SSMS, contra `(localdb)\MSSQLLocalDB`, abrir `docs/A11_Script_DDL.sql` y ejecutarlo **una vez**.
4. Abrir `src/SistemaCanchasSinteticas.sln`, proyecto de inicio Presentación.
5. Revisar `App.config`: mismo servidor, catálogo `ReservaCanchasDB`, usuario `login_bootstrap`, misma clave que en A11, `Encrypt=False`.
6. Compilar y F5. Si no hay usuarios, usar **Primera configuración**. Si ya hay, entrar con la clave de aplicación.
7. La clave AES se crea sola en `%ProgramData%\SistemaCanchas\aes.key` (o en LocalAppData). No se sube a git.

Hasta A18, “instalar” es copiar `bin\Debug\net48\` o usar el Setup.

## 5. Casos

**P** = pasó. **NA** = no aplica en esta máquina.

| ID | Caso | Resultado |
|---|---|---|
| DP-01 | .NET 4.8 instalado | P |
| DP-02 | LocalDB en ejecución | P |
| DP-03 | `Data Source=(localdb)\MSSQLLocalDB` | P |
| DP-04 | `Encrypt=False` | P |
| DP-05 | Compila sin advertencias | P |
| DP-06 | Existe `ReservaCanchasDB` | P |
| DP-07 | Tablas de A11 | P |
| DP-08 | Procedimientos `sp_*` (incluye activar cancha/usuario, cambiar clave, nombre) | P |
| DP-09 | `TRUSTWORTHY` encendido | P |
| DP-10 | Existe `login_bootstrap` | P |
| DP-11 | Bootstrap solo tiene `EXECUTE` sobre `sp_ObtenerCredencialesLogin` | P |
| DP-12 | Ingresos / baja de cliente / usuarios solo admin | P |
| DP-13 | `GRANT` de `sp_GenerarHorariosDia` a empleado y admin (va en A11) | P |
| DP-14 | Primer admin si no hay usuarios | NA si ya hay cuentas |
| DP-15 | Login de dos fases | P |
| DP-16 | No se puede repetir la configuración inicial | P |
| DP-17 | `aes.key` no está en el repo | P |
| DP-18 | `localhost` en vez de LocalDB | Falla de conexión, timeout 8 s | P |
| DP-19 | `Encrypt=True` contra LocalDB | No conecta | P |
| DP-20 | Re-ejecutar A11 con datos | Prohibido por este procedimiento | P |

## 6. Consultas rápidas (SSMS, sysadmin)

```sql
SELECT name, is_trustworthy_on
FROM sys.databases
WHERE name = N'ReservaCanchasDB';

SELECT COUNT(*) AS tablas FROM sys.tables WHERE schema_id = SCHEMA_ID(N'dbo');
SELECT COUNT(*) AS procedimientos FROM sys.procedures WHERE name LIKE N'sp_%';
```

GRANT de franjas:

```sql
SELECT dp.name AS rol
FROM sys.database_permissions p
INNER JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
INNER JOIN sys.objects o ON p.major_id = o.object_id
WHERE o.name = N'sp_GenerarHorariosDia';
```

Deben salir `db_rol_empleado` y `db_rol_administrador`.

## 7. Veredicto

**Aprobado.** Con el script único y `Encrypt=False` el camino de puesta en marcha queda cerrado. El instalador (A18) copia el exe; el SQL lo sigue aplicando quien administra el servidor, a mano, una vez.

## 8. Cómo repetirla

1. `sqllocaldb info MSSQLLocalDB`
2. Revisar `App.config`
3. F5 y login (o primera configuración)
4. No rehacer A11 sobre datos reales

## 9. Cambios del documento

| Versión | Fecha | Qué cambió |
|---|---|---|
| 1.0 | 24 ago 2026 | Primera corrida; aún se hablaba de complementos |
| 1.1 | 25 ago 2026 | Un solo `A11_Script_DDL.sql`; DP-13 queda cubierto por ese archivo |

---

*Sigue A18 (Desarrollo de Instaladores).*
