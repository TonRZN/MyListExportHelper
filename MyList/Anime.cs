using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    public class Anime
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; }
        public string GroupName { get; set; }
        public string ED2K { get; set; }
        public string FileID { get; set; }

        public bool isComplete()
        {
            return !String.IsNullOrEmpty(ID) && !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(EpisodeNumber) && !String.IsNullOrEmpty(EpisodeTitle) && !String.IsNullOrEmpty(GroupName) && !String.IsNullOrEmpty(ED2K) && !String.IsNullOrEmpty(FileID);
        }
    }
}
