using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class CONFIGURACIÓN : Form
    {
        // ==========================================
        // RUTAS DE LOS ARCHIVOS
        // ==========================================

        // Archivo de datos de la empresa
        private string rutaConfiguracion;

        // Archivo de opciones del sistema
        private string rutaOpciones;

        // Archivo de ruta de almacenamiento de PDF
        private string rutaPDF;


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public CONFIGURACIÓN()
        {
            InitializeComponent();

            // Configuración de la ventana
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Ruta de configuracion.txt
            rutaConfiguracion = Path.Combine(
                Application.StartupPath,
                "Datos",
                "configuracion.txt"
            );

            // Ruta de opciones.txt
            rutaOpciones = Path.Combine(
                Application.StartupPath,
                "Datos",
                "opciones.txt"
            );

            // Ruta de rutaPDF.txt
            rutaPDF = Path.Combine(
                Application.StartupPath,
                "Datos",
                "rutaPDF.txt"
            );
        }


        // ==========================================
        // GUARDAR OPCIONES DEL SISTEMA
        // ==========================================

        private void GuardarOpciones()
        {
            // Ruta de la carpeta Datos
            string carpetaDatos = Path.Combine(
                Application.StartupPath,
                "Datos"
            );

            // Crear carpeta si no existe
            if (!Directory.Exists(carpetaDatos))
            {
                Directory.CreateDirectory(carpetaDatos);
            }

            // Guardar el estado de las 6 opciones
            string opciones =
                but_limpiarformulario.Checked + "|" +
                but_mostarmesajeguardar.Checked + "|" +
                but_cargardatosautomatico.Checked + "|" +
                but_generarnumeros_automatico.Checked + "|" +
                but_mostrarconfirmacion.Checked + "|" +
                but_modooscuro.Checked;

            // Guardar en opciones.txt
            File.WriteAllText(
                rutaOpciones,
                opciones
            );
        }


        // ==========================================
        // CARGAR OPCIONES DEL SISTEMA
        // ==========================================

        private void CargarOpciones()
        {
            // Verificar que exista opciones.txt
            if (!File.Exists(rutaOpciones))
            {
                return;
            }

            // Leer archivo
            string contenido = File.ReadAllText(rutaOpciones);

            // Separar los datos mediante |
            string[] opciones = contenido.Split('|');

            // Verificar que existan las 6 opciones
            if (opciones.Length >= 6)
            {
                bool.TryParse(
                    opciones[0].Trim(),
                    out bool limpiar
                );

                bool.TryParse(
                    opciones[1].Trim(),
                    out bool mensaje
                );

                bool.TryParse(
                    opciones[2].Trim(),
                    out bool cargar
                );

                bool.TryParse(
                    opciones[3].Trim(),
                    out bool numeracion
                );

                bool.TryParse(
                    opciones[4].Trim(),
                    out bool confirmar
                );

                bool.TryParse(
                    opciones[5].Trim(),
                    out bool bienvenida
                );

                // Colocar los valores en los CheckBox
                but_limpiarformulario.Checked = limpiar;

                but_mostarmesajeguardar.Checked = mensaje;

                but_cargardatosautomatico.Checked = cargar;

                but_generarnumeros_automatico.Checked = numeracion;

                but_mostrarconfirmacion.Checked = confirmar;

                // Sexta opción:
                // Mostrar bienvenida al abrir el sistema
                but_modooscuro.Checked = bienvenida;
            }
        }


        // ==========================================
        // CARGAR CONFIGURACIÓN DE LA EMPRESA
        // ==========================================

        private void CargarConfiguracion()
        {
            // Verificar que exista configuracion.txt
            if (!File.Exists(rutaConfiguracion))
            {
                return;
            }

            // Leer información
            string contenido = File.ReadAllText(
                rutaConfiguracion
            );

            // Separar los datos
            string[] datos = contenido.Split('|');

            // Verificar que existan los 6 datos
            if (datos.Length >= 6)
            {
                tex_razonsocial.Text = datos[0];

                tex_ruc.Text = datos[1];

                tex_direccion.Text = datos[2];

                tex_telefono.Text = datos[3];

                tex_correo.Text = datos[4];

                tex_horario.Text = datos[5];
            }
        }


        // ==========================================
        // CARGAR RUTA DE ALMACENAMIENTO DE PDF
        // ==========================================

        private void CargarRutaPDF()
        {
            // Si existe rutaPDF.txt
            if (File.Exists(rutaPDF))
            {
                string carpetaGuardada =
                    File.ReadAllText(rutaPDF).Trim();

                // Verificar que no esté vacía
                if (!string.IsNullOrWhiteSpace(carpetaGuardada))
                {
                    txtCarpetaPDF.Text = carpetaGuardada;
                    return;
                }
            }

            // ==========================================
            // CARPETA PREDETERMINADA
            // ==========================================

            string carpetaPredeterminada = Path.Combine(
                Application.StartupPath,
                "Datos",
                "PDF"
            );

            // Crear carpeta si no existe
            if (!Directory.Exists(carpetaPredeterminada))
            {
                Directory.CreateDirectory(
                    carpetaPredeterminada
                );
            }

            txtCarpetaPDF.Text = carpetaPredeterminada;
        }


        // ==========================================
        // GUARDAR RUTA DE ALMACENAMIENTO DE PDF
        // ==========================================

        private void GuardarRutaPDF()
        {
            string carpeta = txtCarpetaPDF.Text.Trim();

            // Verificar que exista una ruta
            if (string.IsNullOrWhiteSpace(carpeta))
            {
                return;
            }

            // Crear la carpeta si no existe
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Guardar la ruta
            File.WriteAllText(
                rutaPDF,
                carpeta
            );
        }


        // ==========================================
        // BOTÓN CERRAR
        // ==========================================

        private void guna2Button1_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }


        // ==========================================
        // PICTUREBOX
        // ==========================================

        private void pictureBox5_Click(
            object sender,
            EventArgs e)
        {

        }


        // ==========================================
        // BOTÓN GESTIONAR
        // ==========================================

        private void guna2Button6_Click(
            object sender,
            EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // BOTÓN REGISTRO
        // ==========================================

        private void guna2Button3_Click(
            object sender,
            EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // BOTÓN INICIO
        // ==========================================

        private void guna2Button2_Click(
            object sender,
            EventArgs e)
        {
            INICIO ventana =
                new INICIO();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // BOTÓN CONSULTAR
        // ==========================================

        private void guna2Button4_Click(
            object sender,
            EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // BOTÓN BUSCAR
        // ==========================================

        private void guna2Button5_Click(
            object sender,
            EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // BOTÓN REPORTES
        // ==========================================

        private void guna2Button7_Click(
            object sender,
            EventArgs e)
        {
            REPORTES ventana =
                new REPORTES();

            ventana.Show();

            this.Close();
        }


        // ==========================================
        // CARGA DEL FORMULARIO
        // ==========================================

        private void CONFIGURACIÓN_Load(
            object sender,
            EventArgs e)
        {
            // Cargar datos de la empresa
            CargarConfiguracion();

            // Cargar las 6 opciones
            CargarOpciones();

            // Cargar carpeta de almacenamiento de PDF
            CargarRutaPDF();
        }


        // ==========================================
        // BOTÓN GUARDAR CONFIGURACIÓN
        // ==========================================

        private void but_guardar_Click(
            object sender,
            EventArgs e)
        {
            // ======================================
            // CREAR CARPETA DATOS
            // ======================================

            string carpetaDatos = Path.Combine(
                Application.StartupPath,
                "Datos"
            );

            if (!Directory.Exists(carpetaDatos))
            {
                Directory.CreateDirectory(carpetaDatos);
            }


            // ======================================
            // OBTENER DATOS DE LA EMPRESA
            // ======================================

            string razonSocial =
                tex_razonsocial.Text.Trim();

            string ruc =
                tex_ruc.Text.Trim();

            string direccion =
                tex_direccion.Text.Trim();

            string telefono =
                tex_telefono.Text.Trim();

            string correo =
                tex_correo.Text.Trim();

            string horario =
                tex_horario.Text.Trim();


            // ======================================
            // UNIR DATOS DE LA EMPRESA
            // ======================================

            string configuracion =
                razonSocial + "|" +
                ruc + "|" +
                direccion + "|" +
                telefono + "|" +
                correo + "|" +
                horario;


            // ======================================
            // GUARDAR CONFIGURACIÓN DE EMPRESA
            // ======================================

            File.WriteAllText(
                rutaConfiguracion,
                configuracion
            );


            // ======================================
            // GUARDAR LAS 6 OPCIONES
            // ======================================

            GuardarOpciones();


            // ======================================
            // GUARDAR RUTA DE PDF
            // ======================================

            GuardarRutaPDF();


            // ======================================
            // MENSAJE
            // ======================================

            MessageBox.Show(
                "La configuración se guardó correctamente.",
                "Configuración",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // ==========================================
        // BOTÓN CAMBIAR CARPETA
        // ==========================================

        private void but_cambiarcarpeta_Click(
            object sender,
            EventArgs e)
        {
            using (FolderBrowserDialog carpeta =
                new FolderBrowserDialog())
            {
                carpeta.Description =
                    "Seleccione la carpeta donde se almacenarán los archivos PDF.";

                // Si ya existe una ruta seleccionada,
                // mostrarla como punto inicial
                if (Directory.Exists(txtCarpetaPDF.Text.Trim()))
                {
                    carpeta.SelectedPath =
                        txtCarpetaPDF.Text.Trim();
                }

                if (carpeta.ShowDialog() ==
                    DialogResult.OK)
                {
                    // Solamente colocar la ruta
                    // en el TextBox.
                    // Se guardará cuando se presione
                    // "Guardar Configuración".
                    txtCarpetaPDF.Text =
                        carpeta.SelectedPath;
                }
            }
        }

      



        // ==========================================
        // BOTÓN RESTAURAR
        // ==========================================
  private void But_restaurar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
       "¿Está seguro de que desea restaurar toda la configuración?\n\n" +
       "Se restaurarán los datos de la empresa, " +
       "las opciones del sistema y la carpeta de almacenamiento de PDF.",
       "Restaurar configuración",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question
   );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            // ==========================================
            // DATOS PREDETERMINADOS DE LA EMPRESA
            // ==========================================

            tex_razonsocial.Text =
                "TRANSPORTE DELGADO UCEDA SAC";

            tex_ruc.Text =
                "20601234567";

            tex_direccion.Text =
                "Av. Principal 123 - Trujillo, La Libertad";

            tex_telefono.Text =
                "+51 994418578 / +51 929645774";

            tex_correo.Text =
                "Transportedelgado@gmail.com";

            tex_horario.Text =
                "Lunes a sábado / 8:00 - 17:00";


            // ==========================================
            // RESTAURAR LAS 6 OPCIONES
            // ==========================================

            but_limpiarformulario.Checked = true;

            but_mostarmesajeguardar.Checked = true;

            but_cargardatosautomatico.Checked = true;

            but_generarnumeros_automatico.Checked = true;

            but_mostrarconfirmacion.Checked = false;

            // Sexta opción:
            // Mostrar bienvenida al abrir el sistema
            but_modooscuro.Checked = false;


            // ==========================================
            // RESTAURAR CARPETA DE PDF
            // ==========================================

            string carpetaPDF = Path.Combine(
                Application.StartupPath,
                "Datos",
                "PDF"
            );

            // Crear carpeta si no existe
            if (!Directory.Exists(carpetaPDF))
            {
                Directory.CreateDirectory(carpetaPDF);
            }

            txtCarpetaPDF.Text = carpetaPDF;


            // ==========================================
            // GUARDAR TODO
            // ==========================================

            // Guardar datos de empresa
            string configuracion =
                tex_razonsocial.Text.Trim() + "|" +
                tex_ruc.Text.Trim() + "|" +
                tex_direccion.Text.Trim() + "|" +
                tex_telefono.Text.Trim() + "|" +
                tex_correo.Text.Trim() + "|" +
                tex_horario.Text.Trim();

            File.WriteAllText(
                rutaConfiguracion,
                configuracion
            );


            // Guardar las 6 opciones
            GuardarOpciones();


            // Guardar ruta PDF
            GuardarRutaPDF();


            // ==========================================
            // MENSAJE FINAL
            // ==========================================

            MessageBox.Show(
                "La configuración se restauró correctamente.",
                "Configuración restaurada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


    }
}