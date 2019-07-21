using IeasteJson2Svg.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg.Tools
{
    public class JsonExtractor
    {
        public static Dictionary<string,string[]> GetJsonData(string JsonText)
        {
            var dictionaryResult = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JsonText);
            return dictionaryResult;
        }
    }
}
