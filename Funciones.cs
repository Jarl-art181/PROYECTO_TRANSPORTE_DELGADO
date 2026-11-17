using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    // =========================================================
    // FUNCIONES COMPARTIDAS
    // ---------------------------------------------------------
    // Este archivo agrupa las operaciones que antes estaban
    // repetidas en varios formularios: rutas de archivos,
    // lectura de expedientes, validaciones y manejo de PDF.
    //
    // Estructura de cada línea de expedientes.txt:
    //   0 Número | 1 Fecha | 2 Tipo | 3 Descripción |
    //   4 Solicitante | 5 DNI | 6 Empresa | 7 Teléfono |
    //   8 Correo | 9 Estado | 10 Ruta PDF | 11 Placa
    // =========================================================

    public static class Funciones
    {
        // Cantidad mínima de campos de un expediente válido
        public const int CAMPOS_MINIMOS = 10;

        // Listas fijas del sistema
        public static readonly string[] TiposDocumento =
        {
            "Solicitud", "Oficio", "Carta", "Informe", "Reclamo"
        };

        public static readonly string[] Estados =
        {
            "En proceso", "Pendiente", "Finalizado"
        };

        // Nombres de los estados (se usan en las reglas de seguimiento)
        public const string PENDIENTE = "Pendiente";
        public const string EN_PROCESO = "En proceso";
        public const string FINALIZADO = "Finalizado";

        // =====================================================
        // SEGUIMIENTO Y CIERRE
        // Después de los 12 campos originales se guardan 4 más:
        //   12 Fecha de inicio | 13 Resultado |
        //   14 Observación final | 15 Fecha de finalización
        // =====================================================

        //   16 Área de destino | 17 Responsable de la atención
        public const int CAMPOS_TOTALES = 18;
        public const int CAMPO_INICIO = 12;
        public const int CAMPO_RESULTADO = 13;
        public const int CAMPO_OBSERVACION = 14;
        public const int CAMPO_FIN = 15;
        public const int CAMPO_AREA = 16;
        public const int CAMPO_RESPONSABLE = 17;

        public const string SIN_REGISTRAR = "Sin registrar";

        // Resultados posibles y su significado (arreglos paralelos)
        public static readonly string[] Resultados =
        {
            "Atendido", "Aprobado", "Rechazado"
        };

        public static readonly string[] SignificadoResultados =
        {
            "Se completó una gestión que no requiere aprobación.",
            "Se autorizó la solicitud.",
            "Se denegó la solicitud y terminó su atención."
        };


        // =====================================================
        // RUTAS DE LOS ARCHIVOS
        // =====================================================

        public static string CarpetaDatos()
        {
            return Path.Combine(Application.StartupPath, "Datos");
        }

        public static string RutaExpedientes()
        {
            return Path.Combine(CarpetaDatos(), "expedientes.txt");
        }

        public static string RutaOpciones()
        {
            return Path.Combine(CarpetaDatos(), "opciones.txt");
        }

        public static string RutaConfiguracion()
        {
            return Path.Combine(CarpetaDatos(), "configuracion.txt");
        }

        public static string RutaArchivoCarpetaPDF()
        {
            return Path.Combine(CarpetaDatos(), "rutaPDF.txt");
        }

        public static string CarpetaPDFPredeterminada()
        {
            return Path.Combine(CarpetaDatos(), "PDF");
        }


        // =====================================================
        // MENSAJE DE ADVERTENCIA (evita repetir MessageBox)
        // =====================================================

        public static void Aviso(string mensaje, string titulo)
        {
            MessageBox.Show(
                mensaje,
                titulo,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }


        // =========================================================
        // VALIDAR UN CAMPO
        // Si la condición no se cumple, muestra el mensaje,
        // coloca el cursor en el control y devuelve false.
        // =========================================================

        public static bool Validar(bool condicion, string mensaje, string titulo, Control control)
        {
            if (condicion)
            {
                return true;
            }

            Aviso(mensaje, titulo);

            if (control != null)
            {
                control.Focus();
            }

            return false;
        }


        // =====================================================
        // LEER OPCIÓN DE CONFIGURACIÓN (opciones.txt)
        // indice 0: limpiar formulario después de guardar
        // indice 1: mostrar mensaje después de guardar
        // indice 2: cargar datos automáticamente
        // indice 3: generar números automáticamente
        // indice 4: confirmar antes de eliminar
        // indice 5: mostrar mensaje de bienvenida
        // =====================================================

        public static bool LeerOpcion(int indice, bool valorPorDefecto)
        {
            try
            {
                if (!File.Exists(RutaOpciones()))
                {
                    return valorPorDefecto;
                }

                string[] opciones =
                    File.ReadAllText(RutaOpciones()).Split('|');

                if (indice < opciones.Length)
                {
                    bool valor;

                    if (bool.TryParse(opciones[indice].Trim(), out valor))
                    {
                        return valor;
                    }
                }
            }
            catch (Exception)
            {
                // Si no se puede leer, se usa el valor por defecto
            }

            return valorPorDefecto;
        }


        // =====================================================
        // LEER TODAS LAS LÍNEAS DE expedientes.txt
        // Devuelve un arreglo vacío si no existe o hay error.
        // (Solo para MOSTRAR datos, nunca para reescribir.)
        // =====================================================

        public static string[] LeerExpedientes()
        {
            try
            {
                if (File.Exists(RutaExpedientes()))
                {
                    return File.ReadAllLines(RutaExpedientes());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo leer el archivo de expedientes.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            return new string[0];
        }


        // =====================================================
        // LEER SOLO LOS EXPEDIENTES VÁLIDOS
        // Ignora líneas vacías y registros incompletos.
        // =====================================================

        public static string[] LeerExpedientesValidos()
        {
            string[] lineas = LeerExpedientes();

            string[] temporal = new string[lineas.Length];
            int cantidad = 0;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (EsRegistroValido(lineas[i]))
                {
                    temporal[cantidad] = lineas[i];
                    cantidad++;
                }
            }

            // Copiar al tamaño exacto
            string[] validos = new string[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                validos[i] = temporal[i];
            }

            return validos;
        }

        public static bool EsRegistroValido(string linea)
        {
            if (string.IsNullOrWhiteSpace(linea))
            {
                return false;
            }

            return linea.Split('|').Length >= CAMPOS_MINIMOS;
        }


        // =====================================================
        // NORMALIZAR NÚMERO DE EXPEDIENTE
        // "EXP-00000012", "00000012" o "12" -> "00000012"
        // Devuelve "" si el número no es válido.
        // =====================================================

        public static string NormalizarNumero(string numero)
        {
            if (numero == null)
            {
                return "";
            }

            numero = numero.Trim().ToUpperInvariant();

            if (numero.StartsWith("EXP-"))
            {
                numero = numero.Substring(4);
            }

            if (!Regex.IsMatch(numero, @"^[0-9]{1,8}$"))
            {
                return "";
            }

            return numero.PadLeft(8, '0');
        }


        // =====================================================
        // LIMPIAR TEXTO ANTES DE GUARDAR
        // El carácter | separa los campos y el salto de línea
        // separa los expedientes; por eso se reemplazan.
        // =====================================================

        public static string LimpiarTexto(string texto)
        {
            if (texto == null)
            {
                return "";
            }

            texto = texto.Replace("|", "/");
            texto = texto.Replace("\r\n", " ");
            texto = texto.Replace("\n", " ");
            texto = texto.Replace("\r", " ");

            return texto.Trim();
        }


        // =====================================================
        // VALIDACIONES (iguales en Registrar y Modificar)
        // =====================================================

        public static bool EsFechaValida(string texto)
        {
            DateTime fecha;
            return ConvertirFecha(texto, out fecha);
        }

        // Interpreta siempre dd/MM/yyyy, sin depender
        // del idioma configurado en Windows
        public static bool ConvertirFecha(string texto, out DateTime fecha)
        {
            return DateTime.TryParseExact(
                (texto ?? "").Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha
            );
        }

        public static bool EsNombreValido(string texto)
        {
            texto = (texto ?? "").Trim();

            if (texto.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsLetter(texto[i]) && !char.IsWhiteSpace(texto[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool EsDNIValido(string texto)
        {
            // [0-9]: solo dígitos comunes (\d aceptaría dígitos de otros alfabetos)
            return Regex.IsMatch((texto ?? "").Trim(), @"^[0-9]{8}$");
        }

        public static bool EsTelefonoValido(string texto)
        {
            return Regex.IsMatch((texto ?? "").Trim(), @"^9[0-9]{8}$");
        }

        public static bool EsCorreoValido(string texto)
        {
            return Regex.IsMatch(
                (texto ?? "").Trim(),
                @"^[^@\s|]+@gmail\.com$",
                RegexOptions.IgnoreCase
            );
        }

        public static bool EsPlacaValida(string texto)
        {
            return Regex.IsMatch(
                (texto ?? "").Trim().ToUpperInvariant(),
                @"^[A-Z0-9]{3}-[A-Z0-9]{3}$"
            );
        }

        public static bool EstaEnLista(string texto, string[] lista)
        {
            texto = (texto ?? "").Trim();

            for (int i = 0; i < lista.Length; i++)
            {
                if (texto.Equals(lista[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        // =====================================================
        // VERIFICAR SI YA EXISTE UN NÚMERO DE EXPEDIENTE
        // =====================================================

        public static bool ExisteExpediente(string numero)
        {
            string buscado = NormalizarNumero(numero);

            // Matriz: cada fila es un expediente, cada columna un campo
            string[,] matriz = LeerMatrizExpedientes();

            // Recorrer las filas revisando la columna 0 (número)
            for (int fila = 0; fila < matriz.GetLength(0); fila++)
            {
                if (NormalizarNumero(matriz[fila, 0]) == buscado)
                {
                    return true;
                }
            }

            return false;
        }


        // =====================================================
        // MATRIZ DE EXPEDIENTES (ARREGLO BIDIMENSIONAL)
        // Filas    = expedientes guardados
        // Columnas = los 16 campos de cada expediente
        //            (12 originales + 4 de seguimiento)
        // Si a un registro antiguo le faltan campos, se dejan
        // vacíos ("") para que la matriz quede completa.
        // =====================================================

        public const int COLUMNAS = CAMPOS_TOTALES;

        public static string[,] LeerMatrizExpedientes()
        {
            string[] lineas = LeerExpedientes();

            // 1. Contar las líneas que tienen datos
            int filas = 0;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lineas[i]))
                {
                    filas++;
                }
            }

            // 2. Crear la matriz y llenarla con ciclos for anidados
            string[,] matriz = new string[filas, COLUMNAS];
            int fila = 0;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i]))
                {
                    continue;
                }

                string[] datos = lineas[i].Split('|');

                for (int columna = 0; columna < COLUMNAS; columna++)
                {
                    if (columna < datos.Length)
                    {
                        matriz[fila, columna] = datos[columna].Trim();
                    }
                    else
                    {
                        matriz[fila, columna] = "";
                    }
                }

                fila++;
            }

            return matriz;
        }


        // =====================================================
        // COMPLETAR LOS 16 CAMPOS DE UN EXPEDIENTE
        // Los registros antiguos tienen 12 campos (o menos);
        // los que faltan se dejan vacíos, sin inventar datos.
        // =====================================================

        public static string[] CompletarCampos(string[] datos)
        {
            int total = Math.Max(CAMPOS_TOTALES, datos.Length);
            string[] completos = new string[total];

            for (int i = 0; i < total; i++)
            {
                if (i < datos.Length)
                {
                    completos[i] = datos[i];
                }
                else
                {
                    completos[i] = "";
                }
            }

            return completos;
        }


        // Valor de un campo para mostrarlo en pantalla:
        // si no existe o está vacío, devuelve "Sin registrar"
        public static string CampoParaMostrar(string[] datos, int posicion)
        {
            if (posicion < datos.Length && !string.IsNullOrWhiteSpace(datos[posicion]))
            {
                return datos[posicion].Trim();
            }

            return SIN_REGISTRAR;
        }


        // Fecha y hora actual en formato dd/MM/yyyy HH:mm
        public static string FechaHoraActual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }


        // Verifica que un texto no tenga | ni saltos de línea,
        // porque dañarían el formato de expedientes.txt
        public static bool TextoValidoParaArchivo(string texto)
        {
            return texto.IndexOf('|') < 0 &&
                   texto.IndexOf('\n') < 0 &&
                   texto.IndexOf('\r') < 0;
        }


        // Estado actual de un expediente ("" si no existe)
        public static string EstadoDe(string numero)
        {
            string[] datos = BuscarExpediente(numero);

            if (datos == null)
            {
                return "";
            }

            return datos[9].Trim();
        }

        public static bool EsEstado(string estado, string buscado)
        {
            return (estado ?? "").Trim().Equals(buscado, StringComparison.OrdinalIgnoreCase);
        }


        // =====================================================
        // CAMBIAR EL ESTADO DE UN EXPEDIENTE
        // Solo cambia si el estado actual es el esperado.
        // posiciones[] y valores[] son arreglos paralelos con
        // los campos de seguimiento que se deben guardar.
        // =====================================================

        private static bool CambiarEstado(
            string numero,
            string estadoEsperado,
            string estadoNuevo,
            int[] posiciones,
            string[] valores)
        {
            string buscado = NormalizarNumero(numero);
            string[] registros = LeerParaModificar();

            if (registros == null || buscado == "")
            {
                return false;
            }

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (NormalizarNumero(datos[0]) != buscado)
                {
                    continue;
                }

                if (datos.Length < CAMPOS_MINIMOS)
                {
                    Aviso("El registro del expediente está incompleto.", "Cambio de estado");
                    return false;
                }

                // Regla: solo se avanza desde el estado esperado
                if (!EsEstado(datos[9], estadoEsperado))
                {
                    Aviso(
                        "El expediente está en estado \"" + datos[9].Trim() + "\".\n" +
                        "Esta acción solo es posible si está \"" + estadoEsperado + "\".",
                        "Cambio de estado no permitido"
                    );

                    return false;
                }

                // Completar a 16 campos y aplicar los cambios
                string[] campos = CompletarCampos(datos);
                campos[9] = estadoNuevo;

                for (int k = 0; k < posiciones.Length; k++)
                {
                    campos[posiciones[k]] = valores[k];
                }

                registros[i] = string.Join("|", campos);

                return GuardarExpedientes(registros);
            }

            Aviso("No se encontró el expediente.", "Cambio de estado");
            return false;
        }


        // Pendiente -> En proceso (guarda la fecha y hora de inicio)
        public static bool IniciarAtencion(string numero)
        {
            return IniciarAtencion(numero, "");
        }

        // Pendiente -> En proceso, guardando el área responsable.
        // Si el expediente no tenía área de destino (registro antiguo),
        // también se guarda como área de destino.
        public static bool IniciarAtencion(string numero, string areaResponsable)
        {
            areaResponsable = (areaResponsable ?? "").Trim();

            if (areaResponsable != "" && !EstaEnLista(areaResponsable, Areas))
            {
                Aviso("Seleccione un área responsable válida.", "Iniciar atención");
                return false;
            }

            string[] datos = BuscarExpediente(numero);
            bool sinArea = datos == null ||
                           CompletarCampos(datos)[CAMPO_AREA].Trim() == "";

            bool iniciado;

            if (sinArea && areaResponsable != "")
            {
                int[] posiciones3 = { CAMPO_INICIO, CAMPO_RESPONSABLE, CAMPO_AREA };
                string[] valores3 = { FechaHoraActual(), areaResponsable, areaResponsable };

                iniciado = CambiarEstado(numero, PENDIENTE, EN_PROCESO, posiciones3, valores3);
            }
            else
            {
                int[] posiciones = { CAMPO_INICIO, CAMPO_RESPONSABLE };
                string[] valores = { FechaHoraActual(), areaResponsable };

                iniciado = CambiarEstado(numero, PENDIENTE, EN_PROCESO, posiciones, valores);
            }

            if (iniciado)
            {
                RegistrarHistorial(numero, ACCION_INICIO,
                    areaResponsable != "" ? areaResponsable : UsuarioActual,
                    "Estado: Pendiente -> En proceso");
            }

            return iniciado;
        }


        // En proceso -> Finalizado (resultado, observación y fecha de fin)
        public static bool FinalizarExpediente(string numero, string resultado, string observacion)
        {
            if (!EstaEnLista(resultado, Resultados) ||
                string.IsNullOrWhiteSpace(observacion) ||
                !TextoValidoParaArchivo(observacion))
            {
                Aviso("El resultado o la observación final no son válidos.", "Finalizar expediente");
                return false;
            }

            int[] posiciones = { CAMPO_RESULTADO, CAMPO_OBSERVACION, CAMPO_FIN };
            string[] valores = { resultado.Trim(), observacion.Trim(), FechaHoraActual() };

            if (!CambiarEstado(numero, EN_PROCESO, FINALIZADO, posiciones, valores))
            {
                return false;
            }

            string[] datos = BuscarExpediente(numero);
            string responsable = datos != null ? CompletarCampos(datos)[CAMPO_RESPONSABLE].Trim() : "";

            RegistrarHistorial(numero, ACCION_FIN,
                responsable != "" ? responsable : UsuarioActual,
                resultado.Trim() + ": " + observacion.Trim());

            return true;
        }


        // =====================================================
        // ARCHIVOS DE SEGUIMIENTO Y CORREOS
        //   historial.txt   : número | fecha | hora | acción |
        //                     responsable | observación
        //   movimientos.txt : formato anterior (solo lectura)
        //   correos.txt     : número | fecha y hora | para | asunto |
        //                     mensaje | adjunto | destinatario
        //                     (Solicitante o el área de destino)
        //   correos_areas.txt : área | encargado | correo
        // =====================================================

        // Áreas de la empresa (Registro e Iniciar atención)
        public static readonly string[] Areas =
        {
            "Administración", "Operaciones", "Contabilidad", "Gerencia"
        };

        public static string RutaMovimientos()
        {
            return Path.Combine(CarpetaDatos(), "movimientos.txt");
        }

        public static string RutaCorreos()
        {
            return Path.Combine(CarpetaDatos(), "correos.txt");
        }

        public static string RutaConfiguracionCorreo()
        {
            return Path.Combine(CarpetaDatos(), "correo.txt");
        }

        // Lee un archivo de datos sin líneas vacías ("" si no existe)
        public static string[] LeerLineasArchivo(string ruta)
        {
            try
            {
                if (!File.Exists(ruta))
                {
                    return new string[0];
                }

                string[] todas = File.ReadAllLines(ruta);
                string[] temporal = new string[todas.Length];
                int cantidad = 0;

                for (int i = 0; i < todas.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(todas[i]))
                    {
                        temporal[cantidad] = todas[i];
                        cantidad++;
                    }
                }

                string[] lineas = new string[cantidad];

                for (int i = 0; i < cantidad; i++)
                {
                    lineas[i] = temporal[i];
                }

                return lineas;
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        // Agrega una línea al final de un archivo de datos
        private static bool AgregarLinea(string ruta, string linea)
        {
            try
            {
                Directory.CreateDirectory(CarpetaDatos());
                File.AppendAllText(ruta, linea + Environment.NewLine);
                return true;
            }
            catch (Exception ex)
            {
                Aviso("No se pudo guardar en " + Path.GetFileName(ruta) + ".\n" + ex.Message, "Error");
                return false;
            }
        }

        // Devuelve solo las líneas de un expediente
        private static string[] LineasDelExpediente(string ruta, string numero)
        {
            string buscado = NormalizarNumero(numero);
            string[] lineas = LeerLineasArchivo(ruta);
            string[] temporal = new string[lineas.Length];
            int cantidad = 0;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (NormalizarNumero(lineas[i].Split('|')[0]) == buscado)
                {
                    temporal[cantidad] = lineas[i];
                    cantidad++;
                }
            }

            string[] resultado = new string[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                resultado[i] = temporal[i];
            }

            return resultado;
        }


        // =====================================================
        // HISTORIAL DEL EXPEDIENTE (historial.txt)
        // Una línea por acción realizada:
        //   número | fecha | hora | acción | responsable | observación
        // Solo se registran acciones que realmente ocurrieron.
        // =====================================================

        // Acciones que se guardan en el historial
        public const string ACCION_REGISTRO = "Expediente registrado";
        public const string ACCION_MODIFICACION = "Expediente modificado";
        public const string ACCION_INICIO = "Atención iniciada";
        public const string ACCION_FIN = "Expediente finalizado";
        public const string ACCION_CORREO_PREPARADO = "Correo preparado";
        public const string ACCION_DERIVACION = "Derivación registrada";

        // "Correo enviado" solo se registraría con la confirmación real
        // de un servicio de correo (todavía no hay ninguno conectado)
        public const string ACCION_CORREO_ENVIADO = "Correo enviado";

        // Usuario que inició sesión (responsable de las acciones del sistema)
        public static string UsuarioActual = "Administrador";

        public static string RutaHistorial()
        {
            return Path.Combine(CarpetaDatos(), "historial.txt");
        }

        // Guarda una acción en el historial.
        // No repite la misma acción, responsable y observación
        // si ya se registró para ese expediente en el mismo minuto.
        public static bool RegistrarHistorial(string numero, string accion, string responsable, string observacion)
        {
            string buscado = NormalizarNumero(numero);

            if (buscado == "")
            {
                return false;
            }

            string fecha = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string hora = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture);

            string linea =
                buscado + "|" + fecha + "|" + hora + "|" + LimpiarTexto(accion) + "|" +
                LimpiarTexto(responsable) + "|" + LimpiarTexto(observacion);

            // Evitar duplicados
            string[] anteriores = LineasDelExpediente(RutaHistorial(), buscado);

            for (int i = 0; i < anteriores.Length; i++)
            {
                if (anteriores[i].Trim() == linea)
                {
                    return true;
                }
            }

            return AgregarLinea(RutaHistorial(), linea);
        }

        // Historial completo como matriz de 5 columnas:
        //   0 fecha | 1 hora | 2 acción | 3 responsable | 4 observación
        // Ordenado por fecha y hora (método burbuja).
        // Si un expediente antiguo no tiene alguna acción en el
        // historial, se completa con los datos de expedientes.txt
        // (sin inventar horas y sin repetir acciones).
        public static string[,] MatrizHistorial(string numero)
        {
            string[] datos = BuscarExpediente(numero);

            if (datos == null)
            {
                return new string[0, 5];
            }

            string[] campos = CompletarCampos(datos);
            string[] lineas = LineasDelExpediente(RutaHistorial(), numero);
            string[] antiguos = LineasDelExpediente(RutaMovimientos(), numero);

            string[,] filas = new string[lineas.Length + antiguos.Length + 3, 5];
            int cantidad = 0;

            // 1. Acciones guardadas en historial.txt
            for (int i = 0; i < lineas.Length; i++)
            {
                string[] p = lineas[i].Split('|');

                if (p.Length < 6)
                {
                    continue;
                }

                for (int k = 0; k < 5; k++)
                {
                    filas[cantidad, k] = p[k + 1].Trim();
                }

                cantidad++;
            }

            // 2. Movimientos del formato anterior (movimientos.txt)
            for (int i = 0; i < antiguos.Length; i++)
            {
                string[] p = antiguos[i].Split('|');

                if (p.Length < 3 || ExisteAccion(filas, cantidad, p[2].Trim()))
                {
                    continue;
                }

                string[] fechaHora = p[1].Trim().Split(' ');

                filas[cantidad, 0] = fechaHora[0];
                filas[cantidad, 1] = fechaHora.Length > 1 ? fechaHora[1] : "";
                filas[cantidad, 2] = p[2].Trim();
                filas[cantidad, 3] = SIN_REGISTRAR;
                filas[cantidad, 4] = "";
                cantidad++;
            }

            // 3. Acciones que solo constan en expedientes.txt
            string responsable = campos[CAMPO_RESPONSABLE].Trim() != "" ? campos[CAMPO_RESPONSABLE].Trim() : SIN_REGISTRAR;

            if (!ExisteAccion(filas, cantidad, ACCION_REGISTRO))
            {
                AgregarFila(filas, ref cantidad, campos[1].Trim(), ACCION_REGISTRO, SIN_REGISTRAR,
                    "Registrado antes del historial");
            }

            if (campos[CAMPO_INICIO].Trim() != "" && !ExisteAccion(filas, cantidad, ACCION_INICIO))
            {
                AgregarFila(filas, ref cantidad, campos[CAMPO_INICIO].Trim(), ACCION_INICIO, responsable,
                    "Estado: En proceso");
            }

            if (campos[CAMPO_FIN].Trim() != "" && !ExisteAccion(filas, cantidad, ACCION_FIN))
            {
                AgregarFila(filas, ref cantidad, campos[CAMPO_FIN].Trim(), ACCION_FIN, responsable,
                    campos[CAMPO_RESULTADO].Trim() + ": " + campos[CAMPO_OBSERVACION].Trim());
            }

            // 4. Ordenar por fecha y hora (método burbuja)
            for (int i = 0; i < cantidad - 1; i++)
            {
                for (int j = 0; j < cantidad - 1 - i; j++)
                {
                    if (FechaParaOrdenar(filas[j, 0] + " " + filas[j, 1]) >
                        FechaParaOrdenar(filas[j + 1, 0] + " " + filas[j + 1, 1]))
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            string temporal = filas[j, k];
                            filas[j, k] = filas[j + 1, k];
                            filas[j + 1, k] = temporal;
                        }
                    }
                }
            }

            // 5. Matriz final con el tamaño exacto
            string[,] historial = new string[cantidad, 5];

            for (int i = 0; i < cantidad; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    historial[i, k] = filas[i, k] ?? "";
                }
            }

            return historial;
        }

        // ¿La acción ya está en las primeras "cantidad" filas?
        private static bool ExisteAccion(string[,] filas, int cantidad, string accion)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (filas[i, 2] == accion)
                {
                    return true;
                }
            }

            return false;
        }

        // Agrega una fila separando la fecha y la hora (si la hay)
        private static void AgregarFila(string[,] filas, ref int cantidad, string fechaHora,
            string accion, string responsable, string observacion)
        {
            string[] partes = fechaHora.Split(' ');

            filas[cantidad, 0] = partes[0];
            filas[cantidad, 1] = partes.Length > 1 ? partes[1] : "";
            filas[cantidad, 2] = accion;
            filas[cantidad, 3] = responsable;
            filas[cantidad, 4] = observacion;
            cantidad++;
        }

        // Historial en texto (una línea por acción) para listas
        public static string[] HistorialDelExpediente(string numero)
        {
            string[,] filas = MatrizHistorial(numero);
            string[] historial = new string[filas.GetLength(0)];

            for (int i = 0; i < historial.Length; i++)
            {
                historial[i] =
                    (filas[i, 0] + " " + filas[i, 1]).Trim() + "   " + filas[i, 2] +
                    (filas[i, 3] != "" && filas[i, 3] != SIN_REGISTRAR ? " (" + filas[i, 3] + ")" : "");
            }

            return historial;
        }

        // Última acción del historial ("Sin registrar" si no hay)
        public static string UltimoMovimiento(string numero)
        {
            string[,] filas = MatrizHistorial(numero);
            int ultima = filas.GetLength(0) - 1;

            if (ultima < 0)
            {
                return SIN_REGISTRAR;
            }

            return (filas[ultima, 0] + " " + filas[ultima, 1]).Trim() + " - " + filas[ultima, 2];
        }

        private static DateTime FechaParaOrdenar(string texto)
        {
            DateTime fecha;
            texto = texto.Trim();

            if (DateTime.TryParseExact(texto, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha) ||
                DateTime.TryParseExact(texto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                return fecha;
            }

            return DateTime.MinValue;
        }


        // =====================================================
        // CORREOS PREPARADOS (correos.txt)
        // El mensaje se guarda en una sola línea: los saltos
        // de línea se escriben como \n y se recuperan al leerlo.
        // =====================================================

        public static bool RegistrarCorreo(string numero, string para, string asunto, string mensaje, string adjunto, string area)
        {
            string mensajeEnUnaLinea = (mensaje ?? "").Replace("|", "/").Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\n");

            string linea =
                NormalizarNumero(numero) + "|" + FechaHoraActual() + "|" +
                LimpiarTexto(para) + "|" + LimpiarTexto(asunto) + "|" +
                mensajeEnUnaLinea + "|" + LimpiarTexto(adjunto) + "|" + LimpiarTexto(area);

            return AgregarLinea(RutaCorreos(), linea);
        }

        // Correos registrados del expediente (cada uno con 7 campos)
        public static string[] CorreosDelExpediente(string numero)
        {
            return LineasDelExpediente(RutaCorreos(), numero);
        }

        public static string MensajeDeCorreo(string guardado)
        {
            return (guardado ?? "").Replace("\\n", Environment.NewLine);
        }


        // =====================================================
        // ENCARGADOS DE ÁREA (correos_areas.txt)
        // Una línea por área:  área | encargado | correo
        // (también se leen líneas antiguas:  área | correo)
        // =====================================================

        public static string RutaCorreosAreas()
        {
            return Path.Combine(CarpetaDatos(), "correos_areas.txt");
        }

        // Encargado y correo guardados para el área (false si no hay)
        public static bool EncargadoDeArea(string area, out string encargado, out string correo)
        {
            encargado = "";
            correo = "";

            string[] lineas = LeerLineasArchivo(RutaCorreosAreas());

            for (int i = 0; i < lineas.Length; i++)
            {
                string[] partes = lineas[i].Split('|');

                if (partes[0].Trim() != (area ?? "").Trim())
                {
                    continue;
                }

                if (partes.Length >= 3)
                {
                    encargado = partes[1].Trim();
                    correo = partes[2].Trim();
                }
                else if (partes.Length == 2)
                {
                    correo = partes[1].Trim();
                }

                return true;
            }

            return false;
        }

        // Guarda (o reemplaza) el encargado y el correo del área
        public static bool GuardarEncargadoDeArea(string area, string encargado, string correo)
        {
            string[] lineas = LeerLineasArchivo(RutaCorreosAreas());
            string[] nuevas = new string[lineas.Length + 1];
            int cantidad = 0;

            // Copiar las demás áreas
            for (int i = 0; i < lineas.Length; i++)
            {
                if (lineas[i].Split('|')[0].Trim() != area.Trim())
                {
                    nuevas[cantidad] = lineas[i];
                    cantidad++;
                }
            }

            // Agregar el área con su encargado
            nuevas[cantidad] = LimpiarTexto(area) + "|" + LimpiarTexto(encargado) + "|" + LimpiarTexto(correo);
            cantidad++;

            string[] resultado = new string[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                resultado[i] = nuevas[i];
            }

            Directory.CreateDirectory(CarpetaDatos());
            return GuardarArchivo(RutaCorreosAreas(), resultado);
        }


        // =====================================================
        // ABRIR GMAIL EN EL NAVEGADOR
        // El sistema no envía correos ni pide contraseñas:
        // abre la ventana de redacción de Gmail en el navegador
        // predeterminado con el destinatario, el asunto y el
        // mensaje ya escritos. Si no hay una sesión iniciada,
        // Gmail pide iniciar sesión y luego muestra el correo.
        // Gmail no permite adjuntar archivos desde el enlace:
        // el PDF se adjunta en Gmail antes de enviar.
        // =====================================================

        // false solo en pruebas automáticas: arma el enlace sin abrirlo
        public static bool AbrirAplicacionDeCorreo = true;

        // Último enlace preparado (para revisar en pruebas)
        public static string UltimoEnlaceCorreo = "";

        public static bool EsCorreoConFormato(string texto)
        {
            texto = (texto ?? "").Trim();

            if (texto == "" || texto.Contains(" ") || texto.Contains("|"))
            {
                return false;
            }

            try
            {
                System.Net.Mail.MailAddress direccion = new System.Net.Mail.MailAddress(texto);
                return direccion.Address == texto;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Dirección oficial de redacción de Gmail
        public const string GMAIL_REDACTAR = "https://mail.google.com/mail/?view=cm&fs=1";

        // Arma el enlace de Gmail (UNA sola dirección, sin copias):
        //   to = destinatario, su = asunto, body = mensaje
        public static string EnlaceGmail(string para, string asunto, string mensaje)
        {
            string texto = (mensaje ?? "").Replace("\r\n", "\n");

            return GMAIL_REDACTAR +
                   "&to=" + Uri.EscapeDataString((para ?? "").Trim()) +
                   "&su=" + Uri.EscapeDataString(asunto ?? "") +
                   "&body=" + Uri.EscapeDataString(texto);
        }

        // Abre Gmail en el navegador predeterminado.
        // Devuelve true si se abrió (NO significa que el correo se envió).
        public static bool AbrirGmail(string para, string asunto, string mensaje, out string error)
        {
            error = "";
            UltimoEnlaceCorreo = EnlaceGmail(para, asunto, mensaje);

            try
            {
                if (AbrirAplicacionDeCorreo)
                {
                    ProcessStartInfo inicio = new ProcessStartInfo(UltimoEnlaceCorreo);
                    inicio.UseShellExecute = true;
                    Process.Start(inicio);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = "No se pudo abrir el navegador predeterminado.\n" + ex.Message;
                return false;
            }
        }

        // =====================================================
        // CERRAR SESIÓN
        // Cierra las ventanas del sistema y vuelve a la
        // pantalla de Iniciar sesión (no cierra el programa).
        // =====================================================

        public static void CerrarSesion(Form actual)
        {
            Form inicioSesion = null;

            // Buscar la ventana de Iniciar sesión y cerrar las demás
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form ventana = Application.OpenForms[i];

                if (ventana is INICIAR_SESIÓN)
                {
                    inicioSesion = ventana;
                }
                else if (ventana != actual)
                {
                    ventana.Close();
                }
            }

            // Si no existe (caso excepcional), cerrar el programa
            if (inicioSesion == null)
            {
                Application.Exit();
                return;
            }

            inicioSesion.Show();
            inicioSesion.WindowState = FormWindowState.Maximized;
            inicioSesion.Activate();

            actual.Close();
        }


        // =====================================================
        // AGREGAR UN EXPEDIENTE AL FINAL DEL ARCHIVO
        // =====================================================

        public static bool AgregarExpediente(string registro)
        {
            try
            {
                Directory.CreateDirectory(CarpetaDatos());

                File.AppendAllText(
                    RutaExpedientes(),
                    registro + Environment.NewLine
                );

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar el expediente.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }


        // =====================================================
        // GUARDAR TODO EL ARCHIVO DE FORMA SEGURA
        // Primero se escribe un archivo temporal y luego se
        // reemplaza el original. Si algo falla, el archivo
        // original queda intacto.
        // =====================================================

        public static bool GuardarExpedientes(string[] lineas)
        {
            return GuardarArchivo(RutaExpedientes(), lineas);
        }

        // Guarda cualquier archivo de datos de forma segura
        public static bool GuardarArchivo(string ruta, string[] lineas)
        {
            string temporal = ruta + ".tmp";

            try
            {
                File.WriteAllLines(temporal, lineas);

                if (File.Exists(ruta))
                {
                    File.Replace(temporal, ruta, null);
                }
                else
                {
                    File.Move(temporal, ruta);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron guardar los cambios.\n" +
                    "El archivo original no fue modificado.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                try
                {
                    if (File.Exists(temporal))
                    {
                        File.Delete(temporal);
                    }
                }
                catch (Exception)
                {
                }

                return false;
            }
        }


        // =====================================================
        // LEER EL ARCHIVO PARA REESCRIBIRLO
        // Devuelve null si hay un error, para que el formulario
        // NO reescriba el archivo con datos incompletos.
        // =====================================================

        public static string[] LeerParaModificar()
        {
            try
            {
                if (!File.Exists(RutaExpedientes()))
                {
                    Aviso(
                        "No existe el archivo de expedientes.",
                        "Archivo no encontrado"
                    );

                    return null;
                }

                return File.ReadAllLines(RutaExpedientes());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo leer el archivo de expedientes.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return null;
            }
        }


        // =====================================================
        // ELIMINAR UN EXPEDIENTE
        // Solo se quita la línea del expediente indicado.
        // Las demás líneas (incluso las incompletas) se
        // conservan tal como estaban.
        // =====================================================

        public static bool EliminarExpediente(string numero, out string rutaPDF)
        {
            rutaPDF = "";

            string buscado = NormalizarNumero(numero);

            string[] registros = LeerParaModificar();

            if (registros == null || buscado == "")
            {
                return false;
            }

            string[] nuevos = new string[registros.Length];
            int posicion = 0;
            bool encontrado = false;

            for (int i = 0; i < registros.Length; i++)
            {
                // Las líneas vacías no contienen información
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                // Quitar solamente el primer expediente que coincida
                if (!encontrado && NormalizarNumero(datos[0]) == buscado)
                {
                    encontrado = true;

                    if (datos.Length > 10)
                    {
                        rutaPDF = datos[10].Trim();
                    }

                    continue;
                }

                // Conservar todos los demás registros
                nuevos[posicion] = registros[i];
                posicion++;
            }

            if (!encontrado)
            {
                return false;
            }

            string[] finales = new string[posicion];

            for (int i = 0; i < posicion; i++)
            {
                finales[i] = nuevos[i];
            }

            if (!GuardarExpedientes(finales))
            {
                return false;
            }

            // Quitar también su historial y sus correos, para que un
            // expediente nuevo con el mismo número no los herede
            QuitarLineasDelExpediente(RutaHistorial(), buscado);
            QuitarLineasDelExpediente(RutaMovimientos(), buscado);
            QuitarLineasDelExpediente(RutaCorreos(), buscado);

            return true;
        }


        // Quita de un archivo las líneas cuyo primer campo es el número
        private static void QuitarLineasDelExpediente(string ruta, string numero)
        {
            if (!File.Exists(ruta))
            {
                return;
            }

            string[] lineas = LeerLineasArchivo(ruta);
            string[] quedan = new string[lineas.Length];
            int cantidad = 0;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (NormalizarNumero(lineas[i].Split('|')[0]) != numero)
                {
                    quedan[cantidad] = lineas[i];
                    cantidad++;
                }
            }

            if (cantidad == lineas.Length)
            {
                return;
            }

            string[] finales = new string[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                finales[i] = quedan[i];
            }

            GuardarArchivo(ruta, finales);
        }


        // =====================================================
        // REEMPLAZAR UN EXPEDIENTE (MODIFICAR)
        // =====================================================

        public static bool ReemplazarExpediente(string numero, string nuevoRegistro)
        {
            string buscado = NormalizarNumero(numero);

            string[] registros = LeerParaModificar();

            if (registros == null || buscado == "")
            {
                return false;
            }

            bool encontrado = false;

            for (int i = 0; i < registros.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(registros[i]))
                {
                    continue;
                }

                string[] datos = registros[i].Split('|');

                if (NormalizarNumero(datos[0]) == buscado)
                {
                    registros[i] = nuevoRegistro;
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                Aviso(
                    "No se encontró el expediente que desea modificar.",
                    "Expediente no encontrado"
                );

                return false;
            }

            return GuardarExpedientes(registros);
        }


        // =====================================================
        // BUSCAR UN EXPEDIENTE POR NÚMERO
        // Devuelve los campos o null si no existe.
        // =====================================================

        public static string[] BuscarExpediente(string numero)
        {
            string buscado = NormalizarNumero(numero);
            string[] lineas = LeerExpedientesValidos();

            for (int i = 0; i < lineas.Length; i++)
            {
                string[] datos = lineas[i].Split('|');

                if (NormalizarNumero(datos[0]) == buscado)
                {
                    return datos;
                }
            }

            return null;
        }


        // =====================================================
        // CARPETA DONDE EL SISTEMA GUARDA LOS PDF
        // Usa la carpeta de rutaPDF.txt si existe; si no,
        // usa Datos\PDF.
        // =====================================================

        public static string ObtenerCarpetaPDF()
        {
            try
            {
                if (File.Exists(RutaArchivoCarpetaPDF()))
                {
                    string carpeta =
                        File.ReadAllText(RutaArchivoCarpetaPDF()).Trim();

                    if (carpeta != "" && Directory.Exists(carpeta))
                    {
                        return carpeta;
                    }
                }

                Directory.CreateDirectory(CarpetaPDFPredeterminada());
            }
            catch (Exception)
            {
            }

            return CarpetaPDFPredeterminada();
        }


        // =====================================================
        // COPIAR EL PDF A LA CARPETA DEL SISTEMA
        // El archivo original del usuario NO se modifica.
        // Nombre único: EXP-00000001_20261112_153000_nombre.pdf
        // Devuelve la nueva ruta, o "" si falló.
        // =====================================================

        public static string CopiarPDFAlSistema(string origen, string numero)
        {
            try
            {
                string carpeta = ObtenerCarpetaPDF();
                Directory.CreateDirectory(carpeta);

                string nombreBase =
                    "EXP-" + NormalizarNumero(numero) + "_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" +
                    Path.GetFileNameWithoutExtension(origen);

                string destino =
                    Path.Combine(carpeta, nombreBase + ".pdf");

                // Si ya existe, agregar _1, _2, ...
                for (int i = 1; File.Exists(destino); i++)
                {
                    destino = Path.Combine(
                        carpeta,
                        nombreBase + "_" + i + ".pdf"
                    );
                }

                // false = nunca sobrescribir
                File.Copy(origen, destino, false);

                return destino;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo copiar el PDF a la carpeta del sistema.\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return "";
            }
        }


        // =====================================================
        // ¿EL PDF ES UNA COPIA CREADA POR EL SISTEMA?
        // Debe estar en la carpeta del sistema y su nombre
        // debe comenzar con EXP-número_ (así nunca se toca un
        // archivo original del usuario).
        // =====================================================

        public static bool EsCopiaDelSistema(string rutaPDF, string numero)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rutaPDF))
                {
                    return false;
                }

                string carpetaArchivo =
                    Path.GetDirectoryName(Path.GetFullPath(rutaPDF));

                string nombre = Path.GetFileName(rutaPDF);

                string prefijo = "EXP-" + NormalizarNumero(numero) + "_";

                if (!nombre.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                string[] carpetasSistema =
                {
                    ObtenerCarpetaPDF(),
                    CarpetaPDFPredeterminada()
                };

                for (int i = 0; i < carpetasSistema.Length; i++)
                {
                    string carpeta =
                        Path.GetFullPath(carpetasSistema[i]).TrimEnd('\\');

                    if (string.Equals(
                        carpetaArchivo.TrimEnd('\\'),
                        carpeta,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }


        // =====================================================
        // ¿OTRO EXPEDIENTE USA EL MISMO PDF?
        // =====================================================

        public static bool PDFUsadoPorOtroExpediente(string rutaPDF)
        {
            string[] lineas = LeerExpedientes();

            for (int i = 0; i < lineas.Length; i++)
            {
                string[] datos = lineas[i].Split('|');

                if (datos.Length > 10 &&
                    string.Equals(
                        datos[10].Trim(),
                        rutaPDF.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        // =====================================================
        // BORRAR LA COPIA DEL PDF AL ELIMINAR UN EXPEDIENTE
        // Solo se borra si es una copia del sistema y ningún
        // otro expediente la utiliza. Los originales del
        // usuario nunca se borran.
        // =====================================================

        public static void BorrarCopiaPDF(string rutaPDF, string numero)
        {
            try
            {
                if (EsCopiaDelSistema(rutaPDF, numero) &&
                    !PDFUsadoPorOtroExpediente(rutaPDF) &&
                    File.Exists(rutaPDF))
                {
                    File.Delete(rutaPDF);
                }
            }
            catch (Exception)
            {
                // Si el PDF está abierto no se puede borrar;
                // el expediente ya fue eliminado correctamente.
            }
        }


        // =====================================================
        // ¿EL EXPEDIENTE TIENE UN PDF ASOCIADO?
        // =====================================================

        public static bool TienePDF(string rutaPDF)
        {
            return !string.IsNullOrWhiteSpace(rutaPDF) &&
                   !rutaPDF.Trim().Equals(
                       "Sin PDF",
                       StringComparison.OrdinalIgnoreCase);
        }


        // =====================================================
        // ABRIR PDF CON EL PROGRAMA PREDETERMINADO
        // =====================================================

        public static void AbrirPDF(string rutaPDF)
        {
            if (!TienePDF(rutaPDF))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (!File.Exists(rutaPDF))
            {
                Aviso("No se encontró el archivo PDF.", "Documento PDF");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = rutaPDF,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir el PDF.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // =====================================================
        // DESCARGAR (GUARDAR UNA COPIA) DEL PDF
        // =====================================================

        public static void DescargarPDF(string rutaPDF)
        {
            if (!TienePDF(rutaPDF))
            {
                MessageBox.Show(
                    "El expediente no tiene un PDF adjunto.",
                    "Documento PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (!File.Exists(rutaPDF))
            {
                Aviso("No se encontró el archivo PDF.", "Documento PDF");
                return;
            }

            using (SaveFileDialog guardar = new SaveFileDialog())
            {
                guardar.Filter = "Archivo PDF (*.pdf)|*.pdf";
                guardar.FileName = Path.GetFileName(rutaPDF);
                guardar.OverwritePrompt = true;

                if (guardar.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    string origen = Path.GetFullPath(rutaPDF);
                    string destino = Path.GetFullPath(guardar.FileName);

                    // Impedir copiar el PDF sobre sí mismo
                    if (string.Equals(
                        origen,
                        destino,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        Aviso(
                            "Seleccione otra ubicación para guardar el PDF.",
                            "Ubicación inválida"
                        );

                        return;
                    }

                    File.Copy(origen, destino, true);

                    MessageBox.Show(
                        "PDF guardado correctamente.",
                        "Documento PDF",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "No se pudo guardar el PDF.\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }


        // =====================================================
        // VERIFICAR TAMAÑO MÁXIMO DEL PDF (10 MB)
        // =====================================================

        public static bool PDFDentroDelLimite(string rutaPDF)
        {
            try
            {
                FileInfo archivo = new FileInfo(rutaPDF);

                if (archivo.Length > 10 * 1024 * 1024)
                {
                    Aviso(
                        "El archivo PDF no puede superar los 10 MB.",
                        "Archivo demasiado grande"
                    );

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Aviso("No se pudo leer el PDF.\n" + ex.Message, "Error");
                return false;
            }
        }


        // =====================================================
        // CARGAR UNA LISTA EN UN COMBOBOX
        // =====================================================

        public static void LlenarCombo(ComboBox combo, string[] lista)
        {
            combo.Items.Clear();

            for (int i = 0; i < lista.Length; i++)
            {
                combo.Items.Add(lista[i]);
            }
        }
    }
}
