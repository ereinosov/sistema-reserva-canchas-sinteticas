using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmUsuarios
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvUsuarios;
        private Button btnCargar;
        private GroupBox grpNuevo;
        private Label lblNombreNuevo;
        private Label lblUsuarioNuevo;
        private Label lblClaveNuevo;
        private Label lblRol;
        private TextBox txtNombreNuevo;
        private TextBox txtUsuarioLogin;
        private TextBox txtClaveApp;
        private ComboBox cboRol;
        private Button btnRegistrar;
        private GroupBox grpSeleccionado;
        private Label lblSeleccionado;
        private Label lblNombreEdicion;
        private TextBox txtNombreEdicion;
        private Button btnGuardarNombre;
        private Label lblClaveNueva;
        private Label lblConfirmarClave;
        private TextBox txtClaveNueva;
        private TextBox txtConfirmarClave;
        private Button btnCambiarClave;
        private Button btnDesactivar;
        private Button btnActivar;
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
            btnCargar = new Button();
            grpNuevo = new GroupBox();
            lblNombreNuevo = new Label();
            txtNombreNuevo = new TextBox();
            lblUsuarioNuevo = new Label();
            txtUsuarioLogin = new TextBox();
            lblClaveNuevo = new Label();
            txtClaveApp = new TextBox();
            lblRol = new Label();
            cboRol = new ComboBox();
            btnRegistrar = new Button();
            grpSeleccionado = new GroupBox();
            lblSeleccionado = new Label();
            lblNombreEdicion = new Label();
            txtNombreEdicion = new TextBox();
            btnGuardarNombre = new Button();
            lblClaveNueva = new Label();
            txtClaveNueva = new TextBox();
            lblConfirmarClave = new Label();
            txtConfirmarClave = new TextBox();
            btnCambiarClave = new Button();
            btnDesactivar = new Button();
            btnActivar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            grpNuevo.SuspendLayout();
            grpSeleccionado.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;

            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colNombre.FillWeight = 36F;

            colLogin.HeaderText = "Usuario";
            colLogin.Name = "colLogin";
            colLogin.FillWeight = 24F;

            colRol.HeaderText = "Rol";
            colRol.Name = "colRol";
            colRol.FillWeight = 20F;

            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.FillWeight = 20F;

            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeColumns = false;
            dgvUsuarios.AllowUserToResizeRows = false;
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
            dgvUsuarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(748, 200);
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;

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
            grpNuevo.Controls.Add(lblUsuarioNuevo);
            grpNuevo.Controls.Add(txtUsuarioLogin);
            grpNuevo.Controls.Add(lblClaveNuevo);
            grpNuevo.Controls.Add(txtClaveApp);
            grpNuevo.Controls.Add(lblRol);
            grpNuevo.Controls.Add(cboRol);
            grpNuevo.Controls.Add(btnRegistrar);
            grpNuevo.Location = new Point(16, 228);
            grpNuevo.Size = new Size(428, 214);
            grpNuevo.Text = "Nuevo usuario";

            lblNombreNuevo.AutoSize = true;
            lblNombreNuevo.Location = new Point(16, 28);
            lblNombreNuevo.Text = "Nombre";
            txtNombreNuevo.Location = new Point(19, 48);
            txtNombreNuevo.MaxLength = ValoresDominio.LongitudMaximaNombreUsuario;
            txtNombreNuevo.Size = new Size(390, 23);
            txtNombreNuevo.TabIndex = 1;

            lblUsuarioNuevo.AutoSize = true;
            lblUsuarioNuevo.Location = new Point(16, 80);
            lblUsuarioNuevo.Text = "Usuario de acceso";
            txtUsuarioLogin.Location = new Point(19, 100);
            txtUsuarioLogin.MaxLength = ValoresDominio.LongitudMaximaUsuarioLogin;
            txtUsuarioLogin.Size = new Size(190, 23);
            txtUsuarioLogin.TabIndex = 2;

            lblClaveNuevo.AutoSize = true;
            lblClaveNuevo.Location = new Point(216, 80);
            lblClaveNuevo.Text = "Clave";
            txtClaveApp.Location = new Point(219, 100);
            txtClaveApp.MaxLength = 128;
            txtClaveApp.Size = new Size(190, 23);
            txtClaveApp.TabIndex = 3;
            txtClaveApp.UseSystemPasswordChar = true;

            lblRol.AutoSize = true;
            lblRol.Location = new Point(16, 132);
            lblRol.Text = "Rol";
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Location = new Point(19, 152);
            cboRol.Size = new Size(190, 23);
            cboRol.TabIndex = 4;

            btnRegistrar.Location = new Point(219, 150);
            btnRegistrar.Size = new Size(190, 28);
            btnRegistrar.TabIndex = 5;
            btnRegistrar.Text = "Registrar usuario";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            grpSeleccionado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpSeleccionado.Controls.Add(lblSeleccionado);
            grpSeleccionado.Controls.Add(lblNombreEdicion);
            grpSeleccionado.Controls.Add(txtNombreEdicion);
            grpSeleccionado.Controls.Add(btnGuardarNombre);
            grpSeleccionado.Controls.Add(lblClaveNueva);
            grpSeleccionado.Controls.Add(txtClaveNueva);
            grpSeleccionado.Controls.Add(lblConfirmarClave);
            grpSeleccionado.Controls.Add(txtConfirmarClave);
            grpSeleccionado.Controls.Add(btnCambiarClave);
            grpSeleccionado.Controls.Add(btnDesactivar);
            grpSeleccionado.Controls.Add(btnActivar);
            grpSeleccionado.Enabled = false;
            grpSeleccionado.Location = new Point(452, 228);
            grpSeleccionado.Size = new Size(436, 214);
            grpSeleccionado.Text = "Editar usuario seleccionado";

            lblSeleccionado.AutoSize = false;
            lblSeleccionado.Location = new Point(16, 24);
            lblSeleccionado.Size = new Size(404, 32);
            lblSeleccionado.Text = "Seleccione un usuario de la lista para editarlo.";

            lblNombreEdicion.AutoSize = true;
            lblNombreEdicion.Location = new Point(16, 60);
            lblNombreEdicion.Text = "Nombre";
            txtNombreEdicion.Location = new Point(19, 80);
            txtNombreEdicion.MaxLength = ValoresDominio.LongitudMaximaNombreUsuario;
            txtNombreEdicion.Size = new Size(250, 23);
            txtNombreEdicion.TabIndex = 6;

            btnGuardarNombre.Location = new Point(275, 78);
            btnGuardarNombre.Size = new Size(140, 28);
            btnGuardarNombre.TabIndex = 7;
            btnGuardarNombre.Text = "Guardar nombre";
            btnGuardarNombre.UseVisualStyleBackColor = true;
            btnGuardarNombre.Click += btnGuardarNombre_Click;

            lblClaveNueva.AutoSize = true;
            lblClaveNueva.Location = new Point(16, 112);
            lblClaveNueva.Text = "Clave nueva";
            txtClaveNueva.Location = new Point(19, 132);
            txtClaveNueva.MaxLength = 128;
            txtClaveNueva.Size = new Size(180, 23);
            txtClaveNueva.TabIndex = 8;
            txtClaveNueva.UseSystemPasswordChar = true;

            lblConfirmarClave.AutoSize = true;
            lblConfirmarClave.Location = new Point(206, 112);
            lblConfirmarClave.Text = "Confirmar clave";
            txtConfirmarClave.Location = new Point(209, 132);
            txtConfirmarClave.MaxLength = 128;
            txtConfirmarClave.Size = new Size(206, 23);
            txtConfirmarClave.TabIndex = 9;
            txtConfirmarClave.UseSystemPasswordChar = true;

            btnCambiarClave.Location = new Point(19, 168);
            btnCambiarClave.Size = new Size(130, 28);
            btnCambiarClave.TabIndex = 10;
            btnCambiarClave.Text = "Cambiar clave";
            btnCambiarClave.UseVisualStyleBackColor = true;
            btnCambiarClave.Click += btnCambiarClave_Click;

            btnDesactivar.Location = new Point(155, 168);
            btnDesactivar.Size = new Size(120, 28);
            btnDesactivar.TabIndex = 11;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;

            btnActivar.Enabled = false;
            btnActivar.Location = new Point(281, 168);
            btnActivar.Size = new Size(134, 28);
            btnActivar.TabIndex = 12;
            btnActivar.Text = "Activar";
            btnActivar.UseVisualStyleBackColor = true;
            btnActivar.Click += btnActivar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(904, 458);
            Controls.Add(grpSeleccionado);
            Controls.Add(grpNuevo);
            Controls.Add(btnCargar);
            Controls.Add(dgvUsuarios);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(820, 520);
            Name = "FrmUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuarios";
            Load += FrmUsuarios_Load;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            grpNuevo.ResumeLayout(false);
            grpNuevo.PerformLayout();
            grpSeleccionado.ResumeLayout(false);
            grpSeleccionado.PerformLayout();
            ResumeLayout(false);
        }
    }
}
