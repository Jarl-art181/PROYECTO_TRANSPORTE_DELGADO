using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class BUSCAR_EXPEDIENTE : Form
    {
        private string rutaPDFSeleccionado = "";

        public BUSCAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Permitir escribir y también seleccionar de la lista
            combo_busquedadeexpediente.DropDownStyle = ComboBoxStyle.DropDown;
        }
        


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana = new GESTIONAR_EXPEDIENTE();
            ventana.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana = new GESTIONAR_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana = new REGISTRO_DE_EXPEDIENTE();
            ventana.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            INICIO ventana = new INICIO();
            ventana.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana = new CONSULTAR_EXPEDIENTE();
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



        private void CargarExpedientes()
        {
            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            combo_busquedadeexpediente.Items.Clear();

            string[] registros = File.ReadAllLines(rutaArchivo);

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                    continue;

                string[] datos = registros[i].Split('|');

                if (datos.Length > 0)
                {
                    string expediente = "EXP-" + datos[0];

                    combo_busquedadeexpediente.Items.Add(expediente);
                }
            }
        }
        private void But_Buscar_Click(object sender, EventArgs e)
        {
            // VALIDAR QUE NO ESTÉ VACÍO
            if (string.IsNullOrWhiteSpace(combo_busquedadeexpediente.Text))
            {
                MessageBox.Show(
                    "Ingrese el número de expediente.",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                combo_busquedadeexpediente.Focus();
                return;
            }

            // VALIDAR FORMATO EXP-12345678
            if (!System.Text.RegularExpressions.Regex.IsMatch(
                combo_busquedadeexpediente.Text.Trim(),
                @"^EXP-\d{8}$"))
            {
                MessageBox.Show(
                    "El número de expediente debe tener el formato EXP-12345678.",
                    "Número de expediente inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                combo_busquedadeexpediente.Focus();
                return;
            }

            // OBTENER SOLO LOS 8 NÚMEROS
            string numeroBuscado =
                combo_busquedadeexpediente.Text.Trim().Substring(4);

            // RUTA DEL ARCHIVO
            string rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            // VERIFICAR ARCHIVO
            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    "No existe el archivo de expedientes.",
                    "Archivo no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // LEER REGISTROS
            string[] registros = File.ReadAllLines(rutaArchivo);

            bool encontrado = false;

            // RECORRER LOS REGISTROS
            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                    continue;

                string[] datos = registros[i].Split('|');

                if (datos.Length < 10)
                    continue;

                // COMPARAR EXPEDIENTE
                if (datos[0] == numeroBuscado)
                {
                    encontrado = true;

                    // AQUÍ MOSTRAREMOS LA INFORMACIÓN
                    MessageBox.Show(
                        "Expediente encontrado.",
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );


                    // DOCUMENTO PDF
                  

                    if (datos.Length > 10 && datos[10] != "Sin PDF")
                    {
                        string rutaPDF = datos[10];

                        rutaPDFSeleccionado = rutaPDF;

                        tex_nombrepdf_des.Text = Path.GetFileName(rutaPDF);

                        FileInfo archivoPDF = new FileInfo(rutaPDF);

                        double pesoMB =
                            archivoPDF.Length / (1024.0 * 1024.0);

                        tex_pesoPDF_des.Text =
                            pesoMB.ToString("0.0") + " MB";
                    }
                    else
                    {
                        rutaPDFSeleccionado = "";

                        tex_nombrepdf_des.Text = "Sin PDF";
                        tex_pesoPDF_des.Text = "";
                    }
                    break;


                }
            }

            // SI NO EXISTE
            if (!encontrado)
            {
                MessageBox.Show(
                    "No se encontró el expediente.",
                    "Resultado de búsqueda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }


            if (combo_busquedadeexpediente.SelectedIndex == -1)
                return;

            string expedienteSeleccionado =
                combo_busquedadeexpediente.SelectedItem.ToString();

            string numeroExpediente =
                expedienteSeleccionado.Substring(4);

             rutaArchivo = Path.Combine(
                Application.StartupPath,
                "Datos",
                "expedientes.txt"
            );

            if (!File.Exists(rutaArchivo))
                return;

            registros = File.ReadAllLines(rutaArchivo);

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                    continue;

                string[] datos = registros[i].Split('|');

                if (datos.Length < 10)
                    continue;

                if (datos[0] == numeroExpediente)
                {
                    lab_Numero_expediente.Text = "EXP-" + datos[0];
                    text_num_expe_des.Text = "EXP-" + datos[0];
                    labe_fecha_des.Text = datos[1];
                    lab_tipodedocumento_des.Text = datos[2];
                    tex_des_des.Text = datos[3];
                    tex_nombreusuario_des.Text = datos[4];
                    tex_dni_des.Text = datos[5];
                    tex_empresa_des.Text = datos[6];
                    tex_telefono_des.Text = datos[7];
                    tex_correo_des.Text = datos[8];
                    // Mostrar estado
                    text_estado.Text = datos[9];
                    tex_estado_des.Text = datos[9];

                    // Cambiar color según el estado
                    if (datos[9] == "En proceso")
                    {
                        text_estado.FillColor = Color.FromArgb(102, 239, 203);
                        tex_estado_des.FillColor = Color.FromArgb(102, 239, 203);
                    }
                    else if (datos[9] == "Pendiente")
                    {
                        text_estado.FillColor = Color.Gold;
                        tex_estado_des.FillColor = Color.Gold;
                    }
                    else if (datos[9] == "Finalizado")
                    {
                        text_estado.FillColor = Color.Salmon;
                        tex_estado_des.FillColor = Color.Salmon;
                    }

                    break;

                }
            }
        }
    
        

        private void But_Limpiar_Click(object sender, EventArgs e)
        {
            combo_busquedadeexpediente.SelectedIndex = -1;
            combo_busquedadeexpediente.Text = "";
            // Limpiar búsqueda
            combo_busquedadeexpediente.SelectedIndex = -1;
            combo_busquedadeexpediente.Text = "";

            // Limpiar información del expediente
            lab_Numero_expediente.Text = "";
            labe_fecha_des.Text = "";
            lab_tipodedocumento_des.Text = "";

            tex_nombreusuario_des.Text = "";
            tex_dni_des.Text = "";
            tex_empresa_des.Text = "";
            tex_telefono_des.Text = "";
            tex_correo_des.Text = "";

            tex_des_des.Text = "";

            // Limpiar estado
            text_estado.Text = "";

            // Limpiar PDF
            tex_nombrepdf_des.Text = "";
            tex_pesoPDF_des.Text = "";

            // Borrar ruta del PDF seleccionado
            rutaPDFSeleccionado = "";
        }

        private void BUSCAR_EXPEDIENTE_Load(object sender, EventArgs e)
        {
            CargarExpedientes();
        }

        private void But_verpdf_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (!File.Exists(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = rutaPDFSeleccionado,
                UseShellExecute = true
            });
        }

        private void But_descargarpdf_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (!File.Exists(rutaPDFSeleccionado))
            {
                MessageBox.Show(
                    "No se encontró el archivo PDF.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            SaveFileDialog guardar = new SaveFileDialog();

            guardar.Filter = "Archivo PDF (*.pdf)|*.pdf";
            guardar.FileName = Path.GetFileName(rutaPDFSeleccionado);

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                File.Copy(
                    rutaPDFSeleccionado,
                    guardar.FileName,
                    true
                );

                MessageBox.Show(
                    "PDF descargado correctamente.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}