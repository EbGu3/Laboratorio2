using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Estructuras.Mdelo
{
    class ArbolBAsterisk<T>  where T : ITextoTamañoFijo
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
            if (nodoActual.PosicionExactaEnNodo(llave) != -1 =)
            {
                throw new InvalidOperationException("La llave indicada ya está contenida en el árbol. ");

            }
            if (nodoActual.EsHoja)
            {
                Subir(nodoActual, llave, dato, Utilidades.ApuntadorVacio);
                GuardarEncabezado();
            }
            else
            {
                AgregarRecursivo(nodoActual.Hijos[nodoActual.PosicionAproximadamenteEnNodo(llave)], llave, dato);
            }
        }
        private void Subir(Nodo<T> nodoActual, string)
        {
            if (!nodoActual.Lleno)
            {
                nodoActual.AgregarDato(llave, dato, hijoDerecho);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                return;
            }
            Nodo<T> nuevoHermano = new Nodo<T>(Orden, _ultimaPosicionLibre, nodoActual.Padre, _gaseosa);
            _ultimaPosicionLibre++;
            string llavePorSubir = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            T datoPorSubir = _gaseosa.CreateNulo();
            nodoActual.SepararNodo(llave, dato, hijoDerecho, nuevoHermano, ref llavePorSubir, ref datoPorSubir);
            Nodo<T> nodoHijo = null;
            for (int i = 0; i < nuevoHermano.Hijos.Count; i++)
            {
                if (nuevoHermano.Hijos[i] != Utilidades.ApuntadorVacio)
                {
                    // Se carga el hijo para modificar su apuntador al padre 
                    nodoHijo = Nodo<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nuevoHermano.Hijos[i], _gaseosa);
                    nodoHijo.Padre = nuevoHermano.Posicion;
                    nodoHijo.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                }
                else
                {
                    break;
                }
                if (nodoActual.Padre == Utilidades.ApuntadorVacio)
                {
                    Nodo<T> nuevaRaiz = new Nodo<T>(Orden, _ultimaPosicionLibre, Utilidades.ApuntadorVacio, _gaseosa);
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


    }
}

