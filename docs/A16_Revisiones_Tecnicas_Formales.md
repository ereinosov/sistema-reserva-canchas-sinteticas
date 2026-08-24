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
| Fecha de inspección | 24 de agosto de 2026 |
| Producto inspeccionado | Solución `src/SistemaCanchasSinteticas.sln` (A13) + `docs/A11_Script_DDL.sql` |
| Baseline de código | Commit `637234e` (A13) sobre `main`; documentación A15 en `936e59d` |
| Criterios de inspección | A3 (capas), A8 (clases y nombres), A11 (DDL y procedimientos), A12 (estándares C# .NET 4.8) |
| Norma de referencia | IEEE 1028-2008 (*Software Reviews and Audits*), tipo **inspección**; Fagan (1976); Pressman & Maxim (2019), revisiones técnicas formales |

---

## 1. Objetivo

Determinar, mediante una inspección formal con listas de comprobación, si el código de A13 **cumple los estándares y el diseño ya aceptados** (A3, A8, A11, A12) antes de pasar a despliegue (A17) e instalación (A18).

La FTR no sustituye a las pruebas (A14/A15): las pruebas preguntan *si el sistema hace lo pedido*; la inspección pregunta *si el producto está construido como se acordó*.

## 2. Alcance

**Incluye**

- Dependencias entre proyectos (Presentación, Negocio, Datos, Entidades, Tests).
- Nomenclatura de servicios, repositorios, formularios y controles (A8 / A12).
- Estilo Allman, versión de lenguaje, advertencias como error y documentación XML de la API pública.
- Acceso a datos: solo procedimientos de A11, parámetros tipados, ausencia de SQL dinámico.
- Cifrado: bcrypt costo 12 y AES-256 CBC + DPAPI (A12 §10).
- Permisos GRANT de A11 alineados con roles de A1 §2.3.
- Visibilidad `InternalsVisibleTo` hacia Tests (constructores internos para dobles).

**Excluye** (artefactos posteriores)

- Instalación en equipo limpio y prueba de arranque (A17).
- Empaquetado del instalador (A18).
- Redacción y evaluación del manual de usuario (A19).
- Re-ejecución de casos RG-xx (ya cerrados en A15).

## 3. Tipo de revisión y proceso

Según IEEE 1028-2008 se aplica una **inspección** (no un walkthrough informal): hay roles, listas de comprobación derivadas de artefactos previos, registro de hallazgos y criterio de salida explícito.

| Fase (Fagan / IEEE 1028) | Actividad en este corte |
|---|---|
| Planificación | Se fijan baseline, criterios (A3/A8/A11/A12) y listas de la sección 6. |
| Visión general | Recorrido de la solución en cuatro capas y del script A11. |
| Preparación | Cada inspector recorre su lista contra el código fuente (no contra capturas). |
| Reunión de inspección | Consolidación de hallazgos; clasificación mayor / menor / observación. |
| Retrabajo | No se abrió defecto mayor. El hallazgo menor de estilo se registra sin bloqueo (sección 8). |
| Seguimiento | El moderador verifica que no queden defectos mayores abiertos. |

## 4. Roles

| Rol IEEE 1028 | Responsable | Función |
|---|---|---|
| Moderador | Reinoso Vélez Eduardo David | Conduce la inspección, evita debate de diseño nuevo, declara el veredicto. |
| Secretario | Calderón Saltos Joseph Alexander | Registra hallazgos, disposición y métricas. |
| Autor (grupo) | CHRS | Responde dudas de intención; no veta hallazgos. |
| Inspector | Herrera Barco Humberto Aldair | Lista A3/A8 (capas, nombres, formularios). |
| Inspector | Silva Triviño John Jairo | Lista A11/A12 (SQL, cifrado, parámetros, estilo). |
| Inspector | Calderón Saltos Joseph Alexander | Lista de seguridad de capas (Presentación no toca Datos; Tests y `InternalsVisibleTo`). |
| Inspector | Reinoso Vélez Eduardo David | Lista de API pública (XML, constructores internos, composición de servicios). |

## 5. Criterios de entrada y de salida

**Entrada (todos cumplidos)**

1. A12 publicado y usado como norma de código.
2. A13 compilable con `TreatWarningsAsErrors=true` (0 errores, 0 advertencias).
3. A14 = 70/70 y A15 con veredicto APROBADO.
4. Baseline identificable en `main`.

**Salida**

La inspección se cierra si y solo si:

1. Toda ítem de las listas de la sección 6 tiene disposición **C** (conforme), **O** (observación no bloqueante) o **NC** retrabajada.
2. Cero defectos **mayores** abiertos.
3. El moderador emite veredicto APROBADO o APROBADO CON OBSERVACIONES.

## 6. Listas de comprobación y evidencia

Disposición: **C** = conforme; **O** = observación (no impide el cierre); **NC** = no conforme (defecto).

### 6.1 Arquitectura (A3)

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| ARQ-01 | Presentación referencia Negocio y Entidades; **no** referencia Datos | `SistemaCanchas.Presentacion.csproj`: ProjectReference solo a Negocio y Entidades | C |
| ARQ-02 | Negocio referencia Datos y Entidades | `SistemaCanchas.Negocio.csproj` | C |
| ARQ-03 | Datos referencia solo Entidades | `SistemaCanchas.Datos.csproj` | C |
| ARQ-04 | Entidades no referencia otras capas del sistema | `SistemaCanchas.Entidades.csproj` sin ProjectReference de aplicación | C |
| ARQ-05 | Formularios no instancian repositorios | Búsqueda `new *Repository` en Presentación: 0 coincidencias. Composición: `new XService(_usuarioService)` en `FrmPrincipal`; raíz `new UsuarioService()` solo en `FrmLogin` | C |
| ARQ-06 | Presentación no usa `SqlConnection` / `SqlCommand` / `using SistemaCanchas.Datos` | Búsqueda en `SistemaCanchas.Presentacion`: 0 coincidencias | C |
| ARQ-07 | Entidades es capa transversal (POCOs 1:1 con A11) | `Cliente`, `Cancha`, `Usuario`, `Reserva`, `Horario`, `Pago`, `Ingreso`, `ConsultaIngresos`, `Rol` | C |
| ARQ-08 | Menú de empleado oculta canchas, usuarios e ingresos (A1 §2.3) | `FrmPrincipal.ConfigurarSegunRol`: `mnuCanchas`, `mnuUsuarios`, `mnuIngresos`, `mnuAdministracion` visibles solo si el rol es administrador | C |

### 6.2 Diagrama de clases y nombres (A8 / A12)

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| NOM-01 | Servicios `*Service` + interfaz `I*Service` | `UsuarioService`, `ClienteService`, `CanchaService`, `ReservaService`, `PagoService`, `IngresoService` y sus interfaces | C |
| NOM-02 | Repositorios `*Repository` + interfaz `I*Repository` | Incluye `HorarioRepository` para `sp_ConsultarDisponibilidad` | C |
| NOM-03 | Formularios `Frm*` | `FrmLogin`, `FrmPrincipal`, `FrmClientes`, `FrmReservas`, `FrmPagos`, `FrmDisponibilidad`, `FrmCanchas`, `FrmUsuarios`, `FrmIngresos`, `FrmConfiguracionInicial` | C |
| NOM-04 | Prefijos húngaros de controles (txt, btn, dgv, cbo, lbl, dtp, grp, pnl, mnu, slbl, err, lnk) | Verificados en todos los `*.Designer.cs`; `errValidacion` (ErrorProvider) y `lnkPrimeraInstalacion` (LinkLabel) | C |
| NOM-05 | Identificadores en español; campos privados `_camelCase` | Recorrido de servicios y repositorios (p. ej. `_usuarioService`, `_gestorConexion`) | C |
| NOM-06 | Tests no referencian Presentación (unidad de negocio/datos) | `SistemaCanchas.Tests.csproj`: Datos, Entidades, Negocio | C |

### 6.3 Estándares de codificación (A12)

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| EST-01 | `LangVersion` 7.3 en toda la solución | `src/Directory.Build.props` | C |
| EST-02 | `TreatWarningsAsErrors` = true, WarningLevel 4 | `src/Directory.Build.props` | C |
| EST-03 | Sin `var`; sin LINQ (`System.Linq`) | Búsqueda `\bvar\b` y `using System.Linq`: 0 coincidencias | C |
| EST-04 | Llaves Allman (apertura en línea propia) | Búsqueda `) {` en `src/**/*.cs`: 0 coincidencias | C |
| EST-05 | Accesores de propiedad en Allman | Un getter en una sola línea en `GestorConexion.Instancia` (`get { return InstanciaUnica; }`) | O |
| EST-06 | XML en API pública de Negocio y Datos | `GenerateDocumentationFile=true` en esos dos csproj; interfaces de servicio con `<summary>`/`<param>` | C |
| EST-07 | Presentación y Tests sin XML obligatorio | `GenerateDocumentationFile=false` + `NoWarn` 1591; A12 exige XML en Servicio/Repositorio, no en formularios | C |
| EST-08 | Constructores de prueba `internal` + `InternalsVisibleTo("SistemaCanchas.Tests")` | `AssemblyInfo.cs` de Datos y Negocio; ctors internos en los seis servicios | C |
| EST-09 | Sin marcadores TODO/FIXME/HACK | Búsqueda en `src`: 0 coincidencias | C |
| EST-10 | `catch (Exception)` en UI no traga fallos inesperados | `MostrarError` solo muestra `ValidacionNegocioException`, `OperacionNoPermitidaException` y `ErrorInfraestructuraException`; el resto se relanza | C |

### 6.4 Acceso a datos y A11

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| SQL-01 | Todo comando es procedimiento almacenado | Cada `SqlCommand` usa nombre `sp_*` y `CommandType.StoredProcedure` | C |
| SQL-02 | Cero SQL embebido (SELECT/INSERT/UPDATE/DELETE concatenados) | Búsqueda de literales SQL y `AddWithValue`: 0 coincidencias | C |
| SQL-03 | Parámetros tipados (`SqlDbType` + tamaño) | `ParametroSql` (VarChar, NVarChar, Char, Int, Decimal, Date, Time); ningún `AddWithValue` | C |
| SQL-04 | Lectura de CHAR recortada | `LectorSql.CadenaFija` → `TrimEnd()` | C |
| SQL-05 | Mapeo C# ↔ A11 completo (llamadas directas) | Tabla 6.4.1 | C |
| SQL-06 | `sp_GenerarHorariosDia` no se invoca desde C# (anidado en `sp_ConsultarDisponibilidad`) | `HorarioRepository` llama solo `sp_ConsultarDisponibilidad`; A11 GRANT a ambos roles | C |
| SQL-07 | GRANT empleado vs administrador según A1 §2.3 | A11 líneas 972–992: empleado no recibe EliminarCliente, Canchas (alta/mod/desact.), Usuarios ni Ingresos | C |
| SQL-08 | `login_bootstrap` solo ejecuta `sp_ObtenerCredencialesLogin` | `GRANT EXECUTE ON dbo.sp_ObtenerCredencialesLogin TO login_bootstrap` | C |

#### 6.4.1 Correspondencia procedimientos A11 ↔ repositorio

| Procedimiento A11 | Repositorio | Método |
|---|---|---|
| `sp_ObtenerCredencialesLogin` | `UsuarioRepository` | `ObtenerCredenciales` |
| `sp_RegistrarUsuario` | `UsuarioRepository` | registro / admin inicial |
| `sp_DesactivarUsuario` | `UsuarioRepository` | desactivar |
| `sp_ConsultarUsuarios` | `UsuarioRepository` | `ObtenerTodos` |
| `sp_RegistrarCliente` / `Consultar` / `Modificar` / `Eliminar` | `ClienteRepository` | CRUD |
| `sp_RegistrarCancha` / `Consultar` / `Modificar` / `Desactivar` | `CanchaRepository` | CRUD |
| `sp_CrearReserva` / `ConsultarReservas` / `ModificarReservaHorario` / `CancelarReserva` | `ReservaRepository` | ciclo de reserva |
| `sp_ConsultarDisponibilidad` | `HorarioRepository` | disponibilidad (RN08) |
| `sp_GenerarHorariosDia` | — (EXEC interno) | franjas 06:00–22:00 |
| `sp_RegistrarPago` / `sp_ConsultarEstadoPago` | `PagoRepository` | pagos (RN04) |
| `sp_ConsultarIngresos` | `IngresoRepository` | dos result sets (total + detalle) |

### 6.5 Seguridad de aplicación (A12 §10 / RNF)

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| SEG-01 | bcrypt costo 12 para `clave_app_hash` | `ValoresDominio.CostoHashAplicacion = 12`; `BCrypt.HashPassword(claveApp, …)` | C |
| SEG-02 | AES-256 CBC, clave 32 bytes, IV aleatorio prefijado, PKCS7 | `CifradorAes.CrearAes`: `KeySize=256`, `Mode=CBC`, IV 16 bytes concatenado, salida Base64 | C |
| SEG-03 | Clave AES persistida con DPAPI; nunca en git | `AlmacenClaveAes`: ProgramData o LocalAppData, archivo `aes.key`; no versionado | C |
| SEG-04 | Login de dos fases: bootstrap → hash app → descifrado `clave_bd_enc` → `SqlCredential` | `UsuarioService.ValidarCredenciales` + `GestorConexion.EstablecerSesion` | C |
| SEG-05 | Sanitización del login de motor (`u_` + [a-z0-9_]) | `IdentificadorSql.DesdeLogin` | C |
| SEG-06 | Sesión de aplicación sin secretos | `SanitizarParaSesion` (hash y clave de motor no viajan a la UI) | C |
| SEG-07 | Cadena bootstrap en `App.config` es valor de laboratorio A11 | Password `CAMBIAR_EN_DESPLIEGUE#2026`; comentario de cambio en despliegue (RNF12). Cierre operativo en A17/A18 | O |

### 6.6 Compilación y pruebas como oráculo de la inspección

| ID | Criterio | Evidencia | Disp. |
|---|---|---|---|
| CAL-01 | La solución trata advertencias como error | `Directory.Build.props`; A15 compiló 0 advertencias | C |
| CAL-02 | Suite A14 vigente | 70/70 el 24 ago 2026 (A15 §5) | C |
| CAL-03 | Tests cubren cifrado y reglas de negocio, no la UI | `CifradorAesTests` + `*ServiceTests`; Presentación fuera del csproj de Tests | C |

## 7. Hallazgos

Clasificación: **Mayor** = viola capa, seguridad, A11 o un estándar que A12 marca como obligatorio y puede introducir defecto en producción. **Menor** = estilo local sin impacto funcional. **Observación** = riesgo de entorno o de proceso, no del diseño inspeccionado.

| ID | Tipo | Ítem | Descripción | Disposición |
|---|---|---|---|---|
| DEF-M01 | Menor | EST-05 | El getter de `GestorConexion.Instancia` usa llaves en la misma línea (`get { return InstanciaUnica; }`). Es la única excepción Allman localizada. | **Aceptado.** No altera comportamiento ni visibilidad. No se exige retrabajo para cerrar A16. |
| OBS-01 | Observación | SEG-07 | La clave de `login_bootstrap` permanece en claro en `App.config` porque A11 la documenta como valor de desarrollo. En producción debe sustituirse y protegerse (DPAPI / instalador). | Trasladado a A17/A18 |
| OBS-02 | Observación | (despliegue) | Los complementos `A11_Complemento_Trustworthy.sql`, `A11_Complemento_sp_ConsultarUsuarios.sql` y `A11_Complemento_GrantGenerarHorarios.sql` no son violaciones de A12: cubren estaciones donde A11 ya se ejecutó o LocalDB. | Trasladado a A17 |

**Defectos mayores abiertos: 0.**

## 8. Métricas de inspección

| Métrica | Valor |
|---|---|
| Productos | Solución A13 (5 proyectos) + script A11 (1006 líneas) |
| Listas aplicadas | 6 (ARQ, NOM, EST, SQL, SEG, CAL) |
| Ítems inspeccionados | 40 |
| Conformes (C) | 38 |
| Observaciones (O) ligadas a ítem | 2 (EST-05, SEG-07) |
| No conformes (NC) | 0 |
| Defectos mayores | 0 |
| Defectos menores | 1 (aceptado, no bloquea) |
| Densidad de defectos mayores | 0 |

La tasa de detección se considera adecuada: las listas están ancladas a artefactos previos (no a opinión) y cada ítem C cita archivo o búsqueda reproducible.

## 9. Veredicto

Se aprueba A16 si y solo si se cumplen los criterios de salida de la sección 5.

**Veredicto: APROBADO CON OBSERVACIONES.**

El código de A13 **cumple** la arquitectura en capas, la nomenclatura A8, el contrato de procedimientos A11 y los estándares A12 (incluido el cifrado). Las observaciones no son fallos de diseño: son pendientes de **despliegue** (clave bootstrap, TRUSTWORTHY/GRANTs en estaciones ya instaladas) y un detalle de estilo no bloqueante.

Queda autorizado el paso a **A17 Pruebas de Despliegue**.

## 10. Cómo repetir esta inspección

1. Fijar baseline (`git log -1` sobre `main`).
2. Compilar `src/SistemaCanchasSinteticas.sln` (debe fallar si hay advertencias).
3. Reaplicar las búsquedas de las listas 6.1–6.5 (repositorios en Presentación, `var`, SQL embebido, `AddWithValue`, `System.Linq`, `) {`).
4. Contrastar cada `new SqlCommand("sp_…")` con A11 y con la tabla 6.4.1.
5. Registrar hallazgos con ID, tipo y disposición. Un defecto mayor reabre A13; no se avanza a A17.

## 11. Control de cambios del artefacto

| Versión | Fecha | Descripción |
|---|---|---|
| 1.0 | 24 ago 2026 | Inspección formal IEEE 1028 sobre A13 contra A3, A8, A11 y A12 |

---

*Fin del artefacto A16. El siguiente artefacto del ciclo es A17 (Pruebas de Despliegue).*
