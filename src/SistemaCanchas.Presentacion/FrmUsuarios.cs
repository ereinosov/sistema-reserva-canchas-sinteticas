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
        private int _idSeleccionado;
        private string _loginSeleccionado;

        public FrmUsuarios(IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _usuarioService = usuarioService;
            InitializeComponent();
            TextosUi.ConfigurarGrilla(dgvUsuarios);
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            cboRol.Items.Add(new ItemRol("Empleado", ValoresDominio.Rol.Empleado));
            cboRol.Items.Add(new ItemRol("Administrador", ValoresDominio.Rol.Administrador));
            cboRol.SelectedIndex = 0;
            CargarUsuarios();
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarPanelEdicion();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            MostrarSeleccion(dgvUsuarios.Rows[e.RowIndex]);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarAlta())
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
                    txtNombreNuevo.Text,
                    txtUsuarioLogin.Text,
                    txtClaveApp.Text,
                    rol.Valor);

                MessageBox.Show(
                    "Usuario registrado. Ya puede iniciar sesión con esa cuenta.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarAlta();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un usuario de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string login = _loginSeleccionado;
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
                _usuarioService.DesactivarUsuario(_idSeleccionado);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un usuario de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                _usuarioService.ActivarUsuario(_idSeleccionado);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnGuardarNombre_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un usuario de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            errValidacion.Clear();
            if (string.IsNullOrWhiteSpace(txtNombreEdicion.Text))
            {
                errValidacion.SetError(txtNombreEdicion, "Ingrese el nombre.");
                return;
            }

            try
            {
                _usuarioService.ActualizarNombreUsuario(_idSeleccionado, txtNombreEdicion.Text);
                MessageBox.Show(
                    "Nombre actualizado.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Seleccione un usuario de la lista.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            errValidacion.Clear();
            if (string.IsNullOrEmpty(txtClaveNueva.Text) || txtClaveNueva.Text.Length < ValoresDominio.LongitudMinimaClaveApp)
            {
                errValidacion.SetError(txtClaveNueva, "La clave debe tener al menos 8 caracteres.");
                return;
            }

            if (!string.Equals(txtClaveNueva.Text, txtConfirmarClave.Text, StringComparison.Ordinal))
            {
                errValidacion.SetError(txtConfirmarClave, "La confirmación no coincide con la clave nueva.");
                return;
            }

            try
            {
                _usuarioService.CambiarClaveUsuario(_idSeleccionado, txtClaveNueva.Text);
                txtClaveNueva.Clear();
                txtConfirmarClave.Clear();
                MessageBox.Show(
                    "Clave actualizada.",
                    TextosUi.TituloAplicacion,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
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
                dgvUsuarios.SelectionChanged -= dgvUsuarios_SelectionChanged;
                try
                {
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

                    TextosUi.QuitarSeleccionGrilla(dgvUsuarios);
                    MostrarSinSeleccion();
                }
                finally
                {
                    dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void ActualizarPanelEdicion()
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.Index < 0 ||
                dgvUsuarios.SelectedRows.Count == 0)
            {
                MostrarSinSeleccion();
                return;
            }

            MostrarSeleccion(dgvUsuarios.CurrentRow);
        }

        private void MostrarSinSeleccion()
        {
            _idSeleccionado = 0;
            _loginSeleccionado = null;
            grpSeleccionado.Enabled = false;
            lblSeleccionado.Text = "Seleccione un usuario de la lista para editarlo.";
            txtNombreEdicion.Clear();
            txtClaveNueva.Clear();
            txtConfirmarClave.Clear();
        }

        private void MostrarSeleccion(DataGridViewRow fila)
        {
            _idSeleccionado = Convert.ToInt32(fila.Cells["colId"].Value);
            string nombre = Convert.ToString(fila.Cells["colNombre"].Value);
            _loginSeleccionado = Convert.ToString(fila.Cells["colLogin"].Value);
            string rol = Convert.ToString(fila.Cells["colRol"].Value);
            string estado = Convert.ToString(fila.Cells["colEstado"].Value);
            bool inactivo = string.Equals(estado, ValoresDominio.EstadoUsuario.Inactivo, StringComparison.Ordinal);

            grpSeleccionado.Enabled = true;
            lblSeleccionado.Text = "Editando: " + nombre + " (" + _loginSeleccionado + ") — " + rol + ", " + estado + ".";
            txtNombreEdicion.Text = nombre;
            btnActivar.Enabled = inactivo;
            btnDesactivar.Enabled = !inactivo;
        }

        private bool ValidarAlta()
        {
            errValidacion.Clear();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombreNuevo.Text))
            {
                errValidacion.SetError(txtNombreNuevo, "Ingrese el nombre.");
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

        private void LimpiarAlta()
        {
            txtNombreNuevo.Clear();
            txtUsuarioLogin.Clear();
            txtClaveApp.Clear();
            cboRol.SelectedIndex = 0;
            txtNombreNuevo.Focus();
        }

        private static void MostrarError(Exception ex)
        {
            if (ex is ValidacionNegocioException ||
                ex is OperacionNoPermitidaException ||
                ex is ErrorInfraestructuraException)
            {
                MessageBox.Show(ex.Message, TextosUi.TituloAplicacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            throw ex;
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
