using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components;
        private MenuStrip mnuPrincipal;
        private ToolStripMenuItem mnuArchivo;
        private ToolStripMenuItem mnuCerrarSesion;
        private ToolStripMenuItem mnuSalir;
        private ToolStripMenuItem mnuGestion;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuReservas;
        private ToolStripMenuItem mnuPagos;
        private ToolStripMenuItem mnuConsultas;
        private ToolStripMenuItem mnuDisponibilidad;
        private ToolStripMenuItem mnuIngresos;
        private ToolStripMenuItem mnuAdministracion;
        private ToolStripMenuItem mnuCanchas;
        private ToolStripMenuItem mnuUsuarios;
        private StatusStrip staEstado;
        private ToolStripStatusLabel slblUsuario;
        private ToolStripStatusLabel slblRol;
        private Label lblBienvenida;

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
            mnuPrincipal = new MenuStrip();
            mnuArchivo = new ToolStripMenuItem();
            mnuCerrarSesion = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            mnuGestion = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuReservas = new ToolStripMenuItem();
            mnuPagos = new ToolStripMenuItem();
            mnuConsultas = new ToolStripMenuItem();
            mnuDisponibilidad = new ToolStripMenuItem();
            mnuIngresos = new ToolStripMenuItem();
            mnuAdministracion = new ToolStripMenuItem();
            mnuCanchas = new ToolStripMenuItem();
            mnuUsuarios = new ToolStripMenuItem();
            staEstado = new StatusStrip();
            slblUsuario = new ToolStripStatusLabel();
            slblRol = new ToolStripStatusLabel();
            lblBienvenida = new Label();
            mnuPrincipal.SuspendLayout();
            staEstado.SuspendLayout();
            SuspendLayout();

            mnuArchivo.Text = "&Archivo";
            mnuArchivo.DropDownItems.AddRange(new ToolStripItem[] { mnuCerrarSesion, mnuSalir });

            mnuCerrarSesion.Text = "Cerrar &sesión";
            mnuCerrarSesion.Click += mnuCerrarSesion_Click;

            mnuSalir.Text = "&Salir";
            mnuSalir.Click += mnuSalir_Click;

            mnuGestion.Text = "&Gestión";
            mnuGestion.DropDownItems.AddRange(new ToolStripItem[] { mnuClientes, mnuReservas, mnuPagos });

            mnuClientes.Text = "&Clientes";
            mnuClientes.Click += mnuClientes_Click;

            mnuReservas.Text = "&Reservas";
            mnuReservas.Click += mnuReservas_Click;

            mnuPagos.Text = "&Pagos";
            mnuPagos.Click += mnuPagos_Click;

            mnuConsultas.Text = "&Consultas";
            mnuConsultas.DropDownItems.AddRange(new ToolStripItem[] { mnuDisponibilidad, mnuIngresos });

            mnuDisponibilidad.Text = "&Disponibilidad";
            mnuDisponibilidad.Click += mnuDisponibilidad_Click;

            mnuIngresos.Text = "&Ingresos";
            mnuIngresos.Click += mnuIngresos_Click;

            mnuAdministracion.Text = "A&dministración";
            mnuAdministracion.DropDownItems.AddRange(new ToolStripItem[] { mnuCanchas, mnuUsuarios });

            mnuCanchas.Text = "Can&chas";
            mnuCanchas.Click += mnuCanchas_Click;

            mnuUsuarios.Text = "&Usuarios";
            mnuUsuarios.Click += mnuUsuarios_Click;

            mnuPrincipal.Items.AddRange(new ToolStripItem[]
            {
                mnuArchivo, mnuGestion, mnuConsultas, mnuAdministracion
            });
            mnuPrincipal.Location = new Point(0, 0);
            mnuPrincipal.Name = "mnuPrincipal";

            slblUsuario.Name = "slblUsuario";
            slblUsuario.Spring = true;
            slblUsuario.Text = "Usuario";
            slblUsuario.TextAlign = ContentAlignment.MiddleLeft;

            slblRol.Name = "slblRol";
            slblRol.Text = "Rol";

            staEstado.Items.AddRange(new ToolStripItem[] { slblUsuario, slblRol });
            staEstado.Name = "staEstado";
            staEstado.SizingGrip = false;

            lblBienvenida.AutoSize = false;
            lblBienvenida.Dock = DockStyle.Fill;
            lblBienvenida.Font = new Font("Segoe UI", 12F);
            lblBienvenida.ForeColor = Color.FromArgb(50, 70, 60);
            lblBienvenida.Text = "Sesión iniciada." + System.Environment.NewLine + System.Environment.NewLine +
                                 "Todos los módulos están disponibles: Clientes, Reservas, Pagos, Disponibilidad, Ingresos, Canchas y Usuarios.";
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            lblBienvenida.Padding = new Padding(24);

            MainMenuStrip = mnuPrincipal;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 520);
            Controls.Add(lblBienvenida);
            Controls.Add(staEstado);
            Controls.Add(mnuPrincipal);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(640, 400);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = TextosUi.TituloAplicacion;
            Load += FrmPrincipal_Load;
            mnuPrincipal.ResumeLayout(false);
            mnuPrincipal.PerformLayout();
            staEstado.ResumeLayout(false);
            staEstado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
