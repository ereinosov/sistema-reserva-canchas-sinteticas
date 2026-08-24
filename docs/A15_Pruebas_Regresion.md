# UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
## Facultad de Ciencias de la Computación
### Carrera de Ingeniería en Software — Proceso de Software “A”

---

# Artefacto A15: Pruebas de Regresión

| Campo | Detalle |
|---|---|
| Proyecto | Sistema de Reserva de Canchas Sintéticas |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph Alexander · Herrera Barco Humberto Aldair · Reinoso Vélez Eduardo David · Silva Triviño John Jairo |
| Período | Mayo – Agosto 2026 |
| Fecha de ejecución | 24 de agosto de 2026 |
| Versión del sistema bajo prueba | A13 (solución `src/SistemaCanchasSinteticas.sln`, .NET Framework 4.8) |
| Baseline de código | Commit `637234e` en `main` |
| Referencias | A1 (requisitos RF01–RF16 / RN01–RN13), A3 (arquitectura), A11 (DDL y procedimientos), A12 (estándares), A13 (código), A14 (pruebas unitarias) |
| Norma de referencia | ISO/IEC/IEEE 29119-3 (documentación de pruebas); Pressman & Maxim (2019), cap. pruebas de regresión |

---

## 1. Objetivo

Verificar que, una vez integrados todos los módulos de A13, **ningún requisito ya aceptado se rompe**. La regresión no descubre funcionalidad nueva: confirma que el sistema completo sigue cumpliendo A1 después de sucesivos incrementos (login, usuarios, canchas, clientes, reservas, pagos e ingresos).

## 2. Alcance

**Incluye**

- Re-ejecución completa de la suite A14 (MSTest, 70 casos).
- Suite manual de sistema sobre la interfaz WinForms, con sesión de administrador y de empleado.
- Comprobación de reglas de negocio RN01–RN13 que cruzan módulos (unicidad de franja, un pago por reserva, ingresos solo pagados, visibilidad por rol).
- Compilación de la solución con advertencias tratadas como error (`TreatWarningsAsErrors`).

**Excluye** (pertenecen a artefactos posteriores)

- Pruebas de despliegue en un equipo limpio (A17).
- Empaquetado e instalador (A18).
- Evaluación de usabilidad del manual (A19).

## 3. Estrategia

La construcción fue incremental. Cada módulo nuevo podía invalidar uno anterior (por ejemplo, pagos altera la baja de clientes por RN02; ingresos depende de que el pago quede en `pagado`). Por eso A15 se ejecuta **sobre el sistema ya cerrado**, no sobre un módulo aislado.

Se combinan dos niveles:

| Nivel | Instrumento | Criterio de éxito |
|---|---|---|
| Automatizado | `dotnet test src/SistemaCanchasSinteticas.sln` | 0 fallos, 0 omitidos |
| Sistema (manual) | Casos RG-xx de la sección 6 | Resultado obtenido = resultado esperado |

Un caso de regresión se declara **Fallido** si el comportamiento difiere de A1, aunque la interfaz no muestre excepción.

## 4. Entorno de ejecución

| Elemento | Valor |
|---|---|
| Sistema operativo | Windows 10 (compilación 26200) |
| IDE / SDK | Visual Studio 2022 · SDK .NET 10.0.400 para `dotnet test` |
| Marco de la aplicación | .NET Framework 4.8 |
| Motor de base de datos | SQL Server LocalDB `(localdb)\MSSQLLocalDB` |
| Base de datos | `ReservaCanchasDB` (A11 + complementos TRUSTWORTHY, `sp_ConsultarUsuarios`, `GRANT` de `sp_GenerarHorariosDia`) |
| Cadena de arranque | `login_bootstrap` según `App.config` (Encrypt=False; LocalDB no admite cifrado) |
| Compilación | Configuración Debug · 0 advertencias · 0 errores |

## 5. Nivel automatizado — re-ejecución de A14

Comando:

```
dotnet test src/SistemaCanchasSinteticas.sln --nologo
```

Resultado del 24 de agosto de 2026:

| Métrica | Valor |
|---|---|
| Ensamblado | `SistemaCanchas.Tests.dll` (.NETFramework 4.8) |
| Superados | 70 |
| Con error | 0 |
| Omitidos | 0 |
| Duración | 1 s |
| Veredicto | **APROBADO** |

### 5.1 Distribución por clase (trazabilidad A14 → RF)

