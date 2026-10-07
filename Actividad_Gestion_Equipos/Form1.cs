using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Actividad_Gestion_Equipos
{
    public partial class Form1 : Form
    {
        // Declaración de controles de la interfaz
        private ComboBox cmbCategoria;
        private ComboBox cmbEquipos;
        private Button btnAgregar;
        private DataGridView dgvEquipos;
        private Label lblCategoria;
        private Label lblEquipos;

        // Decorativo: Panel para el encabezado (opcional)
        private Panel panelHeader;

        // DataTables para la gestión en memoria
        private DataTable dtCategorias;
        private DataTable dtEquiposMaestro; // Datos originales completos
        private DataTable dtGridEquipos;    // DataTable en memoria para el DataGridView

        public Form1()
        {
            InitializeComponent();

            // Configuración principal de la ventana
            this.Text = "Gestor Deportivo ";
            this.Size = new Size(680, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Inicializar y posicionar controles mediante código
            InicializarComponentesPersonalizados();

            // Cargar datos maestros y estructurar el DataTable en memoria
            CargarDatosMaestros();
        }

        private void InicializarComponentesPersonalizados()
        {

            // Paso 2: Agregar el Panel Lateral Decorativo (Panel)
            panelHeader = new Panel();
            panelHeader.Name = "panelHeader";
            panelHeader.Width = 200;
            panelHeader.BackColor = Color.FromArgb(0, 122, 204);
            panelHeader.Location = new Point(12, 12);
            panelHeader.Size = new Size(204, 387);
            this.Controls.Add(panelHeader);

            // 1. Etiqueta y ComboBox de Categoría (Izquierda)
            lblCategoria = new Label();
            lblCategoria.Text = "Categoría:";
            lblCategoria.ForeColor = Color.White;
            lblCategoria.Location = new Point(20, 106);
            lblCategoria.AutoSize = true;
            lblCategoria.BackColor = Color.FromArgb(0, 122, 204);
            panelHeader.Controls.Add(lblCategoria);

            cmbCategoria = new ComboBox();
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Location = new Point(12, 124);
            cmbCategoria.Width = 180;
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.SelectedIndexChanged += new EventHandler(cmbCategoria_SelectedIndexChanged);
            panelHeader.Controls.Add(cmbCategoria);

            // 2. Etiqueta y ComboBox de Equipos (Centro-Derecha)
            lblEquipos = new Label();
            lblEquipos.Text = "Equipos:";
            lblEquipos.Location = new Point(20, 176);
            lblEquipos.AutoSize = true;
            lblEquipos.BackColor = Color.FromArgb(0, 122, 204);
            lblEquipos.ForeColor = Color.White;
            panelHeader.Controls.Add(lblEquipos);

            cmbEquipos = new ComboBox();
            cmbEquipos.Name = "cmbEquipos";
            cmbEquipos.Location = new Point(12, 194);
            cmbEquipos.Width = 180;
            cmbEquipos.DropDownStyle = ComboBoxStyle.DropDownList;
            panelHeader.Controls.Add(cmbEquipos);

            // 3. Botón ADD (Colocado justo después del ComboBox de Equipos)
            btnAgregar = new Button();
            btnAgregar.Text = "ADD";
            btnAgregar.BackColor = Color.FromArgb(0, 122, 204);
            btnAgregar.Location = new Point(72,345);
            btnAgregar.Width = 65;
            btnAgregar.Height = 25;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Click += new EventHandler(btnAgregar_Click);
            panelHeader.Controls.Add(btnAgregar);

            // 4. DataGridView inferior (Alimentado por el DataTable en memoria)
            dgvEquipos = new DataGridView();
            dgvEquipos.Name = "dgvEquipos";
            dgvEquipos.BackgroundColor = Color.FromArgb(0, 122, 204);
            dgvEquipos.Location = new Point(233, 12);
            dgvEquipos.Size = new Size(419, 387);
            dgvEquipos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEquipos.ReadOnly = true;
            this.Controls.Add(dgvEquipos);
        }

        private void CargarDatosMaestros()
        {
            // A. Crear DataTable de Categorías
            dtCategorias = new DataTable();
            dtCategorias.Columns.Add("Id", typeof(int));
            dtCategorias.Columns.Add("Nombre", typeof(string));

            dtCategorias.Rows.Add(1, "Fútbol");
            dtCategorias.Rows.Add(2, "Baloncesto");
            dtCategorias.Rows.Add(3, "Tenis");

            // B. Crear DataTable Maestro de Equipos
            dtEquiposMaestro = new DataTable();
            dtEquiposMaestro.Columns.Add("Id", typeof(int));
            dtEquiposMaestro.Columns.Add("Nombre", typeof(string));
            dtEquiposMaestro.Columns.Add("Ciudad", typeof(string));
            dtEquiposMaestro.Columns.Add("CategoriaId", typeof(int));

            dtEquiposMaestro.Rows.Add(101, "Real Madrid", "Madrid", 1);
            dtEquiposMaestro.Rows.Add(102, "FC Barcelona", "Barcelona", 1);
            dtEquiposMaestro.Rows.Add(103, "Los Angeles Lakers", "Los Ángeles", 2);
            dtEquiposMaestro.Rows.Add(104, "Chicago Bulls", "Chicago", 2);
            dtEquiposMaestro.Rows.Add(105, "Club de Tenis Madrid", "Madrid", 3);

            // C. Inicializar el DataTable en memoria que gestionará los elementos del DataGridView
            dtGridEquipos = new DataTable();
            dtGridEquipos.Columns.Add("ID Equipo", typeof(int));
            dtGridEquipos.Columns.Add("Categoría", typeof(string));
            dtGridEquipos.Columns.Add("Equipo", typeof(string));
            dtGridEquipos.Columns.Add("Ciudad", typeof(string));
            
            // Enlazar el DataTable del Grid al DataGridView
            dgvEquipos.DataSource = dtGridEquipos;

            // D. Enlazar datos al ComboBox principal de categorías
            cmbCategoria.DataSource = dtCategorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "Id";

            // Disparar la selección inicial para poblar el combo de equipos
            if (cmbCategoria.Items.Count > 0)
            {
                cmbCategoria.SelectedIndex = 0;
            }
        }

        // ====================================================================
        // PARTE DE LA PERSONA 2: TU COMPAÑERO IMPLEMENTARÁ LA LÓGICA AQUÍ
        // ====================================================================

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // La Persona 2 programará el filtrado con DataView aquí
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // La Persona 2 programará la lógica del botón ADD y la validación aquí
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}