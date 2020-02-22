using System;
using LecturaArchivo.EstructurasNoLineales;

namespace LecturaArchivo
{
    class Program
    {
        static void Main(string[] args)
        {
            BTREEASTERISK bArbolito = new BTREEASTERISK();
            Archivo.Leer();

            bArbolito.Insertar(1, 5);
            bArbolito.Insertar(2, 5);
            bArbolito.Insertar(3, 5);
            bArbolito.Insertar(4, 5);
            bArbolito.Insertar(5, 5);
            bArbolito.Insertar(6, 5);
            bArbolito.Insertar(7, 5);


            Console.ReadKey();
        }

    }
}
