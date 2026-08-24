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
        private GroupBox grpDatos;
        private Label lblFiltroNombre;
        private Label lblFiltroDocumento;
        private TextBox txtFiltroNombre;
        private TextBox txtFiltroDocumento;
        private Button btnBuscar;
        private Button btnCargar;
        private Label lblNombreCliente;
        private Label lblTipoDocumento;
        private Label lblNumeroDocumento;
        private Label lblTelefonoCliente;
        private Label lblCorreoCliente;
        private TextBox txtNombreCliente;
        private ComboBox cboTipoDocumento;
        private TextBox txtNumeroDocumento;
        private TextBox txtTelefonoCliente;
        private TextBox txtCorreoCliente;
        private Button btnRegistrar;
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
            grpDatos = new GroupBox();
            lblNombreCliente = new Label();
            txtNombreCliente = new TextBox();
            lblTipoDocumento = new Label();
            cboTipoDocumento = new ComboBox();
            lblNumeroDocumento = new Label();
            txtNumeroDocumento = new TextBox();
            lblTelefonoCliente = new Label();
            txtTelefonoCliente = new TextBox();
            lblCorreoCliente = new Label();
            txtCorreoCliente = new TextBox();
            btnRegistrar = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            errValidacion = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            grpFiltros.SuspendLayout();
            grpDatos.SuspendLayout();
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
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(816, 230);
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblFiltroNombre);
            grpFiltros.Controls.Add(txtFiltroNombre);
            grpFiltros.Controls.Add(lblFiltroDocumento);
            grpFiltros.Controls.Add(txtFiltroDocumento);
            grpFiltros.Controls.Add(btnBuscar);
            grpFiltros.Controls.Add(btnCargar);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(816, 64);
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

            grpDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblNombreCliente);
            grpDatos.Controls.Add(txtNombreCliente);
            grpDatos.Controls.Add(lblTipoDocumento);
            grpDatos.Controls.Add(cboTipoDocumento);
            grpDatos.Controls.Add(lblNumeroDocumento);
            grpDatos.Controls.Add(txtNumeroDocumento);
            grpDatos.Controls.Add(lblTelefonoCliente);
            grpDatos.Controls.Add(txtTelefonoCliente);
            grpDatos.Controls.Add(lblCorreoCliente);
            grpDatos.Controls.Add(txtCorreoCliente);
            grpDatos.Controls.Add(btnRegistrar);
            grpDatos.Controls.Add(btnModificar);
            grpDatos.Controls.Add(btnEliminar);
            grpDatos.Location = new Point(16, 328);
            grpDatos.Size = new Size(816, 168);
            grpDatos.Text = "Datos del cliente";

            lblNombreCliente.AutoSize = true;
            lblNombreCliente.Location = new Point(16, 28);
            lblNombreCliente.Text = "Nombre";
            txtNombreCliente.Location = new Point(19, 48);
            txtNombreCliente.MaxLength = ValoresDominio.LongitudMaximaNombreCliente;
            txtNombreCliente.Size = new Size(280, 23);
            txtNombreCliente.TabIndex = 4;

            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Location = new Point(316, 28);
            lblTipoDocumento.Text = "Tipo de documento";
            cboTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoDocumento.Location = new Point(319, 48);
            cboTipoDocumento.Size = new Size(150, 23);
            cboTipoDocumento.TabIndex = 5;

            lblNumeroDocumento.AutoSize = true;
            lblNumeroDocumento.Location = new Point(486, 28);
            lblNumeroDocumento.Text = "Número";
            txtNumeroDocumento.Location = new Point(489, 48);
            txtNumeroDocumento.MaxLength = ValoresDominio.LongitudMaximaNumeroDocumento;
            txtNumeroDocumento.Size = new Size(150, 23);
            txtNumeroDocumento.TabIndex = 6;

            lblTelefonoCliente.AutoSize = true;
            lblTelefonoCliente.Location = new Point(16, 80);
            lblTelefonoCliente.Text = "Teléfono";
            txtTelefonoCliente.Location = new Point(19, 100);
            txtTelefonoCliente.MaxLength = ValoresDominio.LongitudMaximaTelefono;
            txtTelefonoCliente.Size = new Size(180, 23);
            txtTelefonoCliente.TabIndex = 7;

            lblCorreoCliente.AutoSize = true;
            lblCorreoCliente.Location = new Point(216, 80);
            lblCorreoCliente.Text = "Correo";
            txtCorreoCliente.Location = new Point(219, 100);
            txtCorreoCliente.MaxLength = ValoresDominio.LongitudMaximaCorreo;
            txtCorreoCliente.Size = new Size(250, 23);
            txtCorreoCliente.TabIndex = 8;

            btnRegistrar.Location = new Point(19, 132);
            btnRegistrar.Size = new Size(100, 28);
            btnRegistrar.TabIndex = 9;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;

            btnModificar.Location = new Point(125, 132);
            btnModificar.Size = new Size(100, 28);
            btnModificar.TabIndex = 10;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;

            btnEliminar.Location = new Point(231, 132);
            btnEliminar.Size = new Size(100, 28);
            btnEliminar.TabIndex = 11;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;

            errValidacion.ContainerControl = this;
            errValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(848, 512);
            Controls.Add(grpDatos);
            Controls.Add(dgvClientes);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(800, 480);
            Name = "FrmClientes";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Clientes";
            Load += FrmClientes_Load;
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
