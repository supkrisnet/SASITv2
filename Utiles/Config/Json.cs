using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utiles.Config
{
    public class Json
    {
        public static JObject LoadConfig(String cf)
        {
            if (!File.Exists(cf))
            {
                throw new Exception(string.Format(Messages.Exceptions._FILE_NOT_FOUND_, cf));
            }

            String d = File.ReadAllText(cf).Trim();

            try
            {
                return JObject.Parse(d);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static dynamic ConvertoToDynamic(String cf)
        {
            if (!File.Exists(cf))
            {
                throw new Exception(string.Format(Messages.Exceptions._FILE_NOT_FOUND_, cf));
            }

            String d = File.ReadAllText(cf).Trim();


            try
            {
                return JsonConvert.DeserializeObject<ExpandoObject>(d, new ExpandoObjectConverter());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
