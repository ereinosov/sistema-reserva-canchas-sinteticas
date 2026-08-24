# UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
## Facultad de Ciencias de la Computación
### Carrera de Ingeniería en Software — Proceso de Software “A”

---

# Artefacto A19: Manual de Usuario

| Campo | Detalle |
|---|---|
| Proyecto | Sistema de Reserva de Canchas Sintéticas |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph Alexander · Herrera Barco Humberto Aldair · Reinoso Vélez Eduardo David · Silva Triviño John Jairo |
| Período | Mayo – Agosto 2026 |
| Fecha | 24 de agosto de 2026 |
| Sistema descrito | Aplicación de escritorio *Sistema de Reserva de Canchas Sintéticas* (versión de entrega A13 / paquete A18 1.0.0) |
| Destinatarios | Empleado de mostrador y administrador del complejo |
| Norma de referencia | ISO/IEC/IEEE 26514:2022 (documentación de usuario); Pressman & Maxim (2019), manuales de operación |

Este documento explica **cómo usar el programa ya instalado**. La instalación de la base de datos y del paquete está en A17 y A18; aquí no se piden conocimientos de SQL ni de Visual Studio.

---

## 1. Para qué sirve el sistema

El programa centraliza el trabajo diario de una cancha sintética de fútbol:

- registrar y buscar clientes;
- ver horarios libres u ocupados;
- crear, cambiar o cancelar reservas;
- registrar el pago de una reserva;
- (solo administrador) administrar canchas y cuentas, y consultar ingresos.

Horario de operación que usa el sistema: **16 franjas de 60 minutos**, de **06:00 a 22:00**.

## 2. Quién puede hacer qué

Hay dos perfiles. El menú se ajusta solo: lo que no corresponde a su perfil **no aparece**.

| Tarea | Empleado | Administrador |
|---|---|---|
| Iniciar sesión | Sí | Sí |
| Clientes: registrar, buscar, modificar | Sí | Sí |
| Clientes: eliminar | No (el botón no se muestra) | Sí, con condiciones |
| Reservas y disponibilidad | Sí | Sí |
| Pagos | Sí | Sí |
| Canchas | No | Sí |
| Usuarios | No | Sí |
| Ingresos | No | Sí |

En la barra inferior de la ventana principal verá `Usuario:` y `Rol:`.

## 3. Arranque e inicio de sesión

1. Abra **Sistema de Reserva de Canchas Sintéticas** (acceso directo o menú Inicio).
2. En **Inicio de sesión** complete:
   - **Usuario** (máximo 30 caracteres)
   - **Clave** (los caracteres no se muestran)
3. Pulse **Ingresar** (Enter también sirve). **Salir** cierra el programa.

El botón **Ingresar** permanece inactivo hasta que ambos campos tengan texto.

**Si algo falla**

| Mensaje o situación | Qué hacer |
|---|---|
| *Ingrese el usuario.* / *Ingrese la clave.* | Complete el campo marcado. |
| *Usuario o clave incorrectos.* | Vuelva a escribir la clave. El programa no indica si el usuario existe. |
| *El usuario se encuentra inactivo y no puede iniciar sesión.* | Pida al administrador que revise la cuenta. |
| Aviso de conexión / servidor | Compruebe que el equipo tenga red hacia SQL Server / LocalDB y que la base esté creada (quien instaló el sistema). |

Al cerrar la ventana principal (o **Archivo → Cerrar sesión**) vuelve a la pantalla de ingreso. **Archivo → Salir** cierra toda la aplicación.

### 3.1 Primera vez en un equipo nuevo (solo si aún no hay cuentas)

En el login, el enlace **Primera configuración (administrador inicial)** abre **Configuración inicial**:

| Campo | Qué escribir |
|---|---|
| Nombre | Nombre visible de la persona |
| Usuario de acceso | Entre 3 y 30 caracteres; empieza por letra; solo letras, dígitos o `_` |
| Clave (mínimo 8 caracteres) | Clave con la que entrará al programa |
| Confirmar clave | Debe coincidir |

**Crear** da de alta el primer administrador. Luego el login muestra *Administrador creado. Ingrese la clave para iniciar sesión.* Si ya existen usuarios, el sistema rechaza repetir este paso.

Hace falta una cuenta de Windows con permiso de administrador en SQL Server (la usó quien ejecutó el script de la base). El empleado de mostrador **no** usa esta pantalla en el día a día.

## 4. Ventana principal

Tras un ingreso correcto aparece la ventana **Sistema de Reserva de Canchas Sintéticas**.

