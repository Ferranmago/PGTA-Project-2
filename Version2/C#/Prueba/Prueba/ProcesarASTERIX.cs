using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public class ProcesarASTERIX
    {
        public void ProcesarFicheroAsterix(string filePath)
        {
            var cat048Messages = new List<CAT048>();
            var cat021Messages = new List<CAT021>();

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            int maxMessages = 3; // máximo de mensajes a leer (temporalmente)
            int messageCount = 0;
            while (fs.Position < fs.Length && messageCount < maxMessages)
            {
                // Leer categoría (1 byte)
                byte category = br.ReadByte();

                // Leer longitud (2 bytes)
                byte len1 = br.ReadByte();
                byte len2 = br.ReadByte();
                int length = (len1 << 8) + len2;

                if (category == 48)
                {
                    // Copiamos el dataRecord temporalmente
                    byte[] dataRecord = br.ReadBytes(length - 3); // Restar 3 bytes anteriores (Cat + Length)

                    // Primero detectamos el tamaño del FSPEC
                    int Byte_FSPEC = 0;
                    while (true)
                    {
                        Byte_FSPEC++;
                        if ((dataRecord[Byte_FSPEC - 1] & 0x01) == 0) // si MSB=0, fin de FSPEC
                            break;
                    }

                    // Dividimos en FSPEC y Mensaje
                    byte[] FSPEC = dataRecord.Take(Byte_FSPEC).ToArray();
                    byte[] Bytes_Mensaje = dataRecord.Skip(Byte_FSPEC).ToArray();
                    var mensaje = new CAT048 {Length = length, FSPEC = FSPEC, Mensaje = Bytes_Mensaje };

                    mensaje.FRNs = mensaje.AnalizarFSPEC();

                    mensaje.DecodificarCampos();

                    cat048Messages.Add(mensaje);

                }

                else if (category == 21)
                {

                }
                else // ignoramos otras categorías
                {   
                }
                messageCount++;
            }


            CAT048.GuardarFRNs(cat048Messages, "CAT048_FRNs.txt");

            Console.WriteLine($"\nTotal CAT048 messages: {cat048Messages.Count}");
            Console.WriteLine("FRNs guardados en CAT048_FRNs.txt");

        }
    }
}
