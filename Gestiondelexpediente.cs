using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    // =========================================================
    // FICHA "GESTIÓN DEL EXPEDIENTE"
    // ---------------------------------------------------------
    // Se abre desde Gestionar con el CÓDIGO del expediente.
    // Todos los controles están en el diseñador de Visual Studio.
    //
    // Botones de la ficha:
    //   But_restaurar  -> Ver detalle
    //   guna2Button1   -> Modificar
    //   guna2Button2   -> Eliminar
    //   guna2Button3   -> Preparar correo (solicitante o área)
    //   guna2Button4   -> Ver correos preparados
    //   guna2Button5   -> Iniciar atención
    //   guna2Button6   -> Finalizar expediente
    //
    // Paneles (ocultos hasta que se usan):
    //   panelCorreo (pestañas tabSolicitante y tabArea),
    //   panelVerCorreo, panelIniciar, panelFinalizar
    // =========================================================

    public partial class Gestiondelexpediente : Form
    {
        // Código del expediente (8 dígitos)
        private string numeroExpediente = "";

        // Datos actuales del expediente (18 campos)
        private string[] campos = null;

        // Indica a Gestionar que el expediente se eliminó
        public bool ExpedienteEliminado = false;

        // Textos completos al pasar el mouse
        private ToolTip ayuda = new ToolTip();


        // =========================================================
        // CONSTRUCTORES
        // =========================================================

        // Usado por el diseñador de Visual Studio
        public Gestiondelexpediente()
        {
            InitializeComponent();
        }

        // Usado por Gestionar: recibe el código del expediente
        public Gestiondelexpediente(string numero) : this()
        {
            numeroExpediente = Funciones.NormalizarNumero(numero);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            ConectarEventos();

            // Reducir la ventana si no cabe en la pantalla
            Escalado.AjustarDialogo(this);

            this.Load += Gestiondelexpediente_Load;
        }

        private void Gestiondelexpediente_Load(object sender, EventArgs e)
        {
            // Las dos pestañas ocupan el ancho completo (con el tamaño final de la ventana)
            tabsCorreo.TabButtonSize = new Size((tabsCorreo.Width - 8) / 2, tabsCorreo.TabButtonSize.Height);
            tabsCorreo.ItemSize = tabsCorreo.TabButtonSize;

            if (!CargarDatos())
            {
                Funciones.Aviso(
                    "No se encontró el expediente EXP-" + numeroExpediente + ".",
                    "Gestión del expediente"
                );

                this.Close();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            // Evento conectado en el diseñador (sin acción)
        }

        private void ConectarEventos()
        {
            // Botones de la ficha
            But_restaurar.Click += btnVerDetalle_Click;
            guna2Button1.Click += btnModificar_Click;
            guna2Button2.Click += btnEliminar_Click;
            guna2Button3.Click += btnNotificar_Click;
            guna2Button4.Click += btnVerCorreo_Click;
            guna2Button5.Click += btnIniciarAtencion_Click;
            guna2Button6.Click += btnFinalizar_Click;

            // Pestaña Notificar al solicitante
            btnEnviarCorreo.Click += btnPrepararNotificacion_Click;
            btnCancelarCorreo.Click += btnCerrarPanel_Click;
            btnVerPdfCorreo.Click += btnVerPdfCorreo_Click;

            // Pestaña Derivar al encargado del área
            btnPrepararDerivacion.Click += btnPrepararDerivacion_Click;
            btnCancelarDerivacion.Click += btnCerrarPanel_Click;
            btnVerPdfArea.Click += btnVerPdfCorreo_Click;
            cmbAreaDerivar.SelectedIndexChanged += cmbAreaDerivar_SelectedIndexChanged;

            // Panel Correos preparados
            btnCerrarVerCorreo.Click += btnCerrarPanel_Click;

            // Panel Iniciar atención
            btnConfirmarInicio.Click += btnConfirmarInicio_Click;
            btnCancelarInicio.Click += btnCerrarPanel_Click;

            // Panel Finalizar expediente
            cmbResultado.SelectedIndexChanged += cmbResultado_SelectedIndexChanged;
            btnGuardarFinalizacion.Click += btnGuardarFinalizacion_Click;
            btnCancelarFinalizacion.Click += btnCerrarPanel_Click;

            // Límites de los textos
            Restricciones.Texto(txtObservacion, Restricciones.MAX_OBSERVACION);
            Restricciones.Texto(txtAsunto, 200);
            Restricciones.Texto(txtMensaje, 1000);
            Restricciones.Texto(txtEncargado, Restricciones.MAX_NOMBRE);
            Restricciones.Texto(txtCorreoEncargado, Restricciones.MAX_CORREO);
            Restricciones.Texto(txtAsuntoArea, 200);
            Restricciones.Texto(txtMensajeArea, 1000);
        }


        // =========================================================
        // CARGAR DATOS REALES DEL EXPEDIENTE
        // =========================================================

        private bool CargarDatos()
        {
            string[] datos = Funciones.BuscarExpediente(numeroExpediente);

            if (datos == null)
            {
                return false;
            }

            campos = Funciones.CompletarCampos(datos);

            this.Text = "Gestión del expediente EXP-" + numeroExpediente;

            // Encabezado: código, estado (centrado en su recuadro) y asunto
            guna2HtmlLabel1.Text = "EXP-" + numeroExpediente;
            guna2HtmlLabel2.Text = Mostrar(campos[9]);
            guna2HtmlLabel2.Left = guna2TextBox1.Left + (guna2TextBox1.Width - guna2HtmlLabel2.Width) / 2;
            MostrarRecortado(guna2HtmlLabel3, campos[3], 55);

            // Datos del expediente
            MostrarRecortado(guna2HtmlLabel12, campos[2], 22);                                // Documento
            MostrarRecortado(guna2HtmlLabel13, campos[3], 22);                                // Motivo
            MostrarRecortado(guna2HtmlLabel16, campos[4], 22);                                // Solicitante
            MostrarRecortado(guna2HtmlLabel14, Mostrar(campos[Funciones.CAMPO_AREA]), 22);   // Área

            // Seguimiento: historial con fecha y hora
            guna2HtmlLabel7.Text = "Seguimiento";
            lstSeguimiento.Items.Clear();

            string[] historial = Funciones.HistorialDelExpediente(numeroExpediente);

            for (int i = 0; i < historial.Length; i++)
            {
                lstSeguimiento.Items.Add(historial[i]);
            }

            ActualizarBotones();

            return true;
        }

        // Muestra "Sin registrar" cuando falta el dato
        private string Mostrar(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? Funciones.SIN_REGISTRAR : valor.Trim();
        }

        // Recorta textos largos y muestra el completo al pasar el mouse
        private void MostrarRecortado(Control etiqueta, string texto, int maximo)
        {
            texto = Mostrar(texto);
            etiqueta.Text = texto.Length > maximo ? texto.Substring(0, maximo - 1) + "…" : texto;
            ayuda.SetToolTip(etiqueta, texto);
        }

        private string Estado()
        {
            return campos == null ? "" : campos[9].Trim();
        }

        private string RutaPDF()
        {
            return campos == null ? "" : campos[10].Trim();
        }


        // =========================================================
        // HABILITAR LOS BOTONES SEGÚN EL ESTADO
        // =========================================================

        private void ActualizarBotones()
        {
            bool pendiente = Funciones.EsEstado(Estado(), Funciones.PENDIENTE);
            bool enProceso = Funciones.EsEstado(Estado(), Funciones.EN_PROCESO);
            bool finalizado = Funciones.EsEstado(Estado(), Funciones.FINALIZADO);
            bool hayCorreos = Funciones.CorreosDelExpediente(numeroExpediente).Length > 0;

            But_restaurar.Enabled = true;            // Ver detalle
            guna2Button1.Enabled = !finalizado;      // Modificar
            guna2Button2.Enabled = !finalizado;      // Eliminar
            guna2Button3.Enabled = true;             // Preparar correo (también informa el cierre)
            guna2Button4.Enabled = hayCorreos;       // Ver correos preparados
            guna2Button5.Enabled = pendiente;        // Iniciar atención
            guna2Button6.Enabled = enProceso;        // Finalizar expediente
        }


        // =========================================================
        // PANELES: solo uno visible a la vez
        // =========================================================

        private void MostrarPanel(Guna2Panel panel)
        {
            Guna2Panel[] paneles = { panelCorreo, panelVerCorreo, panelIniciar, panelFinalizar };

            for (int i = 0; i < paneles.Length; i++)
            {
                paneles[i].Visible = paneles[i] == panel;
            }

            // Con un panel abierto, los botones de la ficha se ocultan
            // para que no asomen por los márgenes del panel
            Control[] fondo = { But_restaurar, guna2Button1, guna2Button2, guna2Button3,
                                guna2Button4, guna2Button5, guna2Button6, lstSeguimiento };

            for (int i = 0; i < fondo.Length; i++)
            {
                fondo[i].Visible = panel == null;
            }

            if (panel != null)
            {
                panel.BringToFront();
            }
        }

        // Cancelar / Cerrar: oculta el panel sin guardar nada
        private void btnCerrarPanel_Click(object sender, EventArgs e)
        {
            MostrarPanel(null);
        }


        // =========================================================
        // 1. VER DETALLE: datos completos y PDF
        // =========================================================

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            using (VER_DETALLE_DEL_EXPEDIENTE ventana = new VER_DETALLE_DEL_EXPEDIENTE(numeroExpediente))
            {
                ventana.ShowDialog(this);
            }
        }


        // =========================================================
        // 2. MODIFICAR: editar, guardar o cancelar
        // =========================================================

        private void btnModificar_Click(object sender, EventArgs e)
        {
            CargarDatos();

            if (Funciones.EsEstado(Estado(), Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente está Finalizado y no se puede modificar.",
                    "Modificar expediente"
                );

                return;
            }

            using (MODIFICAR_DETALLE_DEL_EXPEDIENTE ventana = new MODIFICAR_DETALLE_DEL_EXPEDIENTE(numeroExpediente))
            {
                ventana.ShowDialog(this);
            }

            CargarDatos();
        }


        // =========================================================
        // 3. ELIMINAR: confirmación con el código, PDF protegido
        // =========================================================

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            CargarDatos();

            if (Funciones.EsEstado(Estado(), Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente está Finalizado.\n" +
                    "Se conserva para consulta y no se puede eliminar.",
                    "Eliminar expediente"
                );

                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar el expediente EXP-" + numeroExpediente + "?\n\n" +
                "Esta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            string rutaPDF;

            if (!Funciones.EliminarExpediente(numeroExpediente, out rutaPDF))
            {
                Funciones.Aviso("No se pudo eliminar el expediente.", "Eliminar expediente");
                return;
            }

            // Solo se borra la copia hecha por el sistema; nunca el original
            Funciones.BorrarCopiaPDF(rutaPDF, numeroExpediente);

            ExpedienteEliminado = true;

            MessageBox.Show(
                "El expediente EXP-" + numeroExpediente + " fue eliminado correctamente.",
                "Expediente eliminado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.Close();
        }


        // =========================================================
        // 4. PREPARAR CORREO (dos pestañas, cada una con su destinatario)
        //   - Notificar al solicitante: correo del solicitante (solo lectura)
        //   - Derivar al encargado del área: área responsable y encargado
        // Se abre Gmail en el navegador predeterminado con el
        // destinatario, el asunto y el mensaje. El sistema NO envía
        // el correo: solo registra lo que realmente hizo (correo
        // preparado y derivación registrada). El estado no cambia.
        // =========================================================

        private void btnNotificar_Click(object sender, EventArgs e)
        {
            CargarDatos();
            PrepararNotificacion();
            PrepararDerivacion();

            tabsCorreo.SelectedTab = tabSolicitante;
            MostrarPanel(panelCorreo);
        }

        // ¿El PDF del expediente existe en el disco?
        private bool ExistePDF()
        {
            string pdf = RutaPDF();
            return Funciones.TienePDF(pdf) && File.Exists(pdf);
        }

        // Nombre del PDF para mostrar
        private string NombrePDF()
        {
            if (ExistePDF())
            {
                return Path.GetFileName(RutaPDF());
            }

            return Funciones.TienePDF(RutaPDF()) ? "PDF no encontrado" : "Sin PDF";
        }

        // Pestaña 1: Notificar al solicitante
        // (correo, nombre, número y estado del expediente)
        private void PrepararNotificacion()
        {
            string nombre = Mostrar(campos[4]);
            string motivo = Mostrar(campos[3]);
            string codigo = "EXP-" + numeroExpediente;

            // Destinatario: el correo del solicitante (solo lectura)
            txtPara.Text = campos[8].Trim();

            // Asunto y mensaje (editables)
            txtAsunto.Text = "Actualización de su expediente " + codigo;
            txtMensaje.Text =
                "Hola, " + nombre + ". Su expediente " + codigo +
                ", sobre " + motivo + ", se encuentra en estado " + Mostrar(Estado()) + ".";

            // Adjunto
            lblNombrePdf.Text = NombrePDF();
            ayuda.SetToolTip(lblNombrePdf, lblNombrePdf.Text);
            btnVerPdfCorreo.Enabled = ExistePDF();
        }

        // Pestaña 2: Derivar al encargado del área
        private void PrepararDerivacion()
        {
            string area = campos[Funciones.CAMPO_AREA].Trim();

            // Área registrada (solo lectura) y área responsable (se puede cambiar)
            txtAreaDestino.Text = Mostrar(area);

            cmbAreaDerivar.SelectedIndex = -1;
            cmbAreaDerivar.SelectedItem = Funciones.EstaEnLista(area, Funciones.Areas) ? area : null;

            CargarEncargado();

            // Adjunto
            lblNombrePdfArea.Text = NombrePDF();
            ayuda.SetToolTip(lblNombrePdfArea, lblNombrePdfArea.Text);
            btnVerPdfArea.Enabled = ExistePDF();
        }

        // Área responsable elegida ("" si no hay)
        private string AreaElegida()
        {
            return cmbAreaDerivar.SelectedItem == null ? "" : cmbAreaDerivar.SelectedItem.ToString();
        }

        // Carga el encargado guardado del área y prepara asunto y mensaje
        private void CargarEncargado()
        {
            string area = AreaElegida();
            string encargado;
            string correo;

            Funciones.EncargadoDeArea(area, out encargado, out correo);

            txtEncargado.Text = encargado;
            txtCorreoEncargado.Text = correo;

            string codigo = "EXP-" + numeroExpediente;

            txtAsuntoArea.Text = "Derivación del expediente " + codigo + " - " + Mostrar(campos[3]);
            txtMensajeArea.Text =
                "Estimado(a) encargado(a) del área de " + (area != "" ? area : "...") + ":\r\n\r\n" +
                "Se deriva el expediente " + codigo + " (" + Mostrar(campos[2]) + "), sobre " + Mostrar(campos[3]) +
                ", presentado por " + Mostrar(campos[4]) + ".\r\n" +
                "Adjunto el expediente para su revisión. Por favor, confirme quién lo atenderá.";
        }

        private void cmbAreaDerivar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (campos != null)
            {
                CargarEncargado();
            }
        }

        private void btnVerPdfCorreo_Click(object sender, EventArgs e)
        {
            Funciones.AbrirPDF(RutaPDF());
        }

        // El PDF debe existir antes de preparar cualquier correo
        private bool PDFListo(string titulo)
        {
            if (ExistePDF())
            {
                return true;
            }

            Funciones.Aviso(
                Funciones.TienePDF(RutaPDF())
                    ? "No se encontró el PDF del expediente en:\n" + RutaPDF() + "\n\nVuelva a adjuntarlo en Modificar antes de preparar el correo."
                    : "El expediente no tiene PDF. Adjúntelo en Modificar antes de preparar el correo.",
                titulo);

            return false;
        }

        // Preparar correo: Notificar al solicitante
        private void btnPrepararNotificacion_Click(object sender, EventArgs e)
        {
            string para = txtPara.Text.Trim();

            if (para == "")
            {
                Funciones.Aviso("El expediente no tiene correo del solicitante. Regístrelo en Modificar.", "Notificar al solicitante");
                return;
            }

            if (!Funciones.EsCorreoConFormato(para))
            {
                Funciones.Aviso("El correo del solicitante no es válido. Corríjalo en Modificar.", "Notificar al solicitante");
                return;
            }

            if (!Funciones.Validar(txtAsunto.Text.Trim() != "",
                    "Ingrese el asunto.", "Campo obligatorio", txtAsunto)) return;

            if (!Funciones.Validar(txtMensaje.Text.Trim() != "",
                    "Ingrese el mensaje.", "Campo obligatorio", txtMensaje)) return;

            if (!PDFListo("Notificar al solicitante")) return;

            if (!AbrirCorreo(para, txtAsunto.Text.Trim(), txtMensaje.Text.Trim(), "Solicitante"))
            {
                return;
            }

            // Solo lo que realmente ocurrió: el correo quedó preparado
            Funciones.RegistrarHistorial(numeroExpediente, Funciones.ACCION_CORREO_PREPARADO, Funciones.UsuarioActual,
                "Notificación al solicitante: " + para);

            TerminarCorreo(para);
        }

        // Preparar correo: Derivar al encargado del área
        private void btnPrepararDerivacion_Click(object sender, EventArgs e)
        {
            string area = AreaElegida();
            string encargado = txtEncargado.Text.Trim();
            string para = txtCorreoEncargado.Text.Trim();

            if (!Funciones.Validar(area != "",
                    "Seleccione el área responsable.", "Campo obligatorio", cmbAreaDerivar)) return;

            if (!Funciones.Validar(encargado != "",
                    "Ingrese el nombre del encargado del área " + area + ".", "Campo obligatorio", txtEncargado)) return;

            if (!Funciones.Validar(para != "",
                    "Ingrese el correo del encargado del área " + area + ".", "Campo obligatorio", txtCorreoEncargado)) return;

            if (!Funciones.Validar(Funciones.EsCorreoConFormato(para),
                    "El correo del encargado no es válido.", "Dato inválido", txtCorreoEncargado)) return;

            if (!Funciones.Validar(txtAsuntoArea.Text.Trim() != "",
                    "Ingrese el asunto.", "Campo obligatorio", txtAsuntoArea)) return;

            if (!Funciones.Validar(txtMensajeArea.Text.Trim() != "",
                    "Ingrese el mensaje.", "Campo obligatorio", txtMensajeArea)) return;

            if (!PDFListo("Derivar al área")) return;

            if (!AbrirCorreo(para, txtAsuntoArea.Text.Trim(), txtMensajeArea.Text.Trim(), area))
            {
                return;
            }

            // Se recuerda el encargado para la próxima derivación a esa área
            Funciones.GuardarEncargadoDeArea(area, encargado, para);

            // Dos acciones reales: la derivación al área y el correo preparado
            Funciones.RegistrarHistorial(numeroExpediente, Funciones.ACCION_DERIVACION, area,
                "Encargado: " + encargado + " (" + para + ")");
            Funciones.RegistrarHistorial(numeroExpediente, Funciones.ACCION_CORREO_PREPARADO, Funciones.UsuarioActual,
                "Derivación al área " + area + ": " + para);

            TerminarCorreo(para);
        }

        // Abre Gmail en el navegador y guarda el correo preparado
        private bool AbrirCorreo(string para, string asunto, string mensaje, string destinatario)
        {
            string error;

            if (!Funciones.AbrirGmail(para, asunto, mensaje, out error))
            {
                MessageBox.Show(
                    "No se pudo abrir Gmail. No se registró nada.\n\n" + error,
                    "Preparar correo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }

            Funciones.RegistrarCorreo(numeroExpediente, para, asunto, mensaje, NombrePDF(), destinatario);
            return true;
        }

        // Cierra el panel y recuerda adjuntar el PDF antes de enviar
        private void TerminarCorreo(string para)
        {
            MostrarPanel(null);
            CargarDatos();

            MessageBox.Show(
                "Se abrió Gmail en su navegador con el mensaje para " + para + ".\n" +
                "Si Gmail le pide iniciar sesión, hágalo y el mensaje aparecerá listo.\n\n" +
                "Antes de enviarlo, adjunte el PDF del expediente (botón Ver PDF):\n" + RutaPDF() + "\n\n" +
                "El correo NO se ha enviado todavía: envíelo desde Gmail.\n" +
                "El expediente mantiene su estado: " + Mostrar(Estado()) + ".",
                "Correo preparado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // =========================================================
        // 5. VER CORREOS PREPARADOS
        // =========================================================

        private void btnVerCorreo_Click(object sender, EventArgs e)
        {
            string[] correos = Funciones.CorreosDelExpediente(numeroExpediente);

            if (correos.Length == 0)
            {
                Funciones.Aviso("Este expediente no tiene correos preparados.", "Ver correos preparados");
                return;
            }

            lblTituloVerCorreo.Text = "Correos preparados (" + correos.Length + ")";

            string texto = "";

            // Del más reciente al más antiguo
            for (int i = correos.Length - 1; i >= 0; i--)
            {
                string[] c = Funciones.CompletarCampos(correos[i].Split('|'));

                texto +=
                    "Fecha: " + c[1] + Environment.NewLine +
                    "Destinatario: " + Mostrar(c[6]) + Environment.NewLine +
                    "Para: " + c[2] + Environment.NewLine +
                    "Asunto: " + c[3] + Environment.NewLine +
                    "Adjunto: " + c[5] + Environment.NewLine +
                    "Mensaje:" + Environment.NewLine + Funciones.MensajeDeCorreo(c[4]) + Environment.NewLine +
                    "----------------------------------------" + Environment.NewLine;
            }

            txtDetalleCorreo.Text = texto;
            MostrarPanel(panelVerCorreo);
        }


        // =========================================================
        // 6. INICIAR ATENCIÓN: Pendiente -> En proceso
        // Muestra el área responsable guardada en Registro.
        // Si es un expediente antiguo sin área, se elige una.
        // =========================================================

        private void btnIniciarAtencion_Click(object sender, EventArgs e)
        {
            CargarDatos();

            if (!Funciones.EsEstado(Estado(), Funciones.PENDIENTE))
            {
                Funciones.Aviso("Solo se puede iniciar un expediente Pendiente.", "Iniciar atención");
                return;
            }

            string area = campos[Funciones.CAMPO_AREA].Trim();
            bool tieneArea = area != "";

            txtAreaResponsable.Text = area;
            txtAreaResponsable.Visible = tieneArea;

            cmbAreaResponsable.SelectedIndex = -1;
            cmbAreaResponsable.Visible = !tieneArea;

            MostrarPanel(panelIniciar);
        }

        private void btnConfirmarInicio_Click(object sender, EventArgs e)
        {
            string area = txtAreaResponsable.Visible
                ? txtAreaResponsable.Text.Trim()
                : (cmbAreaResponsable.SelectedItem == null ? "" : cmbAreaResponsable.SelectedItem.ToString());

            if (!Funciones.Validar(area != "",
                    "Seleccione el área responsable.", "Campo obligatorio", cmbAreaResponsable)) return;

            if (!Funciones.IniciarAtencion(numeroExpediente, area))
            {
                return;
            }

            MostrarPanel(null);
            CargarDatos();

            MessageBox.Show(
                "La atención del expediente EXP-" + numeroExpediente + " se inició.\n" +
                "Área responsable: " + area,
                "Iniciar atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // =========================================================
        // 7. FINALIZAR EXPEDIENTE: En proceso -> Finalizado
        // =========================================================

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            CargarDatos();

            if (!Funciones.EsEstado(Estado(), Funciones.EN_PROCESO))
            {
                Funciones.Aviso("Solo se puede finalizar un expediente En proceso.", "Finalizar expediente");
                return;
            }

            cmbResultado.SelectedIndex = -1;
            lblSignificado.Text = "Seleccione el resultado de la atención.";
            txtObservacion.Text = "";

            MostrarPanel(panelFinalizar);
        }

        private void cmbResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cmbResultado.SelectedIndex;

            lblSignificado.Text = i >= 0 && i < Funciones.SignificadoResultados.Length
                ? Funciones.SignificadoResultados[i]
                : "Seleccione el resultado de la atención.";
        }

        private void btnGuardarFinalizacion_Click(object sender, EventArgs e)
        {
            if (!Funciones.Validar(cmbResultado.SelectedIndex != -1,
                    "Seleccione el resultado: Atendido, Aprobado o Rechazado.",
                    "Campo obligatorio", cmbResultado)) return;

            string observacion = txtObservacion.Text.Trim();

            if (!Funciones.Validar(observacion != "",
                    "Ingrese la observación final explicando lo resuelto.",
                    "Campo obligatorio", txtObservacion)) return;

            if (!Restricciones.LargoValido(txtObservacion, Restricciones.MAX_OBSERVACION, "Observación final")) return;

            if (!Funciones.Validar(Funciones.TextoValidoParaArchivo(observacion),
                    "La observación no puede contener el carácter | ni saltos de línea.\n" +
                    "Escríbala en un solo párrafo.",
                    "Observación inválida", txtObservacion)) return;

            string resultado = cmbResultado.SelectedItem.ToString();

            if (!Funciones.FinalizarExpediente(numeroExpediente, resultado, observacion))
            {
                return;
            }

            MostrarPanel(null);
            CargarDatos();

            MessageBox.Show(
                "El expediente EXP-" + numeroExpediente + " fue finalizado como \"" + resultado + "\".\n" +
                "Queda disponible para consulta.",
                "Finalizar expediente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
