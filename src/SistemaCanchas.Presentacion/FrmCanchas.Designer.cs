using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmCanchas
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvCanchas;
        private Button btnCargar;
        private GroupBox grpNuevo;
        private Label lblNombreNuevo;
        private TextBox txtNombreNuevo;
        private Label lblInicioNuevo;
        private Label lblFinNuevo;
        private DateTimePicker dtpInicioNuevo;
        private DateTimePicker dtpFinNuevo;
        private Button btnRegistrar;
        private GroupBox grpSeleccionado;
        private Label lblSeleccionado;
        private Label lblNombreEdicion;
        private TextBox txtNombreEdicion;
        private Label lblInicioEdicion;
        private Label lblFinEdicion;
        private DateTimePicker dtpInicioEdicion;
        private DateTimePicker dtpFinEdicion;
        private Button btnModificar;
        private Button btnDesactivar;
        private Button btnActivar;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFin;

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
            dgvCanchas = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFin = new DataGridViewTextBoxColumn();
            btnCargar = new Button();
            grpNuevo = new GroupBox();
            lblNombreNuevo = new Label();
            txtNombreNuevo = new TextBox();
            lblInicioNuevo = new Label();
            dtpInicioNuevo = new DateTimePicker();
            lblFinNuevo = new Label();
            dtpFinNuevo = new DateTimePicker();
            btnRegistrar = new Button();
            grpSeleccionado = new GroupBox();
            lblSeleccionado = new Label();
            lblNombreEdicion = new Label();
            txtNombreEdicion = new TextBox();
            lblInicioEdicion = new Label();
            dtpInicioEdicion = new DateTimePicker();
            lblFinEdicion = new Label();
            dtpFinEdicion = new DateTimePicker();
            btnModificar = new Button();
            btnDesactivar = new Button();
            btnActivar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvCanchas).BeginInit();
            grpNuevo.SuspendLayout();
            grpSeleccionado.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colNombre.FillWeight = 44F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.FillWeight = 20F;
            colInicio.HeaderText = "Abre";
            colInicio.Name = "colInicio";
            colInicio.FillWeight = 18F;
            colInicio.DefaultCellStyle.Format = @"hh\:mm";
            colFin.HeaderText = "Cierra";
            colFin.Name = "colFin";
            colFin.FillWeight = 18F;
            colFin.DefaultCellStyle.Format = @"hh\:mm";

            dgvCanchas.AllowUserToAddRows = false;
            dgvCanchas.AllowUserToDeleteRows = false;
            dgvCanchas.AllowUserToResizeColumns = false;
            dgvCanchas.AllowUserToResizeRows = false;
            dgvCanchas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCanchas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCanchas.BackgroundColor = Color.White;
            dgvCanchas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCanchas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colEstado, colInicio, colFin });
            dgvCanchas.Location = new Point(16, 16);
            dgvCanchas.MultiSelect = false;
            dgvCanchas.Name = "dgvCanchas";
            dgvCanchas.ReadOnly = true;
            dgvCanchas.RowHeadersVisible = false;
            dgvCanchas.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvCanchas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanchas.Size = new Size(748, 200);
            dgvCanchas.SelectionChanged += dgvCanchas_SelectionChanged;
            dgvCanchas.CellClick += dgvCanchas_CellClick;

            btnCargar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCargar.Location = new Point(770, 16);
            btnCargar.Size = new Size(118, 28);
            btnCargar.TabIndex = 0;
            btnCargar.Text = "Actualizar lista";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            grpNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpNuevo.Controls.Add(lblNombreNuevo);
            grpNuevo.Controls.Add(txtNombreNuevo);
            grpNuevo.Controls.Add(lblInicioNuevo);
            grpNuevo.Controls.Add(dtpInicioNuevo);
            grpNuevo.Controls.Add(lblFinNuevo);
            grpNuevo.Controls.Add(dtpFinNuevo);
            grpNuevo.Controls.Add(btnRegistrar);
            grpNuevo.Location = new Point(16, 228);
            grpNuevo.Size = new Size(428, 196);
            grpNuevo.Text = "Nueva cancha";

            lblNombreNuevo.AutoSize = true;
            lblNombreNuevo.Location = new Point(16, 28);
            lblNombreNuevo.Text = "Nombre";
            txtNombreNuevo.Location = new Point(19, 48);
            txtNombreNuevo.MaxLength = ValoresDominio.LongitudMaximaNombreCancha;
            txtNombreNuevo.Size = new Size(390, 23);
            txtNombreNuevo.TabIndex = 1;

            lblInicioNuevo.AutoSize = true;
            lblInicioNuevo.Location = new Point(16, 80);
            lblInicioNuevo.Text = "Abre";
            ConfigurarHora(dtpInicioNuevo);
            dtpInicioNuevo.Location = new Point(19, 100);
            dtpInicioNuevo.Size = new Size(80, 23);
            dtpInicioNuevo.TabIndex = 2;

            lblFinNuevo.AutoSize = true;
            lblFinNuevo.Location = new Point(112, 80);
            lblFinNuevo.Text = "Cierra";
            ConfigurarHora(dtpFinNuevo);
            dtpFinNuevo.Location = new Point(115, 100);
            dtpFinNuevo.Size = new Size(80, 23);
            dtpFinNuevo.TabIndex = 3;

            btnRegistrar.Location = new Point(19, 144);
            btnRegistrar.Size = new Size(160, 28);
            btnRegistrar.TabIndex = 4;
            btnRegistrar.Text = "Registrar cancha";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            grpSeleccionado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpSeleccionado.Controls.Add(lblSeleccionado);
            grpSeleccionado.Controls.Add(lblNombreEdicion);
            grpSeleccionado.Controls.Add(txtNombreEdicion);
            grpSeleccionado.Controls.Add(lblInicioEdicion);
            grpSeleccionado.Controls.Add(dtpInicioEdicion);
            grpSeleccionado.Controls.Add(lblFinEdicion);
            grpSeleccionado.Controls.Add(dtpFinEdicion);
            grpSeleccionado.Controls.Add(btnModificar);
            grpSeleccionado.Controls.Add(btnDesactivar);
            grpSeleccionado.Controls.Add(btnActivar);
            grpSeleccionado.Enabled = false;
            grpSeleccionado.Location = new Point(452, 228);
            grpSeleccionado.Size = new Size(436, 196);
            grpSeleccionado.Text = "Editar cancha seleccionada";

            lblSeleccionado.AutoSize = false;
            lblSeleccionado.Location = new Point(16, 24);
            lblSeleccionado.Size = new Size(404, 32);
            lblSeleccionado.Text = "Seleccione una cancha de la lista para editarla.";

            lblNombreEdicion.AutoSize = true;
            lblNombreEdicion.Location = new Point(16, 60);
            lblNombreEdicion.Text = "Nombre";
            txtNombreEdicion.Location = new Point(19, 80);
            txtNombreEdicion.MaxLength = ValoresDominio.LongitudMaximaNombreCancha;
            txtNombreEdicion.Size = new Size(396, 23);
            txtNombreEdicion.TabIndex = 5;

            lblInicioEdicion.AutoSize = true;
            lblInicioEdicion.Location = new Point(16, 112);
            lblInicioEdicion.Text = "Abre";
            ConfigurarHora(dtpInicioEdicion);
            dtpInicioEdicion.Location = new Point(19, 132);
            dtpInicioEdicion.Size = new Size(80, 23);
            dtpInicioEdicion.TabIndex = 6;

            lblFinEdicion.AutoSize = true;
            lblFinEdicion.Location = new Point(112, 112);
            lblFinEdicion.Text = "Cierra";
            ConfigurarHora(dtpFinEdicion);
            dtpFinEdicion.Location = new Point(115, 132);
            dtpFinEdicion.Size = new Size(80, 23);
            dtpFinEdicion.TabIndex = 7;

            btnModificar.Location = new Point(211, 130);
            btnModificar.Size = new Size(130, 28);
            btnModificar.TabIndex = 8;
            btnModificar.Text = "Guardar cambios";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnDesactivar.Location = new Point(19, 164);
            btnDesactivar.Size = new Size(100, 28);
            btnDesactivar.TabIndex = 9;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnActivar.Enabled = false;
            btnActivar.Location = new Point(125, 164);
            btnActivar.Size = new Size(100, 28);
            btnActivar.TabIndex = 10;
            btnActivar.Text = "Activar";
            btnActivar.UseVisualStyleBackColor = true;
            btnActivar.Click += btnActivar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(904, 440);
            Controls.Add(grpSeleccionado);
            Controls.Add(grpNuevo);
            Controls.Add(btnCargar);
            Controls.Add(dgvCanchas);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(820, 500);
            Name = "FrmCanchas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Canchas";
            Load += FrmCanchas_Load;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvCanchas).EndInit();
            grpNuevo.ResumeLayout(false);
            grpNuevo.PerformLayout();
            grpSeleccionado.ResumeLayout(false);
            grpSeleccionado.PerformLayout();
            ResumeLayout(false);
        }

        private static void ConfigurarHora(DateTimePicker selector)
        {
            selector.Format = DateTimePickerFormat.Custom;
            selector.CustomFormat = "HH:mm";
            selector.ShowUpDown = true;
        }
    }
}
