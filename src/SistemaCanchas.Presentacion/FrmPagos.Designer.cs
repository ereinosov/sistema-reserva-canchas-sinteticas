using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmPagos
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvPagos;
        private GroupBox grpFiltros;
        private GroupBox grpDatos;
        private Label lblFiltroReserva;
        private TextBox txtFiltroReserva;
        private Button btnBuscar;
        private Button btnCargar;
        private Label lblReservaSeleccionada;
        private Label lblMontoPago;
        private Label lblFechaPago;
        private Label lblEstadoPago;
        private TextBox txtMontoPago;
        private DateTimePicker dtpFechaPago;
        private ComboBox cboEstadoPago;
        private Button btnRegistrar;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colIdReserva;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colEstado;
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
            dgvPagos = new DataGridView();
            colIdReserva = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colMonto = new DataGridViewTextBoxColumn();
            colFechaPago = new DataGridViewTextBoxColumn();
            grpFiltros = new GroupBox();
            lblFiltroReserva = new Label();
            txtFiltroReserva = new TextBox();
            btnBuscar = new Button();
            btnCargar = new Button();
            grpDatos = new GroupBox();
            lblReservaSeleccionada = new Label();
            lblMontoPago = new Label();
            txtMontoPago = new TextBox();
            lblFechaPago = new Label();
            dtpFechaPago = new DateTimePicker();
            lblEstadoPago = new Label();
            cboEstadoPago = new ComboBox();
            btnRegistrar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            grpFiltros.SuspendLayout();
            grpDatos.SuspendLayout();
            SuspendLayout();

            colIdReserva.HeaderText = "Reserva";
            colIdReserva.Name = "colIdReserva";
            colIdReserva.FillWeight = 70F;
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.DefaultCellStyle.Format = "yyyy-MM-dd";
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colMonto.HeaderText = "Monto";
            colMonto.Name = "colMonto";
            colMonto.DefaultCellStyle.Format = "N2";
            colFechaPago.HeaderText = "Fecha pago";
            colFechaPago.Name = "colFechaPago";
            colFechaPago.DefaultCellStyle.Format = "yyyy-MM-dd";

            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.BackgroundColor = Color.White;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Columns.AddRange(new DataGridViewColumn[]
            {
                colIdReserva, colCliente, colFecha, colInicio, colEstado, colMonto, colFechaPago
            });
            dgvPagos.Location = new Point(16, 86);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(816, 240);
            dgvPagos.SelectionChanged += dgvPagos_SelectionChanged;

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblFiltroReserva);
            grpFiltros.Controls.Add(txtFiltroReserva);
            grpFiltros.Controls.Add(btnBuscar);
            grpFiltros.Controls.Add(btnCargar);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(816, 64);
            grpFiltros.Text = "Búsqueda";

            lblFiltroReserva.AutoSize = true;
            lblFiltroReserva.Location = new Point(16, 28);
            lblFiltroReserva.Text = "Id reserva";
            txtFiltroReserva.Location = new Point(88, 24);
            txtFiltroReserva.MaxLength = 10;
            txtFiltroReserva.Size = new Size(100, 23);
            txtFiltroReserva.TabIndex = 0;

            btnBuscar.Location = new Point(200, 22);
            btnBuscar.Size = new Size(90, 28);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;

            btnCargar.Location = new Point(296, 22);
            btnCargar.Size = new Size(110, 28);
            btnCargar.TabIndex = 2;
            btnCargar.Text = "Ver todas";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            grpDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblReservaSeleccionada);
            grpDatos.Controls.Add(lblMontoPago);
            grpDatos.Controls.Add(txtMontoPago);
            grpDatos.Controls.Add(lblFechaPago);
            grpDatos.Controls.Add(dtpFechaPago);
            grpDatos.Controls.Add(lblEstadoPago);
            grpDatos.Controls.Add(cboEstadoPago);
            grpDatos.Controls.Add(btnRegistrar);
            grpDatos.Location = new Point(16, 338);
            grpDatos.Size = new Size(816, 148);
            grpDatos.Text = "Registrar pago";

            lblReservaSeleccionada.AutoSize = true;
            lblReservaSeleccionada.Location = new Point(16, 28);
            lblReservaSeleccionada.Name = "lblReservaSeleccionada";
            lblReservaSeleccionada.Text = "Seleccione una reserva activa.";

            lblMontoPago.AutoSize = true;
            lblMontoPago.Location = new Point(16, 56);
            lblMontoPago.Text = "Monto";
            txtMontoPago.Location = new Point(19, 76);
            txtMontoPago.MaxLength = 12;
            txtMontoPago.Size = new Size(120, 23);
            txtMontoPago.TabIndex = 3;

            lblFechaPago.AutoSize = true;
            lblFechaPago.Location = new Point(156, 56);
            lblFechaPago.Text = "Fecha";
            dtpFechaPago.Format = DateTimePickerFormat.Short;
            dtpFechaPago.Location = new Point(159, 76);
            dtpFechaPago.Size = new Size(120, 23);
            dtpFechaPago.TabIndex = 4;

            lblEstadoPago.AutoSize = true;
            lblEstadoPago.Location = new Point(296, 56);
            lblEstadoPago.Text = "Estado";
            cboEstadoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoPago.Location = new Point(299, 76);
            cboEstadoPago.Size = new Size(120, 23);
            cboEstadoPago.TabIndex = 5;

            btnRegistrar.Location = new Point(19, 108);
            btnRegistrar.Size = new Size(120, 28);
            btnRegistrar.TabIndex = 6;
            btnRegistrar.Text = "Registrar pago";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(848, 504);
            Controls.Add(grpDatos);
            Controls.Add(dgvPagos);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(800, 460);
            Name = "FrmPagos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pagos";
            Load += FrmPagos_Load;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
