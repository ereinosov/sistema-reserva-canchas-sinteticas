using System.Drawing;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    public partial class FrmDisponibilidad
    {
        private System.ComponentModel.IContainer components;
        private DataGridView dgvDisponibilidad;
        private GroupBox grpFiltros;
        private Label lblCancha;
        private Label lblFecha;
        private ComboBox cboCancha;
        private DateTimePicker dtpFecha;
        private Button btnConsultar;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFin;
        private DataGridViewTextBoxColumn colEstado;

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
            dgvDisponibilidad = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFin = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            grpFiltros = new GroupBox();
            lblCancha = new Label();
            cboCancha = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            btnConsultar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibilidad).BeginInit();
            grpFiltros.SuspendLayout();
            SuspendLayout();

            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.Visible = false;
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colFin.HeaderText = "Fin";
            colFin.Name = "colFin";
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";

            dgvDisponibilidad.AllowUserToAddRows = false;
            dgvDisponibilidad.AllowUserToDeleteRows = false;
            dgvDisponibilidad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDisponibilidad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDisponibilidad.BackgroundColor = Color.White;
            dgvDisponibilidad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDisponibilidad.Columns.AddRange(new DataGridViewColumn[] { colId, colInicio, colFin, colEstado });
            dgvDisponibilidad.Location = new Point(16, 86);
            dgvDisponibilidad.MultiSelect = false;
            dgvDisponibilidad.Name = "dgvDisponibilidad";
            dgvDisponibilidad.ReadOnly = true;
            dgvDisponibilidad.RowHeadersVisible = false;
            dgvDisponibilidad.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDisponibilidad.Size = new Size(552, 300);

            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblCancha);
            grpFiltros.Controls.Add(cboCancha);
            grpFiltros.Controls.Add(lblFecha);
            grpFiltros.Controls.Add(dtpFecha);
            grpFiltros.Controls.Add(btnConsultar);
            grpFiltros.Location = new Point(16, 12);
            grpFiltros.Size = new Size(552, 64);
            grpFiltros.Text = "Consulta";

            lblCancha.AutoSize = true;
            lblCancha.Location = new Point(16, 28);
            lblCancha.Text = "Cancha";
            cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCancha.Location = new Point(72, 24);
            cboCancha.Size = new Size(180, 23);
            cboCancha.TabIndex = 0;

            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(268, 28);
            lblFecha.Text = "Fecha";
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(312, 24);
            dtpFecha.Size = new Size(110, 23);
            dtpFecha.TabIndex = 1;

            btnConsultar.Location = new Point(434, 22);
            btnConsultar.Size = new Size(100, 28);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 412);
            Controls.Add(dgvDisponibilidad);
            Controls.Add(grpFiltros);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(520, 360);
            Name = "FrmDisponibilidad";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Disponibilidad";
            Load += FrmDisponibilidad_Load;
            // Aplicar icono de la aplicación (usa el icono del ejecutable)
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ((System.ComponentModel.ISupportInitialize)dgvDisponibilidad).EndInit();
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            ResumeLayout(false);
        }
    }
}
