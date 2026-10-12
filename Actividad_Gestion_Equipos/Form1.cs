using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Actividad_Gestion_Equipos
{
    public partial class Form1 : Form
    {
        // Barra de Menús Superior
        private MenuStrip menuStripPrincipal;
        private ToolStripMenuItem menuArchivo;
        private ToolStripMenuItem itemLimpiarTabla;
        private ToolStripMenuItem itemSalir;

        // Sistema de Pestañas
        private TabControl tabControlPrincipal;
        private TabPage tabGestion;
        private TabPage tabEstadisticas;

        // Pestaña 1: Declaración de controles de la interfaz
        private ComboBox cmbCategoria;
        private ComboBox cmbEquipos;
        private Button btnAgregar;
        private DataGridView dgvEquipos;
        private Label lblCategoria;
        private Label lblEquipos;

        // Pestaña 2: Estadísticas y Resumen (Adaptada a tu estilo visual)
        private Panel panelEstadisticas;
        private Label lblTituloEstadisticas;
        private Label lblTotalEquipos;
        private Label lblTotalFutbol;
        private Label lblTotalBaloncesto;
        private Label lblTotalTenis;

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
            this.Size = new Size(700, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;

            // Inicializar y posicionar controles mediante código
            InicializarComponentesPersonalizados();

            // Cargar datos maestros y estructurar el DataTable en memoria
            CargarDatosMaestros();

            // Inicializar métricas
            ActualizarEstadisticas();
        }

        private void InicializarComponentesPersonalizados()
        {
            // ----------------------------------------------------
            // 1. Barra de Menús Superior (MenuStrip)
            // ----------------------------------------------------
            menuStripPrincipal = new MenuStrip();
            menuStripPrincipal.Dock = DockStyle.Top;
            menuStripPrincipal.BackColor = Color.WhiteSmoke;

            menuArchivo = new ToolStripMenuItem("&Archivo");
            itemLimpiarTabla = new ToolStripMenuItem("&Limpiar Tabla");
            itemSalir = new ToolStripMenuItem("&Salir");

            itemLimpiarTabla.Click += ItemLimpiarTabla_Click;
            itemSalir.Click += ItemSalir_Click;

            menuArchivo.DropDownItems.Add(itemLimpiarTabla);
            menuArchivo.DropDownItems.Add(new ToolStripSeparator());
            menuArchivo.DropDownItems.Add(itemSalir);

            menuStripPrincipal.Items.Add(menuArchivo);
            this.MainMenuStrip = menuStripPrincipal;

            // ----------------------------------------------------
            // 2. Control de Pestañas (TabControl)
            // ----------------------------------------------------
            tabControlPrincipal = new TabControl();
            tabControlPrincipal.Dock = DockStyle.Fill;

            tabGestion = new TabPage("Gestión de Equipos");
            tabEstadisticas = new TabPage("Estadísticas y Resumen");

            tabControlPrincipal.TabPages.Add(tabGestion);
            tabControlPrincipal.TabPages.Add(tabEstadisticas);

            // Se agregan al formulario garantizando el orden de acoplamiento
            this.Controls.Add(tabControlPrincipal);
            this.Controls.Add(menuStripPrincipal);

            // Paso 2: Agregar el Panel Lateral Decorativo (Panel)
            panelHeader = new Panel();
            panelHeader.Name = "panelHeader";
            panelHeader.BackColor = Color.FromArgb(0, 122, 204);
            panelHeader.Location = new Point(12, 12);
            panelHeader.Size = new Size(204, 387);
            tabGestion.Controls.Add(panelHeader);

            // ====================================================
            // 3. PESTAÑA 1
            // ====================================================

            // 1. Etiqueta y ComboBox de Categoría 
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

            //Etiqueta y ComboBox de Equipos
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
            btnAgregar.BackColor = Color.White;
            btnAgregar.ForeColor = Color.FromArgb(0, 122, 204);
            btnAgregar.Location = new Point(67, 345);
            btnAgregar.Width = 70;
            btnAgregar.Height = 28;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Click += new EventHandler(btnAgregar_Click);
            panelHeader.Controls.Add(btnAgregar);

            // 4. DataGridView inferior 
            dgvEquipos = new DataGridView();
            dgvEquipos.Name = "dgvEquipos";
            dgvEquipos.BackgroundColor = Color.FromArgb(0, 122, 204);
            dgvEquipos.Location = new Point(230, 12);
            dgvEquipos.Size = new Size(435, 387);
            dgvEquipos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEquipos.ReadOnly = true;
            dgvEquipos.AllowUserToAddRows = false;
            tabGestion.Controls.Add(dgvEquipos);

            // ====================================================
            // 4. PESTAÑA 2
            // ====================================================

            panelEstadisticas = new Panel();
            panelEstadisticas.Name = "panelEstadisticas";
            panelEstadisticas.BackColor = Color.FromArgb(0, 122, 204);
            panelEstadisticas.Location = new Point(12, 12);
            panelEstadisticas.Size = new Size(653, 387);
            tabEstadisticas.Controls.Add(panelEstadisticas);

            lblTituloEstadisticas = new Label();
            lblTituloEstadisticas.Text = "MÉTRICAS Y RESUMEN GENERAL";
            lblTituloEstadisticas.Font = new Font(this.Font.FontFamily, 12, FontStyle.Bold);
            lblTituloEstadisticas.ForeColor = Color.White;
            lblTituloEstadisticas.Location = new Point(30, 30);
            lblTituloEstadisticas.AutoSize = true;
            panelEstadisticas.Controls.Add(lblTituloEstadisticas);

            lblTotalEquipos = new Label();
            lblTotalEquipos.Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
            lblTotalEquipos.ForeColor = Color.White;
            lblTotalEquipos.Location = new Point(30, 85);
            lblTotalEquipos.AutoSize = true;
            panelEstadisticas.Controls.Add(lblTotalEquipos);

            lblTotalFutbol = new Label();
            lblTotalFutbol.Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
            lblTotalFutbol.ForeColor = Color.White;
            lblTotalFutbol.Location = new Point(30, 125);
            lblTotalFutbol.AutoSize = true;
            panelEstadisticas.Controls.Add(lblTotalFutbol);

            lblTotalBaloncesto = new Label();
            lblTotalBaloncesto.Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
            lblTotalBaloncesto.ForeColor = Color.White;
            lblTotalBaloncesto.Location = new Point(30, 165);
            lblTotalBaloncesto.AutoSize = true;
            panelEstadisticas.Controls.Add(lblTotalBaloncesto);

            lblTotalTenis = new Label();
            lblTotalTenis.Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
            lblTotalTenis.ForeColor = Color.White;
            lblTotalTenis.Location = new Point(30, 205);
            lblTotalTenis.AutoSize = true;
            panelEstadisticas.Controls.Add(lblTotalTenis);

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

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem == null || !(cmbCategoria.SelectedItem is DataRowView))
                return;

            DataRowView filaCategoria = (DataRowView)cmbCategoria.SelectedItem;
            int idCategoria = (int)filaCategoria["Id"];

            DataView vista = new DataView(dtEquiposMaestro);
            vista.RowFilter = "CategoriaId = " + idCategoria;

            cmbEquipos.DataSource = vista;
            cmbEquipos.DisplayMember = "Nombre";
            cmbEquipos.ValueMember = "Id";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem == null || cmbEquipos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un equipo.");
                return;
            }

            // Extraer DataRowView de ambos ComboBox (Requisito de la pauta)
            DataRowView filaCategoria = (DataRowView)cmbCategoria.SelectedItem;
            DataRowView filaEquipo = (DataRowView)cmbEquipos.SelectedItem;

            int id = (int)filaEquipo["Id"];
            string nombre = filaEquipo["Nombre"].ToString();
            string ciudad = filaEquipo["Ciudad"].ToString();
            string categoria = filaCategoria["Nombre"].ToString();

            // Comprobación de duplicados con casteo simple (int)
            foreach (DataRow fila in dtGridEquipos.Rows)
            {
                if ((int)fila["ID Equipo"] == id)
                {
                    MessageBox.Show("Este equipo ya está en la lista.");
                    return;
                }
            }

            // Insertar directamente la fila en el DataTable
            dtGridEquipos.Rows.Add(id, categoria, nombre, ciudad);

            // Actualizar estadísticas reactivas
            ActualizarEstadisticas();
        }

        private void ItemLimpiarTabla_Click(object sender, EventArgs e)
        {
        }
        private void ItemSalir_Click(object sender, EventArgs e)
        {
        }

        private void ActualizarEstadisticas()
        {
 
        }
    }
}