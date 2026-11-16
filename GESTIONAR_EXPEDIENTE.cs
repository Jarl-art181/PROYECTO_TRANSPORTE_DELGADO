using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class GESTIONAR_EXPEDIENTE : Form
    {
        public GESTIONAR_EXPEDIENTE()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Botones y panel de seguimiento (antes de escalar,
            // para que se ajusten igual que los demás controles)
            CrearControlesSeguimiento();

            // Botón "Gestionar" colocado en el diseñador
            ConectarBotonGestionar();

            // Ajustar el diseño al tamaño y escalado de la pantalla
            Escalado.AjustarPantallaCompleta(this);

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

            // Cargar la tabla una sola vez
            CargarExpedientes();
        }


        // ==========================================
        // NAVEGACIÓN
        // Abre el formulario elegido y cierra el actual
        // para que no se acumulen ventanas abiertas.
        // ==========================================

        private void AbrirFormulario(Form ventana)
        {
            ventana.Show();
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {
            // Evento conectado en el diseñador (sin acción)
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Evento conectado en el diseñador (sin acción)
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            // Ya estamos en Gestionar Expediente
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new INICIO());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REGISTRO_DE_EXPEDIENTE());
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONSULTAR_EXPEDIENTE());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new BUSCAR_EXPEDIENTE());
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REPORTES());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONFIGURACIÓN());
        }

        private void GESTIONAR_EXPEDIENTE_Load(object sender, EventArgs e)
        {
            // Los datos ya se cargaron en el constructor
        }



        private void CargarExpedientes()
        {
            // Limpiar los registros anteriores
            dgvExpedientes.Rows.Clear();

            // Verificar que exista el archivo
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
                // ==========================================
                // LEER SOLO EXPEDIENTES VÁLIDOS
                // ==========================================

                string[] registros = Funciones.LeerExpedientesValidos();

                // RECORRER CON FOR
                for (int i = 0; i < registros.Length; i++)
                {
                    string[] datos = registros[i].Split('|');

                    // ======================================
                    // NÚMERO DE EXPEDIENTE (8 dígitos)
                    // ======================================

                    string numero = Funciones.NormalizarNumero(datos[0]);

                    if (numero == "")
                    {
                        numero = datos[0].Trim();
                    }

                    // ======================================
                    // NÚMERO DE PLACA
                    // ======================================

                    string placa = "Sin registrar";

                    if (datos.Length > 11)
                    {
                        if (!string.IsNullOrWhiteSpace(datos[11]))
                        {
                            placa = datos[11]
                                .Trim()
                                .ToUpperInvariant();
                        }
                    }

                    // ======================================
                    // CREAR FILA EN LA TABLA
                    // ======================================

                    int fila = dgvExpedientes.Rows.Add();

                    // N.º EXPEDIENTE
                    dgvExpedientes.Rows[fila]
                        .Cells["colExpediente"].Value =
                        "EXP-" + numero;

                    // FECHA
                    dgvExpedientes.Rows[fila]
                        .Cells["colFechaRegistro"].Value =
                        datos[1];

                    // TIPO DE DOCUMENTO
                    dgvExpedientes.Rows[fila]
                        .Cells["colDocumento"].Value =
                        datos[2];

                    // PLACA
                    dgvExpedientes.Rows[fila]
                        .Cells["Columnplaca"].Value =
                        placa;

                    // DESCRIPCIÓN
                    dgvExpedientes.Rows[fila]
                        .Cells["ColuDescripción"].Value =
                        datos[3];

                    // SOLICITANTE
                    dgvExpedientes.Rows[fila]
                        .Cells["coloSolicitante"].Value =
                        datos[4];

                    // DNI
                    dgvExpedientes.Rows[fila]
                        .Cells["colDNI"].Value =
                        datos[5];

                    // EMPRESA
                    dgvExpedientes.Rows[fila]
                        .Cells["coloEmpresa"].Value =
                        datos[6];

                    // TELÉFONO
                    dgvExpedientes.Rows[fila]
                        .Cells["colTelefono"].Value =
                        datos[7];

                    // CORREO
                    dgvExpedientes.Rows[fila]
                        .Cells["colCorreo"].Value =
                        datos[8];

                    // ESTADO
                    dgvExpedientes.Rows[fila]
                        .Cells["coluEstadoExpediente"].Value =
                        datos[9];
                }

                // Quitar selección inicial
                dgvExpedientes.ClearSelection();

                dgvExpedientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los expedientes:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private void But_Ordenar_Click(object sender, EventArgs e)
        {
            // ==========================================
            // 1. VALIDAR CRITERIO DE ORDENAMIENTO
            // ==========================================

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

            // ==========================================
            // 2. VALIDAR ASCENDENTE O DESCENDENTE
            // ==========================================

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

            // ==========================================
            // 3. VERIFICAR QUE EXISTAN EXPEDIENTES
            // ==========================================

            if (dgvExpedientes.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No hay expedientes registrados para ordenar.",
                    "Ordenamiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // ==========================================
            // 4. IDENTIFICAR LA COLUMNA
            // ==========================================

            string columna = "";

            switch (combo_orden.Text)
            {
                case "N.º de Expediente":
                    columna = "colExpediente";
                    break;

                case "Fecha":
                    columna = "colFechaRegistro";
                    break;

                case "Tipo de documento":
                    columna = "colDocumento";
                    break;

                case "Solicitante":
                    columna = "coloSolicitante";
                    break;

                case "Empresa":
                    columna = "coloEmpresa";
                    break;

                case "Estado":
                    columna = "coluEstadoExpediente";
                    break;

                default:
                    MessageBox.Show(
                        "Seleccione un criterio válido.",
                        "Ordenamiento",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
            }

            // ==========================================
            // 5. VALIDAR DATOS ANTES DE ORDENAR
            // ==========================================

            string titulo;
            string problema = ProblemaParaOrdenar(columna, out titulo);

            if (problema != "")
            {
                MessageBox.Show(
                    problema,
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // ==========================================
            // 6 y 7. MÉTODO DE ORDENAMIENTO BURBUJA
            // ==========================================

            OrdenarTabla(columna, but_Ascendente.Checked);

            // Recordar el orden para mantenerlo al volver
            // de la ficha de gestión
            ultimaColumnaOrden = columna;
            ultimoOrdenAscendente = but_Ascendente.Checked;

            // ==========================================
            // 8. QUITAR SELECCIÓN
            // ==========================================

            dgvExpedientes.ClearSelection();

            dgvExpedientes.Refresh();

            // ==========================================
            // 9. MENSAJE FINAL
            // ==========================================

            MessageBox.Show(
                "Expedientes ordenados correctamente.",
                "Ordenamiento",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // Orden elegido por última vez ("" = sin ordenar)
        private string ultimaColumnaOrden = "";
        private bool ultimoOrdenAscendente = true;


        // ==========================================
        // REVISAR QUE LOS DATOS SE PUEDAN ORDENAR
        // Devuelve "" si todo está bien, o el mensaje del problema.
        // ==========================================

        private string ProblemaParaOrdenar(string columna, out string titulo)
        {
            titulo = "";

            for (int i = 0; i < dgvExpedientes.Rows.Count; i++)
            {
                string valor = Convert.ToString(
                    dgvExpedientes.Rows[i].Cells[columna].Value
                ).Trim();

                // VALIDAR FECHAS
                if (columna == "colFechaRegistro")
                {
                    DateTime fechaValidada;

                    if (!DateTime.TryParseExact(
                        valor,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fechaValidada))
                    {
                        titulo = "Fecha incorrecta";
                        return "Se encontró una fecha inválida.\n" +
                               "El formato correcto es dd/MM/yyyy.";
                    }
                }

                // VALIDAR NÚMEROS DE EXPEDIENTE
                if (columna == "colExpediente")
                {
                    string numero = valor.Replace("EXP-", "");

                    long numeroValidado;

                    if (!long.TryParse(numero, out numeroValidado))
                    {
                        titulo = "Número incorrecto";
                        return "Se encontró un número de expediente inválido.";
                    }
                }
            }

            return "";
        }


        // ==========================================
        // ORDENAR LA TABLA (MÉTODO BURBUJA)
        // ==========================================

        private void OrdenarTabla(string columna, bool ascendente)
        {
            for (int i = 0; i < dgvExpedientes.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvExpedientes.Rows.Count - 1 - i; j++)
                {
                    // OBTENER LOS DOS DATOS A COMPARAR
                    string dato1 = Convert.ToString(
                        dgvExpedientes.Rows[j].Cells[columna].Value
                    ).Trim();

                    string dato2 = Convert.ToString(
                        dgvExpedientes.Rows[j + 1].Cells[columna].Value
                    ).Trim();

                    bool cambiar = false;

                    // ======================================
                    // ORDENAR POR FECHA
                    // ======================================

                    if (columna == "colFechaRegistro")
                    {
                        DateTime fecha1 = DateTime.ParseExact(
                            dato1,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture
                        );

                        DateTime fecha2 = DateTime.ParseExact(
                            dato2,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture
                        );

                        // ASCENDENTE: ANTIGUA A RECIENTE
                        if (ascendente)
                        {
                            if (fecha1 > fecha2)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: RECIENTE A ANTIGUA
                        if (!ascendente)
                        {
                            if (fecha1 < fecha2)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // ORDENAR POR NÚMERO DE EXPEDIENTE
                    // ======================================

                    else if (columna == "colExpediente")
                    {
                        string texto1 = dato1.Replace("EXP-", "");
                        string texto2 = dato2.Replace("EXP-", "");

                        long numero1 = long.Parse(texto1);
                        long numero2 = long.Parse(texto2);

                        // ASCENDENTE: MENOR A MAYOR
                        if (ascendente)
                        {
                            if (numero1 > numero2)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: MAYOR A MENOR
                        if (!ascendente)
                        {
                            if (numero1 < numero2)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // ORDENAR POR TEXTO
                    // ======================================

                    else
                    {
                        int comparacion = string.Compare(
                            dato1,
                            dato2,
                            StringComparison.CurrentCultureIgnoreCase
                        );

                        // ASCENDENTE: A - Z
                        if (ascendente)
                        {
                            if (comparacion > 0)
                            {
                                cambiar = true;
                            }
                        }

                        // DESCENDENTE: Z - A
                        if (!ascendente)
                        {
                            if (comparacion < 0)
                            {
                                cambiar = true;
                            }
                        }
                    }

                    // ======================================
                    // 7. INTERCAMBIAR FILAS COMPLETAS
                    // ======================================

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
        }


        private void but_eliminar_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                Funciones.Aviso(
                    "Seleccione un expediente para eliminar.",
                    "Eliminar expediente"
                );

                return;
            }

            // Obtener expediente seleccionado
            string expedienteSeleccionado =
                dgvExpedientes.SelectedRows[0]
                .Cells["colExpediente"]
                .Value?.ToString() ?? "";

            string numeroExpediente =
                Funciones.NormalizarNumero(expedienteSeleccionado);

            if (numeroExpediente == "")
            {
                Funciones.Aviso(
                    "No se pudo identificar el expediente.",
                    "Eliminar expediente"
                );

                return;
            }

            // Los expedientes finalizados se conservan para consulta
            if (Funciones.EsEstado(Funciones.EstadoDe(numeroExpediente), Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente " + expedienteSeleccionado + " está Finalizado.\n" +
                    "Se conserva para consulta y no se puede eliminar.",
                    "Eliminar expediente"
                );

                return;
            }

            // Confirmar eliminación según configuración (opción 5)
            if (Funciones.LeerOpcion(4, true))
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

            // Quitar solo la línea de este expediente.
            // Los demás registros se conservan aunque estén incompletos.
            string rutaPDF;

            if (!Funciones.EliminarExpediente(numeroExpediente, out rutaPDF))
            {
                Funciones.Aviso(
                    "No se pudo eliminar el expediente " +
                    expedienteSeleccionado + ".",
                    "Eliminar expediente"
                );

                return;
            }

            // Borrar el PDF solo si es una copia creada por el sistema
            // y ningún otro expediente la usa. El PDF original del
            // usuario nunca se borra.
            Funciones.BorrarCopiaPDF(rutaPDF, numeroExpediente);

            // Actualizar la tabla conservando el orden elegido
            RecargarYSeleccionar("");

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

            // Los expedientes finalizados no se modifican
            if (Funciones.EsEstado(Funciones.EstadoDe(numeroExpediente), Funciones.FINALIZADO))
            {
                Funciones.Aviso(
                    "El expediente " + expedienteSeleccionado + " está Finalizado.\n" +
                    "Se conserva para consulta y no se puede modificar.",
                    "Modificar expediente"
                );

                return;
            }

            MODIFICAR_DETALLE_DEL_EXPEDIENTE ventana =
                new MODIFICAR_DETALLE_DEL_EXPEDIENTE(
                    numeroExpediente
                );

            ventana.ShowDialog();

            // Actualizar conservando el orden y el expediente seleccionado
            RecargarYSeleccionar(Funciones.NormalizarNumero(numeroExpediente));
        }


        // =========================================================
        // ESTADO DEL EXPEDIENTE SELECCIONADO
        // ---------------------------------------------------------
        // Iniciar atención y Finalizar expediente se realizan desde
        // la ficha (Gestiondelexpediente), con el botón "Gestionar".
        // =========================================================

        private Guna.UI2.WinForms.Guna2HtmlLabel lblEstadoSeleccionado;


        // =========================================================
        // FRANJA AMARILLA SOBRE LA TABLA (guna2Panel10)
        // Muestra el título del listado y el estado de la fila
        // seleccionada, ambos centrados en la altura de la franja.
        // =========================================================

        private void CrearControlesSeguimiento()
        {
            Control franja = guna2Panel10;

            // El título está sobre la franja (en el mismo contenedor):
            // se centra verticalmente dentro de ella
            guna2HtmlLabel10.Top = franja.Top + (franja.Height - guna2HtmlLabel10.Height) / 2;
            guna2HtmlLabel10.BringToFront();

            // El estado se muestra a continuación del título
            int despuesDelTitulo = guna2HtmlLabel10.Right - franja.Left + 25;

            lblEstadoSeleccionado = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblEstadoSeleccionado.BackColor = System.Drawing.Color.Transparent;
            lblEstadoSeleccionado.ForeColor = System.Drawing.Color.Black;
            lblEstadoSeleccionado.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            lblEstadoSeleccionado.Text = "Seleccione un expediente de la tabla.";
            franja.Controls.Add(lblEstadoSeleccionado);

            lblEstadoSeleccionado.Location = new System.Drawing.Point(
                despuesDelTitulo,
                (franja.Height - lblEstadoSeleccionado.Height) / 2);

            // Actualizar el estado y los botones al seleccionar una fila
            dgvExpedientes.SelectionChanged += dgvExpedientes_SelectionChanged;
        }


        // =========================================================
        // ESTADO DE LA FILA SELECCIONADA
        // =========================================================

        private string NumeroSeleccionado()
        {
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                return "";
            }

            object valor = dgvExpedientes.SelectedRows[0].Cells["colExpediente"].Value;

            return Funciones.NormalizarNumero(valor == null ? "" : valor.ToString());
        }

        private string EstadoSeleccionado()
        {
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                return "";
            }

            object valor = dgvExpedientes.SelectedRows[0].Cells["coluEstadoExpediente"].Value;

            return valor == null ? "" : valor.ToString().Trim();
        }


        // =========================================================
        // HABILITAR O DESHABILITAR BOTONES SEGÚN EL ESTADO
        // Finalizado -> solo consulta (sin modificar ni eliminar)
        // =========================================================

        private void ActualizarBotonesSeguimiento()
        {
            string numero = NumeroSeleccionado();
            string estado = EstadoSeleccionado();

            bool finalizado = Funciones.EsEstado(estado, Funciones.FINALIZADO);

            But_Modificar.Enabled = !finalizado;
            but_eliminar.Enabled = !finalizado;

            // "Gestionar" solo con una fila seleccionada
            guna2Button9.Enabled = numero != "";

            if (numero == "")
            {
                lblEstadoSeleccionado.Text = "Seleccione un expediente de la tabla.";
            }
            else if (finalizado)
            {
                lblEstadoSeleccionado.Text = "EXP-" + numero + ": Finalizado (solo consulta)";
            }
            else
            {
                lblEstadoSeleccionado.Text = "EXP-" + numero + ": " + estado;
            }
        }

        private void dgvExpedientes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonesSeguimiento();
        }


        // Vuelve a cargar la tabla, mantiene el orden elegido
        // y selecciona el expediente indicado (por su código,
        // no por su posición, que puede cambiar al ordenar)
        private void RecargarYSeleccionar(string numero)
        {
            CargarExpedientes();

            // Volver a aplicar el último orden (sin mensajes)
            string titulo;

            if (ultimaColumnaOrden != "" &&
                ProblemaParaOrdenar(ultimaColumnaOrden, out titulo) == "")
            {
                OrdenarTabla(ultimaColumnaOrden, ultimoOrdenAscendente);
            }

            dgvExpedientes.ClearSelection();

            for (int i = 0; i < dgvExpedientes.Rows.Count; i++)
            {
                object valor = dgvExpedientes.Rows[i].Cells["colExpediente"].Value;

                if (numero != "" && valor != null &&
                    Funciones.NormalizarNumero(valor.ToString()) == numero)
                {
                    dgvExpedientes.CurrentCell = dgvExpedientes.Rows[i].Cells[0];
                    dgvExpedientes.Rows[i].Selected = true;
                    break;
                }
            }

            ActualizarBotonesSeguimiento();
        }


        // =========================================================
        // BOTÓN "GESTIONAR" (guna2Button9, colocado en el diseñador)
        // Se habilita solo con una fila seleccionada y abre la ficha
        // del expediente usando su CÓDIGO.
        // =========================================================

        private void ConectarBotonGestionar()
        {
            guna2Button9.Click += btnGestionar_Click;

            // El icono está encima del botón: también debe responder
            guna2PictureBox9.Click += btnGestionar_Click;
            guna2PictureBox9.Cursor = Cursors.Hand;

            // Sin fila seleccionada, deshabilitado
            guna2Button9.Enabled = false;
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            // El icono no se deshabilita: comprobar de nuevo
            if (!guna2Button9.Enabled)
            {
                return;
            }

            string numero = NumeroSeleccionado();

            if (numero == "")
            {
                Funciones.Aviso(
                    "Seleccione un expediente de la tabla.",
                    "Gestionar expediente"
                );

                return;
            }

            AbrirFichaDeGestion(numero);
        }

        // Abre la ficha del expediente y, al cerrarla, actualiza
        // la tabla conservando el orden y la selección
        private void AbrirFichaDeGestion(string numero)
        {
            using (Gestiondelexpediente ficha = new Gestiondelexpediente(numero))
            {
                ficha.ShowDialog(this);
            }

            // Al volver: datos actualizados, mismo orden y misma selección
            // (si se eliminó, simplemente ya no aparece en la tabla)
            RecargarYSeleccionar(numero);
        }
    }
   
    
}
