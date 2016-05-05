using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace KizuBot.Pixiv {
    class Tools {

        private static string pixivWorkUrl = @"http://www.pixiv.net/member_illust.php?mode=medium&illust_id=";
        private static string pixivArtistUrl = @"http://www.pixiv.net/member.php?id=";

        private static Regex _pixivIdRegex = new Regex("[1-9][0-9]*");
        private static Regex pageRegex = new Regex("_p[0-9][0-9]*");

        private static string pixivDirExpression = @"\\(?<artist>\d+)\\(?<id>\d+)(_p(?<page>\d+))?";
        public static Regex PixivDirRegex = new Regex(pixivDirExpression);

        private static string _PixivIDPageExpression = @"(?<id>\d+)(_p(?<page>\d+))?";
        public static Regex PixivIDPageRegex = new Regex(_PixivIDPageExpression);


        public static int GetPixivIdFromPath(string path) {
            var filename = Path.GetFileName(path);
            return GetPixivIdFromFilename(filename);
        }

        public static int GetPixivIdFromFilename(string filename) {
            var regexResult = _pixivIdRegex.Match(filename).Value;
            var result = int.Parse(regexResult);
            return result;
        }

        public static string GetPixivIdStringFromPath(string path) {
            var filename = Path.GetFileName(path);
            return GetPixivIdStringFromFilename(filename);
        }

        public static string GetPixivIdStringFromFilename(string filename) {
            var regexResult = _pixivIdRegex.Match(filename).Value;
            return regexResult;
        }

        public static int GetPageFromPath(string path) {
            return GetPageFromFilename(Path.GetFileName(path));
        }

        public static int GetPageFromFilename(string filename) {
            int result;
            Match pageMatch = pageRegex.Match(filename);
            if (pageMatch.Success) {
                string sTemp = pageMatch.Value.Substring(2);
                result = int.Parse(sTemp);
            }
            else {
                result = 0;
            }
            return result;
        }

        public static string GetPixivWorkLink(string path) {
            string id = GetPixivIdStringFromPath(path);
            return pixivWorkUrl + id;
        }

        public static string GetPixivWorkLink(int pixivID) {
            return pixivWorkUrl + pixivID;
        }

        public static string GetPixivArtistLink(int artistID) {
            return pixivArtistUrl + artistID;
        }

        public static string GetPixivArtistLink(string artistID) {
            return pixivArtistUrl + artistID;
        }

        public static void MoveToDir(string file, string destination) {
            string fname = Path.GetFileName(file);
            File.Move(file, destination + fname);
        }

        public static void CopyToDir(string file, string destination) {
            string fname = Path.GetFileName(file);
            File.Copy(file, destination + fname);
        }
    }
}
