using System;
namespace LecturaArchivo.EstructurasNoLineales
{
    public class Nodo
    {
        public double id { get; set; }
        public Nodo[] Childreen { get; set; }
        public double[] Values { get; set; }
        public bool Isleaf { get; set; }
     

        public Nodo(int grado, int Id)
        {
            id = Id;
            Childreen = new Nodo[grado + 1];
            Values = new double[grado];
            Isleaf = false;
        }

        public Nodo(int _Id,bool isleaf, int Grado)
        { 
            id = _Id;
            Childreen = new Nodo[Grado + 1];
            Values = new double[Grado];
            Isleaf = true;
            

        }


        public static double TamañoRaiz(int grado)
        {
            double Tamaño = 0 ;

            Tamaño = ((4 / 3) * (grado-1));

            return (Math.Ceiling(Tamaño));
        }

        public static double MinimoValoresHoja(int grado)
        {
            double Tamaño = 0;

            Tamaño = ((2 * grado) - 1) / 3;

            return (Math.Ceiling(Tamaño));
        }

        
    }
}
