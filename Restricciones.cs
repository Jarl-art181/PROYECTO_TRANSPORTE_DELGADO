using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    // =========================================================
    // RESTRICCIONES DE LOS CAMPOS
    // ---------------------------------------------------------
    // Limita lo que se puede escribir o pegar en cada campo.
    // Se usa el evento TextChanged, que ocurre al escribir,
    // al pegar y al borrar; así el control funciona siempre,
    // no solo con el teclado.
    //
    // Antes de guardar, cada formulario vuelve a validar
    // los datos y muestra un mensaje claro si algo falla.
    // =========================================================

    public static class Restricciones
    {
        // Largos máximos
        public const int LARGO_CODIGO = 12;      // EXP- + 8 dígitos
        public const int DIGITOS_CODIGO = 8;
        public const int LARGO_DNI = 8;
        public const int LARGO_CELULAR = 9;
        public const int LARGO_PLACA = 7;        // ABC-123
        public const int LARGO_FECHA = 10;       // dd/MM/yyyy

        public const int MAX_NOMBRE = 100;
        public const int MAX_EMPRESA = 150;
        public const int MAX_CORREO = 254;
        public const int MAX_DESCRIPCION = 500;
        public const int MAX_OBSERVACION = 500;

        public const int NUMERO_MAXIMO = 99999999;

        // Límite de entrada (MaxLength) para los campos que se
        // limpian solos (código, DNI, celular, placa, fecha).
        // Es más amplio que el largo final para que, al pegar
        // "987-654-321" o " EXP-00000001 ", Windows no corte el
        // texto antes de limpiarlo. Después de limpiar, el campo
        // nunca muestra más del largo permitido.
        public const int LARGO_ENTRADA = 40;

        public const string PREFIJO = "EXP-";


        // =====================================================
        // FUNCIONES DE LIMPIEZA
        // Reciben el texto escrito o pegado y devuelven solo
        // la parte permitida.
        // =====================================================

        // Deja solo los dígitos, hasta un máximo
        public static string SoloDigitos(string texto, int maximo)
        {
            string resultado = "";

            for (int i = 0; i < (texto ?? "").Length && resultado.Length < maximo; i++)
            {
                if (texto[i] >= '0' && texto[i] <= '9')
                {
                    resultado += texto[i];
                }
            }

            return resultado;
        }

        // Código EXP-00000001: prefijo fijo y hasta 8 dígitos.
        // prefijoFijo = true: el prefijo se mantiene aunque no
        // haya dígitos (Registrar con numeración manual).
        public static string LimpiarCodigo(string texto, bool prefijoFijo)
        {
            string digitos = SoloDigitos(texto, DIGITOS_CODIGO);

            if (digitos == "" && !prefijoFijo)
            {
                return "";
            }

            return PREFIJO + digitos;
        }

        // Placa ABC-123 o A1B-234: letras A-Z y números,
        // en mayúsculas y con el guion en la cuarta posición
        public static string LimpiarPlaca(string texto)
        {
            string caracteres = "";
            string t = (texto ?? "").ToUpperInvariant();

            for (int i = 0; i < t.Length && caracteres.Length < 6; i++)
            {
                bool letra = t[i] >= 'A' && t[i] <= 'Z';
                bool digito = t[i] >= '0' && t[i] <= '9';

                if (letra || digito)
                {
                    caracteres += t[i];
                }
            }

            if (caracteres.Length > 3)
            {
                return caracteres.Substring(0, 3) + "-" + caracteres.Substring(3);
            }

            return caracteres;
        }

        // Fecha: solo dígitos y "/", hasta 10 caracteres
        public static string LimpiarFecha(string texto)
        {
            string resultado = "";

            for (int i = 0; i < (texto ?? "").Length && resultado.Length < LARGO_FECHA; i++)
            {
                if ((texto[i] >= '0' && texto[i] <= '9') || texto[i] == '/')
                {
                    resultado += texto[i];
                }
            }

            return resultado;
        }


        // =====================================================
        // APLICAR LAS RESTRICCIONES A LOS CONTROLES
        // =====================================================

        // Código en un TextBox (Registrar o filtro de Consultar)
        public static void Codigo(Guna2TextBox campo, bool prefijoFijo)
        {
            campo.MaxLength = LARGO_ENTRADA;
            campo.Tag = prefijoFijo ? "prefijoFijo" : "";
            campo.TextChanged += Codigo_TextChanged;
        }

        // Código en la lista desplegable de Buscar
        public static void Codigo(ComboBox lista)
        {
            lista.MaxLength = LARGO_ENTRADA;
            lista.TextChanged += CodigoLista_TextChanged;
        }

        // DNI y celular: solo dígitos; el largo final se guarda en Tag
        public static void Dni(Guna2TextBox campo)
        {
            campo.MaxLength = LARGO_ENTRADA;
            campo.Tag = LARGO_DNI;
            campo.TextChanged += Digitos_TextChanged;
        }

        public static void Celular(Guna2TextBox campo)
        {
            campo.MaxLength = LARGO_ENTRADA;
            campo.Tag = LARGO_CELULAR;
            campo.TextChanged += Digitos_TextChanged;
        }

        public static void Placa(Guna2TextBox campo)
        {
            campo.MaxLength = LARGO_ENTRADA;
            campo.TextChanged += Placa_TextChanged;
        }

        public static void Fecha(Guna2TextBox campo)
        {
            campo.MaxLength = LARGO_ENTRADA;
            campo.TextChanged += Fecha_TextChanged;
        }

        // Textos libres: solo largo máximo
        public static void Texto(Guna2TextBox campo, int maximo)
        {
            campo.MaxLength = maximo;
        }


        // =====================================================
        // EVENTOS (se ejecutan al escribir, pegar o borrar)
        // Solo cambian el texto si contiene algo no permitido.
        // =====================================================

        private static void Codigo_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox campo = (Guna2TextBox)sender;

            // Numeración automática: el sistema pone el código
            if (campo.ReadOnly)
            {
                return;
            }

            bool prefijoFijo = "prefijoFijo".Equals(campo.Tag);
            Reemplazar(campo, LimpiarCodigo(campo.Text, prefijoFijo));
        }

        private static void CodigoLista_TextChanged(object sender, EventArgs e)
        {
            ComboBox lista = (ComboBox)sender;
            string limpio = LimpiarCodigo(lista.Text, false);

            if (lista.Text != limpio)
            {
                lista.Text = limpio;
                lista.SelectionStart = limpio.Length;
            }
        }

        private static void Digitos_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox campo = (Guna2TextBox)sender;
            int largo = (int)campo.Tag;   // 8 para DNI, 9 para celular
            Reemplazar(campo, SoloDigitos(campo.Text, largo));
        }

        private static void Placa_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox campo = (Guna2TextBox)sender;
            Reemplazar(campo, LimpiarPlaca(campo.Text));
        }

        private static void Fecha_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox campo = (Guna2TextBox)sender;
            Reemplazar(campo, LimpiarFecha(campo.Text));
        }

        // Cambia el texto solo si hace falta y deja el cursor al final
        private static void Reemplazar(Guna2TextBox campo, string limpio)
        {
            if (campo.Text != limpio)
            {
                campo.Text = limpio;
                campo.SelectionStart = limpio.Length;
            }
        }


        // =====================================================
        // VALIDACIONES ANTES DE GUARDAR
        // =====================================================

        public static bool EsCodigoValido(string texto)
        {
            return texto != null &&
                   texto.Length == LARGO_CODIGO &&
                   texto.StartsWith(PREFIJO) &&
                   SoloDigitos(texto, 99).Length == DIGITOS_CODIGO &&
                   SoloDigitos(texto.Substring(4), 99) == texto.Substring(4);
        }

        // Verifica el largo máximo y muestra un mensaje claro
        public static bool LargoValido(Control campo, int maximo, string nombreCampo)
        {
            int largo = campo.Text.Trim().Length;

            return Funciones.Validar(
                largo <= maximo,
                "El campo " + nombreCampo + " admite como máximo " + maximo +
                " caracteres (tiene " + largo + ").",
                "Texto demasiado largo",
                campo);
        }
    }
}
