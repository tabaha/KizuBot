using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.IO;
using System.Text.RegularExpressions;

namespace KizuBot.Modules {
    class PixivModule : IModule {

        private Random SRandom;
        private List<ulong> _AllowedChannels;
        private List<PixivImage> _Images;

        public PixivModule() {
            SRandom = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            _AllowedChannels = new List<ulong>();
            _Images = new List<PixivImage>();
            LoadImages();
        }

        public void LoadImages() {
            foreach(var folder in Directory.GetDirectories(@"D:\pictures\japan_related\pixiv")) {
                if (folder.Contains("unknown")) continue;
                foreach(var image in Directory.GetFiles(folder)) {
                    var match = Pixiv.Tools.PixivDirRegex.Match(image);
                    var artist = match.Groups["artist"].Value;
                    var id = match.Groups["id"].Value;
                    var page = 0;
                    if (match.Groups["page"].Success) page = int.Parse(match.Groups["page"].Value);
                    _Images.Add(new PixivImage(int.Parse(id), page, int.Parse(artist), image));
                }
            }
            foreach(var image in Directory.GetFiles(@"D:\pictures\japan_related\pixiv")) {
                var match = Pixiv.Tools.PixivIDPageRegex.Match(image);
                var id = match.Groups["id"].Value;
                var page = 0;
                if (match.Groups["page"].Success) page = int.Parse(match.Groups["page"].Value);
                _Images.Add(new PixivImage(int.Parse(id), page, -1, image));
            }
            foreach (var image in Directory.GetFiles(@"D:\pictures\japan_related\pixiv\unknown_artist")) {
                var match = Pixiv.Tools.PixivIDPageRegex.Match(image);
                var id = match.Groups["id"].Value;
                var page = 0;
                if (match.Groups["page"].Success) page = int.Parse(match.Groups["page"].Value);
                _Images.Add(new PixivImage(int.Parse(id), page, -1, image));
            }
        }

        public void AddToAllowedChannels(ulong channelID) {
            if(!_AllowedChannels.Contains(channelID)) {
                _AllowedChannels.Add(channelID);
            }
        }

        public async void ProcessInMessage(object sender, MessageEventArgs e) {
            if (e.Message.IsAuthor) return;

            var splitMessage = e.Message.Text.Split(' ');
            if (splitMessage[0] != "!pixiv") return;
            if (!_AllowedChannels.Contains(e.Channel.Id)) {
                Console.WriteLine("Not allowed on this channel");
                return;
            }

            var n = 1;
            var id = -1;
            var artist = -1;

            if(splitMessage.Length == 1) {
                var image = GetRandomPixivImage();
                await e.Channel.SendMessage(image.ToString());
                await e.Channel.SendFile(image.Path);
            }
            else if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out n)) {
                if (n > 5) {
                    await e.Channel.SendMessage("Can't send that many. Limiting to 5");
                    n = 5;
                }
                for (var i = 0; i < n; i++) {
                    var image = GetRandomPixivImage();
                    await e.Channel.SendMessage(image.ToString());
                    await e.Channel.SendFile(image.Path);
                }
            }
            else if(splitMessage.Length == 3 && splitMessage[1].ToLower() == "-id"  && int.TryParse(splitMessage[2], out id)) {
                foreach(var image in _Images.Where(_image => _image.ID == id)) {
                    await e.Channel.SendMessage(image.ToString());
                    await e.Channel.SendFile(image.Path);
                }
            }
            else if (splitMessage.Length == 3 && splitMessage[1].ToLower() == "-artist" && int.TryParse(splitMessage[2], out artist)) {
                foreach (var image in _Images.Where(_image => _image.Artist == artist)) {
                    try {
                        await e.Channel.SendMessage(image.ToString());
                        await e.Channel.SendFile(image.Path);
                    }
                    catch (Discord.Net.HttpException exception) {
                        Console.WriteLine(exception.StackTrace);
                    }
                }
            }
        }

        public void RemoveFromAllowedChannels(ulong channelID) {
            if(_AllowedChannels.Contains(channelID)) {
                _AllowedChannels.Remove(channelID);
            }
        }

        private void LoadConfig() {

        }

        private PixivImage GetRandomPixivImage() {
            /*var basePath = @"D:\pictures\japan_related\pixiv";
            bool userFolder = false;
            if (SRandom.Next(101) > 60) {
                userFolder = true;
            }
            var artistID = -1;
            var imageID = -1;
            var page = 0;
            if (userFolder) {
                var dirs = Directory.GetDirectories(basePath);
                var pickedDir = dirs[SRandom.Next(dirs.Count())];
                if (!pickedDir.Contains("unknown")) {
                    basePath = pickedDir;
                    artistID = int.Parse(Regex.Match(basePath, @"\d+").Value);
                }
            }
            var images = Directory.GetFiles(basePath);
            if (images.Length == 0) {
                artistID = -1;
                images = Directory.GetFiles(@"D:\pictures\japan_related\pixiv");
            }
            var pickedImage = images[SRandom.Next(images.Count())];
            imageID = Pixiv.Tools.GetPixivIdFromPath(pickedImage);
            if (int.TryParse(Regex.Match(pickedImage, @"_p(<page>\d+)").Groups["page"].Value, out page)) ;
            return new PixivImage(imageID, page, artistID, pickedImage);*/
            return _Images.ElementAt(SRandom.Next(_Images.Count));
        }
    }
}
