using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace Estructuras.Mdelo
{
    public class Gaseosa : ITextoTamañoFijo
    {
        public string Nombre { get; set; }
        public string Sabor { get; set; }
        public double Volumen { get; set; }
        public double Precio { get; set; }
        public string CasaProductura { get; set; }


        private const string FormatoConst = "#########################¡#########################¡#########################¡0000000000¡0000000000";

        public Gaseosa()
        {
            Nombre = "";
            Sabor = "";
            CasaProductura = "";
            Volumen = 0;
            Precio = 0;

        }

        public Gaseosa(string nombre, string sabor, string casaproductura, double volumen, double precio)
        {
            Nombre = nombre;
            Sabor = sabor;
            CasaProductura = casaproductura;
            Volumen = volumen;
            Precio = precio;
        }

        public int FixedSizeText
        {
            get
            {
                return 424;
            }
        }

        public override string ToString()
        {
            return ToFixedToSizeString();
        }

        public string ToFixedToSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Nombre.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(Sabor.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(CasaProductura.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(Volumen.ToString().PadLeft(10, '#'));
            sb.Append('^');
            sb.Append(Precio.ToString().PadLeft(10, '#'));
            sb.Append('^');

            return sb.ToString();


        }

    }
}