| Menú | Opciones |
|---|---|
| **Archivo** | Cerrar sesión · Salir |
| **Gestión** | Clientes · Reservas · Pagos |
| **Consultas** | Disponibilidad · Ingresos (esta última solo administrador) |
| **Administración** | Canchas · Usuarios (todo el menú solo administrador) |

Cada opción abre una ventana propia. Ciérrela cuando termine; la principal sigue abierta.

---

## 5. Clientes *(Gestión → Clientes)*

Ventana **Clientes**. Arriba, **Búsqueda**; abajo, **Datos del cliente**.

**Buscar:** filtre por **Nombre** y/o **Documento** y pulse **Buscar**. **Ver todos** limpia los filtros.

**Registrar** un cliente nuevo:

1. Complete **Nombre**, **Tipo de documento**, **Número**, **Teléfono** y **Correo**.
2. Tipos: **Cédula**, **Pasaporte** o **RUC**.
3. Pulse **Registrar**. Debe ver *Cliente registrado.*

**Modificar:** seleccione la fila, edite los datos y pulse **Modificar**. Mensaje: *Cliente actualizado.*

**Eliminar** (solo administrador): el botón **Eliminar** no existe para el empleado. El administrador selecciona la fila, confirma *¿Eliminar el cliente seleccionado? Solo se permite si no tiene reservas activas ni pagos pendientes.*

| Lista (columnas) | Nombre · Tipo · Documento · Teléfono · Correo |
|---|---|

**Reglas que verá en pantalla**

- La cédula debe ser **exactamente 10 dígitos**.
- Teléfono: 7 a 15 dígitos; puede empezar con `+`.
- Correo con formato válido (debe incluir `@` y un dominio).
- No puede haber dos clientes con el mismo tipo y número de documento.
- No se elimina un cliente con reservas activas o con pagos pendientes.

## 6. Disponibilidad *(Consultas → Disponibilidad)*

Ventana **Disponibilidad**. Elija **Cancha** y **Fecha**, pulse **Consultar**.

Aparecen las 16 franjas del día, con **Inicio**, **Fin** y **Estado** (`libre` u `ocupada`).

- Solo se listan canchas **activas**.
- Si no hay canchas activas, el sistema pide *Seleccione una cancha activa.*

Use esta consulta **antes** de reservar, o vaya directo a Reservas: allí el combo **Franja libre** ya oculta las ocupadas.

## 7. Reservas *(Gestión → Reservas)*

Ventana **Reservas**.

**Búsqueda:** **Fecha**, **Cliente**, **Cancha**, **Estado** (Todos / Activa / Cancelada). **Buscar** aplica filtros; **Ver todas** los quita.

**Datos de la reserva:** **Cliente**, **Cancha**, **Fecha**, **Franja libre**.

El calendario de fecha **no permite días anteriores a hoy**.

**Registrar**

1. Cliente y cancha existentes (si falta el cliente, créelo primero).
2. Fecha de hoy o futura.
3. Una **Franja libre** (formato `06:00 - 07:00`, etc.).
4. **Registrar** → *Reserva registrada.*

La lista muestra quién **Registró** la reserva (el usuario de la sesión).

**Cambiar horario:** seleccione una reserva **activa**, elija otra franja libre y pulse **Cambiar horario**. Mensaje: *Horario de la reserva actualizado.* Las canceladas no se reprograman.

**Cancelar reserva:** seleccione una activa y confirme *¿Cancelar la reserva seleccionada? La franja quedará libre.* La reserva queda en estado **cancelada** (no se borra el historial).

**Mensajes frecuentes**

- *La franja horaria seleccionada ya se encuentra ocupada.* Otra persona la tomó; vuelva a consultar.
- *No se pueden registrar ni reprogramar reservas con fecha anterior a la actual.*
- *La cancha de la franja seleccionada no está activa.*

## 8. Pagos *(Gestión → Pagos)*

Ventana **Pagos**. Lista reservas (con o sin pago). **Id reserva** + **Buscar**, o **Ver todas**.

Seleccione una reserva **sin monto** en la columna **Monto**. El recuadro **Registrar pago** muestra *Reserva N — nombre del cliente*.

| Campo | Uso |
|---|---|
| Monto | Número mayor que cero (hasta 9.999.999,99) |
| Fecha | Fecha del pago (por defecto hoy) |
| Estado | **Pagado** (predeterminado) o **Pendiente** |

