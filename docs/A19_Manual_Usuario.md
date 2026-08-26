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
| Fecha | 25 de agosto de 2026 |
| Para quién | Empleado de mostrador y administrador del complejo |

Este texto explica **cómo usar el programa ya instalado**. La base de datos y el instalador están en A17 y A18. Aquí no hace falta saber SQL.

---

## 1. Para qué sirve

Sirve para el día a día de una cancha sintética: clientes, horarios libres, reservas, cobros y, si es administrador, canchas, cuentas e ingresos.

Cada cancha tiene su propio horario de apertura y cierre. El sistema arma franjas de **60 minutos** dentro de ese horario.

## 2. Quién puede hacer qué

El menú se recorta solo. Lo que no le toca **no aparece**.

| Tarea | Empleado | Administrador |
|---|---|---|
| Iniciar sesión | Sí | Sí |
| Clientes (alta, búsqueda, edición) | Sí | Sí |
| Eliminar cliente | No | Sí, si no tiene reservas activas ni pagos pendientes |
| Reservas, disponibilidad, pagos | Sí | Sí |
| Canchas, usuarios, ingresos | No | Sí |

Abajo a la izquierda de la ventana principal se ve `Usuario:` y `Rol:`.

## 3. Entrar

1. Abra **Sistema de Reserva de Canchas Sintéticas**.
2. Escriba **Usuario** y **Clave**.
3. **Ingresar** (también Enter). **Salir** cierra el programa.

El botón Ingresar se activa cuando hay texto en los dos campos.

| Si sale esto | Qué hacer |
|---|---|
| *Ingrese el usuario.* / *Ingrese la clave.* | Complete el campo. |
| *Usuario o clave incorrectos.* | Vuelva a escribir la clave. El aviso no dice si el usuario existe. |
| *El usuario se encuentra inactivo…* | Pida al administrador que active la cuenta. |
| Error de servidor | Quien instaló el sistema debe revisar LocalDB / SQL Server. |

**Archivo → Cerrar sesión** vuelve al login. **Archivo → Salir** cierra todo.

### Primera vez en un equipo (sin cuentas)

En el login, **Primera configuración (administrador inicial)**. Nombre, usuario (empieza por letra), clave de 8 caracteres o más, confirmar. **Crear**. Después entre con esa clave. Si ya hay usuarios, este paso no se puede repetir.

Hace falta una cuenta de Windows con permiso en SQL Server. El empleado de mostrador no usa esta pantalla.

## 4. Ventana principal

| Menú | Opciones |
|---|---|
| **Archivo** | Cerrar sesión · Salir |
| **Gestión** | Clientes · Reservas · Pagos |
| **Consultas** | Disponibilidad · Ingresos (solo admin) |
| **Administración** | Canchas · Usuarios (solo admin) |

Cada opción abre su ventana. Ciérrela cuando termine.

En las listas **no viene ninguna fila marcada**. Para editar hay que hacer clic. Tampoco se pueden estirar columnas ni filas a mano.

Casi todas las ventanas de gestión tienen **dos recuadros abajo**:

- A la **izquierda**: alta (datos nuevos). Elegir una fila **no** copia esos datos.
- A la **derecha**: lo seleccionado. Empieza apagado hasta que haga clic en la lista.

---

## 5. Clientes *(Gestión → Clientes)*

Arriba: búsqueda por nombre y/o documento. **Buscar** / **Ver todos**.

**Nuevo cliente** (izquierda): nombre, tipo (cédula, pasaporte o RUC), número, teléfono, correo. **Registrar**.

**Editar cliente seleccionado** (derecha): se llena al hacer clic en una fila. **Modificar**. El administrador ve **Eliminar** (pide confirmación).

- Cédula: exactamente 10 dígitos y dígito verificador válido.
- Teléfono: 7 a 15 dígitos; puede ir con `+`.
- No se repite tipo+número, ni teléfono, ni correo.

## 6. Disponibilidad *(Consultas → Disponibilidad)*

