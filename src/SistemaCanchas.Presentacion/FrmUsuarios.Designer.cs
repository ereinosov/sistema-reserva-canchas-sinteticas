using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmUsuarios
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvUsuarios;
        private Label lblNombre;
        private Label lblUsuario;
        private Label lblClave;
        private Label lblRol;
        private TextBox txtNombreUsuario;
        private TextBox txtUsuarioLogin;
        private TextBox txtClaveApp;
        private ComboBox cboRol;
        private Button btnRegistrar;
        private Button btnDesactivar;
        private Button btnCargar;
        private GroupBox grpDatos;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colLogin;
        private DataGridViewTextBoxColumn colRol;
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
            dgvUsuarios = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colLogin = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            grpDatos = new GroupBox();
            lblNombre = new Label();
            txtNombreUsuario = new TextBox();
            lblUsuario = new Label();
            txtUsuarioLogin = new TextBox();
            lblClave = new Label();
            txtClaveApp = new TextBox();
            lblRol = new Label();
            cboRol = new ComboBox();
            btnRegistrar = new Button();
            btnDesactivar = new Button();
            btnCargar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            grpDatos.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;

            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colNombre.Width = 220;

            colLogin.HeaderText = "Usuario";
            colLogin.Name = "colLogin";
            colLogin.Width = 140;

            colRol.HeaderText = "Rol";
            colRol.Name = "colRol";
            colRol.Width = 120;

            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.Width = 100;

            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colLogin, colRol, colEstado });
            dgvUsuarios.Location = new Point(16, 16);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(736, 250);

            grpDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombreUsuario);
            grpDatos.Controls.Add(lblUsuario);
            grpDatos.Controls.Add(txtUsuarioLogin);
            grpDatos.Controls.Add(lblClave);
            grpDatos.Controls.Add(txtClaveApp);
            grpDatos.Controls.Add(lblRol);
            grpDatos.Controls.Add(cboRol);
            grpDatos.Controls.Add(btnRegistrar);
            grpDatos.Controls.Add(btnDesactivar);
            grpDatos.Controls.Add(btnCargar);
            grpDatos.Location = new Point(16, 278);
            grpDatos.Size = new Size(736, 164);
            grpDatos.Text = "Nuevo usuario";

            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 28);
            lblNombre.Text = "Nombre";

            txtNombreUsuario.Location = new Point(19, 48);
            txtNombreUsuario.MaxLength = ValoresDominio.LongitudMaximaNombreUsuario;
            txtNombreUsuario.Size = new Size(280, 23);
            txtNombreUsuario.TabIndex = 0;

            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(316, 28);
            lblUsuario.Text = "Usuario de acceso";

            txtUsuarioLogin.Location = new Point(319, 48);
            txtUsuarioLogin.MaxLength = ValoresDominio.LongitudMaximaUsuarioLogin;
            txtUsuarioLogin.Size = new Size(180, 23);
            txtUsuarioLogin.TabIndex = 1;

            lblClave.AutoSize = true;
            lblClave.Location = new Point(516, 28);
            lblClave.Text = "Clave";

            txtClaveApp.Location = new Point(519, 48);
            txtClaveApp.MaxLength = 128;
            txtClaveApp.Size = new Size(196, 23);
            txtClaveApp.TabIndex = 2;
            txtClaveApp.UseSystemPasswordChar = true;

            lblRol.AutoSize = true;
            lblRol.Location = new Point(16, 84);
            lblRol.Text = "Rol";

            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Location = new Point(19, 104);
            cboRol.Size = new Size(180, 23);
            cboRol.TabIndex = 3;

            btnRegistrar.Location = new Point(319, 102);
            btnRegistrar.Size = new Size(110, 28);
            btnRegistrar.TabIndex = 4;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnDesactivar.Location = new Point(435, 102);
            btnDesactivar.Size = new Size(110, 28);
            btnDesactivar.TabIndex = 5;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnCargar.Location = new Point(551, 102);
            btnCargar.Size = new Size(110, 28);
            btnCargar.TabIndex = 6;
            btnCargar.Text = "Actualizar lista";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(768, 462);
            Controls.Add(grpDatos);
            Controls.Add(dgvUsuarios);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(720, 420);
            Name = "FrmUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuarios";
            Load += FrmUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
