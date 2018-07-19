using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDataTypes.TVDataTypes
{
    public class Network
    {
        public string Name { get; set; }
        public string LogoFilePath { get; set; }
        [JsonIgnore]
        public List<Series> SeriesList { get; set; }

        public Network(string name, string logoFilePath)
        {
            Name = name;
            LogoFilePath = logoFilePath;
        }

    }
}
