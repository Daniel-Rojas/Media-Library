using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDataTypes.TVDataTypes
{
    public class Series
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Network { get; set; }
        public string SeriesCoverFilePath { get; set; }
        public string SeriesPhotoFilePath { get; set; }
        public List<Season> SeasonList { get; set; }

        //public List<Actors> TopBilledCast { get; set; }

        public Series(string title, string network, List<Season> SeasonList)
        {
            Title = title;
            Network = network;
        }
    }
}
