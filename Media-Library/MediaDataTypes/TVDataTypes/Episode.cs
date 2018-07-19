using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDataTypes.TVDataTypes
{
    public class Episode : Media
    {
        public string Summary { get; set; }

        public Episode(string title, string filePath)
        {
            Title = title;
            FilePath = filePath;
        }
    }
}
