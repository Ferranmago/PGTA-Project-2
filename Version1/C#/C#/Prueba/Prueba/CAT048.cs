using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public class CAT048
    {
        public int Length;
        public byte[] FSPEC; // Aquí solo guardamos el FSPEC
        public byte[] Mensaje; // Aquí los datos después del FSPEC
        public List<int> FRNs = new List<int>();

        public List<int> AnalizarFSPEC()
        {
            List<int> FNRs = new List<int>();
            int Index_Bit = 1;
            int pos = 0;

            while (true)
            {
                byte fspecByte = FSPEC[pos];

                for (int bit = 7; bit >= 1; bit--)
                {
                    if (((fspecByte >> bit) & 0x01) == 1)
                        FNRs.Add(Index_Bit);

                    Index_Bit++;
                }

                // bit 8 indica si hay más FSPEC
                if ((fspecByte & 0x01) == 1)
                {
                    pos++;
                }
                else
                {
                    break;
                }
            }
            return FNRs;
        }

        public static void GuardarFRNs(List<CAT048> cat048Messages, string fileName)
        {
            var lines = new List<string>();

            for (int i = 0; i < cat048Messages.Count; i++)
            {
                var mensaje = cat048Messages[i];
                string frns = string.Join(", ", mensaje.FRNs);
                string line = $"Mensaje: {i + 1} || Length: {mensaje.Length} || FRN: {frns}";
                lines.Add(line);
            }
            File.WriteAllLines(fileName, lines);
        }

        public int CurrentIndex = 0; // Puntero dinámico en el array Mensaje

        public void DecodificarCampos()
        {
            CurrentIndex = 0; // Reiniciamos para cada mensaje nuevo

            foreach (int FRN in FRNs)
            {
                switch (FRN)
                {
                    case 1: FRN1(); break;
                    case 2: FRN2(); break;
                    case 3: FRN3(); break;
                    case 4: FRN4(); break;
                    case 5: FRN5(); break;
                    case 6: FRN6(); break;
                    case 7: FRN7(); break;
                    case 8: FRN8(); break;
                    case 9: FRN9(); break;
                    case 10: FRN10(); break;
                    case 11: FRN11(); break;
                    case 13: FRN13(); break;
                    case 14: FRN14(); break;
                    case 21: FRN21(); break;
                    default:
                        Console.WriteLine($"FRN {FRN} no implementado.");
                        break;
                }
            }
        }

        public void FRN1() // Data Sourcer Identifier (2 bytes)
        {
            if (Mensaje.Length >= CurrentIndex + 2)
            {
                string SAC_Hex = Mensaje[CurrentIndex].ToString("X2");
                string SIC_Hex = Mensaje[CurrentIndex + 1].ToString("X2");

                string country = CAT048_Tables.SAC_Countries.TryGetValue(SAC_Hex, out string nombrePais)? nombrePais : "Desconocido";

                Console.WriteLine("FRN1");
                Console.WriteLine($"SAC: {SAC_Hex} [{country}] || SIC: {SIC_Hex}");

                CurrentIndex += 2; // Avanzamos 2 bytes
            }
            else
            {
                Console.WriteLine("FRN1 -> Mensaje demasiado corto.");
            }
        }
        public void FRN2() // Time of Day (3 bytes)
        {
            if (Mensaje.Length >= CurrentIndex + 3)
            {
                byte B1 = Mensaje[CurrentIndex];
                byte B2 = Mensaje[CurrentIndex + 1];
                byte B3 = Mensaje[CurrentIndex + 2];

                // Mostrar en Hexadecimal
                string Value_Hex = $"{B1:X2} {B2:X2} {B3:X2}";

                // Unir bytes para valor de 24 bits
                int TOD_value = (B1 << 16) | (B2 << 8) | B3;

                // Calcular segundos totales (1 unidad = 1/128 s)
                double TOD_Seconds = TOD_value / 128.0;

                // Convertir a TimeSpan
                TimeSpan TOD = TimeSpan.FromSeconds(TOD_Seconds);

                // Mostrar resultados
                Console.WriteLine("FRN2");
                Console.WriteLine($"Hex: {Value_Hex}");
                Console.WriteLine($"Seconds: {TOD_Seconds:F1} s");
                Console.WriteLine($"Time of Day: {TOD:hh\\:mm\\:ss\\.fff}");

                // Avanzar puntero
                CurrentIndex += 3;
            }
            else
            {
                Console.WriteLine("FRN2 -> Mensaje demasiado corto.");
            }
        }
        public void FRN3()
        {

        }
        public void FRN4()
        {

        }
        public void FRN5()
        {

        }
        public void FRN6()
        {

        }
        public void FRN7()
        {

        }
        public void FRN8()
        {

        }
        public void FRN9()
        {

        }
        public void FRN10()
        {

        }
        public void FRN11()
        {

        }
        public void FRN13()
        {

        }
        public void FRN14()
        {

        }
        public void FRN21()
        {

        }
    }
}
