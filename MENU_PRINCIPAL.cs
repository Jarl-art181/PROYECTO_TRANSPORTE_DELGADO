using Guna.Charts.Interfaces;
using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            MostrarFechaHora();
        }

        private void MostrarFechaHora()
        {
            DateTime ahora = DateTime.Now;

            lblFecha.Text = ahora.ToString("dddd dd 'de' MMMM 'de' yyyy");
            lblhora.Text = ahora.ToString("hh:mm:ss tt");
        }

     

        private void guna2PictureCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonINICIO_Click(object sender, EventArgs e)
        {
            ///MISMA PAGINA
        }

        private void Button_REGISTRO_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana = new REGISTRO_DE_EXPEDIENTE();
            ventana.Show();
            this.Close();
        }

        private void Button_CONSULTAR_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana = new CONSULTAR_EXPEDIENTE();
            ventana.Show(); this.Close();
        }

        private void Button_BUSCAR_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana =new BUSCAR_EXPEDIENTE();
            ventana.Show(); this.Close();
        }

        private void But_Gestionar_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana = new GESTIONAR_EXPEDIENTE();
            ventana.Show(); this.Close();
        }

        private void but_Reportes_Click(object sender, EventArgs e)
        {
            REPORTES ventana = new REPORTES();
            ventana.Show(); this.Close();
        }

        private void but_configuración_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana = new CONFIGURACIÓN();
            ventana.Show(); this.Close();
        }

        private void but_Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
