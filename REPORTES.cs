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
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(ruta))
                return;

            dgvExpedientes.Rows.Clear();

            string[] lineas =
                File.ReadAllLines(ruta);

            // Recorrer todos los registros
            for (int i = 0; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i]))
                    continue;

                string[] datos =
                    lineas[i].Split('|');

                // Verificar que tenga los 10 campos
                if (datos.Length >= 10)
                {
                    dgvExpedientes.Rows.Add(
                        datos[0], // Expediente
                        datos[1], // Fecha
                        datos[2], // Tipo
                        datos[3], // Descripción
                        datos[4], // Solicitante
                        datos[5], // DNI
                        datos[6], // Empresa
                        datos[7], // Teléfono
                        datos[8], // Correo
                        datos[9]  // Estado
                    );
                }
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

            if (!File.Exists(ruta))
                return;

            string[] expedientes =
                File.ReadAllLines(ruta);

            string[] estados =
            {
                "En Proceso",
                "Pendiente",
                "Finalizado"
            };

            int[] cantidades =
            {
                0, 0, 0
            };

            // Recorrer expedientes
            for (int i = 0; i < expedientes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(expedientes[i]))
                    continue;

                string[] datos =
                    expedientes[i].Split('|');

                if (datos.Length >= 10)
                {
                    string estado =
                        datos[9].Trim();

                    for (int j = 0;
                         j < estados.Length;
                         j++)
                    {
                        if (estado.Equals(
                            estados[j],
                            StringComparison.OrdinalIgnoreCase))
                        {
                            cantidades[j]++;
                            break;
                        }
                    }
                }
            }

            // Limpiar gráfico
            gunaDoughnutDataset1.DataPoints.Clear();

            // Agregar datos
            for (int i = 0;
                 i < estados.Length;
                 i++)
            {
                gunaDoughnutDataset1.DataPoints.Add(
                    new LPoint(
                        estados[i],
                        cantidades[i]
                    )
                );
            }
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
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(ruta))
                return;

            string[] expedientes =
                File.ReadAllLines(ruta);

            string[] meses =
            {
                "Ene", "Feb", "Mar", "Abr",
                "May", "Jun", "Jul", "Ago",
                "Sep", "Oct", "Nov", "Dic"
            };

            int[] cantidades =
            {
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
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
                    DateTime fecha;

                    if (DateTime.TryParse(
                        datos[1].Trim(),
                        out fecha))
                    {
                        int mes =
                            fecha.Month - 1;

                        cantidades[mes]++;
                    }
                }
            }

            // Limpiar gráfico
            gunaLineDataset1.DataPoints.Clear();

            // Agregar meses
            for (int i = 0;
                 i < meses.Length;
                 i++)
            {
                gunaLineDataset1.DataPoints.Add(
                    new LPoint(
                        meses[i],
                        cantidades[i]
                    )
                );
            }
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

            DateTime fechaDesde;
            DateTime fechaHasta;

            bool usarFechaDesde =
                DateTime.TryParse(
                    Text_fechaantes.Text,
                    out fechaDesde
                );

            bool usarFechaHasta =
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

                if (!DateTime.TryParse(
                    datos[1].Trim(),
                    out fecha))
                {
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
            // Limpiar tabla
            dgvExpedientes.Rows.Clear();

            // Recorrer resultados
            for (int i = 0;
                 i < cantidad;
                 i++)
            {
                string[] datos =
                    filtrados[i].Split('|');

                dgvExpedientes.Rows.Add(
                    datos[0],
                    datos[1],
                    datos[2],
                    datos[3],
                    datos[4],
                    datos[5],
                    datos[6],
                    datos[7],
                    datos[8],
                    datos[9]
                );
            }
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
            string[] meses =
            {
                "Ene", "Feb", "Mar", "Abr",
                "May", "Jun", "Jul", "Ago",
                "Sep", "Oct", "Nov", "Dic"
            };

            int[] cantidades =
            {
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            };

            // Recorrer expedientes filtrados
            for (int i = 0;
                 i < cantidad;
                 i++)
            {
                string[] datos =
                    filtrados[i].Split('|');

                DateTime fecha;

                if (DateTime.TryParse(
                    datos[1].Trim(),
                    out fecha))
                {
                    int mes =
                        fecha.Month - 1;

                    cantidades[mes]++;
                }
            }

            // Limpiar gráfico
            gunaLineDataset1.DataPoints.Clear();

            // Agregar resultados
            for (int i = 0;
                 i < meses.Length;
                 i++)
            {
                gunaLineDataset1.DataPoints.Add(
                    new LPoint(
                        meses[i],
                        cantidades[i]
                    )
                );
            }
        }
    }
}