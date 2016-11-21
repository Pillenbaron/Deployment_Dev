using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awinta.Deployment_NET.Extensions
{
    internal static class DataExtension
    {

        public static Dictionary<string,string> ToDictionary(this EnvDTE.Properties value)
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

    }
}
