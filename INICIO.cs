using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class INICIO : Form
    {
        // Temporizador del reloj. Se guarda como variable
        // del formulario para poder detenerlo al cerrar.
        private Timer timer = new Timer();

        public INICIO()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Ajustar el diseño al tamaño y escalado de la pantalla
            Escalado.AjustarPantallaCompleta(this);

            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Detener el reloj cuando se cierre el formulario
            this.FormClosed += INICIO_FormClosed;

            MostrarFechaHora();
        }

        // ==========================================
        // DETENER EL TEMPORIZADOR AL CERRAR
        // ==========================================

        private void INICIO_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            timer.Dispose();
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
        // Abre el formulario elegido y cierra el actual
        // ==========================================

        private void AbrirFormulario(Form ventana)
        {
            ventana.Show();
            this.Close();
        }

        private void guna2PictureCerrar_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        private void ButtonINICIO_Click(object sender, EventArgs e)
        {
            // Ya estamos en Inicio
        }

        private void Button_REGISTRO_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REGISTRO_DE_EXPEDIENTE());
        }

        private void Button_CONSULTAR_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONSULTAR_EXPEDIENTE());
        }

        private void Button_BUSCAR_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new BUSCAR_EXPEDIENTE());
        }

        private void But_Gestionar_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new GESTIONAR_EXPEDIENTE());
        }

        private void but_Reportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new REPORTES());
        }

        private void but_configuración_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new CONFIGURACIÓN());
        }

        private void but_Cerrar_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de Iniciar sesión
            Funciones.CerrarSesion(this);
        }

        // ==========================================
        // CARGA INICIAL
        // ==========================================

        private void INICIO_Load(object sender, EventArgs e)
        {
            // CONTADORES DE EXPEDIENTES
            // (el archivo se lee una sola vez)
            ContarExpedientes();

            // DATOS DE LA EMPRESA
            CargarDatosEmpresa();

            // MENSAJE DE BIENVENIDA
            MostrarMensajeBienvenida();
        }

        // ==========================================
        // CONTAR EXPEDIENTES POR ESTADO
        // ==========================================

        private void ContarExpedientes()
        {
            // Solo expedientes válidos (sin líneas vacías)
            string[] registros = Funciones.LeerExpedientesValidos();

            int proceso = 0;
            int pendiente = 0;
            int finalizado = 0;

            for (int i = 0; i < registros.Length; i++)
            {
                string estado = registros[i].Split('|')[9].Trim();

                if (estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase))
                {
                    proceso++;
                }
                else if (estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    pendiente++;
                }
                else if (estado.Equals("Finalizado", StringComparison.OrdinalIgnoreCase))
                {
                    finalizado++;
                }
            }

            n_ex_registrado.Text = registros.Length.ToString();
            n_ex_proceso.Text = proceso.ToString();
            n_ex_pendiente.Text = pendiente.ToString();
            n_ex_finalizado.Text = finalizado.ToString();
        }

        // ==========================================
        // CARGAR DATOS DE LA EMPRESA
        // ==========================================

        private void CargarDatosEmpresa()
        {
            try
            {
                string ruta = Funciones.RutaConfiguracion();

                if (!File.Exists(ruta))
                {
                    return;
                }

                string[] datos = File.ReadAllText(ruta).Split('|');

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
            catch (Exception)
            {
                // Si no se puede leer, se mantienen los datos del diseño
            }
        }

        // ==========================================
        // MENSAJE DE BIENVENIDA (opción 6)
        // ==========================================

        private void MostrarMensajeBienvenida()
        {
            // Si está desactivado o no existe la opción, no mostrar nada
            if (!Funciones.LeerOpcion(5, false))
            {
                return;
            }

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
