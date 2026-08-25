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
        private Label lblFiltroFecha;
        private DateTimePicker dtpFiltroFecha;
        private Label lblFiltroCliente;
        private ComboBox cboFiltroCliente;
        private Label lblFiltroCancha;
        private ComboBox cboFiltroCancha;
        private Label lblFiltroEstado;
        private ComboBox cboFiltroEstado;
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
        private DataGridViewTextBoxColumn colCancha;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colEstadoReserva;
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
            colCancha = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colEstadoReserva = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colMonto = new DataGridViewTextBoxColumn();
            colFechaPago = new DataGridViewTextBoxColumn();
            grpFiltros = new GroupBox();
            lblFiltroFecha = new Label();
            dtpFiltroFecha = new DateTimePicker();
            lblFiltroCliente = new Label();
            cboFiltroCliente = new ComboBox();
            lblFiltroCancha = new Label();
            cboFiltroCancha = new ComboBox();
            lblFiltroEstado = new Label();
            cboFiltroEstado = new ComboBox();
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
            colCancha.HeaderText = "Cancha";
            colCancha.Name = "colCancha";
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.DefaultCellStyle.Format = "yyyy-MM-dd";
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colEstadoReserva.HeaderText = "Reserva";
            colEstadoReserva.Name = "colEstadoReserva";
            colEstado.HeaderText = "Pago";
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
                colIdReserva, colCliente, colCancha, colFecha, colInicio, colEstadoReserva, colEstado, colMonto, colFechaPago
            });
            dgvPagos.Location = new Point(16, 92);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(952, 240);
            dgvPagos.SelectionChanged += dgvPagos_SelectionChanged;

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblFiltroFecha);
            grpFiltros.Controls.Add(dtpFiltroFecha);
            grpFiltros.Controls.Add(lblFiltroCliente);
            grpFiltros.Controls.Add(cboFiltroCliente);
            grpFiltros.Controls.Add(lblFiltroCancha);
            grpFiltros.Controls.Add(cboFiltroCancha);
            grpFiltros.Controls.Add(lblFiltroEstado);
            grpFiltros.Controls.Add(cboFiltroEstado);
            grpFiltros.Controls.Add(btnBuscar);
            grpFiltros.Controls.Add(btnCargar);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(952, 70);
            grpFiltros.Text = "Búsqueda";

            lblFiltroFecha.AutoSize = true;
            lblFiltroFecha.Location = new Point(16, 32);
            lblFiltroFecha.Text = "Fecha";
            dtpFiltroFecha.Format = DateTimePickerFormat.Short;
            dtpFiltroFecha.Location = new Point(58, 28);
            dtpFiltroFecha.ShowCheckBox = true;
            dtpFiltroFecha.Checked = false;
            dtpFiltroFecha.Size = new Size(120, 23);
            dtpFiltroFecha.TabIndex = 0;

            lblFiltroCliente.AutoSize = true;
            lblFiltroCliente.Location = new Point(188, 32);
            lblFiltroCliente.Text = "Cliente";
            cboFiltroCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroCliente.Location = new Point(236, 28);
            cboFiltroCliente.Size = new Size(150, 23);
            cboFiltroCliente.TabIndex = 1;

            lblFiltroCancha.AutoSize = true;
            lblFiltroCancha.Location = new Point(396, 32);
            lblFiltroCancha.Text = "Cancha";
            cboFiltroCancha.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroCancha.Location = new Point(450, 28);
            cboFiltroCancha.Size = new Size(130, 23);
            cboFiltroCancha.TabIndex = 2;

            lblFiltroEstado.AutoSize = true;
            lblFiltroEstado.Location = new Point(590, 32);
            lblFiltroEstado.Text = "Estado";
            cboFiltroEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroEstado.Location = new Point(636, 28);
            cboFiltroEstado.Size = new Size(100, 23);
            cboFiltroEstado.TabIndex = 3;

            btnBuscar.Location = new Point(748, 26);
            btnBuscar.Size = new Size(80, 28);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;

            btnCargar.Location = new Point(834, 26);
            btnCargar.Size = new Size(100, 28);
            btnCargar.TabIndex = 5;
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
            grpDatos.Location = new Point(16, 344);
            grpDatos.Size = new Size(952, 148);
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
            txtMontoPago.TabIndex = 6;

            lblFechaPago.AutoSize = true;
            lblFechaPago.Location = new Point(156, 56);
            lblFechaPago.Text = "Fecha";
            dtpFechaPago.Format = DateTimePickerFormat.Short;
            dtpFechaPago.Location = new Point(159, 76);
            dtpFechaPago.Size = new Size(120, 23);
            dtpFechaPago.TabIndex = 7;

            lblEstadoPago.AutoSize = true;
            lblEstadoPago.Location = new Point(296, 56);
            lblEstadoPago.Text = "Estado";
            cboEstadoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoPago.Location = new Point(299, 76);
            cboEstadoPago.Size = new Size(120, 23);
            cboEstadoPago.TabIndex = 8;

            btnRegistrar.Location = new Point(19, 108);
            btnRegistrar.Size = new Size(120, 28);
            btnRegistrar.TabIndex = 9;
            btnRegistrar.Text = "Registrar pago";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 510);
            Controls.Add(grpDatos);
            Controls.Add(dgvPagos);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 480);
            Name = "FrmPagos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pagos";
            Load += FrmPagos_Load;
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