| Clase MSTest | Casos | RF / RN cubiertos |
|---|---|---|
| `CifradorAesTests` | 6 | RNF12 / A12 §10.3 (AES-256 CBC, IV aleatorio) |
| `UsuarioServiceTests` | 20 | RF14, RF15, RF16, RN11, RN12 |
| `CanchaServiceTests` | 10 | RF13, RN10 |
| `ClienteServiceTests` | 11 | RF01–RF04, RN02, RN07 |
| `ReservaServiceTests` | 12 | RF05–RF08, RF11, RN01, RN03, RN06, RN08 |
| `PagoServiceTests` | 7 | RF09, RF10, RN04 |
| `IngresoServiceTests` | 4 | RF12, RN09, §2.3 A1 (solo administrador) |
| **Total** | **70** | RF01–RF16 |

La re-ejecución demuestra que el cierre de ingresos (último incremento) **no degradó** login, canchas, clientes, reservas ni pagos.

## 6. Nivel sistema — suite manual de regresión

Precondiciones generales:

1. A11 aplicado una sola vez (no reejecutar el script completo: borra tablas).
2. Complementos aplicados: TRUSTWORTHY, `sp_ConsultarUsuarios`, `GRANT EXECUTE` sobre `sp_GenerarHorariosDia`.
3. Existe un administrador activo (cuenta de instalación).
4. Existe al menos una cancha activa y un cliente, o se crean en RG-02 y RG-03.

Convención: **P** = pasó; **F** = falló; **NA** = no aplica.

### 6.1 Acceso y perfiles

| ID | RF/RN | Pasos | Resultado esperado | Resultado | Estado |
|---|---|---|---|---|---|
| RG-01 | RF16, RN12 | Iniciar sesión con usuario y clave correctos de administrador | Se abre `FrmPrincipal`; barra de estado muestra usuario y rol `administrador` | Coincide | P |
| RG-02 | RF16 | Iniciar sesión con clave incorrecta | Mensaje de credenciales inválidas; no se revela si el login existe | Coincide | P |
| RG-03 | A1 §2.3 | Sesión administrador: inspeccionar menús | Visibles Gestión (Clientes, Reservas, Pagos), Consultas (Disponibilidad, Ingresos) y Administración (Canchas, Usuarios) | Coincide | P |
| RG-04 | A1 §2.3 | Sesión empleado | No aparecen Canchas, Usuarios ni Ingresos. Sí aparecen Clientes, Reservas, Pagos y Disponibilidad | Coincide | P |
| RG-05 | RF16 | Cerrar sesión | Vuelve al login; la conexión individual de SQL Server se cierra | Coincide | P |

### 6.2 Canchas y usuarios (administrador)

| ID | RF/RN | Pasos | Resultado esperado | Resultado | Estado |
|---|---|---|---|---|---|
| RG-06 | RF13 | Administración → Canchas. Registrar `Cancha Norte` | Fila activa en la grilla | Coincide | P |
| RG-07 | RF13 | Registrar el mismo nombre | Mensaje de nombre duplicado; no se inserta otra fila | Coincide | P |
| RG-08 | RN10 | Desactivar la cancha. Intentar usarla en una reserva nueva | No aparece entre canchas activas; reservas previas (si las hay) no se cancelan | Coincide | P |
| RG-09 | RF13 | Reactivar escenario: registrar `Cancha Sur` y dejarla activa para el resto de la suite | Queda activa | Coincide | P |
| RG-10 | RF14 | Administración → Usuarios. Registrar empleado con login que empiece por letra, clave ≥ 8 | Cuenta activa; puede iniciar sesión | Coincide | P |
| RG-11 | RN11 | Intentar login de empleado en minúsculas/mayúsculas según A12 | El login se compara de forma consistente con lo persistido | Coincide | P |

### 6.3 Clientes

| ID | RF/RN | Pasos | Resultado esperado | Resultado | Estado |
|---|---|---|---|---|---|
| RG-12 | RF01, RN07 | Registrar cliente: nombre `Ana Pérez`, cédula `0102030405`, teléfono `0987654321`, correo `ana@uteq.edu.ec` | Registro correcto | Coincide | P |
| RG-13 | RN07 | Registrar cédula con menos de 10 dígitos | Validación; no persiste | Coincide | P |
| RG-14 | RF01 | Repetir tipo + número de documento | “Ya existe un cliente…” | Coincide | P |
| RG-15 | RF02 | Buscar por fragmento de nombre `Ana` | Aparece el cliente | Coincide | P |
| RG-16 | RF03 | Modificar teléfono y correo | Grilla actualizada | Coincide | P |
| RG-17 | RF04 | Empleado: el botón Eliminar no se muestra. Administrador: eliminar cliente **sin** reservas ni pagos | Se elimina | Coincide | P |

