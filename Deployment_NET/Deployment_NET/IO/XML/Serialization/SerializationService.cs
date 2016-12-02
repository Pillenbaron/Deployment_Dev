﻿using System.IO;
using System.Xml.Serialization;

namespace awinta.Deployment_NET.IO.XML.Serialization
{
    internal abstract class SerializationService
    {
        /// <summary>
        /// Serialisiert ein Objekt vom Typ 'T' im XML-Format in die angegebene Datei
        /// </summary>
        /// <param name="file">Das System.IO.FileInfo Objekt, welches Informationen über die Zieldatei enthält.</param>
        /// <param name="value">Das zu Serialisierende Objekt vom Typ 'T'</param>
        /// <remarks></remarks>
        public static void ToXML<T>(FileInfo file, T value)
        {

            using (StreamWriter writer = new StreamWriter(file.FullName))
            {

                var serialize = new XmlSerializer(typeof(T));
                serialize.Serialize(writer, value);

                writer.Flush();
                writer.Close();

            }

        }

        /// <summary>
        /// Serialisiert ein Objekt vom Typ 'T' im XML-Format in die angegebene Datei
        /// </summary>
        /// <param name="file">Der Pfad zur Zieldatei</param>
        /// <param name="value">Das zu Serialisierende Objekt vom Typ 'T'</param>
        /// <remarks></remarks>
        public static void ToXML<T>(string file, T value) => ToXML(new FileInfo(file), value);

        /// <summary>
        /// Deserialisiert ein Objekt vom Typ 'T' aus der angegebenen Datei
        /// </summary>
        /// <param name="file">Ein System.IO.FileInfo Objekt, welches Informationen zur Quelldatei enthält. </param>
        /// <returns>Das deserialisierte Objekt vom Typ 'T'</returns>
        /// <remarks></remarks>
        public static T FromXML<T>(FileInfo file)
        {
            var objReturn = default(T);

            if ((!file.Exists)) return objReturn;
            var objReader = new StreamReader(file.FullName);
            var objSerialize = new XmlSerializer(typeof(T));

            objReturn = (T)objSerialize.Deserialize(objReader);

            objReader.Close();

            return objReturn;
        }

        /// <summary>
        /// Deserialisiert ein Objekt vom Typ 'T' aus der angegebenen Datei
        /// </summary>
        /// <param name="file">Der Pfad zur Quelldatei.</param>
        /// <returns>Das deserialisierte Objekt vom Typ 'T'</returns>
        /// <remarks></remarks>
        public static T FromXML<T>(string file)
        {
            return SerializationService.FromXML<T>(new FileInfo(file));
        }

        private SerializationService() { }

    }
}