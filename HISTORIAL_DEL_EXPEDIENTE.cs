using System;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    // =========================================================
    // HISTORIAL DEL EXPEDIENTE
    // ---------------------------------------------------------
    // Se abre desde Consultar ("Ver detalle o Historial").
    // Muestra los datos principales del expediente y cada
    // acción registrada: fecha, hora, acción, responsable y
    // observación (historial.txt). Es solo de consulta.
    // =========================================================

    public partial class HISTORIAL_DEL_EXPEDIENTE : Form
    {
        // Código del expediente (8 dígitos)
        private string numeroExpediente = "";


        // Usado por el diseñador de Visual Studio
        public HISTORIAL_DEL_EXPEDIENTE()
        {
            InitializeComponent();
        }

        // Usado por Consultar: recibe el código del expediente
        public HISTORIAL_DEL_EXPEDIENTE(string numero) : this()
        {
            numeroExpediente = Funciones.NormalizarNumero(numero);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Reducir la ventana si no cabe en la pantalla
            Escalado.AjustarDialogo(this);

            this.Load += HISTORIAL_DEL_EXPEDIENTE_Load;
        }

        private void HISTORIAL_DEL_EXPEDIENTE_Load(object sender, EventArgs e)
        {
            if (!CargarHistorial())
            {
                Funciones.Aviso(
                    "No se encontró el expediente EXP-" + numeroExpediente + ".",
                    "Historial del expediente"
                );

                this.Close();
            }
        }


        // =========================================================
        // CARGAR DATOS E HISTORIAL
        // =========================================================

        private bool CargarHistorial()
        {
            string[] datos = Funciones.BuscarExpediente(numeroExpediente);

            if (datos == null)
            {
                return false;
            }

            string[] campos = Funciones.CompletarCampos(datos);

            this.Text = "Historial del expediente EXP-" + numeroExpediente;

            // Datos principales
            lblCodigo.Text = "EXP-" + numeroExpediente;
            lblEstado.Text = Mostrar(campos[9]);
            lblSolicitante.Text = Mostrar(campos[4]);
            lblArea.Text = Mostrar(campos[Funciones.CAMPO_AREA]);

            // Historial: una fila por acción
            string[,] historial = Funciones.MatrizHistorial(numeroExpediente);

            dgvHistorial.Rows.Clear();

            for (int i = 0; i < historial.GetLength(0); i++)
            {
                dgvHistorial.Rows.Add(
                    historial[i, 0],
                    Mostrar(historial[i, 1]),
                    historial[i, 2],
                    Mostrar(historial[i, 3]),
                    historial[i, 4]
                );
            }

            lblTotal.Text = historial.GetLength(0) + " acción(es) registradas";
            dgvHistorial.ClearSelection();

            return true;
        }

        // "Sin registrar" cuando falta el dato
        private string Mostrar(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? Funciones.SIN_REGISTRAR : valor.Trim();
        }


        // =========================================================
        // BOTONES
        // =========================================================

        // Ver detalle: datos completos y PDF del mismo expediente
        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            using (VER_DETALLE_DEL_EXPEDIENTE ventana = new VER_DETALLE_DEL_EXPEDIENTE(numeroExpediente))
            {
                ventana.ShowDialog(this);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
