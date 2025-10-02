using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public static class CAT048_Tables
    {
        // Tabla de traducción para FRN1 -> SAC (System Area Code)
        public static readonly Dictionary<string, string> SAC_Countries = new Dictionary<string, string>
        {
            { "02", "Greece" },
            { "04", "The Netherlands" },
            { "06", "Belgium" },
            { "08", "France" },
            { "11", "Ukraine" },
            { "12", "Monaco" },
            { "14", "Spain" },
            { "16", "Hungary" },
            { "19", "Croatia" },
            { "20", "Yugoslavia" },
            { "22", "Italy" },
            { "26", "Rumania" },
            { "28", "Switzerland" },
            { "30", "Slovak Republic" },
            { "31", "Czech Republic" },
            { "32", "Austria" },
            { "34", "United Kingdom" },
            { "35", "United Kingdom" },
            { "36", "United Kingdom" },
            { "38", "Denmark" },
            { "40", "Sweden" },
            { "42", "Norway" },
            { "44", "Finland" },
            { "46", "Lithuania" },
            { "47", "Latvia" },
            { "60", "Poland" },
            { "62", "Germany" },
            { "68", "Portugal" },
            { "70", "Luxembourg" },
            { "72", "Ireland" },
            { "74", "Iceland" },
            { "78", "Malta" },
            { "80", "Cyprus" },
            { "84", "Bulgaria" },
            { "86", "Turkey" },
            { "93", "Republic of Slovenia" }
        };
        public static string FRN3_Type_Translator(int typ)
        {
            switch (typ)
            {
                case 0: return "No detection";
                case 1: return "Single PSR detection";
                case 2: return "Single SSR detection";
                case 3: return "SSR + PSR detection";
                case 4: return "Single Mode-S All-Call";
                case 5: return "Single Mode-S Roll-Call";
                case 6: return "Mode-S All-Call + PSR";
                case 7: return "Mode-S Roll-Call + PSR";
                default: return "Desconocido";
            }
        }
        public static string FRN3_FOE_FRI_Translator(int val)
        {
            switch (val)
            {
                case 0: return "No Mode 4 interrogation";
                case 1: return "Friendly target";
                case 2: return "Unknown target";
                case 3: return "No reply";
                default: return "Desconocido";
            }
        }
    }
}
