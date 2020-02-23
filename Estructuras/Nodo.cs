using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using System.Collections.Generic;
namespace Estructuras
{
    public class Nodo<T> where T : ITextoTamañoFijo
    {
        public const int OrdenMinimo = 3;
        public const int OrdenMaximo = 99;

        internal int Orden { get; private set; }
        internal int Posicion { get; private set; }
        internal int Padre { get;  set; }

        internal List<int> Hijos { get; set; }
        internal List<string> Llaves { get; set; }
        internal List<T> Datos { get; set; }


        internal int CantidadDatos
        {
            get
            {
                int i = 0;
                while ((i < Llaves.Count) && (Llaves[i] != "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"))
                {
                    i++;
                }
                return i;
            }
        }

        internal bool Lleno
        {
            get
            {
                return (CantidadDatos >= Orden - 1);
            }
        }

        internal bool EsHoja
        {
            get
            {
                bool EsHoja = true;
                for (int i = 0; i < Hijos.Count; i++)
                {
                    EsHoja = false;
                    break;
                }
                return EsHoja;
            }
        }

        internal int TamañoTexto
        {
            get
            {
                int tamañoEnTexto = 0;
                //Tamaño del indicador de posicion
                tamañoEnTexto += Utilidades.TextoEnteroTamaño + 1;
                //Tamaño apuntado al padre
                tamañoEnTexto += Utilidades.TextoEnteroTamaño + 1;
                //Separadores Adicionales
                tamañoEnTexto += 2;
                tamañoEnTexto += (Utilidades.TextoEnteroTamaño + 1) *(Orden);
                //Separadores Adicionales
                tamañoEnTexto += 2;
                //Tamaño Llave
                tamañoEnTexto += (Utilidades.TextoEnteroTamaño + 1) * (Orden - 1);
                //Separadores Adicionales
                tamañoEnTexto += 2;
                //Tamaño Datos
                tamañoEnTexto += (Datos[0].FixedSizeText + 1) * (Orden - 1);
                //Tamaño del Enter
                tamañoEnTexto += Utilidades.TextoNuevaLineaTamaño;
                return tamañoEnTexto;
            }
        }
        internal int TamañoEnBytes
        {
            get
            {
                return TamañoTexto * Utilidades.BinarioCaracterTamaño;
            }
        }
        internal Nodo(int orden, int posicion, int padre, IGaseosasTamañoTextoFijo<T> gaseosa)
        {
            if((orden<OrdenMinimo) || (orden > OrdenMaximo)) { throw new ArgumentException("No es valido el tamaño"); }
            if(posicion < 0) { throw new ArgumentOutOfRangeException("Posicion invalida"); }
            Orden = orden; Posicion = posicion; Padre = padre;
            LimpiarNodo(gaseosa);
        }
        private int CalcularPosicionEnDisco(int tamañoEncabezado)
        {
            return tamañoEncabezado + (Posicion * TamañoEnBytes);
        }

        private string ConvertirATextoTamañoFijo()
        {
            StringBuilder datosCadena = new StringBuilder();
            datosCadena.Append(Utilidades.FormatearEntero(Posicion));
            datosCadena.Append(Utilidades.TextoSeparador);
            datosCadena.Append(Utilidades.FormatearEntero(Padre));
            datosCadena.Append(Utilidades.TextoSeparador);
            datosCadena.Append(Utilidades.TextoSeparador);
            datosCadena.Append(Utilidades.TextoSeparador);

            for (int i = 0; i < Hijos.Count; i++)
            {
                datosCadena.Append(Utilidades.FormatearEntero(Hijos[i]));
                datosCadena.Append(Utilidades.TextoSeparador);
            }

            datosCadena.Append(Utilidades.TextoSeparador);
            datosCadena.Append(Utilidades.TextoSeparador);

            for (int i = 0; i < Llaves.Count; i++)
            {
                datosCadena.Append(Utilidades.FormatearLlave(Llaves[i]));
                datosCadena.Append(Utilidades.TextoSeparador);
            }
            datosCadena.Append(Utilidades.TextoSeparador);
            datosCadena.Append(Utilidades.TextoSeparador);

            for (int i = 0; i < Datos.Count; i++)
            {
                datosCadena.Append(Datos[i].ToFixedToSizeString().Replace(Utilidades.TextoSeparador, Utilidades.TextoSustitutoSeparador));
                datosCadena.Append(Utilidades.TextoSeparador);
            }
            datosCadena.Append(Utilidades.TextoNuevaLinea);
            return datosCadena.ToString();
        }

