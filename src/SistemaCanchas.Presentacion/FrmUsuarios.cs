using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmUsuarios : Form
    {
        private readonly IUsuarioService _usuarioService;

        public FrmUsuarios(IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _usuarioService = usuarioService;
            InitializeComponent();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            cboRol.Items.Add(new ItemRol("Empleado", ValoresDominio.Rol.Empleado));
            cboRol.Items.Add(new ItemRol("Administrador", ValoresDominio.Rol.Administrador));
            cboRol.SelectedIndex = 0;
            CargarUsuarios();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormato())
            {
                return;
            }

            ItemRol rol = cboRol.SelectedItem as ItemRol;
            if (rol == null)
            {
                errValidacion.SetError(cboRol, "Seleccione un rol.");
                return;
            }

            try
            {
                _usuarioService.RegistrarUsuario(
                    txtNombreUsuario.Text,
                    txtUsuarioLogin.Text,
                    txtClaveApp.Text,
                    rol.Valor);

                MessageBox.Show(
                    "Usuario registrado. Ya puede iniciar sesión con esa cuenta.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarUsuarios();
            }
            catch (ValidacionNegocioException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (OperacionNoPermitidaException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ErrorInfraestructuraException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione un usuario de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            string login = Convert.ToString(dgvUsuarios.CurrentRow.Cells["colLogin"].Value);

            DialogResult confirmacion = MessageBox.Show(
                "¿Desactivar al usuario \"" + login + "\"? No podrá iniciar sesión y el historial se conserva.",
                TextosUi.TituloAplicacion,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _usuarioService.DesactivarUsuario(idUsuario);
                CargarUsuarios();
            }
            catch (ValidacionNegocioException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (OperacionNoPermitidaException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ErrorInfraestructuraException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                IList<Usuario> usuarios = _usuarioService.ObtenerTodos();
                dgvUsuarios.Rows.Clear();
                for (int i = 0; i < usuarios.Count; i++)
                {
                    Usuario usuario = usuarios[i];
                    dgvUsuarios.Rows.Add(
                        usuario.IdUsuario,
                        usuario.NombreUsuario,
                        usuario.UsuarioLogin,
                        usuario.NombreRol,
                        usuario.EstadoUsuario);
                }
            }
            catch (ErrorInfraestructuraException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (OperacionNoPermitidaException ex)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

            if (string.IsNullOrEmpty(txtClaveApp.Text) || txtClaveApp.Text.Length < ValoresDominio.LongitudMinimaClaveApp)
            {
                errValidacion.SetError(txtClaveApp, "La clave debe tener al menos 8 caracteres.");
                valido = false;
            }

            return valido;
        }

        private void LimpiarFormulario()
        {
            txtNombreUsuario.Clear();
            txtUsuarioLogin.Clear();
            txtClaveApp.Clear();
            cboRol.SelectedIndex = 0;
            txtNombreUsuario.Focus();
        }

        private sealed class ItemRol
        {
            internal ItemRol(string texto, string valor)
            {
                Texto = texto;
                Valor = valor;
            }

            internal string Texto { get; private set; }

            internal string Valor { get; private set; }

            public override string ToString()
            {
                return Texto;
            }
        }
    }
}
