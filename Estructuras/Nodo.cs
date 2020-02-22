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

        internal Nodo(int orden, int posicion, int padre, IGaseosasTamañoTextoFijo<T> gaseosa)
        {
            if((orden<OrdenMinimo) || (orden > OrdenMaximo)) { throw new ArgumentException("No es valido el tamaño"); }
            if(posicion < 0) { throw new ArgumentOutOfRangeException("Posicion invalida"); }
            Orden = orden; Posicion = posicion; Padre = padre;
            LimpiarNodo(gaseosa);
        }



        private void LimpiarNodo(IGaseosasTamañoTextoFijo<T> gaseosas)
        {
            Hijos = new List<int>();

            for (int i = 0; i < Orden; i++)
            {
                Hijos.Add(Utilidades.apuntadoVacio);
            }
        }
        

    }
}
