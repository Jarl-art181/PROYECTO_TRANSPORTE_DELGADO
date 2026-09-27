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
    public partial class GESTIONAR_EXPEDIENTE : Form
    {
        public GESTIONAR_EXPEDIENTE()
        {
            InitializeComponent();
            InitializeComponent();
          this.WindowState = FormWindowState.Maximized;
           this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
        }

        private void MENU_PRINCIPAL_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            INICIO ventana = new INICIO();
            ventana.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana = new REGISTRO_DE_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana = new CONSULTAR_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana = new BUSCAR_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            REPORTES ventana = new REPORTES();
            ventana.Show();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana = new CONFIGURACIÓN();
            ventana.Show();
        }
    }
}
