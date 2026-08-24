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
        private int _idReservaSeleccionada;
        private bool _tienePago;

        public FrmPagos(IPagoService pagoService)
        {
            if (pagoService == null)
            {
                throw new ArgumentNullException(nameof(pagoService));
            }

            _pagoService = pagoService;
            InitializeComponent();
        }

        private void FrmPagos_Load(object sender, EventArgs e)
        {
            cboEstadoPago.Items.Add(new ItemEstado("Pagado", ValoresDominio.EstadoPago.Pagado));
            cboEstadoPago.Items.Add(new ItemEstado("Pendiente", ValoresDominio.EstadoPago.Pendiente));
            cboEstadoPago.SelectedIndex = 0;
            dtpFechaPago.Value = DateTime.Today;
            btnRegistrar.Enabled = false;
            CargarPagos();
        }

        private void dgvPagos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPagos.CurrentRow == null || dgvPagos.CurrentRow.Index < 0)
            {
                return;
            }

            _idReservaSeleccionada = Convert.ToInt32(dgvPagos.CurrentRow.Cells["colIdReserva"].Value);
            string estado = Convert.ToString(dgvPagos.CurrentRow.Cells["colEstado"].Value);
            object monto = dgvPagos.CurrentRow.Cells["colMonto"].Value;
            _tienePago = monto != null && monto != DBNull.Value && !string.IsNullOrWhiteSpace(Convert.ToString(monto));
            lblReservaSeleccionada.Text = "Reserva " + _idReservaSeleccionada + " — " +
                                          Convert.ToString(dgvPagos.CurrentRow.Cells["colCliente"].Value);
            btnRegistrar.Enabled = !_tienePago;

            if (_tienePago)
            {
                txtMontoPago.Text = Convert.ToString(monto);
                object fecha = dgvPagos.CurrentRow.Cells["colFechaPago"].Value;
                if (fecha is DateTime)
                {
                    dtpFechaPago.Value = ((DateTime)fecha).Date;
                }

                SeleccionarEstado(estado);
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
            txtFiltroReserva.Clear();
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

        private void CargarPagos()
        {
            try
            {
                int? idReserva = null;
                if (!string.IsNullOrWhiteSpace(txtFiltroReserva.Text))
                {
                    int filtro;
                    if (!int.TryParse(txtFiltroReserva.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out filtro) ||
                        filtro <= 0)
                    {
                        errValidacion.SetError(txtFiltroReserva, "Indique un identificador de reserva numérico.");
                        return;
                    }

                    idReserva = filtro;
                }

                errValidacion.Clear();
                IList<Pago> pagos = _pagoService.ConsultarEstadoPago(idReserva);
                dgvPagos.Rows.Clear();
                _idReservaSeleccionada = 0;
                _tienePago = false;
                btnRegistrar.Enabled = false;
                lblReservaSeleccionada.Text = "Seleccione una reserva activa.";

                for (int i = 0; i < pagos.Count; i++)
                {
                    Pago pago = pagos[i];
                    dgvPagos.Rows.Add(
                        pago.IdReserva,
                        pago.NombreCliente,
                        pago.FechaHorario.Date,
                        FormatearHora(pago.HoraInicioHorario),
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
