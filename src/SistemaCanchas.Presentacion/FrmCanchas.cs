using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmCanchas : Form
    {
        private readonly ICanchaService _canchaService;
        private int _idSeleccionado;

        public FrmCanchas(ICanchaService canchaService)
        {
            if (canchaService == null)
            {
                throw new ArgumentNullException(nameof(canchaService));
            }

            _canchaService = canchaService;
            InitializeComponent();
            TextosUi.ConfigurarGrilla(dgvCanchas);
        }

        private void FrmCanchas_Load(object sender, EventArgs e)
        {
            AsignarHorarioPorDefecto(dtpInicioNuevo, dtpFinNuevo);
            AsignarHorarioPorDefecto(dtpInicioEdicion, dtpFinEdicion);
            CargarCanchas();
        }

        private void dgvCanchas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarPanelEdicion();
        }

        private void dgvCanchas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            MostrarSeleccion(dgvCanchas.Rows[e.RowIndex]);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarNombre(txtNombreNuevo))
            {
                return;
            }

            try
            {
                _canchaService.RegistrarCancha(
                    txtNombreNuevo.Text,
                    dtpInicioNuevo.Value.TimeOfDay,
                    dtpFinNuevo.Value.TimeOfDay);
                MessageBox.Show(
                    "Cancha registrada.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtNombreNuevo.Clear();
                AsignarHorarioPorDefecto(dtpInicioNuevo, dtpFinNuevo);
                CargarCanchas();
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
                    "Seleccione una cancha de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!ValidarNombre(txtNombreEdicion))
            {
                return;
            }

            try
            {
                _canchaService.ModificarCancha(
                    _idSeleccionado,
                    txtNombreEdicion.Text,
                    dtpInicioEdicion.Value.TimeOfDay,
                    dtpFinEdicion.Value.TimeOfDay);
                MessageBox.Show(
                    "Cancha actualizada.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CargarCanchas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione una cancha de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desactivar la cancha seleccionada? No recibirá reservas nuevas. " +
                "Las reservas ya registradas no se modifican.",
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
                _canchaService.DesactivarCancha(_idSeleccionado);
                CargarCanchas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione una cancha de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                _canchaService.ActivarCancha(_idSeleccionado);
                CargarCanchas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarCanchas();
        }

        private void CargarCanchas()
        {
            try
            {
                IList<Cancha> canchas = _canchaService.ObtenerTodas();
                dgvCanchas.SelectionChanged -= dgvCanchas_SelectionChanged;
                try
                {
                    dgvCanchas.Rows.Clear();
                    for (int i = 0; i < canchas.Count; i++)
                    {
                        Cancha cancha = canchas[i];
                        dgvCanchas.Rows.Add(
                            cancha.IdCancha,
                            cancha.NombreCancha,
                            cancha.EstadoCancha,
                            cancha.HoraInicioOperacion,
                            cancha.HoraFinOperacion);
                    }

                    TextosUi.QuitarSeleccionGrilla(dgvCanchas);
                    MostrarSinSeleccion();
                }
                finally
                {
                    dgvCanchas.SelectionChanged += dgvCanchas_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void ActualizarPanelEdicion()
        {
            if (dgvCanchas.CurrentRow == null || dgvCanchas.CurrentRow.Index < 0 ||
                dgvCanchas.SelectedRows.Count == 0)
            {
                MostrarSinSeleccion();
                return;
            }

            MostrarSeleccion(dgvCanchas.CurrentRow);
        }

        private void MostrarSinSeleccion()
        {
            _idSeleccionado = 0;
            grpSeleccionado.Enabled = false;
            lblSeleccionado.Text = "Seleccione una cancha de la lista para editarla.";
            txtNombreEdicion.Clear();
            AsignarHorarioPorDefecto(dtpInicioEdicion, dtpFinEdicion);
        }

        private void MostrarSeleccion(DataGridViewRow fila)
        {
            _idSeleccionado = Convert.ToInt32(fila.Cells["colId"].Value);
            string nombre = Convert.ToString(fila.Cells["colNombre"].Value);
            string estado = Convert.ToString(fila.Cells["colEstado"].Value);
            bool inactiva = string.Equals(estado, ValoresDominio.EstadoCancha.Inactiva, StringComparison.Ordinal);

            grpSeleccionado.Enabled = true;
            lblSeleccionado.Text = "Editando: " + nombre + " — " + estado + ".";
            txtNombreEdicion.Text = nombre;
            AsignarHora(dtpInicioEdicion, fila.Cells["colInicio"].Value);
            AsignarHora(dtpFinEdicion, fila.Cells["colFin"].Value);
            btnActivar.Enabled = inactiva;
            btnDesactivar.Enabled = !inactiva;
        }

        private bool ValidarNombre(TextBox cuadro)
        {
            errValidacion.Clear();
            if (string.IsNullOrWhiteSpace(cuadro.Text))
            {
                errValidacion.SetError(cuadro, "Ingrese el nombre de la cancha.");
                return false;
            }

            return true;
        }

        private static void AsignarHorarioPorDefecto(DateTimePicker inicio, DateTimePicker fin)
        {
            inicio.Value = DateTime.Today.AddHours(ValoresDominio.HoraInicioFranja);
            fin.Value = DateTime.Today.AddHours(ValoresDominio.HoraFinOperacion);
        }

        private static void AsignarHora(DateTimePicker selector, object valor)
        {
            if (valor is TimeSpan)
            {
                selector.Value = DateTime.Today.Add((TimeSpan)valor);
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
    }
}
