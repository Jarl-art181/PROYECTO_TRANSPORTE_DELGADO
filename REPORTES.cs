using Guna.Charts.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class REPORTES : Form
    {
        public REPORTES()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

           

            // ==========================================
            // CARGA INICIAL DEL FORMULARIO
            // ==========================================

            CargarTipos();
            CargarEstadosCombo();

            // Mostrar todos los expedientes al iniciar
            CargarExpedientes();

            // Cargar tarjetas
            CargarResumenExpedientes();

            // Cargar gráficos
            CargarEstados();
            CargarTiposDocumento();
            CargarExpedientesPorMes();
        }


        // =========================================================
        // COMBOBOX - TIPO DE DOCUMENTO
        // =========================================================

        private void CargarTipos()
        {
            comb_tipodedocumento.Items.Clear();

            comb_tipodedocumento.Items.Add("Todos");
            comb_tipodedocumento.Items.Add("Solicitud");
            comb_tipodedocumento.Items.Add("Oficio");
            comb_tipodedocumento.Items.Add("Carta");
            comb_tipodedocumento.Items.Add("Informe");
            comb_tipodedocumento.Items.Add("Reclamo");

            comb_tipodedocumento.SelectedIndex = 0;
        }


        // =========================================================
        // COMBOBOX - ESTADO
        // =========================================================

        private void CargarEstadosCombo()
        {
            combo_estado.Items.Clear();

            combo_estado.Items.Add("Todos");
            combo_estado.Items.Add("En Proceso");
            combo_estado.Items.Add("Pendiente");
            combo_estado.Items.Add("Finalizado");

            combo_estado.SelectedIndex = 0;
        }


        // =========================================================
        // BOTÓN SALIR
        // =========================================================

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // =========================================================
        // BOTÓN GESTIONAR EXPEDIENTE
        // =========================================================

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
        }


        // =========================================================
        // BOTÓN BUSCAR EXPEDIENTE
        // =========================================================

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();
        }


        // =========================================================
        // BOTÓN INICIO
        // =========================================================

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            INICIO ventana =
                new INICIO();

            ventana.Show();
        }


        // =========================================================
        // BOTÓN REGISTRAR EXPEDIENTE
        // =========================================================

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();
        }


        // =========================================================
        // BOTÓN CONSULTAR EXPEDIENTE
        // =========================================================

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();
        }


        // =========================================================
        // BOTÓN CONFIGURACIÓN
        // =========================================================

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();
        }


        // =========================================================
        // LOAD DEL FORMULARIO
        // =========================================================

        private void REPORTE_Load(object sender, EventArgs e)
        {

        }


        // =========================================================
        // CARGAR EXPEDIENTES EN EL DATAGRIDVIEW
        // =========================================================


        private void CargarExpedientes()
        {
            // ==========================================
            // UBICAR ARCHIVO DE EXPEDIENTES
            // ==========================================

            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            // Limpiar tabla
            dgvExpedientes.Rows.Clear();

            if (!File.Exists(ruta))
            {
                return;
            }

            try
            {
                // Leer registros
                string[] registros = File.ReadAllLines(ruta);

                // Recorrer expedientes
                for (int i = 0; i < registros.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(registros[i]))
                    {
                        continue;
                    }

                    string[] datos = registros[i].Split('|');

                    if (datos.Length < 10)
                    {
                        continue;
                    }

                    // Mostrar los 11 campos visibles
                    AgregarFilaReporte(datos);
                }

                // Quitar selección
                dgvExpedientes.ClearSelection();

                dgvExpedientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar expedientes:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        // =========================================================
        // CARGAR RESUMEN DE LAS TARJETAS
        // =========================================================

        private void CargarResumenExpedientes()
        {
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(ruta))
                return;

            string[] expedientes =
                File.ReadAllLines(ruta);

            int total = 0;
            int proceso = 0;
            int pendiente = 0;
            int finalizado = 0;

            // Recorrer los expedientes
            for (int i = 0; i < expedientes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(expedientes[i]))
                    continue;

                string[] datos =
                    expedientes[i].Split('|');

                if (datos.Length >= 10)
                {
                    total++;

                    string estado =
                        datos[9].Trim();

                    if (estado.Equals(
                        "En Proceso",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        proceso++;
                    }
                    else if (estado.Equals(
                        "Pendiente",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        pendiente++;
                    }
                    else if (estado.Equals(
                        "Finalizado",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        finalizado++;
                    }
                }
            }

            // Mostrar datos en las tarjetas
            num_Totalexpe.Text =
                total.ToString();

            num_proceso_ex.Text =
                proceso.ToString();

            num_pendientes_exp.Text =
                pendiente.ToString();

            num_finalizados_exp.Text =
                finalizado.ToString();
        }


        // =========================================================
        // CARGAR GRÁFICO DE ESTADOS
        // =========================================================

        private void CargarEstados()
        {
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            string[] expedientes = new string[0];

            if (File.Exists(ruta))
            {
                expedientes = File.ReadAllLines(ruta);
            }

            // Actualizar la dona, la leyenda y el total
            // desde el inicio, no solamente al generar reporte.
            ActualizarGraficoEstados(expedientes, expedientes.Length);
        }


        // =========================================================
        // CARGAR GRÁFICO POR TIPO DE DOCUMENTO
        // =========================================================

        private void CargarTiposDocumento()
        {
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(ruta))
                return;

            string[] expedientes =
                File.ReadAllLines(ruta);

            string[] tipos =
            {
                "Solicitud",
                "Oficio",
                "Carta",
                "Informe",
                "Reclamo"
            };

            int[] cantidades =
            {
                0, 0, 0, 0, 0
            };

            // Recorrer expedientes
            for (int i = 0;
                 i < expedientes.Length;
                 i++)
            {
                if (string.IsNullOrWhiteSpace(expedientes[i]))
                    continue;

                string[] datos =
                    expedientes[i].Split('|');

                if (datos.Length >= 10)
                {
                    string tipo =
                        datos[2].Trim();

                    // Buscar el tipo
                    for (int j = 0;
                         j < tipos.Length;
                         j++)
                    {
                        if (tipo.Equals(
                            tipos[j],
                            StringComparison.OrdinalIgnoreCase))
                        {
                            cantidades[j]++;
                            break;
                        }
                    }
                }
            }

            // Limpiar gráfico
            gunaBarDataset1.DataPoints.Clear();

            // Agregar datos
            for (int i = 0;
                 i < tipos.Length;
                 i++)
            {
                gunaBarDataset1.DataPoints.Add(
                    new LPoint(
                        tipos[i],
                        cantidades[i]
                    )
                );
            }
        }


        // =========================================================
        // CARGAR GRÁFICO POR MES
        // =========================================================


        private void CargarExpedientesPorMes()
        {
            // ==========================================
            // OBTENER RUTA DE LOS EXPEDIENTES
            // ==========================================

            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            // Limpiar gráfico si no existen datos
            if (!File.Exists(ruta))
            {
                gunaLineDataset1.DataPoints.Clear();
                return;
            }

            // Leer todos los registros
            string[] expedientes = File.ReadAllLines(ruta);

            // Utilizar el método de actualización
            // para no repetir código
            ActualizarGraficoMeses(
                expedientes,
                expedientes.Length
            );
        }


        // =========================================================
        // BOTÓN GENERAR REPORTE
        // =========================================================

        private void but_generar_Click(
            object sender,
            EventArgs e)
        {
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            // Verificar archivo
            if (!File.Exists(ruta))
            {
                MessageBox.Show(
                    "No se encontró el archivo de expedientes.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Leer archivo
            string[] expedientes =
                File.ReadAllLines(ruta);

            // Arreglo para guardar
            // solamente los expedientes filtrados
            string[] filtrados =
                new string[expedientes.Length];

            int cantidadFiltrados = 0;

            // ==========================================
            // FECHAS
            // ==========================================

            // ==========================================
            // VALIDAR FILTRO POR FECHAS
            // ==========================================

            string textoDesde =
                Text_fechaantes.Text.Trim();

            string textoHasta =
                text_fechahasta.Text.Trim();

            bool tieneDesde =
                !string.IsNullOrWhiteSpace(textoDesde);

            bool tieneHasta =
                !string.IsNullOrWhiteSpace(textoHasta);

            bool usarFechaDesde = false;
            bool usarFechaHasta = false;

            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MaxValue;

            // Si una fecha está llena, ambas deben estarlo
            if (tieneDesde != tieneHasta)
            {
                MessageBox.Show(
                    "Para filtrar por fechas debe completar " +
                    "Fecha desde y Fecha hasta.",
                    "Fechas incompletas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Validar fechas cuando se ingresa un período
            if (tieneDesde && tieneHasta)
            {
                if (!DateTime.TryParseExact(
                    textoDesde,
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaDesde))
                {
                    MessageBox.Show(
                        "Fecha desde inválida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    Text_fechaantes.Focus();
                    return;
                }

                if (!DateTime.TryParseExact(
                    textoHasta,
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fechaHasta))
                {
                    MessageBox.Show(
                        "Fecha hasta inválida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fechahasta.Focus();
                    return;
                }

                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show(
                        "Fecha desde no puede ser posterior " +
                        "a Fecha hasta.",
                        "Rango incorrecto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                usarFechaDesde = true;
                usarFechaHasta = true;
            }


             usarFechaDesde =
                DateTime.TryParse(
                    Text_fechaantes.Text,
                    out fechaDesde
                );

             usarFechaHasta =
                DateTime.TryParse(
                    text_fechahasta.Text,
                    out fechaHasta
                );

            // ==========================================
            // FILTROS SELECCIONADOS
            // ==========================================

            string tipoSeleccionado =
                comb_tipodedocumento.Text.Trim();

            string estadoSeleccionado =
                combo_estado.Text.Trim();


            // ==========================================
            // RECORRER TODOS LOS EXPEDIENTES
            // ==========================================

            for (int i = 0;
                 i < expedientes.Length;
                 i++)
            {
                if (string.IsNullOrWhiteSpace(
                    expedientes[i]))
                {
                    continue;
                }

                // Separar los campos
                string[] datos =
                    expedientes[i].Split('|');

                if (datos.Length < 10)
                    continue;

                bool cumple = true;


                // ======================================
                // FILTRO POR FECHA
                // ======================================

                DateTime fecha;

                

                if (!DateTime.TryParseExact(
                    datos[1].Trim(),
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fecha))
                {
                    // Ignorar expedientes con fecha incorrecta
                    continue;
                }

                // Fecha desde
                if (usarFechaDesde &&
                    fecha.Date < fechaDesde.Date)
                {
                    cumple = false;
                }

                // Fecha hasta
                if (usarFechaHasta &&
                    fecha.Date > fechaHasta.Date)
                {
                    cumple = false;
                }


                // ======================================
                // FILTRO POR TIPO DE DOCUMENTO
                // ======================================

                if (tipoSeleccionado != "Todos")
                {
                    if (!datos[2].Trim().Equals(
                        tipoSeleccionado,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cumple = false;
                    }
                }


                // ======================================
                // FILTRO POR ESTADO
                // ======================================

                if (estadoSeleccionado != "Todos")
                {
                    if (!datos[9].Trim().Equals(
                        estadoSeleccionado,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cumple = false;
                    }
                }


                // ======================================
                // GUARDAR SI CUMPLE LOS FILTROS
                // ======================================

                if (cumple)
                {
                    filtrados[cantidadFiltrados] =
                        expedientes[i];

                    cantidadFiltrados++;
                }
            }


            // ==========================================
            // MOSTRAR RESULTADOS
            // ==========================================

            MostrarTablaFiltrada(
                filtrados,
                cantidadFiltrados
            );


            // ==========================================
            // ACTUALIZAR TARJETAS
            // ==========================================

            ActualizarTarjetas(
                filtrados,
                cantidadFiltrados
            );


            // ==========================================
            // ACTUALIZAR GRÁFICO DE ESTADOS
            // ==========================================

            ActualizarGraficoEstados(
                filtrados,
                cantidadFiltrados
            );


            // ==========================================
            // ACTUALIZAR GRÁFICO DE TIPOS
            // ==========================================

            ActualizarGraficoTipos(
                filtrados,
                cantidadFiltrados
            );


            // ==========================================
            // ACTUALIZAR GRÁFICO DE MESES
            // ==========================================

            ActualizarGraficoMeses(
                filtrados,
                cantidadFiltrados
            );


            // ==========================================
            // MENSAJE FINAL
            // ==========================================

            MessageBox.Show(
                "Se encontraron " +
                cantidadFiltrados +
                " expedientes.",
                "Reporte generado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // =========================================================
        // MOSTRAR TABLA FILTRADA
        // =========================================================


        private void MostrarTablaFiltrada(
            string[] filtrados,
            int cantidad)
        {
            // ==========================================
            // LIMPIAR TABLA ANTERIOR
            // ==========================================

            dgvExpedientes.Rows.Clear();

            // ==========================================
            // RECORRER RESULTADOS FILTRADOS
            // ==========================================

            for (int i = 0; i < cantidad; i++)
            {
                if (string.IsNullOrWhiteSpace(filtrados[i]))
                {
                    continue;
                }

                string[] datos = filtrados[i].Split('|');

                if (datos.Length < 10)
                {
                    continue;
                }

                // Agregar todos los campos, incluida la placa
                AgregarFilaReporte(datos);
            }

            // ==========================================
            // ACTUALIZAR TABLA
            // ==========================================

            dgvExpedientes.ClearSelection();

            dgvExpedientes.Refresh();
        }



        // =========================================================
        // ACTUALIZAR TARJETAS
        // =========================================================

        private void ActualizarTarjetas(
            string[] filtrados,
            int cantidad)
        {
            int proceso = 0;
            int pendiente = 0;
            int finalizado = 0;

            // Contar estados de los filtrados
            for (int i = 0;
                 i < cantidad;
                 i++)
            {
                string[] datos =
                    filtrados[i].Split('|');

                string estado =
                    datos[9].Trim();

                if (estado.Equals(
                    "En Proceso",
                    StringComparison.OrdinalIgnoreCase))
                {
                    proceso++;
                }
                else if (estado.Equals(
                    "Pendiente",
                    StringComparison.OrdinalIgnoreCase))
                {
                    pendiente++;
                }
                else if (estado.Equals(
                    "Finalizado",
                    StringComparison.OrdinalIgnoreCase))
                {
                    finalizado++;
                }
            }

            // Mostrar cantidades
            num_Totalexpe.Text =
                cantidad.ToString();

            num_proceso_ex.Text =
                proceso.ToString();

            num_pendientes_exp.Text =
                pendiente.ToString();

            num_finalizados_exp.Text =
                finalizado.ToString();
        }


        // =========================================================
        // ACTUALIZAR GRÁFICO DE ESTADOS
        // =========================================================
        private void ActualizarGraficoEstados(
    string[] filtrados,
    int cantidad)
        {
            int proceso = 0;
            int pendiente = 0;
            int finalizado = 0;

            // ==========================================
            // CONTAR LOS ESTADOS DE LOS EXPEDIENTES
            // FILTRADOS
            // ==========================================

            for (int i = 0; i < cantidad; i++)
            {
                string[] datos =
                    filtrados[i].Split('|');

                string estado =
                    datos[9].Trim();

                if (estado.Equals(
                    "En Proceso",
                    StringComparison.OrdinalIgnoreCase))
                {
                    proceso++;
                }
                else if (estado.Equals(
                    "Pendiente",
                    StringComparison.OrdinalIgnoreCase))
                {
                    pendiente++;
                }
                else if (estado.Equals(
                    "Finalizado",
                    StringComparison.OrdinalIgnoreCase))
                {
                    finalizado++;
                }
            }

            // ==========================================
            // ACTUALIZAR LA DONA
            // ==========================================

            gunaDoughnutDataset1.DataPoints.Clear();

            gunaDoughnutDataset1.DataPoints.Add(
                new LPoint("En Proceso", proceso)
            );

            gunaDoughnutDataset1.DataPoints.Add(
                new LPoint("Pendiente", pendiente)
            );

            gunaDoughnutDataset1.DataPoints.Add(
                new LPoint("Finalizado", finalizado)
            );


            // ==========================================
            // CALCULAR PORCENTAJES
            // ==========================================

            double porcentajeProceso = 0;
            double porcentajePendiente = 0;
            double porcentajeFinalizado = 0;

            if (cantidad > 0)
            {
                porcentajeProceso =
                    (proceso * 100.0) / cantidad;

                porcentajePendiente =
                    (pendiente * 100.0) / cantidad;

                porcentajeFinalizado =
                    (finalizado * 100.0) / cantidad;
            }


            // ==========================================
            // ACTUALIZAR LAS ETIQUETAS DE LA LEYENDA
            // ==========================================

            DAT_PROCESO.Text =

                proceso.ToString("00") +
                " (" +
                porcentajeProceso.ToString("0") +
                "%)";

            DAT_PENDIENTE.Text =

                pendiente.ToString("00") +
                " (" +
                porcentajePendiente.ToString("0") +
                "%)";

            DAT_FINALIZADO.Text =

                finalizado.ToString("00") +
                " (" +
                porcentajeFinalizado.ToString("0") +
                "%)";


            // ==========================================
            // TOTAL DE EXPEDIENTES
            // ==========================================

            label_tot.Text =
                "TOTAL DE EXPEDIENTES: " +
                cantidad.ToString("00");
        }


        // =========================================================
        // ACTUALIZAR GRÁFICO DE TIPOS
        // =========================================================

        private void ActualizarGraficoTipos(
            string[] filtrados,
            int cantidad)
        {
            string[] tipos =
            {
                "Solicitud",
                "Oficio",
                "Carta",
                "Informe",
                "Reclamo"
            };

            int[] cantidades =
            {
                0, 0, 0, 0, 0
            };

            // Recorrer expedientes filtrados
            for (int i = 0;
                 i < cantidad;
                 i++)
            {
                string[] datos =
                    filtrados[i].Split('|');

                string tipo =
                    datos[2].Trim();

                // Buscar tipo
                for (int j = 0;
                     j < tipos.Length;
                     j++)
                {
                    if (tipo.Equals(
                        tipos[j],
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cantidades[j]++;
                        break;
                    }
                }
            }

            // Limpiar gráfico
            gunaBarDataset1.DataPoints.Clear();

            // Agregar resultados
            for (int i = 0;
                 i < tipos.Length;
                 i++)
            {
                gunaBarDataset1.DataPoints.Add(
                    new LPoint(
                        tipos[i],
                        cantidades[i]
                    )
                );
            }
        }


        // =========================================================
        // ACTUALIZAR GRÁFICO DE MESES
        // =========================================================


        private void ActualizarGraficoMeses(
            string[] filtrados,
            int cantidad)
        {
            // ==========================================
            // 1. AGRUPAR EXPEDIENTES POR MES Y AÑO
            // ==========================================

            System.Collections.Generic.SortedDictionary<DateTime, int>
                expedientesPorMes =
                new System.Collections.Generic.SortedDictionary<DateTime, int>();

            // ==========================================
            // 2. RECORRER LOS EXPEDIENTES
            // ==========================================

            for (int i = 0; i < cantidad; i++)
            {
                // Ignorar registros vacíos
                if (string.IsNullOrWhiteSpace(filtrados[i]))
                {
                    continue;
                }

                string[] datos = filtrados[i].Split('|');

                // Verificar que exista el campo fecha
                if (datos.Length < 2)
                {
                    continue;
                }

                DateTime fecha;

                // ======================================
                // 3. VALIDAR FECHA dd/MM/yyyy
                // ======================================

                if (!DateTime.TryParseExact(
                    datos[1].Trim(),
                    "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out fecha))
                {
                    continue;
                }

                // ======================================
                // 4. IDENTIFICAR MES Y AÑO
                // ======================================

                DateTime periodo = new DateTime(
                    fecha.Year,
                    fecha.Month,
                    1
                );

                // ======================================
                // 5. CONTAR EXPEDIENTES
                // ======================================

                if (expedientesPorMes.ContainsKey(periodo))
                {
                    expedientesPorMes[periodo]++;
                }
                else
                {
                    expedientesPorMes.Add(periodo, 1);
                }
            }

            // ==========================================
            // 6. LIMPIAR DATOS ANTERIORES
            // ==========================================

            gunaLineDataset1.DataPoints.Clear();

            // ==========================================
            // 7. MOSTRAR MESES CON SUS AÑOS
            // ==========================================

            foreach (var elemento in expedientesPorMes)
            {
                // Ejemplo: oct 2026
                string nombreMes = elemento.Key.ToString(
                    "MMM yyyy",
                    new System.Globalization.CultureInfo("es-PE")
                );

                // Cantidad de expedientes del período
                int total = elemento.Value;

                gunaLineDataset1.DataPoints.Add(
                    new LPoint(
                        nombreMes,
                        total
                    )
                );
            }
        }


        private void AgregarFilaReporte(string[] datos)
        {
            // ==========================================
            // VERIFICAR LOS DATOS DEL EXPEDIENTE
            // ==========================================

            if (datos.Length < 10)
            {
                return;
            }

            // ==========================================
            // OBTENER NÚMERO DE EXPEDIENTE
            // ==========================================

            string numero = datos[0].Trim();

            if (numero.StartsWith(
                "EXP-",
                StringComparison.OrdinalIgnoreCase))
            {
                numero = numero.Substring(4);
            }

            // ==========================================
            // OBTENER NÚMERO DE PLACA
            // ==========================================

            string placa = "Sin registrar";

            if (datos.Length > 11)
            {
                if (!string.IsNullOrWhiteSpace(datos[11]))
                {
                    placa = datos[11].Trim().ToUpperInvariant();
                }
            }

            // ==========================================
            // CREAR FILA EN LA TABLA
            // ==========================================

            int fila = dgvExpedientes.Rows.Add();

            // NÚMERO DE EXPEDIENTE
            dgvExpedientes.Rows[fila]
                .Cells["colExpediente"].Value =
                "EXP-" + numero;

            // FECHA
            dgvExpedientes.Rows[fila]
                .Cells["colFechaRegistro"].Value =
                datos[1];

            // TIPO DE DOCUMENTO
            dgvExpedientes.Rows[fila]
                .Cells["colDocumento"].Value =
                datos[2];

            // PLACA
            dgvExpedientes.Rows[fila]
                .Cells["Columnplaca"].Value =
                placa;

            // DESCRIPCIÓN
            dgvExpedientes.Rows[fila]
                .Cells["ColuDescripción"].Value =
                datos[3];

            // SOLICITANTE
            dgvExpedientes.Rows[fila]
                .Cells["coloSolicitante"].Value =
                datos[4];

            // DNI
            dgvExpedientes.Rows[fila]
                .Cells["colDNI"].Value =
                datos[5];

            // EMPRESA
            dgvExpedientes.Rows[fila]
                .Cells["coloEmpresa"].Value =
                datos[6];

            // TELÉFONO
            dgvExpedientes.Rows[fila]
                .Cells["colTelefono"].Value =
                datos[7];

            // CORREO
            dgvExpedientes.Rows[fila]
                .Cells["colCorreo"].Value =
                datos[8];

            // ESTADO
            dgvExpedientes.Rows[fila]
                .Cells["coluEstadoExpediente"].Value =
                datos[9];
        }

    }
}