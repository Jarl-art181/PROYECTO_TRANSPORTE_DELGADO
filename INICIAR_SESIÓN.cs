using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class INICIAR_SESIÓN : Form
    {
        public INICIAR_SESIÓN()
        {
            InitializeComponent();
           this.WindowState = FormWindowState.Maximized;
          this.FormBorderStyle = FormBorderStyle.None;
           this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.SendToBack();
            timer1.Start();
            ActualizarFechaHora();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ActualizarFechaHora();
        }

        private void ActualizarFechaHora()
        {
            lblFecha.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");
            lblhora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

   
        

       

        private void But_Iniciar_sesion_Click(object sender, EventArgs e)
        {
            INICIO ventana = new INICIO();
            ventana.Show();
        }

        private void But_Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}