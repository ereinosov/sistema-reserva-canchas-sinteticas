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
        private Button btnActivar;
        private Button btnCargar;
        private Button btnGuardarNombre;
        private Button btnCambiarClave;
        private Label lblClaveNueva;
        private Label lblConfirmarClave;
        private TextBox txtClaveNueva;
        private TextBox txtConfirmarClave;
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
            btnActivar = new Button();
            btnCargar = new Button();
            lblClaveNueva = new Label();
            txtClaveNueva = new TextBox();
            lblConfirmarClave = new Label();
            txtConfirmarClave = new TextBox();
            btnGuardarNombre = new Button();
            btnCambiarClave = new Button();
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
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;

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
            grpDatos.Controls.Add(btnActivar);
            grpDatos.Controls.Add(btnCargar);
            grpDatos.Controls.Add(lblClaveNueva);
            grpDatos.Controls.Add(txtClaveNueva);
            grpDatos.Controls.Add(lblConfirmarClave);
            grpDatos.Controls.Add(txtConfirmarClave);
            grpDatos.Controls.Add(btnGuardarNombre);
            grpDatos.Controls.Add(btnCambiarClave);
            grpDatos.Location = new Point(16, 278);
            grpDatos.Size = new Size(736, 220);
            grpDatos.Text = "Datos del usuario";

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

            btnRegistrar.Location = new Point(211, 102);
            btnRegistrar.Size = new Size(100, 28);
            btnRegistrar.TabIndex = 4;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnDesactivar.Location = new Point(317, 102);
            btnDesactivar.Size = new Size(100, 28);
            btnDesactivar.TabIndex = 5;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnActivar.Enabled = false;
            btnActivar.Location = new Point(423, 102);
            btnActivar.Size = new Size(100, 28);
            btnActivar.TabIndex = 6;
            btnActivar.Text = "Activar";
            btnActivar.UseVisualStyleBackColor = true;
            btnActivar.Click += btnActivar_Click;

            btnCargar.Location = new Point(529, 102);
            btnCargar.Size = new Size(110, 28);
            btnCargar.TabIndex = 7;
            btnCargar.Text = "Actualizar lista";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            lblClaveNueva.AutoSize = true;
            lblClaveNueva.Location = new Point(16, 144);
            lblClaveNueva.Text = "Clave nueva";

            txtClaveNueva.Location = new Point(19, 164);
            txtClaveNueva.MaxLength = 128;
            txtClaveNueva.Size = new Size(180, 23);
            txtClaveNueva.TabIndex = 8;
            txtClaveNueva.UseSystemPasswordChar = true;

            lblConfirmarClave.AutoSize = true;
            lblConfirmarClave.Location = new Point(211, 144);
            lblConfirmarClave.Text = "Confirmar clave";

            txtConfirmarClave.Location = new Point(214, 164);
            txtConfirmarClave.MaxLength = 128;
            txtConfirmarClave.Size = new Size(180, 23);
            txtConfirmarClave.TabIndex = 9;
            txtConfirmarClave.UseSystemPasswordChar = true;

            btnGuardarNombre.Location = new Point(411, 162);
            btnGuardarNombre.Size = new Size(120, 28);
            btnGuardarNombre.TabIndex = 10;
            btnGuardarNombre.Text = "Guardar nombre";
            btnGuardarNombre.UseVisualStyleBackColor = true;
            btnGuardarNombre.Click += btnGuardarNombre_Click;

            btnCambiarClave.Location = new Point(537, 162);
            btnCambiarClave.Size = new Size(120, 28);
            btnCambiarClave.TabIndex = 11;
            btnCambiarClave.Text = "Cambiar clave";
            btnCambiarClave.UseVisualStyleBackColor = true;
            btnCambiarClave.Click += btnCambiarClave_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(768, 518);
            Controls.Add(grpDatos);
            Controls.Add(dgvUsuarios);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(720, 480);
            Name = "FrmUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuarios";
            Load += FrmUsuarios_Load;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
