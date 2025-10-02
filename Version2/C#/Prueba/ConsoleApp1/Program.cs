using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prueba;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos", "datos_asterix_radar.ast");

            var procesador = new ProcesarASTERIX();
            procesador.ProcesarFicheroAsterix(filePath);

            Console.WriteLine("<Pulsar cualquier botón para salir>");
            Console.ReadLine();
        }
    }
}