*Nota:* RG-17 se ejecuta sobre un cliente de prueba distinto al usado en reservas (RG-18), para no romper la cadena de regresión cruzada.

### 6.4 Reservas, disponibilidad, pagos e ingresos (cadena cruzada)

Esta secuencia es el núcleo de la regresión: un fallo aquí indica que la integración A13 quedó inconsistente.

| ID | RF/RN | Pasos | Resultado esperado | Resultado | Estado |
|---|---|---|---|---|---|
| RG-18 | RF05, RN06 | Gestión → Reservas. Cliente Ana, cancha activa, fecha de hoy, franja libre. Registrar | Reserva `activa`; `id_usuario` de sesión (RN13) | Coincide | P |
| RG-19 | RN06 | Intentar fecha anterior a hoy en el selector de alta | El control no permite fechas pasadas; el motor rechazaría 50006 | Coincide | P |
| RG-20 | RF11, RN05, RN08 | Consultas → Disponibilidad. Misma cancha y fecha | 16 franjas 06:00–22:00; la reservada figura `ocupada` | Coincide | P |
| RG-21 | RN01 | Reservar de nuevo la misma franja | Rechazo: franja ocupada | Coincide | P |
| RG-22 | RF06 | Buscar reservas del día | Aparece la reserva de RG-18 | Coincide | P |
| RG-23 | RF07 | Cambiar a otra franja libre | Horario actualizado; la franja anterior queda libre | Coincide | P |
| RG-24 | RF09, RN04 | Gestión → Pagos. Seleccionar la reserva. Monto `25,00`, estado `Pagado`. Registrar | Un solo pago; el botón de registro se deshabilita | Coincide | P |
| RG-25 | RN04 | Intentar un segundo pago sobre la misma reserva | No se permite | Coincide | P |
| RG-26 | RF10 | Filtrar por id de reserva | Estado `pagado`, monto 25,00 | Coincide | P |
| RG-27 | RF12, RN09 | Consultas → Ingresos (solo admin). Rango que incluya la fecha de la **franja** | Total 25,00; detalle con cliente, cancha y monto. Un pago `pendiente` no suma | Coincide | P |
| RG-28 | RF08, RN03 | Cancelar una reserva **activa distinta**, sin pago | Estado `cancelada`; la franja vuelve a `libre` | Coincide | P |
| RG-29 | RN02 | Administrador: eliminar el cliente de RG-18 (tiene reserva activa o historial según motor) | Si hay reserva activa o pago pendiente, el motor rechaza (50003/50004) | Coincide | P |
| RG-30 | RF08 | Cancelar una reserva ya cancelada | Mensaje de ya cancelada | Coincide | P |

### 6.5 Infraestructura observada durante la suite

| ID | Tema | Observación | Impacto en regresión |
|---|---|---|---|
| RG-31 | LocalDB y `Encrypt=True` | La instancia no admite cifrado de canal | `App.config` usa `Encrypt=False`. No es defecto de negocio; queda documentado para A17 |
| RG-32 | `sp_GenerarHorariosDia` | Sin `GRANT` al rol, Disponibilidad falla al generar las 16 franjas | Cubierto por `docs/A11_Complemento_GrantGenerarHorarios.sql`. Con el GRANT, RG-20 pasa |

## 7. Matriz de cobertura de requisitos

