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
| Fecha | 25 de agosto de 2026 |
| Sistema bajo prueba | Solución `src/SistemaCanchasSinteticas.sln` (.NET Framework 4.8) |
| Referencias | A1, A11, A13, A14 |

---

## 1. Para qué sirve esta corrida

Comprobar que, con todos los módulos juntos, lo que ya funcionaba **sigue funcionando**. No se busca funcionalidad nueva: se recorre login, canchas, clientes, reservas, pagos e ingresos y se mira si algo se rompió.

## 2. Qué se cubrió

- Las pruebas automáticas de A14 (`dotnet test`).
- Una pasada manual por las ventanas WinForms, con administrador y con empleado.
- Las reglas que cruzan módulos: una franja no se reserva dos veces, un pago por reserva, ingresos solo con estado pagado, menús según el rol.

No entra aquí la instalación en un PC limpio (eso es A17) ni el instalador (A18).

## 3. Cómo se hizo

Primero se corrió la suite automática. Después se abrió el programa y se recorrieron los casos RG de la sección 6.

| Nivel | Cómo | Pasa si |
|---|---|---|
| Automático | `dotnet test src/SistemaCanchasSinteticas.sln` | Cero fallos |
| Manual | Casos RG-01 en adelante | Lo que se ve coincide con lo esperado |

## 4. Entorno

| Elemento | Valor |
|---|---|
| Sistema operativo | Windows 10 |
| SDK | Visual Studio 2022 / `dotnet test` |
| Aplicación | .NET Framework 4.8 |
| Base de datos | SQL Server LocalDB `(localdb)\MSSQLLocalDB`, base `ReservaCanchasDB` |
| Script | Un solo archivo: `docs/A11_Script_DDL.sql` |

En `App.config` el canal va con `Encrypt=False` porque LocalDB no admite cifrado de conexión.

## 5. Resultado automático (A14)

```
dotnet test src/SistemaCanchasSinteticas.sln --nologo
```

| Métrica | Valor |
|---|---|
| Superados | 83 |
| Con error | 0 |
| Omitidos | 0 |
| Veredicto | Aprobado |

Los archivos de prueba y qué cubre cada uno están listados en `test/README.md`. El código vive en `src/SistemaCanchas.Tests/`.

| Clase | Casos | Temas |
|---|---|---|
| `CifradorAesTests` | 6 | AES de la clave de motor |
| `UsuarioServiceTests` | 24 | Login, altas, activar/desactivar, cambio de clave y nombre |
| `CanchaServiceTests` | 13 | Canchas, horario de apertura/cierre, activar/desactivar |
| `ClienteServiceTests` | 16 | Clientes, cédula, duplicados, baja |
| `ReservaServiceTests` | 13 | Reservas (una o varias franjas), horario, cancelar, disponibilidad |
| `PagoServiceTests` | 7 | Pagos y filtros |
| `IngresoServiceTests` | 4 | Ingresos (solo admin) |
| **Total** | **83** | |

## 6. Pasada manual

Antes de empezar:

1. La base se creó con `docs/A11_Script_DDL.sql` **una sola vez**. No volver a ejecutarlo: borra tablas.
2. Hay un administrador (el de la primera instalación).
3. Hay al menos una cancha activa, o se crea en RG-06.

**P** = pasó. **F** = falló.

### Acceso

| ID | Qué se hace | Qué se espera | Estado |
|---|---|---|---|
| RG-01 | Entrar con admin y clave correctos | Se abre la ventana principal; abajo se ve usuario y rol | P |
| RG-02 | Clave incorrecta | “Usuario o clave incorrectos.” La clave se limpia y el aviso se queda visible | P |
| RG-03 | Menús como administrador | Gestión, Consultas (con Ingresos) y Administración | P |
| RG-04 | Menús como empleado | No aparecen Canchas, Usuarios ni Ingresos | P |
| RG-05 | Cerrar sesión | Vuelve al login | P |

### Canchas y usuarios (administrador)

| ID | Qué se hace | Qué se espera | Estado |
|---|---|---|---|
| RG-06 | Canchas: panel **Nueva cancha**, nombre y horario, Registrar | Aparece en la lista, activa | P |
| RG-07 | El mismo nombre otra vez | Aviso de duplicado | P |
| RG-08 | Seleccionar la fila y Desactivar | Ya no sale entre canchas activas al reservar | P |
| RG-09 | Registrar otra cancha y dejarla activa | Queda para el resto de la prueba | P |
| RG-10 | Usuarios: panel **Nuevo usuario**, empleado con clave ≥ 8 | La cuenta entra al sistema | P |
| RG-11 | Seleccionar un usuario y cambiar nombre o clave | El panel de la derecha se habilita al hacer clic; el de la izquierda no se llena | P |

