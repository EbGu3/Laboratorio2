using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras
{
    public class Utilidades
    {

        internal const int TextoEnteroTamaño = 11;
        private const string TextoEnteroFormato = "0000000000;-0000000000";
        internal const int TextoLlaveTamaño = 50;

        internal const int TextoNuevaLineaTamaño = 2;
        internal const string TextoNuevaLinea = "\r\n";

        internal const char TextoSeparador = '|';
        internal const char TextoSustitutoSeparador = '?';

        internal static string FormatearEntero(int numero)
        {
            return numero.ToString(TextoEnteroFormato);
        }

        public static string FormatearLlave(string llave)
        {
            return llave.PadLeft(50, 'x');
        }


        //Utilidades Byte
        internal const int BinarioCaracterTamaño = 1;


        internal static string ConvertirBinarioYTexto(byte[] datosBinario)
        {
            return Encoding.ASCII.GetString(datosBinario);
        }

        internal static byte[] ConvertirBinarioYtexto(string datosTexto)
        {
            return Encoding.ASCII.GetBytes(datosTexto);
        }

        //Utilidades Entero
        internal const int EnteroYEnteroTamaño = (Utilidades.TextoEnteroTamaño + Utilidades.TextoNuevaLineaTamaño);
        internal const int EnteroYEnteroBinarioTamaño = EnteroYEnteroTamaño * Utilidades.BinarioCaracterTamaño;


        private static byte[] ConvertirEneroYEnter(int numero)
        {
            return Utilidades.ConvertirBinarioYtexto(Utilidades.FormatearEntero(numero) + Utilidades.TextoNuevaLinea);
        
        }

        private static int ConvertirEnteroYEnter(byte[] buffer)
        {
            return Convert.ToInt32(Utilidades.ConvertirBinarioYTexto(buffer).Replace(Utilidades.TextoNuevaLinea, ""));
        }

        //Archivo
        internal static int LeerEntero(FileStream archivo, int posicion)
        {
            if(archivo == null)
            {
                throw new ArgumentNullException("Archivo no existe");
            }

            if(posicion < 0)
            {
                throw new ArgumentNullException("Posicion inexistente");
            }

            try
            {
                byte[] buffer = new byte[EnteroYEnteroBinarioTamaño];
                posicion = posicion * EnteroYEnteroBinarioTamaño;
                archivo.Seek(posicion, SeekOrigin.Begin);
                archivo.Read(buffer, 0, EnteroYEnteroBinarioTamaño);
                return ConvertirEnteroYEnter(buffer);
            }
            catch (Exception)
            {
                return Utilidades.apuntadoVacio;
            }
        }

        internal static void EscribirEntero(FileStream archivo, int posicion, int numero)
        {
            if(archivo == null)
            {
                throw new ArgumentNullException("No se escribe en el archivo");
            }

            if(posicion < 0)
            {
                throw new ArgumentOutOfRangeException("Posicion inexistente");
            }

            byte[] buffer = ConvertirEneroYEnter(numero);
            posicion = posicion * EnteroYEnteroBinarioTamaño;
            archivo.Seek(posicion, SeekOrigin.Begin);
            archivo.Write(buffer, 0, EnteroYEnteroBinarioTamaño);

        }


        internal const int apuntadoVacio = -1;

    }
}
