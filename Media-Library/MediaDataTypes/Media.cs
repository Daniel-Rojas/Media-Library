using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDataTypes
{
    public class Media
    {
        public string Title { get; set; }
        public string FilePath { get; set; }

        public Media() {}

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                Media objMedia = (Media)obj;
                if (objMedia.Title == this.Title && objMedia.FilePath == this.FilePath)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public override string ToString()
        {
            string mediaSummary = "Media:: Title: " + Title + ", Path: " + FilePath;
            return mediaSummary;
        }
    }
}