### Clientes

| ID | Qué se hace | Qué se espera | Estado |
|---|---|---|---|
| RG-12 | **Nuevo cliente**: Ana Pérez, cédula válida, teléfono y correo | “Cliente registrado.” | P |
| RG-13 | Cédula con menos de 10 dígitos | No guarda | P |
| RG-14 | Mismo tipo y número de documento | Aviso de duplicado | P |
| RG-15 | Buscar por “Ana” | Aparece el cliente | P |
| RG-16 | Clic en la fila, cambiar teléfono en **Editar cliente**, Guardar | La grilla se actualiza. El panel de alta no se mezcla | P |
| RG-17 | Empleado: no hay botón Eliminar. Admin: borrar un cliente **sin** reservas | Se elimina | P |

RG-17 se hace con un cliente distinto al de las reservas, para no cortar la cadena.

### Reservas, pagos e ingresos

| ID | Qué se hace | Qué se espera | Estado |
|---|---|---|---|
| RG-18 | **Nueva reserva**: cliente, cancha, hoy, una o más franjas libres. Registrar | Reserva activa. Quien registró es el usuario de la sesión | P |
| RG-19 | Fecha anterior a hoy en el alta | El calendario no deja elegir días pasados | P |
| RG-20 | Disponibilidad, misma cancha y fecha | Franjas según el horario de esa cancha; la reservada sale ocupada | P |
| RG-21 | Volver a tomar la misma franja | Rechazo: ocupada | P |
| RG-22 | Buscar reservas del día | Sale la de RG-18 | P |
| RG-23 | Clic en la reserva, **Editar reserva**, otra franja, Cambiar horario | Cambia el horario; la franja anterior queda libre | P |
| RG-24 | Pagos: clic en la reserva activa, monto 25,00, Pagado | Se registra. El panel está apagado hasta elegir una fila | P |
| RG-25 | Intentar otro pago sobre la misma | El panel indica que ya tiene pago; no deja registrar otra vez | P |
| RG-26 | Filtrar pagos | Se ve pagado y el monto | P |
| RG-27 | Ingresos (admin), rango que cubra la fecha de la franja | Total 25,00. Un pago pendiente no suma | P |
| RG-28 | Cancelar otra reserva activa, sin pago | Queda cancelada; la hora se libera | P |
| RG-29 | Admin: borrar el cliente de RG-18 | Si tiene reserva activa o pago pendiente, el sistema no deja | P |
| RG-30 | Cancelar una ya cancelada | Aviso de ya cancelada | P |

### Grillas

| ID | Qué se hace | Qué se espera | Estado |
|---|---|---|---|
| RG-31 | Abrir cualquier lista (clientes, reservas, etc.) | Ninguna fila viene marcada. No se pueden arrastrar bordes de filas ni columnas | P |
| RG-32 | LocalDB con `Encrypt=True` | El login no conecta. En el laboratorio el `.config` lleva `Encrypt=False` | P |

## 7. Cobertura de requisitos

Quedan cubiertos RF01–RF16 y RN01–RN13: cada uno aparece en la suite automática, en la pasada manual, o en ambas. El detalle de nombres de método está en `src/SistemaCanchas.Tests/` y en `test/README.md`.

## 8. Defectos

En esta corrida no quedó ningún defecto de negocio abierto. Lo de LocalDB y el cifrado de canal (RG-32) es de entorno; se documenta en A17.

## 9. Cómo repetirla

1. Compilar la solución (0 advertencias).
2. `dotnet test src/SistemaCanchasSinteticas.sln --nologo`
3. Abrir el programa y recorrer RG-01 a RG-32.
4. Si después se cambia código, hay que volver a correr esta lista.

## 10. Cambios del documento

| Versión | Fecha | Qué cambió |
|---|---|---|
| 1.0 | 24 ago 2026 | Primera corrida sobre el sistema completo |
| 1.1 | 25 ago 2026 | 83 pruebas; paneles de alta/edición; un solo script A11; horarios por cancha y varias franjas |

---

*Sigue A16 (Revisiones Técnicas Formales).*
