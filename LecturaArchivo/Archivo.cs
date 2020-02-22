using System;
using System.IO;
namespace LecturaArchivo
{
    public class Archivo
    {
        public static void Leer()
        {
            try
            {

                StreamReader sr;
                StreamWriter sw;

                using (sr = new StreamReader("/Users/eber.g/Projects/Laboratorio2/LecturaArchivo/bin/Debug/netcoreapp3.1/Arbol.txt"))
                {
                    using (sw = new StreamWriter("/Users/eber.g/Projects/Laboratorio2/LecturaArchivo/bin/Debug/netcoreapp3.1/Arbol.txt"))
                    {

                        

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }

        

           
        




    }
}
