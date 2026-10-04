using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class MODIFICAR_DETALLE_DEL_EXPEDIENTE : Form
    {
        // ==========================================
        // VARIABLES
        // ==========================================

        private string rutaPDFSeleccionado = "";

        private string numeroExpedienteActual = "";


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public MODIFICAR_DETALLE_DEL_EXPEDIENTE(
            string numeroExpediente)
        {
            InitializeComponent();

            this.StartPosition =
                FormStartPosition.CenterScreen;

            this.FormBorderStyle =
                FormBorderStyle.FixedSingle;

            this.MaximizeBox = false;

            this.MinimizeBox = false;


            // Guardar número del expediente
            numeroExpedienteActual =
                numeroExpediente;


            // Cargar expediente
            CargarExpediente(
                numeroExpediente
            );
        }


        // ==========================================
        // CARGAR EXPEDIENTE
        // ==========================================

        private void CargarExpediente(
            string numeroExpediente)
        {
            string rutaArchivo =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "expedientes.txt"
                );


            // ======================================
            // VERIFICAR ARCHIVO
            // ======================================

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


            // ======================================
            // LEER REGISTROS
            // ======================================

            string[] registros =
                File.ReadAllLines(rutaArchivo);


            // ======================================
            // BUSCAR EXPEDIENTE
            // ======================================

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


                if (datos.Length < 11)
                {
                    continue;
                }


                // ==================================
                // COMPARAR NÚMERO DE EXPEDIENTE
                // ==================================

                if (datos[0] ==
                    numeroExpediente)
                {
                    // ==============================
                    // CARGAR DATOS
                    // ==============================

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


                    // ==============================
                    // CARGAR COMBO ESTADO
                    // ==============================

                    text_estado.Items.Clear();

                    text_estado.Items.Add(
                        "En proceso"
                    );

                    text_estado.Items.Add(
                        "Pendiente"
                    );

                    text_estado.Items.Add(
                        "Finalizado"
                    );

                    text_estado.Text =
                        datos[9];


                    // ==============================
                    // CARGAR PDF
                    // ==============================

                    string rutaPDF =
                        datos[10];

                    MostrarPDF(rutaPDF);


                    // ==============================
                    // CAMPOS EDITABLES
                    // ==============================

                    text_fecha.ReadOnly =
                        false;

                    text_tipoDocumento.ReadOnly =
                        false;

                    text_descripcion.ReadOnly =
                        false;

                    text_solicitante.ReadOnly =
                        false;

                    text_dni.ReadOnly =
                        false;

                    tex_empresa.ReadOnly =
                        false;

                    text_telefono.ReadOnly =
                        false;

                    text_correo.ReadOnly =
                        false;


                    // ==============================
                    // NÚMERO NO SE MODIFICA
                    // ==============================

                    text_numer_expe.ReadOnly =
                        true;


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
            // SIN PDF
            // ======================================

            if (string.IsNullOrWhiteSpace(rutaPDF) ||
                rutaPDF == "Sin PDF")
            {
                rutaPDFSeleccionado = "";

                webViewPDF.Source = null;

                return;
            }


            // ======================================
            // VERIFICAR SI EXISTE
            // ======================================

            if (!File.Exists(rutaPDF))
            {
                rutaPDFSeleccionado = "";

                webViewPDF.Source = null;

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

            rutaPDFSeleccionado =
                rutaPDF;


            // ======================================
            // MOSTRAR PDF
            // ======================================

            webViewPDF.Source =
                new Uri(rutaPDF);
        }


        // ==========================================
        // ADJUNTAR NUEVO PDF
        // ==========================================

        private void but_adjuntarPDF_Click(
            object sender,
            EventArgs e)
        {
            OpenFileDialog seleccionar =
                new OpenFileDialog();


            seleccionar.Filter =
                "Archivo PDF (*.pdf)|*.pdf";


            seleccionar.Title =
                "Seleccionar nuevo PDF";


            if (seleccionar.ShowDialog() ==
                DialogResult.OK)
            {
                // ==============================
                // INFORMACIÓN DEL ARCHIVO
                // ==============================

                FileInfo archivo =
                    new FileInfo(
                        seleccionar.FileName
                    );


                // ==============================
                // CALCULAR TAMAÑO
                // ==============================

                double pesoMB =
                    archivo.Length /
                    (1024.0 * 1024.0);


                // ==============================
                // MÁXIMO 10 MB
                // ==============================

                if (pesoMB > 10)
                {
                    MessageBox.Show(
                        "El PDF no puede superar los 10 MB.",
                        "PDF demasiado grande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }


                // ==============================
                // GUARDAR NUEVA RUTA
                // ==============================

                rutaPDFSeleccionado =
                    seleccionar.FileName;


                // ==============================
                // MOSTRAR NUEVO PDF
                // ==============================

                webViewPDF.Source =
                    new Uri(
                        rutaPDFSeleccionado
                    );


                MessageBox.Show(
                    "Nuevo PDF seleccionado correctamente.",
                    "PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // ==========================================
        // QUITAR PDF
        // ==========================================

        private void but_quitarPDF_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Desea quitar el PDF del expediente?",
                    "Quitar PDF",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (respuesta !=
                DialogResult.Yes)
            {
                return;
            }


            // Quitar ruta
            rutaPDFSeleccionado = "";


            // Quitar PDF del visor
            webViewPDF.Source = null;


            MessageBox.Show(
                "El PDF será quitado al guardar los cambios.",
                "PDF",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // ==========================================
        // ABRIR PDF
        // ==========================================

      

        // ==========================================
        // GUARDAR CAMBIOS
        // ==========================================

        private void but_guardarCambios_Click(
            object sender,
            EventArgs e)
        {
            // ======================================
            // VALIDAR CAMPOS
            // ======================================

            if (string.IsNullOrWhiteSpace(
                text_fecha.Text) ||

                string.IsNullOrWhiteSpace(
                text_tipoDocumento.Text) ||

                string.IsNullOrWhiteSpace(
                text_descripcion.Text) ||

                string.IsNullOrWhiteSpace(
                text_solicitante.Text) ||

                string.IsNullOrWhiteSpace(
                text_dni.Text) ||

                string.IsNullOrWhiteSpace(
                tex_empresa.Text) ||

                string.IsNullOrWhiteSpace(
                text_telefono.Text) ||

                string.IsNullOrWhiteSpace(
                text_correo.Text) ||

                string.IsNullOrWhiteSpace(
                text_estado.Text))
            {
                MessageBox.Show(
                    "Complete todos los campos.",
                    "Datos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ======================================
            // RUTA DEL ARCHIVO
            // ======================================

            string rutaArchivo =
                Path.Combine(
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


            // ======================================
            // LEER REGISTROS
            // ======================================

            string[] registros =
                File.ReadAllLines(rutaArchivo);


            // ======================================
            // ARREGLO NUEVO
            // ======================================

            string[] nuevosRegistros =
                new string[registros.Length];


            // ======================================
            // MODIFICAR REGISTRO
            // ======================================

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


                if (datos.Length < 11)
                {
                    continue;
                }


                // ==================================
                // EXPEDIENTE ENCONTRADO
                // ==================================

                if (datos[0] ==
                    numeroExpedienteActual)
                {
                    string rutaPDF =
                        rutaPDFSeleccionado;


                    // ==============================
                    // SI NO TIENE PDF
                    // ==============================

                    if (string.IsNullOrWhiteSpace(
                        rutaPDF))
                    {
                        rutaPDF =
                            "Sin PDF";
                    }


                    // ==============================
                    // CREAR NUEVO REGISTRO
                    // ==============================

                    nuevosRegistros[i] =
                        datos[0] + "|" +
                        text_fecha.Text + "|" +
                        text_tipoDocumento.Text + "|" +
                        text_descripcion.Text + "|" +
                        text_solicitante.Text + "|" +
                        text_dni.Text + "|" +
                        tex_empresa.Text + "|" +
                        text_telefono.Text + "|" +
                        text_correo.Text + "|" +
                        text_estado.Text + "|" +
                        rutaPDF;
                }
                else
                {
                    // ==============================
                    // CONSERVAR REGISTRO
                    // ==============================

                    nuevosRegistros[i] =
                        registros[i];
                }
            }


            // ======================================
            // GUARDAR CAMBIOS
            // ======================================

            File.WriteAllLines(
                rutaArchivo,
                nuevosRegistros
            );


            // ======================================
            // MENSAJE
            // ======================================

            MessageBox.Show(
                "El expediente EXP-" +
                numeroExpedienteActual +
                " fue modificado correctamente.",
                "Cambios guardados",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );


            // ======================================
            // CERRAR
            // ======================================

            this.Close();
        }


        // ==========================================
        // CANCELAR
        // ==========================================

        private void but_cancelar_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Desea cancelar los cambios?",
                    "Cancelar modificación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (respuesta ==
                DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void But_adjuntarpdf_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog seleccionar = new OpenFileDialog();

            seleccionar.Filter = "Archivos PDF (*.pdf)|*.pdf";

            if (seleccionar.ShowDialog() == DialogResult.OK)
            {
                FileInfo archivo = new FileInfo(seleccionar.FileName);

                double tamañoMB =
                    archivo.Length / (1024.0 * 1024.0);

                if (tamañoMB > 10)
                {
                    MessageBox.Show(
                        "El archivo PDF no puede superar los 10 MB.",
                        "PDF demasiado grande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                rutaPDFSeleccionado = seleccionar.FileName;

                webViewPDF.Source =
                    new Uri(rutaPDFSeleccionado);

                MessageBox.Show(
                    "PDF adjuntado correctamente.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void but_quitarpdf_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No hay ningún PDF adjunto para quitar.",
                    "Quitar PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de quitar el PDF?",
                "Quitar PDF",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta == DialogResult.Yes)
            {
                rutaPDFSeleccionado = "";

                webViewPDF.Source = null;

                MessageBox.Show(
                    "El PDF ha sido quitado.",
                    "Quitar PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void but_cancelar_Click_1(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
      "¿Está seguro de cancelar la modificación?",
      "Cancelar modificación",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Question
  );

            if (respuesta == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void But_guardarpdf_Click(object sender, EventArgs e)
        {
            // Verificar los datos obligatorios
            if (string.IsNullOrWhiteSpace(text_fecha.Text) ||
                string.IsNullOrWhiteSpace(text_tipoDocumento.Text) ||
                string.IsNullOrWhiteSpace(text_descripcion.Text) ||
                string.IsNullOrWhiteSpace(text_solicitante.Text) ||
                string.IsNullOrWhiteSpace(text_dni.Text) ||
                string.IsNullOrWhiteSpace(tex_empresa.Text) ||
                string.IsNullOrWhiteSpace(text_telefono.Text) ||
                string.IsNullOrWhiteSpace(text_correo.Text) ||
                string.IsNullOrWhiteSpace(text_estado.Text))
            {
                MessageBox.Show(
                    "Complete todos los datos del expediente.",
                    "Guardar cambios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    "No existe el archivo de expedientes.",
                    "Guardar cambios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string[] registros = File.ReadAllLines(rutaArchivo);

            string[] nuevosRegistros =
                new string[registros.Length];

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    nuevosRegistros[i] = registros[i];
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (datos.Length < 11)
                {
                    nuevosRegistros[i] = registros[i];
                    continue;
                }

                if (datos[0] == numeroExpedienteActual)
                {
                    string pdf = rutaPDFSeleccionado;

                    if (string.IsNullOrWhiteSpace(pdf))
                    {
                        pdf = "Sin PDF";
                    }

                    nuevosRegistros[i] =
                        datos[0] + "|" +
                        text_fecha.Text + "|" +
                        text_tipoDocumento.Text + "|" +
                        text_descripcion.Text + "|" +
                        text_solicitante.Text + "|" +
                        text_dni.Text + "|" +
                        tex_empresa.Text + "|" +
                        text_telefono.Text + "|" +
                        text_correo.Text + "|" +
                        text_estado.Text + "|" +
                        pdf;
                }
                else
                {
                    nuevosRegistros[i] = registros[i];
                }
            }

            File.WriteAllLines(
                rutaArchivo,
                nuevosRegistros
            );

            MessageBox.Show(
                "Los datos del expediente se modificaron correctamente.",
                "Guardar cambios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.Close();
        }
    }
}