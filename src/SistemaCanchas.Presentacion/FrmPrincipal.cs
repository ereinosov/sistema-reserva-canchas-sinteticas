using System;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly Usuario _sesion;

        public FrmPrincipal(IUsuarioService usuarioService, Usuario sesion)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            if (sesion == null)
            {
                throw new ArgumentNullException(nameof(sesion));
            }

            _usuarioService = usuarioService;
            _sesion = sesion;
            InitializeComponent();
            ConfigurarSegunRol();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            slblUsuario.Text = "Usuario: " + _sesion.NombreUsuario;
            slblRol.Text = "Rol: " + _sesion.NombreRol;
        }

        private void ConfigurarSegunRol()
        {
            bool esAdministrador = string.Equals(
                _sesion.NombreRol,
                ValoresDominio.Rol.Administrador,
                StringComparison.Ordinal);

            // A1 §2.3: el empleado no gestiona canchas, usuarios ni consulta ingresos.
            mnuCanchas.Visible = esAdministrador;
            mnuUsuarios.Visible = esAdministrador;
            mnuIngresos.Visible = esAdministrador;
            mnuAdministracion.Visible = esAdministrador;
        }

        private void mnuCerrarSesion_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuUsuarios_Click(object sender, EventArgs e)
        {
            using (FrmUsuarios formulario = new FrmUsuarios(_usuarioService))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuCanchas_Click(object sender, EventArgs e)
        {
            using (FrmCanchas formulario = new FrmCanchas(new CanchaService(_usuarioService)))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuClientes_Click(object sender, EventArgs e)
        {
            bool puedeEliminar = string.Equals(
                _sesion.NombreRol,
                ValoresDominio.Rol.Administrador,
                StringComparison.Ordinal);

            using (FrmClientes formulario = new FrmClientes(new ClienteService(_usuarioService), puedeEliminar))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuReservas_Click(object sender, EventArgs e)
        {
            using (FrmReservas formulario = new FrmReservas(
                new ReservaService(_usuarioService),
                new ClienteService(_usuarioService),
                new CanchaService(_usuarioService)))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuDisponibilidad_Click(object sender, EventArgs e)
        {
            using (FrmDisponibilidad formulario = new FrmDisponibilidad(
                new ReservaService(_usuarioService),
                new CanchaService(_usuarioService)))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuPagos_Click(object sender, EventArgs e)
        {
            using (FrmPagos formulario = new FrmPagos(new PagoService(_usuarioService)))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuIngresos_Click(object sender, EventArgs e)
        {
            using (FrmIngresos formulario = new FrmIngresos(new IngresoService(_usuarioService)))
            {
                formulario.ShowDialog(this);
            }
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
