using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estructuras.Mdelo
{
    public class CreateGaseosa : IGaseosasTamañoTextoFijo<Gaseosa>
    {

        public Gaseosa Create(string textoTamañoFijo)
        {
            Gaseosa gaseosa = new Gaseosa();
            var datos = textoTamañoFijo.Split('^');
            gaseosa.Nombre = datos[0].Trim();
            gaseosa.Sabor = datos[1].Trim();
            gaseosa.CasaProductura = datos[2].Trim();
            gaseosa.Volumen = Convert.ToInt32(datos[3].Trim());
            gaseosa.Precio = Convert.ToInt32(datos[4].Trim());


            return gaseosa;
        }

        public Gaseosa CreateTrim(string textoTamañoFijo)
        {
            Gaseosa gaseosa = new Gaseosa();
            var datos = textoTamañoFijo.Split('^');
            gaseosa.Nombre = datos[0].Trim('#');
            gaseosa.Sabor = datos[1].Trim('#');
            gaseosa.CasaProductura = datos[2].Trim('#');
            gaseosa.Volumen = Convert.ToInt32(datos[3].Trim());
            gaseosa.Precio = Convert.ToInt32(datos[4].Trim());

            return gaseosa;
        }

        public Gaseosa CreateNulo()
        {
            return new Gaseosa();
        }
    }
}
