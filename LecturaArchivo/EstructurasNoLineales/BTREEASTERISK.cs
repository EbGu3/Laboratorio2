using System;
using LecturaArchivo.EstructurasNoLineales;
using System.Collections.Generic;

namespace LecturaArchivo.EstructurasNoLineales
{
    public class BTREEASTERISK
    {
        public Nodo Root { get; set; }
        public int Posicion = 0;
        public int Id = 1;

        public BTREEASTERISK()
        {
            Root = null;
        }

        public void Insertar(int Value, int grado)
        {
            Root = InsertarRecursivo(Value, grado);
        }

        Nodo InsertarRecursivo(int Value, int grado)
        {
            if(Root == null)
            {
                Root = new Nodo(grado, Id);
                Root.Values[Posicion] = Value;
            }
            else if((Posicion < Nodo.TamañoRaiz(grado)) && (Root.Isleaf == false))
            {
                Posicion++;
                Root.Values[Posicion] = Value;
            }

            else if ((Posicion == Nodo.TamañoRaiz(grado)) && (Root.Isleaf == false))
            {

                List<double> RootOrden = new List<double>();
                //Ordenar
                for (int i = 0; i < Nodo.TamañoRaiz(grado) ; i++)
                {
                    RootOrden.Add(Root.Values[i]);
                }


                RootOrden.Add(Value);

                RootOrden.Sort();

                Split(RootOrden, grado, false);

            }
            
            return Root;
        }

        Nodo Insertleft(int Value, int grado)
        {


            return Root;
        }

        Nodo EspacioDisponible(int Value, int grado)
        {


            return Root;
        }

        Nodo Split(List<double> Actual, int grado, bool Isleaf)
        {
            if(Isleaf == false)
            {
                var Ingresados = 0;
                Posicion++;
                Nodo Aux = new Nodo(grado, 1);
                Nodo Secund = new Nodo(Posicion , true, grado);
                Posicion++;
                Nodo Third = new Nodo(Posicion, true, grado);

                foreach (var item in Actual)
                {
                    if(Ingresados < Nodo.MinimoValoresHoja(grado))
                    {
                        Aux.Values[Ingresados] = item;
                    }
                    if(Ingresados == Ingresados+1)
                    {
                        Third.Values[Ingresados] = item;
                    }
                    else if(Ingresados > Nodo.MinimoValoresHoja(grado))
                    {
                        Secund.Values[Ingresados] = item;
                    }
                }


                Root = Aux;
                
            }
            
            return Root;
        }






    }
}
