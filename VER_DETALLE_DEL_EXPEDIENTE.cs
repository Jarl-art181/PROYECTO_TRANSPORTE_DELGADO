using System;
using System.Windows.Forms;
using System.IO;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class VER_DETALLE_DEL_EXPEDIENTE : Form
    {
        // RUTA DEL PDF

        private string rutaPDFSeleccionado = "";

        // Valores de seguimiento
        private Label valorInicio;
        private Label valorFin;
        private Label valorResultado;
        private Label valorObservacion;


        // ==========================================
        // SECCIÓN "SEGUIMIENTO DEL EXPEDIENTE"
        // Se agrega al pie de la ventana, con el mismo estilo
        // de las secciones existentes (barra dorada y recuadro).
        // ==========================================

        private Label NuevaEtiqueta(Control padre, string texto, int x, int y, int ancho, int alto, bool negrita)
        {
            Label etiqueta = new Label();

            etiqueta.AutoSize = false;
            etiqueta.AutoEllipsis = true;
            etiqueta.Bounds = new System.Drawing.Rectangle(x, y, ancho, alto);
            etiqueta.BackColor = System.Drawing.Color.Transparent;
            etiqueta.ForeColor = System.Drawing.Color.Black;
            etiqueta.Font = new System.Drawing.Font(
                "Arial", negrita ? 9.75F : 10F,
                negrita ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
            etiqueta.Text = texto;

            padre.Controls.Add(etiqueta);
            etiqueta.BringToFront();

            return etiqueta;
        }

        private void CrearSeccionSeguimiento()
        {
            const int ALTO_SECCION = 160;

            // Agrandar la ventana y el panel de fondo
            int inicioY = but_cerrar.Bottom + 12;
            this.ClientSize = new System.Drawing.Size(
                this.ClientSize.Width,
                this.ClientSize.Height + ALTO_SECCION);
            guna2Panel1.Height += ALTO_SECCION;

            int x = guna2Panel2.Left;
            int ancho = but_cerrar.Right - x;

            // Barra dorada con el título
            Guna.UI2.WinForms.Guna2Panel barra = new Guna.UI2.WinForms.Guna2Panel();
            barra.Bounds = new System.Drawing.Rectangle(x, inicioY, ancho, 36);
            barra.BackColor = System.Drawing.Color.Transparent;
            barra.FillColor = System.Drawing.Color.Gold;
            barra.BorderRadius = 8;
            guna2Panel1.Controls.Add(barra);

            Guna.UI2.WinForms.Guna2HtmlLabel titulo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            titulo.BackColor = System.Drawing.Color.Transparent;
            titulo.Font = new System.Drawing.Font("Cooper Black", 14.25F);
            titulo.ForeColor = System.Drawing.Color.Black;
            titulo.Text = "SEGUIMIENTO DEL EXPEDIENTE";
            titulo.Location = new System.Drawing.Point(16, 7);
            barra.Controls.Add(titulo);

            // Recuadro blanco con los datos
            Guna.UI2.WinForms.Guna2Panel recuadro = new Guna.UI2.WinForms.Guna2Panel();
            recuadro.Bounds = new System.Drawing.Rectangle(x, inicioY + 44, ancho, ALTO_SECCION - 56);
            recuadro.BackColor = System.Drawing.Color.Transparent;
            recuadro.FillColor = System.Drawing.Color.White;
            recuadro.BorderColor = System.Drawing.Color.DarkGray;
            recuadro.BorderThickness = 1;
            recuadro.BorderRadius = 15;
            guna2Panel1.Controls.Add(recuadro);

            // Fila 1: inicio, finalización y resultado
            NuevaEtiqueta(recuadro, "Inicio de atención:", 20, 14, 150, 22, true);
            valorInicio = NuevaEtiqueta(recuadro, "", 170, 14, 200, 22, false);

            NuevaEtiqueta(recuadro, "Finalización:", 390, 14, 110, 22, true);
            valorFin = NuevaEtiqueta(recuadro, "", 500, 14, 200, 22, false);

            NuevaEtiqueta(recuadro, "Resultado:", 730, 14, 95, 22, true);
            valorResultado = NuevaEtiqueta(recuadro, "", 825, 14, ancho - 845, 22, false);

            // Fila 2: observación final (hasta dos líneas)
            NuevaEtiqueta(recuadro, "Observación final:", 20, 46, 150, 22, true);
            valorObservacion = NuevaEtiqueta(recuadro, "", 170, 46, ancho - 190, 44, false);
        }


        // CONSTRUCTOR SOLO VER DETALLE

        public VER_DETALLE_DEL_EXPEDIENTE(
            string numeroExpediente)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Sección de seguimiento al pie de la ventana
            CrearSeccionSeguimiento();

            // Reducir la ventana si no cabe en la pantalla
            Escalado.AjustarDialogo(this);

            CargarExpediente(numeroExpediente);
        }


        // CARGAR EXPEDIENTE

        private void CargarExpediente(
            string numeroExpediente)
        {
            // VERIFICAR ARCHIVO

            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                Funciones.Aviso(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado"
                );

                return;
            }


            // BUSCAR EXPEDIENTE (número normalizado)

            string[] datos =
                Funciones.BuscarExpediente(numeroExpediente);

            if (datos == null)
            {
                Funciones.Aviso(
                    "No se encontró el expediente seleccionado.",
                    "Expediente no encontrado"
                );

                return;
            }


            // DATOS DEL EXPEDIENTE

            text_numer_expe.Text =
                "EXP-" + Funciones.NormalizarNumero(datos[0]);

            text_fecha.Text = datos[1];

            text_tipoDocumento.Text = datos[2];

            if (datos.Length > 11 &&
                !string.IsNullOrWhiteSpace(datos[11]))
            {
                text_placa.Text = datos[11].Trim().ToUpper();
            }
            else
            {
                text_placa.Text = "Sin registrar";
            }

            text_descripcion.Text = datos[3];

            text_solicitante.Text = datos[4];

            text_dni.Text = datos[5];

            tex_empresa.Text = datos[6];

            text_telefono.Text = datos[7];

            text_correo.Text = datos[8];

            text_estado.Text = datos[9];


            // PDF (puede faltar en registros antiguos)

            string rutaPDF = "";

            if (datos.Length > 10)
            {
                rutaPDF = datos[10].Trim();
            }

            MostrarPDF(rutaPDF);


            // SEGUIMIENTO ("Sin registrar" si falta el dato)

            valorInicio.Text = Funciones.CampoParaMostrar(datos, Funciones.CAMPO_INICIO);
            valorFin.Text = Funciones.CampoParaMostrar(datos, Funciones.CAMPO_FIN);
            valorResultado.Text = Funciones.CampoParaMostrar(datos, Funciones.CAMPO_RESULTADO);
            valorObservacion.Text = Funciones.CampoParaMostrar(datos, Funciones.CAMPO_OBSERVACION);
        }


        // ==========================================
        // MOSTRAR PDF
        // ==========================================

        private void MostrarPDF(
            string rutaPDF)
        {
            rutaPDFSeleccionado = "";

            // NO TIENE PDF

            if (!Funciones.TienePDF(rutaPDF))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }


            // VERIFICAR SI EXISTE EL PDF

            if (!File.Exists(rutaPDF))
            {
                Funciones.Aviso(
                    "No se encontró el archivo PDF.",
                    "Documento PDF"
                );

                return;
            }


            // GUARDAR RUTA Y MOSTRAR EN WEBVIEW2

            rutaPDFSeleccionado = rutaPDF;

            try
            {
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
        // LOAD
        // ==========================================

        private void DETALLE_DEL_EXPEDIENTE_Load(
            object sender,
            EventArgs e)
        {
            // Los datos se cargan en el constructor
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
        // DESCARGAR PDF (con control de errores)
        // ==========================================

        private void but_descargar_Click(
            object sender,
            EventArgs e)
        {
            Funciones.DescargarPDF(rutaPDFSeleccionado);
        }


        // ==========================================
        // ABRIR PDF (con control de errores)
        // ==========================================

        private void But_abrirpdf_Click(
            object sender,
            EventArgs e)
        {
            Funciones.AbrirPDF(rutaPDFSeleccionado);
        }
    }
}
