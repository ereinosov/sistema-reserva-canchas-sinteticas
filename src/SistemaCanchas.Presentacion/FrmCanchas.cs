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
        }

        private void FrmCanchas_Load(object sender, EventArgs e)
        {
            CargarCanchas();
        }

        private void dgvCanchas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCanchas.CurrentRow == null || dgvCanchas.CurrentRow.Index < 0)
            {
                return;
            }

            _idSeleccionado = Convert.ToInt32(dgvCanchas.CurrentRow.Cells["colId"].Value);
            txtNombreCancha.Text = Convert.ToString(dgvCanchas.CurrentRow.Cells["colNombre"].Value);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarNombre())
            {
                return;
            }

            try
            {
                _canchaService.RegistrarCancha(txtNombreCancha.Text);
                MessageBox.Show(
                    "Cancha registrada.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtNombreCancha.Clear();
                _idSeleccionado = 0;
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

            if (!ValidarNombre())
            {
                return;
            }

            try
            {
                _canchaService.ModificarCancha(_idSeleccionado, txtNombreCancha.Text);
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
                txtNombreCancha.Clear();
                _idSeleccionado = 0;
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
                dgvCanchas.Rows.Clear();
                for (int i = 0; i < canchas.Count; i++)
                {
                    Cancha cancha = canchas[i];
                    dgvCanchas.Rows.Add(cancha.IdCancha, cancha.NombreCancha, cancha.EstadoCancha);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private bool ValidarNombre()
        {
            errValidacion.Clear();
            if (string.IsNullOrWhiteSpace(txtNombreCancha.Text))
            {
                errValidacion.SetError(txtNombreCancha, "Ingrese el nombre de la cancha.");
                return false;
            }

            return true;
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
