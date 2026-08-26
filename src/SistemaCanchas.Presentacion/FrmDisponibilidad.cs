using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmDisponibilidad : Form
    {
        private readonly IReservaService _reservaService;
        private readonly ICanchaService _canchaService;

        public FrmDisponibilidad(IReservaService reservaService, ICanchaService canchaService)
        {
            if (reservaService == null)
            {
                throw new ArgumentNullException(nameof(reservaService));
            }

            if (canchaService == null)
            {
                throw new ArgumentNullException(nameof(canchaService));
            }

            _reservaService = reservaService;
            _canchaService = canchaService;
            InitializeComponent();
            TextosUi.ConfigurarGrilla(dgvDisponibilidad);
        }

        private void FrmDisponibilidad_Load(object sender, EventArgs e)
        {
            dtpFecha.MinDate = DateTime.Today.AddYears(-1);
            dtpFecha.MaxDate = DateTime.Today.AddYears(1);
            dtpFecha.Value = DateTime.Today;
            try
            {
                IList<Cancha> canchas = _canchaService.ObtenerActivas();
                for (int i = 0; i < canchas.Count; i++)
                {
                    cboCancha.Items.Add(new ItemId(canchas[i].IdCancha, canchas[i].NombreCancha));
                }

                if (cboCancha.Items.Count > 0)
                {
                    cboCancha.SelectedIndex = 0;
                    Consultar();
                }
            }
            catch (Exception ex)
            {
                if (ex is ValidacionNegocioException ||
                    ex is OperacionNoPermitidaException ||
                    ex is ErrorInfraestructuraException)
                {
                    MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                throw;
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void Consultar()
        {
            ItemId cancha = cboCancha.SelectedItem as ItemId;
            if (cancha == null || cancha.Id <= 0)
            {
                MessageBox.Show(
                    "Seleccione una cancha activa.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                IList<Horario> franjas = _reservaService.ConsultarDisponibilidad(cancha.Id, dtpFecha.Value.Date);
                dgvDisponibilidad.Rows.Clear();
                for (int i = 0; i < franjas.Count; i++)
                {
                    Horario franja = franjas[i];
                    dgvDisponibilidad.Rows.Add(
                        franja.IdHorario,
                        FormatearHora(franja.HoraInicioHorario),
                        FormatearHora(franja.HoraFinHorario),
                        franja.EstadoFranja);
                }

                TextosUi.QuitarSeleccionGrilla(dgvDisponibilidad);
            }
            catch (Exception ex)
            {
                if (ex is ValidacionNegocioException ||
                    ex is OperacionNoPermitidaException ||
                    ex is ErrorInfraestructuraException)
                {
                    MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                throw;
            }
        }

        private static string FormatearHora(TimeSpan hora)
        {
            return hora.Hours.ToString("00") + ":" + hora.Minutes.ToString("00");
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
    }
}