| Requisito | Casos A14 (automatizado) | Casos A15 (sistema) | Cubierto |
|---|---|---|---|
| RF01 Registrar cliente | `RegistrarCliente_*` | RG-12, RG-14 | Sí |
| RF02 Consultar clientes | `ConsultarClientes_*` | RG-15 | Sí |
| RF03 Modificar cliente | `ModificarCliente_*` | RG-16 | Sí |
| RF04 Eliminar cliente | `EliminarCliente_*` | RG-17, RG-29 | Sí |
| RF05 Crear reserva | `CrearReserva_*` | RG-18, RG-21 | Sí |
| RF06 Consultar reservas | `ConsultarReservas_*` | RG-22 | Sí |
| RF07 Modificar horario | `ModificarHorario_*` | RG-23 | Sí |
| RF08 Cancelar reserva | `CancelarReserva_*` | RG-28, RG-30 | Sí |
| RF09 Registrar pago | `RegistrarPago_*` | RG-24, RG-25 | Sí |
| RF10 Consultar estado de pago | `ConsultarEstadoPago_*` | RG-26 | Sí |
| RF11 Disponibilidad | `ConsultarDisponibilidad_*` | RG-20 | Sí |
| RF12 Ingresos | `ConsultarIngresos_*` | RG-27 | Sí |
| RF13 Canchas | `CanchaServiceTests` | RG-06–RG-09 | Sí |
| RF14 Registrar usuario | `RegistrarUsuario_*`, `RegistrarAdministradorInicial_*` | RG-10 | Sí |
| RF15 Desactivar usuario | `DesactivarUsuario_*` | (cubierto en A14: no auto-baja ni último admin) | Sí |
| RF16 Inicio de sesión | `ValidarCredenciales_*`, `CerrarSesion_*` | RG-01, RG-02, RG-05 | Sí |
| RN01 Unicidad de franja activa | `CrearReserva_FranjaOcupada_*` | RG-21 | Sí |
| RN02 Baja de cliente | `EliminarCliente_ConReservas/Pagos_*` | RG-29 | Sí |
| RN03 Cancelación lógica | `CancelarReserva_*` | RG-28 | Sí |
| RN04 Un pago / monto > 0 | `RegistrarPago_MontoCero/YaRegistrado_*` | RG-24, RG-25 | Sí |
| RN05 Franjas de 60 min | `ConsultarDisponibilidad_DevuelveFranjas` | RG-20 | Sí |
| RN06 Sin fechas pasadas | `CrearReserva_FechaAnterior_*` | RG-19 | Sí |
| RN07 Documento / cédula | `RegistrarCliente_CedulaInvalida/Duplicado_*` | RG-13, RG-14 | Sí |
| RN08 Franja ocupada | `CrearReserva_FranjaOcupada_*` | RG-20, RG-21 | Sí |
| RN09 Ingresos pagados por fecha de franja | `ConsultarIngresos_*` | RG-27 | Sí |
| RN10 Cancha inactiva | `DesactivarCancha_*`, `CanchaActiva_*` | RG-08 | Sí |
| RN11 Roles | `RegistrarUsuario_*` | RG-03, RG-04, RG-10 | Sí |
| RN12 Usuario inactivo | `ValidarCredenciales_UsuarioInactivo_*` | RG-02 (credenciales) / A14 | Sí |
| RN13 Usuario que registra la reserva | `CrearReserva_Empleado_Persiste...IdUsuario` | RG-18 | Sí |

**Cobertura funcional de A1: 16/16 RF y 13/13 RN.**

## 8. Defectos

No se abrieron defectos bloqueantes en esta corrida. Los dos hallazgos de entorno (RG-31, RG-32) están mitigados en configuración y en complemento SQL; no invalidan el veredicto de regresión del código A13.

| ID | Severidad | Descripción | Estado |
|---|---|---|---|
| — | — | Ningún defecto funcional abierto | — |

## 9. Criterio de salida y veredicto

Se aprueba A15 si y solo si:

1. La suite A14 re-ejecutada termina 70/70.
2. Todos los casos RG-01 a RG-30 resultan P (o NA justificado).
3. La matriz de la sección 7 no deja RF/RN sin evidencia.

**Veredicto: APROBADO.** El sistema integrado no presenta regresiones respecto de A1.

## 10. Cómo repetir este artefacto

1. Compilar `src/SistemaCanchasSinteticas.sln` (0 advertencias).
2. Ejecutar `dotnet test src/SistemaCanchasSinteticas.sln --nologo`.
3. Iniciar `SistemaCanchas.Presentacion` (F5) y recorrer RG-01 a RG-30.
4. Anexar captura de la salida de `dotnet test` y de Ingresos = 25,00 si se replica la cadena RG-18–RG-27.
5. Si se añade código después de esta fecha, **A15 debe volver a ejecutarse** antes de declarar estable el corte.

## 11. Control de cambios del artefacto

| Versión | Fecha | Descripción |
|---|---|---|
| 1.0 | 24 ago 2026 | Primera ejecución de regresión sobre el sistema completo (A13 + A14) |

---

*Fin del artefacto A15. El siguiente artefacto del ciclo es A16 (Revisiones Técnicas Formales).*
