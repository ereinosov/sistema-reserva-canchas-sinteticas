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
        private Button btnRegistrar;
        private Button btnModificar;
        private Button btnDesactivar;
        private Button btnActivar;
        private Button btnCargar;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colEstado;

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
            grpDatos = new GroupBox();
            lblNombreCancha = new Label();
            txtNombreCancha = new TextBox();
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
            colNombre.FillWeight = 70F;

            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.FillWeight = 30F;

            dgvCanchas.AllowUserToAddRows = false;
            dgvCanchas.AllowUserToDeleteRows = false;
            dgvCanchas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCanchas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCanchas.BackgroundColor = Color.White;
            dgvCanchas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCanchas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colEstado });
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
            txtNombreCancha.Size = new Size(300, 23);
            txtNombreCancha.TabIndex = 0;

            btnRegistrar.Location = new Point(19, 88);
            btnRegistrar.Size = new Size(100, 28);
            btnRegistrar.TabIndex = 1;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnModificar.Location = new Point(125, 88);
            btnModificar.Size = new Size(100, 28);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnDesactivar.Location = new Point(231, 88);
            btnDesactivar.Size = new Size(100, 28);
            btnDesactivar.TabIndex = 3;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnActivar.Enabled = false;
            btnActivar.Location = new Point(337, 88);
            btnActivar.Size = new Size(100, 28);
            btnActivar.TabIndex = 4;
            btnActivar.Text = "Activar";
            btnActivar.UseVisualStyleBackColor = true;
            btnActivar.Click += btnActivar_Click;

            btnCargar.Location = new Point(443, 88);
            btnCargar.Size = new Size(120, 28);
            btnCargar.TabIndex = 5;
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
