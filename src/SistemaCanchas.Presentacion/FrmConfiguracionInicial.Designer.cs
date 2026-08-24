using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmConfiguracionInicial
    {
        private System.ComponentModel.IContainer components;
        private Label lblNombre;
        private Label lblUsuario;
        private Label lblClave;
        private Label lblConfirmacion;
        private TextBox txtNombreUsuario;
        private TextBox txtUsuarioLogin;
        private TextBox txtClaveApp;
        private TextBox txtClaveConfirmacion;
        private Button btnCrear;
        private Button btnCancelar;
        private ErrorProvider errValidacion;
        private Panel pnlEncabezado;
        private Label lblTitulo;

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
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblNombre = new Label();
            txtNombreUsuario = new TextBox();
            lblUsuario = new Label();
            txtUsuarioLogin = new TextBox();
            lblClave = new Label();
            txtClaveApp = new TextBox();
            lblConfirmacion = new Label();
            txtClaveConfirmacion = new TextBox();
            btnCrear = new Button();
            btnCancelar = new Button();
            errValidacion = new ErrorProvider(components);
            pnlEncabezado.SuspendLayout();
            SuspendLayout();

            pnlEncabezado.BackColor = Color.FromArgb(25, 95, 65);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Height = 56;
            pnlEncabezado.Controls.Add(lblTitulo);

            lblTitulo.AutoSize = false;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Text = "Configuración inicial";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Padding = new Padding(16, 0, 16, 0);

            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(28, 72);
            lblNombre.Text = "Nombre";

            txtNombreUsuario.Location = new Point(31, 92);
            txtNombreUsuario.MaxLength = ValoresDominio.LongitudMaximaNombreUsuario;
            txtNombreUsuario.Size = new Size(380, 23);
            txtNombreUsuario.TabIndex = 0;

            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(28, 126);
            lblUsuario.Text = "Usuario de acceso";

            txtUsuarioLogin.Location = new Point(31, 146);
            txtUsuarioLogin.MaxLength = ValoresDominio.LongitudMaximaUsuarioLogin;
            txtUsuarioLogin.Size = new Size(380, 23);
            txtUsuarioLogin.TabIndex = 1;

            lblClave.AutoSize = true;
            lblClave.Location = new Point(28, 180);
            lblClave.Text = "Clave (mínimo 8 caracteres)";

            txtClaveApp.Location = new Point(31, 200);
            txtClaveApp.MaxLength = 128;
            txtClaveApp.Size = new Size(380, 23);
            txtClaveApp.TabIndex = 2;
            txtClaveApp.UseSystemPasswordChar = true;

            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(28, 234);
            lblConfirmacion.Text = "Confirmar clave";

            txtClaveConfirmacion.Location = new Point(31, 254);
            txtClaveConfirmacion.MaxLength = 128;
            txtClaveConfirmacion.Size = new Size(380, 23);
            txtClaveConfirmacion.TabIndex = 3;
            txtClaveConfirmacion.UseSystemPasswordChar = true;

            btnCrear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCrear.Location = new Point(211, 300);
            btnCrear.Size = new Size(110, 32);
            btnCrear.TabIndex = 4;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;

            btnCancelar.Location = new Point(327, 300);
            btnCancelar.Size = new Size(84, 32);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AcceptButton = btnCrear;
            CancelButton = btnCancelar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 354);
            Controls.Add(btnCancelar);
            Controls.Add(btnCrear);
            Controls.Add(txtClaveConfirmacion);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtClaveApp);
            Controls.Add(lblClave);
            Controls.Add(txtUsuarioLogin);
            Controls.Add(lblUsuario);
            Controls.Add(txtNombreUsuario);
            Controls.Add(lblNombre);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmConfiguracionInicial";
            StartPosition = FormStartPosition.CenterParent;
            Text = TextosUi.TituloAplicacion;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            pnlEncabezado.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
