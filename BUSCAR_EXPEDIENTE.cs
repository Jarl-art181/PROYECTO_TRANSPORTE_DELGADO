
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class BUSCAR_EXPEDIENTE : Form
    {
        // ==========================================
        // VARIABLES GENERALES
        // ==========================================

        private string rutaPDFSeleccionado = "";

        private Label labelValorPlaca;
        private Label labelNombrePDF;
        private ToolTip ayudaPDF = new ToolTip();

        private bool placaEnDatosExpediente = false;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public BUSCAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Permitir escribir o seleccionar expedientes
            combo_busquedadeexpediente.DropDownStyle =
                ComboBoxStyle.DropDown;

            // Configurar la placa y el nombre del PDF
            ConfigurarCampoPlaca();
            ConfigurarNombrePDF();
        }

        // ==========================================
        // RUTA DEL ARCHIVO DE EXPEDIENTES
        // ==========================================

        private string ObtenerRutaExpedientes()
        {
            return Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );
        }

        // ==========================================
        // CONFIGURAR EL CAMPO DE PLACA
        // ==========================================

        private void ConfigurarCampoPlaca()
        {
            labelValorPlaca = new Label();

            labelValorPlaca.AutoSize = false;
            labelValorPlaca.Size = new Size(240, 24);

            labelValorPlaca.Font = new Font(
                "Arial",
                12F,
                FontStyle.Regular
            );

            labelValorPlaca.ForeColor = Color.Black;
            labelValorPlaca.BackColor = Color.Transparent;
            labelValorPlaca.TextAlign =
                ContentAlignment.MiddleLeft;

            labelValorPlaca.Text = "";

            // Buscar el título "N° de placa"
            // que agregaste en el diseñador
            Control tituloPlaca =
                BuscarTituloPlaca(guna2Panel8);

            if (tituloPlaca != null)
            {
                // Mostrar el valor junto al título
                Control contenedor = tituloPlaca.Parent;

                int posicionX;

                if (contenedor == guna2Panel12)
                {
                    // Alinear con los demás valores
                    posicionX = text_num_expe_des.Left;
                }
                else
                {
                    posicionX = tituloPlaca.Right + 15;
                }

                int posicionY = tituloPlaca.Top;

                labelValorPlaca.Location = new Point(
                    posicionX,
                    posicionY
                );

                int anchoDisponible =
                    contenedor.ClientSize.Width -
                    posicionX - 10;

                if (anchoDisponible > 50)
                {
                    labelValorPlaca.Width =
                        anchoDisponible;
                }

                contenedor.Controls.Add(labelValorPlaca);

                placaEnDatosExpediente = true;
            }
            else
            {
                // Compatibilidad con el diseño anterior
                // que todavía no tiene título de placa
                labelValorPlaca.Location =
                    new Point(43, 79);

                guna2Panel13.Controls.Add(
                    labelValorPlaca
                );

                placaEnDatosExpediente = false;
            }

            labelValorPlaca.BringToFront();
        }

        // ==========================================
        // BUSCAR TÍTULO DE PLACA
        // ==========================================


        private Control BuscarTituloPlaca(Control contenedor)
        {
            // Verificar que el contenedor exista
            if (contenedor == null)
            {
                return null;
            }

            // Recorrer todos los controles
            for (int i = 0; i < contenedor.Controls.Count; i++)
            {
                Control control = contenedor.Controls[i];

                if (control == null)
                {
                    continue;
                }

                // Revisar solamente controles de texto
                if (control is Label ||
                    control is Guna.UI2.WinForms.Guna2HtmlLabel)
                {
                    // Algunos controles pueden tener Text en null
                    string texto = control.Text;

                    if (!string.IsNullOrEmpty(texto))
                    {
                        if (texto.IndexOf(
                            "placa",
                            StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return control;
                        }
                    }
                }

                // Buscar dentro de los paneles y contenedores
                if (control.HasChildren)
                {
                    Control encontrado =
                        BuscarTituloPlaca(control);

                    if (encontrado != null)
                    {
                        return encontrado;
                    }
                }
            }

            return null;
        }


        // ==========================================
        // CONFIGURAR NOMBRE DEL PDF
        // ==========================================

        private void ConfigurarNombrePDF()
        {
            // Ocultar el Guna2HtmlLabel original
            // para evitar que el texto se expanda
            // encima del botón Ver PDF
            tex_nombrepdf_des.Visible = false;

            // Crear una etiqueta de ancho fijo
            labelNombrePDF = new Label();

            labelNombrePDF.AutoSize = false;
            labelNombrePDF.AutoEllipsis = true;

            labelNombrePDF.Location =
                new Point(81, 36);

            labelNombrePDF.Size =
                new Size(250, 25);

            labelNombrePDF.Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold
            );

            labelNombrePDF.ForeColor = Color.Black;
            labelNombrePDF.BackColor = Color.White;

            labelNombrePDF.TextAlign =
                ContentAlignment.MiddleLeft;

            labelNombrePDF.Text = "";

            guna2Panel16.Controls.Add(labelNombrePDF);

            labelNombrePDF.BringToFront();
        }

        // ==========================================
        // ABREVIAR NOMBRE DEL PDF
        // ==========================================

        private void MostrarNombrePDF(string rutaPDF)
        {
            string nombreCompleto =
                Path.GetFileName(rutaPDF);

            string nombreBase =
                Path.GetFileNameWithoutExtension(
                    nombreCompleto
                );

            string extension =
                Path.GetExtension(nombreCompleto);

            string nombreVisible = nombreCompleto;

            // Espacio disponible para el texto
            int anchoMaximo = labelNombrePDF.Width - 12;

            if (TextRenderer.MeasureText(
                nombreVisible,
                labelNombrePDF.Font).Width > anchoMaximo)
            {
                // Recortar conservando .pdf
                while (nombreBase.Length > 0)
                {
                    nombreVisible =
                        nombreBase + "..." + extension;

                    if (TextRenderer.MeasureText(
                        nombreVisible,
                        labelNombrePDF.Font).Width <= anchoMaximo)
                    {
                        break;
                    }

                    nombreBase = nombreBase.Substring(
                        0,
                        nombreBase.Length - 1
                    );
                }

                if (nombreBase.Length == 0)
                {
                    nombreVisible = "..." + extension;
                }
            }

            labelNombrePDF.Text = nombreVisible;

            // Mostrar nombre completo al pasar el mouse
            ayudaPDF.SetToolTip(
                labelNombrePDF,
                nombreCompleto
            );
        }

        // ==========================================
        // NORMALIZAR NÚMERO DE EXPEDIENTE
        // ==========================================

        private string NormalizarNumero(string numero)
        {
            numero = numero.Trim();

            // Compatible con números antiguos
            if (numero.StartsWith(
                "EXP-",
                StringComparison.OrdinalIgnoreCase))
            {
                numero = numero.Substring(4);
            }

            if (!Regex.IsMatch(
                numero,
                @"^\d{1,8}$"))
            {
                return "";
            }

            return numero.PadLeft(8, '0');
        }

        // ==========================================
        // CARGAR EXPEDIENTES AL COMBOBOX
        // ==========================================

        private void CargarExpedientes()
        {
            combo_busquedadeexpediente.Items.Clear();

            string rutaArchivo =
                ObtenerRutaExpedientes();

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
                string[] registros =
                    File.ReadAllLines(rutaArchivo);

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

                    if (datos.Length < 10)
                    {
                        continue;
                    }

                    string numero =
                        NormalizarNumero(datos[0]);

                    if (numero == "")
                    {
                        continue;
                    }

                    string expediente =
                        "EXP-" + numero;

                    if (!combo_busquedadeexpediente
                        .Items.Contains(expediente))
                    {
                        combo_busquedadeexpediente
                            .Items.Add(expediente);
                    }
                }
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

        // ==========================================
        // LIMPIAR INFORMACIÓN ANTERIOR
        // ==========================================

        private void LimpiarDetalles()
        {
            // Encabezado
            lab_Numero_expediente.Text = "";
            label7.Text = "";

            // Datos del expediente
            text_num_expe_des.Text = "";
            labe_fecha_des.Text = "";
            lab_tipodedocumento_des.Text = "";

            // Número de placa
            labelValorPlaca.Text = "";

            // Descripción
            tex_des_des.Text = "";

            // Datos del solicitante
            tex_nombreusuario_des.Text = "";
            tex_dni_des.Text = "";
            tex_empresa_des.Text = "";
            tex_telefono_des.Text = "";
            tex_correo_des.Text = "";

            // Estado
            text_estado.Text = "";
            tex_estado_des.Text = "";

            text_estado.FillColor = Color.White;
            tex_estado_des.FillColor = Color.White;

            // Documento PDF
            labelNombrePDF.Text = "";
            tex_nombrepdf_des.Text = "";

            tex_pesoPDF_des.Text = "";

            rutaPDFSeleccionado = "";

            ayudaPDF.SetToolTip(
                labelNombrePDF,
                ""
            );

            But_verpdf.Enabled = false;
            But_descargarpdf.Enabled = false;
        }

        // ==========================================
        // MOSTRAR INFORMACIÓN DEL PDF
        // ==========================================

        private void MostrarInformacionPDF(
            string[] datos)
        {
            rutaPDFSeleccionado = "";

            labelNombrePDF.Text = "Sin PDF";
            tex_pesoPDF_des.Text = "";

            But_verpdf.Enabled = false;
            But_descargarpdf.Enabled = false;

            // El PDF se encuentra en datos[10]
            if (datos.Length <= 10)
            {
                return;
            }

            string rutaPDF = datos[10].Trim();

            if (string.IsNullOrWhiteSpace(rutaPDF) ||
                rutaPDF.Equals(
                    "Sin PDF",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            rutaPDFSeleccionado = rutaPDF;

            try
            {
                MostrarNombrePDF(rutaPDF);

                // Verificar que el archivo exista
                if (!File.Exists(rutaPDF))
                {
                    tex_pesoPDF_des.Text =
                        "Archivo no encontrado";

                    return;
                }

                FileInfo archivoPDF =
                    new FileInfo(rutaPDF);

                double pesoMB =
                    archivoPDF.Length /
                    (1024.0 * 1024.0);

                tex_pesoPDF_des.Text =
                    pesoMB.ToString("0.0") + " MB";

                // Activar botones
                But_verpdf.Enabled = true;
                But_descargarpdf.Enabled = true;
            }
            catch (Exception)
            {
                labelNombrePDF.Text =
                    "PDF no disponible";

                tex_pesoPDF_des.Text = "";

                But_verpdf.Enabled = false;
                But_descargarpdf.Enabled = false;
            }
        }

        // ==========================================
        // MOSTRAR DATOS DEL EXPEDIENTE
        // ==========================================

        private void MostrarExpediente(string[] datos)
        {
            // NÚMERO
            string numero =
                NormalizarNumero(datos[0]);

            lab_Numero_expediente.Text =
                "EXP-" + numero;

            text_num_expe_des.Text =
                "EXP-" + numero;

            // FECHA
            labe_fecha_des.Text = datos[1];

            label7.Text =
                "Registrado el: " + datos[1];

            // TIPO DE DOCUMENTO
            lab_tipodedocumento_des.Text =
                datos[2];

            // DESCRIPCIÓN
            tex_des_des.Text = datos[3];

            // SOLICITANTE
            tex_nombreusuario_des.Text =
                datos[4];

            // DNI
            tex_dni_des.Text = datos[5];

            // EMPRESA
            tex_empresa_des.Text = datos[6];

            // TELÉFONO
            tex_telefono_des.Text = datos[7];

            // CORREO
            tex_correo_des.Text = datos[8];

            // ======================================
            // MOSTRAR NÚMERO DE PLACA
            // ======================================

            string placa = "Sin registrar";

            // Placa se guarda en datos[11]
            if (datos.Length > 11)
            {
                if (!string.IsNullOrWhiteSpace(
                    datos[11]))
                {
                    placa = datos[11].Trim();
                }
            }

            if (placaEnDatosExpediente)
            {
                // Ya existe el título N° de placa
                labelValorPlaca.Text = placa;
            }
            else
            {
                // Diseño anterior: mostrar en descripción
                labelValorPlaca.Text =
                    "Placa: " + placa;
            }

            // ======================================
            // MOSTRAR ESTADO
            // ======================================

            string estado = datos[9].Trim();

            text_estado.Text = estado;
            tex_estado_des.Text = estado;

            if (estado.Equals(
                "En proceso",
                StringComparison.OrdinalIgnoreCase))
            {
                text_estado.FillColor =
                    Color.FromArgb(102, 239, 203);

                tex_estado_des.FillColor =
                    Color.FromArgb(102, 239, 203);
            }
            else if (estado.Equals(
                "Pendiente",
                StringComparison.OrdinalIgnoreCase))
            {
                text_estado.FillColor = Color.Gold;
                tex_estado_des.FillColor = Color.Gold;
            }
            else if (estado.Equals(
                "Finalizado",
                StringComparison.OrdinalIgnoreCase))
            {
                text_estado.FillColor = Color.Salmon;
                tex_estado_des.FillColor = Color.Salmon;
            }
            else
            {
                text_estado.FillColor = Color.White;
                tex_estado_des.FillColor = Color.White;
            }

            // ======================================
            // MOSTRAR DOCUMENTO PDF
            // ======================================

            MostrarInformacionPDF(datos);
        }

        // ==========================================
        // BOTÓN BUSCAR
        // ==========================================

        private void But_Buscar_Click(
            object sender, EventArgs e)
        {
            LimpiarDetalles();

            string numeroTexto =
                combo_busquedadeexpediente
                    .Text.Trim()
                    .ToUpperInvariant();

            // VALIDAR CAMPO VACÍO
            if (string.IsNullOrWhiteSpace(
                numeroTexto))
            {
                MessageBox.Show(
                    "Ingrese el número de expediente.",
                    "Campo obligatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                combo_busquedadeexpediente.Focus();

                return;
            }

            // VALIDAR FORMATO
            if (!Regex.IsMatch(
                numeroTexto,
                @"^EXP-\d{8}$"))
            {
                MessageBox.Show(
                    "El número debe tener el formato " +
                    "EXP-12345678.",
                    "Número inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                combo_busquedadeexpediente.Focus();

                return;
            }

            // OBTENER SOLO LOS 8 DÍGITOS
            string numeroBuscado =
                numeroTexto.Substring(4);

            string rutaArchivo =
                ObtenerRutaExpedientes();

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

            bool encontrado = false;

            try
            {
                string[] registros =
                    File.ReadAllLines(rutaArchivo);

                // BUSCAR CON CICLO FOR
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

                    // Validar cantidad mínima de campos
                    if (datos.Length < 10)
                    {
                        continue;
                    }

                    string numeroArchivo =
                        NormalizarNumero(datos[0]);

                    if (numeroArchivo == numeroBuscado)
                    {
                        encontrado = true;

                        MostrarExpediente(datos);

                        break;
                    }
                }

                // RESULTADO
                if (encontrado)
                {
                    MessageBox.Show(
                        "Expediente encontrado correctamente.",
                        "Resultado de búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "No se encontró el expediente.",
                        "Resultado de búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al buscar expediente:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==========================================
        // BOTÓN LIMPIAR
        // ==========================================

        private void But_Limpiar_Click(
            object sender, EventArgs e)
        {
            combo_busquedadeexpediente.SelectedIndex = -1;
            combo_busquedadeexpediente.Text = "";

            LimpiarDetalles();

            // Recargar lista de expedientes
            CargarExpedientes();

            combo_busquedadeexpediente.Focus();
        }

        // ==========================================
        // CARGAR FORMULARIO
        // ==========================================

        private void BUSCAR_EXPEDIENTE_Load(
            object sender, EventArgs e)
        {
            LimpiarDetalles();
            CargarExpedientes();
        }

        // ==========================================
        // BOTÓN VER PDF
        // ==========================================

        private void But_verpdf_Click(
            object sender, EventArgs e)
        {
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

            if (!File.Exists(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = rutaPDFSeleccionado,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir el PDF:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==========================================
        // BOTÓN DESCARGAR PDF
        // ==========================================

        private void But_descargarpdf_Click(
            object sender, EventArgs e)
        {
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

            if (!File.Exists(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            using (SaveFileDialog guardar =
                new SaveFileDialog())
            {
                guardar.Filter =
                    "Archivo PDF (*.pdf)|*.pdf";

                guardar.FileName =
                    Path.GetFileName(
                        rutaPDFSeleccionado
                    );

                guardar.OverwritePrompt = true;

                if (guardar.ShowDialog() ==
                    DialogResult.OK)
                {
                    try
                    {
                        string origen =
                            Path.GetFullPath(
                                rutaPDFSeleccionado
                            );

                        string destino =
                            Path.GetFullPath(
                                guardar.FileName
                            );

                        // Impedir copiar sobre el mismo PDF
                        if (string.Equals(
                            origen,
                            destino,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(
                                "Seleccione otra ubicación " +
                                "para guardar el PDF.",
                                "Ubicación inválida",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );

                            return;
                        }

                        File.Copy(
                            origen,
                            destino,
                            true
                        );

                        MessageBox.Show(
                            "PDF guardado correctamente.",
                            "Documento PDF",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "No se pudo guardar el PDF:\n" +
                            ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        // ==========================================
        // BOTONES DE NAVEGACIÓN
        // ==========================================

        private void guna2Button1_Click(
            object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel5_Click(
            object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
        }

        private void guna2Button6_Click(
            object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
        }

        private void guna2Button3_Click(
            object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();
        }

        private void guna2Button2_Click(
            object sender, EventArgs e)
        {
            INICIO ventana = new INICIO();

            ventana.Show();
        }

        private void guna2Button4_Click(
            object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();
        }

        private void guna2Button7_Click(
            object sender, EventArgs e)
        {
            REPORTES ventana = new REPORTES();

            ventana.Show();
        }

        private void guna2Button8_Click(
            object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();
        }

      

        // EVENTO DEL CAMPO ESTADO
        private void tex_estado_des_TextChanged(
            object sender, EventArgs e)
        {

        }

        // EVENTO DEL LABEL ESTADO
        private void guna2HtmlLabel17_Click(
            object sender, EventArgs e)
        {

        }

    } // Cierra la clase BUSCAR_EXPEDIENTE
}     // Cierra el namespace
    
