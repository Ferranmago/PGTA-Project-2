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

        public int CurrentIndex = 0; // Contador para packs de octetos en Mensaje

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

                Console.WriteLine("\nFRN1");
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

                // Avanzar contador
                CurrentIndex += 3;
            }
            else
            {
                Console.WriteLine("FRN2 -> Mensaje demasiado corto.");
            }
        }
        public void FRN3()
        {
            if (Mensaje.Length >= CurrentIndex + 1)
            {
                bool moreOctets = true;
                int octetCount = 0;

                while (moreOctets && (CurrentIndex < Mensaje.Length))
                {
                    byte octet = Mensaje[CurrentIndex];
                    octetCount++;

                    if (octetCount == 1)
                    {
                        int TYP = (octet >> 5) & 0b111; // bits 8-6
                        int SIM = (octet >> 4) & 0b1; // bit 5
                        int RDP = (octet >> 3) & 0b1; // bit 4
                        int SPI = (octet >> 2) & 0b1; // bit 3
                        int RAB = (octet >> 1) & 0b1; // bit 2
                        int FX = (octet) & 0b1; // bit 1

                        // Mostrar resultados interpretados
                        Console.WriteLine("FRN3");
                        // Console.WriteLine($"Octeto {octetCount}: {octet:X2}");
                        Console.WriteLine($"TYP = {TYP} -> {CAT048_Tables.FRN3_Type_Translator(TYP)}");
                        Console.WriteLine($"SIM = {SIM} -> {(SIM == 0 ? "Actual target report" : "Simulated target report")}");
                        Console.WriteLine($"RDP = {RDP} -> {(RDP == 0 ? "Report from RDP Chain 1" : "Report from RDP Chain 2")}");
                        Console.WriteLine($"SPI = {SPI} -> {(SPI == 0 ? "Absence of SPI" : "Special Position Identification")}");
                        Console.WriteLine($"RAB = {RAB} -> {(RAB == 0 ? "Report from aircraft transponder" : "Report from field monitor (fixed transponder)")}");
                        Console.WriteLine($"FX = {FX} -> {(FX == 1 ? "Extension" : "End of Data Item")}");

                        // Si FX=0 terminamos
                        moreOctets = (FX == 1);
                    }

                    else if (octetCount == 2)
                    {
                        int TST = (octet >> 7) & 0b1; // bit 8
                        int ERR = (octet >> 6) & 0b1; // bit 7
                        int XPP = (octet >> 5) & 0b1; // bit 6
                        int ME = (octet >> 4) & 0b1; // bit 5
                        int MI = (octet >> 3) & 0b1; // bit 4
                        int FOEFRI = (octet >> 1) & 0b11; // bits 3-2
                        int FX = (octet) & 0b1; // bit 1

                        Console.WriteLine($"TST = {TST} -> {(TST == 0 ? "Real target report" : "Test target report")}");
                        Console.WriteLine($"ERR = {ERR} -> {(ERR == 0 ? "No Extended Range" : "Extended Range present")}");
                        Console.WriteLine($"XPP = {XPP} -> {(XPP == 0 ? "No X-Pulse present" : "X-Pulse present")}");
                        Console.WriteLine($"ME = {ME} -> {(ME == 0 ? "No military emergency" : "Military emergency")}");
                        Console.WriteLine($"MI = {MI} -> {(MI == 0 ? "No military identification" : "Military identification")}");
                        Console.WriteLine($"FOE/FRI = {FOEFRI} -> {CAT048_Tables.FRN3_FOE_FRI_Translator(FOEFRI)}");
                        Console.WriteLine($"FX = {FX} -> {(FX == 1 ? "Extension" : "End of Data Item")}");

                        moreOctets = (FX == 1);
                    }

                    else if (octetCount == 3)
                    {
                        int ADSB_EP = (octet >> 7) & 0b1;
                        int ADSB_VAL = (octet >> 6) & 0b1;
                        int SCN_EP = (octet >> 5) & 0b1;
                        int SCN_VAL = (octet >> 4) & 0b1;
                        int PAI_EP = (octet >> 3) & 0b1;
                        int PAI_VAL = (octet >> 2) & 0b1;
                        int SPARE = (octet >> 1) & 0b1;
                        int FX = (octet) & 0b1;

                        Console.WriteLine($"ADSB/EP = {ADSB_EP} -> {(ADSB_EP == 0 ? "ADSB not populated" : "ADSB populated")}");
                        Console.WriteLine($"ADSB/VAL = {ADSB_VAL} -> {(ADSB_VAL == 0 ? "ADSB info not available" : "ADSB info available")}");
                        Console.WriteLine($"SCN/EP = {SCN_EP} -> {(SCN_EP == 0 ? "SCN not populated" : "SCN populated")}");
                        Console.WriteLine($"SCN/VAL = {SCN_VAL} -> {(SCN_VAL == 0 ? "SCN not available" : "SCN available")}");
                        Console.WriteLine($"PAI/EP = {PAI_EP} -> {(PAI_EP == 0 ? "PAI not populated" : "PAI populated")}");
                        Console.WriteLine($"PAI/VAL = {PAI_VAL} -> {(PAI_VAL==0 ? "PAI not available" : "PAI available")}");
                        Console.WriteLine($"SPARE = {SPARE} -> (set to 0)");
                        Console.WriteLine($"FX = {FX} -> {(FX==1 ? "Extension" : "End of Data Item")}");
                    }

                    else
                    {
                        Console.WriteLine("Extensión adicional no implementada.");
                        moreOctets = false;
                    }

                    CurrentIndex++; // avanzamos un byte

                }
            }
            else
            {
                Console.WriteLine("FRN3 -> Mensaje demasiado corto.");
            }
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
