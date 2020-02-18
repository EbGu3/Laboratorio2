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
                    using(StreamReader sr = new StreamReader("/Users/eber.g/Projects/Laboratorio2/LecturaArchivo/bin/Debug/netcoreapp3.1/Arbol.txt"))
                    {

                        var line="";
                        while ((line = sr.ReadLine())!= null)
                        {
                            Console.WriteLine(line);
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
