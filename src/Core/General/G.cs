using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class G
    {
        public static object CreateGeneralType(Type type)
        {
            try
            {
                var result = Activator.CreateInstance(type);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public static object CreateGeneralType(Type type, params object[] args)
        {
            try
            {
                var result = Activator.CreateInstance(type, args);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public static string ToJson(this Object input)
        {
            var count = 0;
            start:
            try
            {
                return JsonConvert.SerializeObject(input);
            }
            catch { count++;if (count < 100) goto start;return ""; }

        }
    }
}
