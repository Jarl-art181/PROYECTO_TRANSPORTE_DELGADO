
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class CONSULTAR_EXPEDIENTE : Form
    {
        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public CONSULTAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Configurar la tabla
            dgvExpedientes.AllowUserToAddRows = false;
            dgvExpedientes.ReadOnly = true;
            dgvExpedientes.RowHeadersVisible = false;

            dgvExpedientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Utilizar la columna Placa ya creada
            Columnplaca.Visible = true;
            Columnplaca.HeaderText = "Placa";

            // Corregir el ejemplo del filtro
            text_nu_expediente.PlaceholderText =
                "Ej: EXP-00000001";

           

        }

        // =====================================================
        // OBTENER LA RUTA DE EXPEDIENTES
        // =====================================================

        private string ObtenerRutaExpedientes()
        {
            return Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );
        }

        // =====================================================
        // NORMALIZAR EL NÚMERO DE EXPEDIENTE
        // =====================================================

        private string NormalizarNumero(string numero)
        {
            numero = numero.Trim().ToUpperInvariant();

            // Retirar EXP- cuando exista
            if (numero.StartsWith("EXP-"))
            {
                numero = numero.Substring(4);
            }

            // Admitir expedientes anteriores
            // con números de hasta ocho dígitos
            if (!Regex.IsMatch(numero, @"^\d{1,8}$"))
            {
                return "";
            }

            // Completar con ceros a la izquierda
            return numero.PadLeft(8, '0');
        }

        // =====================================================
        // AGREGAR UN EXPEDIENTE A LA TABLA
        // =====================================================


        private void AgregarExpediente(string[] datos)
        {
            // Verificar que el expediente tenga los datos necesarios
            if (datos.Length < 10)
            {
                return;
            }

            // Número de expediente
            string numero = NormalizarNumero(datos[0]);

            if (numero == "")
            {
                return;
            }

            // Obtener placa vehicular
            string placa = "Sin registrar";

            if (datos.Length > 11)
            {
                if (!string.IsNullOrWhiteSpace(datos[11]))
                {
                    placa = datos[11].Trim().ToUpperInvariant();
                }
            }

            // Crear nueva fila
            int fila = dgvExpedientes.Rows.Add();

            // Mostrar todos los datos
            dgvExpedientes.Rows[fila].Cells["colExpediente"].Value =
                "EXP-" + numero;

            dgvExpedientes.Rows[fila].Cells["colFechaRegistro"].Value =
                datos[1];

            dgvExpedientes.Rows[fila].Cells["colDocumento"].Value =
                datos[2];

            dgvExpedientes.Rows[fila].Cells["Columnplaca"].Value =
                placa;

            dgvExpedientes.Rows[fila].Cells["ColuDescripción"].Value =
                datos[3];

            dgvExpedientes.Rows[fila].Cells["coloSolicitante"].Value =
                datos[4];

            dgvExpedientes.Rows[fila].Cells["colDNI"].Value =
                datos[5];

            dgvExpedientes.Rows[fila].Cells["coloEmpresa"].Value =
                datos[6];

            dgvExpedientes.Rows[fila].Cells["colTelefono"].Value =
                datos[7];

            dgvExpedientes.Rows[fila].Cells["colCorreo"].Value =
                datos[8];

            dgvExpedientes.Rows[fila].Cells["coluEstadoExpediente"].Value =
                datos[9];
        }


        // =====================================================
        // CARGAR TODOS LOS EXPEDIENTES
        // =====================================================

        private void CargarDatosExpedientes()
        {
            string rutaArchivo =
                ObtenerRutaExpedientes();

            // Limpiar tabla
            dgvExpedientes.Rows.Clear();

            // Verificar archivo
            if (!File.Exists(rutaArchivo))
            {
                return;
            }

            try
            {
                string[] registros =
                    File.ReadAllLines(rutaArchivo);

                // Recorrer todos los expedientes
                for (int i = 0; i < registros.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(registros[i]))
                    {
                        continue;
                    }

                    string[] datos =
                        registros[i].Split('|');

                    if (datos.Length < 10)
                    {
                        continue;
                    }

                    // Agregar registro
                    AgregarExpediente(datos);
                }

                dgvExpedientes.ClearSelection();
                dgvExpedientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los expedientes.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // CONFIGURACIÓN: CARGAR DATOS AUTOMÁTICAMENTE
        // =====================================================

        private bool CargarDatosAutomaticamente()
        {
            string rutaOpciones = Path.Combine(
                Application.StartupPath,
                "Datos",
                "opciones.txt"
            );

            // Si no existe configuración,
            // cargar automáticamente
            if (!File.Exists(rutaOpciones))
            {
                return true;
            }

            try
            {
                string contenido =
                    File.ReadAllText(rutaOpciones);

                string[] opciones =
                    contenido.Split('|');

                // Índice 2: cargar datos automáticamente
                if (opciones.Length >= 3)
                {
                    bool cargarDatos;

                    if (bool.TryParse(
                        opciones[2].Trim(),
                        out cargarDatos))
                    {
                        return cargarDatos;
                    }
                }
            }
            catch (Exception)
            {
                return true;
            }

            return true;
        }

        // =====================================================
        // BOTÓN BUSCAR / FILTRAR
        // =====================================================



        private void but_buscar_con_Click(object sender, EventArgs e)
        {
            // ==========================================
            // 1. OBTENER CRITERIOS DE BÚSQUEDA
            // ==========================================

            string numeroTexto =
                text_nu_expediente.Text.Trim().ToUpperInvariant();

            string tipoDocumento =
                comb_tipo_de_documento.Text.Trim();

            string estado =
                comb_estado.Text.Trim();

            string fechaDesdeTexto =
                text_fecha_desde.Text.Trim();

            string fechaHastaTexto =
                text_fecha_hasta.Text.Trim();


            // ==========================================
            // 2. IDENTIFICAR FILTROS
            // ==========================================

            bool buscarNumero =
                !string.IsNullOrWhiteSpace(numeroTexto);

            bool filtrarTipo =
                !string.IsNullOrWhiteSpace(tipoDocumento) &&
                !tipoDocumento.Equals(
                    "Todos",
                    StringComparison.OrdinalIgnoreCase);

            bool filtrarEstado =
                !string.IsNullOrWhiteSpace(estado) &&
                !estado.Equals(
                    "Todos",
                    StringComparison.OrdinalIgnoreCase);

            bool tieneFechaDesde =
                !string.IsNullOrWhiteSpace(fechaDesdeTexto);

            bool tieneFechaHasta =
                !string.IsNullOrWhiteSpace(fechaHastaTexto);

            bool filtrarFechas =
                tieneFechaDesde || tieneFechaHasta;


            // ==========================================
            // 3. VALIDAR BÚSQUEDA POR NÚMERO
            // ==========================================

            string numeroBuscado = "";

            if (buscarNumero)
            {
                // El número de expediente es una búsqueda independiente
                if (filtrarTipo || filtrarEstado || filtrarFechas)
                {
                    MessageBox.Show(
                        "Para buscar por número de expediente, " +
                        "deje vacíos los demás filtros.",
                        "Filtros de búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(
                    numeroTexto, @"^(EXP-)?\d{8}$"))
                {
                    MessageBox.Show(
                        "Ingrese el formato EXP-12345678.",
                        "Número inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_nu_expediente.Focus();
                    return;
                }

                // Normalizar número
                numeroBuscado = NormalizarNumero(numeroTexto);
            }


            // ==========================================
            // 4. VALIDAR FECHAS
            // ==========================================

            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MaxValue;

            if (filtrarFechas)
            {
                // Deben existir ambas fechas
                if (!tieneFechaDesde || !tieneFechaHasta)
                {
                    MessageBox.Show(
                        "Debe completar Fecha desde y Fecha hasta.",
                        "Fechas incompletas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                // Validar fecha inicial
                if (!DateTime.TryParseExact(
                    fechaDesdeTexto,
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaDesde))
                {
                    MessageBox.Show(
                        "La fecha desde no es válida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_desde.Focus();
                    return;
                }

                // Validar fecha final
                if (!DateTime.TryParseExact(
                    fechaHastaTexto,
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaHasta))
                {
                    MessageBox.Show(
                        "La fecha hasta no es válida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_hasta.Focus();
                    return;
                }

                // Validar orden de fechas
                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show(
                        "La fecha desde no puede ser posterior " +
                        "a la fecha hasta.",
                        "Rango incorrecto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
            }


            // ==========================================
            // 5. SI NO EXISTEN FILTROS
            // ==========================================

            if (!buscarNumero &&
                !filtrarTipo &&
                !filtrarEstado &&
                !filtrarFechas)
            {
                CargarDatosExpedientes();
                return;
            }


            // ==========================================
            // 6. VERIFICAR ARCHIVO
            // ==========================================

            string rutaArchivo = ObtenerRutaExpedientes();

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
            // 7. LEER Y FILTRAR EXPEDIENTES
            // ==========================================

            try
            {
                string[] registros = File.ReadAllLines(rutaArchivo);

                // Arreglo que guardará solamente las coincidencias
                string[] resultados = new string[registros.Length];

                int cantidadResultados = 0;

                for (int i = 0; i < registros.Length; i++)
                {
                    // Ignorar líneas vacías
                    if (string.IsNullOrWhiteSpace(registros[i]))
                    {
                        continue;
                    }

                    string[] datos = registros[i].Split('|');

                    if (datos.Length < 10)
                    {
                        continue;
                    }


                    // ======================================
                    // FILTRO A: SOLO POR NÚMERO
                    // ======================================

                    if (buscarNumero)
                    {
                        string numeroArchivo =
                            NormalizarNumero(datos[0]);

                        if (numeroArchivo != numeroBuscado)
                        {
                            continue;
                        }

                        // Guardar coincidencia
                        resultados[cantidadResultados] = registros[i];

                        cantidadResultados++;

                        // Un expediente específico
                        break;
                    }


                    // ======================================
                    // FILTRO B: TIPO DE DOCUMENTO
                    // ======================================

                    if (filtrarTipo)
                    {
                        if (!datos[2].Trim().Equals(
                            tipoDocumento,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // FILTRO C: ESTADO
                    // ======================================

                    if (filtrarEstado)
                    {
                        if (!datos[9].Trim().Equals(
                            estado,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // FILTRO D: RANGO DE FECHAS
                    // ======================================

                    if (filtrarFechas)
                    {
                        DateTime fechaRegistro;

                        if (!DateTime.TryParseExact(
                            datos[1].Trim(),
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out fechaRegistro))
                        {
                            continue;
                        }

                        if (fechaRegistro < fechaDesde ||
                            fechaRegistro > fechaHasta)
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // EXPEDIENTE QUE CUMPLE LOS FILTROS
                    // ======================================

                    resultados[cantidadResultados] = registros[i];

                    cantidadResultados++;
                }


                // ==========================================
                // 8. MOSTRAR LOS RESULTADOS EN LA TABLA
                // ==========================================

                // Eliminar las filas anteriores
                dgvExpedientes.Rows.Clear();

                // Mostrar únicamente las coincidencias
                for (int i = 0; i < cantidadResultados; i++)
                {
                    string[] datos = resultados[i].Split('|');

                    AgregarExpediente(datos);
                }

                // Quitar selección de filas
                dgvExpedientes.ClearSelection();

                // Actualizar visualmente la tabla
                dgvExpedientes.Refresh();


                // ==========================================
                // 9. MOSTRAR MENSAJE SI NO HAY RESULTADOS
                // ==========================================

                if (cantidadResultados == 0)
                {
                    MessageBox.Show(
                        "No se encontraron expedientes " +
                        "con los filtros ingresados.",
                        "Sin resultados",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al filtrar expedientes:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        // =====================================================
        // BOTÓN LIMPIAR
        // =====================================================

        private void but_limpieza_Click(
            object sender,
            EventArgs e)
        {
            // Limpiar número
            text_nu_expediente.Clear();

            // Limpiar tipo de documento
            comb_tipo_de_documento.SelectedIndex = -1;

            // Limpiar estado
            comb_estado.SelectedIndex = -1;

            // Limpiar ambas fechas
            text_fecha_desde.Clear();
            text_fecha_hasta.Clear();

            // Mostrar todos los expedientes
            CargarDatosExpedientes();

            // Colocar cursor
            text_nu_expediente.Focus();
        }

        // =====================================================
        // LOAD DEL FORMULARIO
        // =====================================================

        private void CONSULTAR_EXPEDIENTE_Load(
            object sender,
            EventArgs e)
        {
           
            // ==========================================
            // CARGAR TIPOS DE DOCUMENTO
            // ==========================================

            comb_tipo_de_documento.Items.Clear();

            comb_tipo_de_documento.Items.Add("Solicitud");
            comb_tipo_de_documento.Items.Add("Oficio");
            comb_tipo_de_documento.Items.Add("Carta");
            comb_tipo_de_documento.Items.Add("Informe");
            comb_tipo_de_documento.Items.Add("Reclamo");

            comb_tipo_de_documento.SelectedIndex = -1;

            // ==========================================
            // CARGAR ESTADOS
            // ==========================================

            comb_estado.Items.Clear();

            comb_estado.Items.Add("En proceso");
            comb_estado.Items.Add("Pendiente");
            comb_estado.Items.Add("Finalizado");

            comb_estado.SelectedIndex = -1;

            // ==========================================
            // CARGAR DATOS SEGÚN CONFIGURACIÓN
            // ==========================================

            if (CargarDatosAutomaticamente())
            {
                CargarDatosExpedientes();
            }
            else
            {
                dgvExpedientes.Rows.Clear();
            }
        }

        // =====================================================
        // BOTÓN CERRAR SESIÓN
        // =====================================================

        private void but_Cerrar_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }

        // =====================================================
        // BOTÓN INICIO
        // =====================================================

        private void but_inicio_Click_1(
            object sender,
            EventArgs e)
        {
            INICIO ventana = new INICIO();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void but_consultar_Click(
            object sender,
            EventArgs e)
        {
            // Estamos en Consultar Expedientes.
            // Actualizar los datos sin modificar los filtros
            but_buscar_con_Click(sender, e);
        }

        // =====================================================
        // BOTÓN BUSCAR EXPEDIENTE
        // =====================================================

        private void but_buscar_Click(
            object sender,
            EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN GESTIONAR EXPEDIENTE
        // =====================================================

        private void but_gestionar_Click(
            object sender,
            EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN REPORTES
        // =====================================================

        private void but_reportes_Click(
            object sender,
            EventArgs e)
        {
            REPORTES ventana = new REPORTES();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN CONFIGURACIÓN
        // =====================================================

        private void but_configuración_Click(
            object sender,
            EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN REGISTRO DE EXPEDIENTE
        // =====================================================

        private void but_registro_Click(
            object sender,
            EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }
    }
}
