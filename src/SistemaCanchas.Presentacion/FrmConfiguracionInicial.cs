using System;
using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmConfiguracionInicial : Form
    {
        private readonly IUsuarioService _usuarioService;

        public string UsuarioCreado { get; private set; }

        public FrmConfiguracionInicial(IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _usuarioService = usuarioService;
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (!ValidarFormato())
            {
                return;
            }

            Cursor cursorAnterior = Cursor.Current;
            btnCrear.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                _usuarioService.RegistrarAdministradorInicial(
                    txtNombreUsuario.Text,
                    txtUsuarioLogin.Text,
                    txtClaveApp.Text);

                UsuarioCreado = txtUsuarioLogin.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ValidacionNegocioException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ErrorInfraestructuraException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor.Current = cursorAnterior;
                btnCrear.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidarFormato()
        {
            errValidacion.Clear();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                errValidacion.SetError(txtNombreUsuario, "Ingrese el nombre.");
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtUsuarioLogin.Text))
            {
                errValidacion.SetError(txtUsuarioLogin, "Ingrese el usuario.");
                valido = false;
            }

            if (string.IsNullOrEmpty(txtClaveApp.Text))
            {
                errValidacion.SetError(txtClaveApp, "Ingrese la clave.");
                valido = false;
            }
            else if (txtClaveApp.Text.Length < ValoresDominio.LongitudMinimaClaveApp)
            {
                errValidacion.SetError(txtClaveApp, "Mínimo 8 caracteres.");
                valido = false;
            }

            if (txtClaveApp.Text != txtClaveConfirmacion.Text)
            {
                errValidacion.SetError(txtClaveConfirmacion, "Las claves no coinciden.");
                valido = false;
            }

            return valido;
        }
    }
}
