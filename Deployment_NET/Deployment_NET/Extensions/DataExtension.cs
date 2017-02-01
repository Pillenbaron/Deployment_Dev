using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace awinta.Deployment_NET.Extensions
{
    internal static class DataExtension
    {
        public static Dictionary<string, string> ToDictionary(this Properties value)
        {
            var result = new Dictionary<string, string>();

            foreach (Property property in value)
                try
                {
                    result.Add(property.Name, property.Value.ToString());
                }
                catch (Exception)
                {
                    Debug.Print("Fehler!");
                }

            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var item in value)
                action(item);
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var item in value)
                await Task.Run(() => action(item));
        }

        public static string ToFileHash(this FileInfo value)
        {
            byte[] result;

            using (var fileCheck = value.OpenRead())
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                result = md5.ComputeHash(fileCheck);
            }

            return Encoding.UTF8.GetString(result);
        }

        public static void ToXml<T>(this T value)
        {
        }

    }
}