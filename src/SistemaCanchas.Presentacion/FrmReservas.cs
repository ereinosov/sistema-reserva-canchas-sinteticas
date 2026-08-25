using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmReservas : Form
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly ICanchaService _canchaService;
        private int _idSeleccionado;
        private bool _suspenderEventos;

        public FrmReservas(IReservaService reservaService, IClienteService clienteService, ICanchaService canchaService)
        {
            if (reservaService == null)
            {
                throw new ArgumentNullException(nameof(reservaService));
            }

            if (clienteService == null)
            {
                throw new ArgumentNullException(nameof(clienteService));
            }

            if (canchaService == null)
            {
                throw new ArgumentNullException(nameof(canchaService));
            }

            _reservaService = reservaService;
            _clienteService = clienteService;
            _canchaService = canchaService;
            InitializeComponent();
        }

        private void FrmReservas_Load(object sender, EventArgs e)
        {
            dtpFecha.MinDate = DateTime.Today;
            dtpFecha.Value = DateTime.Today;
            try
            {
                CargarCombos();
                CargarReservas();
                CargarFranjas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void dgvReservas_SelectionChanged(object sender, EventArgs e)
        {
            if (_suspenderEventos || dgvReservas.CurrentRow == null || dgvReservas.CurrentRow.Index < 0)
            {
                return;
            }

            _idSeleccionado = Convert.ToInt32(dgvReservas.CurrentRow.Cells["colId"].Value);
            string estado = Convert.ToString(dgvReservas.CurrentRow.Cells["colEstado"].Value);
            bool activa = string.Equals(estado, ValoresDominio.EstadoReserva.Activa, StringComparison.Ordinal);
            btnModificar.Enabled = activa;
            btnCancelar.Enabled = activa;

            object valorFecha = dgvReservas.CurrentRow.Cells["colFecha"].Value;
            if (valorFecha is DateTime)
            {
                DateTime fecha = ((DateTime)valorFecha).Date;
                if (fecha < DateTime.Today)
                {
                    fecha = DateTime.Today;
                }

                _suspenderEventos = true;
                dtpFecha.Value = fecha;
                _suspenderEventos = false;
            }

            SeleccionarCanchaPorNombre(Convert.ToString(dgvReservas.CurrentRow.Cells["colCancha"].Value));
            CargarFranjas();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarReservas();
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

            CargarReservas();
        }

        private void cboCancha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspenderEventos)
            {
                return;
            }

            CargarFranjas();
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (_suspenderEventos)
            {
                return;
            }

            CargarFranjas();
        }

        private void cboCliente_TextoCambiado(object sender, EventArgs e)
        {
            ActualizarBotonRegistrar();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ItemId cliente = ResolverCliente();
            ItemId horario = cboHorario.SelectedItem as ItemId;
            if (cliente == null || cliente.Id <= 0)
            {
                errValidacion.SetError(cboCliente, "Seleccione un cliente.");
                return;
            }

            if (horario == null || horario.Id <= 0)
            {
                errValidacion.SetError(cboHorario, "Seleccione una franja libre.");
                return;
            }

            errValidacion.Clear();
            try
            {
                _reservaService.CrearReserva(cliente.Id, horario.Id);
                MessageBox.Show(
                    "Reserva registrada.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                _idSeleccionado = 0;
                CargarReservas();
                CargarFranjas();
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
                    "Seleccione una reserva activa de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            ItemId horario = cboHorario.SelectedItem as ItemId;
            if (horario == null || horario.Id <= 0)
            {
                errValidacion.SetError(cboHorario, "Seleccione una franja libre.");
                return;
            }

            errValidacion.Clear();
            try
            {
                _reservaService.ModificarHorario(_idSeleccionado, horario.Id);
                MessageBox.Show(
                    "Horario de la reserva actualizado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CargarReservas();
                CargarFranjas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione una reserva activa de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Cancelar la reserva seleccionada? La franja quedará libre.",
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
                _reservaService.CancelarReserva(_idSeleccionado);
                _idSeleccionado = 0;
                CargarReservas();
                CargarFranjas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarCombos()
        {
            _suspenderEventos = true;
            try
            {
                cboFiltroEstado.Items.Clear();
                cboFiltroEstado.Items.Add(new ItemTexto(null, "Todos"));
                cboFiltroEstado.Items.Add(new ItemTexto(ValoresDominio.EstadoReserva.Activa, "Activa"));
                cboFiltroEstado.Items.Add(new ItemTexto(ValoresDominio.EstadoReserva.Cancelada, "Cancelada"));
                cboFiltroEstado.SelectedIndex = 0;

                IList<Cliente> clientes = _clienteService.ConsultarClientes(null, null);
                cboCliente.Items.Clear();
                cboFiltroCliente.Items.Clear();
                cboFiltroCliente.Items.Add(new ItemId(0, "Todos"));
                for (int i = 0; i < clientes.Count; i++)
                {
                    ItemId item = new ItemId(clientes[i].IdCliente, clientes[i].NombreCliente);
                    cboCliente.Items.Add(item);
                    cboFiltroCliente.Items.Add(item);
                }

                if (cboCliente.Items.Count > 0)
                {
                    cboCliente.SelectedIndex = 0;
                }

                cboFiltroCliente.SelectedIndex = 0;

                IList<Cancha> canchas = _canchaService.ObtenerActivas();
                cboCancha.Items.Clear();
                cboFiltroCancha.Items.Clear();
                cboFiltroCancha.Items.Add(new ItemId(0, "Todas"));
                for (int i = 0; i < canchas.Count; i++)
                {
                    ItemId item = new ItemId(canchas[i].IdCancha, canchas[i].NombreCancha);
                    cboCancha.Items.Add(item);
                    cboFiltroCancha.Items.Add(item);
                }

                if (cboCancha.Items.Count > 0)
                {
                    cboCancha.SelectedIndex = 0;
                }

                cboFiltroCancha.SelectedIndex = 0;
            }
            finally
            {
                _suspenderEventos = false;
            }

            ActualizarBotonRegistrar();
        }

        private void CargarReservas()
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

                IList<Reserva> reservas = _reservaService.ConsultarReservas(fecha, idCliente, idCancha, estadoValor);

                _suspenderEventos = true;
                dgvReservas.Rows.Clear();
                for (int i = 0; i < reservas.Count; i++)
                {
                    Reserva reserva = reservas[i];
                    dgvReservas.Rows.Add(
                        reserva.IdReserva,
                        reserva.NombreCliente,
                        reserva.NombreCancha,
                        reserva.FechaHorario.Date,
                        FormatearHora(reserva.HoraInicioHorario),
                        FormatearHora(reserva.HoraFinHorario),
                        reserva.EstadoReserva,
                        reserva.RegistradoPor);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                _suspenderEventos = false;
            }
        }

        private void CargarFranjas()
        {
            cboHorario.Items.Clear();
            ItemId cancha = cboCancha.SelectedItem as ItemId;
            if (cancha == null || cancha.Id <= 0)
            {
                return;
            }

            try
            {
                IList<Horario> franjas = _reservaService.ConsultarDisponibilidad(cancha.Id, dtpFecha.Value.Date);
                for (int i = 0; i < franjas.Count; i++)
                {
                    if (!string.Equals(franjas[i].EstadoFranja, ValoresDominio.EstadoFranja.Libre, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string texto = FormatearHora(franjas[i].HoraInicioHorario) + " - " + FormatearHora(franjas[i].HoraFinHorario);
                    cboHorario.Items.Add(new ItemId(franjas[i].IdHorario, texto));
                }

                if (cboHorario.Items.Count > 0)
                {
                    cboHorario.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void SeleccionarCanchaPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return;
            }

            _suspenderEventos = true;
            try
            {
                for (int i = 0; i < cboCancha.Items.Count; i++)
                {
                    ItemId item = cboCancha.Items[i] as ItemId;
                    if (item != null && string.Equals(item.Texto, nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        cboCancha.SelectedIndex = i;
                        return;
                    }
                }
            }
            finally
            {
                _suspenderEventos = false;
            }
        }

        private ItemId ResolverCliente()
        {
            string texto = cboCliente.Text == null ? string.Empty : cboCliente.Text.Trim();
            if (texto.Length == 0)
            {
                return null;
            }

            ItemId seleccionado = cboCliente.SelectedItem as ItemId;
            if (seleccionado != null && seleccionado.Id > 0 &&
                string.Equals(seleccionado.Texto, texto, StringComparison.OrdinalIgnoreCase))
            {
                return seleccionado;
            }

            for (int i = 0; i < cboCliente.Items.Count; i++)
            {
                ItemId item = cboCliente.Items[i] as ItemId;
                if (item != null && string.Equals(item.Texto, texto, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        private void ActualizarBotonRegistrar()
        {
            ItemId cliente = ResolverCliente();
            btnRegistrar.Enabled = cliente != null && cliente.Id > 0;
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
    }
}
