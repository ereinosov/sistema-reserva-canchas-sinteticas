# Sistema de Reserva de Canchas Sintéticas

![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-yellow?style=flat-square)
![Metodología](https://img.shields.io/badge/Metodolog%C3%ADa-Cascada-blue?style=flat-square)
![Lenguaje](https://img.shields.io/badge/Lenguaje-C%23-purple?style=flat-square&logo=csharp)
![Plataforma](https://img.shields.io/badge/Plataforma-WinForms%20.NET-blueviolet?style=flat-square&logo=dotnet)
![Base de datos](https://img.shields.io/badge/Base%20de%20datos-SQL%20Server-red?style=flat-square&logo=microsoftsqlserver)
![Universidad](https://img.shields.io/badge/UTEQ-FCC-green?style=flat-square)

Aplicación de escritorio desarrollada en **Windows Forms (C#)** con **SQL Server** para la gestión integral de una cancha sintética de fútbol. Centraliza en un único módulo la administración de clientes, reservas, pagos y disponibilidad.

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
/docs       Artefactos y documentación del ciclo de desarrollo
/src        Código fuente de la aplicación WinForms C#
/tests      Casos y registros de pruebas
README.md
```

---

### Módulos del sistema

| | Módulo | Descripción |
|---|---|---|
| <img src="docs/icons/clientes.svg" width="20" height="20"> | Clientes | Registro, búsqueda y actualización de datos de clientes |
| <img src="docs/icons/reservas.svg" width="20" height="20"> | Reservas | Creación, modificación y cancelación de reservas |
| <img src="docs/icons/pagos.svg" width="20" height="20"> | Pagos | Registro de pagos y seguimiento de estado |
| <img src="docs/icons/disponibilidad.svg" width="20" height="20"> | Disponibilidad | Visualización de horarios libres y ocupados |
| <img src="docs/icons/canchas.svg" width="20" height="20"> | Canchas | Registro, consulta, modificación y desactivación de canchas |
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

> **Plan de recuperación vigente:** no se amplía el alcance funcional; se corrigen los artefactos A1–A10, A11 (script DDL) está en desarrollo, y las pruebas unitarias inician junto con A13 en lugar de dejarse para el final.

---

### Artefactos

| # | Artefacto | Estado | Archivo |
|---|---|---|---|
| A1 | Especificación de Requisitos del Usuario y del Sistema | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PDF](./docs/A1_Especificacion_Requisitos_corregido.pdf) |
| A2 | Diagrama de Flujo de Datos del Usuario y del Sistema| ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A2_de_Flujo_de_Datos.png) |
| A3 | Diagrama de Arquitectura en Capas | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A3_Arquitectura_Capas.png) |
| A4 | Diagrama de Casos de Uso | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A4_Diagramas_Casos_De_Uso/.png) |
| A5 | Modelo Conceptual | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A5_Modelo_Conceptual.png) |
| A6 | Modelo Lógico | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A6_Modelo_Logico.png) |
| A7 | Modelo Físico | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A7_Modelo_Fisico.png) |
| A8 | Diagrama de Clases  | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PNG](./docs/A8_Diagrama_Clases.png) |
| A9 | Diagrama de Secuencias | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A10 | Diagrama de Flujo de Datos del Sistema | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A11 | Script DDL de creación de tablas (SQL Server)  | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [SQL](./docs/A11_Script_DDL.sql) |
| A12 | Documento de Estándares de Codificación C# (.NET) | ![Completado](https://img.shields.io/badge/-Completado-2ea44f?style=flat-square) | [PDF](./docs/A12_Estandares_Codificacion.pdf) |
| A13 | Código Fuente del Sistema (WinForms C#) | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A14 | Pruebas Unitarias | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A15 | Pruebas de Regresión | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A16 | Revisiones Técnicas Formales | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A17 | Pruebas de Despliegue | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A18 | Desarrollo de Instaladores | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |
| A19 | Manual de Usuario | ![Pendiente](https://img.shields.io/badge/-Pendiente-9e9e9e?style=flat-square) | — |

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
