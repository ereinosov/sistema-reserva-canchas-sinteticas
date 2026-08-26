# Pruebas del sistema

Las pruebas automatizadas no están en esta carpeta. Están en el proyecto MSTest:

**`src/SistemaCanchas.Tests/`**

Se corren junto con el resto de la solución:

```
dotnet test src/SistemaCanchasSinteticas.sln
```

No necesitan SQL Server: los repositorios se reemplazan por dobles en memoria (`DoblesPrueba.cs`).

## Dónde está cada prueba

| Archivo | Qué cubre |
|---|---|
| `CifradorAesTests.cs` | Cifrado AES de la clave de SQL Server (cifrar, descifrar, clave incorrecta). |
| `UsuarioServiceTests.cs` | Login, registro de usuarios, activar/desactivar, cambio de clave y de nombre. |
| `CanchaServiceTests.cs` | Alta de canchas, horario de apertura/cierre, activar y desactivar. |
| `ClienteServiceTests.cs` | Alta de clientes, cédula ecuatoriana, duplicados, modificar y eliminar. |
| `ReservaServiceTests.cs` | Crear reserva (una o varias franjas), cambiar horario, cancelar, disponibilidad. |
| `PagoServiceTests.cs` | Registrar pago, monto inválido, reserva ya pagada, filtros de consulta. |
| `IngresoServiceTests.cs` | Consulta de ingresos (solo administrador) y rango de fechas. |
| `DoblesPrueba.cs` | Fakes de repositorios y de sesión. No son tests; los usan los archivos de arriba. |
| `AyudasPrueba.cs` | Usuarios de ejemplo (admin y empleado) para armar la sesión. |

El detalle de la corrida de regresión (pantalla + `dotnet test`) está en `docs/A15_Pruebas_Regresion.md`.
Las pruebas de instalación están en `docs/A17_Pruebas_Despliegue.md`.
