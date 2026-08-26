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
            TextosUi.ConfigurarGrilla(dgvClientes);
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            CargarTipos(cboTipoNuevo);
            CargarTipos(cboTipoEdicion);
            btnEliminar.Visible = _puedeEliminar;
            CargarClientes();
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarPanelEdicion();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            MostrarSeleccion(dgvClientes.Rows[e.RowIndex]);
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
            ItemTipo tipo = cboTipoNuevo.SelectedItem as ItemTipo;
            if (tipo == null)
            {
                errValidacion.SetError(cboTipoNuevo, "Seleccione el tipo de documento.");
                return;
            }

            try
            {
                _clienteService.RegistrarCliente(
                    txtNombreNuevo.Text,
                    tipo.Valor,
                    txtNumeroNuevo.Text,
                    txtTelefonoNuevo.Text,
                    txtCorreoNuevo.Text);

                MessageBox.Show(
                    "Cliente registrado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarAlta();
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

            ItemTipo tipo = cboTipoEdicion.SelectedItem as ItemTipo;
            if (tipo == null)
            {
                errValidacion.SetError(cboTipoEdicion, "Seleccione el tipo de documento.");
                return;
            }

            try
            {
                _clienteService.ModificarCliente(
                    _idSeleccionado,
                    txtNombreEdicion.Text,
                    tipo.Valor,
                    txtNumeroEdicion.Text,
                    txtTelefonoEdicion.Text,
                    txtCorreoEdicion.Text);

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

                dgvClientes.SelectionChanged -= dgvClientes_SelectionChanged;
                try
                {
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

                    TextosUi.QuitarSeleccionGrilla(dgvClientes);
                    MostrarSinSeleccion();
                }
                finally
                {
                    dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void ActualizarPanelEdicion()
        {
            if (dgvClientes.CurrentRow == null || dgvClientes.CurrentRow.Index < 0 ||
                dgvClientes.SelectedRows.Count == 0)
            {
                MostrarSinSeleccion();
                return;
            }

            MostrarSeleccion(dgvClientes.CurrentRow);
        }

        private void MostrarSinSeleccion()
        {
            _idSeleccionado = 0;
            grpSeleccionado.Enabled = false;
            lblSeleccionado.Text = "Seleccione un cliente de la lista para editarlo.";
            txtNombreEdicion.Clear();
            txtNumeroEdicion.Clear();
            txtTelefonoEdicion.Clear();
            txtCorreoEdicion.Clear();
            if (cboTipoEdicion.Items.Count > 0)
            {
                cboTipoEdicion.SelectedIndex = 0;
            }
        }

        private void MostrarSeleccion(DataGridViewRow fila)
        {
            _idSeleccionado = Convert.ToInt32(fila.Cells["colId"].Value);
            string nombre = Convert.ToString(fila.Cells["colNombre"].Value);
            grpSeleccionado.Enabled = true;
            lblSeleccionado.Text = "Editando: " + nombre + ".";
            txtNombreEdicion.Text = nombre;
            txtNumeroEdicion.Text = Convert.ToString(fila.Cells["colDocumento"].Value);
            txtTelefonoEdicion.Text = Convert.ToString(fila.Cells["colTelefono"].Value);
            txtCorreoEdicion.Text = Convert.ToString(fila.Cells["colCorreo"].Value);
            SeleccionarTipo(cboTipoEdicion, Convert.ToString(fila.Cells["colTipo"].Value));
        }

        private void LimpiarAlta()
        {
            txtNombreNuevo.Clear();
            txtNumeroNuevo.Clear();
            txtTelefonoNuevo.Clear();
            txtCorreoNuevo.Clear();
            cboTipoNuevo.SelectedIndex = 0;
            txtNombreNuevo.Focus();
        }

        private static void CargarTipos(ComboBox combo)
        {
            combo.Items.Clear();
            combo.Items.Add(new ItemTipo("Cédula", ValoresDominio.TipoDocumento.Cedula));
            combo.Items.Add(new ItemTipo("Pasaporte", ValoresDominio.TipoDocumento.Pasaporte));
            combo.Items.Add(new ItemTipo("RUC", ValoresDominio.TipoDocumento.Ruc));
            combo.SelectedIndex = 0;
        }

        private static void SeleccionarTipo(ComboBox combo, string valor)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                ItemTipo item = combo.Items[i] as ItemTipo;
                if (item != null && string.Equals(item.Valor, valor, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedIndex = i;
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
