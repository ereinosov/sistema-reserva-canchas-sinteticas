using System.Drawing;
using System.Windows.Forms;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmClientes
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvClientes;
        private GroupBox grpFiltros;
        private GroupBox grpNuevo;
        private GroupBox grpSeleccionado;
        private Label lblFiltroNombre;
        private Label lblFiltroDocumento;
        private TextBox txtFiltroNombre;
        private TextBox txtFiltroDocumento;
        private Button btnBuscar;
        private Button btnCargar;
        private Label lblNombreNuevo;
        private Label lblTipoNuevo;
        private Label lblNumeroNuevo;
        private Label lblTelefonoNuevo;
        private Label lblCorreoNuevo;
        private TextBox txtNombreNuevo;
        private ComboBox cboTipoNuevo;
        private TextBox txtNumeroNuevo;
        private TextBox txtTelefonoNuevo;
        private TextBox txtCorreoNuevo;
        private Button btnRegistrar;
        private Label lblSeleccionado;
        private Label lblNombreEdicion;
        private Label lblTipoEdicion;
        private Label lblNumeroEdicion;
        private Label lblTelefonoEdicion;
        private Label lblCorreoEdicion;
        private TextBox txtNombreEdicion;
        private ComboBox cboTipoEdicion;
        private TextBox txtNumeroEdicion;
        private TextBox txtTelefonoEdicion;
        private TextBox txtCorreoEdicion;
        private Button btnModificar;
        private Button btnEliminar;
        private ErrorProvider errValidacion;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colCorreo;

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
            dgvClientes = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colDocumento = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colCorreo = new DataGridViewTextBoxColumn();
            grpFiltros = new GroupBox();
            lblFiltroNombre = new Label();
            txtFiltroNombre = new TextBox();
            lblFiltroDocumento = new Label();
            txtFiltroDocumento = new TextBox();
            btnBuscar = new Button();
            btnCargar = new Button();
            grpNuevo = new GroupBox();
            lblNombreNuevo = new Label();
            txtNombreNuevo = new TextBox();
            lblTipoNuevo = new Label();
            cboTipoNuevo = new ComboBox();
            lblNumeroNuevo = new Label();
            txtNumeroNuevo = new TextBox();
            lblTelefonoNuevo = new Label();
            txtTelefonoNuevo = new TextBox();
            lblCorreoNuevo = new Label();
            txtCorreoNuevo = new TextBox();
            btnRegistrar = new Button();
            grpSeleccionado = new GroupBox();
            lblSeleccionado = new Label();
            lblNombreEdicion = new Label();
            txtNombreEdicion = new TextBox();
            lblTipoEdicion = new Label();
            cboTipoEdicion = new ComboBox();
            lblNumeroEdicion = new Label();
            txtNumeroEdicion = new TextBox();
            lblTelefonoEdicion = new Label();
            txtTelefonoEdicion = new TextBox();
            lblCorreoEdicion = new Label();
            txtCorreoEdicion = new TextBox();
            btnModificar = new Button();
            btnEliminar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            grpFiltros.SuspendLayout();
            grpNuevo.SuspendLayout();
            grpSeleccionado.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colTipo.HeaderText = "Tipo";
            colTipo.Name = "colTipo";
            colDocumento.HeaderText = "Documento";
            colDocumento.Name = "colDocumento";
            colTelefono.HeaderText = "Teléfono";
            colTelefono.Name = "colTelefono";
            colCorreo.HeaderText = "Correo";
            colCorreo.Name = "colCorreo";

            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AllowUserToResizeColumns = false;
            dgvClientes.AllowUserToResizeRows = false;
            dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = Color.White;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colTipo, colDocumento, colTelefono, colCorreo });
            dgvClientes.Location = new Point(16, 86);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(868, 200);
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
            dgvClientes.CellClick += dgvClientes_CellClick;

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblFiltroNombre);
            grpFiltros.Controls.Add(txtFiltroNombre);
            grpFiltros.Controls.Add(lblFiltroDocumento);
            grpFiltros.Controls.Add(txtFiltroDocumento);
            grpFiltros.Controls.Add(btnBuscar);
            grpFiltros.Controls.Add(btnCargar);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(868, 64);
            grpFiltros.Text = "Búsqueda";

            lblFiltroNombre.AutoSize = true;
            lblFiltroNombre.Location = new Point(16, 28);
            lblFiltroNombre.Text = "Nombre";
            txtFiltroNombre.Location = new Point(72, 24);
            txtFiltroNombre.Size = new Size(180, 23);
            txtFiltroNombre.TabIndex = 0;

            lblFiltroDocumento.AutoSize = true;
            lblFiltroDocumento.Location = new Point(268, 28);
            lblFiltroDocumento.Text = "Documento";
            txtFiltroDocumento.Location = new Point(348, 24);
            txtFiltroDocumento.MaxLength = ValoresDominio.LongitudMaximaNumeroDocumento;
            txtFiltroDocumento.Size = new Size(140, 23);
            txtFiltroDocumento.TabIndex = 1;

            btnBuscar.Location = new Point(504, 22);
            btnBuscar.Size = new Size(90, 28);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;

            btnCargar.Location = new Point(600, 22);
            btnCargar.Size = new Size(110, 28);
            btnCargar.TabIndex = 3;
            btnCargar.Text = "Ver todos";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;

            grpNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpNuevo.Controls.Add(lblNombreNuevo);
            grpNuevo.Controls.Add(txtNombreNuevo);
            grpNuevo.Controls.Add(lblTipoNuevo);
            grpNuevo.Controls.Add(cboTipoNuevo);
            grpNuevo.Controls.Add(lblNumeroNuevo);
            grpNuevo.Controls.Add(txtNumeroNuevo);
            grpNuevo.Controls.Add(lblTelefonoNuevo);
            grpNuevo.Controls.Add(txtTelefonoNuevo);
            grpNuevo.Controls.Add(lblCorreoNuevo);
            grpNuevo.Controls.Add(txtCorreoNuevo);
            grpNuevo.Controls.Add(btnRegistrar);
            grpNuevo.Location = new Point(16, 298);
            grpNuevo.Size = new Size(428, 236);
            grpNuevo.Text = "Nuevo cliente";

            lblNombreNuevo.AutoSize = true;
            lblNombreNuevo.Location = new Point(16, 24);
            lblNombreNuevo.Text = "Nombre";
            txtNombreNuevo.Location = new Point(19, 44);
            txtNombreNuevo.MaxLength = ValoresDominio.LongitudMaximaNombreCliente;
            txtNombreNuevo.Size = new Size(390, 23);
            txtNombreNuevo.TabIndex = 4;

            lblTipoNuevo.AutoSize = true;
            lblTipoNuevo.Location = new Point(16, 76);
            lblTipoNuevo.Text = "Tipo de documento";
            cboTipoNuevo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoNuevo.Location = new Point(19, 96);
            cboTipoNuevo.Size = new Size(180, 23);
            cboTipoNuevo.TabIndex = 5;

            lblNumeroNuevo.AutoSize = true;
            lblNumeroNuevo.Location = new Point(210, 76);
            lblNumeroNuevo.Text = "Número";
            txtNumeroNuevo.Location = new Point(213, 96);
            txtNumeroNuevo.MaxLength = ValoresDominio.LongitudMaximaNumeroDocumento;
            txtNumeroNuevo.Size = new Size(196, 23);
            txtNumeroNuevo.TabIndex = 6;

            lblTelefonoNuevo.AutoSize = true;
            lblTelefonoNuevo.Location = new Point(16, 128);
            lblTelefonoNuevo.Text = "Teléfono";
            txtTelefonoNuevo.Location = new Point(19, 148);
            txtTelefonoNuevo.MaxLength = ValoresDominio.LongitudMaximaTelefono;
            txtTelefonoNuevo.Size = new Size(180, 23);
            txtTelefonoNuevo.TabIndex = 7;

            lblCorreoNuevo.AutoSize = true;
            lblCorreoNuevo.Location = new Point(210, 128);
            lblCorreoNuevo.Text = "Correo";
            txtCorreoNuevo.Location = new Point(213, 148);
            txtCorreoNuevo.MaxLength = ValoresDominio.LongitudMaximaCorreo;
            txtCorreoNuevo.Size = new Size(196, 23);
            txtCorreoNuevo.TabIndex = 8;

            btnRegistrar.Location = new Point(19, 188);
            btnRegistrar.Size = new Size(160, 28);
            btnRegistrar.TabIndex = 9;
            btnRegistrar.Text = "Registrar cliente";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            grpSeleccionado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpSeleccionado.Controls.Add(lblSeleccionado);
            grpSeleccionado.Controls.Add(lblNombreEdicion);
            grpSeleccionado.Controls.Add(txtNombreEdicion);
            grpSeleccionado.Controls.Add(lblTipoEdicion);
            grpSeleccionado.Controls.Add(cboTipoEdicion);
            grpSeleccionado.Controls.Add(lblNumeroEdicion);
            grpSeleccionado.Controls.Add(txtNumeroEdicion);
            grpSeleccionado.Controls.Add(lblTelefonoEdicion);
            grpSeleccionado.Controls.Add(txtTelefonoEdicion);
            grpSeleccionado.Controls.Add(lblCorreoEdicion);
            grpSeleccionado.Controls.Add(txtCorreoEdicion);
            grpSeleccionado.Controls.Add(btnModificar);
            grpSeleccionado.Controls.Add(btnEliminar);
            grpSeleccionado.Enabled = false;
            grpSeleccionado.Location = new Point(452, 298);
            grpSeleccionado.Size = new Size(432, 236);
            grpSeleccionado.Text = "Editar cliente seleccionado";

            lblSeleccionado.AutoSize = false;
            lblSeleccionado.Location = new Point(16, 22);
            lblSeleccionado.Size = new Size(400, 28);
            lblSeleccionado.Text = "Seleccione un cliente de la lista para editarlo.";

            lblNombreEdicion.AutoSize = true;
            lblNombreEdicion.Location = new Point(16, 52);
            lblNombreEdicion.Text = "Nombre";
            txtNombreEdicion.Location = new Point(19, 72);
            txtNombreEdicion.MaxLength = ValoresDominio.LongitudMaximaNombreCliente;
            txtNombreEdicion.Size = new Size(394, 23);
            txtNombreEdicion.TabIndex = 10;

            lblTipoEdicion.AutoSize = true;
            lblTipoEdicion.Location = new Point(16, 104);
            lblTipoEdicion.Text = "Tipo de documento";
            cboTipoEdicion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoEdicion.Location = new Point(19, 124);
            cboTipoEdicion.Size = new Size(180, 23);
            cboTipoEdicion.TabIndex = 11;

            lblNumeroEdicion.AutoSize = true;
            lblNumeroEdicion.Location = new Point(210, 104);
            lblNumeroEdicion.Text = "Número";
            txtNumeroEdicion.Location = new Point(213, 124);
            txtNumeroEdicion.MaxLength = ValoresDominio.LongitudMaximaNumeroDocumento;
            txtNumeroEdicion.Size = new Size(200, 23);
            txtNumeroEdicion.TabIndex = 12;

            lblTelefonoEdicion.AutoSize = true;
            lblTelefonoEdicion.Location = new Point(16, 156);
            lblTelefonoEdicion.Text = "Teléfono";
            txtTelefonoEdicion.Location = new Point(19, 176);
            txtTelefonoEdicion.MaxLength = ValoresDominio.LongitudMaximaTelefono;
            txtTelefonoEdicion.Size = new Size(180, 23);
            txtTelefonoEdicion.TabIndex = 13;

            lblCorreoEdicion.AutoSize = true;
            lblCorreoEdicion.Location = new Point(210, 156);
            lblCorreoEdicion.Text = "Correo";
            txtCorreoEdicion.Location = new Point(213, 176);
            txtCorreoEdicion.MaxLength = ValoresDominio.LongitudMaximaCorreo;
            txtCorreoEdicion.Size = new Size(200, 23);
            txtCorreoEdicion.TabIndex = 14;

            btnModificar.Location = new Point(19, 208);
            btnModificar.Size = new Size(120, 28);
            btnModificar.TabIndex = 15;
            btnModificar.Text = "Guardar cambios";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnEliminar.Location = new Point(145, 208);
            btnEliminar.Size = new Size(100, 28);
            btnEliminar.TabIndex = 16;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 550);
            Controls.Add(grpSeleccionado);
            Controls.Add(grpNuevo);
            Controls.Add(dgvClientes);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(860, 560);
            Name = "FrmClientes";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Clientes";
            Load += FrmClientes_Load;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpNuevo.ResumeLayout(false);
            grpNuevo.PerformLayout();
            grpSeleccionado.ResumeLayout(false);
            grpSeleccionado.PerformLayout();
            ResumeLayout(false);
        }
    }
}
