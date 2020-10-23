using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Utils
{
    public static class DataUtil
    {
        public static List<int> GetMonthsNumbers()
        {
            List<int> lista = new List<int> { 1, 2,3,4,5,6,7,8,9,10,11,12};
            return lista;
        }

        public static String GetNameMonth(int number)
        {
            switch (number)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    return "erro";
            }
        }

     
    }
}