        private byte[] ObtenerBytes()
        {
            byte[] datosBinarios = null;
            datosBinarios = Utilidades.ConvertirBinarioYtexto(ConvertirATextoTamañoFijo());
            return datosBinarios;
        }


        private void LimpiarNodo(IGaseosasTamañoTextoFijo<T> gaseosas)
        {
            Hijos = new List<int>();

            for (int i = 0; i < Orden; i++)
            {
                Hijos.Add(Utilidades.apuntadoVacio);
            }
            Llaves = new List<string>();

            for (int i = 0; i < Orden - 1; i++)
            {
                Llaves.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }

            Datos = new List<T>();

            for (int i = 0; i < Orden - 1; i++)
            {
                Datos.Add(gaseosas.CreateNulo());
            }
        }
        internal static Nodo<T> LeerNodoDesdeDisco(FileStream archivo, int tamañoEncabezado, int orden, int posicion, IGaseosasTamañoTextoFijo<T> gaseosas)
        {
            if (archivo == null)
            {
                throw new ArgumentNullException("archivo");
            }
            if (tamañoEncabezado < 0)
            {
                throw new ArgumentOutOfRangeException("tamañoEncabezado");
            }
            if ((orden < OrdenMinimo) || (orden > OrdenMaximo))
            {
                throw new ArgumentOutOfRangeException("orden");
            }
            if (posicion < 0)
            {
                throw new ArgumentOutOfRangeException("posicion");
            }
            if (gaseosas == null)
            {
                throw new ArgumentNullException("fabrica");
            }
            Nodo<T> nuevoNodo = new Nodo<T>(orden, posicion, 0, gaseosas);
            byte[] datosBinario = new byte[nuevoNodo.TamañoEnBytes];
            string datosCadena = "";
            string[] datosSeparados = null;
            int PosicionEnDatosCadena = 1;
            archivo.Seek(nuevoNodo.CalcularPosicionEnDisco(tamañoEncabezado), SeekOrigin.Begin);
            archivo.Read(datosBinario, 0, nuevoNodo.TamañoEnBytes);
            datosCadena = Utilidades.ConvertirBinarioYTexto(datosBinario);
            datosCadena = datosCadena.Replace(Utilidades.TextoNuevaLinea, "");
            datosCadena = datosCadena.Replace("".PadRight(3, Utilidades.TextoSeparador), Utilidades.TextoSeparador.ToString());
            datosSeparados = datosCadena.Split(Utilidades.TextoSeparador);
            nuevoNodo.Padre = Convert.ToInt32(datosSeparados[PosicionEnDatosCadena]);
            PosicionEnDatosCadena++;
            for (int i = 0; i < nuevoNodo.Hijos.Count; i++)
            {
                nuevoNodo.Hijos[i] = Convert.ToInt32(datosSeparados[PosicionEnDatosCadena]);
                PosicionEnDatosCadena++;
            }
            for (int i = 0; i < nuevoNodo.Llaves.Count; i++)
            {
                nuevoNodo.Llaves[i] = datosSeparados[PosicionEnDatosCadena];
                PosicionEnDatosCadena++;
            }
            for (int i = 0; i < nuevoNodo.Datos.Count; i++)
            {
                datosSeparados[PosicionEnDatosCadena] = datosSeparados[PosicionEnDatosCadena].Replace(Utilidades.TextoSustitutoSeparador, Utilidades.TextoSeparador);
                nuevoNodo.Datos[i] = gaseosas.Create(datosSeparados[PosicionEnDatosCadena]);
                PosicionEnDatosCadena++;
            }
            return nuevoNodo;
                
        }
        internal void GuardarNodoEnDisco(FileStream archivo, int tamañoEncabezado)
        {   
            archivo.Seek(CalcularPosicionEnDisco(tamañoEncabezado), SeekOrigin.Begin);
            archivo.Write(ObtenerBytes(), 0, TamañoEnBytes);
            archivo.Flush();
        }
        internal void LimpiarNodoEnDisco(FileStream archivo, int tamañoEncabezado, IGaseosasTamañoTextoFijo<T> gaseosas)
        {
            LimpiarNodo(gaseosas);
            GuardarNodoEnDisco(archivo, tamañoEncabezado);
        }
        internal int PosicionAproximadaEnNodo(string llave)
        {
            int posicion = Llaves.Count;
            int llaveBuscar = GetNumericString(llave);

            for (int i = 0; i < Llaves.Count; i++)
            {
                int llaveArbol = GetNumericString(Llaves[i]);

                if (llaveArbol > llaveBuscar || (Llaves[i] == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"))
                {
                    posicion = i;
                    break;
                }
            }
            return posicion;
        }
        internal int GetNumericString(string llave)
        {
            var chars = llave.ToCharArray();
            int result = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                result += (int)chars[i];
            }
            return result;
        }
        internal int PosicionExactaEnNodo(string llave)
        {
            int posicion = -1;

            for (int i = 0; i < Llaves.Count; i++)
            {
                string temp = Llaves[i];

                if (llave.Trim() == temp.Trim('x'))
                {
                    posicion = i;
                    break;
                }
            }
            return posicion;
        }
        internal void AgregarDato(string llave, T dato, int hijoDerecho)
        {
            AgregarDato(llave, dato, hijoDerecho, true);
        }
        internal void AgregarDato(string llave, T dato, int hijoDerecho, bool ValidarLleno)
        {
            if (Lleno && ValidarLleno)
            {
                throw new IndexOutOfRangeException("El nodo está lleno, ya no puede insertar más datos");
            }
            if (llave == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
            {
                throw new ArgumentOutOfRangeException("llave");
            }
            int posicionParaInsertar = 0;
            posicionParaInsertar = PosicionAproximadaEnNodo(llave);
            for (int i = Hijos.Count - 1; i > posicionParaInsertar + 1; i--)
            {
                Hijos[i] = Hijos[i - 1];
            }
            Hijos[posicionParaInsertar + 1] = hijoDerecho;
            for (int i = Llaves.Count - 1; i > posicionParaInsertar; i--)
            {
                Llaves[i] = Llaves[i - 1];
                Datos[i] = Datos[i - 1];
            }
            Llaves[posicionParaInsertar] = Utilidades.FormatearLlave(llave);
            Datos[posicionParaInsertar] = dato;
        }
        internal void AgregarDato(string llave, T dato)
        {
            AgregarDato(llave, dato, Utilidades.apuntadoVacio);
        }
        internal void SepararNodo(string llave, T dato, int hijoDerecho, Nodo<T> nuevoNodo, ref string llavePorSubir, ref T datoPorSubir)
        {
            if (!Lleno)
            {
                throw new Exception("Uno nodo solo puede separarse si está lleno");
            }
            Llaves.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            Datos.Add(dato);
            Hijos.Add(Utilidades.apuntadoVacio);
            AgregarDato(llave, dato, hijoDerecho, false);
            int mitad = (Orden / 2);
            llavePorSubir = Utilidades.FormatearLlave(Llaves[mitad]);
            datoPorSubir = Datos[mitad];
            Llaves[mitad] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            int j = 0;
            for (int i = mitad + 1; i < Llaves.Count; i++)
            {
                nuevoNodo.Llaves[j] = Llaves[i];
                nuevoNodo.Datos[j] = Datos[i];
                Llaves[i] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                j++;
            }
            j = 0;
            for (int i = mitad + 1; i < Hijos.Count; i++)
            {
                nuevoNodo.Hijos[j] = Hijos[i];
                Hijos[i] = Utilidades.apuntadoVacio;
                j++;
            }
            Llaves.RemoveAt(Llaves.Count - 1);
            Datos.RemoveAt(Datos.Count - 1);
            Hijos.RemoveAt(Hijos.Count - 1);
        }
    }
}
