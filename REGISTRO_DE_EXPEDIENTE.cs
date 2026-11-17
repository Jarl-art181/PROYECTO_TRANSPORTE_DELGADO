using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class REGISTRO_DE_EXPEDIENTE : Form
    {
        // Ruta del PDF elegido por el usuario (archivo original)
        private string rutaPDF = "";

        public REGISTRO_DE_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Estado de solo lectura: "Pendiente"
            ConfigurarCampoEstado();

            // Límites y formatos de los campos (al escribir y al pegar)
            AplicarRestricciones();

            // Ajustar el diseño al tamaño y escalado de la pantalla
            Escalado.AjustarPantallaCompleta(this);
        }


        // =========================================================
        // CAMPO DE ESTADO (SOLO LECTURA)
        // Usa el cuadro tex_estado colocado en el diseñador.
        // Todo expediente nuevo se registra como "Pendiente".
        // =========================================================

        private void ConfigurarCampoEstado()
        {
            tex_estado.ReadOnly = true;
            tex_estado.TabStop = false;
            tex_estado.Text = Funciones.PENDIENTE;
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
            Funciones.LlenarCombo(ComboTipoDocumento, Funciones.TiposDocumento);

            // ÁREAS DE DESTINO (Administración, Operaciones, Contabilidad, Gerencia)
            Funciones.LlenarCombo(comb_destino, Funciones.Areas);

            ComboTipoDocumento.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // INICIALMENTE NO HAY PDF
            txtRutaPDF.Text = "";

            picPDF.Visible = false;
            lblNombrePDF.Visible = false;
            lblTipoPDF.Visible = false;

            lblTipoPDF.Text = "";
            lblNombrePDF.Text = "";


            // ESTADO: siempre "Pendiente" (no se puede cambiar aquí)
            tex_estado.Text = Funciones.PENDIENTE;


            // GENERAR NÚMERO SEGÚN CONFIGURACIÓN
            GenerarNumeroExpediente();
        }


        // =========================================================
        // ADJUNTAR PDF
        // =========================================================

        private void btnAdjuntarPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog abrirPDF = new OpenFileDialog())
            {
                abrirPDF.Filter = "Archivos PDF (*.pdf)|*.pdf";
                abrirPDF.Title = "Seleccionar documento PDF";

                if (abrirPDF.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Comprobar que no supere 10 MB
                if (!Funciones.PDFDentroDelLimite(abrirPDF.FileName))
                {
                    return;
                }

                // Guardar la ruta completa del original
                rutaPDF = abrirPDF.FileName;

                // Mostrar logo y nombre
                lblTipoPDF.Text = "Documento PDF";
                lblNombrePDF.Text = Path.GetFileName(abrirPDF.FileName);

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
                Funciones.Aviso(
                    "Primero debe adjuntar un documento PDF.",
                    "Aviso"
                );

                return;
            }

            Funciones.AbrirPDF(rutaPDF);
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
            textPLACA.Clear();
            textFecha.Clear();

            ComboTipoDocumento.SelectedIndex = -1;
            comb_destino.SelectedIndex = -1;
            tex_estado.Text = Funciones.PENDIENTE;

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
        // LÍMITES Y FORMATOS DE LOS CAMPOS
        // =========================================================

        private void AplicarRestricciones()
        {
            // EXP- fijo + 8 dígitos (si la numeración es manual)
            Restricciones.Codigo(txtN_expediente, true);

            Restricciones.Dni(textDNI);
            Restricciones.Celular(textTELEFONO);
            Restricciones.Placa(textPLACA);

            Restricciones.Texto(texNAME_USUARIO, Restricciones.MAX_NOMBRE);
            Restricciones.Texto(textEMPRESA, Restricciones.MAX_EMPRESA);
            Restricciones.Texto(textCORREO, Restricciones.MAX_CORREO);
            Restricciones.Texto(txtDescripcion, Restricciones.MAX_DESCRIPCION);

            // La fecha de registro es automática
            textFecha.ReadOnly = true;
        }


        // =========================================================
        // OBTENER SIGUIENTE NÚMERO DE EXPEDIENTE
        // Devuelve "" si ya se llegó a EXP-99999999.
        // =========================================================

        private string ObtenerSiguienteNumeroExpediente()
        {
            string[] registros = Funciones.LeerExpedientes();

            int mayor = 0;

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                // Compatible con expedientes anteriores
                string numero =
                    Funciones.NormalizarNumero(registros[i].Split('|')[0]);

                int valor;

                if (int.TryParse(numero, out valor) && valor > mayor)
                {
                    mayor = valor;
                }
            }

            // No se puede pasar de 8 dígitos
            if (mayor >= Restricciones.NUMERO_MAXIMO)
            {
                return "";
            }

            int siguiente = mayor + 1;

            return "EXP-" + siguiente.ToString("D8");
        }


        // =========================================================
        // GENERAR NÚMERO SEGÚN CONFIGURACIÓN
        // =========================================================

        private void GenerarNumeroExpediente()
        {
            // Opción 4: generar números automáticamente
            if (Funciones.LeerOpcion(3, false))
            {
                // Bloquear para evitar modificaciones
                txtN_expediente.ReadOnly = true;

                string siguiente = ObtenerSiguienteNumeroExpediente();

                if (siguiente == "")
                {
                    txtN_expediente.Text = "EXP-";

                    Funciones.Aviso(
                        "Se alcanzó el número máximo EXP-99999999.\n" +
                        "No se pueden generar más números automáticos.",
                        "Numeración automática"
                    );
                }
                else
                {
                    txtN_expediente.Text = siguiente;
                }
            }
            else
            {
                // Permitir ingresar el número manualmente
                txtN_expediente.ReadOnly = false;
                txtN_expediente.Text = "EXP-";
            }
        }


        // =========================================================
        // PREPARAR NUEVO EXPEDIENTE
        // =========================================================

        private void PrepararNuevoExpediente()
        {
            LimpiarFormulario();

            textFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            GenerarNumeroExpediente();

            txtN_expediente.Focus();
        }


        // =========================================================
        // BOTÓN LIMPIEZA
        // =========================================================

        private void But_Limpieza_Click(object sender, EventArgs e)
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
                PrepararNuevoExpediente();
            }
        }


        // =========================================================
        // CANCELAR REGISTRO
        // =========================================================

        private void guna2Button14_Click(object sender, EventArgs e)
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
                AbrirFormulario(new INICIO());
            }
        }


        // =========================================================
        // NUEVO EXPEDIENTE
        // =========================================================

        private void But_nuevo_Click(object sender, EventArgs e)
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
        // VALIDAR TODOS LOS DATOS DEL FORMULARIO
        // (mismas reglas que Modificar Expediente)
        // =========================================================

        private bool ValidarFormulario()
        {
            const string OBLIGATORIO = "Campo obligatorio";

            // 1. CAMPOS VACÍOS
            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(txtN_expediente.Text),
                    "Ingrese el número de expediente.", OBLIGATORIO, txtN_expediente)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textFecha.Text),
                    "Ingrese la fecha.", OBLIGATORIO, textFecha)) return false;

            if (!Funciones.Validar(ComboTipoDocumento.SelectedIndex != -1,
                    "Seleccione el tipo de documento.", OBLIGATORIO, ComboTipoDocumento)) return false;

            // Área de destino: obligatoria
            if (!Funciones.Validar(comb_destino.SelectedIndex != -1,
                    "Seleccione el área de destino.", OBLIGATORIO, comb_destino)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(txtDescripcion.Text),
                    "Ingrese la descripción.", OBLIGATORIO, txtDescripcion)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(texNAME_USUARIO.Text),
                    "Ingrese el nombre del usuario.", OBLIGATORIO, texNAME_USUARIO)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textDNI.Text),
                    "Ingrese el documento de identificación.", OBLIGATORIO, textDNI)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textEMPRESA.Text),
                    "Ingrese la empresa.", OBLIGATORIO, textEMPRESA)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textTELEFONO.Text),
                    "Ingrese el teléfono.", OBLIGATORIO, textTELEFONO)) return false;

            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textCORREO.Text),
                    "Ingrese el correo electrónico.", OBLIGATORIO, textCORREO)) return false;


            // 2. NÚMERO DE EXPEDIENTE: EXP- y exactamente 8 dígitos
            if (!Funciones.Validar(Restricciones.EsCodigoValido(txtN_expediente.Text.Trim()),
                    "El número debe tener el formato EXP-00000001:\n" +
                    "prefijo EXP- y exactamente 8 dígitos.",
                    "Número inválido", txtN_expediente)) return false;

            if (!Funciones.Validar(Funciones.NormalizarNumero(txtN_expediente.Text) != "00000000",
                    "El número debe estar entre EXP-00000001 y EXP-99999999.",
                    "Número inválido", txtN_expediente)) return false;

            // 2b. LARGOS MÁXIMOS DE LOS TEXTOS
            if (!Restricciones.LargoValido(texNAME_USUARIO, Restricciones.MAX_NOMBRE, "Nombre")) return false;
            if (!Restricciones.LargoValido(textEMPRESA, Restricciones.MAX_EMPRESA, "Empresa")) return false;
            if (!Restricciones.LargoValido(textCORREO, Restricciones.MAX_CORREO, "Correo")) return false;
            if (!Restricciones.LargoValido(txtDescripcion, Restricciones.MAX_DESCRIPCION, "Descripción")) return false;

            // 3. FECHA dd/MM/yyyy
            if (!Funciones.Validar(Funciones.EsFechaValida(textFecha.Text),
                    "La fecha debe tener el formato dd/MM/yyyy.",
                    "Fecha incorrecta", textFecha)) return false;

            // 4. NOMBRE: solo letras y espacios
            if (!Funciones.Validar(Funciones.EsNombreValido(texNAME_USUARIO.Text),
                    "El nombre solo debe contener letras y espacios.",
                    "Dato inválido", texNAME_USUARIO)) return false;

            // 5. DNI: 8 dígitos
            if (!Funciones.Validar(Funciones.EsDNIValido(textDNI.Text),
                    "El DNI debe contener exactamente 8 dígitos.",
                    "DNI inválido", textDNI)) return false;

            // 6. TELÉFONO: 9 dígitos y empieza con 9
            if (!Funciones.Validar(Funciones.EsTelefonoValido(textTELEFONO.Text),
                    "El número de celular debe tener exactamente " +
                    "9 dígitos y comenzar con 9.\n\nEjemplo: 987654321.",
                    "Teléfono inválido", textTELEFONO)) return false;

            // 7. CORREO: termina en @gmail.com
            if (!Funciones.Validar(Funciones.EsCorreoValido(textCORREO.Text),
                    "El correo debe terminar en @gmail.com.",
                    "Correo inválido", textCORREO)) return false;

            // 8. PDF OBLIGATORIO
            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(rutaPDF),
                    "Debe adjuntar un documento PDF.",
                    "Archivo obligatorio", null)) return false;

            if (!Funciones.Validar(File.Exists(rutaPDF),
                    "No se encontró el archivo PDF adjuntado.\n" +
                    "Vuelva a adjuntarlo.",
                    "Archivo no encontrado", null)) return false;

            // 9. NÚMERO REPETIDO
            if (!Funciones.Validar(!Funciones.ExisteExpediente(txtN_expediente.Text),
                    "El número de expediente ya existe.\n" +
                    "Ingrese un número diferente.",
                    "Expediente duplicado", txtN_expediente)) return false;

            // 10. PLACA
            if (!Funciones.Validar(!string.IsNullOrWhiteSpace(textPLACA.Text),
                    "Ingrese el número de placa del vehículo.",
                    OBLIGATORIO, textPLACA)) return false;

            if (!Funciones.Validar(Funciones.EsPlacaValida(textPLACA.Text),
                    "La placa debe tener un formato como ABC-123 o A1B-234.",
                    "Placa inválida", textPLACA)) return false;

            return true;
        }


        // =========================================================
        // GUARDAR EXPEDIENTE
        // =========================================================

        private void but_guardar_Click(object sender, EventArgs e)
        {
            // 1. VALIDAR DATOS
            if (!ValidarFormulario())
            {
                return;
            }

            // Número sin el prefijo EXP-
            string numeroExpediente =
                Funciones.NormalizarNumero(txtN_expediente.Text);


            // 2. COPIAR EL PDF A LA CARPETA DEL SISTEMA
            //    (el archivo original del usuario se conserva)
            string pdfGuardado =
                Funciones.CopiarPDFAlSistema(rutaPDF, numeroExpediente);

            if (pdfGuardado == "")
            {
                // No se guarda un expediente sin su PDF
                return;
            }


            // 3. CREAR EL REGISTRO (12 campos + 4 de seguimiento)
            //    LimpiarTexto evita que | o saltos de línea
            //    desordenen el archivo.
            string registro =
                numeroExpediente + "|" +
                Funciones.LimpiarTexto(textFecha.Text) + "|" +
                Funciones.LimpiarTexto(ComboTipoDocumento.Text) + "|" +
                Funciones.LimpiarTexto(txtDescripcion.Text) + "|" +
                Funciones.LimpiarTexto(texNAME_USUARIO.Text) + "|" +
                Funciones.LimpiarTexto(textDNI.Text) + "|" +
                Funciones.LimpiarTexto(textEMPRESA.Text) + "|" +
                Funciones.LimpiarTexto(textTELEFONO.Text) + "|" +
                Funciones.LimpiarTexto(textCORREO.Text) + "|" +
                Funciones.PENDIENTE + "|" +
                pdfGuardado + "|" +
                Funciones.LimpiarTexto(textPLACA.Text).ToUpperInvariant() +

                // Seguimiento (vacío hasta que se atienda):
                // fecha de inicio | resultado | observación | fecha de fin
                "||||" +

                // Área de destino | responsable (vacío hasta iniciar)
                "|" + Funciones.LimpiarTexto(comb_destino.Text) + "|";


            // 4. GUARDAR EN EL ARCHIVO
            if (!Funciones.AgregarExpediente(registro))
            {
                // Si no se pudo guardar, quitar la copia del PDF
                // para no dejar archivos sin expediente
                try
                {
                    File.Delete(pdfGuardado);
                }
                catch (Exception)
                {
                }

                return;
            }

            // Primera acción del historial (fecha, hora y responsable)
            Funciones.RegistrarHistorial(numeroExpediente, Funciones.ACCION_REGISTRO, Funciones.UsuarioActual,
                "Estado: Pendiente. Área de destino: " + Funciones.LimpiarTexto(comb_destino.Text));


            // 5. LIMPIAR SEGÚN CONFIGURACIÓN (opción 1)
            if (Funciones.LeerOpcion(0, false))
            {
                PrepararNuevoExpediente();
            }
            else if (Funciones.LeerOpcion(3, false))
            {
                // Sin limpiar, pero con numeración automática:
                // avanzar al siguiente número para no repetir el anterior
                GenerarNumeroExpediente();
            }


            // 6. MENSAJE DE CONFIRMACIÓN (opción 2)
            if (Funciones.LeerOpcion(1, true))
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
        // NAVEGACIÓN
        // Abre el formulario elegido y cierra el actual para
        // que no se acumulen ventanas abiertas.
        // =========================================================

        private void AbrirFormulario(Form ventana)
        {
            ventana.Show();
            this.Close();
        }

        private void But_Cerrar_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        private void ButtonINICIO_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new INICIO());
        }

        private void But_Consultar_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONSULTAR_EXPEDIENTE());
        }

        private void But_buscar_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new BUSCAR_EXPEDIENTE());
        }

        private void But_gestionar_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new GESTIONAR_EXPEDIENTE());
        }

        private void But_Reportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REPORTES());
        }

        private void But_Configuración_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONFIGURACIÓN());
        }
    }
}
