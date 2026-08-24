using System;
using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmLogin : Form
    {
        private readonly IUsuarioService _usuarioService;

        public FrmLogin()
            : this(new UsuarioService())
        {
        }

        internal FrmLogin(IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _usuarioService = usuarioService;
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            ActualizarEstadoBotonIngresar();
            txtUsuarioLogin.Focus();
        }

        private void CamposEntrada_TextChanged(object sender, EventArgs e)
        {
            errValidacion.SetError(txtUsuarioLogin, string.Empty);
            errValidacion.SetError(txtClaveApp, string.Empty);
            lblMensaje.Text = string.Empty;
            ActualizarEstadoBotonIngresar();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormatoEntrada())
            {
                return;
            }

            Ingresar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lnkPrimeraInstalacion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmConfiguracionInicial configuracion = new FrmConfiguracionInicial(_usuarioService))
            {
                if (configuracion.ShowDialog(this) == DialogResult.OK)
                {
                    txtUsuarioLogin.Text = configuracion.UsuarioCreado;
                    txtClaveApp.Clear();
                    lblMensaje.ForeColor = Color.DarkGreen;
                    lblMensaje.Text = "Administrador creado. Ingrese la clave para iniciar sesión.";
                    txtClaveApp.Focus();
                }
            }
        }

        private bool ValidarFormatoEntrada()
        {
            bool valido = true;
            errValidacion.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuarioLogin.Text))
            {
                errValidacion.SetError(txtUsuarioLogin, "Ingrese el usuario.");
                valido = false;
            }
            else if (txtUsuarioLogin.Text.Trim().Length > ValoresDominio.LongitudMaximaUsuarioLogin)
            {
                errValidacion.SetError(txtUsuarioLogin, "El usuario no puede superar 30 caracteres.");
                valido = false;
            }

            if (string.IsNullOrEmpty(txtClaveApp.Text))
            {
                errValidacion.SetError(txtClaveApp, "Ingrese la clave.");
                valido = false;
            }

            return valido;
        }

        private void Ingresar()
        {
            Cursor cursorAnterior = Cursor.Current;
            btnIngresar.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Usuario sesion = _usuarioService.ValidarCredenciales(txtUsuarioLogin.Text, txtClaveApp.Text);
                AbrirPrincipal(sesion);
            }
            catch (CredencialesInvalidasException ex)
            {
                MostrarErrorLogin(ex.Message);
                txtClaveApp.Clear();
                txtClaveApp.Focus();
            }
            catch (UsuarioInactivoException ex)
            {
                MostrarErrorLogin(ex.Message);
            }
            catch (ErrorInfraestructuraException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor.Current = cursorAnterior;
                ActualizarEstadoBotonIngresar();
            }
        }

        private void AbrirPrincipal(Usuario sesion)
        {
            Hide();
            try
            {
                using (FrmPrincipal principal = new FrmPrincipal(_usuarioService, sesion))
                {
                    principal.ShowDialog(this);
                }
            }
            finally
            {
                _usuarioService.CerrarSesion();
                txtClaveApp.Clear();
                txtUsuarioLogin.Clear();
                lblMensaje.Text = string.Empty;
                Show();
                txtUsuarioLogin.Focus();
            }
        }

        private void MostrarErrorLogin(string mensaje)
        {
            lblMensaje.ForeColor = Color.Firebrick;
            lblMensaje.Text = mensaje;
        }

        private void ActualizarEstadoBotonIngresar()
        {
            btnIngresar.Enabled =
                !string.IsNullOrWhiteSpace(txtUsuarioLogin.Text) &&
                !string.IsNullOrEmpty(txtClaveApp.Text);
        }
    }
}