Elija cancha y fecha, **Consultar**. Verá inicio, fin y estado (`libre` / `ocupada`). Solo canchas activas. El número de franjas depende del horario de esa cancha.

## 7. Reservas *(Gestión → Reservas)*

Filtros arriba: fecha, cliente, cancha, estado.

**Nueva reserva** (izquierda): cliente, cancha, fecha (no días pasados) y **una o varias** franjas libres. **Registrar**.

**Editar reserva seleccionada** (derecha): al hacer clic en una activa puede **Cambiar horario** (una franja) o **Cancelar**. La cancelada no se borra; la hora queda libre.

Si otra persona tomó la misma hora: *La franja horaria seleccionada ya se encuentra ocupada.*

## 8. Pagos *(Gestión → Pagos)*

La lista muestra reservas con o sin pago. El recuadro de abajo **está apagado** hasta que elija una fila.

- Reserva activa sin pago: complete monto, fecha y estado (**Pagado** o **Pendiente**) y **Registrar pago**.
- Si ya tiene pago, el recuadro lo dice y no deja cargar otro.
- Si la reserva no está activa, tampoco deja pagar.

Solo hay **un pago por reserva**. Los ingresos del admin suman únicamente los **pagados**, según la **fecha de la franja**, no la del botón.

## 9. Canchas *(Administración → Canchas)* — solo admin

**Nueva cancha**: nombre, hora de abre y de cierra. **Registrar**. El nombre no se duplica. El cierre debe ser después de la apertura.

**Editar cancha seleccionada**: nombre, horario, **Guardar cambios**, **Desactivar** o **Activar**. Desactivar impide reservas nuevas; las que ya estaban no se tocan.

## 10. Usuarios *(Administración → Usuarios)* — solo admin

**Nuevo usuario**: nombre, usuario de acceso, clave (≥ 8), rol empleado o administrador. **Registrar**. Ese recuadro no se llena al elegir una fila.

**Editar usuario seleccionado**: cambia el nombre, la clave (escribirla dos veces) o activa/desactiva. No puede desactivarse a sí mismo ni al único administrador activo.

## 11. Ingresos *(Consultas → Ingresos)* — solo admin

**Desde** y **Hasta** (por defecto: mes actual hasta hoy). **Consultar**. Total y detalle de pagos **pagado** cuya franja cae en el rango. Si *Desde* es posterior a *Hasta*, lo rechaza.

## 12. Un turno típico (empleado)

1. Entrar.
2. Si el cliente es nuevo → Clientes, recuadro izquierdo.
3. Reservas → recuadro izquierdo: cliente, cancha, fecha, franjas, Registrar.
4. Cuando pague → Pagos, clic en la reserva, monto, Registrar pago.
5. Cerrar sesión al terminar.

El administrador da de alta canchas y cuentas cuando hace falta y mira Ingresos al cierre.

## 13. Preguntas frecuentes

**No veo Canchas, Usuarios o Ingresos.**  
Es empleado. Esas opciones son de administrador.

**¿Puedo anotar un partido de ayer?**  
No. El calendario no deja fechas pasadas.

**¿Dos reservas a la misma hora en la misma cancha?**  
No.

**Al hacer clic en la lista se me borró lo que iba a registrar.**  
Ya no: el alta (izquierda) y la edición (derecha) están separados.

**Olvidé la clave.**  
El programa no la recupera. El administrador crea otra cuenta o cambia la clave desde Usuarios.

**Desinstalé el programa y nadie entra.**  
La clave de cifrado de esa PC (`aes.key`) no se borra a propósito. Si alguien la eliminó a mano, hay que avisar a quien instaló el sistema.

## 14. Cambios del documento

| Versión | Fecha | Qué cambió |
|---|---|---|
| 1.0 | 24 ago 2026 | Primera versión alineada a menús y mensajes |
| 1.1 | 25 ago 2026 | Paneles de alta y edición; varias franjas; horario por cancha; pagos al seleccionar; grillas fijas |

---

*Con A19 se cierra el ciclo de artefactos A1–A19.*
