using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Gestión de clientes (RF01-RF04 / RN02 / RN07).
    /// </summary>
    public interface IClienteService
    {
        /// <summary>Registra un cliente. Requiere sesión iniciada.</summary>
        /// <param name="nombre">Nombre completo.</param>
        /// <param name="tipoDocumento">cedula, pasaporte o ruc.</param>
        /// <param name="numeroDocumento">Número de documento.</param>
        /// <param name="telefono">Teléfono.</param>
        /// <param name="correo">Correo electrónico.</param>
        /// <returns>Id generado.</returns>
        int RegistrarCliente(string nombre, string tipoDocumento, string numeroDocumento, string telefono, string correo);

        /// <summary>Busca clientes por documento y/o nombre (RF02). Requiere sesión iniciada.</summary>
        /// <param name="numeroDocumento">Número exacto; vacío para no filtrar.</param>
        /// <param name="nombre">Fragmento de nombre; vacío para no filtrar.</param>
        /// <returns>Clientes encontrados.</returns>
        IList<Cliente> ConsultarClientes(string numeroDocumento, string nombre);

        /// <summary>Actualiza un cliente existente. Requiere sesión iniciada.</summary>
        /// <param name="idCliente">Identificador.</param>
        /// <param name="nombre">Nombre completo.</param>
        /// <param name="tipoDocumento">cedula, pasaporte o ruc.</param>
        /// <param name="numeroDocumento">Número de documento.</param>
        /// <param name="telefono">Teléfono.</param>
        /// <param name="correo">Correo electrónico.</param>
        void ModificarCliente(int idCliente, string nombre, string tipoDocumento, string numeroDocumento, string telefono, string correo);

        /// <summary>Elimina un cliente si no tiene reservas activas ni pagos pendientes (RF04 / RN02). Requiere administrador.</summary>
        /// <param name="idCliente">Identificador.</param>
        void EliminarCliente(int idCliente);
    }
}
