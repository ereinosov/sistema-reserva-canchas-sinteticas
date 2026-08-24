using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmIngresos
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvIngresos;
        private GroupBox grpFiltros;
        private Label lblFechaInicio;
        private Label lblFechaFin;
        private DateTimePicker dtpFechaInicio;
        private DateTimePicker dtpFechaFin;
        private Button btnConsultar;
        private Label lblTotalIngresos;
        private DataGridViewTextBoxColumn colIdPago;
        private DataGridViewTextBoxColumn colIdReserva;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colCancha;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colMonto;
        private DataGridViewTextBoxColumn colFechaPago;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvIngresos = new DataGridView();
            colIdPago = new DataGridViewTextBoxColumn();
            colIdReserva = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colCancha = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colMonto = new DataGridViewTextBoxColumn();
            colFechaPago = new DataGridViewTextBoxColumn();
            grpFiltros = new GroupBox();
            lblFechaInicio = new Label();
            dtpFechaInicio = new DateTimePicker();
            lblFechaFin = new Label();
            dtpFechaFin = new DateTimePicker();
            btnConsultar = new Button();
            lblTotalIngresos = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).BeginInit();
            grpFiltros.SuspendLayout();
            SuspendLayout();

            colIdPago.HeaderText = "Pago";
            colIdPago.Name = "colIdPago";
            colIdPago.FillWeight = 60F;
            colIdReserva.HeaderText = "Reserva";
            colIdReserva.Name = "colIdReserva";
            colIdReserva.FillWeight = 60F;
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colCancha.HeaderText = "Cancha";
            colCancha.Name = "colCancha";
            colFecha.HeaderText = "Fecha franja";
            colFecha.Name = "colFecha";
            colFecha.DefaultCellStyle.Format = "yyyy-MM-dd";
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colMonto.HeaderText = "Monto";
            colMonto.Name = "colMonto";
            colMonto.DefaultCellStyle.Format = "N2";
            colFechaPago.HeaderText = "Fecha pago";
            colFechaPago.Name = "colFechaPago";
            colFechaPago.DefaultCellStyle.Format = "yyyy-MM-dd";

            dgvIngresos.AllowUserToAddRows = false;
            dgvIngresos.AllowUserToDeleteRows = false;
            dgvIngresos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvIngresos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIngresos.BackgroundColor = Color.White;
            dgvIngresos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIngresos.Columns.AddRange(new DataGridViewColumn[]
            {
                colIdPago, colIdReserva, colCliente, colCancha, colFecha, colInicio, colMonto, colFechaPago
            });
            dgvIngresos.Location = new Point(16, 86);
            dgvIngresos.MultiSelect = false;
            dgvIngresos.Name = "dgvIngresos";
            dgvIngresos.ReadOnly = true;
            dgvIngresos.RowHeadersVisible = false;
            dgvIngresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngresos.Size = new Size(816, 300);

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblFechaInicio);
            grpFiltros.Controls.Add(dtpFechaInicio);
            grpFiltros.Controls.Add(lblFechaFin);
            grpFiltros.Controls.Add(dtpFechaFin);
            grpFiltros.Controls.Add(btnConsultar);
            grpFiltros.Controls.Add(lblTotalIngresos);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(816, 64);
            grpFiltros.Text = "Rango de fechas de franja";

            lblFechaInicio.AutoSize = true;
            lblFechaInicio.Location = new Point(16, 28);
            lblFechaInicio.Text = "Desde";
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(62, 24);
            dtpFechaInicio.Size = new Size(120, 23);
            dtpFechaInicio.TabIndex = 0;

            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new Point(198, 28);
            lblFechaFin.Text = "Hasta";
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(242, 24);
            dtpFechaFin.Size = new Size(120, 23);
            dtpFechaFin.TabIndex = 1;

            btnConsultar.Location = new Point(378, 22);
            btnConsultar.Size = new Size(100, 28);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;

            lblTotalIngresos.AutoSize = true;
            lblTotalIngresos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalIngresos.Location = new Point(500, 28);
            lblTotalIngresos.Name = "lblTotalIngresos";
            lblTotalIngresos.Text = "Total ingresos: 0,00";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(848, 412);
            Controls.Add(dgvIngresos);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(780, 360);
            Name = "FrmIngresos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ingresos";
            Load += FrmIngresos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvIngresos).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            ResumeLayout(false);
        }
    }
}
