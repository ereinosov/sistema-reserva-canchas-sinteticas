using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmPagos : Form
    {
        private readonly IPagoService _pagoService;
        private readonly IClienteService _clienteService;
        private readonly ICanchaService _canchaService;
        private int _idReservaSeleccionada;
        private bool _tienePago;
        private bool _reservaActiva;

        public FrmPagos(IPagoService pagoService, IClienteService clienteService, ICanchaService canchaService)
        {
            if (pagoService == null)
            {
                throw new ArgumentNullException(nameof(pagoService));
            }

            if (clienteService == null)
            {
                throw new ArgumentNullException(nameof(clienteService));
            }

            if (canchaService == null)
            {
                throw new ArgumentNullException(nameof(canchaService));
            }

            _pagoService = pagoService;
            _clienteService = clienteService;
            _canchaService = canchaService;
            InitializeComponent();
        }

        private void FrmPagos_Load(object sender, EventArgs e)
        {
            cboEstadoPago.Items.Add(new ItemEstado("Pagado", ValoresDominio.EstadoPago.Pagado));
            cboEstadoPago.Items.Add(new ItemEstado("Pendiente", ValoresDominio.EstadoPago.Pendiente));
            cboEstadoPago.SelectedIndex = 0;
            dtpFechaPago.Value = DateTime.Today;
            btnRegistrar.Enabled = false;
            CargarCombosFiltro();
            CargarPagos();
        }

        private void dgvPagos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPagos.CurrentRow == null || dgvPagos.CurrentRow.Index < 0)
            {
                return;
            }

            _idReservaSeleccionada = Convert.ToInt32(dgvPagos.CurrentRow.Cells["colIdReserva"].Value);
            string estadoPago = Convert.ToString(dgvPagos.CurrentRow.Cells["colEstado"].Value);
            string estadoReserva = Convert.ToString(dgvPagos.CurrentRow.Cells["colEstadoReserva"].Value);
            object monto = dgvPagos.CurrentRow.Cells["colMonto"].Value;
            _tienePago = monto != null && monto != DBNull.Value && !string.IsNullOrWhiteSpace(Convert.ToString(monto));
            _reservaActiva = string.Equals(estadoReserva, ValoresDominio.EstadoReserva.Activa, StringComparison.Ordinal);
            lblReservaSeleccionada.Text = "Reserva " + _idReservaSeleccionada + " — " +
                                          Convert.ToString(dgvPagos.CurrentRow.Cells["colCliente"].Value);
            btnRegistrar.Enabled = !_tienePago && _reservaActiva;

            if (_tienePago)
            {
                txtMontoPago.Text = Convert.ToString(monto);
                object fecha = dgvPagos.CurrentRow.Cells["colFechaPago"].Value;
                if (fecha is DateTime)
                {
                    dtpFechaPago.Value = ((DateTime)fecha).Date;
                }

                SeleccionarEstado(estadoPago);
            }
            else
            {
                txtMontoPago.Clear();
                dtpFechaPago.Value = DateTime.Today;
                cboEstadoPago.SelectedIndex = 0;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarPagos();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            dtpFiltroFecha.Checked = false;
            if (cboFiltroCliente.Items.Count > 0)
            {
                cboFiltroCliente.SelectedIndex = 0;
            }

            if (cboFiltroCancha.Items.Count > 0)
            {
                cboFiltroCancha.SelectedIndex = 0;
            }

            if (cboFiltroEstado.Items.Count > 0)
            {
                cboFiltroEstado.SelectedIndex = 0;
            }

            CargarPagos();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (_idReservaSeleccionada <= 0)
            {
                MessageBox.Show(
                    "Seleccione una reserva activa de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (_tienePago)
            {
                MessageBox.Show(
                    "La reserva seleccionada ya tiene un pago registrado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            decimal monto;
            if (!decimal.TryParse(txtMontoPago.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out monto))
            {
                errValidacion.SetError(txtMontoPago, "Ingrese un monto numérico válido.");
                return;
            }

            ItemEstado estado = cboEstadoPago.SelectedItem as ItemEstado;
            if (estado == null)
            {
                errValidacion.SetError(cboEstadoPago, "Seleccione el estado del pago.");
                return;
            }

            errValidacion.Clear();
            try
            {
                _pagoService.RegistrarPago(_idReservaSeleccionada, monto, dtpFechaPago.Value.Date, estado.Valor);
                MessageBox.Show(
                    "Pago registrado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CargarPagos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarCombosFiltro()
        {
            cboFiltroEstado.Items.Clear();
            cboFiltroEstado.Items.Add(new ItemTexto(null, "Todos"));
            cboFiltroEstado.Items.Add(new ItemTexto(ValoresDominio.EstadoReserva.Activa, "Activa"));
            cboFiltroEstado.Items.Add(new ItemTexto(ValoresDominio.EstadoReserva.Cancelada, "Cancelada"));
            cboFiltroEstado.SelectedIndex = 0;

            IList<Cliente> clientes = _clienteService.ConsultarClientes(null, null);
            cboFiltroCliente.Items.Clear();
            cboFiltroCliente.Items.Add(new ItemId(0, "Todos"));
            for (int i = 0; i < clientes.Count; i++)
            {
                cboFiltroCliente.Items.Add(new ItemId(clientes[i].IdCliente, clientes[i].NombreCliente));
            }

            cboFiltroCliente.SelectedIndex = 0;

            IList<Cancha> canchas = _canchaService.ObtenerActivas();
            cboFiltroCancha.Items.Clear();
            cboFiltroCancha.Items.Add(new ItemId(0, "Todas"));
            for (int i = 0; i < canchas.Count; i++)
            {
                cboFiltroCancha.Items.Add(new ItemId(canchas[i].IdCancha, canchas[i].NombreCancha));
            }

            cboFiltroCancha.SelectedIndex = 0;
        }

        private void CargarPagos()
        {
            try
            {
                DateTime? fecha = dtpFiltroFecha.Checked ? (DateTime?)dtpFiltroFecha.Value.Date : null;
                ItemId cliente = cboFiltroCliente.SelectedItem as ItemId;
                ItemId cancha = cboFiltroCancha.SelectedItem as ItemId;
                ItemTexto estado = cboFiltroEstado.SelectedItem as ItemTexto;
                int? idCliente = cliente != null && cliente.Id > 0 ? (int?)cliente.Id : null;
                int? idCancha = cancha != null && cancha.Id > 0 ? (int?)cancha.Id : null;
                string estadoValor = estado != null ? estado.Valor : null;

                errValidacion.Clear();
                IList<Pago> pagos = _pagoService.ConsultarEstadoPago(fecha, idCliente, idCancha, estadoValor);
                dgvPagos.Rows.Clear();
                _idReservaSeleccionada = 0;
                _tienePago = false;
                _reservaActiva = false;
                btnRegistrar.Enabled = false;
                lblReservaSeleccionada.Text = "Seleccione una reserva activa.";

                for (int i = 0; i < pagos.Count; i++)
                {
                    Pago pago = pagos[i];
                    dgvPagos.Rows.Add(
                        pago.IdReserva,
                        pago.NombreCliente,
                        pago.NombreCancha,
                        pago.FechaHorario.Date,
                        FormatearHora(pago.HoraInicioHorario),
                        pago.EstadoReserva,
                        pago.EstadoPago,
                        pago.MontoPago.HasValue ? (object)pago.MontoPago.Value : null,
                        pago.FechaPago.HasValue ? (object)pago.FechaPago.Value.Date : null);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void SeleccionarEstado(string valor)
        {
            for (int i = 0; i < cboEstadoPago.Items.Count; i++)
            {
                ItemEstado item = cboEstadoPago.Items[i] as ItemEstado;
                if (item != null && string.Equals(item.Valor, valor, StringComparison.OrdinalIgnoreCase))
                {
                    cboEstadoPago.SelectedIndex = i;
                    return;
                }
            }
        }

        private static string FormatearHora(TimeSpan hora)
        {
            return hora.Hours.ToString("00") + ":" + hora.Minutes.ToString("00");
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

        private sealed class ItemId
        {
            internal ItemId(int id, string texto)
            {
                Id = id;
                Texto = texto;
            }

            internal int Id { get; private set; }

            internal string Texto { get; private set; }

            public override string ToString()
            {
                return Texto;
            }
        }

        private sealed class ItemTexto
        {
            internal ItemTexto(string valor, string texto)
            {
                Valor = valor;
                Texto = texto;
            }

            internal string Valor { get; private set; }

            internal string Texto { get; private set; }

            public override string ToString()
            {
                return Texto;
            }
        }

        private sealed class ItemEstado
        {
            internal ItemEstado(string texto, string valor)
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
