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

            dgvExpedientes.Rows.Clear();

            string[] registros = File.ReadAllLines(rutaArchivo);

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                    continue;

                string[] datos = registros[i].Split('|');

                if (datos.Length < 10)
                    continue;

                int fila = dgvExpedientes.Rows.Add();

                dgvExpedientes.Rows[fila].Cells["colExpediente"].Value =
                    "EXP-" + datos[0];

                dgvExpedientes.Rows[fila].Cells["colFechaRegistro"].Value =
                    datos[1];

                dgvExpedientes.Rows[fila].Cells["colDocumento"].Value =
                    datos[2];

                dgvExpedientes.Rows[fila].Cells["coluDescripción"].Value =
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

            dgvExpedientes.ClearSelection();
        }

        private void But_Ordenar_Click(object sender, EventArgs e)
        {

            // Verificar que se haya seleccionado un criterio
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

            // Verificar que se haya seleccionado Ascendente o Descendente
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

            // MÉTODO DE BURBUJA
            for (int i = 0; i < dgvExpedientes.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvExpedientes.Rows.Count - 1 - i; j++)
                {
                    string dato1 = "";
                    string dato2 = "";

                    // N.º DE EXPEDIENTE
                    if (combo_orden.Text == "N.º de Expediente")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[0].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[0].Value?.ToString() ?? "";
                    }

                    // FECHA
                    else if (combo_orden.Text == "Fecha")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[1].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[1].Value?.ToString() ?? "";
                    }

                    // TIPO DE DOCUMENTO
                    else if (combo_orden.Text == "Tipo de documento")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[2].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[2].Value?.ToString() ?? "";
                    }

                    // SOLICITANTE
                    else if (combo_orden.Text == "Solicitante")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[4].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[4].Value?.ToString() ?? "";
                    }

                    // EMPRESA
                    else if (combo_orden.Text == "Empresa")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[6].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[6].Value?.ToString() ?? "";
                    }

                    // ESTADO
                    else if (combo_orden.Text == "Estado")
                    {
                        dato1 = dgvExpedientes.Rows[j].Cells[9].Value?.ToString() ?? "";
                        dato2 = dgvExpedientes.Rows[j + 1].Cells[9].Value?.ToString() ?? "";
                    }

                    bool cambiar = false;

                    if (combo_orden.Text == "Fecha")
                    {
                        DateTime fecha1 = DateTime.Parse(dato1);
                        DateTime fecha2 = DateTime.Parse(dato2);

                        if (but_Ascendente.Checked)
                        {
                            if (fecha1 > fecha2)
                            {
                                cambiar = true;
                            }
                        }

                        if (But_descendente.Checked)
                        {
                            if (fecha1 < fecha2)
                            {
                                cambiar = true;
                            }
                        }
                    }
                    else
                    {
                        if (but_Ascendente.Checked)
                        {
                            if (string.Compare(dato1, dato2) > 0)
                            {
                                cambiar = true;
                            }
                        }

                        if (But_descendente.Checked)
                        {
                            if (string.Compare(dato1, dato2) < 0)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // INTERCAMBIAR LAS FILAS
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

            // Quitar selección
            dgvExpedientes.ClearSelection();

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

                    // Eliminar el PDF asociado
                    if (!string.IsNullOrWhiteSpace(rutaPDF) &&
                        rutaPDF != "Sin PDF" &&
                        File.Exists(rutaPDF))
                    {
                        File.Delete(rutaPDF);
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
