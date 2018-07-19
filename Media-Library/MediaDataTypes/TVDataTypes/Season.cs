using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDataTypes.TVDataTypes
{
    public class Season
    {
        public int NumberOfEpisodes { get; set; }
        public List<Episode> EpisodeList { get; set; }

        public Season(List<Episode> episodeList, int numberOfEpisodes)
        {
            NumberOfEpisodes = numberOfEpisodes;
            EpisodeList = episodeList;
        }
    }
}
