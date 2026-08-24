using System;
using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmLogin
    {
        private System.ComponentModel.IContainer components;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblUsuario;
        private Label lblClave;
        private Label lblMensaje;
        private TextBox txtUsuarioLogin;
        private TextBox txtClaveApp;
        private Button btnIngresar;
        private Button btnSalir;
        private Panel pnlEncabezado;
        private ErrorProvider errValidacion;
        private LinkLabel lnkPrimeraInstalacion;

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
            lblSubtitulo = new Label();
            lblUsuario = new Label();
            txtUsuarioLogin = new TextBox();
            lblClave = new Label();
            txtClaveApp = new TextBox();
            btnIngresar = new Button();
            btnSalir = new Button();
            lblMensaje = new Label();
            lnkPrimeraInstalacion = new LinkLabel();
            errValidacion = new ErrorProvider(components);
            pnlEncabezado.SuspendLayout();
            SuspendLayout();

            pnlEncabezado.BackColor = Color.FromArgb(25, 95, 65);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Height = 78;
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(lblTitulo);

            lblTitulo.AutoSize = false;
            lblTitulo.Location = new Point(0, 8);
            lblTitulo.Size = new Size(424, 36);
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Text = TextosUi.TituloAplicacion;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Padding = new Padding(16, 0, 16, 0);

            lblSubtitulo.AutoSize = false;
            lblSubtitulo.Location = new Point(0, 44);
            lblSubtitulo.Size = new Size(424, 28);
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(210, 235, 220);
            lblSubtitulo.Text = "Inicio de sesión";
            lblSubtitulo.Padding = new Padding(16, 0, 16, 0);

            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9F);
            lblUsuario.Location = new Point(28, 100);
            lblUsuario.Text = "Usuario";

            txtUsuarioLogin.Font = new Font("Segoe UI", 10F);
            txtUsuarioLogin.Location = new Point(31, 122);
            txtUsuarioLogin.MaxLength = ValoresDominio.LongitudMaximaUsuarioLogin;
            txtUsuarioLogin.Size = new Size(360, 25);
            txtUsuarioLogin.TabIndex = 0;
            txtUsuarioLogin.TextChanged += CamposEntrada_TextChanged;

            lblClave.AutoSize = true;
            lblClave.Font = new Font("Segoe UI", 9F);
            lblClave.Location = new Point(28, 162);
            lblClave.Text = "Clave";

            txtClaveApp.Font = new Font("Segoe UI", 10F);
            txtClaveApp.Location = new Point(31, 184);
            txtClaveApp.MaxLength = 128;
            txtClaveApp.Size = new Size(360, 25);
            txtClaveApp.TabIndex = 1;
            txtClaveApp.UseSystemPasswordChar = true;
            txtClaveApp.TextChanged += CamposEntrada_TextChanged;

            btnIngresar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnIngresar.Location = new Point(196, 236);
            btnIngresar.Size = new Size(100, 32);
            btnIngresar.TabIndex = 2;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;

            btnSalir.Font = new Font("Segoe UI", 9F);
            btnSalir.Location = new Point(302, 236);
            btnSalir.Size = new Size(90, 32);
            btnSalir.TabIndex = 3;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;

            lblMensaje.AutoSize = false;
            lblMensaje.Font = new Font("Segoe UI", 8.5F);
            lblMensaje.Location = new Point(31, 276);
            lblMensaje.Size = new Size(360, 36);
            lblMensaje.Text = string.Empty;

            lnkPrimeraInstalacion.AutoSize = true;
            lnkPrimeraInstalacion.Location = new Point(31, 318);
            lnkPrimeraInstalacion.TabIndex = 4;
            lnkPrimeraInstalacion.TabStop = true;
            lnkPrimeraInstalacion.Text = "Primera configuración (administrador inicial)";
            lnkPrimeraInstalacion.LinkClicked += lnkPrimeraInstalacion_LinkClicked;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnSalir;
            ClientSize = new Size(424, 358);
            Controls.Add(lnkPrimeraInstalacion);
            Controls.Add(lblMensaje);
            Controls.Add(btnSalir);
            Controls.Add(btnIngresar);
            Controls.Add(txtClaveApp);
            Controls.Add(lblClave);
            Controls.Add(txtUsuarioLogin);
            Controls.Add(lblUsuario);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = TextosUi.TituloAplicacion;
            Load += FrmLogin_Load;
            pnlEncabezado.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
