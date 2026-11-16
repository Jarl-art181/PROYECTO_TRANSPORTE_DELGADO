
using System;
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

        // Seguimiento: fecha de inicio, finalización, resultado y observación
        private Label labelFechasSeguimiento;
        private Label labelResultado;
        private Label labelObservacion;

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

            // Configurar los datos de seguimiento
            ConfigurarSeguimiento();

            // Número de expediente: EXP- + 8 dígitos (al escribir y al pegar)
            Restricciones.Codigo(combo_busquedadeexpediente);

            // Ajustar el diseño al tamaño y escalado de la pantalla
            // (al final, para incluir las etiquetas creadas por código)
            Escalado.AjustarPantallaCompleta(this);
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
        // CARGAR EXPEDIENTES AL COMBOBOX
        // ==========================================

        private void CargarExpedientes()
        {
            combo_busquedadeexpediente.Items.Clear();

            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                Funciones.Aviso(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado"
                );

                return;
            }

            try
            {
                // Solo expedientes válidos (sin líneas vacías)
                string[] registros =
                    Funciones.LeerExpedientesValidos();

                for (int i = 0;
                     i < registros.Length;
                     i++)
                {
                    string[] datos =
                        registros[i].Split('|');

                    string numero =
                        Funciones.NormalizarNumero(datos[0]);

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

        // ==========================================
        // CONFIGURAR DATOS DE SEGUIMIENTO
        // - Fechas: en la fila superior de "Información
        //   del Expediente" (espacio libre del diseño).
        // - Resultado y observación: en el recuadro de
        //   Descripción, debajo del texto de descripción.
        // ==========================================

        private Label CrearEtiquetaSeguimiento(Control padre, int x, int y, int ancho, int alto, bool negrita)
        {
            Label etiqueta = new Label();

            etiqueta.AutoSize = false;
            etiqueta.AutoEllipsis = true;
            etiqueta.Bounds = new Rectangle(x, y, ancho, alto);
            etiqueta.BackColor = Color.Transparent;
            etiqueta.ForeColor = Color.Black;
            etiqueta.Font = new Font("Arial", 10F, negrita ? FontStyle.Bold : FontStyle.Regular);
            etiqueta.TextAlign = ContentAlignment.MiddleLeft;
            etiqueta.Text = "";

            padre.Controls.Add(etiqueta);
            etiqueta.BringToFront();

            return etiqueta;
        }

        private void ConfigurarSeguimiento()
        {
            // Entre el estado (text_estado) y "Registrado el" (label7)
            int inicioX = text_estado.Right + 20;
            int ancho = label7.Left - inicioX - 10;

            labelFechasSeguimiento = CrearEtiquetaSeguimiento(
                guna2Panel8, inicioX, text_estado.Top, ancho, text_estado.Height, false);

            // Debajo de la descripción
            int x = tex_des_des.Left;
            int anchoTexto = guna2Panel13.Width - x - 15;

            labelResultado = CrearEtiquetaSeguimiento(
                guna2Panel13, x, tex_des_des.Bottom + 4, anchoTexto, 22, true);

            // Usa el alto que queda libre dentro del recuadro
            int arribaObservacion = tex_des_des.Bottom + 26;
            int altoObservacion = Math.Max(20, guna2Panel13.Height - arribaObservacion - 4);

            labelObservacion = CrearEtiquetaSeguimiento(
                guna2Panel13, x, arribaObservacion, anchoTexto, altoObservacion, false);
        }

        private void MostrarSeguimiento(string[] datos)
        {
            labelFechasSeguimiento.Text =
                "Inicio de atención: " +
                Funciones.CampoParaMostrar(datos, Funciones.CAMPO_INICIO) +
                "     Finalización: " +
                Funciones.CampoParaMostrar(datos, Funciones.CAMPO_FIN);

            labelResultado.Text =
                "Resultado: " +
                Funciones.CampoParaMostrar(datos, Funciones.CAMPO_RESULTADO);

            labelObservacion.Text =
                "Observación final: " +
                Funciones.CampoParaMostrar(datos, Funciones.CAMPO_OBSERVACION);

            // Texto completo al pasar el mouse
            ayudaPDF.SetToolTip(labelObservacion, labelObservacion.Text);
        }


        private void LimpiarDetalles()
        {
            // Seguimiento
            labelFechasSeguimiento.Text = "";
            labelResultado.Text = "";
            labelObservacion.Text = "";
            ayudaPDF.SetToolTip(labelObservacion, "");

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

            if (!Funciones.TienePDF(rutaPDF))
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
                Funciones.NormalizarNumero(datos[0]);

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

            // ======================================
            // MOSTRAR SEGUIMIENTO
            // ======================================

            MostrarSeguimiento(datos);
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
                @"^EXP-[0-9]{8}$"))
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

            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                Funciones.Aviso(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado"
                );

                return;
            }

            bool encontrado = false;

            try
            {
                string[] registros =
                    Funciones.LeerExpedientesValidos();

                // BUSCAR CON CICLO FOR
                for (int i = 0;
                     i < registros.Length;
                     i++)
                {
                    string[] datos =
                        registros[i].Split('|');

                    string numeroArchivo =
                        Funciones.NormalizarNumero(datos[0]);

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
            // Abrir con control de errores
            Funciones.AbrirPDF(rutaPDFSeleccionado);
        }

        // ==========================================
        // BOTÓN DESCARGAR PDF
        // ==========================================

        private void But_descargarpdf_Click(
            object sender, EventArgs e)
        {
            // Guardar una copia con control de errores
            Funciones.DescargarPDF(rutaPDFSeleccionado);
        }

        // ==========================================
        // BOTONES DE NAVEGACIÓN
        // Abre el formulario elegido y cierra el actual
        // para que no se acumulen ventanas abiertas.
        // ==========================================

        private void AbrirFormulario(Form ventana)
        {
            ventana.Show();
            this.Close();
        }

        private void guna2Button1_Click(
            object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        private void guna2HtmlLabel5_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new GESTIONAR_EXPEDIENTE());
        }

        private void guna2Button6_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new GESTIONAR_EXPEDIENTE());
        }

        private void guna2Button3_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new REGISTRO_DE_EXPEDIENTE());
        }

        private void guna2Button2_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new INICIO());
        }

        private void guna2Button4_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new CONSULTAR_EXPEDIENTE());
        }

        private void guna2Button7_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new REPORTES());
        }

        private void guna2Button8_Click(
            object sender, EventArgs e)
        {
            AbrirFormulario(new CONFIGURACIÓN());
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
    
