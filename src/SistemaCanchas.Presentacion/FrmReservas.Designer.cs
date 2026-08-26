using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmReservas
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvReservas;
        private GroupBox grpFiltros;
        private GroupBox grpNuevo;
        private GroupBox grpSeleccionado;
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
        private Label lblSeleccionado;
        private Label lblCanchaEdicion;
        private Label lblFechaEdicion;
        private Label lblHorarioEdicion;
        private ComboBox cboCanchaEdicion;
        private DateTimePicker dtpFechaEdicion;
        private CheckedListBox clbHorariosEdicion;
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
            grpNuevo = new GroupBox();
            lblCliente = new Label();
            cboCliente = new ComboBox();
            lblCancha = new Label();
            cboCancha = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblHorario = new Label();
            clbHorarios = new CheckedListBox();
            btnRegistrar = new Button();
            grpSeleccionado = new GroupBox();
            lblSeleccionado = new Label();
            lblCanchaEdicion = new Label();
            cboCanchaEdicion = new ComboBox();
            lblFechaEdicion = new Label();
            dtpFechaEdicion = new DateTimePicker();
            lblHorarioEdicion = new Label();
            clbHorariosEdicion = new CheckedListBox();
            btnModificar = new Button();
            btnCancelar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvReservas).BeginInit();
            grpFiltros.SuspendLayout();
            grpNuevo.SuspendLayout();
            grpSeleccionado.SuspendLayout();
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
            dgvReservas.AllowUserToResizeColumns = false;
            dgvReservas.AllowUserToResizeRows = false;
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
            dgvReservas.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvReservas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReservas.Size = new Size(952, 190);
            dgvReservas.SelectionChanged += dgvReservas_SelectionChanged;
            dgvReservas.CellClick += dgvReservas_CellClick;

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

            grpNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpNuevo.Controls.Add(lblCliente);
            grpNuevo.Controls.Add(cboCliente);
            grpNuevo.Controls.Add(lblCancha);
            grpNuevo.Controls.Add(cboCancha);
            grpNuevo.Controls.Add(lblFecha);
            grpNuevo.Controls.Add(dtpFecha);
            grpNuevo.Controls.Add(lblHorario);
            grpNuevo.Controls.Add(clbHorarios);
            grpNuevo.Controls.Add(btnRegistrar);
            grpNuevo.Location = new Point(16, 294);
            grpNuevo.Size = new Size(468, 236);
            grpNuevo.Text = "Nueva reserva";

            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(16, 24);
            lblCliente.Text = "Cliente";
            cboCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cboCliente.Location = new Point(19, 44);
            cboCliente.Size = new Size(200, 23);
            cboCliente.TabIndex = 6;
            cboCliente.SelectedIndexChanged += cboCliente_TextoCambiado;
            cboCliente.TextUpdate += cboCliente_TextoCambiado;
            cboCliente.Leave += cboCliente_TextoCambiado;

            lblCancha.AutoSize = true;
            lblCancha.Location = new Point(230, 24);
            lblCancha.Text = "Cancha";
            cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCancha.Location = new Point(233, 44);
            cboCancha.Size = new Size(140, 23);
            cboCancha.TabIndex = 7;
            cboCancha.SelectedIndexChanged += cboCancha_SelectedIndexChanged;

            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(16, 76);
            lblFecha.Text = "Fecha";
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(19, 96);
            dtpFecha.Size = new Size(120, 23);
            dtpFecha.TabIndex = 8;
            dtpFecha.ValueChanged += dtpFecha_ValueChanged;

            lblHorario.AutoSize = true;
            lblHorario.Location = new Point(230, 76);
            lblHorario.Text = "Franjas libres";
            clbHorarios.CheckOnClick = true;
            clbHorarios.Location = new Point(233, 96);
            clbHorarios.Size = new Size(216, 84);
            clbHorarios.TabIndex = 9;

            btnRegistrar.Enabled = false;
            btnRegistrar.Location = new Point(19, 196);
            btnRegistrar.Size = new Size(160, 28);
            btnRegistrar.TabIndex = 10;
            btnRegistrar.Text = "Registrar reserva";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            grpSeleccionado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpSeleccionado.Controls.Add(lblSeleccionado);
            grpSeleccionado.Controls.Add(lblCanchaEdicion);
            grpSeleccionado.Controls.Add(cboCanchaEdicion);
            grpSeleccionado.Controls.Add(lblFechaEdicion);
            grpSeleccionado.Controls.Add(dtpFechaEdicion);
            grpSeleccionado.Controls.Add(lblHorarioEdicion);
            grpSeleccionado.Controls.Add(clbHorariosEdicion);
            grpSeleccionado.Controls.Add(btnModificar);
            grpSeleccionado.Controls.Add(btnCancelar);
            grpSeleccionado.Enabled = false;
            grpSeleccionado.Location = new Point(492, 294);
            grpSeleccionado.Size = new Size(476, 236);
            grpSeleccionado.Text = "Editar reserva seleccionada";

            lblSeleccionado.AutoSize = false;
            lblSeleccionado.Location = new Point(16, 22);
            lblSeleccionado.Size = new Size(444, 32);
            lblSeleccionado.Text = "Seleccione una reserva activa de la lista para cambiar horario o cancelar.";

            lblCanchaEdicion.AutoSize = true;
            lblCanchaEdicion.Location = new Point(16, 58);
            lblCanchaEdicion.Text = "Cancha";
            cboCanchaEdicion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCanchaEdicion.Location = new Point(19, 78);
            cboCanchaEdicion.Size = new Size(180, 23);
            cboCanchaEdicion.TabIndex = 11;
            cboCanchaEdicion.SelectedIndexChanged += cboCanchaEdicion_SelectedIndexChanged;

            lblFechaEdicion.AutoSize = true;
            lblFechaEdicion.Location = new Point(210, 58);
            lblFechaEdicion.Text = "Fecha";
            dtpFechaEdicion.Format = DateTimePickerFormat.Short;
            dtpFechaEdicion.Location = new Point(213, 78);
            dtpFechaEdicion.Size = new Size(120, 23);
            dtpFechaEdicion.TabIndex = 12;
            dtpFechaEdicion.ValueChanged += dtpFechaEdicion_ValueChanged;

            lblHorarioEdicion.AutoSize = true;
            lblHorarioEdicion.Location = new Point(16, 110);
            lblHorarioEdicion.Text = "Nueva franja libre (una sola)";
            clbHorariosEdicion.CheckOnClick = true;
            clbHorariosEdicion.Location = new Point(19, 130);
            clbHorariosEdicion.Size = new Size(314, 58);
            clbHorariosEdicion.TabIndex = 13;

            btnModificar.Location = new Point(19, 196);
            btnModificar.Size = new Size(140, 28);
            btnModificar.TabIndex = 14;
            btnModificar.Text = "Cambiar horario";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnCancelar.Location = new Point(165, 196);
            btnCancelar.Size = new Size(140, 28);
            btnCancelar.TabIndex = 15;
            btnCancelar.Text = "Cancelar reserva";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 546);
            Controls.Add(grpSeleccionado);
            Controls.Add(grpNuevo);
            Controls.Add(dgvReservas);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(920, 560);
            Name = "FrmReservas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reservas";
            Load += FrmReservas_Load;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvReservas).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpNuevo.ResumeLayout(false);
            grpNuevo.PerformLayout();
            grpSeleccionado.ResumeLayout(false);
            grpSeleccionado.PerformLayout();
            ResumeLayout(false);
        }
    }
}
