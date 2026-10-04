using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class REGISTRO_DE_EXPEDIENTE : Form
    {
        private string rutaPDF = "";

        public REGISTRO_DE_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
        }


        // =========================================================
        // CARGAR FORMULARIO
        // =========================================================

        private void REGISTRO_DE_EXPEDIENTE_Load(object sender, EventArgs e)
        {
            // FECHA ACTUAL
            textFecha.Text =
                DateTime.Now.ToString("dd/MM/yyyy");


            // TIPOS DE DOCUMENTO
            ComboTipoDocumento.Items.Clear();

            ComboTipoDocumento.Items.Add("Solicitud");
            ComboTipoDocumento.Items.Add("Oficio");
            ComboTipoDocumento.Items.Add("Carta");
            ComboTipoDocumento.Items.Add("Informe");
            ComboTipoDocumento.Items.Add("Reclamo");

            ComboTipoDocumento.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // INICIALMENTE NO HAY PDF
            txtRutaPDF.Text = "";

            picPDF.Visible = false;
            lblNombrePDF.Visible = false;
            lblTipoPDF.Visible = false;

            lblTipoPDF.Text = "";
            lblNombrePDF.Text = "";


            // ESTADOS DEL EXPEDIENTE
            comboESTADO.Items.Clear();

            comboESTADO.Items.Add("En proceso");
            comboESTADO.Items.Add("Pendiente");
            comboESTADO.Items.Add("Finalizado");

            comboESTADO.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // GENERAR NÚMERO SEGÚN CONFIGURACIÓN
            GenerarNumeroExpediente();
        }


        // =========================================================
        // ADJUNTAR PDF
        // =========================================================

        private void btnAdjuntarPDF_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirPDF =
                new OpenFileDialog();

            abrirPDF.Filter =
                "Archivos PDF (*.pdf)|*.pdf";

            abrirPDF.Title =
                "Seleccionar documento PDF";


            if (abrirPDF.ShowDialog() == DialogResult.OK)
            {
                FileInfo archivo =
                    new FileInfo(abrirPDF.FileName);


                // Comprobar que no supere 10 MB
                if (archivo.Length >
                    10 * 1024 * 1024)
                {
                    MessageBox.Show(
                        "El archivo PDF no puede superar los 10 MB.",
                        "Archivo demasiado grande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }


                // Guardar la ruta completa
                rutaPDF =
                    abrirPDF.FileName;


                // Obtener solamente el nombre del PDF
                string nombrePDF =
                    Path.GetFileName(
                        abrirPDF.FileName
                    );


                lblTipoPDF.Text =
                    "Documento PDF";


                // Mostrar el nombre
                lblNombrePDF.Text =
                    nombrePDF;


                // Mostrar logo y nombre
                picPDF.Visible = true;
                lblNombrePDF.Visible = true;
                lblTipoPDF.Visible = true;
            }
        }


        // =========================================================
        // VER PDF
        // =========================================================

        private void btnVerPDF_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rutaPDF))
            {
                MessageBox.Show(
                    "Primero debe adjuntar un documento PDF.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            if (!File.Exists(rutaPDF))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                rutaPDF = "";

                return;
            }


            try
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = rutaPDF,
                        UseShellExecute = true
                    }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir el PDF.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // =========================================================
        // LIMPIAR NOMBRE
        // =========================================================

        private void texNAME_USUARIO_Enter(
            object sender,
            EventArgs e)
        {
            if (texNAME_USUARIO.Text ==
                "Juan Garcia Perez")
            {
                texNAME_USUARIO.Clear();
            }
        }


        // =========================================================
        // LIMPIAR NÚMERO
        // =========================================================

        private void txtN_expediente_Enter(
            object sender,
            EventArgs e)
        {
            if (txtN_expediente.Text ==
                "12345678")
            {
                txtN_expediente.Clear();
            }
        }


        // =========================================================
        // LIMPIAR FECHA
        // =========================================================

        private void textFecha_Enter(
            object sender,
            EventArgs e)
        {
            if (textFecha.Text ==
                "26/09/2026")
            {
                textFecha.Clear();
            }
        }


        // =========================================================
        // LIMPIAR DNI
        // =========================================================

        private void textDNI_Enter(
            object sender,
            EventArgs e)
        {
            if (textDNI.Text ==
                "78459554")
            {
                textDNI.Clear();
            }
        }


        // =========================================================
        // LIMPIAR EMPRESA
        // =========================================================

        private void textEMPRESA_Enter(
            object sender,
            EventArgs e)
        {
            if (textEMPRESA.Text ==
                "Transporte Rojas SAC")
            {
                textEMPRESA.Clear();
            }
        }


        // =========================================================
        // LIMPIAR TELÉFONO
        // =========================================================

        private void textTELEFONO_Enter(
            object sender,
            EventArgs e)
        {
            if (textTELEFONO.Text ==
                "994418578")
            {
                textTELEFONO.Clear();
            }
        }


        // =========================================================
        // LIMPIAR CORREO
        // =========================================================

        private void textCORREO_Enter(
            object sender,
            EventArgs e)
        {
            if (textCORREO.Text ==
                "juanperez@rojassac.com")
            {
                textCORREO.Clear();
            }
        }


        // =========================================================
        // MÉTODO PARA LIMPIAR FORMULARIO
        // =========================================================

        private void LimpiarFormulario()
        {
            txtN_expediente.Clear();

            txtDescripcion.Clear();

            texNAME_USUARIO.Clear();

            textDNI.Clear();

            textEMPRESA.Clear();

            textTELEFONO.Clear();

            textCORREO.Clear();


            ComboTipoDocumento.SelectedIndex = -1;

            comboESTADO.SelectedIndex = -1;


            textFecha.Clear();


            rutaPDF = "";


            txtRutaPDF.Text = "";

            lblNombrePDF.Text = "";

            lblTipoPDF.Text = "";


            picPDF.Visible = false;

            lblNombrePDF.Visible = false;

            lblTipoPDF.Visible = false;


            // Permitir escritura nuevamente
            txtN_expediente.ReadOnly = false;
        }


        // =========================================================
        // LEER OPCIÓN:
        // LIMPIAR FORMULARIO DESPUÉS DE GUARDAR
        // =========================================================

        private bool LimpiarDespuesDeGuardar()
        {
            string rutaOpciones =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "opciones.txt"
                );


            if (!File.Exists(rutaOpciones))
                return false;


            string contenido =
                File.ReadAllText(rutaOpciones);


            string[] opciones =
                contenido.Split('|');


            if (opciones.Length >= 1)
            {
                bool.TryParse(
                    opciones[0],
                    out bool limpiar
                );

                return limpiar;
            }


            return false;
        }


        // =========================================================
        // LEER OPCIÓN:
        // MOSTRAR MENSAJE DESPUÉS DE GUARDAR
        // =========================================================

        private bool MostrarMensajeDespuesDeGuardar()
        {
            string rutaOpciones =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "opciones.txt"
                );


            if (!File.Exists(rutaOpciones))
                return true;


            string contenido =
                File.ReadAllText(rutaOpciones);


            string[] opciones =
                contenido.Split('|');


            if (opciones.Length >= 2)
            {
                bool.TryParse(
                    opciones[1],
                    out bool mostrarMensaje
                );

                return mostrarMensaje;
            }


            return true;
        }


        // =========================================================
        // LEER OPCIÓN:
        // GENERAR NÚMEROS AUTOMÁTICAMENTE
        // =========================================================

        private bool GenerarNumerosAutomaticamente()
        {
            string rutaOpciones =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "opciones.txt"
                );


            if (!File.Exists(rutaOpciones))
                return false;


            string contenido =
                File.ReadAllText(rutaOpciones);


            string[] opciones =
                contenido.Split('|');


            if (opciones.Length >= 4)
            {
                bool.TryParse(
                    opciones[3],
                    out bool generarNumero
                );

                return generarNumero;
            }


            return false;
        }


        // =========================================================
        // OBTENER SIGUIENTE NÚMERO DE EXPEDIENTE
        // =========================================================

        private string ObtenerSiguienteNumeroExpediente()
        {
            string ruta =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "expedientes.txt"
                );


            int mayor = 0;


            if (File.Exists(ruta))
            {
                string[] registros =
                    File.ReadAllLines(ruta);


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


                    if (datos.Length > 0)
                    {
                        if (int.TryParse(
                            datos[0].Trim(),
                            out int numero))
                        {
                            if (numero > mayor)
                            {
                                mayor = numero;
                            }
                        }
                    }
                }
            }


            int siguiente =
                mayor + 1;


            return siguiente.ToString("D8");
        }


        // =========================================================
        // GENERAR NÚMERO SEGÚN CONFIGURACIÓN
        // =========================================================

        private void GenerarNumeroExpediente()
        {
            if (GenerarNumerosAutomaticamente())
            {
                txtN_expediente.Text =
                    ObtenerSiguienteNumeroExpediente();

                txtN_expediente.ReadOnly = true;
            }
            else
            {
                txtN_expediente.Clear();

                txtN_expediente.ReadOnly = false;
            }
        }


        // =========================================================
        // PREPARAR NUEVO EXPEDIENTE
        // =========================================================

        private void PrepararNuevoExpediente()
        {
            // Limpiar todos los campos
            LimpiarFormulario();


            // Colocar fecha actual
            textFecha.Text =
                DateTime.Now.ToString("dd/MM/yyyy");


            // Generar número si está activado
            GenerarNumeroExpediente();


            // Colocar cursor
            txtN_expediente.Focus();
        }


        // =========================================================
        // BOTÓN LIMPIEZA
        // =========================================================

        private void But_Limpieza_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Está seguro de limpiar los datos?",
                    "Confirmar limpieza",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (respuesta == DialogResult.Yes)
            {
                LimpiarFormulario();

                textFecha.Text =
                    DateTime.Now.ToString("dd/MM/yyyy");

                GenerarNumeroExpediente();

                txtN_expediente.Focus();
            }
        }


        // =========================================================
        // CANCELAR REGISTRO
        // =========================================================

        private void guna2Button14_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Está seguro de cancelar el registro?",
                    "Cancelar registro",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (respuesta == DialogResult.Yes)
            {
                INICIO ventana =
                    new INICIO();

                ventana.Show();

                this.Close();
            }
        }


        // =========================================================
        // NUEVO EXPEDIENTE
        // =========================================================

        private void But_nuevo_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Desea ingresar otro expediente?",
                    "Nuevo expediente",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (respuesta == DialogResult.Yes)
            {
                // PREPARAR NUEVO EXPEDIENTE
                PrepararNuevoExpediente();


                MessageBox.Show(
                    "La pantalla está lista para ingresar un nuevo expediente.",
                    "Nuevo expediente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // =========================================================
        // GUARDAR EXPEDIENTE
        // =========================================================

        private void but_guardar_Click(
            object sender,
            EventArgs e)
        {
            // =====================================================
            // 1. VERIFICAR CAMPOS VACÍOS
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                txtN_expediente.Text))
            {
                MessageBox.Show(
                    "Ingrese el número de expediente.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtN_expediente.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                textFecha.Text))
            {
                MessageBox.Show(
                    "Ingrese la fecha.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textFecha.Focus();

                return;
            }


            if (ComboTipoDocumento.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Seleccione el tipo de documento.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                ComboTipoDocumento.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                txtDescripcion.Text))
            {
                MessageBox.Show(
                    "Ingrese la descripción.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtDescripcion.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                texNAME_USUARIO.Text))
            {
                MessageBox.Show(
                    "Ingrese el nombre del usuario.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                texNAME_USUARIO.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                textDNI.Text))
            {
                MessageBox.Show(
                    "Ingrese el documento de identificación.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textDNI.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                textEMPRESA.Text))
            {
                MessageBox.Show(
                    "Ingrese la empresa.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textEMPRESA.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                textTELEFONO.Text))
            {
                MessageBox.Show(
                    "Ingrese el teléfono.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textTELEFONO.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                textCORREO.Text))
            {
                MessageBox.Show(
                    "Ingrese el correo electrónico.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textCORREO.Focus();

                return;
            }


            if (comboESTADO.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Seleccione el estado.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                comboESTADO.Focus();

                return;
            }


            // =====================================================
            // 2. VALIDAR NÚMERO DE EXPEDIENTE
            // =====================================================

            int numeroExpediente;


            if (!int.TryParse(
                txtN_expediente.Text.Trim(),
                out numeroExpediente))
            {
                MessageBox.Show(
                    "El número de expediente debe contener solo números.",
                    "Dato inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtN_expediente.Focus();

                return;
            }


            // VALIDAR EXACTAMENTE 8 NÚMEROS

            if (txtN_expediente.Text.Trim().Length != 8)
            {
                MessageBox.Show(
                    "El número de expediente debe tener exactamente 8 números.",
                    "Número inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtN_expediente.Focus();

                return;
            }


            // =====================================================
            // 3. VALIDAR FECHA
            // =====================================================

            DateTime fecha;


            if (!DateTime.TryParseExact(
                textFecha.Text,
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out fecha))
            {
                MessageBox.Show(
                    "La fecha debe tener el formato dd/MM/yyyy.",
                    "Fecha incorrecta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textFecha.Focus();

                return;
            }


            // =====================================================
            // 4. VALIDAR NOMBRE
            // =====================================================

            if (!texNAME_USUARIO.Text.All(
                c => char.IsLetter(c) ||
                     char.IsWhiteSpace(c)))
            {
                MessageBox.Show(
                    "El nombre solo debe contener letras y espacios.",
                    "Dato inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                texNAME_USUARIO.Focus();

                return;
            }


            // =====================================================
            // 5. VALIDAR DNI
            // =====================================================

            if (!int.TryParse(
                textDNI.Text.Trim(),
                out int dni) ||
                textDNI.Text.Trim().Length != 8)
            {
                MessageBox.Show(
                    "El DNI debe contener exactamente 8 dígitos.",
                    "DNI inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textDNI.Focus();

                return;
            }


            // =====================================================
            // 6. VALIDAR TELÉFONO
            // =====================================================

            if (!int.TryParse(
                textTELEFONO.Text.Trim(),
                out int telefono) ||
                textTELEFONO.Text.Trim().Length != 9)
            {
                MessageBox.Show(
                    "El teléfono debe contener exactamente 9 dígitos.",
                    "Teléfono inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textTELEFONO.Focus();

                return;
            }


            // =====================================================
            // 7. VALIDAR CORREO
            // =====================================================

            if (!textCORREO.Text.Trim()
                .EndsWith("@gmail.com"))
            {
                MessageBox.Show(
                    "El correo debe terminar en @gmail.com.",
                    "Correo inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textCORREO.Focus();

                return;
            }


            // =====================================================
            // 8. VALIDAR PDF
            // =====================================================

            if (string.IsNullOrWhiteSpace(rutaPDF))
            {
                MessageBox.Show(
                    "Debe adjuntar un documento PDF.",
                    "Archivo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // =====================================================
            // 9. CREAR CARPETA DE DATOS
            // =====================================================

            string carpetaDatos =
                Path.Combine(
                    Application.StartupPath,
                    "Datos"
                );


            Directory.CreateDirectory(
                carpetaDatos
            );


            // =====================================================
            // 10. RUTA DEL ARCHIVO
            // =====================================================

            string archivoExpedientes =
                Path.Combine(
                    carpetaDatos,
                    "expedientes.txt"
                );


            // =====================================================
            // 11. VERIFICAR NÚMERO REPETIDO
            //     UTILIZANDO MATRIZ + FOR
            // =====================================================

            string numero_Expediente =
                txtN_expediente.Text.Trim();


            if (File.Exists(archivoExpedientes))
            {
                // Leer todos los registros
                string[] registros =
                    File.ReadAllLines(
                        archivoExpedientes
                    );


                // Crear matriz
                string[,] matriz =
                    new string[registros.Length, 9];


                // GUARDAR DATOS EN LA MATRIZ
                for (int i = 0;
                     i < registros.Length;
                     i++)
                {
                    // Separar los datos
                    string[] datos =
                        registros[i].Split('|');


                    // Guardar datos dentro
                    // de la matriz
                    for (int j = 0;
                         j < datos.Length &&
                         j < 9;
                         j++)
                    {
                        matriz[i, j] =
                            datos[j];
                    }
                }


                // BUSCAR NÚMERO REPETIDO
                for (int i = 0;
                     i < matriz.GetLength(0);
                     i++)
                {
                    // La columna 0 contiene
                    // el número de expediente
                    if (matriz[i, 0] ==
                        numero_Expediente)
                    {
                        MessageBox.Show(
                            "El número de expediente ya existe.\n" +
                            "Ingrese un número diferente.",
                            "Expediente duplicado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        txtN_expediente.Focus();

                        return;
                    }
                }
            }


            // =====================================================
            // 12. OBTENER LOS DATOS
            // =====================================================

            string fechaRegistro =
                textFecha.Text.Trim();


            string tipoDocumento =
                ComboTipoDocumento.Text.Trim();


            string descripcion =
                txtDescripcion.Text.Trim();


            string nombreUsuario =
                texNAME_USUARIO.Text.Trim();


            string identificacion =
                textDNI.Text.Trim();


            string empresa =
                textEMPRESA.Text.Trim();


            string telefono_text =
                textTELEFONO.Text.Trim();


            string correo =
                textCORREO.Text.Trim();


            string estado =
                comboESTADO.Text.Trim();


            string pdf =
                rutaPDF;


            // =====================================================
            // 13. CREAR EL REGISTRO
            // =====================================================

            string registro =
                numeroExpediente + "|" +
                fechaRegistro + "|" +
                tipoDocumento + "|" +
                descripcion + "|" +
                nombreUsuario + "|" +
                identificacion + "|" +
                empresa + "|" +
                telefono_text + "|" +
                correo + "|" +
                estado + "|" +
                pdf;


            // =====================================================
            // 14. GUARDAR EN EL ARCHIVO
            // =====================================================

            File.AppendAllText(
                archivoExpedientes,
                registro +
                Environment.NewLine
            );


            // =====================================================
            // 15. LIMPIAR SEGÚN CONFIGURACIÓN
            // =====================================================

            if (LimpiarDespuesDeGuardar())
            {
                PrepararNuevoExpediente();
            }


            // =====================================================
            // 16. MENSAJE DE CONFIRMACIÓN
            // =====================================================

            if (MostrarMensajeDespuesDeGuardar())
            {
                MessageBox.Show(
                    "El expediente se registró correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // =========================================================
        // CERRAR
        // =========================================================

        private void But_Cerrar_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }


        // =========================================================
        // INICIO
        // =========================================================

        private void ButtonINICIO_Click(
            object sender,
            EventArgs e)
        {
            INICIO ventana =
                new INICIO();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // CONSULTAR
        // =========================================================

        private void But_Consultar_Click(
            object sender,
            EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // BUSCAR
        // =========================================================

        private void But_buscar_Click(
            object sender,
            EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // GESTIONAR
        // =========================================================

        private void But_gestionar_Click(
            object sender,
            EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // REPORTES
        // =========================================================

        private void But_Reportes_Click(
            object sender,
            EventArgs e)
        {
            REPORTES ventana =
                new REPORTES();

            ventana.Show();

            this.Close();
        }


        // =========================================================
        // CONFIGURACIÓN
        // =========================================================

        private void But_Configuración_Click(
            object sender,
            EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();

            this.Close();
        }
    }
}