using System;
namespace Laboratorio2.Data
{
    public class DATA
    {
        private static DATA _instance = null;

        public static DATA INSTANCE
        {
            get
            {
                if (_instance == null) _instance = new DATA();
                return _instance;
            }
        }

        public const int Grado = 5;


    }
}
