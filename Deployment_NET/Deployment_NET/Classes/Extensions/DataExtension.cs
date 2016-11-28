using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awinta.Deployment_NET.Extensions
{
    internal static class DataExtension
    {

        public static Dictionary<string, string> ToDictionary(this EnvDTE.Properties value)
        {

            Dictionary<string, string> Result = new Dictionary<string, string>();

            foreach (Property property in value)
            {
                try
                {

                    Result.Add(property.Name, property.Value.ToString());

                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.Print("Fehler!");
                }

            }

            return Result;

        }

        public static void ForEach<T>(this IEnumerable<T> Value, Action<T> Action)
        {

            foreach (T Item in Value)
            {
                Action(Item);

            }

        }

        public static string ToFileHash(this FileInfo value)
        {

            byte[] Result = new byte[] { };

            using (FileStream FileCheck = value.OpenRead())
            {

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                Result = md5.ComputeHash(FileCheck);

            }

            return Encoding.UTF8.GetString(Result);

        }

        public static void ToXML<T>(this T Value)
        {



        }

    }
}