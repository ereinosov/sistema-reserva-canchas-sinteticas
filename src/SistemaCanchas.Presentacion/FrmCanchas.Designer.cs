using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmCanchas
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvCanchas;
        private GroupBox grpDatos;
        private Label lblNombreCancha;
        private TextBox txtNombreCancha;
        private Label lblHoraInicio;
        private Label lblHoraFin;
        private DateTimePicker dtpHoraInicio;
        private DateTimePicker dtpHoraFin;
        private Button btnRegistrar;
        private Button btnModificar;
        private Button btnDesactivar;
        private Button btnActivar;
        private Button btnCargar;
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
            grpDatos = new GroupBox();
            lblNombreCancha = new Label();
            txtNombreCancha = new TextBox();
            lblHoraInicio = new Label();
            dtpHoraInicio = new DateTimePicker();
            lblHoraFin = new Label();
            dtpHoraFin = new DateTimePicker();
            btnRegistrar = new Button();
            btnModificar = new Button();
            btnDesactivar = new Button();
            btnActivar = new Button();
            btnCargar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvCanchas).BeginInit();
            grpDatos.SuspendLayout();
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
            dgvCanchas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanchas.Size = new Size(656, 250);
            dgvCanchas.SelectionChanged += dgvCanchas_SelectionChanged;

            grpDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblNombreCancha);
            grpDatos.Controls.Add(txtNombreCancha);
            grpDatos.Controls.Add(lblHoraInicio);
            grpDatos.Controls.Add(dtpHoraInicio);
            grpDatos.Controls.Add(lblHoraFin);
            grpDatos.Controls.Add(dtpHoraFin);
            grpDatos.Controls.Add(btnRegistrar);
            grpDatos.Controls.Add(btnModificar);
            grpDatos.Controls.Add(btnDesactivar);
            grpDatos.Controls.Add(btnActivar);
            grpDatos.Controls.Add(btnCargar);
            grpDatos.Location = new Point(16, 278);
            grpDatos.Size = new Size(656, 128);
            grpDatos.Text = "Datos de la cancha";

            lblNombreCancha.AutoSize = true;
            lblNombreCancha.Location = new Point(16, 32);
            lblNombreCancha.Text = "Nombre";

            txtNombreCancha.Location = new Point(19, 52);
            txtNombreCancha.MaxLength = ValoresDominio.LongitudMaximaNombreCancha;
            txtNombreCancha.Size = new Size(220, 23);
            txtNombreCancha.TabIndex = 0;

            lblHoraInicio.AutoSize = true;
            lblHoraInicio.Location = new Point(256, 32);
            lblHoraInicio.Text = "Abre";
            dtpHoraInicio.Format = DateTimePickerFormat.Custom;
            dtpHoraInicio.CustomFormat = "HH:mm";
            dtpHoraInicio.ShowUpDown = true;
            dtpHoraInicio.Location = new Point(259, 52);
            dtpHoraInicio.Size = new Size(80, 23);
            dtpHoraInicio.TabIndex = 1;

            lblHoraFin.AutoSize = true;
            lblHoraFin.Location = new Point(352, 32);
            lblHoraFin.Text = "Cierra";
            dtpHoraFin.Format = DateTimePickerFormat.Custom;
            dtpHoraFin.CustomFormat = "HH:mm";
            dtpHoraFin.ShowUpDown = true;
            dtpHoraFin.Location = new Point(355, 52);
            dtpHoraFin.Size = new Size(80, 23);
            dtpHoraFin.TabIndex = 2;

            btnRegistrar.Location = new Point(19, 88);
            btnRegistrar.Size = new Size(100, 28);
            btnRegistrar.TabIndex = 3;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnModificar.Location = new Point(125, 88);
            btnModificar.Size = new Size(100, 28);
            btnModificar.TabIndex = 4;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnDesactivar.Location = new Point(231, 88);
            btnDesactivar.Size = new Size(100, 28);
            btnDesactivar.TabIndex = 5;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnActivar.Enabled = false;
            btnActivar.Location = new Point(337, 88);
            btnActivar.Size = new Size(100, 28);
            btnActivar.TabIndex = 6;
            btnActivar.Text = "Activar";
            btnActivar.UseVisualStyleBackColor = true;
            btnActivar.Click += btnActivar_Click;

            btnCargar.Location = new Point(443, 88);
            btnCargar.Size = new Size(120, 28);
            btnCargar.TabIndex = 7;
            btnCargar.Text = "Actualizar lista";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(688, 422);
            Controls.Add(grpDatos);
            Controls.Add(dgvCanchas);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(640, 400);
            Name = "FrmCanchas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Canchas";
            Load += FrmCanchas_Load;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvCanchas).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
