using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class CONSULTAR_EXPEDIENTE : Form
    {
        public CONSULTAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
        }


        // =========================================================
        // CARGAR DATOS DE LOS EXPEDIENTES
        // =========================================================

        private void CargarDatosExpedientes()
        {
            // 1. UBICAR EL ARCHIVO

            string archivoExpedientes =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "expedientes.txt"
                );


            // 2. VERIFICAR QUE EXISTA EL ARCHIVO

            if (!File.Exists(archivoExpedientes))
            {
                return;
            }


            // 3. LIMPIAR LA TABLA

            dgvExpedientes.Rows.Clear();


            // 4. LEER TODOS LOS REGISTROS

            string[] registros =
                File.ReadAllLines(archivoExpedientes);


            // =================== MATRIZ ===================

            // 5. CREAR LA MATRIZ
            // Filas = cantidad de expedientes
            // Columnas = 10 datos que mostraremos

            string[,] matriz = new string[registros.Length, 10];


            // 6. PASAR LOS DATOS DEL ARCHIVO
            //    A LA MATRIZ

            for (int i = 0; i < registros.Length; i++)
            {
                // Separar los datos del expediente

                string[] datos =
                    registros[i].Split('|');


                // Verificar que existan los datos

                if (datos.Length < 10)
                {
                    continue;
                }


                // GUARDAR LOS DATOS EN LA MATRIZ

                matriz[i, 0] = datos[0]; // Número de expediente
                matriz[i, 1] = datos[1]; // Fecha
                matriz[i, 2] = datos[2]; // Tipo de documento
                matriz[i, 3] = datos[3]; // Descripción
                matriz[i, 4] = datos[4]; // Solicitante
                matriz[i, 5] = datos[5]; // DNI
                matriz[i, 6] = datos[6]; // Empresa
                matriz[i, 7] = datos[7]; // Teléfono
                matriz[i, 8] = datos[8]; // Correo
                matriz[i, 9] = datos[9]; // Estado
            }


            // 7. MOSTRAR LA MATRIZ EN LA TABLA

            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                // Si no existe número de expediente,
                // no mostramos esa fila

                if (string.IsNullOrWhiteSpace(matriz[i, 0]))
                {
                    continue;
                }


                // Crear una nueva fila

                int fila =
                    dgvExpedientes.Rows.Add();


                // COLUMNA 0 → N.º EXPEDIENTE

                dgvExpedientes.Rows[fila]
                    .Cells["colExpediente"]
                    .Value = matriz[i, 0];


                // COLUMNA 1 → FECHA

                dgvExpedientes.Rows[fila]
                    .Cells["colFechaRegistro"]
                    .Value = matriz[i, 1];


                // COLUMNA 2 → TIPO DE DOCUMENTO

                dgvExpedientes.Rows[fila]
                    .Cells["colDocumento"]
                    .Value = matriz[i, 2];


                // COLUMNA 3 → DESCRIPCIÓN

                dgvExpedientes.Rows[fila]
                    .Cells["ColuDescripción"]
                    .Value = matriz[i, 3];


                // COLUMNA 4 → SOLICITANTE

                dgvExpedientes.Rows[fila]
                    .Cells["coloSolicitante"]
                    .Value = matriz[i, 4];


                // COLUMNA 5 → DNI

                dgvExpedientes.Rows[fila]
                    .Cells["colDNI"]
                    .Value = matriz[i, 5];


                // COLUMNA 6 → EMPRESA

                dgvExpedientes.Rows[fila]
                    .Cells["coloEmpresa"]
                    .Value = matriz[i, 6];


                // COLUMNA 7 → TELÉFONO

                dgvExpedientes.Rows[fila]
                    .Cells["colTelefono"]
                    .Value = matriz[i, 7];


                // COLUMNA 8 → CORREO

                dgvExpedientes.Rows[fila]
                    .Cells["colCorreo"]
                    .Value = matriz[i, 8];


                // COLUMNA 9 → ESTADO

                dgvExpedientes.Rows[fila]
                    .Cells["coluEstadoExpediente"]
                    .Value = matriz[i, 9];
            }


            // 8. QUITAR SELECCIÓN

            dgvExpedientes.ClearSelection();

            dgvExpedientes.Refresh();
        }


        // =========================================================
        // CARGAR DATOS AUTOMÁTICAMENTE
        // =========================================================

        private bool CargarDatosAutomaticamente()
        {
            string rutaOpciones =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "opciones.txt"
                );


            // Si no existe opciones.txt,
            // por seguridad se cargan los datos

            if (!File.Exists(rutaOpciones))
            {
                return true;
            }


            // Leer las opciones

            string contenido =
                File.ReadAllText(rutaOpciones);

            string[] opciones =
                contenido.Split('|');


            // La opción 2 corresponde a:
            // Cargar datos automáticamente

            if (opciones.Length >= 3)
            {
                bool.TryParse(
                    opciones[2],
                    out bool cargarDatos
                );

                return cargarDatos;
            }


            // Si la opción no existe,
            // se cargan los datos por defecto

            return true;
        }


        // =========================================================
        // BOTÓN CERRAR SESIÓN
        // =========================================================

        private void but_Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // =========================================================
        // BOTÓN INICIO
        // =========================================================

        private void but_inicio_Click_1(object sender, EventArgs e)
        {
            INICIO ventana =
                new INICIO();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN CONSULTAR
        // =========================================================

        private void but_consultar_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN BUSCAR
        // =========================================================

        private void but_buscar_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN GESTIONAR
        // =========================================================

        private void but_gestionar_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN REPORTES
        // =========================================================

        private void but_reportes_Click(object sender, EventArgs e)
        {
            REPORTES ventana =
                new REPORTES();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN CONFIGURACIÓN
        // =========================================================

        private void but_configuración_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN REGISTRO
        // =========================================================

        private void but_registro_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BOTÓN BUSCAR / FILTRAR
        // =========================================================

        private void but_buscar_con_Click(object sender, EventArgs e)
        {
            // ==========================================
            // 1. VALIDAR NÚMERO DE EXPEDIENTE
            // ==========================================

            // El número es OPCIONAL.
            // Solo se valida si el usuario escribió algo.

            if (!string.IsNullOrWhiteSpace(text_nu_expediente.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(
                    text_nu_expediente.Text.Trim(),
                    @"^EXP-\d{6}$"))
                {
                    MessageBox.Show(
                        "El número de expediente debe tener el formato EXP-123456.",
                        "Número de expediente inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_nu_expediente.Focus();

                    return;
                }
            }


            // ==========================================
            // 2. VERIFICAR QUE HAYA ALGÚN FILTRO
            // ==========================================

            bool hayFiltro = false;


            if (!string.IsNullOrWhiteSpace(
                text_nu_expediente.Text))
            {
                hayFiltro = true;
            }


            if (comb_tipo_de_documento.SelectedIndex != -1)
            {
                hayFiltro = true;
            }


            if (comb_estado.SelectedIndex != -1)
            {
                hayFiltro = true;
            }


            if (!string.IsNullOrWhiteSpace(
                text_fecha_desde.Text))
            {
                hayFiltro = true;
            }


            if (!string.IsNullOrWhiteSpace(
                text_fecha_hasta.Text))
            {
                hayFiltro = true;
            }


            if (!hayFiltro)
            {
                MessageBox.Show(
                    "Ingrese o seleccione al menos un criterio de búsqueda.",
                    "Búsqueda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ==========================================
            // 3. RUTA DEL ARCHIVO
            // ==========================================

            string rutaArchivo =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "expedientes.txt"
                );


            // ==========================================
            // 4. VERIFICAR QUE EXISTA EL ARCHIVO
            // ==========================================

            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ==========================================
            // 5. LIMPIAR LA TABLA
            // ==========================================

            dgvExpedientes.Rows.Clear();


            // ==========================================
            // 6. LEER LOS REGISTROS
            // ==========================================

            string[] registros =
                File.ReadAllLines(rutaArchivo);

            bool encontrado = false;


            // ==========================================
            // 7. OBTENER EL NÚMERO DE EXPEDIENTE
            // ==========================================

            string numeroBuscado = "";


            if (!string.IsNullOrWhiteSpace(
                text_nu_expediente.Text))
            {
                numeroBuscado =
                    text_nu_expediente.Text.Trim();

                // Quitar "EXP-"

                numeroBuscado =
                    numeroBuscado.Substring(4);
            }


            // ==========================================
            // 8. VALIDAR FECHA DESDE
            // ==========================================

            DateTime fechaDesde =
                DateTime.MinValue;

            bool usarFechaDesde = false;


            if (!string.IsNullOrWhiteSpace(
                text_fecha_desde.Text))
            {
                if (!DateTime.TryParseExact(
                    text_fecha_desde.Text.Trim(),
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaDesde))
                {
                    MessageBox.Show(
                        "La fecha desde debe tener el formato dd/MM/yyyy.",
                        "Fecha inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_desde.Focus();

                    return;
                }

                usarFechaDesde = true;
            }


            // ==========================================
            // 9. VALIDAR FECHA HASTA
            // ==========================================

            DateTime fechaHasta =
                DateTime.MaxValue;

            bool usarFechaHasta = false;


            if (!string.IsNullOrWhiteSpace(
                text_fecha_hasta.Text))
            {
                if (!DateTime.TryParseExact(
                    text_fecha_hasta.Text.Trim(),
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaHasta))
                {
                    MessageBox.Show(
                        "La fecha hasta debe tener el formato dd/MM/yyyy.",
                        "Fecha inválida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_hasta.Focus();

                    return;
                }

                usarFechaHasta = true;
            }


            // ==========================================
            // 10. VALIDAR RANGO DE FECHAS
            // ==========================================

            if (usarFechaDesde &&
                usarFechaHasta)
            {
                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show(
                        "La fecha desde no puede ser posterior a la fecha hasta.",
                        "Rango de fechas inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_desde.Focus();

                    return;
                }
            }


            // ==========================================
            // 11. RECORRER LOS EXPEDIENTES
            // ==========================================

            for (int i = 0;
                 i < registros.Length;
                 i++)
            {
                // Ignorar líneas vacías

                if (string.IsNullOrWhiteSpace(
                    registros[i]))
                {
                    continue;
                }


                // ==========================================
                // SEPARAR LOS DATOS
                // ==========================================

                string[] datos =
                    registros[i].Split('|');


                // Verificar que tenga los campos necesarios

                if (datos.Length < 10)
                {
                    continue;
                }


                // ==========================================
                // FILTRO POR NÚMERO DE EXPEDIENTE
                // ==========================================

                if (!string.IsNullOrWhiteSpace(
                    text_nu_expediente.Text))
                {
                    if (datos[0] != numeroBuscado)
                    {
                        continue;
                    }
                }


                // ==========================================
                // FILTRO POR TIPO DE DOCUMENTO
                // ==========================================

                if (comb_tipo_de_documento.SelectedIndex != -1)
                {
                    if (datos[2] !=
                        comb_tipo_de_documento.Text)
                    {
                        continue;
                    }
                }


                // ==========================================
                // FILTRO POR ESTADO
                // ==========================================

                if (comb_estado.SelectedIndex != -1)
                {
                    if (datos[9] !=
                        comb_estado.Text)
                    {
                        continue;
                    }
                }


                // ==========================================
                // CONVERTIR FECHA DEL REGISTRO
                // ==========================================

                DateTime fechaRegistro;


                if (!DateTime.TryParseExact(
                    datos[1],
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaRegistro))
                {
                    continue;
                }


                // ==========================================
                // FILTRO POR FECHA DESDE
                // ==========================================

                if (usarFechaDesde)
                {
                    if (fechaRegistro < fechaDesde)
                    {
                        continue;
                    }
                }


                // ==========================================
                // FILTRO POR FECHA HASTA
                // ==========================================

                if (usarFechaHasta)
                {
                    if (fechaRegistro > fechaHasta)
                    {
                        continue;
                    }
                }


                // ==========================================
                // EXPEDIENTE ENCONTRADO
                // ==========================================

                encontrado = true;


                // ==========================================
                // AGREGAR FILA A LA TABLA
                // ==========================================

                int fila =
                    dgvExpedientes.Rows.Add();


                dgvExpedientes.Rows[fila]
                    .Cells["colExpediente"]
                    .Value =
                    "EXP-" + datos[0];


                dgvExpedientes.Rows[fila]
                    .Cells["colFechaRegistro"]
                    .Value =
                    datos[1];


                dgvExpedientes.Rows[fila]
                    .Cells["colDocumento"]
                    .Value =
                    datos[2];


                dgvExpedientes.Rows[fila]
                    .Cells["ColuDescripción"]
                    .Value =
                    datos[3];


                dgvExpedientes.Rows[fila]
                    .Cells["coloSolicitante"]
                    .Value =
                    datos[4];


                dgvExpedientes.Rows[fila]
                    .Cells["colDNI"]
                    .Value =
                    datos[5];


                dgvExpedientes.Rows[fila]
                    .Cells["coloEmpresa"]
                    .Value =
                    datos[6];


                dgvExpedientes.Rows[fila]
                    .Cells["colTelefono"]
                    .Value =
                    datos[7];


                dgvExpedientes.Rows[fila]
                    .Cells["colCorreo"]
                    .Value =
                    datos[8];


                dgvExpedientes.Rows[fila]
                    .Cells["coluEstadoExpediente"]
                    .Value =
                    datos[9];


                // ==========================================
                // SI SE BUSCÓ POR NÚMERO,
                // SOLO DEBE MOSTRAR UNO
                // ==========================================

                if (!string.IsNullOrWhiteSpace(
                    text_nu_expediente.Text))
                {
                    break;
                }
            }


            // ==========================================
            // 12. SI NO ENCONTRÓ RESULTADOS
            // ==========================================

            if (!encontrado)
            {
                MessageBox.Show(
                    "No se encontraron expedientes con los criterios ingresados.",
                    "Resultado de búsqueda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // =========================================================
        // BOTÓN LIMPIEZA
        // =========================================================

        private void but_limpieza_Click(object sender, EventArgs e)
        {
            text_nu_expediente.Clear();

            comb_tipo_de_documento.SelectedIndex = -1;

            comb_estado.SelectedIndex = -1;

            text_fecha_desde.Clear();

            text_fecha_hasta.Clear();

            CargarDatosExpedientes();
        }


        // =========================================================
        // LOAD DEL FORMULARIO
        // =========================================================

        private void CONSULTAR_EXPEDIENTE_Load(object sender, EventArgs e)
        {
            // CARGAR DATOS AUTOMÁTICAMENTE
            // Solo si la opción está activada

            if (CargarDatosAutomaticamente())
            {
                CargarDatosExpedientes();
            }


            // ESTADOS

            comb_estado.Items.Add("En proceso");
            comb_estado.Items.Add("Pendiente");
            comb_estado.Items.Add("Finalizado");


            // TIPOS DE DOCUMENTO

            comb_tipo_de_documento.Items.Clear();

            comb_tipo_de_documento.Items.Add("Solicitud");
            comb_tipo_de_documento.Items.Add("Oficio");
            comb_tipo_de_documento.Items.Add("Carta");
            comb_tipo_de_documento.Items.Add("Informe");
            comb_tipo_de_documento.Items.Add("Reclamo");
        }
    }
}