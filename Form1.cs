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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           // this.WindowState = FormWindowState.Maximized;
          //  this.FormBorderStyle = FormBorderStyle.None;
           // this.DoubleBuffered = true;
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

   
        

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            MENU_PRINCIPAL ventana = new MENU_PRINCIPAL();
            ventana.Show();
        }
    }
}