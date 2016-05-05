using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KizuBot {
    class PixivImage {
        public int ID { get; private set; }
        public int Page { get; private set; }
        public int Artist { get; private set; }
        public string Path { get; private set; }

        public PixivImage(int id, int page, int artist, string path) {
            ID = id;
            Page = page;
            Artist = artist;
            Path = path;
        }

        public override string ToString() {
            var result = "Artist: ";
            if(Artist == -1) {
                result += "unknown";
            }
            else {
                result += Artist;
            }
            result += " ";
            result += "ID: " + ID;
            result += " ";
            result += "Page: " + Page;
            /*result += " ";
            result += Pixiv.Tools.GetPixivWorkLink(ID);*/
            return result;
        }
    }
}