**Registrar pago** → *Pago registrado.* El botón se deshabilita si esa reserva ya tiene pago: *La reserva seleccionada ya tiene un pago registrado.* Solo hay **un pago por reserva**.

Los ingresos del administrador **solo suman los pagos en estado Pagado**, según la **fecha de la franja** reservada (no la fecha en que se pulsó el botón, si difieren).

## 9. Canchas *(Administración → Canchas)* — solo administrador

Ventana **Canchas**. **Nombre** (máximo 60 caracteres).

- **Registrar** → *Cancha registrada.* El nombre no puede repetirse.
- **Modificar** el nombre de la fila seleccionada → *Cancha actualizada.*
- **Desactivar:** confirma *¿Desactivar la cancha seleccionada? No recibirá reservas nuevas. Las reservas ya registradas no se modifican.* Estado **inactiva**. No se usa para reservas nuevas; el historial se conserva.
- **Actualizar lista** recarga la tabla (Id, Nombre, Estado).

## 10. Usuarios *(Administración → Usuarios)* — solo administrador

Ventana **Usuarios**. Recuadro **Nuevo usuario**.

| Campo | Uso |
|---|---|
| Nombre | Nombre visible |
| Usuario de acceso | 3–30 caracteres; empieza por letra; letras, dígitos o `_` |
| Clave | Mínimo 8 caracteres |
| Rol | **Empleado** (predeterminado) o **Administrador** |

**Registrar** → *Usuario registrado. Ya puede iniciar sesión con esa cuenta.*

**Desactivar:** seleccione la fila y confirme. Esa persona ya no entra; el historial permanece. No puede desactivarse a sí mismo ni al **único** administrador activo.

**Actualizar lista** recarga Id, Nombre, Usuario, Rol y Estado.

## 11. Ingresos *(Consultas → Ingresos)* — solo administrador

Ventana **Ingresos**. **Rango de fechas de franja:** **Desde** y **Hasta** (al abrir: primer día del mes actual hasta hoy). **Consultar**.

El total (*Total ingresos: …*) y el detalle incluyen solo pagos **pagado** cuya **fecha de franja** cae en el rango. Columnas: Pago, Reserva, Cliente, Cancha, Fecha franja, Inicio, Monto, Fecha pago.

Si *Desde* es posterior a *Hasta*, el sistema lo rechaza.

## 12. Flujo de un día típico (empleado)

1. Iniciar sesión.
2. **Disponibilidad** (opcional) o ir a **Reservas**.
3. Si el cliente es nuevo: **Clientes → Registrar**.
4. **Reservas → Registrar** (cliente, cancha, fecha, franja libre).
5. Cuando pague: **Pagos**, seleccionar la reserva, monto y **Registrar pago** (estado **Pagado** si el dinero ya ingresó).
6. **Archivo → Cerrar sesión** al terminar el turno.

El administrador, además, da de alta canchas y cuentas al inicio de temporada y revisa **Ingresos** al cierre del día o del mes.

## 13. Preguntas frecuentes

**¿Por qué no veo Canchas, Usuarios o Ingresos?**  
Su rol es empleado. Esas opciones solo existen para administrador.

**¿Puedo reservar ayer “porque el partido ya se jugó”?**  
No. El calendario no deja fechas pasadas.

**¿Dos reservas a la misma hora en la misma cancha?**  
No. Si lo intenta, verá que la franja está ocupada.

**¿Borro una reserva cancelada?**  
No. Queda cancelada y la hora se libera para otra reserva.

**Olvidé la clave.**  
El programa no recupera claves. El administrador crea otra cuenta o, en un caso extremo, quien mantiene el servidor debe intervenir. No hay “olvidé mi contraseña” en la pantalla de login.

**Desinstalé el programa y nadie puede entrar.**  
La clave de cifrado de la estación (`aes.key`) no se borra a propósito. Si alguien la eliminó a mano, las cuentas existentes no podrán autenticarse. Avise a quien instaló el sistema (A18).

## 14. Cómo repetir / actualizar este manual

Si cambia un texto de botón, un menú o un mensaje de validación en A13, hay que actualizar **este** archivo en la misma entrega. El manual no cita códigos internos de requisitos: describe la pantalla tal como la ve el usuario.

## 15. Control de cambios del artefacto

| Versión | Fecha | Descripción |
|---|---|---|
| 1.0 | 24 ago 2026 | Manual de operación alineado a menús, campos y mensajes de A13 |

---

*Fin del artefacto A19. Con este documento queda cerrado el ciclo de artefactos A1–A19 del proyecto.*
