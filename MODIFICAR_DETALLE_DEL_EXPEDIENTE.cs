
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

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            numeroExpedienteActual = numeroExpediente;

            CargarExpediente(numeroExpediente);
        }

        // ==========================================
        // CARGAR EXPEDIENTE
        // ==========================================

        private void CargarExpediente(string numeroExpediente)
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

            string[] registros = File.ReadAllLines(rutaArchivo);

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

                // Buscar el expediente por número
                if (datos[0].Trim() == numeroExpediente.Trim())
                {
                    // Número de expediente
                    text_numer_expe.Text = "EXP-" + datos[0].Trim();

                    // Fecha
                    text_fecha.Text = datos[1];

                    // Tipo de documento
                    text_tipoDocumento.Text = datos[2];

                    // Placa: campo número 12 del archivo
                    if (datos.Length > 11)
                    {
                        text_placa.Text = datos[11].Trim();
                    }
                    else
                    {
                        text_placa.Text = "";
                    }

                    // Descripción
                    text_descripcion.Text = datos[3];

                    // Solicitante
                    text_solicitante.Text = datos[4];

                    // DNI
                    text_dni.Text = datos[5];

                    // Empresa
                    tex_empresa.Text = datos[6];

                    // Teléfono
                    text_telefono.Text = datos[7];

                    // Correo electrónico
                    text_correo.Text = datos[8];

                    // Cargar estados
                    text_estado.Items.Clear();

                    text_estado.Items.Add("En proceso");
                    text_estado.Items.Add("Pendiente");
                    text_estado.Items.Add("Finalizado");

                    text_estado.Text = datos[9];

                    // Cargar PDF
                    string rutaPDF = datos[10];

                    MostrarPDF(rutaPDF);

                    // Permitir modificar los datos
                    text_fecha.ReadOnly = false;
                    text_tipoDocumento.ReadOnly = false;
                    text_placa.ReadOnly = false;
                    text_descripcion.ReadOnly = false;
                    text_solicitante.ReadOnly = false;
                    text_dni.ReadOnly = false;
                    tex_empresa.ReadOnly = false;
                    text_telefono.ReadOnly = false;
                    text_correo.ReadOnly = false;

                    // El número de expediente no se modifica
                    text_numer_expe.ReadOnly = true;

                    break;
                }
            }
        }

        // ==========================================
        // MOSTRAR PDF
        // ==========================================

        private void MostrarPDF(string rutaPDF)
        {
            // Verificar si el expediente tiene PDF
            if (string.IsNullOrWhiteSpace(rutaPDF) ||
                rutaPDF == "Sin PDF")
            {
                rutaPDFSeleccionado = "";
                webViewPDF.Source = null;
                return;
            }

            // Verificar que exista físicamente
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

            rutaPDFSeleccionado = rutaPDF;

            // Mostrar documento en el visor
            webViewPDF.Source = new Uri(rutaPDF);
        }

        // ==========================================
        // ADJUNTAR NUEVO PDF
        // ==========================================

        private void but_adjuntarPDF_Click(
            object sender, EventArgs e)
        {
            OpenFileDialog seleccionar = new OpenFileDialog();

            seleccionar.Filter =
                "Archivo PDF (*.pdf)|*.pdf";

            seleccionar.Title =
                "Seleccionar nuevo PDF";

            if (seleccionar.ShowDialog() == DialogResult.OK)
            {
                FileInfo archivo =
                    new FileInfo(seleccionar.FileName);

                double pesoMB =
                    archivo.Length / (1024.0 * 1024.0);

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

                rutaPDFSeleccionado = seleccionar.FileName;

                webViewPDF.Source =
                    new Uri(rutaPDFSeleccionado);

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
            object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea quitar el PDF del expediente?",
                "Quitar PDF",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            // Quitar asociación con el PDF
            rutaPDFSeleccionado = "";

            // Limpiar visor
            webViewPDF.Source = null;

            MessageBox.Show(
                "El PDF será quitado al guardar los cambios.",
                "PDF",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ==========================================
        // GUARDAR CAMBIOS - MÉTODO COMPATIBLE
        // ==========================================

        private void but_guardarCambios_Click(
            object sender, EventArgs e)
        {
            // Utilizar un único método de guardado
            But_guardarpdf_Click(sender, e);
        }

        // ==========================================
        // BOTÓN ADJUNTAR NUEVO PDF
        // ==========================================

        private void But_adjuntarpdf_Click_1(
            object sender, EventArgs e)
        {
            OpenFileDialog seleccionar =
                new OpenFileDialog();

            seleccionar.Filter =
                "Archivos PDF (*.pdf)|*.pdf";

            if (seleccionar.ShowDialog() == DialogResult.OK)
            {
                FileInfo archivo =
                    new FileInfo(seleccionar.FileName);

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

        // ==========================================
        // BOTÓN QUITAR PDF ACTUAL
        // ==========================================

        private void but_quitarpdf_Click_1(
            object sender, EventArgs e)
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

        // ==========================================
        // BOTÓN CANCELAR
        // ==========================================

        private void but_cancelar_Click_1(
            object sender, EventArgs e)
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

        // ==========================================
        // GUARDAR CAMBIOS PRINCIPAL
        // ==========================================

        private void But_guardarpdf_Click(
            object sender, EventArgs e)
        {
            // ======================================
            // 1. VALIDAR PLACA
            // ======================================

            string placa =
                text_placa.Text.Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(placa))
            {
                MessageBox.Show(
                    "Ingrese el número de placa del vehículo.",
                    "Placa obligatoria",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                text_placa.Focus();
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                placa, @"^[A-Z0-9]{3}-[A-Z0-9]{3}$"))
            {
                MessageBox.Show(
                    "La placa debe tener un formato como ABC-123 o A1B-145.",
                    "Placa inválida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                text_placa.Focus();
                return;
            }

            // ======================================
            // 2. VALIDAR CAMPOS OBLIGATORIOS
            // ======================================

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

           
            string telefonoValidar = text_telefono.Text.Trim();

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                telefonoValidar, @"^9\d{8}$"))
            {
                MessageBox.Show(
                    "El número de celular debe tener exactamente " +
                    "9 dígitos y comenzar con 9.\n\n" +
                    "Ejemplo: 987654321.",
                    "Teléfono inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                text_telefono.Focus();
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

            // ======================================
            // 4. LEER EXPEDIENTES
            // ======================================

            string[] registros = File.ReadAllLines(rutaArchivo);

            string[] nuevosRegistros =
                new string[registros.Length];

            // La variable debe declararse ANTES del for
            bool expedienteEncontrado = false;

            // ======================================
            // 5. BUSCAR Y MODIFICAR EXPEDIENTE
            // ======================================

            for (int i = 0; i < registros.Length; i++)
            {
                // Conservar líneas vacías
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    nuevosRegistros[i] = registros[i];
                    continue;
                }

                string[] datos = registros[i].Split('|');

                // Conservar registros antiguos o incompletos
                if (datos.Length < 11)
                {
                    nuevosRegistros[i] = registros[i];
                    continue;
                }

                // Buscar el expediente correspondiente
                if (datos[0].Trim() == numeroExpedienteActual.Trim())
                {
                    // CORRECCIÓN PRINCIPAL
                    // El expediente sí fue encontrado
                    expedienteEncontrado = true;

                    // Obtener PDF
                    string pdf = rutaPDFSeleccionado;

                    if (string.IsNullOrWhiteSpace(pdf))
                    {
                        pdf = "Sin PDF";
                    }

                    // ==================================
                    // GUARDAR LOS 12 CAMPOS
                    // ==================================

                    nuevosRegistros[i] =
                        datos[0] + "|" +
                        text_fecha.Text.Trim() + "|" +
                        text_tipoDocumento.Text.Trim() + "|" +
                        text_descripcion.Text.Trim() + "|" +
                        text_solicitante.Text.Trim() + "|" +
                        text_dni.Text.Trim() + "|" +
                        tex_empresa.Text.Trim() + "|" +
                        text_telefono.Text.Trim() + "|" +
                        text_correo.Text.Trim() + "|" +
                        text_estado.Text.Trim() + "|" +
                        pdf + "|" +
                        placa;
                }
                else
                {
                    // Mantener los demás registros
                    nuevosRegistros[i] = registros[i];
                }
            }

            // ======================================
            // 6. VERIFICAR SI SE ENCONTRÓ
            // ======================================

            if (!expedienteEncontrado)
            {
                MessageBox.Show(
                    "No se encontró el expediente que desea modificar.",
                    "Expediente no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // ======================================
            // 7. GUARDAR CAMBIOS EN EL ARCHIVO
            // ======================================

            File.WriteAllLines(
                rutaArchivo,
                nuevosRegistros
            );

            // ======================================
            // 8. MOSTRAR CONFIRMACIÓN
            // ======================================

            MessageBox.Show(
                "Los datos del expediente se modificaron correctamente.",
                "Guardar cambios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // ======================================
            // 9. CERRAR FORMULARIO
            // ======================================

            this.Close();
        }
    }
}
