using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Estructuras.Mdelo
{
    public class ArbolBAsterisk<T> : ArbolBusqueda<string, T> where T : ITextoTamañoFijo
    {
        private const int _tamañoEncabezadoBinario = 5 * Utilidades.EnteroYEnteroTamaño;
        private int _raiz;
        private int _ultimaPosicionLibre;
        private FileStream _archivo = null; 
        private string _archivoNombre = "";
        private IGaseosasTamañoTextoFijo<T> _gaseosa = null;
        public int Orden { get; private set; }
        public int Altura { get; private set; }
        public List<string> datos = new List<string>();
        public ArbolBAsterisk(int orden, string nombreArchivo, IGaseosasTamañoTextoFijo<T> bebidas)
        {
            _archivoNombre = nombreArchivo;
            _gaseosa = bebidas;
            _archivo = new FileStream(_archivoNombre, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            _raiz = Utilidades.LeerEntero(_archivo, 0);
            _ultimaPosicionLibre = Utilidades.LeerEntero(_archivo, 1);
            Tamaño = Utilidades.LeerEntero(_archivo, 2);
            Orden = Utilidades.LeerEntero(_archivo, 3);
            Altura = Utilidades.LeerEntero(_archivo, 4);

            if (_ultimaPosicionLibre == Utilidades.apuntadoVacio)
            {
                _ultimaPosicionLibre = 0;
            }
            if (Tamaño == Utilidades.apuntadoVacio)
            {
                Tamaño = 0;
            }
            if (Orden == Utilidades.apuntadoVacio)
            {
                Orden = orden;
            }
            if (Altura == Utilidades.apuntadoVacio)
            {
                Altura = 1;
            }
            if (_raiz == Utilidades.apuntadoVacio)
            {
                Nodo<T> nodoCabeza = new Nodo<T>(Orden, _ultimaPosicionLibre, Utilidades.apuntadoVacio, _gaseosa);
                _ultimaPosicionLibre++;
                _raiz = nodoCabeza.Posicion;
                nodoCabeza.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
            }
            GuardarEncabezado();
        }
        private void GuardarEncabezado()
        {
            Utilidades.EscribirEntero(_archivo, 0, _raiz);
            Utilidades.EscribirEntero(_archivo, 1, _ultimaPosicionLibre);
            Utilidades.EscribirEntero(_archivo, 2, Tamaño);
            Utilidades.EscribirEntero(_archivo, 3, Orden);
            Utilidades.EscribirEntero(_archivo, 4, Altura);
            _archivo.Flush();
        }
        private void AgregarRecursivo(int posicionNodoActual, string llave, T dato)
        {
            Nodo<T> nodoActual = Nodo<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _gaseosa);
            if (nodoActual.PosicionExactaEnNodo(llave) != -1)
            {
                throw new InvalidOperationException("La llave indicada ya está contenida en el árbol. ");

            }
            if (nodoActual.EsHoja)
            {
                Subir(nodoActual, llave, dato, Utilidades.apuntadoVacio);
                GuardarEncabezado();
            }
            else
            {
                AgregarRecursivo(nodoActual.Hijos[nodoActual.PosicionAproximadaEnNodo(llave)], llave, dato);
            }
        }
        private void Subir(Nodo<T> nodoActual, string llave, T dato, int hijoDerecho)
        {
            if (!nodoActual.Lleno)
            {
                nodoActual.AgregarDato(llave, dato, hijoDerecho);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                return;
            }
            Nodo<T> nuevoHermano = new Nodo<T>(Orden, _ultimaPosicionLibre, nodoActual.Padre, _gaseosa);
            _ultimaPosicionLibre++;
            string llavePorSubir = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            T datoPorSubir = _gaseosa.CreateNulo();
            nodoActual.SepararNodo(llave, dato, hijoDerecho, nuevoHermano, ref llavePorSubir, ref datoPorSubir);
            Nodo<T> nodoHijo = null;
            for (int i = 0; i < nuevoHermano.Hijos.Count; i++)
            {
                if (nuevoHermano.Hijos[i] != Utilidades.apuntadoVacio)
                {
                    nodoHijo = Nodo<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nuevoHermano.Hijos[i], _gaseosa);
                    nodoHijo.Padre = nuevoHermano.Posicion;
                    nodoHijo.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                }
                else
                {
                    break;
                }
                if (nodoActual.Padre == Utilidades.apuntadoVacio)
                {
                    Nodo<T> nuevaRaiz = new Nodo<T>(Orden, _ultimaPosicionLibre, Utilidades.apuntadoVacio, _gaseosa);
                    _ultimaPosicionLibre++;
                    Altura++;
                    nuevaRaiz.Hijos[0] = nodoActual.Posicion;
                    nuevaRaiz.AgregarDato(llavePorSubir, datoPorSubir, nuevoHermano.Posicion);
                    nodoActual.Padre = nuevaRaiz.Posicion;
                    nuevoHermano.Padre = nuevaRaiz.Posicion;
                    _raiz = nuevaRaiz.Posicion;

                    nuevaRaiz.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                    nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                    nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);

                }
                else
                {
                    nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                    nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);

                    Nodo<T> nodoPadre = Nodo<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nodoActual.Padre, _gaseosa);
                    Subir(nodoPadre, llavePorSubir, datoPorSubir, nuevoHermano.Posicion);
                }
            }
        }
        private Nodo<T> ObtenerRecursivo(int posicionNodoActual, string llave, out int posicion)
        {
            Nodo<T> nodoActual = Nodo<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _gaseosa);
            posicion = nodoActual.PosicionExactaEnNodo(llave);
            if (posicion != -1)
            {
                return nodoActual;
            }
            else
            {
                if (nodoActual.EsHoja)
                {
                    return null;
                }
                else
                {
                    int posicionAproximada = nodoActual.PosicionAproximadaEnNodo(llave);
                    return ObtenerRecursivo(nodoActual.Hijos[posicionAproximada], llave, out posicion);
                }
            }
        }

        public override void Agregar(string llave, T dato, string llaveAux)
        {
            try
            {
                if (llave == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                {
                    throw new ArgumentOutOfRangeException("llave");
                }
                llave = llave + llaveAux;
                AgregarRecursivo(_raiz, llave, dato);
                Tamaño++;
            }
            catch(Exception)
            {
                throw new ArgumentOutOfRangeException("TU MAMA ES HOMBRE");
            }
        }
        public override T Obtener(string llave)
        {
            int posicion = -1;
            Nodo<T> nodoObtenido = ObtenerRecursivo(_raiz, llave, out posicion);
            if (nodoObtenido == null)
            {
                throw new InvalidOperationException("La llave indicada no esta en el árbol. ");
            }
            else
            {
                return nodoObtenido.Datos[posicion];
            }
        }
        public override bool Contiene(string llave)
        {
            int posicion = -1;
            Nodo<T> nodoObtenido = ObtenerRecursivo(_raiz, llave, out posicion);
            if (nodoObtenido == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
            private void EscribirNodo(Nodo<T> nodoActual, StringBuilder texto)
            {
                for (int i= 0; i < nodoActual.Llaves.Count; i++)
                {
                    if(nodoActual.Llaves[i] != "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                    {
                        texto.AppendLine(nodoActual.Llaves[i].ToString());
                        texto.AppendLine(nodoActual.Datos[i].ToString());
                        texto.AppendLine("---------------");
                    }
                    else
                    {
                        break;
                    }
                }
            }
         

        public T Search(Delegate comparer, string llave)
        {
            return (T)comparer.DynamicInvoke(this, llave);
        }
    }

    
}

