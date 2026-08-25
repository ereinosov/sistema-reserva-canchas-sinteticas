using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmReservas
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvReservas;
        private GroupBox grpFiltros;
        private GroupBox grpDatos;
        private Label lblFiltroFecha;
        private Label lblFiltroCliente;
        private Label lblFiltroCancha;
        private Label lblFiltroEstado;
        private DateTimePicker dtpFiltroFecha;
        private ComboBox cboFiltroCliente;
        private ComboBox cboFiltroCancha;
        private ComboBox cboFiltroEstado;
        private Button btnBuscar;
        private Button btnCargar;
        private Label lblCliente;
        private Label lblCancha;
        private Label lblFecha;
        private Label lblHorario;
        private ComboBox cboCliente;
        private ComboBox cboCancha;
        private DateTimePicker dtpFecha;
        private CheckedListBox clbHorarios;
        private Button btnRegistrar;
        private Button btnModificar;
        private Button btnCancelar;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colCancha;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFin;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colRegistradoPor;

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
            dgvReservas = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colCancha = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFin = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colRegistradoPor = new DataGridViewTextBoxColumn();
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
            lblCliente = new Label();
            cboCliente = new ComboBox();
            lblCancha = new Label();
            cboCancha = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblHorario = new Label();
            clbHorarios = new CheckedListBox();
            btnRegistrar = new Button();
            btnModificar = new Button();
            btnCancelar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvReservas).BeginInit();
            grpFiltros.SuspendLayout();
            grpDatos.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colCancha.HeaderText = "Cancha";
            colCancha.Name = "colCancha";
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.DefaultCellStyle.Format = "yyyy-MM-dd";
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colFin.HeaderText = "Fin";
            colFin.Name = "colFin";
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colRegistradoPor.HeaderText = "Registró";
            colRegistradoPor.Name = "colRegistradoPor";

            dgvReservas.AllowUserToAddRows = false;
            dgvReservas.AllowUserToDeleteRows = false;
            dgvReservas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReservas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReservas.BackgroundColor = Color.White;
            dgvReservas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReservas.Columns.AddRange(new DataGridViewColumn[]
            {
                colId, colCliente, colCancha, colFecha, colInicio, colFin, colEstado, colRegistradoPor
            });
            dgvReservas.Location = new Point(16, 92);
            dgvReservas.MultiSelect = false;
            dgvReservas.Name = "dgvReservas";
            dgvReservas.ReadOnly = true;
            dgvReservas.RowHeadersVisible = false;
            dgvReservas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReservas.Size = new Size(952, 250);
            dgvReservas.SelectionChanged += dgvReservas_SelectionChanged;

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
            grpDatos.Controls.Add(lblCliente);
            grpDatos.Controls.Add(cboCliente);
            grpDatos.Controls.Add(lblCancha);
            grpDatos.Controls.Add(cboCancha);
            grpDatos.Controls.Add(lblFecha);
            grpDatos.Controls.Add(dtpFecha);
            grpDatos.Controls.Add(lblHorario);
            grpDatos.Controls.Add(clbHorarios);
            grpDatos.Controls.Add(btnRegistrar);
            grpDatos.Controls.Add(btnModificar);
            grpDatos.Controls.Add(btnCancelar);
            grpDatos.Location = new Point(16, 348);
            grpDatos.Size = new Size(952, 186);
            grpDatos.Text = "Datos de la reserva";

            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(16, 28);
            lblCliente.Text = "Cliente";
            cboCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cboCliente.Location = new Point(19, 48);
            cboCliente.Size = new Size(220, 23);
            cboCliente.TabIndex = 6;
            cboCliente.SelectedIndexChanged += cboCliente_TextoCambiado;
            cboCliente.TextUpdate += cboCliente_TextoCambiado;
            cboCliente.Leave += cboCliente_TextoCambiado;

            lblCancha.AutoSize = true;
            lblCancha.Location = new Point(256, 28);
            lblCancha.Text = "Cancha";
            cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCancha.Location = new Point(259, 48);
            cboCancha.Size = new Size(180, 23);
            cboCancha.TabIndex = 7;
            cboCancha.SelectedIndexChanged += cboCancha_SelectedIndexChanged;

            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(456, 28);
            lblFecha.Text = "Fecha";
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(459, 48);
            dtpFecha.Size = new Size(120, 23);
            dtpFecha.TabIndex = 8;
            dtpFecha.ValueChanged += dtpFecha_ValueChanged;

            lblHorario.AutoSize = true;
            lblHorario.Location = new Point(596, 28);
            lblHorario.Text = "Franjas libres";
            clbHorarios.CheckOnClick = true;
            clbHorarios.Location = new Point(599, 48);
            clbHorarios.Size = new Size(200, 94);
            clbHorarios.TabIndex = 9;

            btnRegistrar.Enabled = false;
            btnRegistrar.Location = new Point(19, 148);
            btnRegistrar.Size = new Size(100, 28);
            btnRegistrar.TabIndex = 10;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnModificar.Enabled = false;
            btnModificar.Location = new Point(125, 148);
            btnModificar.Size = new Size(140, 28);
            btnModificar.TabIndex = 11;
            btnModificar.Text = "Cambiar horario";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnCancelar.Enabled = false;
            btnCancelar.Location = new Point(271, 148);
            btnCancelar.Size = new Size(130, 28);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar reserva";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 556);
            Controls.Add(grpDatos);
            Controls.Add(dgvReservas);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 500);
            Name = "FrmReservas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reservas";
            Load += FrmReservas_Load;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvReservas).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
