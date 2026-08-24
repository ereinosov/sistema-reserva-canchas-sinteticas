using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmClientes : Form
    {
        private readonly IClienteService _clienteService;
        private readonly bool _puedeEliminar;
        private int _idSeleccionado;

        public FrmClientes(IClienteService clienteService, bool puedeEliminar)
        {
            if (clienteService == null)
            {
                throw new ArgumentNullException(nameof(clienteService));
            }

            _clienteService = clienteService;
            _puedeEliminar = puedeEliminar;
            InitializeComponent();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.Add(new ItemTipo("Cédula", ValoresDominio.TipoDocumento.Cedula));
            cboTipoDocumento.Items.Add(new ItemTipo("Pasaporte", ValoresDominio.TipoDocumento.Pasaporte));
            cboTipoDocumento.Items.Add(new ItemTipo("RUC", ValoresDominio.TipoDocumento.Ruc));
            cboTipoDocumento.SelectedIndex = 0;
            btnEliminar.Visible = _puedeEliminar;
            CargarClientes();
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null || dgvClientes.CurrentRow.Index < 0)
            {
                return;
            }

            _idSeleccionado = Convert.ToInt32(dgvClientes.CurrentRow.Cells["colId"].Value);
            txtNombreCliente.Text = Convert.ToString(dgvClientes.CurrentRow.Cells["colNombre"].Value);
            txtNumeroDocumento.Text = Convert.ToString(dgvClientes.CurrentRow.Cells["colDocumento"].Value);
            txtTelefonoCliente.Text = Convert.ToString(dgvClientes.CurrentRow.Cells["colTelefono"].Value);
            txtCorreoCliente.Text = Convert.ToString(dgvClientes.CurrentRow.Cells["colCorreo"].Value);
            SeleccionarTipo(Convert.ToString(dgvClientes.CurrentRow.Cells["colTipo"].Value));
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Clear();
            txtFiltroDocumento.Clear();
            CargarClientes();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ItemTipo tipo = cboTipoDocumento.SelectedItem as ItemTipo;
            if (tipo == null)
            {
                errValidacion.SetError(cboTipoDocumento, "Seleccione el tipo de documento.");
                return;
            }

            try
            {
                _clienteService.RegistrarCliente(
                    txtNombreCliente.Text,
                    tipo.Valor,
                    txtNumeroDocumento.Text,
                    txtTelefonoCliente.Text,
                    txtCorreoCliente.Text);

                MessageBox.Show(
                    "Cliente registrado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un cliente de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            ItemTipo tipo = cboTipoDocumento.SelectedItem as ItemTipo;
            if (tipo == null)
            {
                errValidacion.SetError(cboTipoDocumento, "Seleccione el tipo de documento.");
                return;
            }

            try
            {
                _clienteService.ModificarCliente(
                    _idSeleccionado,
                    txtNombreCliente.Text,
                    tipo.Valor,
                    txtNumeroDocumento.Text,
                    txtTelefonoCliente.Text,
                    txtCorreoCliente.Text);

                MessageBox.Show(
                    "Cliente actualizado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarClientes();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un cliente de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Eliminar el cliente seleccionado? Solo se permite si no tiene reservas activas ni pagos pendientes.",
                TextosUi.TituloAplicacion,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _clienteService.EliminarCliente(_idSeleccionado);
                LimpiarFormulario();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarClientes()
        {
            try
            {
                IList<Cliente> clientes = _clienteService.ConsultarClientes(
                    txtFiltroDocumento.Text,
                    txtFiltroNombre.Text);

                dgvClientes.Rows.Clear();
                for (int i = 0; i < clientes.Count; i++)
                {
                    Cliente cliente = clientes[i];
                    dgvClientes.Rows.Add(
                        cliente.IdCliente,
                        cliente.NombreCliente,
                        cliente.TipoDocumentoCliente,
                        cliente.NumeroDocumentoCliente,
                        cliente.TelefonoCliente,
                        cliente.CorreoCliente);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            txtNombreCliente.Clear();
            txtNumeroDocumento.Clear();
            txtTelefonoCliente.Clear();
            txtCorreoCliente.Clear();
            cboTipoDocumento.SelectedIndex = 0;
            txtNombreCliente.Focus();
        }

        private void SeleccionarTipo(string valor)
        {
            for (int i = 0; i < cboTipoDocumento.Items.Count; i++)
            {
                ItemTipo item = cboTipoDocumento.Items[i] as ItemTipo;
                if (item != null && string.Equals(item.Valor, valor, StringComparison.OrdinalIgnoreCase))
                {
                    cboTipoDocumento.SelectedIndex = i;
                    return;
                }
            }
        }

        private static void MostrarError(Exception ex)
        {
            if (ex is ValidacionNegocioException ||
                ex is OperacionNoPermitidaException ||
                ex is ErrorInfraestructuraException)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            throw ex;
        }

        private sealed class ItemTipo
        {
            internal ItemTipo(string texto, string valor)
            {
                Texto = texto;
                Valor = valor;
            }

            internal string Texto { get; private set; }

            internal string Valor { get; private set; }

            public override string ToString()
            {
                return Texto;
            }
        }
    }
}
