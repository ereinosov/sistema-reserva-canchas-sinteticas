# Sistema de Reserva de Canchas Sintéticas

![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-yellow?style=flat-square)
![Metodología](https://img.shields.io/badge/Metodolog%C3%ADa-Cascada-blue?style=flat-square)
![Lenguaje](https://img.shields.io/badge/Lenguaje-C%23-purple?style=flat-square&logo=csharp)
![Plataforma](https://img.shields.io/badge/Plataforma-WinForms%20.NET-blueviolet?style=flat-square&logo=dotnet)
![Base de datos](https://img.shields.io/badge/Base%20de%20datos-SQL%20Server-red?style=flat-square&logo=microsoftsqlserver)
![Universidad](https://img.shields.io/badge/UTEQ-FCC-green?style=flat-square)

Aplicación de escritorio en **Windows Forms (C#)** y **SQL Server** para el día a día de una cancha sintética: clientes, reservas (una o varias horas), pagos, disponibilidad, canchas con su propio horario y cuentas por rol.

---

### Información académica

| Campo | Detalle |
|---|---|
| Universidad | Universidad Técnica Estatal de Quevedo (UTEQ) |
| Facultad | Facultad de Ciencias de la Computación (FCC) |
| Carrera | Ingeniería en Software |
| Materia | Proceso de Software "A" |
| Grupo | CHRS |
| Integrantes | Calderón Saltos Joseph · Herrera Barco Humberto · Reinoso Vélez Eduardo · Silva Triviño John |
| Período | Mayo – Agosto 2026 |

---

### Estructura del repositorio

```
/docs                           Artefactos del ciclo (A1–A19) y el script SQL
/src                            Solución WinForms en cuatro capas
/src/SistemaCanchas.Tests       Código de las pruebas unitarias (MSTest)
/test                           Índice de esas pruebas (README)
/setup                          Scripts del instalador (A18)
README.md
```

La base de datos se crea con **un solo archivo**: [`docs/A11_Script_DDL.sql`](./docs/A11_Script_DDL.sql). Hay que ejecutarlo una vez, en una instancia vacía. Si la base ya tiene datos, no lo vuelva a correr: borra las tablas.

Las pruebas automáticas se corren así:

```
dotnet test src/SistemaCanchasSinteticas.sln
```

Qué archivo cubre cada módulo está en [`test/README.md`](./test/README.md).

---

### Módulos del sistema

| | Módulo | Descripción |
|---|---|---|
| <img src="docs/icons/clientes.svg" width="20" height="20"> | Clientes | Registro, búsqueda y actualización de datos de clientes |
| <img src="docs/icons/reservas.svg" width="20" height="20"> | Reservas | Alta de una o varias horas, cambio de horario y cancelación |
| <img src="docs/icons/pagos.svg" width="20" height="20"> | Pagos | Cobro de una reserva (un pago por reserva) |
| <img src="docs/icons/disponibilidad.svg" width="20" height="20"> | Disponibilidad | Horarios libres y ocupados |
| <img src="docs/icons/canchas.svg" width="20" height="20"> | Canchas | Nombre, horario de abre/cierra, activar y desactivar |
| <img src="docs/icons/usuarios.svg" width="20" height="20"> | Usuarios y acceso | Gestión de cuentas de empleado e inicio de sesión por rol |

---

### Planificación

El proyecto se desarrolla en **67 días hábiles** distribuidos en 4 fases:

| Fase | Días hábiles | Inicio | Fin |
|---|---|---|---|
| Investigación | 7 | 27 may 2026 | 04 jun 2026 |
| Análisis y diseño | 25 | 05 jun 2026 | 29 jun 2026 |
| Construcción y pruebas | 25 | 30 jun 2026 | 14 ago 2026 |
| Implementación | 10 | 15 ago 2026 | 28 ago 2026 |

El diagrama de Gantt detallado está disponible en [`/docs/GANTT_Corte2.xlsx`](./docs/GANTT_Corte2.xlsx).

> A1–A19 están cerrados. No se suma alcance funcional nuevo.

---

### Artefactos

| # | Artefacto | Estado | Archivo |
|---|---|---|---|
| A1 | Especificación de Requisitos del Usuario y del Sistema | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PDF](./docs/A1_Especificacion_Requisitos_corregido.pdf) |
| A2 | Diagrama de Flujo de Datos del Usuario y del Sistema| ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A2_de_Flujo_de_Datos.png) |
| A3 | Diagrama de Arquitectura en Capas | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A3_Arquitectura_Capas.png) |
| A4 | Diagrama de Casos de Uso | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A4_Diagramas_Casos_De_Uso/) |
| A5 | Modelo Conceptual | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A5_Modelo_Conceptual.png) |
| A6 | Modelo Lógico | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A6_Modelo_Logico.png) |
| A7 | Modelo Físico | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A7_Modelo_Fisico.png) |
| A8 | Diagrama de Clases  | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A8_Diagrama_Clases.png) |
| A9 | Diagrama de Secuencias | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A9_Diagrama_De_Secuencia.png) |
| A10 | Diagrama de Flujo de Datos del Sistema | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A10_DFD_Sistema_Nivel1.png) |
| A11 | Script DDL de creación de tablas (SQL Server)  | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [SQL](./docs/A11_Script_DDL.sql) (archivo único) |
| A12 | Documento de Estándares de Codificación C# (.NET) | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PDF](./docs/A12_Estandares_Codificacion.pdf) |
| A13 | Código Fuente del Sistema (WinForms C#) | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [SLN](./src/SistemaCanchasSinteticas.sln) |
| A14 | Pruebas Unitarias | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [Tests](./src/SistemaCanchas.Tests/) · [índice](./test/README.md) |
| A15 | Pruebas de Regresión | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [MD](./docs/A15_Pruebas_Regresion.md) |
| A16 | Revisiones Técnicas Formales | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [MD](./docs/A16_Revisiones_Tecnicas_Formales.md) |
| A17 | Pruebas de Despliegue | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [MD](./docs/A17_Pruebas_Despliegue.md) |
| A18 | Desarrollo de Instaladores | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [MD](./docs/A18_Desarrollo_Instaladores.md) · [setup](./setup/) |
| A19 | Manual de Usuario | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [MD](./docs/A19_Manual_Usuario.md) |

---

### Tecnologías

![C#](https://img.shields.io/badge/C%23-12.0-purple?style=flat-square&logo=csharp)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.8-blueviolet?style=flat-square&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red?style=flat-square&logo=microsoftsqlserver)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-blue?style=flat-square&logo=visualstudio)

---

### Referencias

- ISO/IEC. (2023). *ISO/IEC 25010:2023 — Systems and software engineering — SQuaRE — Product quality model* (2.ª ed.). International Organization for Standardization.
- ISO/IEC/IEEE. (2018). *Systems and software engineering — Life cycle processes — Requirements engineering (ISO/IEC/IEEE 29148:2018)* (2.ª ed.). International Organization for Standardization.
- Pressman, R. S., & Maxim, B. R. (2019). *Software Engineering: A Practitioner's Approach* (9.ª ed.). McGraw-Hill Education. ISBN 978-1-259-87297-6
- Sommerville, I. (2016). *Software Engineering* (10.ª ed.). Pearson Education. ISBN 978-0-13-394303-0
