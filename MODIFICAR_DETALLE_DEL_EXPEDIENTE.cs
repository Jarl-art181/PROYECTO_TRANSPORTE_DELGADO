
using System;
using System.Windows.Forms;
using System.IO;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class MODIFICAR_DETALLE_DEL_EXPEDIENTE : Form
    {
        // ==========================================
        // VARIABLES
        // ==========================================

        // PDF que se mostrará y guardará
        private string rutaPDFSeleccionado = "";

        // PDF que tenía el expediente al abrir el formulario
        private string rutaPDFOriginal = "";

        // true cuando el usuario presiona "Quitar PDF"
        private bool pdfQuitado = false;

        // Número de expediente (8 dígitos)
        private string numeroExpedienteActual = "";

        // Número tal como está guardado en el archivo
        private string numeroGuardado = "";

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

            // Reducir la ventana si no cabe en la pantalla
            Escalado.AjustarDialogo(this);

            numeroExpedienteActual =
                Funciones.NormalizarNumero(numeroExpediente);

            CargarExpediente();

            // Límites y formatos (después de cargar, para no
            // modificar en silencio los datos ya guardados)
            Restricciones.Dni(text_dni);
            Restricciones.Celular(text_telefono);
            Restricciones.Placa(text_placa);
            Restricciones.Texto(text_solicitante, Restricciones.MAX_NOMBRE);
            Restricciones.Texto(tex_empresa, Restricciones.MAX_EMPRESA);
            Restricciones.Texto(text_correo, Restricciones.MAX_CORREO);
            Restricciones.Texto(text_descripcion, Restricciones.MAX_DESCRIPCION);
        }

        // ==========================================
        // CARGAR EXPEDIENTE
        // ==========================================

        private void CargarExpediente()
        {
            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                Funciones.Aviso(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado"
                );

                return;
            }

            // Buscar el expediente por número
            string[] datos =
                Funciones.BuscarExpediente(numeroExpedienteActual);

            if (datos == null)
            {
                Funciones.Aviso(
                    "No se encontró el expediente seleccionado.",
                    "Expediente no encontrado"
                );

                return;
            }

            numeroGuardado = datos[0].Trim();

            // Número de expediente
            text_numer_expe.Text = "EXP-" + numeroExpedienteActual;

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
            Funciones.LlenarCombo(text_estado, Funciones.Estados);

            text_estado.Text = datos[9];

            // El estado solo cambia desde Gestionar
            // (Iniciar atención / Finalizar expediente)
            text_estado.Enabled = false;

            // Un expediente finalizado se conserva solo para consulta
            if (Funciones.EsEstado(datos[9], Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente está Finalizado y no se puede modificar.\n" +
                    "Puede consultarlo en Buscar o en Ver detalle.",
                    "Modificar expediente"
                );
            }

            // Cargar PDF (campo 11; puede faltar en registros antiguos)
            if (datos.Length > 10)
            {
                rutaPDFOriginal = datos[10].Trim();
            }

            MostrarPDF(rutaPDFOriginal);

            // Permitir modificar los datos
            // La fecha de registro no se cambia
            text_fecha.ReadOnly = true;
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
        }

        // ==========================================
        // MOSTRAR PDF
        // ==========================================

        private void MostrarPDF(string rutaPDF)
        {
            rutaPDFSeleccionado = "";

            // Verificar si el expediente tiene PDF
            if (!Funciones.TienePDF(rutaPDF))
            {
                webViewPDF.Source = null;
                return;
            }

            // Verificar que exista físicamente
            if (!File.Exists(rutaPDF))
            {
                webViewPDF.Source = null;

                Funciones.Aviso(
                    "No se encontró el archivo PDF.",
                    "Documento PDF"
                );

                return;
            }

            rutaPDFSeleccionado = rutaPDF;

            try
            {
                // Mostrar documento en el visor
                webViewPDF.Source = new Uri(rutaPDF);
            }
            catch (Exception ex)
            {
                Funciones.Aviso(
                    "No se pudo mostrar el PDF.\n" + ex.Message,
                    "Documento PDF"
                );
            }
        }

        // ==========================================
        // BOTÓN ADJUNTAR NUEVO PDF
        // ==========================================

        private void But_adjuntarpdf_Click_1(
            object sender, EventArgs e)
        {
            using (OpenFileDialog seleccionar = new OpenFileDialog())
            {
                seleccionar.Filter = "Archivos PDF (*.pdf)|*.pdf";

                if (seleccionar.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (!Funciones.PDFDentroDelLimite(seleccionar.FileName))
                {
                    return;
                }

                // Se copiará a la carpeta del sistema al guardar
                MostrarPDF(seleccionar.FileName);
                pdfQuitado = false;

                if (rutaPDFSeleccionado != "")
                {
                    MessageBox.Show(
                        "PDF adjuntado correctamente.",
                        "Documento PDF",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
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
                Funciones.Aviso(
                    "No hay ningún PDF adjunto para quitar.",
                    "Quitar PDF"
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
                // Solo se quita la asociación; el archivo no se borra
                rutaPDFSeleccionado = "";
                pdfQuitado = true;

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
        // VALIDAR DATOS
        // (mismas reglas que Registrar Expediente)
        // ==========================================

        private bool ValidarFormulario()
        {
            const string OBLIGATORIO = "Campo obligatorio";

            // 1. CAMPOS VACÍOS
            if (string.IsNullOrWhiteSpace(text_fecha.Text) ||
                string.IsNullOrWhiteSpace(text_tipoDocumento.Text) ||
                string.IsNullOrWhiteSpace(text_descripcion.Text) ||
                string.IsNullOrWhiteSpace(text_solicitante.Text) ||
                string.IsNullOrWhiteSpace(text_dni.Text) ||
                string.IsNullOrWhiteSpace(tex_empresa.Text) ||
                string.IsNullOrWhiteSpace(text_telefono.Text) ||
                string.IsNullOrWhiteSpace(text_correo.Text))
            {
                Funciones.Aviso(
                    "Complete todos los datos del expediente.",
                    "Guardar cambios"
                );

                return false;
            }

            // 1b. LARGOS MÁXIMOS DE LOS TEXTOS
            if (!Restricciones.LargoValido(text_solicitante, Restricciones.MAX_NOMBRE, "Solicitante")) return false;
            if (!Restricciones.LargoValido(tex_empresa, Restricciones.MAX_EMPRESA, "Empresa")) return false;
            if (!Restricciones.LargoValido(text_correo, Restricciones.MAX_CORREO, "Correo")) return false;
            if (!Restricciones.LargoValido(text_descripcion, Restricciones.MAX_DESCRIPCION, "Descripción")) return false;

            // 2. FECHA: es de solo lectura (se conserva la fecha de registro)

            // 3. TIPO DE DOCUMENTO
            if (!Funciones.Validar(
                    Funciones.EstaEnLista(text_tipoDocumento.Text, Funciones.TiposDocumento),
                    "El tipo de documento debe ser: " +
                    string.Join(", ", Funciones.TiposDocumento) + ".",
                    "Tipo de documento inválido", text_tipoDocumento)) return false;

            // 4. NOMBRE
            if (!Funciones.Validar(Funciones.EsNombreValido(text_solicitante.Text),
                    "El nombre solo debe contener letras y espacios.",
                    "Dato inválido", text_solicitante)) return false;

            // 5. DNI
            if (!Funciones.Validar(Funciones.EsDNIValido(text_dni.Text),
                    "El DNI debe contener exactamente 8 dígitos.",
                    "DNI inválido", text_dni)) return false;

            // 6. TELÉFONO
            if (!Funciones.Validar(Funciones.EsTelefonoValido(text_telefono.Text),
                    "El número de celular debe tener exactamente " +
                    "9 dígitos y comenzar con 9.\n\nEjemplo: 987654321.",
                    "Teléfono inválido", text_telefono)) return false;

            // 7. CORREO
            if (!Funciones.Validar(Funciones.EsCorreoValido(text_correo.Text),
                    "El correo debe terminar en @gmail.com.",
                    "Correo inválido", text_correo)) return false;

            // 9. PLACA
            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(text_placa.Text),
                    "Ingrese el número de placa del vehículo.",
                    OBLIGATORIO, text_placa)) return false;

            if (!Funciones.Validar(Funciones.EsPlacaValida(text_placa.Text),
                    "La placa debe tener un formato como ABC-123 o A1B-145.",
                    "Placa inválida", text_placa)) return false;

            return true;
        }

        // ==========================================
        // GUARDAR CAMBIOS PRINCIPAL
        // ==========================================

        private void But_guardarpdf_Click(
            object sender, EventArgs e)
        {
            // 1. LEER EL EXPEDIENTE ACTUAL DEL ARCHIVO
            //    (para conservar su estado y su seguimiento)
            string[] actual = Funciones.BuscarExpediente(numeroExpedienteActual);

            if (actual == null)
            {
                Funciones.Aviso(
                    "No se encontró el expediente que desea modificar.",
                    "Expediente no encontrado"
                );

                return;
            }

            if (Funciones.EsEstado(actual[9], Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente está Finalizado y no se puede modificar.",
                    "Modificar expediente"
                );

                return;
            }

            // 2. VALIDAR
            if (!ValidarFormulario())
            {
                return;
            }

            // 2. DEFINIR EL PDF QUE SE GUARDARÁ
            string pdf = rutaPDFSeleccionado;
            bool pdfNuevo = false;

            if (string.IsNullOrWhiteSpace(pdf))
            {
                // Si el usuario no quitó el PDF, se conserva la ruta
                // anterior aunque el archivo no se haya encontrado
                if (!pdfQuitado && Funciones.TienePDF(rutaPDFOriginal))
                {
                    pdf = rutaPDFOriginal;
                }
                else
                {
                    pdf = "Sin PDF";
                }
            }
            else if (!string.Equals(
                pdf,
                rutaPDFOriginal,
                StringComparison.OrdinalIgnoreCase))
            {
                // Se adjuntó un PDF nuevo: copiarlo al sistema
                // conservando el archivo original del usuario
                pdf = Funciones.CopiarPDFAlSistema(pdf, numeroExpedienteActual);

                if (pdf == "")
                {
                    return;
                }

                pdfNuevo = true;
            }

            // 3. CREAR EL REGISTRO
            //    Se parte del expediente guardado (16 campos) y solo se
            //    cambian los datos editables. El estado (campo 9) y el
            //    seguimiento (campos 12 a 15) se conservan sin cambios.
            string[] campos = Funciones.CompletarCampos(actual);

            campos[0] = numeroGuardado;
            // campos[1] (fecha de registro) se conserva: es de solo lectura
            campos[2] = Funciones.LimpiarTexto(text_tipoDocumento.Text);
            campos[3] = Funciones.LimpiarTexto(text_descripcion.Text);
            campos[4] = Funciones.LimpiarTexto(text_solicitante.Text);
            campos[5] = Funciones.LimpiarTexto(text_dni.Text);
            campos[6] = Funciones.LimpiarTexto(tex_empresa.Text);
            campos[7] = Funciones.LimpiarTexto(text_telefono.Text);
            campos[8] = Funciones.LimpiarTexto(text_correo.Text);
            campos[10] = pdf;
            campos[11] = Funciones.LimpiarTexto(text_placa.Text).ToUpperInvariant();

            string registro = string.Join("|", campos);

            // 4. REEMPLAZAR EN EL ARCHIVO (escritura segura)
            if (!Funciones.ReemplazarExpediente(numeroExpedienteActual, registro))
            {
                // No dejar copias de PDF sin expediente
                if (pdfNuevo)
                {
                    try
                    {
                        File.Delete(pdf);
                    }
                    catch (Exception)
                    {
                    }
                }

                return;
            }

            // Acción en el historial
            Funciones.RegistrarHistorial(numeroExpedienteActual, Funciones.ACCION_MODIFICACION, Funciones.UsuarioActual,
                "Datos del expediente actualizados");

            // 5. MOSTRAR CONFIRMACIÓN
            MessageBox.Show(
                "Los datos del expediente se modificaron correctamente.",
                "Guardar cambios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // 6. CERRAR FORMULARIO
            this.Close();
        }
    }
}
