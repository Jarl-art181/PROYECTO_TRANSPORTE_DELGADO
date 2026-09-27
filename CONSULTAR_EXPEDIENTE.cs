using System;
using System.IO;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    public partial class CONSULTAR_EXPEDIENTE : Form
    {
        public CONSULTAR_EXPEDIENTE()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
        }


        // CARGAR EXPEDIENTES AL ABRIR LA VENTANA

        private void MENU_PRINCIPAL_Load(object sender, EventArgs e)
        {
            CargarDatosExpedientes();
        }


        // CARGAR DATOS DE LOS EXPEDIENTES

        private void CargarDatosExpedientes()
        
         
        {
            // 1. UBICAR EL ARCHIVO

            string archivoExpedientes =
                Path.Combine(
                    Application.StartupPath,
                    "Datos",
                    "expedientes.txt"
                );


            // 2. VERIFICAR SI EXISTE EL ARCHIVO

            if (!File.Exists(archivoExpedientes))
            {
                return;
            }


            // 3. LIMPIAR LA TABLA

            dgvExpedientes.Rows.Clear();


            // 4. LEER TODOS LOS REGISTROS

            string[] registros =
                File.ReadAllLines(archivoExpedientes);

        //===================MATRIZ=======================
            // 5. CREAR LA MATRIZ
            // Filas = cantidad de expedientes
            // Columnas = 9 datos que mostraremos

            string[,] matriz = new string[registros.Length, 10];


            // 6. PASAR LOS DATOS DEL ARCHIVO
            //    A LA MATRIZ

            for (int i = 0;i < registros.Length;  i++)
            {
               


                // Separar los datos del expediente
                string[] datos =
                    registros[i].Split('|');


                // Verificar que existan los datos
                if (datos.Length < 10)
                {
                    continue;
                }


                // GUARDAR LOS 9 DATOS EN LA MATRIZ

                matriz[i, 0] = datos[0]; // Número de expediente
                matriz[i, 1] = datos[1]; // Fecha
                matriz[i, 2] = datos[2]; // Tipo de documento
                matriz[i,3]= datos[3];// descripción
                matriz[i, 4] = datos[4]; // Solicitante
                matriz[i, 5] = datos[5]; // DNI
                matriz[i, 6] = datos[6]; // Empresa
                matriz[i, 7] = datos[7]; // Teléfono
                matriz[i, 8] = datos[8]; // Correo
                matriz[i, 9] = datos[9]; // Estado
            }


            // 7. MOSTRAR LA MATRIZ EN LA TABLA

            for (int i = 0;i < matriz.GetLength(0); i++)
            {
                // Si no existe número de expediente,
                // no mostramos esa fila

                if (string.IsNullOrWhiteSpace(matriz[i, 0]))
                {
                    continue;
                }


                // Crear una nueva fila
                int fila =dgvExpedientes.Rows.Add();


                // COLUMNA 0 → N.º EXPEDIENTE

                dgvExpedientes.Rows[fila]
                    .Cells["colExpediente"]
                    .Value = matriz[i, 0];


                // COLUMNA 1 → FECHA

                dgvExpedientes.Rows[fila]
                    .Cells["colFechaRegistro"]
                    .Value = matriz[i, 1];


                // COLUMNA 2 → TIPO DE DOCUMENTO

                dgvExpedientes.Rows[fila]
                    .Cells["colDocumento"]
                    .Value = matriz[i, 2];

                // COLUMNA 3 → DESCRIPCIÓN

                dgvExpedientes.Rows[fila]
                    .Cells["ColuDescripción"]
                    .Value = matriz[i, 3];


                // COLUMNA 4 → SOLICITANTE

                dgvExpedientes.Rows[fila]
                    .Cells["coloSolicitante"]
                    .Value = matriz[i, 4];



                // COLUMNA 4 → DNI

                dgvExpedientes.Rows[fila]
                    .Cells["colDNI"]
                    .Value = matriz[i, 5];


                // COLUMNA 5 → EMPRESA

                dgvExpedientes.Rows[fila]
                    .Cells["coloEmpresa"]
                    .Value = matriz[i, 6];


                // COLUMNA 6 → TELÉFONO

                dgvExpedientes.Rows[fila]
                    .Cells["colTelefono"]
                    .Value = matriz[i, 7];


                // COLUMNA 7 → CORREO

                dgvExpedientes.Rows[fila]
                    .Cells["colCorreo"]
                    .Value = matriz[i, 8];


                // COLUMNA 8 → ESTADO

                dgvExpedientes.Rows[fila]
                    .Cells["coluEstadoExpediente"]
                    .Value = matriz[i, 9];
            }


            // 8. QUITAR SELECCIÓN

            dgvExpedientes.ClearSelection();

            dgvExpedientes.Refresh();
        }
        


        // BOTÓN CERRAR SESIÓN 
        private void but_Cerrar_Click(object sender, EventArgs e)
        {
         Application.Exit();
        }



        // BOTÓN INICIO
        private void but_inicio_Click_1(object sender, EventArgs e)
        {
         INICIO ventana = new INICIO(); ventana.Show();

            this.Close();
        }

        private void but_consultar_Click(object sender, EventArgs e)
        {
            CONSULTAR_EXPEDIENTE ventana = new CONSULTAR_EXPEDIENTE(); 
            ventana.Show();
             this.Close();

        }

        private void but_buscar_Click(object sender, EventArgs e)
        {
            BUSCAR_EXPEDIENTE ventana = new BUSCAR_EXPEDIENTE(); ventana.Show();

            this.Close();
        }

        private void but_gestionar_Click(object sender, EventArgs e)
        {
            GESTIONAR_EXPEDIENTE ventana = new GESTIONAR_EXPEDIENTE(); ventana.Show();

            this.Close();
        }

        private void but_reportes_Click(object sender, EventArgs e)
        {
            REPORTES ventana = new REPORTES(); ventana.Show();

            this.Close();
        }

        private void but_configuración_Click(object sender, EventArgs e)
        {
            CONFIGURACIÓN ventana = new CONFIGURACIÓN(); ventana.Show();

            this.Close();
        }

        private void but_registro_Click(object sender, EventArgs e)
        {
            REGISTRO_DE_EXPEDIENTE ventana = new REGISTRO_DE_EXPEDIENTE(); ventana.Show();

            this.Close();
        }
    }
}