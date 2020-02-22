using System;
namespace Estructuras
{
    public interface ITextoTamañoFijo
    {
        int FixedSizeText { get; }

        string ToFixedToSizeString();
        
    }
}
