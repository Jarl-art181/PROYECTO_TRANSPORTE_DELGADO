using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class GESTIONAR_EXPEDIENTE : Form
    {
        private string rutaPDFSeleccionado = "";
        public GESTIONAR_EXPEDIENTE()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            CargarExpedientes();
            combo_orden.Items.Clear();

            combo_orden.Items.Add("N.º de Expediente");
            combo_orden.Items.Add("Fecha");
            combo_orden.Items.Add("Tipo de documento");
            combo_orden.Items.Add("Solicitante");
            combo_orden.Items.Add("Empresa");
            combo_orden.Items.Add("Estado");

            combo_orden.SelectedIndex = -1;
            dgvExpedientes.BorderStyle = BorderStyle.None;

            // Quitar espacio entre encabezado y primera fila
            dgvExpedientes.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            dgvExpedientes.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            CargarExpedientes();
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            INICIO ventana = new INICIO();
            ventana.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana = new REGISTRO_DE_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana = new CONSULTAR_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana = new BUSCAR_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            REPORTES ventana = new REPORTES();
            ventana.Show();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana = new CONFIGURACIÓN();
            ventana.Show();
        }

        private void GESTIONAR_EXPEDIENTE_Load(object sender, EventArgs e)
        {

        }



        private void CargarExpedientes()
        {
            // ==========================================
            // RUTA DEL ARCHIVO
            // ==========================================

            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            // Limpiar los registros anteriores
            dgvExpedientes.Rows.Clear();

            // Verificar que exista el archivo
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

            try
            {
                // ==========================================
                // LEER EXPEDIENTES
                // ==========================================

                string[] registros =
                    File.ReadAllLines(rutaArchivo);

                // RECORRER CON FOR
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

                    // ======================================
                    // NÚMERO DE EXPEDIENTE
                    // ======================================

                    string numero = datos[0].Trim();

                    // Evitar repetir EXP-
                    if (numero.StartsWith(
                        "EXP-",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        numero = numero.Substring(4);
                    }

                    // ======================================
                    // NÚMERO DE PLACA
                    // ======================================

                    string placa = "Sin registrar";

                    if (datos.Length > 11)
                    {
                        if (!string.IsNullOrWhiteSpace(datos[11]))
                        {
                            placa = datos[11]
                                .Trim()
                                .ToUpperInvariant();
                        }
                    }

                    // ======================================
                    // CREAR FILA EN LA TABLA
                    // ======================================

                    int fila = dgvExpedientes.Rows.Add();

                    // N.º EXPEDIENTE
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

                // Quitar selección inicial
                dgvExpedientes.ClearSelection();

                dgvExpedientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los expedientes:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private void But_Ordenar_Click(object sender, EventArgs e)
        {
            // ==========================================
            // 1. VALIDAR CRITERIO DE ORDENAMIENTO
            // ==========================================

            if (combo_orden.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Seleccione un criterio para ordenar.",
                    "Ordenamiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // ==========================================
            // 2. VALIDAR ASCENDENTE O DESCENDENTE
            // ==========================================

            if (!but_Ascendente.Checked && !But_descendente.Checked)
            {
                MessageBox.Show(
                    "Seleccione Ascendente o Descendente.",
                    "Ordenamiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // ==========================================
            // 3. VERIFICAR QUE EXISTAN EXPEDIENTES
            // ==========================================

            if (dgvExpedientes.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No hay expedientes registrados para ordenar.",
                    "Ordenamiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // ==========================================
            // 4. IDENTIFICAR LA COLUMNA
            // ==========================================

            string columna = "";

            switch (combo_orden.Text)
            {
                case "N.º de Expediente":
                    columna = "colExpediente";
                    break;

                case "Fecha":
                    columna = "colFechaRegistro";
                    break;

                case "Tipo de documento":
                    columna = "colDocumento";
                    break;

                case "Solicitante":
                    columna = "coloSolicitante";
                    break;

                case "Empresa":
                    columna = "coloEmpresa";
                    break;

                case "Estado":
                    columna = "coluEstadoExpediente";
                    break;

                default:
                    MessageBox.Show(
                        "Seleccione un criterio válido.",
                        "Ordenamiento",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
            }

            // ==========================================
            // 5. VALIDAR DATOS ANTES DE ORDENAR
            // ==========================================

            for (int i = 0; i < dgvExpedientes.Rows.Count; i++)
            {
                string valor = Convert.ToString(
                    dgvExpedientes.Rows[i].Cells[columna].Value
                ).Trim();

                // VALIDAR FECHAS
                if (columna == "colFechaRegistro")
                {
                    DateTime fechaValidada;

                    if (!DateTime.TryParseExact(
                        valor,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fechaValidada))
                    {
                        MessageBox.Show(
                            "Se encontró una fecha inválida.\n" +
                            "El formato correcto es dd/MM/yyyy.",
                            "Fecha incorrecta",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        return;
                    }
                }

                // VALIDAR NÚMEROS DE EXPEDIENTE
                if (columna == "colExpediente")
                {
                    string numero = valor.Replace("EXP-", "");

                    long numeroValidado;

                    if (!long.TryParse(numero, out numeroValidado))
                    {
                        MessageBox.Show(
                            "Se encontró un número de expediente inválido.",
                            "Número incorrecto",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        return;
                    }
                }
            }

            // ==========================================
            // 6. MÉTODO DE ORDENAMIENTO BURBUJA
            // ==========================================

            for (int i = 0; i < dgvExpedientes.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvExpedientes.Rows.Count - 1 - i; j++)
                {
                    // OBTENER LOS DOS DATOS A COMPARAR
                    string dato1 = Convert.ToString(
                        dgvExpedientes.Rows[j].Cells[columna].Value
                    ).Trim();

                    string dato2 = Convert.ToString(
                        dgvExpedientes.Rows[j + 1].Cells[columna].Value
                    ).Trim();

                    bool cambiar = false;

                    // ======================================
                    // ORDENAR POR FECHA
                    // ======================================

                    if (columna == "colFechaRegistro")
                    {
                        DateTime fecha1 = DateTime.ParseExact(
                            dato1,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture
                        );

                        DateTime fecha2 = DateTime.ParseExact(
                            dato2,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture
                        );

                        // ASCENDENTE: ANTIGUA A RECIENTE
                        if (but_Ascendente.Checked)
                        {
                            if (fecha1 > fecha2)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: RECIENTE A ANTIGUA
                        if (But_descendente.Checked)
                        {
                            if (fecha1 < fecha2)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // ORDENAR POR NÚMERO DE EXPEDIENTE
                    // ======================================

                    else if (columna == "colExpediente")
                    {
                        string texto1 = dato1.Replace("EXP-", "");
                        string texto2 = dato2.Replace("EXP-", "");

                        long numero1 = long.Parse(texto1);
                        long numero2 = long.Parse(texto2);

                        // ASCENDENTE: MENOR A MAYOR
                        if (but_Ascendente.Checked)
                        {
                            if (numero1 > numero2)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: MAYOR A MENOR
                        if (But_descendente.Checked)
                        {
                            if (numero1 < numero2)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // ORDENAR POR TEXTO
                    // ======================================

                    else
                    {
                        int comparacion = string.Compare(
                            dato1,
                            dato2,
                            StringComparison.CurrentCultureIgnoreCase
                        );

                        // ASCENDENTE: A - Z
                        if (but_Ascendente.Checked)
                        {
                            if (comparacion > 0)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: Z - A
                        if (But_descendente.Checked)
                        {
                            if (comparacion < 0)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // 7. INTERCAMBIAR FILAS COMPLETAS
                    // ======================================

                    if (cambiar)
                    {
                        for (int k = 0; k < dgvExpedientes.Columns.Count; k++)
                        {
                            object temporal =
                                dgvExpedientes.Rows[j].Cells[k].Value;

                            dgvExpedientes.Rows[j].Cells[k].Value =
                                dgvExpedientes.Rows[j + 1].Cells[k].Value;

                            dgvExpedientes.Rows[j + 1].Cells[k].Value =
                                temporal;
                        }
                    }
                }
            }

            // ==========================================
            // 8. QUITAR SELECCIÓN
            // ==========================================

            dgvExpedientes.ClearSelection();

            dgvExpedientes.Refresh();

            // ==========================================
            // 9. MENSAJE FINAL
            // ==========================================

            MessageBox.Show(
                "Expedientes ordenados correctamente.",
                "Ordenamiento",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        private void but_eliminar_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Seleccione un expediente para eliminar.",
                    "Eliminar expediente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Obtener expediente seleccionado
            string expedienteSeleccionado =
                dgvExpedientes.SelectedRows[0]
                .Cells["colExpediente"]
                .Value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(expedienteSeleccionado))
            {
                MessageBox.Show(
                    "No se pudo identificar el expediente.",
                    "Eliminar expediente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Quitar EXP-
            string numeroExpediente =
                expedienteSeleccionado.Replace("EXP-", "");
            // Confirmar eliminación según configuración
            if (MostrarConfirmacionAlEliminar())
            {
                DialogResult respuesta = MessageBox.Show(
                    "¿Está seguro de eliminar el expediente " +
                    expedienteSeleccionado + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }
            }

            // Ruta del archivo
            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

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

            // Leer todos los registros
            string[] registros = File.ReadAllLines(rutaArchivo);

            // Arreglo para guardar los registros que permanecerán
            string[] nuevosRegistros = new string[registros.Length];

            int posicion = 0;

            // Recorrer los registros
            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (datos.Length < 11)
                {
                    continue;
                }

                // Buscar el expediente seleccionado
                if (datos[0] == numeroExpediente)
                {
                    // Obtener la ruta del PDF
                    string rutaPDF = datos[10];

                    if (!string.IsNullOrWhiteSpace(rutaPDF) &&
       !rutaPDF.Equals(
           "Sin PDF",
           StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(rutaPDF))
                        {
                            File.Delete(rutaPDF);
                        }
                    }
                    // No guardar este registro
                    continue;
                }

                // Guardar los demás registros
                nuevosRegistros[posicion] = registros[i];

                posicion++;
            }

            // Crear un arreglo con el tamaño exacto
            string[] registrosFinales = new string[posicion];

            // Copiar los registros que permanecen
            for (int i = 0; i < posicion; i++)
            {
                registrosFinales[i] = nuevosRegistros[i];
            }

            // Guardar nuevamente el archivo
            File.WriteAllLines(
                rutaArchivo,
                registrosFinales
            );

            // Actualizar la tabla
            CargarExpedientes();

            MessageBox.Show(
                "El expediente " + expedienteSeleccionado +
                " fue eliminado correctamente.",
                "Expediente eliminado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }



        /// VER DETALLE

        private void but_verDetalle_Click(object sender, EventArgs e)
        {
            // Verificar que se haya seleccionado una fila
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Seleccione un expediente para ver el detalle.",
                    "Ver detalle",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Obtener el número del expediente
            string expedienteSeleccionado = "";

            object valorCelda =
                dgvExpedientes.SelectedRows[0]
                .Cells["colExpediente"]
                .Value;

            if (valorCelda != null)
            {
                expedienteSeleccionado = valorCelda.ToString();
            }

            // Verificar que se haya obtenido el expediente
            if (expedienteSeleccionado == "")
            {
                MessageBox.Show(
                    "No se pudo identificar el expediente.",
                    "Ver detalle",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Quitar EXP-
            string numeroExpediente =
                expedienteSeleccionado.Replace("EXP-", "");

            // Abrir el formulario en modo Ver Detalle
            VER_DETALLE_DEL_EXPEDIENTE ventana =
                new VER_DETALLE_DEL_EXPEDIENTE(numeroExpediente);

            ventana.ShowDialog();
        }

        private void But_Modificar_Click(object sender, EventArgs e)
        {
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Seleccione un expediente para modificar.",
                    "Modificar expediente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string expedienteSeleccionado = "";

            object valorCelda =
                dgvExpedientes.SelectedRows[0]
                .Cells["colExpediente"]
                .Value;

            if (valorCelda != null)
            {
                expedienteSeleccionado = valorCelda.ToString();
            }

            if (expedienteSeleccionado == "")
            {
                MessageBox.Show(
                    "No se pudo identificar el expediente.",
                    "Modificar expediente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string numeroExpediente =
                expedienteSeleccionado.Replace("EXP-", "");

            MODIFICAR_DETALLE_DEL_EXPEDIENTE ventana =
                new MODIFICAR_DETALLE_DEL_EXPEDIENTE(
                    numeroExpediente
                );

            ventana.ShowDialog();

            CargarExpedientes();




        }

        private bool MostrarConfirmacionAlEliminar()
        {
            string rutaOpciones = Path.Combine(
                Application.StartupPath,
                "Datos",
                "opciones.txt"
            );

            // Si no existe configuración,
            // por seguridad mostramos confirmación
            if (!File.Exists(rutaOpciones))
                return true;

            string contenido = File.ReadAllText(rutaOpciones);

            string[] opciones = contenido.Split('|');

            if (opciones.Length >= 5)
            {
                bool.TryParse(
                    opciones[4],
                    out bool confirmar
                );

                return confirmar;
            }

            return true;
        }

    }
   
    
}
