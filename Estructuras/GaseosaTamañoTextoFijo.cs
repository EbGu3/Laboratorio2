using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estructuras
{
    public interface IGaseosasTamañoTextoFijo<T> where T : ITextoTamañoFijo
    {
        T Create(string textoTamañoFijo);

        T CreateNulo();
    }
}
