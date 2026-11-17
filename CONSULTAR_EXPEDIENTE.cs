
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class CONSULTAR_EXPEDIENTE : Form
    {
        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public CONSULTAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Ajustar el diseño al tamaño y escalado de la pantalla
            Escalado.AjustarPantallaCompleta(this);

            // Configurar la tabla
            dgvExpedientes.AllowUserToAddRows = false;
            dgvExpedientes.ReadOnly = true;
            dgvExpedientes.RowHeadersVisible = false;

            dgvExpedientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Utilizar la columna Placa ya creada
            Columnplaca.Visible = true;
            Columnplaca.HeaderText = "Placa";

            // Filtros: número EXP- + 8 dígitos y fechas dd/MM/yyyy
            // (al escribir y al pegar)
            Restricciones.Codigo(text_nu_expediente, false);
            Restricciones.Fecha(text_fecha_desde);
            Restricciones.Fecha(text_fecha_hasta);

            // Corregir el ejemplo del filtro
            text_nu_expediente.PlaceholderText =
                "Ej: EXP-00000001";

           

        }

        // =====================================================
        // AGREGAR UN EXPEDIENTE A LA TABLA
        // =====================================================


        private void AgregarExpediente(string[] datos)
        {
            // Verificar que el expediente tenga los datos necesarios
            if (datos.Length < 10)
            {
                return;
            }

            // Número de expediente
            string numero = Funciones.NormalizarNumero(datos[0]);

            if (numero == "")
            {
                return;
            }

            // Obtener placa vehicular
            string placa = "Sin registrar";

            if (datos.Length > 11)
            {
                if (!string.IsNullOrWhiteSpace(datos[11]))
                {
                    placa = datos[11].Trim().ToUpperInvariant();
                }
            }

            // Crear nueva fila
            int fila = dgvExpedientes.Rows.Add();

            // Mostrar todos los datos.
            // PonerCelda solo escribe si la columna existe en el diseño,
            // así la tabla sigue funcionando si se quita o agrega una columna.
            PonerCelda(fila, "colExpediente", "EXP-" + numero);
            PonerCelda(fila, "colFechaRegistro", datos[1]);
            PonerCelda(fila, "colDocumento", datos[2]);
            PonerCelda(fila, "Columnplaca", placa);
            PonerCelda(fila, "ColuDescripción", datos[3]);
            PonerCelda(fila, "coloSolicitante", datos[4]);
            PonerCelda(fila, "colDNI", datos[5]);
            PonerCelda(fila, "coloEmpresa", datos[6]);
            PonerCelda(fila, "colTelefono", datos[7]);
            PonerCelda(fila, "colCorreo", datos[8]);
            PonerCelda(fila, "coluEstadoExpediente", datos[9]);
        }

        // Escribe un valor en una celda solo si la columna existe
        private void PonerCelda(int fila, string columna, string valor)
        {
            if (dgvExpedientes.Columns.Contains(columna))
            {
                dgvExpedientes.Rows[fila].Cells[columna].Value = valor;
            }
        }


        // =====================================================
        // CARGAR TODOS LOS EXPEDIENTES
        // =====================================================

        private void CargarDatosExpedientes()
        {
            // Limpiar tabla
            dgvExpedientes.Rows.Clear();

            try
            {
                // Solo expedientes válidos (sin líneas vacías
                // ni registros incompletos)
                string[] registros =
                    Funciones.LeerExpedientesValidos();

                // Recorrer todos los expedientes
                for (int i = 0; i < registros.Length; i++)
                {
                    AgregarExpediente(registros[i].Split('|'));
                }

                dgvExpedientes.ClearSelection();
                dgvExpedientes.Refresh();

                // Resumen de Expedientes Registrados (sin selección)
                MostrarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los expedientes.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // CONFIGURACIÓN: CARGAR DATOS AUTOMÁTICAMENTE
        // =====================================================

        private bool CargarDatosAutomaticamente()
        {
            // Opción 3 de Configuración (por defecto: sí)
            return Funciones.LeerOpcion(2, true);
        }

        // =====================================================
        // BOTÓN BUSCAR / FILTRAR
        // =====================================================



        private void but_buscar_con_Click(object sender, EventArgs e)
        {
            // ==========================================
            // 1. OBTENER CRITERIOS DE BÚSQUEDA
            // ==========================================

            string numeroTexto =
                text_nu_expediente.Text.Trim().ToUpperInvariant();

            string tipoDocumento =
                comb_tipo_de_documento.Text.Trim();

            string estado =
                comb_estado.Text.Trim();

            string fechaDesdeTexto =
                text_fecha_desde.Text.Trim();

            string fechaHastaTexto =
                text_fecha_hasta.Text.Trim();


            // ==========================================
            // 2. IDENTIFICAR FILTROS
            // ==========================================

            bool buscarNumero =
                !string.IsNullOrWhiteSpace(numeroTexto);

            bool filtrarTipo =
                !string.IsNullOrWhiteSpace(tipoDocumento) &&
                !tipoDocumento.Equals(
                    "Todos",
                    StringComparison.OrdinalIgnoreCase);

            bool filtrarEstado =
                !string.IsNullOrWhiteSpace(estado) &&
                !estado.Equals(
                    "Todos",
                    StringComparison.OrdinalIgnoreCase);

            bool tieneFechaDesde =
                !string.IsNullOrWhiteSpace(fechaDesdeTexto);

            bool tieneFechaHasta =
                !string.IsNullOrWhiteSpace(fechaHastaTexto);

            bool filtrarFechas =
                tieneFechaDesde || tieneFechaHasta;


            // ==========================================
            // 3. VALIDAR BÚSQUEDA POR NÚMERO
            // ==========================================

            string numeroBuscado = "";

            if (buscarNumero)
            {
                // El número de expediente es una búsqueda independiente
                if (filtrarTipo || filtrarEstado || filtrarFechas)
                {
                    MessageBox.Show(
                        "Para buscar por número de expediente, " +
                        "deje vacíos los demás filtros.",
                        "Filtros de búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(
                    numeroTexto, @"^(EXP-)?[0-9]{8}$"))
                {
                    MessageBox.Show(
                        "Ingrese el formato EXP-12345678.",
                        "Número inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_nu_expediente.Focus();
                    return;
                }

                // Normalizar número
                numeroBuscado = Funciones.NormalizarNumero(numeroTexto);
            }


            // ==========================================
            // 4. VALIDAR FECHAS
            // ==========================================

            DateTime fechaDesde = DateTime.MinValue;
            DateTime fechaHasta = DateTime.MaxValue;

            if (filtrarFechas)
            {
                // Deben existir ambas fechas
                if (!tieneFechaDesde || !tieneFechaHasta)
                {
                    MessageBox.Show(
                        "Debe completar Fecha desde y Fecha hasta.",
                        "Fechas incompletas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                // Validar fecha inicial
                if (!Funciones.ConvertirFecha(fechaDesdeTexto, out fechaDesde))
                {
                    MessageBox.Show(
                        "La fecha desde no es válida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_desde.Focus();
                    return;
                }

                // Validar fecha final
                if (!Funciones.ConvertirFecha(fechaHastaTexto, out fechaHasta))
                {
                    MessageBox.Show(
                        "La fecha hasta no es válida. Use dd/MM/yyyy.",
                        "Fecha incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    text_fecha_hasta.Focus();
                    return;
                }

                // Validar orden de fechas
                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show(
                        "La fecha desde no puede ser posterior " +
                        "a la fecha hasta.",
                        "Rango incorrecto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
            }


            // ==========================================
            // 5. SI NO EXISTEN FILTROS
            // ==========================================

            if (!buscarNumero &&
                !filtrarTipo &&
                !filtrarEstado &&
                !filtrarFechas)
            {
                CargarDatosExpedientes();
                return;
            }


            // ==========================================
            // 6. VERIFICAR ARCHIVO
            // ==========================================

            if (!File.Exists(Funciones.RutaExpedientes()))
            {
                Funciones.Aviso(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado"
                );

                return;
            }


            // ==========================================
            // 7. LEER Y FILTRAR EXPEDIENTES
            // ==========================================

            try
            {
                string[] registros = Funciones.LeerExpedientesValidos();

                // Arreglo que guardará solamente las coincidencias
                string[] resultados = new string[registros.Length];

                int cantidadResultados = 0;

                for (int i = 0; i < registros.Length; i++)
                {
                    // Ignorar líneas vacías
                    if (string.IsNullOrWhiteSpace(registros[i]))
                    {
                        continue;
                    }

                    string[] datos = registros[i].Split('|');

                    if (datos.Length < 10)
                    {
                        continue;
                    }


                    // ======================================
                    // FILTRO A: SOLO POR NÚMERO
                    // ======================================

                    if (buscarNumero)
                    {
                        string numeroArchivo =
                            Funciones.NormalizarNumero(datos[0]);

                        if (numeroArchivo != numeroBuscado)
                        {
                            continue;
                        }

                        // Guardar coincidencia
                        resultados[cantidadResultados] = registros[i];

                        cantidadResultados++;

                        // Un expediente específico
                        break;
                    }


                    // ======================================
                    // FILTRO B: TIPO DE DOCUMENTO
                    // ======================================

                    if (filtrarTipo)
                    {
                        if (!datos[2].Trim().Equals(
                            tipoDocumento,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // FILTRO C: ESTADO
                    // ======================================

                    if (filtrarEstado)
                    {
                        if (!datos[9].Trim().Equals(
                            estado,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // FILTRO D: RANGO DE FECHAS
                    // ======================================

                    if (filtrarFechas)
                    {
                        DateTime fechaRegistro;

                        if (!Funciones.ConvertirFecha(datos[1], out fechaRegistro))
                        {
                            continue;
                        }

                        if (fechaRegistro < fechaDesde ||
                            fechaRegistro > fechaHasta)
                        {
                            continue;
                        }
                    }


                    // ======================================
                    // EXPEDIENTE QUE CUMPLE LOS FILTROS
                    // ======================================

                    resultados[cantidadResultados] = registros[i];

                    cantidadResultados++;
                }


                // ==========================================
                // 8. MOSTRAR LOS RESULTADOS EN LA TABLA
                // ==========================================

                // Eliminar las filas anteriores
                dgvExpedientes.Rows.Clear();

                // Mostrar únicamente las coincidencias
                for (int i = 0; i < cantidadResultados; i++)
                {
                    string[] datos = resultados[i].Split('|');

                    AgregarExpediente(datos);
                }

                // Quitar selección de filas
                dgvExpedientes.ClearSelection();

                // Actualizar visualmente la tabla
                dgvExpedientes.Refresh();


                // ==========================================
                // 9. MOSTRAR MENSAJE SI NO HAY RESULTADOS
                // ==========================================

                if (cantidadResultados == 0)
                {
                    MessageBox.Show(
                        "No se encontraron expedientes " +
                        "con los filtros ingresados.",
                        "Sin resultados",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al filtrar expedientes:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        // =====================================================
        // BOTÓN LIMPIAR
        // =====================================================

        private void but_limpieza_Click(
            object sender,
            EventArgs e)
        {
            // Limpiar número
            text_nu_expediente.Clear();

            // Limpiar tipo de documento
            comb_tipo_de_documento.SelectedIndex = -1;

            // Limpiar estado
            comb_estado.SelectedIndex = -1;

            // Limpiar ambas fechas
            text_fecha_desde.Clear();
            text_fecha_hasta.Clear();

            // Mostrar todos los expedientes
            CargarDatosExpedientes();

            // Colocar cursor
            text_nu_expediente.Focus();
        }

        // =====================================================
        // EXPEDIENTES REGISTRADOS: RESUMEN DE LA FILA ELEGIDA
        // Solicitante, documento, motivo, último movimiento
        // del historial e inicio de atención.
        // =====================================================

        // Código (8 dígitos) de la fila seleccionada ("" si no hay)
        private string NumeroSeleccionado()
        {
            if (dgvExpedientes.SelectedRows.Count == 0 ||
                !dgvExpedientes.Columns.Contains("colExpediente"))
            {
                return "";
            }

            object valor = dgvExpedientes.SelectedRows[0].Cells["colExpediente"].Value;

            return Funciones.NormalizarNumero(valor == null ? "" : valor.ToString());
        }

        private void MostrarResumen()
        {
            string numero = NumeroSeleccionado();
            string[] datos = numero != "" ? Funciones.BuscarExpediente(numero) : null;

            // Sin selección: valores vacíos y botón deshabilitado
            if (datos == null)
            {
                Label[] valores = { lblSolicitanteValor, lblDocumentoValor, lblMotivoValor,
                                    lblUltimoMovimientoValor, lblInicioAtencionValor };

                for (int i = 0; i < valores.Length; i++)
                {
                    valores[i].Text = "-";
                }

                lblSolicitanteValor.Text = "Seleccione un expediente de la tabla";
                guna2Button1.Enabled = false;
                return;
            }

            string[] campos = Funciones.CompletarCampos(datos);

            lblSolicitanteValor.Text = Mostrar(campos[4]);
            lblDocumentoValor.Text = Mostrar(campos[2]);
            lblMotivoValor.Text = Mostrar(campos[3]);
            lblUltimoMovimientoValor.Text = Funciones.UltimoMovimiento(numero);
            lblInicioAtencionValor.Text = Mostrar(campos[Funciones.CAMPO_INICIO]);

            guna2Button1.Enabled = true;
        }

        // "Sin registrar" cuando falta el dato
        private string Mostrar(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? Funciones.SIN_REGISTRAR : valor.Trim();
        }

        private void dgvExpedientes_SelectionChanged(object sender, EventArgs e)
        {
            MostrarResumen();
        }

        // =====================================================
        // BOTÓN "VER DETALLE O HISTORIAL"
        // Abre el historial del expediente seleccionado
        // (fecha, hora, acción, responsable y observación).
        // =====================================================

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string numero = NumeroSeleccionado();

            if (numero == "")
            {
                Funciones.Aviso("Seleccione un expediente de la tabla.", "Ver detalle o Historial");
                return;
            }

            using (HISTORIAL_DEL_EXPEDIENTE ventana = new HISTORIAL_DEL_EXPEDIENTE(numero))
            {
                ventana.ShowDialog(this);
            }

            // El historial puede haber cambiado (por ejemplo, desde Ver detalle)
            MostrarResumen();
        }

        // =====================================================
        // LOAD DEL FORMULARIO
        // =====================================================

        private void CONSULTAR_EXPEDIENTE_Load(
            object sender,
            EventArgs e)
        {
           
            // ==========================================
            // CARGAR TIPOS DE DOCUMENTO
            // ==========================================

            Funciones.LlenarCombo(comb_tipo_de_documento, Funciones.TiposDocumento);

            comb_tipo_de_documento.SelectedIndex = -1;

            // ==========================================
            // CARGAR ESTADOS
            // ==========================================

            Funciones.LlenarCombo(comb_estado, Funciones.Estados);

            comb_estado.SelectedIndex = -1;

            // ==========================================
            // CARGAR DATOS SEGÚN CONFIGURACIÓN
            // ==========================================

            if (CargarDatosAutomaticamente())
            {
                CargarDatosExpedientes();
            }
            else
            {
                dgvExpedientes.Rows.Clear();
            }
        }

        // =====================================================
        // BOTÓN CERRAR SESIÓN
        // =====================================================

        private void but_Cerrar_Click(
            object sender,
            EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        // =====================================================
        // BOTÓN INICIO
        // =====================================================

        private void but_inicio_Click_1(
            object sender,
            EventArgs e)
        {
            INICIO ventana = new INICIO();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void but_consultar_Click(
            object sender,
            EventArgs e)
        {
            // Estamos en Consultar Expedientes.
            // Actualizar los datos sin modificar los filtros
            but_buscar_con_Click(sender, e);
        }

        // =====================================================
        // BOTÓN BUSCAR EXPEDIENTE
        // =====================================================

        private void but_buscar_Click(
            object sender,
            EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN GESTIONAR EXPEDIENTE
        // =====================================================

        private void but_gestionar_Click(
            object sender,
            EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN REPORTES
        // =====================================================

        private void but_reportes_Click(
            object sender,
            EventArgs e)
        {
            REPORTES ventana = new REPORTES();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN CONFIGURACIÓN
        // =====================================================

        private void but_configuración_Click(
            object sender,
            EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();
            this.Close();
        }

        // =====================================================
        // BOTÓN REGISTRO DE EXPEDIENTE
        // =====================================================

        private void but_registro_Click(
            object sender,
            EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }
    }
}
