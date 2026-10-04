using Guna.Charts.Interfaces;
using Guna.Charts.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class INICIO : Form
    {
        public INICIO()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            MostrarFechaHora();
        }

        // ==========================================
        // FECHA Y HORA
        // ==========================================

        private void Timer_Tick(object sender, EventArgs e)
        {
            MostrarFechaHora();
        }

        private void MostrarFechaHora()
        {
            DateTime ahora = DateTime.Now;

            lblFecha.Text =
                ahora.ToString("dddd dd 'de' MMMM 'de' yyyy");

            lblhora.Text =
                ahora.ToString("hh:mm:ss tt");
        }

        // ==========================================
        // NAVEGACIÓN
        // ==========================================

        private void guna2PictureCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonINICIO_Click(object sender, EventArgs e)
        {
            // Ya estamos en Inicio
        }

        private void Button_REGISTRO_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana =
                new REGISTRO_DE_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        private void Button_CONSULTAR_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana =
                new CONSULTAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        private void Button_BUSCAR_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =
                new BUSCAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        private void But_Gestionar_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana =
                new GESTIONAR_EXPEDIENTE();

            ventana.Show();
            this.Close();
        }

        private void but_Reportes_Click(object sender, EventArgs e)
        {
            REPORTES ventana =
                new REPORTES();

            ventana.Show();
            this.Close();
        }

        private void but_configuración_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana =
                new CONFIGURACIÓN();

            ventana.Show();
            this.Close();
        }

        private void but_Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ==========================================
        // CARGA INICIAL
        // ==========================================

        private void INICIO_Load(object sender, EventArgs e)
        {
            // ==========================================
            // CONTADORES DE EXPEDIENTES
            // ==========================================

            n_ex_registrado.Text =
                ObtenerExpedientes().Length.ToString();

            n_ex_proceso.Text =
                ObtenerEnProceso().ToString();

            n_ex_pendiente.Text =
                ObtenerPendientes().ToString();

            n_ex_finalizado.Text =
                ObtenerFinalizados().ToString();

            // ==========================================
            // DATOS DE LA EMPRESA
            // ==========================================

            CargarDatosEmpresa();

            // ==========================================
            // MENSAJE DE BIENVENIDA
            // ==========================================

            MostrarMensajeBienvenida();
        }

        // ==========================================
        // OBTENER TODOS LOS EXPEDIENTES
        // ==========================================

        private string[] ObtenerExpedientes()
        {
            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(rutaArchivo))
            {
                return new string[0];
            }

            return File.ReadAllLines(rutaArchivo);
        }

        // ==========================================
        // CONTAR EXPEDIENTES EN PROCESO
        // ==========================================

        private int ObtenerEnProceso()
        {
            string[] registros = ObtenerExpedientes();

            int contador = 0;

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (datos.Length > 9 &&
                    datos[9].Trim() == "En proceso")
                {
                    contador++;
                }
            }

            return contador;
        }

        // ==========================================
        // CONTAR EXPEDIENTES PENDIENTES
        // ==========================================

        private int ObtenerPendientes()
        {
            string[] registros = ObtenerExpedientes();

            int contador = 0;

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (datos.Length > 9 &&
                    datos[9].Trim() == "Pendiente")
                {
                    contador++;
                }
            }

            return contador;
        }

        // ==========================================
        // CONTAR EXPEDIENTES FINALIZADOS
        // ==========================================

        private int ObtenerFinalizados()
        {
            string[] registros = ObtenerExpedientes();

            int contador = 0;

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (datos.Length > 9 &&
                    datos[9].Trim() == "Finalizado")
                {
                    contador++;
                }
            }

            return contador;
        }

        // ==========================================
        // CARGAR DATOS DE LA EMPRESA
        // ==========================================

        private void CargarDatosEmpresa()
        {
            string ruta = Path.Combine(
                Application.StartupPath,
                "Datos",
                "configuracion.txt"
            );

            if (!File.Exists(ruta))
            {
                return;
            }

            string contenido = File.ReadAllText(ruta);

            string[] datos = contenido.Split('|');

            if (datos.Length >= 6)
            {
                lab_razonsocial.Text = datos[0];
                lab_ruc.Text = datos[1];
                lab_direccion.Text = datos[2];
                lab_telefono.Text = datos[3];
                labcorreo.Text = datos[4];
                lab_horario.Text = datos[5];
            }
        }

        // ==========================================
        // MENSAJE DE BIENVENIDA
        // ==========================================

        private void MostrarMensajeBienvenida()
        {
            string rutaOpciones = Path.Combine(
                Application.StartupPath,
                "Datos",
                "opciones.txt"
            );

            // Si no existe el archivo, no mostrar bienvenida
            if (!File.Exists(rutaOpciones))
            {
                return;
            }

            // Leer las 6 opciones
            string contenido = File.ReadAllText(rutaOpciones);

            string[] opciones = contenido.Split('|');

            // ==========================================
            // OPCIÓN 6
            // ÍNDICE 5 = MOSTRAR BIENVENIDA
            // ==========================================

            if (opciones.Length >= 6)
            {
                bool mostrarBienvenida;

                bool.TryParse(
                    opciones[5].Trim(),
                    out mostrarBienvenida
                );

                // Si está desactivado, no mostrar nada
                if (!mostrarBienvenida)
                {
                    return;
                }

                // Si está activado, mostrar bienvenida
                MessageBox.Show(
                    "¡Bienvenido al Sistema de Gestión Documentaria!\n\n" +
                    "TRANSPORTE DELGADO UCEDA SAC\n\n" +
                    "Gracias por utilizar nuestro sistema.\n" +
                    "Ahora puede gestionar sus expedientes " +
                    "de manera rápida, segura y organizada.",
                    "Bienvenido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}