using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class VER_DETALLE_DEL_EXPEDIENTE : Form
    {
        // RUTA DEL PDF

        private string rutaPDFSeleccionado = "";


        // CONSTRUCTOR SOLO VER DETALLE

        public VER_DETALLE_DEL_EXPEDIENTE(
            string numeroExpediente)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CargarExpediente(numeroExpediente);
        }


        // CARGAR EXPEDIENTE

        private void CargarExpediente(
            string numeroExpediente)
        {
            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );


            // VERIFICAR ARCHIVO

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


            // LEER REGISTROS

            string[] registros =
                File.ReadAllLines(rutaArchivo);


            // BUSCAR EXPEDIENTE

            for (int i = 0;
                 i < registros.Length;
                 i++)
            {
                if (string.IsNullOrWhiteSpace(
                    registros[i]))
                {
                    continue;
                }


                string[] datos =
                    registros[i].Split('|');


                // El registro debe tener 11 datos
                if (datos.Length < 11)
                {
                    continue;
                }


                // COMPARAR NÚMERO DE EXPEDIENTE

                if (datos[0] == numeroExpediente)
                {
                    // DATOS DEL EXPEDIENTE

                    text_numer_expe.Text =
                        "EXP-" + datos[0];

                    text_fecha.Text =
                        datos[1];

                    text_tipoDocumento.Text =
                        datos[2];

                    text_descripcion.Text =
                        datos[3];

                    text_solicitante.Text =
                        datos[4];

                    text_dni.Text =
                        datos[5];

                    tex_empresa.Text =
                        datos[6];

                    text_telefono.Text =
                        datos[7];

                    text_correo.Text =
                        datos[8];

                    text_estado.Text =
                        datos[9];


                    // ==================================
                    // PDF
                    // ==================================

                    string rutaPDF =
                        datos[10];

                    MostrarPDF(rutaPDF);


                    // Ya encontramos el expediente
                    break;
                }
            }
        }


        // ==========================================
        // MOSTRAR PDF
        // ==========================================

        private void MostrarPDF(
            string rutaPDF)
        {
            // ======================================
            // NO TIENE PDF
            // ======================================

            if (string.IsNullOrWhiteSpace(rutaPDF) ||
                rutaPDF == "Sin PDF")
            {
                rutaPDFSeleccionado = "";

                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }


            // ======================================
            // VERIFICAR SI EXISTE EL PDF
            // ======================================

            if (!File.Exists(rutaPDF))
            {
                rutaPDFSeleccionado = "";

                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ======================================
            // GUARDAR RUTA
            // ======================================

            rutaPDFSeleccionado = rutaPDF;


            // ======================================
            // MOSTRAR PDF EN WEBVIEW2
            // ======================================

            webViewPDF.Source =
                new Uri(rutaPDF);
        }


        // ==========================================
        // LOAD
        // ==========================================

        private void DETALLE_DEL_EXPEDIENTE_Load(
            object sender,
            EventArgs e)
        {
        }


        // ==========================================
        // CERRAR
        // ==========================================

        private void but_cerrar_Click(
            object sender,
            EventArgs e)
        {
            this.Close();
        }


        // ==========================================
        // DESCARGAR PDF
        // ==========================================

        private void but_descargar_Click(
            object sender,
            EventArgs e)
        {
            // ======================================
            // VERIFICAR PDF
            // ======================================

            if (string.IsNullOrWhiteSpace(
                rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }


            // ======================================
            // VERIFICAR QUE EXISTA
            // ======================================

            if (!File.Exists(
                rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ======================================
            // CREAR GUARDAR COMO
            // ======================================

            SaveFileDialog guardar =
                new SaveFileDialog();


            guardar.Filter =
                "Archivo PDF (*.pdf)|*.pdf";


            guardar.FileName =
                Path.GetFileName(
                    rutaPDFSeleccionado);


            // ======================================
            // SELECCIONAR UBICACIÓN
            // ======================================

            if (guardar.ShowDialog() ==
                DialogResult.OK)
            {
                File.Copy(
                    rutaPDFSeleccionado,
                    guardar.FileName,
                    true
                );


                MessageBox.Show(
                    "PDF descargado correctamente.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // ==========================================
        // ABRIR PDF
        // ==========================================

        private void But_abrirpdf_Click(
            object sender,
            EventArgs e)
        {
            // ======================================
            // VERIFICAR PDF
            // ======================================

            if (string.IsNullOrWhiteSpace(
                rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }


            // ======================================
            // VERIFICAR QUE EXISTA
            // ======================================

            if (!File.Exists(
                rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ======================================
            // ABRIR PDF
            // ======================================

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = rutaPDFSeleccionado,
                    UseShellExecute = true
                }
            );
        }
    }
}