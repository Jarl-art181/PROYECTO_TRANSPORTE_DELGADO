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

            // Ajustar el diseño al tamaño y escalado de la pantalla
            Escalado.AjustarPantallaCompleta(this);

           

            // ==========================================
            // CARGA INICIAL DEL FORMULARIO
            // ==========================================

            // Filtros de fecha: dd/MM/yyyy (al escribir y al pegar)
            Restricciones.Fecha(Text_fechaantes);
            Restricciones.Fecha(text_fechahasta);

            CargarTipos();
            CargarEstadosCombo();

            // Leer el archivo UNA sola vez y mostrar
            // tabla, tarjetas y gráficos con todos los datos
            string[] expedientes = Funciones.LeerExpedientesValidos();

            MostrarReporte(expedientes, expedientes.Length);
        }


        // =========================================================
        // MOSTRAR TABLA, TARJETAS Y GRÁFICOS
        // =========================================================

        private void MostrarReporte(string[] expedientes, int cantidad)
        {
            MostrarTablaFiltrada(expedientes, cantidad);
            ActualizarTarjetas(expedientes, cantidad);
            ActualizarGraficoEstados(expedientes, cantidad);
            ActualizarGraficoTipos(expedientes, cantidad);
            ActualizarGraficoMeses(expedientes, cantidad);
        }


        // =========================================================
        // COMBOBOX - TIPO DE DOCUMENTO
        // =========================================================

        private void CargarTipos()
        {
            Funciones.LlenarCombo(comb_tipodedocumento, Funciones.TiposDocumento);

            comb_tipodedocumento.Items.Insert(0, "Todos");

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
        // NAVEGACIÓN
        // Abre el formulario elegido y cierra el actual para que
        // no se acumulen ventanas abiertas.
        // =========================================================

        private void AbrirFormulario(Form ventana)
        {
            ventana.Show();
            this.Close();
        }


        // =========================================================
        // BOTÓN SALIR
        // =========================================================

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }


        // =========================================================
        // BOTÓN GESTIONAR EXPEDIENTE
        // =========================================================

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new GESTIONAR_EXPEDIENTE());
        }


        // =========================================================
        // BOTÓN BUSCAR EXPEDIENTE
        // =========================================================

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new BUSCAR_EXPEDIENTE());
        }


        // =========================================================
        // BOTÓN INICIO
        // =========================================================

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new INICIO());
        }


        // =========================================================
        // BOTÓN REGISTRAR EXPEDIENTE
        // =========================================================

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REGISTRO_DE_EXPEDIENTE());
        }


        // =========================================================
        // BOTÓN CONSULTAR EXPEDIENTE
        // =========================================================

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONSULTAR_EXPEDIENTE());
        }


        // =========================================================
        // BOTÓN CONFIGURACIÓN
        // =========================================================

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONFIGURACIÓN());
        }


        // =========================================================
        // LOAD DEL FORMULARIO
        // =========================================================

        private void REPORTE_Load(object sender, EventArgs e)
        {

        }


        // =========================================================
        // BOTÓN GENERAR REPORTE
        // =========================================================

        private void but_generar_Click(
            object sender,
            EventArgs e)
        {
            // Verificar archivo
            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                MessageBox.Show(
                    "No se encontró el archivo de expedientes.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Leer solo expedientes válidos
            // (sin líneas vacías ni registros incompletos)
            string[] expedientes =
                Funciones.LeerExpedientesValidos();

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
                if (!Funciones.ConvertirFecha(textoDesde, out fechaDesde))
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

                if (!Funciones.ConvertirFecha(textoHasta, out fechaHasta))
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


            // Nota: las fechas se interpretan siempre como
            // dd/MM/yyyy, sin depender del idioma de Windows.

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

                bool cumple = true;


                // ======================================
                // FILTRO POR FECHA
                // (solo cuando se ingresaron ambas fechas)
                // ======================================

                if (usarFechaDesde || usarFechaHasta)
                {
                    DateTime fecha;

                    if (!Funciones.ConvertirFecha(datos[1], out fecha))
                    {
                        // Fecha incorrecta: no entra en el período
                        cumple = false;
                    }
                    else if (fecha.Date < fechaDesde.Date ||
                             fecha.Date > fechaHasta.Date)
                    {
                        cumple = false;
                    }
                }


                // ======================================
                // FILTRO POR TIPO DE DOCUMENTO
                // ======================================

                if (tipoSeleccionado != "" && tipoSeleccionado != "Todos")
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

                if (estadoSeleccionado != "" && estadoSeleccionado != "Todos")
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
            // MOSTRAR RESULTADOS EN TABLA, TARJETAS Y GRÁFICOS
            // ==========================================

            MostrarReporte(filtrados, cantidadFiltrados);


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
                if (!Funciones.EsRegistroValido(filtrados[i]))
                {
                    continue;
                }

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
                if (!Funciones.EsRegistroValido(filtrados[i]))
                {
                    continue;
                }

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

            // Redibujar el gráfico con los nuevos datos
            GRAFICO_DONA.Update();


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
            string[] tipos = Funciones.TiposDocumento;

            int[] cantidades =
            {
                0, 0, 0, 0, 0
            };

            // Recorrer expedientes filtrados
            for (int i = 0;
                 i < cantidad;
                 i++)
            {
                if (!Funciones.EsRegistroValido(filtrados[i]))
                {
                    continue;
                }

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

            // Redibujar el gráfico con los nuevos datos
            GRAFICO_BARRAS.Update();
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

            System.Globalization.CultureInfo cultura =
                new System.Globalization.CultureInfo("es-PE");

            foreach (var elemento in expedientesPorMes)
            {
                // Ejemplo: oct 2026
                string nombreMes = elemento.Key.ToString(
                    "MMM yyyy",
                    cultura
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

            // Redibujar el gráfico con los nuevos datos
            GRAFICO_LINEAL.Update();
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

            string numero = Funciones.NormalizarNumero(datos[0]);

            if (numero == "")
            {
                numero = datos[0].Trim();
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