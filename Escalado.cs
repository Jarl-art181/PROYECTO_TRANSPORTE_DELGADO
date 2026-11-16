using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    // =========================================================
    // ESCALADO DE LOS FORMULARIOS
    // ---------------------------------------------------------
    // Los formularios se diseñaron con un área de 1540 x 941.
    // Cuando la pantalla tiene otro tamaño (por ejemplo, con
    // el escalado de Windows al 125 %), este archivo ajusta
    // la posición, el tamaño y la letra de cada control en la
    // misma proporción, para que el diseño se vea completo y
    // sin controles cortados.
    //
    // Reglas:
    // - Posición y tamaño: proporcionales al ancho y al alto.
    // - Letras: se usa el factor menor, así el texto siempre
    //   cabe dentro de su control.
    // - Iconos y logotipos: conservan su proporción (no se
    //   deforman) y quedan centrados en su espacio.
    // =========================================================

    public static class Escalado
    {
        // Área con la que se diseñaron los formularios principales
        public const int ANCHO_DISENO = 1540;
        public const int ALTO_DISENO = 941;

        // Las imágenes de hasta este tamaño se tratan como
        // iconos o logotipos (no se deforman). Las más grandes
        // son fondos o banners y cubren su espacio completo.
        private const int ANCHO_MAXIMO_ICONO = 400;
        private const int ALTO_MAXIMO_ICONO = 200;

        // Permite desactivar el ajuste automático (se usa en pruebas)
        public static bool Activado = true;


        // =====================================================
        // FORMULARIOS A PANTALLA COMPLETA
        // (Inicio, Registro, Consultar, Buscar, Gestionar,
        //  Reportes, Configuración e Iniciar sesión)
        // =====================================================

        public static void AjustarPantallaCompleta(Form formulario)
        {
            if (!Activado)
            {
                return;
            }

            // Pantalla donde se encuentra el puntero del mouse
            Rectangle pantalla =
                Screen.FromPoint(Cursor.Position).Bounds;

            EscalarContenido(
                formulario,
                pantalla.Width,
                pantalla.Height
            );
        }


        // Escala el contenido para un área de ancho x alto
        public static void EscalarContenido(Form formulario, int ancho, int alto)
        {
            float factorX = ancho / (float)ANCHO_DISENO;
            float factorY = alto / (float)ALTO_DISENO;

            // Si la pantalla es igual al diseño, no hacer nada
            if (Math.Abs(factorX - 1) < 0.005f &&
                Math.Abs(factorY - 1) < 0.005f)
            {
                return;
            }

            EscalarHijos(formulario, factorX, factorY);
        }


        // =====================================================
        // VENTANAS DE DIÁLOGO
        // (Ver detalle y Modificar detalle)
        // Solo se reducen si no caben en la pantalla.
        // =====================================================

        public static void AjustarDialogo(Form formulario)
        {
            if (!Activado)
            {
                return;
            }

            // Área libre de la pantalla (sin la barra de tareas)
            Rectangle area =
                Screen.FromPoint(Cursor.Position).WorkingArea;

            float factorX = (area.Width - 20) / (float)formulario.Width;
            float factorY = (area.Height - 20) / (float)formulario.Height;

            float factor = Math.Min(1f, Math.Min(factorX, factorY));

            EscalarDialogo(formulario, factor);
        }


        public static void EscalarDialogo(Form formulario, float factor)
        {
            if (factor >= 0.995f)
            {
                return;
            }

            Size cliente = formulario.ClientSize;

            // El mismo factor en ambos ejes: nada se deforma
            EscalarHijos(formulario, factor, factor);

            formulario.ClientSize = new Size(
                (int)Math.Round(cliente.Width * factor),
                (int)Math.Round(cliente.Height * factor)
            );
        }


        // =====================================================
        // ESCALAR TODOS LOS CONTROLES DEL FORMULARIO
        // =====================================================

        private static void EscalarHijos(Form formulario, float factorX, float factorY)
        {
            float factorLetra = Math.Min(factorX, factorY);

            formulario.SuspendLayout();

            for (int i = 0; i < formulario.Controls.Count; i++)
            {
                EscalarControl(
                    formulario.Controls[i],
                    factorX,
                    factorY,
                    factorLetra
                );
            }

            formulario.ResumeLayout(true);
        }


        // =====================================================
        // ESCALAR UN CONTROL Y SUS CONTROLES INTERNOS
        // =====================================================

        private static void EscalarControl(
            Control control,
            float factorX,
            float factorY,
            float factorLetra)
        {
            control.SuspendLayout();

            // 1. Primero los controles internos.
            //    Así cada uno toma su letra original antes de
            //    que cambie la letra del contenedor.
            for (int i = 0; i < control.Controls.Count; i++)
            {
                EscalarControl(
                    control.Controls[i],
                    factorX,
                    factorY,
                    factorLetra
                );
            }

            // 2. Posición y tamaño.
            //    Se calculan los bordes para que los controles
            //    vecinos sigan juntos (sin huecos de 1 píxel).
            Rectangle r = control.Bounds;

            int izquierda = Redondear(r.Left * factorX);
            int arriba = Redondear(r.Top * factorY);
            int ancho = Redondear(r.Right * factorX) - izquierda;
            int alto = Redondear(r.Bottom * factorY) - arriba;

            // Iconos y logotipos: misma proporción, centrados
            if (EsIcono(control))
            {
                int anchoIcono = Redondear(r.Width * factorLetra);
                int altoIcono = Redondear(r.Height * factorLetra);

                izquierda += (ancho - anchoIcono) / 2;
                arriba += (alto - altoIcono) / 2;
                ancho = anchoIcono;
                alto = altoIcono;
            }

            control.Bounds = new Rectangle(izquierda, arriba, ancho, alto);

            // 3. Tamaño de letra
            //    Una tabla copia su letra general a los encabezados
            //    y celdas; por eso primero se guardan sus letras.
            Font letraEncabezado = null;
            Font letraCeldas = null;
            Font letraAlternas = null;

            if (control is DataGridView)
            {
                DataGridView tabla = (DataGridView)control;

                letraEncabezado = tabla.ColumnHeadersDefaultCellStyle.Font;
                letraCeldas = tabla.DefaultCellStyle.Font;
                letraAlternas = tabla.AlternatingRowsDefaultCellStyle.Font;
            }

            control.Font = EscalarFuente(control.Font, factorLetra);

            // 4. Ajustes de controles especiales
            if (control is DataGridView)
            {
                DataGridView tabla = (DataGridView)control;

                tabla.ColumnHeadersDefaultCellStyle.Font =
                    EscalarFuente(letraEncabezado, factorLetra);

                tabla.DefaultCellStyle.Font =
                    EscalarFuente(letraCeldas, factorLetra);

                tabla.AlternatingRowsDefaultCellStyle.Font =
                    EscalarFuente(letraAlternas, factorLetra);

                EscalarTabla(tabla, factorX, factorY);
            }

            if (control is ComboBox)
            {
                // La altura de un ComboBox depende de ItemHeight
                ComboBox combo = (ComboBox)control;
                combo.ItemHeight = Math.Max(1, Redondear(combo.ItemHeight * factorY));
            }

            if (control is Guna.UI2.WinForms.Guna2Button)
            {
                // Tamaño del icono dentro del botón
                Guna.UI2.WinForms.Guna2Button boton =
                    (Guna.UI2.WinForms.Guna2Button)control;

                boton.ImageSize = new Size(
                    Redondear(boton.ImageSize.Width * factorLetra),
                    Redondear(boton.ImageSize.Height * factorLetra)
                );
            }

            // Reanudar el diseño y recalcularlo.
            // (Guna2HtmlLabel necesita este cálculo para dibujar su texto)
            control.ResumeLayout(true);
        }


        // =====================================================
        // ESCALAR UNA TABLA (DataGridView)
        // =====================================================

        private static void EscalarTabla(
            DataGridView tabla,
            float factorX,
            float factorY)
        {
            // Altura del encabezado y de las filas
            if (tabla.ColumnHeadersHeightSizeMode !=
                DataGridViewColumnHeadersHeightSizeMode.AutoSize)
            {
                tabla.ColumnHeadersHeight =
                    Math.Max(4, Redondear(tabla.ColumnHeadersHeight * factorY));
            }

            tabla.RowTemplate.Height =
                Math.Max(4, Redondear(tabla.RowTemplate.Height * factorY));

            // Filas ya cargadas (si la tabla no calcula su altura sola)
            if (tabla.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.None)
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    tabla.Rows[i].Height = tabla.RowTemplate.Height;
                }
            }

            // Ancho de columnas (si no se ajustan solas)
            if (tabla.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
            {
                for (int i = 0; i < tabla.Columns.Count; i++)
                {
                    tabla.Columns[i].Width =
                        Math.Max(5, Redondear(tabla.Columns[i].Width * factorX));
                }
            }
        }


        // =====================================================
        // FUNCIONES DE APOYO
        // =====================================================

        private static bool EsIcono(Control control)
        {
            return control is PictureBox &&
                   control.Width <= ANCHO_MAXIMO_ICONO &&
                   control.Height <= ALTO_MAXIMO_ICONO;
        }

        private static Font EscalarFuente(Font fuente, float factor)
        {
            if (fuente == null)
            {
                return null;
            }

            return new Font(
                fuente.FontFamily,
                Math.Max(1f, fuente.Size * factor),
                fuente.Style,
                fuente.Unit
            );
        }

        private static int Redondear(float valor)
        {
            return (int)Math.Round(valor);
        }
    }
}
