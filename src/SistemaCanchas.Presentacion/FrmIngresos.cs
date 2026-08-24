using System;
using System.Globalization;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmIngresos : Form
    {
        private readonly IIngresoService _ingresoService;

        public FrmIngresos(IIngresoService ingresoService)
        {
            if (ingresoService == null)
            {
                throw new ArgumentNullException(nameof(ingresoService));
            }

            _ingresoService = ingresoService;
            InitializeComponent();
        }

        private void FrmIngresos_Load(object sender, EventArgs e)
        {
            DateTime hoy = DateTime.Today;
            dtpFechaInicio.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpFechaFin.Value = hoy;
            Consultar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void Consultar()
        {
            try
            {
                ConsultaIngresos consulta = _ingresoService.ConsultarIngresos(
                    dtpFechaInicio.Value.Date,
                    dtpFechaFin.Value.Date);

                dgvIngresos.Rows.Clear();
                for (int i = 0; i < consulta.Detalle.Count; i++)
                {
                    Ingreso ingreso = consulta.Detalle[i];
                    dgvIngresos.Rows.Add(
                        ingreso.IdPago,
                        ingreso.IdReserva,
                        ingreso.NombreCliente,
                        ingreso.NombreCancha,
                        ingreso.FechaHorario.Date,
                        FormatearHora(ingreso.HoraInicioHorario),
                        ingreso.MontoPago,
                        ingreso.FechaPago.HasValue ? (object)ingreso.FechaPago.Value.Date : null);
                }

                lblTotalIngresos.Text = "Total ingresos: " +
                    consulta.TotalIngresos.ToString("N2", CultureInfo.CurrentCulture);
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
    }
}
