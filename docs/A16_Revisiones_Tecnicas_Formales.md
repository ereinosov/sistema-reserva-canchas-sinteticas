# UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
## Facultad de Ciencias de la Computación
### Carrera de Ingeniería en Software — Proceso de Software “A”

---

# Artefacto A16: Revisiones Técnicas Formales

| Campo | Detalle |
|---|---|
| Proyecto | Sistema de Reserva de Canchas Sintéticas |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph Alexander · Herrera Barco Humberto Aldair · Reinoso Vélez Eduardo David · Silva Triviño John Jairo |
| Período | Mayo – Agosto 2026 |
| Fecha | 25 de agosto de 2026 |
| Producto revisado | Solución `src/SistemaCanchasSinteticas.sln` y `docs/A11_Script_DDL.sql` |
| Criterios | A3 (capas), A8 (nombres), A11 (SQL), A12 (estándares C#) |
| Tipo de revisión | Inspección (IEEE 1028-2008) |

---

## 1. Qué se buscó

Ver si el código está armado como se acordó en A3, A8, A11 y A12. Las pruebas (A14/A15) dicen si el programa hace lo pedido; esta revisión mira **cómo está escrito**.

## 2. Alcance

Se revisaron referencias entre proyectos, nombres de servicios y formularios, estilo Allman, advertencias como error, acceso solo por procedimientos de A11, bcrypt/AES y los GRANT del script único.

No se revisó aquí la instalación (A17), el Setup (A18) ni el manual (A19).

## 3. Roles

| Rol | Quién |
|---|---|
| Moderador | Reinoso Vélez Eduardo David |
| Secretario | Calderón Saltos Joseph Alexander |
| Inspectores | Herrera Barco Humberto Aldair (capas y nombres), Silva Triviño John Jairo (SQL y cifrado), Calderón Saltos y Reinoso Vélez (API y Tests) |

## 4. Entrada y salida

Para empezar hacía falta A12 publicado, la solución compilando con `TreatWarningsAsErrors`, A14 en 83/83 y A15 aprobado.

Se cierra la inspección si no queda ningún defecto **mayor** abierto. Un detalle de estilo se puede dejar como observación.

## 5. Listas (resumen)

**C** = cumple. **O** = observación. **NC** = no cumple.

### Arquitectura

| ID | Criterio | Disp. |
|---|---|---|
| ARQ-01 | Presentación no referencia Datos | C |
| ARQ-02 | Negocio referencia Datos y Entidades | C |
| ARQ-03 | Datos solo referencia Entidades | C |
| ARQ-04 | Entidades no depende de otras capas | C |
| ARQ-05 | Los formularios no crean repositorios | C |
| ARQ-06 | Presentación no usa `SqlConnection` | C |
| ARQ-07 | El menú del empleado oculta canchas, usuarios e ingresos | C |

### Nombres

Servicios `*Service`, repositorios `*Repository`, formularios `Frm*`, prefijos de controles (txt, btn, dgv, cbo…). Los tests no referencian Presentación. Todo **C**.

### Estándares A12

LangVersion 7.3, advertencias como error, sin `var`, sin LINQ, llaves Allman. XML en Negocio y Datos. `InternalsVisibleTo` para Tests.

| ID | Nota | Disp. |
|---|---|---|
| EST-05 | El getter de `GestorConexion.Instancia` lleva las llaves en la misma línea | O |

El resto de ítems de estilo: **C**.

### SQL y A11

Todo comando es `CommandType.StoredProcedure`. No hay SQL concatenado ni `AddWithValue`. Un solo script: `docs/A11_Script_DDL.sql` (incluye TRUSTWORTHY, `QUOTED_IDENTIFIER`, tipo `ListaIdsHorario`, horarios por cancha y GRANT de `sp_GenerarHorariosDia`).

`login_bootstrap` solo ejecuta `sp_ObtenerCredencialesLogin`. El empleado no tiene EliminarCliente, alta de canchas/usuarios ni ingresos.

### Seguridad

bcrypt costo 12, AES-256 CBC, clave AES con DPAPI fuera de git, login en dos fases (bootstrap → hash de app → clave de motor). La contraseña de `login_bootstrap` en `App.config` es de laboratorio (**O**, se trata en A18).

## 6. Hallazgos

| ID | Tipo | Qué se vio | Qué se hizo |
|---|---|---|---|
| DEF-M01 | Menor | Getter de `GestorConexion.Instancia` no Allman | Se deja. No cambia el comportamiento |
| OBS-01 | Observación | Clave bootstrap en claro en `App.config` | En producción se cambia y se cifra en la estación (A18) |

Defectos mayores abiertos: **0**.

## 7. Veredicto

**Aprobado con observaciones.** El código respeta las capas, los nombres y el script A11. Lo pendiente es de despliegue, no de diseño.

## 8. Cómo repetirla

1. `git log -1` para saber el commit.
2. Compilar la solución (debe fallar si hay advertencias).
3. Buscar `new *Repository` en Presentación, `var`, SQL embebido, `AddWithValue`, `System.Linq`.
4. Cruzar cada `sp_*` con `docs/A11_Script_DDL.sql`.

## 9. Cambios del documento

| Versión | Fecha | Qué cambió |
|---|---|---|
| 1.0 | 24 ago 2026 | Primera inspección |
| 1.1 | 25 ago 2026 | Un solo script A11; se quitan los “complementos”; A14 = 83 casos |

---

*Sigue A17 (Pruebas de Despliegue).*
