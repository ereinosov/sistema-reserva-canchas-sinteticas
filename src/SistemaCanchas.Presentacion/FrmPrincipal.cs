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
            AbrirModulo(typeof(FrmUsuarios), () => new FrmUsuarios(_usuarioService));
        }

        private void mnuCanchas_Click(object sender, EventArgs e)
        {
            AbrirModulo(typeof(FrmCanchas), () => new FrmCanchas(new CanchaService(_usuarioService)));
        }

        private void mnuClientes_Click(object sender, EventArgs e)
        {
            bool puedeEliminar = string.Equals(
                _sesion.NombreRol,
                ValoresDominio.Rol.Administrador,
                StringComparison.Ordinal);

            AbrirModulo(typeof(FrmClientes), () => new FrmClientes(new ClienteService(_usuarioService), puedeEliminar));
        }

        private void mnuReservas_Click(object sender, EventArgs e)
        {
            AbrirModulo(
                typeof(FrmReservas),
                () => new FrmReservas(
                    new ReservaService(_usuarioService),
                    new ClienteService(_usuarioService),
                    new CanchaService(_usuarioService)));
        }

        private void mnuDisponibilidad_Click(object sender, EventArgs e)
        {
            AbrirModulo(
                typeof(FrmDisponibilidad),
                () => new FrmDisponibilidad(
                    new ReservaService(_usuarioService),
                    new CanchaService(_usuarioService)));
        }

        private void mnuPagos_Click(object sender, EventArgs e)
        {
            AbrirModulo(
                typeof(FrmPagos),
                () => new FrmPagos(
                    new PagoService(_usuarioService),
                    new ClienteService(_usuarioService),
                    new CanchaService(_usuarioService)));
        }

        private void mnuIngresos_Click(object sender, EventArgs e)
        {
            AbrirModulo(typeof(FrmIngresos), () => new FrmIngresos(new IngresoService(_usuarioService)));
        }

        private void mnuCascada_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuMosaicoHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnuOrganizarIconos_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AbrirModulo(Type tipoFormulario, Func<Form> crear)
        {
            for (int i = 0; i < MdiChildren.Length; i++)
            {
                Form abierto = MdiChildren[i];
                if (abierto.GetType() == tipoFormulario)
                {
                    if (abierto.WindowState == FormWindowState.Minimized)
                    {
                        abierto.WindowState = FormWindowState.Normal;
                    }

                    abierto.Activate();
                    return;
                }
            }

            Form formulario = crear();
            formulario.MdiParent = this;
            formulario.Show();
        }
    }
}
