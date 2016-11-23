using System.IO;
using System.Xml.Serialization;

namespace awinta.Deployment_NET.IO.XML.Serialization
{
    internal abstract class SerializationService
    {
        /// <summary>
        /// Serialisiert ein Objekt vom Typ 'T' im XML-Format in die angegebene Datei
        /// </summary>
        /// <param name="File">Das System.IO.FileInfo Objekt, welches Informationen über die Zieldatei enthält.</param>
        /// <param name="Value">Das zu Serialisierende Objekt vom Typ 'T'</param>
        /// <remarks></remarks>
        public static void ToXML<T>(FileInfo File, T Value)
        {

            using (StreamWriter Writer = new StreamWriter(File.FullName))
            {

                XmlSerializer Serialize = new XmlSerializer(typeof(T));
                Serialize.Serialize(Writer, Value);

                Writer.Flush();
                Writer.Close();

            }

        }

        /// <summary>
        /// Serialisiert ein Objekt vom Typ 'T' im XML-Format in die angegebene Datei
        /// </summary>
        /// <param name="File">Der Pfad zur Zieldatei</param>
        /// <param name="Value">Das zu Serialisierende Objekt vom Typ 'T'</param>
        /// <remarks></remarks>
        public static void ToXML<T>(string File, T Value)
        {
            SerializationService.ToXML(new FileInfo(File), Value);
        }

        /// <summary>
        /// Deserialisiert ein Objekt vom Typ 'T' aus der angegebenen Datei
        /// </summary>
        /// <param name="File">Ein System.IO.FileInfo Objekt, welches Informationen zur Quelldatei enthält. </param>
        /// <returns>Das deserialisierte Objekt vom Typ 'T'</returns>
        /// <remarks></remarks>
        public static T FromXML<T>(FileInfo File)
        {
            T objReturn = default(T);

            if ((File.Exists))
            {
                StreamReader objReader = new StreamReader(File.FullName);
                XmlSerializer objSerialize = new XmlSerializer(typeof(T));

                objReturn = (T)objSerialize.Deserialize(objReader);

                objReader.Close();
            }

            return objReturn;
        }

        /// <summary>
        /// Deserialisiert ein Objekt vom Typ 'T' aus der angegebenen Datei
        /// </summary>
        /// <param name="File">Der Pfad zur Quelldatei.</param>
        /// <returns>Das deserialisierte Objekt vom Typ 'T'</returns>
        /// <remarks></remarks>
        public static T FromXML<T>(string File)
        {
            return SerializationService.FromXML<T>(new FileInfo(File));
        }

        private SerializationService() { }

    }
}